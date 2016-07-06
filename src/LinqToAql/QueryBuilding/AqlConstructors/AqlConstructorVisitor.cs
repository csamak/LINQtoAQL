﻿// Licensed to the Apache Software Foundation (ASF) under one
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

using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LinqToAql.QueryBuilding.AqlConstructors
{
    internal abstract class AqlConstructorVisitor
        : ISingleExpressionVisitor<NewExpression>, ISingleExpressionVisitor<ConstantExpression>
    {
        protected StringBuilder AqlExpression;
        protected ExpressionVisitor Visitor;

        public abstract bool IsVisitable(ConstantExpression expression);
        public abstract void Visit(ConstantExpression expression);

        public virtual void Initialize(StringBuilder aqlExpression, ExpressionVisitor visitor)
        {
            AqlExpression = aqlExpression;
            Visitor = visitor;
        }

        public abstract bool IsVisitable(NewExpression expression);
        public abstract void Visit(NewExpression expression);

        protected void AqlConstructor(string name, params Expression[] args)
        {
            AqlExpression.Append($"{name}(");
            Visitor.Visit(args[0]);
            foreach (var arg in args.Skip(1))
            {
                AqlExpression.Append(", ");
                Visitor.Visit(arg);
            }
            AqlExpression.Append(")");
        }
    }
}