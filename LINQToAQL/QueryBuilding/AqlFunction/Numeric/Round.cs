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
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.Numeric
{
    //Math.Round defaults to MidpointRounding.ToEven, which is equivalent to numeric-round-half-to-even
    internal class Round : AqlFunctionVisitor
    {
        public Round(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return
                new[]
                {
                    typeof (Math).GetMethod("Round", new[] {typeof (double)}),
                    typeof (Math).GetMethod("Round", new[] {typeof (decimal)})
                }.Contains(expression.Method) ||
                (new[]
                {
                    typeof (Math).GetMethod("Round", new[] {typeof (double), typeof (MidpointRounding)}),
                    typeof (Math).GetMethod("Round", new[] {typeof (decimal), typeof (MidpointRounding)})
                }.Contains(
                    expression.Method) &&
                 ((ConstantExpression) expression.Arguments[1]).Value.Equals(MidpointRounding.ToEven));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("round-half-to-even", expression.Arguments[0]);
        }
    }
}