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
using NUnit.Framework;

namespace LINQToAQL.Tests.Common.Queries
{
    public class TestQuery
    {
        public TestQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public IQueryable<object> LinqQuery { get; set; }
        public string Aql { get; set; }
        public string CleanJsonApi { get; set; }
        public IEnumerable<object> QueryResult { get; set; }
        public bool EnforceOrder { get; set; }

        public TestCaseData QuerySynthesisTestData => new TestCaseData(LinqQuery).Returns(Aql).SetName(Name);

        public TestCaseData DeserializationTestData
            => new TestCaseData(CleanJsonApi, QueryResult, EnforceOrder).SetName(Name);

        public TestCaseData EndToEndTestData => new TestCaseData(QueryResult, LinqQuery, EnforceOrder).SetName(Name);
    }
}