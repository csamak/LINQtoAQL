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

using LINQToAQL.DataAnnotations;
using LINQToAQL.Spatial;

namespace LINQToAQL.Tests.Common.Model
{
    [Dataset(Name = "FacebookMessages", Open = false)]
    public class FacebookMessage
    {
        [Field(Name = "message-id")]
        public int Id { get; set; }

        [Field(Name = "author-id")]
        public int? AuthorId { get; set; }

        [Field(Name = "in-response-to")]
        public int? InResponseTo { get; set; }

        [Field(Name = "sender-location")]
        public Point SenderLocation { get; set; } //not so sure about GeoCoordinate

        [Field(Name = "message")]
        public string Message { get; set; }
    }
}