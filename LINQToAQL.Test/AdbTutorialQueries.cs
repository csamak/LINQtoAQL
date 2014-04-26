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
        [Test]
        public void ExactMatch0A()
        {
            var query = from user in new AqlQueryable<FacebookUser>("constring")
                        where user.id == 8
                        select user;
            Assert.AreEqual("for $user in dataset FacebookUsers where ($user.id = 8) return $user;", GetQueryString(query.Expression));
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
