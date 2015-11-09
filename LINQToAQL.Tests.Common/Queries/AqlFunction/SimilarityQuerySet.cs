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
            //TODO: Query results in AsterixDB error
            new TestQuery("EditDistance")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.FriendIds.EditDistance(new[] {1, 5, 9}) <= 2),
                Aql = "for $u in dataset FacebookUsers where (edit-distance($u.friend-ids, [1,5,9]) <= 2) return $u",
                CleanJsonApi = null,
                QueryResult = null
            },
            //TODO: Query results in AsterixDB error
            new TestQuery("EditDistanceCheck")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.name.EditDistanceCheck("Suzanna Tilson", 2)),
                Aql = "for $u in dataset FacebookUsers where edit-distance-check($u.name, \"Suzanna Tilson\", 2) return $u",
                CleanJsonApi =  null,
                QueryResult = null
            },
            //Weird that although this is a sample query, all have similarity of 0.0.
            new TestQuery("Jaccard")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.FriendIds.Jaccard(new[] {1, 5, 9}) >= 0.6),
                Aql = "for $u in dataset FacebookUsers where (similarity-jaccard($u.friend-ids, [1,5,9]) >= 0.6) return $u",
                CleanJsonApi = "[]",
                QueryResult = Enumerable.Empty<object>()
            },
            new TestQuery("JaccardCheck")
            {
                LinqQuery = from u in dv.FacebookUsers let sim = u.FriendIds.JaccardCheck(new[] {1, 5, 9}, 0.6) where (bool) sim[0] select sim[1],
                Aql = "for $u in dataset FacebookUsers where similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[0] return similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[1]",
                CleanJsonApi = "[]",
                QueryResult = Enumerable.Empty<object>()
            }
        };
    }
}