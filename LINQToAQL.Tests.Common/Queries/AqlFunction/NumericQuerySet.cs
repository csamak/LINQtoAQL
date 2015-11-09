using System;
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Tests.Common.Model;
using LINQToAQL.Tests.Common.Model.Data;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class NumericQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("NumericAbs")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Abs(-0.99*u.id) > 9),
                Aql = "for $u in dataset FacebookUsers where (abs((-0.99 * $u.id)) > 9) return $u",
                CleanJsonApi =
                    "[{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]}]",
                QueryResult = new[] {TinySocialData.FacebookUsers.Single(u => u.id == 10)}
            },
            new TestQuery("NumericCeiling")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Ceiling(u.id + 0.1) > 10),
                Aql = "for $u in dataset FacebookUsers where (ceiling(($u.id + 0.1)) > 10) return $u",
                CleanJsonApi =
                    "[{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]}]",
                QueryResult = new[] {TinySocialData.FacebookUsers.Single(u => u.id == 10)}
            },
            new TestQuery("NumericFloor")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Floor(u.id + 0.3) > 8.9 && Math.Floor(u.id + 0.3) < 9.1),
                Aql =
                    "for $u in dataset FacebookUsers where ((floor(($u.id + 0.3)) > 8.9) and (floor(($u.id + 0.3)) < 9.1)) return $u",
                CleanJsonApi =
                    "[{\"id\":9,\"alias\":\"Woodrow\",\"name\":\"WoodrowNehling\",\"user-since\":\"2005-09-20T10:10:00.000Z\",\"friend-ids\":[3,10],\"employment\":[{\"organization-name\":\"Zuncan\",\"start-date\":\"2003-04-22\",\"end-date\":\"2009-12-13\"}]}]",
                QueryResult = new[] {TinySocialData.FacebookUsers.Single(u => u.id == 9)}
            },
            new TestQuery("NumericRoundHalfToEven")
            {
                LinqQuery =
                    dv.FacebookUsers.Where(u => Math.Abs(Math.Round(u.id + 0.5, MidpointRounding.ToEven) - 9.0) < 0.3),
                Aql =
                    "for $u in dataset FacebookUsers where (abs((round-half-to-even(($u.id + 0.5)) - 9)) < 0.3) return $u",
                CleanJsonApi = "[]",
                QueryResult = Enumerable.Empty<FacebookUser>()
            },
            new TestQuery("NumericRoundHalfToEvenImplicit")
            {
                LinqQuery = dv.FacebookUsers.Where(u => Math.Abs(Math.Round(u.id + 0.5) - 9.0) < 0.3),
                Aql =
                    "for $u in dataset FacebookUsers where (abs((round-half-to-even(($u.id + 0.5)) - 9)) < 0.3) return $u",
                CleanJsonApi = "[]",
                QueryResult = Enumerable.Empty<FacebookUser>()
            }
        };
    }
}