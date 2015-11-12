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
using LINQToAQL.DataAnnotations;
using LINQToAQL.Spatial;

namespace LINQToAQL.Tests.Common.Model
{
    [Dataset(Name = "TweetMessages")]
    public class TweetMessage
    {
        public string tweetid { get; set; }
        public TwitterUser user { get; set; }

        [Field(Name = "sender-location")]
        public Point SenderLocation { get; set; }

        [Field(Name = "send-time")]
        public DateTime SendTime { get; set; }

        //referred-topics: {{ string }}
        //list the right type?
        [Field(Name = "referred-topics")]
        public List<string> ReferredTopics { get; set; }

        [Field(Name = "message-text")]
        public string MessageText { get; set; }
    }
}