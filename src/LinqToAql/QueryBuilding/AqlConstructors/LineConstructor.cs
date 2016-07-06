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
using LinqToAql.Spatial;

namespace LinqToAql.QueryBuilding.AqlConstructors
{
    internal class LineConstructor : AqlConstructorVisitor
    {
        private const string CreateLine = "create-line";

        public override bool IsVisitable(ConstantExpression expression)
        {
            return expression.Type == typeof(Line);
        }

        public override void Visit(ConstantExpression expression)
        {
            var line = (Line) expression.Value;
            AqlExpression.Append(
                $"{CreateLine}(create-point({line.First.X}, {line.First.Y}), create-point({line.Second.X}, {line.Second.Y}))");
        }

        public override bool IsVisitable(NewExpression expression)
        {
            return expression.Type == typeof(Line);
        }

        public override void Visit(NewExpression expression)
        {
            AqlConstructor(CreateLine, expression.Arguments[0], expression.Arguments[1]);
        }
    }
}