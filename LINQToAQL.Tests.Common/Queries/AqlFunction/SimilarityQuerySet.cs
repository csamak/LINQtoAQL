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
using LINQToAQL.Tests.Common.Model.Data;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class SimilarityQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            //TODO: Query results in AsterixDB error
            new TestQuery("EditDistance")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.name.EditDistance("Suzanna Tilson") <= 2),
                Aql = "for $u in dataset FacebookUsers where (edit-distance($u.name, \"Suzanna Tilson\") <= 2) return $u",
                CleanJsonApi = "[{\"id\":7,\"alias\":\"Suzanna\",\"name\":\"SuzannaTillson\",\"user-since\":\"2012-08-07T10:10:00.000Z\",\"friend-ids\":[6],\"employment\":[{\"organization-name\":\"Labzatron\",\"start-date\":\"2011-04-19\",\"end-date\":null}]}]",
                QueryResult = TinySocialData.FacebookUsers.Where(u => u.id == 7)
            },
            new TestQuery("Jaccard")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.FriendIds.Jaccard(new[] {1, 5, 9}) >= 0.6),
                Aql =
                    "for $u in dataset FacebookUsers where (similarity-jaccard($u.friend-ids, [1,5,9]) >= 0.6) return $u",
                CleanJsonApi = "[{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult = TinySocialData.FacebookUsers.Where(u => new[] {10, 3}.Contains(u.id))
            }
        };
    }
}