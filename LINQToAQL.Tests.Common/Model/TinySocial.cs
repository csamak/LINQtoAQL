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

namespace LINQToAQL.Tests.Common.Model
{
    public class TinySocial
    {
        public TinySocial(Uri baseUri)
        {
            FacebookMessages = new AqlQueryable<FacebookMessage>(baseUri, "TinySocial");
            FacebookUsers = new AqlQueryable<FacebookUser>(baseUri, "TinySocial");
            TweetMessages = new AqlQueryable<TweetMessage>(baseUri, "TinySocial");
            TwitterUsers = new AqlQueryable<TwitterUser>(baseUri, "TinySocial");
        }

        public IQueryable<FacebookMessage> FacebookMessages { get; private set; }
        public IQueryable<FacebookUser> FacebookUsers { get; private set; }
        public IQueryable<TweetMessage> TweetMessages { get; private set; }
        public IQueryable<TwitterUser> TwitterUsers { get; private set; }
    }
}