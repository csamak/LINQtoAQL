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

using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Tests.Common;
using LINQToAQL.Tests.Common.Model;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Tests.Unit.QueryBuilding
{
    internal abstract class QueryBuildingBase
    {
        protected readonly TinySocial dv = TestEnvironment.Dataverse;

        public static string GetQueryString(Expression exp)
        {
            return AqlQueryGenerator.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}