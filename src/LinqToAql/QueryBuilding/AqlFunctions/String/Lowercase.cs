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
using System.Linq.Expressions;
using System.Reflection;

namespace LinqToAql.QueryBuilding.AqlFunctions.String
{
    internal class Lowercase : AqlFunctionVisitor
    {
        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof(string).GetTypeInfo().GetMethod("ToLower", new Type[0]));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("lowercase", expression.Object);
        }
    }
}