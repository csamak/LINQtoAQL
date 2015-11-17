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
using LINQToAQL.Tests.Common.Model.Data;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class StringQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("StringToCodepoint")
            {
                LinqQuery = dv.FacebookUsers.Select(u => u.name.ToCharArray()),
                Aql = "for $u in dataset FacebookUsers return string-to-codepoint($u.name)",
                CleanJsonApi =
                    "[[87,105,108,108,105,115,87,121,110,110,101],[77,97,114,103,97,114,105,116,97,83,116,111,100,100,97,114,100],[73,115,98,101,108,68,117,108,108],[78,105,99,104,111,108,97,115,83,116,114,111,104],[78,105,108,97,77,105,108,108,105,114,111,110],[87,111,111,100,114,111,119,78,101,104,108,105,110,103],[66,114,97,109,72,97,116,99,104],[69,109,111,114,121,85,110,107],[86,111,110,75,101,109,98,108,101],[83,117,122,97,110,110,97,84,105,108,108,115,111,110]]",
                QueryResult = TinySocialData.FacebookUsers.Select(u => u.name.ToCharArray())
            },
            new TestQuery("CodepointToString")
            {
                LinqQuery = dv.FacebookUsers.Select(u => new string(u.name.ToCharArray())),
                Aql = "for $u in dataset FacebookUsers return codepoint-to-string(string-to-codepoint($u.name))",
                CleanJsonApi =
                    "[\"WillisWynne\",\"MargaritaStoddard\",\"IsbelDull\",\"NicholasStroh\",\"NilaMilliron\",\"WoodrowNehling\",\"BramHatch\",\"EmoryUnk\",\"VonKemble\",\"SuzannaTillson\"]",
                QueryResult = TinySocialData.FacebookUsers.Select(u => u.name)
            },
            new TestQuery("Contains")
            {
                LinqQuery =
                    dv.FacebookMessages.Where(i => i.Message.Contains("phone"))
                        .Select(i => new {mid = i.Id, message = i.Message}),
                Aql =
                    "for $i in dataset FacebookMessages where contains($i.message, \"phone\") return { \"mid\": $i.message-id, \"message\": $i.message }",
                CleanJsonApi =
                    "[{\"mid\":2,\"message\":\" dislike iphone its touch-screen is horrible\"},{\"mid\":13,\"message\":\" dislike iphone the voice-command is bad:(\"},{\"mid\":15,\"message\":\" like iphone the voicemail-service is awesome\"} ]",
                QueryResult =
                    TinySocialData.FacebookMessages.Where(i => i.Message.Contains("phone"))
                        .Select(i => new {mid = i.Id, message = i.Message})
            },
            new TestQuery("StartsWith")
            {
                LinqQuery = dv.FacebookMessages.Where(i => i.Message.StartsWith(" like")).Select(i => i.Message),
                Aql = "for $i in dataset FacebookMessages where starts-with($i.message, \" like\") return $i.message",
                CleanJsonApi =
                    "[\" like t-mobile its platform is mind-blowing\",\" like iphone the voicemail-service is awesome\",\" like verizon the 3G is awesome:)\",\" like samsung the plan is amazing\"]",
                QueryResult =
                    TinySocialData.FacebookMessages.Where(i => i.Message.StartsWith(" like")).Select(i => i.Message)
            },
            new TestQuery("EndsWith")
            {
                LinqQuery = dv.FacebookMessages.Where(i => i.Message.EndsWith(":)")).Select(i => i.Message),
                Aql = "for $i in dataset FacebookMessages where ends-with($i.message, \":)\") return $i.message",
                CleanJsonApi =
                    "[\" love at&t its 3G is good:)\",\" love sprint its shortcut-menu is awesome:)\",\" like verizon the 3G is awesome:)\"]",
                QueryResult =
                    TinySocialData.FacebookMessages.Where(i => i.Message.EndsWith(":)")).Select(i => i.Message)
            },
            new TestQuery("StringJoin")
            {
                LinqQuery = dv.FacebookUsers.Where(u => u.id < 3).Select(m => string.Join(",", m.alias, m.name)),
                Aql =
                    "for $u in dataset FacebookUsers where ($u.id < 3) return string-join([$u.alias, $u.name, ], \",\")",
                CleanJsonApi = "[\"Margarita,MargaritaStoddard\",\"Isbel,IsbelDull\"]",
                QueryResult =
                    TinySocialData.FacebookUsers.Where(u => u.id < 3).Select(m => string.Join(",", m.alias, m.name))
            },
            new TestQuery("ToLower")
            {
                LinqQuery = dv.FacebookUsers.Select(u => u.name.ToLower()),
                Aql = "for $u in dataset FacebookUsers return lowercase($u.name)",
                CleanJsonApi =
                    "[\"williswynne\",\"margaritastoddard\",\"isbeldull\",\"nicholasstroh\",\"nilamilliron\",\"woodrownehling\",\"bramhatch\",\"emoryunk\",\"vonkemble\",\"suzannatillson\"]",
                QueryResult = TinySocialData.FacebookUsers.Select(u => u.name.ToLower())
            },
            new TestQuery("StringLength")
            {
                LinqQuery = dv.FacebookMessages.Select(i => new {mid = i.Id, mlen = i.Message.Length}),
                Aql =
                    "for $i in dataset FacebookMessages return { \"mid\": $i.message-id, \"mlen\": string-length($i.message) }",
                CleanJsonApi =
                    "[{\"mid\":6,\"mlen\":43},{\"mid\":11,\"mlen\":38},{\"mid\":12,\"mlen\":52},{\"mid\":14,\"mlen\":27},{\"mid\":1,\"mlen\":43},{\"mid\":2,\"mlen\":44},{\"mid\":4,\"mlen\":43},{\"mid\":13,\"mlen\":42},{\"mid\":15,\"mlen\":45},{\"mid\":8,\"mlen\":33},{\"mid\":9,\"mlen\":34},{\"mid\":10,\"mlen\":50},{\"mid\":3,\"mlen\":33},{\"mid\":5,\"mlen\":46},{\"mid\":7,\"mlen\":37}]",
                QueryResult = TinySocialData.FacebookMessages.Select(i => new {mid = i.Id, mlen = i.Message.Length})
            },
            //TODO: should this project subtract 1 for the user to the behavior is the same as C#?
            new TestQuery("SubstringToEnd")
            {
                LinqQuery = dv.FacebookMessages.Select(i => i.Message.Substring(29)),
                Aql = "for $i in dataset FacebookMessages return substring($i.message, (29 + 1))",
                CleanJsonApi =
                    "[\"s mind-blowing\",\" terrible\",\"cemail-service is OMG:(\",\"\",\"u is awesome:)\",\"een is horrible\",\" is horrible:(\",\"mand is bad:(\",\"rvice is awesome\",\"me:)\",\" good\",\"ch-screen is terrible\",\"zing\",\"n is mind-blowing\",\"horrible\"]",
                QueryResult =
                    TinySocialData.FacebookMessages.Select(m => m.Message.Length > 29 ? m.Message.Substring(29) : "")
            },
            new TestQuery("SubstringLimitedLength")
            {
                LinqQuery = dv.FacebookMessages.Select(i => i.Message.Substring(29, 5)),
                Aql = "for $i in dataset FacebookMessages return substring($i.message, (29 + 1), 5)",
                CleanJsonApi =
                    "[\"s min\",\" terr\",\"cemai\",\"\",\"u is \",\"een i\",\" is h\",\"mand \",\"rvice\",\"me:)\",\" good\",\"ch-sc\",\"zing\",\"n is \",\"horri\"]",
                QueryResult =
                    TinySocialData.FacebookMessages.Select(
                        m => m.Message.Length > 29 ? m.Message.Substring(29, Math.Min(5, m.Message.Length - 29)) : "")
            }
        };
    }
}