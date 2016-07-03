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

using System;
using System.Linq;
using System.Linq.Expressions;
using LinqToAql.Tests.Common.Model;
using LinqToAql.Tests.Common.Queries;
using NUnit.Framework;

namespace LinqToAql.Tests.Unit.QueryBuilding
{
    internal class QueryBuildingTests
    {
        [Test, TestCaseSource(typeof(QueryTestCases), nameof(QueryTestCases.QuerySynthesisTestCases))]
        public string TestCommonQueries(IQueryable<object> query)
        {
            return QueryBuildingBase.GetQueryString(query.Expression);
        }

        //Needs to use Expression directly, so cannot be included with the other QueryTestCases.
        [Test, Category("AdbTutorialQuerySet")]
        public void SimpleAggregation8()
        {
            Expression<Func<int>> query = () => new TinySocial(null).FacebookUsers.Count();
            Assert.AreEqual("count(for $generated_1 in dataset FacebookUsers return $generated_1)",
                QueryBuildingBase.GetQueryString(query.Body));
        }
    }
}