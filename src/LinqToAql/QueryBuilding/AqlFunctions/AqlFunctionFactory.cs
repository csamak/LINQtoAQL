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
using LinqToAql.QueryBuilding.AqlFunctions.Numeric;
using LinqToAql.QueryBuilding.AqlFunctions.Similarity;
using LinqToAql.QueryBuilding.AqlFunctions.Spatial;
using LinqToAql.QueryBuilding.AqlFunctions.String;
using LinqToAql.QueryBuilding.AqlFunctions.Tokenizing;

namespace LinqToAql.QueryBuilding.AqlFunctions
{
    internal class AqlFunctionFactory : SingleExpressionVisitorFactory<AqlFunctionVisitor, MethodCallExpression>
    {
        public AqlFunctionFactory()
        {
            //Should we autodetect these with reflection?
            RegisterVisitor<Abs>();
            RegisterVisitor<Ceiling>();
            RegisterVisitor<Floor>();
            RegisterVisitor<Round>();
            RegisterVisitor<CharIndex>();
            RegisterVisitor<Contains>();
            RegisterVisitor<EndsWith>();
            RegisterVisitor<Join>();
            RegisterVisitor<Lowercase>();
            RegisterVisitor<StartsWith>();
            RegisterVisitor<Substring>();
            RegisterVisitor<SubstringWithLength>();
            RegisterVisitor<ToCodepoint>();
            RegisterVisitor<EditDistance>();
            RegisterVisitor<Jaccard>();
            RegisterVisitor<WordTokens>();
            RegisterVisitor<SpatialDistance>();
        }

    }
}