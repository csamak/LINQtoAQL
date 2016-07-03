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
using System.Linq;
using LinqToAql.DataAnnotations;
using LinqToAql.Spatial;

namespace LinqToAql.Tests.Common.Model
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

        public override bool Equals(object obj)
        {
            var tweet = obj as TweetMessage;
            if (tweet == null) return false;
            return tweetid == tweet.tweetid && user.Equals(tweet.user) && SenderLocation == tweet.SenderLocation && MessageText == tweet.MessageText && ReferredTopics.SequenceEqual(tweet.ReferredTopics);
        }

        public static bool operator ==(TweetMessage tweet1, TweetMessage tweet2)
        {
            if (ReferenceEquals(tweet1, tweet2))
                return true;
            if ((object)tweet1 == null || (object)tweet2 == null)
                return false;
            return tweet1.Equals(tweet2);
        }

        public static bool operator !=(TweetMessage tweet1, TweetMessage tweet2)
        {
            return !(tweet1 == tweet2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash * 31 + tweetid.GetHashCode();
                hash = hash * 31 + user.GetHashCode();
                hash = hash * 31 + SenderLocation.GetHashCode();
                hash = hash * 31 + SendTime.GetHashCode();
                hash = hash * 31 + MessageText.GetHashCode();
                return hash;
            }
        }
    }
}