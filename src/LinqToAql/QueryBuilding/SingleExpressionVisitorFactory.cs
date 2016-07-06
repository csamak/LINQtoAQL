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
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqToAql.QueryBuilding
{
    internal abstract class SingleExpressionVisitorFactory<T, TExpr> where TExpr : Expression
        where T : ISingleExpressionVisitor<TExpr>
    {
        private readonly List<Func<StringBuilder, ExpressionVisitor, T>> _functions =
            new List<Func<StringBuilder, ExpressionVisitor, T>>();

        public void RegisterVisitor<TVisitor>() where TVisitor : T, new()
        {
            _functions.Add((aqlExpression, visitor) =>
            {
                var v = new TVisitor();
                v.Initialize(aqlExpression, visitor);
                return v;
            });
        }

        //If typical initializations become expensive for some reason, we can cache these per visitor.
        public T CreateVisitableVisitor(TExpr expression, StringBuilder aqlExpression, ExpressionVisitor visitor)
        {
            return _functions.Select(f => f(aqlExpression, visitor)).FirstOrDefault(f => f.IsVisitable(expression));
        }
    }
}