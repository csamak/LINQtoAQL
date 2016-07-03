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
using System.Reflection;
using System.Text;

namespace LinqToAql.QueryBuilding.AqlFunctions.String
{
    internal class SubstringWithLength : AqlFunctionVisitor
    {
        public SubstringWithLength(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
            : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof(string).GetTypeInfo()
                .GetMethod("Substring", new[] { typeof(int), typeof(int) }));
        }

        //AQL uses offset while C# uses index. Also, the length is relative in AQL while it is a final index in C#.
        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("substring", expression.Object,
                Expression.MakeBinary(ExpressionType.Add, expression.Arguments[0], Expression.Constant(1)),
                expression.Arguments[1]);
        }
    }
}