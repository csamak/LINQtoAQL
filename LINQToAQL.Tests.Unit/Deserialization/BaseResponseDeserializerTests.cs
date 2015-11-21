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
using System.IO;
using System.Linq;
using LINQToAQL.Deserialization;
using LINQToAQL.Tests.Common.Queries;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.Deserialization
{
    internal abstract class BaseResponseDeserializerTests
    {
        protected abstract IResponseDeserializer Deserializer { get; }

        [Test, TestCaseSource(typeof (QueryTestCases), nameof(QueryTestCases.DeserializationTestCases))]
        public void TestCommonQueries(string apiResponse, IEnumerable<object> expected, bool enforceOrder)
        {
            using (var reader = new StringReader(apiResponse))
            {
                var deserialized = Deserializer.DeserializeResponse(reader, QueryResultType(expected));
                if (enforceOrder)
                    CollectionAssert.AreEqual(expected, deserialized);
                else
                    CollectionAssert.AreEquivalent(expected, deserialized);
            }
        }

        private static Type QueryResultType(IEnumerable<object> queryResult)
        {
            if (queryResult == null) return null;
            var type = queryResult.GetType();
            return type.IsArray
                ? type.GetElementType()
                : type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .SingleOrDefault(t => t.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                    ?.GetGenericArguments()[0];
        }
    }
}