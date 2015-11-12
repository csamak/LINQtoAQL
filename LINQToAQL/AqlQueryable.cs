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
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL
{
    /// <summary>
    ///     Evaluates queries remotely on an instance of AsterixDB.
    /// </summary>
    /// <typeparam name="T">The result type of the query</typeparam>
    public class AqlQueryable<T> : QueryableBase<T>
    {
        /// <summary>
        ///     Creates an <see cref="AqlQueryable{T}" /> with a dataverse and AsterixDB connection information.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="dataverse"></param>
        public AqlQueryable(Uri baseUri, string dataverse) :
            base(QueryParser.CreateDefault(), new AqlQueryExecutor(baseUri, dataverse))
        {
        }

        //TODO: Support an HttpClient overload, or the equivalent to allow specifying things like proxies

        //called by LINQ
        /// <inheritdoc />
        public AqlQueryable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }
}