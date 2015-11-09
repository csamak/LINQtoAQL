using System;
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Tests.Common.Model;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class NumericQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("NumericAbs")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Abs(-1*u.id) > 9),
                Aql = "for $u in dataset FacebookUsers where (numeric-abs((-1 * $u.id)) > 9) return $u",
                ApiResponse =
                    "[{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]}]",
                QueryResult =
                    new[]
                    {
                        new FacebookUser(null, null)
                        {
                            id = 10,
                            alias = "Bram",
                            name = "BramHatch",
                            UserSince = new DateTime(2010, 10, 16, 10, 0, 0),
                            FriendIds = new HashSet<int> {1, 5, 9}
                        }
                    }
            },
            new TestQuery("NumericCeiling")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Ceiling(u.id + 0.1) > 10),
                Aql = "for $u in dataset FacebookUsers where (numeric-ceiling(($u.id + 0.1)) > 10) return $u",
                ApiResponse =
                    "[{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]}]",
                QueryResult =
                    new[]
                    {
                        new FacebookUser(null, null)
                        {
                            id = 10,
                            alias = "Bram",
                            name = "BramHatch",
                            UserSince = new DateTime(2010, 10, 16, 10, 0, 0),
                            FriendIds = new HashSet<int> {1, 5, 9}
                        }
                    }
            },
            new TestQuery("NumericFloor")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Floor(u.id + 0.3) > 8.9 && Math.Floor(u.id + 0.3) < 9.1),
                Aql =
                    "for $u in dataset FacebookUsers where ((numeric-floor(($u.id + 0.3)) > 8.9) and (numeric-floor(($u.id + 0.3)) < 9.1)) return $u",
                ApiResponse =
                    "[{\"id\":9,\"alias\":\"Woodrow\",\"name\":\"WoodrowNehling\",\"user-since\":\"2005-09-20T10:10:00.000Z\",\"friend-ids\":[3,10],\"employment\":[{\"organization-name\":\"Zuncan\",\"start-date\":\"2003-04-22\",\"end-date\":\"2009-12-13\"}]}]",
                QueryResult =
                    new[]
                    {
                        new FacebookUser(null, null)
                        {
                            id = 9,
                            alias = "Woodrow",
                            name = "WoodrowNehling",
                            UserSince = new DateTime(2005, 09, 20, 10, 0, 0),
                            FriendIds = new HashSet<int> {3, 10}
                        }
                    }
            },
            new TestQuery("NumericRoundHalfToEven")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Abs(Math.Round(u.id + 0.5, MidpointRounding.ToEven) - 9.0) < 0.3),
                Aql = "for $u in dataset FacebookUsers where (numeric-abs((numeric-round-half-to-even(($u.id + 0.5)) - 9)) < 0.3) return $u",
                ApiResponse = "[]",
                QueryResult = Enumerable.Empty<FacebookUser>()
            },
            new TestQuery("NumericRoundHalfToEvenImplicit")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Abs(Math.Round(u.id + 0.5) - 9.0) < 0.3),
                Aql = "for $u in dataset FacebookUsers where (numeric-abs((numeric-round-half-to-even(($u.id + 0.5)) - 9)) < 0.3) return $u",
                ApiResponse = "[]",
                QueryResult = Enumerable.Empty<FacebookUser>()
            }
        };
    }
}