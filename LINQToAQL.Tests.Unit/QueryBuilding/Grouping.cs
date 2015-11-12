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

using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding
{
    internal class Grouping : QueryBuildingBase
    {
        [Test]
        public void SingleReturnKey()
        {
            IQueryable<int> query = from u in dv.FacebookUsers
                group u by u.id
                into g
                select g.Key;
            Assert.AreEqual(
                "for $g in (for $u in dataset FacebookUsers group by $u.id with $u return $u) return $g[0].id",
                GetQueryString(query.Expression));
        }

        [Test]
        public void ReturnCount()
        {
            IQueryable<int> query = from u in dv.FacebookUsers
                group u by u.id
                into g
                select g.Count();
            Assert.AreEqual(
                "for $g in (for $u in dataset FacebookUsers group by $u.id with $u return $u) return count((for $generated_uservar_1 in $g return $generated_uservar_1))",
                GetQueryString(query.Expression));
        }
    }
}