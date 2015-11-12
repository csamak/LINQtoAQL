// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Similarity;

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
                Aql =
                    "for $u in dataset FacebookUsers where edit-distance-check($u.name, \"Suzanna Tilson\", 2) return $u",
                CleanJsonApi = null,
                QueryResult = null
            },
            //Weird that although this is a sample query, all have similarity of 0.0.
            new TestQuery("Jaccard")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.FriendIds.Jaccard(new[] {1, 5, 9}) >= 0.6),
                Aql =
                    "for $u in dataset FacebookUsers where (similarity-jaccard($u.friend-ids, [1,5,9]) >= 0.6) return $u",
                CleanJsonApi = "[]",
                QueryResult = Enumerable.Empty<object>()
            },
            new TestQuery("JaccardCheck")
            {
                LinqQuery =
                    from u in dv.FacebookUsers
                    let sim = u.FriendIds.JaccardCheck(new[] {1, 5, 9}, 0.6)
                    where (bool) sim[0]
                    select sim[1],
                Aql =
                    "for $u in dataset FacebookUsers where similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[0] return similarity-jaccard-check($u.friend-ids, [1,5,9], 0.6)[1]",
                CleanJsonApi = "[]",
                QueryResult = Enumerable.Empty<object>()
            }
        };
    }
}