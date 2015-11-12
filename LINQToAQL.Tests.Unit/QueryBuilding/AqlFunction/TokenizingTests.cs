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
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding.AqlFunction
{
    internal class TokenizingTests : QueryBuildingBase
    {
        [Test]
        public void WordTokensRemoveEmptyEntries()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
        }

        [Test]
        public void WordTokensWrongSeparator()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new[] {"wrong"}, StringSplitOptions.None));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
            query = dv.FacebookUsers.Select(u => u.name.Split('x'));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
        }
    }
}