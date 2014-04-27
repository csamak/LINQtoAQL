using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.DataAnnotations;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Test.Annotations;
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
        private const string ConString = "constring";

        [Test]
        public void ExactMatch0A()
        {
            IQueryable<FacebookUser> query = from user in new AqlQueryable<FacebookUser>(ConString)
                where user.id == 8
                select user;
            Assert.AreEqual("for $user in dataset FacebookUsers where ($user.id = 8) return $user;",
                GetQueryString(query.Expression));
        }

        [Test]
        public void RangeScan0B()
        {
            IQueryable<FacebookUser> query = from user in new AqlQueryable<FacebookUser>(ConString)
                where user.id >= 2 && user.id <= 4
                select user;
            Assert.AreEqual(
                "for $user in dataset FacebookUsers where (($user.id >= 2) and ($user.id <= 4)) return $user;",
                GetQueryString(query.Expression));
        }

        [Test]
        public void OtherQueryFilters1()
        {
            IQueryable<FacebookUser> query =
                new AqlQueryable<FacebookUser>(ConString).Where(
                    user =>
                        user.UserSince >= new DateTime(2010, 7, 22) &&
                        user.UserSince <= new DateTime(2012, 7, 29, 23, 59, 59));
            Assert.AreEqual(
                "for $user in dataset FacebookUsers where (($user.user-since >= datetime('2010-07-22T00:00:00.0000000')) and ($user.user-since <= datetime('2012-07-29T23:59:59.0000000'))) return $user;",
                GetQueryString(query.Expression));
        }

        [Test]
        public void EquiJoin2A()
        {
            var query = from user in new AqlQueryable<FacebookUser>(ConString)
                from message in new AqlQueryable<FacebookMessage>(ConString)
                where message.AuthorId == user.id
                select new {uname = user.name, message = message.Message};
            Assert.AreEqual(
                "for $user in dataset FacebookUsers for $message in dataset FacebookMessages where ($message.author-id = $user.id) return { \"uname\": $user.name, \"message\": $message.message };",
                GetQueryString(query.Expression));
        }

        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }

        [Dataset("FacebookMessages", Open = false)]
        private class FacebookMessage
        {
            [Field(Name = "message-id")]
            public int Id { get; set; }

            [Field(Name = "author-id")]
            public int? AuthorId { get; set; }

            [Field(Name = "in-response-to")]
            public int? InResponseTo { get; set; }

            [Field(Name = "sender-location")]
            public GeoCoordinate SenderLocation { get; set; } //not so sure about GeoCoordinate

            [Field(Name = "message")]
            public string Message { get; set; }
        }

        [Dataset("FacebookUsers"), UsedImplicitly]
        private class FacebookUser
        {
            public int id { get; set; }
            public string alias { get; set; }
            public string name { get; set; }

            [Field(Name = "user-since")]
            public DateTime UserSince { get; set; }

            [Field(Name = "friend-ids")]
            public HashSet<int> FriendIds { get; set; } //dupes allowed?
            public object employment { get; set; }
        }
    }
}