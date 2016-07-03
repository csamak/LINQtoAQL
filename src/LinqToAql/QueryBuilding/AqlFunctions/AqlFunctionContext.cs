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
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LinqToAql.QueryBuilding.AqlFunctions.Numeric;
using LinqToAql.QueryBuilding.AqlFunctions.Similarity;
using LinqToAql.QueryBuilding.AqlFunctions.Spatial;
using LinqToAql.QueryBuilding.AqlFunctions.String;
using LinqToAql.QueryBuilding.AqlFunctions.Tokenizing;

namespace LinqToAql.QueryBuilding.AqlFunctions
{
    internal class AqlFunctionContext
    {
        private readonly ReadOnlyCollection<AqlFunctionVisitor> _functions;

        public AqlFunctionContext(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
        {
            //TODO: Improve registration rather than keeping a list here (also needed for UDFs)
            _functions =
                new ReadOnlyCollection<AqlFunctionVisitor>(
                    new[]
                    {
                        typeof(Abs), typeof(Ceiling), typeof(Floor), typeof(Round), typeof(CharIndex),
                        typeof(Contains), typeof(EndsWith), typeof(Join), typeof(Lowercase), typeof(StartsWith),
                        typeof(Substring), typeof(SubstringWithLength), typeof(ToCodepoint), typeof(EditDistance),
                        typeof(Jaccard), typeof(WordTokens), typeof(SpatialDistance)
                    }.Select(
                        t => Activator.CreateInstance(t, aqlExpression, visitor))
                        .Cast<AqlFunctionVisitor>()
                        .ToList());
        }

        public AqlFunctionVisitor FindVisitableVisitor(MethodCallExpression expression)
        {
            return _functions.FirstOrDefault(f => f.IsVisitable(expression));
        }
    }
}