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
using LINQToAQL.QueryBuilding;
using Remotion.Linq;

namespace LINQToAQL
{
    internal class AqlQueryExecutor : IQueryExecutor
    {
        private readonly AqlQueryResultRetriever _resultRetriever;

        public AqlQueryExecutor(Uri baseUri, string dataverse)
        {
            _resultRetriever = new AqlQueryResultRetriever(baseUri, dataverse);
        }

        /// <inheritdoc />
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            return _resultRetriever.GetResults<T>(AqlQueryGenerator.GenerateAqlQuery(queryModel));
        }

        /// <inheritdoc />
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return _resultRetriever.GetScalar<T>(AqlQueryGenerator.GenerateAqlQuery(queryModel));
        }

        /// <inheritdoc />
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            throw new NotImplementedException();
        }
    }
}