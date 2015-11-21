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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using LINQToAQL.Deserialization;
using LINQToAQL.Deserialization.Json;

namespace LINQToAQL
{
    internal class AqlQueryResultRetriever
    {
        private readonly HttpClient _client =
            new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        private readonly string _dataverse;
        private readonly IResponseDeserializer _deserializer = new JsonResponseDeserializer();

        public AqlQueryResultRetriever(Uri baseAddress, string dataverse)
        {
            if (baseAddress != null)
                _client.BaseAddress = baseAddress;
            _dataverse = dataverse;
        }

        //TODO: Read results incrementally
        public IEnumerable<T> GetResults<T>(string query)
        {
            using (
                var stream =
                    _client.PostAsync("query", new StringContent(FullQuery(query)))
                        .Result.Content.ReadAsStreamAsync()
                        .Result)
            using (var sr = new StreamReader(stream))
                foreach (T curr in _deserializer.DeserializeResponse<T>(sr))
                    yield return curr; //so the stream isn't disposed until we're done
        }

        public T GetScalar<T>(string query)
        {
            using (
                var stream =
                    _client.PostAsync("query", new StringContent(FullQuery(query)))
                        .Result.Content.ReadAsStreamAsync()
                        .Result)
            using (var sr = new StreamReader(stream))
                return _deserializer.DeserializeResponse<T>(sr).Single();
        }

        private string FullQuery(string query)
        {
            return $"use dataverse {_dataverse}; {query}";
        }
    }
}