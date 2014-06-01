using System;
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.Similarity;
using LINQToAQL.Test.Model;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding
{
    /// <summary>
    ///     Tests queries used in the "AsterixDB 101: An ADM and AQL Primer" tutorial, found at
    ///     http://asterixdb.ics.uci.edu/documentation/aql/primer.html.
    /// </summary>
    /// <remarks>We can do better than exact string matching to test by using ADB's actual query parser.</remarks>
    internal class AdbTutorialQueries : QueryBuildingBase
    {
        [Test]
        public void ExactMatch0A()
        {
            IQueryable<FacebookUser> query = from user in dv.FacebookUsers
                where user.id == 8
                select user;
            Assert.AreEqual("for $user in dataset FacebookUsers where ($user.id = 8) return $user",
                GetQueryString(query.Expression));
        }

        [Test]
        public void RangeScan0B()
        {
            IQueryable<FacebookUser> query = from user in dv.FacebookUsers
                where user.id >= 2 && user.id <= 4
                select user;
            Assert.AreEqual(
                "for $user in dataset FacebookUsers where (($user.id >= 2) and ($user.id <= 4)) return $user",
                GetQueryString(query.Expression));
        }

        [Test]
        public void OtherQueryFilters1()
        {
            IQueryable<FacebookUser> query =
                dv.FacebookUsers.Where(
                    user =>
                        user.UserSince >= new DateTime(2010, 7, 22) &&
                        user.UserSince <= new DateTime(2012, 7, 29, 23, 59, 59));
            Assert.AreEqual(
                "for $user in dataset FacebookUsers where (($user.user-since >= datetime('2010-07-22T00:00:00.0000000')) and ($user.user-since <= datetime('2012-07-29T23:59:59.0000000'))) return $user",
                GetQueryString(query.Expression));
        }

        [Test]
        public void EquiJoin2A()
        {
            var query = from user in dv.FacebookUsers
                from message in dv.FacebookMessages
                where message.AuthorId == user.id
                select new {uname = user.name, message = message.Message};
            Assert.AreEqual(
                "for $user in dataset FacebookUsers for $message in dataset FacebookMessages where ($message.author-id = $user.id) return { \"uname\": $user.name, \"message\": $message.message }",
                GetQueryString(query.Expression));

            query = from user in dv.FacebookUsers
                join message in dv.FacebookMessages
                    on user.id equals message.AuthorId
                select new {uname = user.name, message = message.Message};
            Assert.AreEqual(
                "for $user in dataset FacebookUsers for $message in dataset FacebookMessages where ($user.id = $message.author-id) return { \"uname\": $user.name, \"message\": $message.message }",
                GetQueryString(query.Expression));
        }

        [Test]
        public void NestedOuterJoin3()
        {
            var query = from user in dv.FacebookUsers
                select new
                {
                    uname = user.name,
                    messages =
                        (from message in dv.FacebookMessages where message.AuthorId == user.id select message.Message)
                };
            Assert.AreEqual(
                "for $user in dataset FacebookUsers return { \"uname\": $user.name, \"messages\": (for $message in dataset FacebookMessages where ($message.author-id = $user.id) return $message.message) }",
                GetQueryString(query.Expression));
        }

        //skipped queries 4 and 5 for now.

        [Test]
        public void ExistentialQuantification6()
        {
            IQueryable<FacebookUser> query = from fbu in dv.FacebookUsers
                where (fbu.employment.Any(e => e.EndDate == null))
                select fbu;
            Assert.AreEqual(
                "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies is-null($e.end-date)) return $fbu",
                GetQueryString(query.Expression));
            query = from fbu in dv.FacebookUsers
                where (fbu.employment.Any(e => !e.EndDate.HasValue))
                select fbu;
            Assert.AreEqual(
                "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies not(not(is-null($e.end-date)))) return $fbu",
                GetQueryString(query.Expression));
        }

        [Test]
        public void UniversalQuantification7()
        {
            IQueryable<FacebookUser> query = from fbu in dv.FacebookUsers
                where (fbu.employment.All(e => e.EndDate != null))
                select fbu;
            Assert.AreEqual(
                "for $fbu in dataset FacebookUsers where (every $e in $fbu.employment satisfies not(is-null($e.end-date))) return $fbu",
                GetQueryString(query.Expression));
            query = from fbu in dv.FacebookUsers
                where (fbu.employment.All(e => e.EndDate.HasValue))
                select fbu;
            Assert.AreEqual(
                "for $fbu in dataset FacebookUsers where (every $e in $fbu.employment satisfies not(is-null($e.end-date))) return $fbu",
                GetQueryString(query.Expression));
        }

        [Test]
        public void SimpleAggregation8()
        {
            Expression<Func<int>> query = (() => dv.FacebookUsers.Count());
            Assert.AreEqual("count(for $generated_1 in dataset FacebookUsers return $generated_1)",
                GetQueryString(query.Body));
        }

        [Test]
        public void GroupingAndAggregation9A()
        {
            var query =
                dv.TweetMessages.GroupBy(t => t.user.ScreenName)
                    .Select(uid => new {user = uid.Key, count = uid.Count()});
            //TODO: check validity once UNNEST is fixed (asterix issue 780)
            Assert.AreEqual(
                "for $uid in (for $t in dataset TweetMessages group by $t.user.screen-name with $t return $t) return { \"user\": $uid[0].screen-name, \"count\": count((for $generated_uservar_1 in $uid return $generated_uservar_1)) }",
                GetQueryString(query.Expression));
        }

        [Test]
        public void GroupingAndLimits10()
        {
            var query =
                dv.TweetMessages.GroupBy(t => t.user.ScreenName)
                    .OrderByDescending(g => g.Count())
                    .Take(3)
                    .Select(uid => new {user = uid.Key, count = uid.Count()});
            Assert.AreEqual(
                "for $uid in (for $g in (for $t in dataset TweetMessages group by $t.user.screen-name with $t return $t) order by count((for $generated_uservar_uservar_1 in $g return $generated_uservar_uservar_1)) desc limit 3 return $g) return { \"user\": $uid[0].screen-name, \"count\": count((for $generated_uservar_1 in $uid return $generated_uservar_1)) }",
                GetQueryString(query.Expression));
        }

        [Test]
        public void LeftOuterFuzzyJoin11()
        {
            var query =
                dv.TweetMessages.Select(
                    t =>
                        new
                        {
                            tweet = t,
                            similarTweets =
                                dv.TweetMessages.Where(
                                    t2 =>
                                        (bool) (t.ReferredTopics.JaccardCheck(t2.ReferredTopics, 0.3)[0]) &&
                                        t.tweetid != t2.tweetid).Select(t2 => t2.ReferredTopics)
                        });
            Assert.AreEqual(
                "for $t in dataset TweetMessages return { \"tweet\": $t, \"similarTweets\": (for $t2 in dataset TweetMessages where (similarity-jaccard-check($t.referred-topics, $t2.referred-topics, 0.3)[0] and ($t.tweetid != $t2.tweetid)) return $t2.referred-topics) }",
                GetQueryString(query.Expression));
        }
    }
}