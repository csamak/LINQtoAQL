using System;
using System.Collections.Generic;
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
    ///     Tests queries used in the "AsterixDB 101: An ADM and AQL Primer" tutorial, found at http://asterixdb.ics.uci.edu/documentation/aql/primer.html.
    /// </summary>
    /// <remarks>We can do better than exact string matching to test by using ADB's actual query parser.</remarks>
    class AdbTutorialQueries
    {
        private const string conString = "constring";

        [Test]
        public void ExactMatch0A()
        {
            var query = from user in new AqlQueryable<FacebookUser>(conString)
                        where user.id == 8
                        select user;
            Assert.AreEqual("for $user in dataset FacebookUsers where ($user.id = 8) return $user;", GetQueryString(query.Expression));
        }

        [Test]
        public void RangeScan0B()
        {
            var query = from user in new AqlQueryable<FacebookUser>(conString)
                        where user.id >= 2 && user.id <= 4
                        select user;
            Assert.AreEqual("for $user in dataset FacebookUsers where (($user.id >= 2) and ($user.id <= 4)) return $user;",
                GetQueryString(query.Expression));
        }

        [Test]
        public void OtherQueryFilters1()
        {
            var query = new AqlQueryable<FacebookUser>(conString).Where(user => user.UserSince >= new DateTime(2010, 7, 22) && user.UserSince <= new DateTime(2012, 7, 29, 23, 59, 59));
            Assert.AreEqual("for $user in dataset FacebookUsers where (($user.user-since >= datetime('2010-07-22T00:00:00.0000000')) and ($user.user-since <= datetime('2012-07-29T23:59:59.0000000'))) return $user;", GetQueryString(query.Expression));
        }

        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }

        [Dataset("FacebookUsers"), UsedImplicitly]
        class FacebookUser
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
