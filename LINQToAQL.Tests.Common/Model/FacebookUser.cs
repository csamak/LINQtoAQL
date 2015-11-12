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
using LINQToAQL.DataAnnotations;
using Newtonsoft.Json;

namespace LINQToAQL.Tests.Common.Model
{
    [Dataset(Name = "FacebookUsers")]
    public class FacebookUser
    {
        public FacebookUser(Uri baseUri, string dataverse)
        {
            employment = new AqlQueryable<Employment>(baseUri, dataverse);
        }

        public int id { get; set; }
        public string alias { get; set; }
        public string name { get; set; }

        [Field(Name = "user-since")]
        public DateTime UserSince { get; set; }

        [Field(Name = "friend-ids")]
        public HashSet<int> FriendIds { get; set; } = new HashSet<int>(); //dupes allowed?

        [JsonIgnore]
        public IQueryable<Employment> employment { get; } //ordering?

        public override bool Equals(object obj)
        {
            var fbu = obj as FacebookUser;
            if (fbu == null) return false;
            return id == fbu.id && alias == fbu.alias && name == fbu.name && FriendIds.SetEquals(fbu.FriendIds);
        }

        public static bool operator ==(FacebookUser fbu1, FacebookUser fbu2)
        {
            if (ReferenceEquals(fbu1, fbu2))
                return true;
            if ((object) fbu1 == null || (object) fbu2 == null)
                return false;
            return fbu1.Equals(fbu2);
        }

        public static bool operator !=(FacebookUser fbu1, FacebookUser fbu2)
        {
            return !(fbu1 == fbu2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                hash = hash*31 + id.GetHashCode();
                hash = hash*31 + alias.GetHashCode();
                hash = hash*31 + name.GetHashCode();
                hash = hash*31 + UserSince.GetHashCode();
                hash = hash*31 + FriendIds.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            var friendIds = FriendIds == null ? "null" : string.Join(",", FriendIds);
            return $"Id: {id}, Name: {name}, Alias: {alias}, UserSince: {UserSince}, FriendIds: [{friendIds}]";
        }
    }
}