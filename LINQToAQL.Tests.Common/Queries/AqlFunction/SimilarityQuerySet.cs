using System;
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Similarity;
using LINQToAQL.Tests.Common.Model;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class SimilarityQuerySet : QuerySet
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
            //TODO: Query results in AsterixDB error
            new TestQuery("EditDistance")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.FriendIds.EditDistance(new[] {1, 5, 9}) <= 2),
                Aql = "for $u in dataset FacebookUsers where (edit-distance($u.friend-ids, [1,5,9]) <= 2) return $u",
                ApiResponse = null,
                QueryResult = null
            },
            //TODO: Query results in AsterixDB error
            new TestQuery("EditDistanceCheck")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.name.EditDistanceCheck("Suzanna Tilson", 2)),
                Aql = "for $u in dataset FacebookUsers where edit-distance-check($u.name, \"Suzanna Tilson\", 2) return $u",
                ApiResponse =  null,
                QueryResult = null
            },
            //Weird that although this is a sample query, all have similarity of 0.0.
            new TestQuery("Jaccard")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.FriendIds.Jaccard(new[] {1, 5, 9}) >= 0.6),
                Aql = "for $u in dataset FacebookUsers where (similarity-jaccard($u.friend-ids, [1,5,9]) >= 0.6) return $u",
                ApiResponse = "[]",
                QueryResult = Enumerable.Empty<object>()
            },
            new TestQuery("JaccardCheck")
            {
                LinqQuery = from u in dv.FacebookUsers let sim = u.FriendIds.JaccardCheck(new[] {1, 5, 9}, 0.6) where (bool) sim[0] select sim[1],
                Aql = "for $u in dataset FacebookUsers where similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[0] return similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[1]",
                ApiResponse = "[]",
                QueryResult = Enumerable.Empty<object>()
            }
        };
    }
}