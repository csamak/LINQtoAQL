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
using NUnit.Framework;

namespace LinqToAql.Tests.Unit.QueryBuilding.AqlFunctions
{
    internal class AggregateTests : QueryBuildingBase
    {
        [Test]
        public void Count()
        {
            Expression<Func<int>> query = () => dv.FacebookMessages.Count();
            Assert.AreEqual("count(for $generated_1 in dataset FacebookMessages return $generated_1)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Avg()
        {
            Expression<Func<double>> query = () => dv.FacebookMessages.Average(m => m.Id);
            Assert.AreEqual("avg(for $m in dataset FacebookMessages return $m.message-id)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Sum()
        {
            Expression<Func<int>> query = () => dv.TwitterUsers.Sum(i => i.friends_count);
            Assert.AreEqual("sum(for $i in dataset TwitterUsers return $i.friends_count)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Max()
        {
            Expression<Func<int>> query = () => dv.TwitterUsers.Max(i => i.friends_count);
            Assert.AreEqual("max(for $i in dataset TwitterUsers return $i.friends_count)",
                GetQueryString(query.Body));
        }

        [Test]
        public void Min()
        {
            Expression<Func<int>> query = () => dv.TwitterUsers.Min(i => i.friends_count);
            Assert.AreEqual("min(for $i in dataset TwitterUsers return $i.friends_count)",
                GetQueryString(query.Body));
        }
    }
}