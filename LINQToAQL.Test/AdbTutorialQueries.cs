using System;
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Test.Model;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Test
{
    /// <summary>
    ///     Tests queries used in the "AsterixDB 101: An ADM and AQL Primer" tutorial, found at
    ///     http://asterixdb.ics.uci.edu/documentation/aql/primer.html.
    /// </summary>
    /// <remarks>We can do better than exact string matching to test by using ADB's actual query parser.</remarks>
    internal class AdbTutorialQueries
    {
        private readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

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
                where (fbu.employment.Any(e => e.EndDate == null))//!e.EndDate.HasValue))
                select fbu;
            Assert.AreEqual(
                "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies " + /*is-null*/ "($e.end-date = null)) return $fbu",
                GetQueryString(query.Expression));
        }

        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}