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
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Tests.Common.Model.Data;
using LINQToAQL.Tests.Common.Queries.AqlFunction;
using NUnit.Framework;

namespace LINQToAQL.Tests.Common.Queries
{
    //TODO: Order is currently enforced in result sets.
    //When this is resolved, we can sometimes use the same queries in memory to get the QueryResults
    public class QueryTestCases
    {
        private static readonly List<IEnumerable<Tuple<Type, TestQuery>>> _testQueries =
            new List<IEnumerable<Tuple<Type, TestQuery>>>();

        static QueryTestCases() 
        {
            RegisterQuerySets(new AdbTutorialQuerySet(), new NumericQuerySet(), new TokenizingQuerySet(),
                new StringQuerySet(), new SimilarityQuerySet(), new SpatialQuerySet());
        }

        public static IEnumerable<TestCaseData> QuerySynthesisTestCases
        {
            get
            {
                foreach (
                    var query in
                        _testQueries.SelectMany(x => x)
                            .Select(t => t.Item2.QuerySynthesisTestData.SetCategory(t.Item1.Name)))
                {
                    if (query.Arguments.All(a => a == null) || query.ExpectedResult == null)
                        query.Ignore("Query test case not complete");
                    yield return query;
                }
            }
        }

        public static IEnumerable<TestCaseData> DeserializationTestCases
        {
            get
            {
                foreach (
                    var query in
                        _testQueries.SelectMany(x => x)
                            .Select(t => t.Item2.DeserializationTestData.SetCategory(t.Item1.Name)))
                {
                    if (query.Arguments.Any(a => a == null))
                        query.Ignore("Query test case not complete");
                    yield return query;
                }
            }
        }

        public static IEnumerable<TestCaseData> EndToEndTestCases
        {
            get
            {
                foreach (
                    var query in
                        _testQueries.SelectMany(x => x).Select(t => t.Item2.EndToEndTestData.SetCategory(t.Item1.Name)))
                {
                    if (query.Arguments.Any(a => a == null))
                        query.Ignore("Query test case not complete");
                    yield return query;
                }
            }
        }

        public static void RegisterQuerySets(params QuerySet[] querySets)
        {
            foreach (var querySet in querySets)
                _testQueries.Add(querySet.Queries.Select(t => Tuple.Create(querySet.GetType(), t)));
        }
    }
}