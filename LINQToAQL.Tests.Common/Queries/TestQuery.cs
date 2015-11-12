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
using System.Collections;
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

        public TestCaseData QuerySynthesisTestData => new TestCaseData(LinqQuery).Returns(Aql).SetName(Name);

        public TestCaseData DeserializationTestData
            =>
                new TestCaseData(CleanJsonApi, QueryResultType(QueryResult)).Returns(QueryResult)
                    .SetName(Name);

        public TestCaseData EndToEndTestData => new TestCaseData(LinqQuery).Returns(QueryResult).SetName(Name);

        private static Type QueryResultType(IEnumerable queryResult)
        {
            if (queryResult == null) return null;
            var type = queryResult.GetType();
            return type.IsArray
                ? type.GetElementType()
                : type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .SingleOrDefault(t => t.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                    .GetGenericArguments()[0];
        }
    }
}