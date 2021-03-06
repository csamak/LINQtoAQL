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

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace LinqToAql.Tests.Common.Model.Data
{
    internal static class TinySocialData
    {
        static TinySocialData()
        {
            var assembly = typeof(TinySocialData).GetTypeInfo().Assembly;
            var prefix = $"{assembly.GetName().Name}.Resources.";
            var serializer = new JsonSerializer();
            using (var tr = new JsonTextReader(new StreamReader(assembly.GetManifestResourceStream($"{prefix}FacebookMessages.json"))))
                FacebookMessages = serializer.Deserialize<IEnumerable<FacebookMessage>>(tr);
            using (var tr = new JsonTextReader(new StreamReader(assembly.GetManifestResourceStream($"{prefix}FacebookUsers.json"))))
                FacebookUsers = serializer.Deserialize<IEnumerable<FacebookUser>>(tr);
            using (var tr = new JsonTextReader(new StreamReader(assembly.GetManifestResourceStream($"{prefix}TweetMessages.json"))))
                TweetMessages = serializer.Deserialize<IEnumerable<TweetMessage>>(tr);
        }

        public static IEnumerable<FacebookMessage> FacebookMessages { get; }

        public static IEnumerable<FacebookUser> FacebookUsers { get; }

        public static IEnumerable<TweetMessage> TweetMessages { get; }
    }
}