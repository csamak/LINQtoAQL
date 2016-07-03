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

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace LinqToAql.QueryBuilding
{
    internal static class Operators
    {
        public static readonly ReadOnlyDictionary<ExpressionType, string> Binary =
            new ReadOnlyDictionary<ExpressionType, string>(new Dictionary<ExpressionType, string>
            {
                { ExpressionType.Equal, " = " },
                { ExpressionType.AndAlso, " and " },
                { ExpressionType.And, " and " },
                { ExpressionType.OrElse, " or " },
                { ExpressionType.Or, " or " },
                { ExpressionType.Add, " + " },
                { ExpressionType.Subtract, " - " },
                { ExpressionType.Multiply, " * " },
                { ExpressionType.Divide, " / " },
                { ExpressionType.GreaterThan, " > " },
                { ExpressionType.LessThan, " < " },
                { ExpressionType.GreaterThanOrEqual, " >= " },
                { ExpressionType.LessThanOrEqual, " <= " },
                { ExpressionType.NotEqual, " != " }
            });
    }
}