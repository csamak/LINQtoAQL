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

namespace LinqToAql.QueryBuilding.AqlConstructors
{
    //Why doesn't NewExpression happen? Evaluated locally? DateTime is a Struct?
    internal class DateTimeConstructor : AqlConstructorVisitor
    {
        public override bool IsVisitable(ConstantExpression expression) => expression.Type == typeof(DateTime);

        public override void Visit(ConstantExpression expression)
        {
            AqlExpression.AppendFormat("datetime('{0}')",
                ((DateTime) expression.Value).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
        }

        public override bool IsVisitable(NewExpression expression)
        {
            return false;
        }

        public override void Visit(NewExpression expression)
        {
            throw new NotImplementedException();
        }
    }
}