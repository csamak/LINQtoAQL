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
using LINQToAQL.Spatial;
using LINQToAQL.Tests.Common.Model.Data;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class SpatialQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("CreatePoint")
            {
                LinqQuery = dv.FacebookUsers.Select(u => new Point(u.id + 1.3, 3.2)),
                Aql = "for $u in dataset FacebookUsers return create-point(($u.id + 1.3), 3.2)",
                CleanJsonApi =
                    "[{\"point\":[7.3,3.2]},{\"point\":[2.3,3.2]},{\"point\":[3.3,3.2]},{\"point\":[5.3,3.2]},{\"point\":[9.3,3.2]},{\"point\":[10.3,3.2]},{\"point\":[11.3,3.2]},{\"point\":[4.3,3.2]},{\"point\":[6.3,3.2]},{\"point\":[8.3,3.2]}]",
                QueryResult =
                    TinySocialData.FacebookUsersByIds(6, 1, 2, 4, 8, 9, 10, 3, 5, 7)
                        .Select(u => new Point(u.id + 1.3, 3.2))
            },
            //TODO: handle conversion to double (make the forced conversion unecessary)
            new TestQuery("CreateLine")
            {
                LinqQuery =
                    dv.FacebookUsers.Where(u => u.id == 3)
                        .Select(u => new Line(new Point(u.id + 1.3, 1.9), new Point(2.4, u.id + 1.4))),
                Aql =
                    "for $u in dataset FacebookUsers where ($u.id = 3) return create-line(create-point(($u.id + 1.3), 1.9), create-point(2.4, ($u.id + 1.4)))",
                CleanJsonApi = "[{\"line\":[{\"point\":[4.3,1.9]},{\"point\":[2.4,4.4]}]}]",
                QueryResult =
                    TinySocialData.FacebookUsersByIds(3)
                        .Select(u => new Line(new Point(u.id + 1.3, 1.9), new Point(2.4, u.id + 1.4)))
            },
            new TestQuery("Distance")
            {
                LinqQuery =
                    dv.TweetMessages.Where(t => t.tweetid == "2")
                        .Select(t => new {distance = t.SenderLocation.Distance(new Point(30.2, 70.7))}),
                Aql =
                    "for $t in dataset TweetMessages where ($t.tweetid = \"2\") return { \"distance\": spatial-distance($t.sender-location, create-point(30.2, 70.7)) }",
                CleanJsonApi = "[{\"distance\":4.432064981473087}]",
                QueryResult = new[] {new {distance = 4.432064981473087}}
            }
        };
    }
}