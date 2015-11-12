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
using LINQToAQL.Similarity;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding.AqlFunction
{
    internal class SimilarityTests : QueryBuildingBase
    {
        [Test]
        public void OnlyAllowsRemoteExecution()
        {
            OnlyRemote(() => "".EditDistance("other"));
            OnlyRemote(() => "".EditDistanceCheck("other", 32));
            OnlyRemote(() => new[] {1, 5, 9}.Jaccard(new[] {1, 7, 9}));
            OnlyRemote(() => new[] {1, 5, 9}.JaccardCheck(new[] {1, 7, 9}, 0.6));
        }

        private static void OnlyRemote(Action x)
        {
            //AsterixRemoteOnlyException is internal, so we can't use Assert.Throws<AsterixRemoteOnlyException>
            try
            {
                x();
            }
            catch (Exception ex)
            {
                Assert.True(ex.GetType().Name == "AsterixRemoteOnlyException");
                return;
            }
            Assert.Fail("AsterixRemoteOnlyException has not been thrown.");
        }
    }
}