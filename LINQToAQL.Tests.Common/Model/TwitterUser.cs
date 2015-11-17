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

namespace LINQToAQL.Tests.Common.Model
{
    [Dataset(Name = "TwitterUsers", Open = true)]
    public class TwitterUser
    {
        [Field(Name = "screen-name")]
        public string ScreenName { get; set; }

        public string lang { get; set; }
        public int friends_count { get; set; }
        public int statuses_count { get; set; }
        public string name { get; set; }
        public int followers_count { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TwitterUser) obj);
        }

        private bool Equals(TwitterUser other)
        {
            return string.Equals(ScreenName, other.ScreenName) && string.Equals(lang, other.lang) &&
                   friends_count == other.friends_count && statuses_count == other.statuses_count &&
                   string.Equals(name, other.name) && followers_count == other.followers_count;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ScreenName?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ (lang?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ friends_count;
                hashCode = (hashCode*397) ^ statuses_count;
                hashCode = (hashCode*397) ^ (name?.GetHashCode() ?? 0);
                hashCode = (hashCode*397) ^ followers_count;
                return hashCode;
            }
        }

        public static bool operator ==(TwitterUser left, TwitterUser right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(TwitterUser left, TwitterUser right)
        {
            return !Equals(left, right);
        }
    }
}