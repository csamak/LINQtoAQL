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
using LINQToAQL.Tests.Common;
using LINQToAQL.Tests.Common.Model;
using LINQToAQL.Tests.Common.Queries;
using LINQToAQL.Tests.Integration.Properties;
using NUnit.Framework;

namespace LINQToAQL.Tests.Integration
{
    internal class CommonQueryTests
    {
        private readonly TinySocial _dv = TestEnvironment.Dataverse;

        [TestFixtureSetUp]
        public void SetupTinySocialData()
        {
            try
            {
                TestEnvironment.ExecuteAql(Resources.TinySocial);
            }
            catch (Exception e)
            {
                throw new Exception("Could not set up TinySocial dataverse", e);
            }
        }

        [Test, TestCaseSource(typeof (QueryTestCases), nameof(QueryTestCases.EndToEndTestCases))]
        public object TestCommonQueries(IQueryable<object> linqQuery)
        {
            return linqQuery.ToList(); //force remote evaluation
        }

        [Test]
        public void CountScalar()
        {
            //TODO: support LongCount()
            Assert.AreEqual(_dv.FacebookUsers.Count(), 10);
        }
    }
}