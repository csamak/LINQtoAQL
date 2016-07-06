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
using System.Reflection;

namespace LinqToAql.QueryBuilding.AqlFunctions.Tokenizing
{
    internal class WordTokens : AqlFunctionVisitor
    {
        public override bool IsVisitable(MethodCallExpression expression)
        {
            if (expression.Method.Equals(typeof(string).GetTypeInfo().GetMethod("Split", new[] { typeof(char[]) })))
            {
                var arg = (IEnumerable<char>) ((ConstantExpression) expression.Arguments[0]).Value;
                return arg.Count() == 1 && arg.First() == ' ';
            }
            if (expression.Method.Equals(typeof(string).GetTypeInfo()
                .GetMethod("Split", new[] { typeof(string[]), typeof(StringSplitOptions) })))
            {
                var arg = (IEnumerable<string>) ((ConstantExpression) expression.Arguments[0]).Value;
                return ((ConstantExpression) expression.Arguments[1]).Value.Equals(StringSplitOptions.None) &&
                       arg.Count() == 1 && arg.First() == " ";
            }
            if (expression.Method.Equals(typeof(string).GetTypeInfo()
                .GetMethod("Split", new[] { typeof(char[]), typeof(StringSplitOptions) })))
            {
                var arg = (IEnumerable<char>) ((ConstantExpression) expression.Arguments[0]).Value;
                return ((ConstantExpression) expression.Arguments[1]).Value.Equals(StringSplitOptions.None) &&
                       arg.Count() == 1 && arg.First() == ' ';
            }
            return false;
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("word-tokens", expression.Object);
        }
    }
}