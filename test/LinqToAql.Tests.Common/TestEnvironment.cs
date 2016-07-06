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
using System.Net.Http;
using LinqToAql.Tests.Common.Model;
using Microsoft.Extensions.Configuration;

namespace LinqToAql.Tests.Common
{
    public static class TestEnvironment
    {
        private const string ConfigAsterixDbEndpoint = "AsterixDBEndpoint";

        private static Dictionary<string, string> defaultConfigValues = new Dictionary<string, string>
        {
            { ConfigAsterixDbEndpoint, "http://localhost:19002" }
        };

        private static readonly IConfigurationRoot config =
            new ConfigurationBuilder().AddInMemoryCollection(defaultConfigValues)
                .AddJsonFile("testsettings.json", true)
                .AddEnvironmentVariables("LinqToAql.Tests.")
                .Build();

        private static readonly Uri AsterixDbEndpoint = new Uri(config[ConfigAsterixDbEndpoint]);

        private static readonly HttpClient Client = new HttpClient();

        public static readonly TinySocial Dataverse = new TinySocial(AsterixDbEndpoint);

        public static string ExecuteAql(string aql)
        {
            return
                Client.PostAsync(new Uri(AsterixDbEndpoint, "aql"), new StringContent(aql))
                    .Result.Content.ReadAsStringAsync()
                    .Result;
        }
    }
}