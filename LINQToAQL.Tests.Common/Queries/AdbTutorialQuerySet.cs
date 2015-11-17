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
using LINQToAQL.Similarity;
using LINQToAQL.Tests.Common.Model.Data;

namespace LINQToAQL.Tests.Common.Queries
{
    /// <summary>
    ///     Tests queries used in the "AsterixDB 101: An ADM and Aql Primer" tutorial, found at
    ///     http://asterixdb.ics.uci.edu/documentation/aql/primer.html.
    /// </summary>
    internal class AdbTutorialQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("ExactMatch0A")
            {
                LinqQuery = from user in dv.FacebookUsers where user.id == 8 select user,
                Aql = "for $user in dataset FacebookUsers where ($user.id = 8) return $user",
                CleanJsonApi =
                    "[{\"id\":8,\"alias\":\"Nila\",\"name\":\"NilaMilliron\",\"user-since\":\"2008-01-01T10:10:00.000Z\",\"friend-ids\":[3],\"employment\":[{\"organization-name\":\"Plexlane\",\"start-date\":\"2010-02-28\",\"end-date\":null}]}]",
                QueryResult = from user in TinySocialData.FacebookUsers where user.id == 8 select user
            },
            new TestQuery("RangeScan0B")
            {
                LinqQuery = from user in dv.FacebookUsers where user.id >= 2 && user.id <= 4 select user,
                Aql =
                    "for $user in dataset FacebookUsers where (($user.id >= 2) and ($user.id <= 4)) return $user",
                CleanJsonApi =
                    "[{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult = from user in TinySocialData.FacebookUsers where user.id >= 2 && user.id <= 4 select user
            },
            new TestQuery("OtherQueryFilters1")
            {
                LinqQuery =
                    dv.FacebookUsers.Where(
                        user =>
                            user.UserSince >= new DateTime(2010, 7, 22) &&
                            user.UserSince <= new DateTime(2012, 7, 29, 23, 59, 59)),
                Aql =
                    $"for $user in dataset FacebookUsers where (($user.user-since >= datetime('2010-07-22T00:00:00.000{new DateTime(2010, 7, 22).ToString("zzz")}')) and ($user.user-since <= datetime('2012-07-29T23:59:59.000{new DateTime(2012, 7, 29, 23, 59, 59).ToString("zzz")}'))) return $user",
                CleanJsonApi =
                    "[{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult =
                    TinySocialData.FacebookUsers.Where(
                        user =>
                            user.UserSince >= new DateTime(2010, 7, 22) &&
                            user.UserSince <= new DateTime(2012, 7, 29, 23, 59, 59))
            },
            new TestQuery("EquiJoin2AWhereSyntax")
            {
                LinqQuery = from user in dv.FacebookUsers
                    from message in dv.FacebookMessages
                    where message.AuthorId == user.id
                    select new {uname = user.name, message = message.Message},
                Aql =
                    "for $user in dataset FacebookUsers for $message in dataset FacebookMessages where ($message.author-id = $user.id) return { \"uname\": $user.name, \"message\": $message.message }",
                CleanJsonApi =
                    "[{\"uname\":\"WillisWynne\",\"message\":\" love sprint the customization is mind-blowing\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" can't stand at&t its plan is terrible\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" dislike iphone its touch-screen is horrible\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" can't stand at&t the network is horrible:(\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" like verizon the 3G is awesome:)\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" can't stand motorola the touch-screen is terrible\"},{\"uname\":\"IsbelDull\",\"message\":\" like t-mobile its platform is mind-blowing\"},{\"uname\":\"IsbelDull\",\"message\":\" like samsung the plan is amazing\"},{\"uname\":\"WoodrowNehling\",\"message\":\" love at&t its 3G is good:)\"},{\"uname\":\"BramHatch\",\"message\":\" can't stand t-mobile its voicemail-service is OMG:(\"},{\"uname\":\"BramHatch\",\"message\":\" dislike iphone the voice-command is bad:(\"},{\"uname\":\"EmoryUnk\",\"message\":\" love sprint its shortcut-menu is awesome:)\"},{\"uname\":\"EmoryUnk\",\"message\":\" love verizon its wireless is good\"},{\"uname\":\"VonKemble\",\"message\":\" dislike sprint the speed is horrible\"},{\"uname\":\"SuzannaTillson\",\"message\":\" like iphone the voicemail-service is awesome\"}]",
                QueryResult = from user in TinySocialData.FacebookUsers
                    from message in TinySocialData.FacebookMessages
                    where message.AuthorId == user.id
                    select new {uname = user.name, message = message.Message}
            },
            new TestQuery("EquiJoin2AJoinSyntax")
            {
                LinqQuery = from user in dv.FacebookUsers
                    join message in dv.FacebookMessages on user.id equals message.AuthorId
                    select new {uname = user.name, message = message.Message},
                Aql =
                    "for $user in dataset FacebookUsers for $message in dataset FacebookMessages where ($user.id = $message.author-id) return { \"uname\": $user.name, \"message\": $message.message }",
                CleanJsonApi =
                    "[{\"uname\":\"WillisWynne\",\"message\":\" love sprint the customization is mind-blowing\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" can't stand at&t its plan is terrible\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" dislike iphone its touch-screen is horrible\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" can't stand at&t the network is horrible:(\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" like verizon the 3G is awesome:)\"},{\"uname\":\"MargaritaStoddard\",\"message\":\" can't stand motorola the touch-screen is terrible\"},{\"uname\":\"IsbelDull\",\"message\":\" like t-mobile its platform is mind-blowing\"},{\"uname\":\"IsbelDull\",\"message\":\" like samsung the plan is amazing\"},{\"uname\":\"WoodrowNehling\",\"message\":\" love at&t its 3G is good:)\"},{\"uname\":\"BramHatch\",\"message\":\" can't stand t-mobile its voicemail-service is OMG:(\"},{\"uname\":\"BramHatch\",\"message\":\" dislike iphone the voice-command is bad:(\"},{\"uname\":\"EmoryUnk\",\"message\":\" love sprint its shortcut-menu is awesome:)\"},{\"uname\":\"EmoryUnk\",\"message\":\" love verizon its wireless is good\"},{\"uname\":\"VonKemble\",\"message\":\" dislike sprint the speed is horrible\"},{\"uname\":\"SuzannaTillson\",\"message\":\" like iphone the voicemail-service is awesome\"}]",
                QueryResult = from user in TinySocialData.FacebookUsers
                    join message in TinySocialData.FacebookMessages on user.id equals message.AuthorId
                    select new {uname = user.name, message = message.Message}
            },
            new TestQuery("NestedOuterJoin3")
            {
                LinqQuery = from user in dv.FacebookUsers
                    select new StringAndEnumerableResult<string>
                    {
                        String = user.name,
                        Enumerable =
                            from message in dv.FacebookMessages
                            where message.AuthorId == user.id
                            orderby message.Id
                            select message.Message
                    },
                Aql =
                    "for $user in dataset FacebookUsers return { \"String\": $user.name, \"Enumerable\": (for $message in dataset FacebookMessages where ($message.author-id = $user.id) order by $message.message-id asc return $message.message) }",
                CleanJsonApi =
                    "[{\"String\":\"WillisWynne\",\"Enumerable\":[\" love sprint the customization is mind-blowing\"]},{\"String\":\"MargaritaStoddard\",\"Enumerable\":[\" dislike iphone its touch-screen is horrible\",\" can't stand at&t the network is horrible:(\",\" like verizon the 3G is awesome:)\",\" can't stand motorola the touch-screen is terrible\",\" can't stand at&t its plan is terrible\"]},{\"String\":\"IsbelDull\",\"Enumerable\":[\" like samsung the plan is amazing\",\" like t-mobile its platform is mind-blowing\"]},{\"String\":\"NicholasStroh\",\"Enumerable\":[]},{\"String\":\"NilaMilliron\",\"Enumerable\":[]},{\"String\":\"WoodrowNehling\",\"Enumerable\":[\" love at&t its 3G is good:)\"]},{\"String\":\"BramHatch\",\"Enumerable\":[\" can't stand t-mobile its voicemail-service is OMG:(\",\" dislike iphone the voice-command is bad:(\"]},{\"String\":\"EmoryUnk\",\"Enumerable\":[\" love sprint its shortcut-menu is awesome:)\",\" love verizon its wireless is good\"]},{\"String\":\"VonKemble\",\"Enumerable\":[\" dislike sprint the speed is horrible\"]},{\"String\":\"SuzannaTillson\",\"Enumerable\":[\" like iphone the voicemail-service is awesome\" ] }]",
                QueryResult = from user in TinySocialData.FacebookUsers
                    select new StringAndEnumerableResult<string>
                    {
                        String = user.name,
                        Enumerable =
                            from message in TinySocialData.FacebookMessages
                            where message.AuthorId == user.id
                            orderby message.Id
                            select message.Message
                    }
            },
            new TestQuery("ThetaJoin4")
            {
                LinqQuery =
                    dv.TweetMessages.Where(t => t.tweetid == "1").Select(
                        t =>
                            new StringAndEnumerableResult<string>
                            {
                                String = t.MessageText,
                                Enumerable =
                                    dv.TweetMessages.Where(t2 => t.SenderLocation.Distance(t2.SenderLocation) <= 1)
                                        .Select(t2 => t2.MessageText)
                            }),
                Aql =
                    "for $t in dataset TweetMessages where ($t.tweetid = \"1\") return { \"String\": $t.message-text, \"Enumerable\": (for $t2 in dataset TweetMessages where (spatial-distance($t.sender-location, $t2.sender-location) <= 1) return $t2.message-text) }",
                CleanJsonApi =
                    "[{\"String\":\" love t-mobile its customization is good:)\",\"Enumerable\":[\" love t-mobile its customization is good:)\"]}]",
                QueryResult = TinySocialData.TweetMessages.Where(t => t.tweetid == "1").Select(
                    t =>
                        new StringAndEnumerableResult<string>
                        {
                            String = t.MessageText,
                            Enumerable = new[] {" love t-mobile its customization is good:)"}
                        })
            },
            new TestQuery("ExistentialQuantification6")
            {
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.Any(e => e.EndDate == null) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies is-null($e.end-date)) return $fbu",
                CleanJsonApi =
                    "[{\"id\":6,\"alias\":\"Willis\",\"name\":\"WillisWynne\",\"user-since\":\"2005-01-17T10:10:00.000Z\",\"friend-ids\":[1,3,7],\"employment\":[{\"organization-name\":\"jaydax\",\"start-date\":\"2009-05-15\",\"end-date\":null}]},{\"id\":1,\"alias\":\"Margarita\",\"name\":\"MargaritaStoddard\",\"user-since\":\"2012-08-20T10:10:00.000Z\",\"friend-ids\":[2,3,6,10],\"employment\":[{\"organization-name\":\"Codetechno\",\"start-date\":\"2006-08-06\",\"end-date\":null}]},{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":8,\"alias\":\"Nila\",\"name\":\"NilaMilliron\",\"user-since\":\"2008-01-01T10:10:00.000Z\",\"friend-ids\":[3],\"employment\":[{\"organization-name\":\"Plexlane\",\"start-date\":\"2010-02-28\",\"end-date\":null}]},{\"id\":5,\"alias\":\"Von\",\"name\":\"VonKemble\",\"user-since\":\"2010-01-05T10:10:00.000Z\",\"friend-ids\":[3,6,10],\"employment\":[{\"organization-name\":\"Kongreen\",\"start-date\":\"2010-11-27\",\"end-date\":null}]},{\"id\":7,\"alias\":\"Suzanna\",\"name\":\"SuzannaTillson\",\"user-since\":\"2012-08-07T10:10:00.000Z\",\"friend-ids\":[6],\"employment\":[{\"organization-name\":\"Labzatron\",\"start-date\":\"2011-04-19\",\"end-date\":null}]}]",
                QueryResult =
                    from fbu in TinySocialData.FacebookUsers where fbu.employment.Any(e => e.EndDate == null) select fbu
            },
            new TestQuery("ExistentialQuantification6DoubleNegation")
            {
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.Any(e => !e.EndDate.HasValue) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies not(not(is-null($e.end-date)))) return $fbu",
                CleanJsonApi =
                    "[{\"id\":6,\"alias\":\"Willis\",\"name\":\"WillisWynne\",\"user-since\":\"2005-01-17T10:10:00.000Z\",\"friend-ids\":[1,3,7],\"employment\":[{\"organization-name\":\"jaydax\",\"start-date\":\"2009-05-15\",\"end-date\":null}]},{\"id\":1,\"alias\":\"Margarita\",\"name\":\"MargaritaStoddard\",\"user-since\":\"2012-08-20T10:10:00.000Z\",\"friend-ids\":[2,3,6,10],\"employment\":[{\"organization-name\":\"Codetechno\",\"start-date\":\"2006-08-06\",\"end-date\":null}]},{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":8,\"alias\":\"Nila\",\"name\":\"NilaMilliron\",\"user-since\":\"2008-01-01T10:10:00.000Z\",\"friend-ids\":[3],\"employment\":[{\"organization-name\":\"Plexlane\",\"start-date\":\"2010-02-28\",\"end-date\":null}]},{\"id\":5,\"alias\":\"Von\",\"name\":\"VonKemble\",\"user-since\":\"2010-01-05T10:10:00.000Z\",\"friend-ids\":[3,6,10],\"employment\":[{\"organization-name\":\"Kongreen\",\"start-date\":\"2010-11-27\",\"end-date\":null}]},{\"id\":7,\"alias\":\"Suzanna\",\"name\":\"SuzannaTillson\",\"user-since\":\"2012-08-07T10:10:00.000Z\",\"friend-ids\":[6],\"employment\":[{\"organization-name\":\"Labzatron\",\"start-date\":\"2011-04-19\",\"end-date\":null}]}]",
                QueryResult =
                    from fbu in TinySocialData.FacebookUsers
                    where fbu.employment.Any(e => !e.EndDate.HasValue)
                    select fbu
            },
            new TestQuery("UniversalQuantification7DateNotNull")
            {
                Name = "UniversalQuantification7DateNotNull",
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.All(e => e.EndDate != null) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (every $e in $fbu.employment satisfies not(is-null($e.end-date))) return $fbu",
                CleanJsonApi =
                    "[{\"id\":9,\"alias\":\"Woodrow\",\"name\":\"WoodrowNehling\",\"user-since\":\"2005-09-20T10:10:00.000Z\",\"friend-ids\":[3,10],\"employment\":[{\"organization-name\":\"Zuncan\",\"start-date\":\"2003-04-22\",\"end-date\":\"2009-12-13\"}]},{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult =
                    from fbu in TinySocialData.FacebookUsers where fbu.employment.All(e => e.EndDate != null) select fbu
            },
            new TestQuery("UniversalQuantification7DateHasValue")
            {
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.All(e => e.EndDate.HasValue) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (every $e in $fbu.employment satisfies not(is-null($e.end-date))) return $fbu",
                CleanJsonApi =
                    "[{\"id\":9,\"alias\":\"Woodrow\",\"name\":\"WoodrowNehling\",\"user-since\":\"2005-09-20T10:10:00.000Z\",\"friend-ids\":[3,10],\"employment\":[{\"organization-name\":\"Zuncan\",\"start-date\":\"2003-04-22\",\"end-date\":\"2009-12-13\"}]},{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult =
                    from fbu in TinySocialData.FacebookUsers
                    where fbu.employment.All(e => e.EndDate.HasValue)
                    select fbu
            },
            //TODO: The ApiResponse is different than expected (Grouping is a known issue).
            new TestQuery("GroupingAndAggregation9A")
            {
                LinqQuery =
                    dv.TweetMessages.GroupBy(t => t.user.ScreenName)
                        .Select(uid => new {user = uid.Key, count = uid.Count()}),
                Aql =
                    "for $uid in (for $t in dataset TweetMessages group by $t.user.screen-name with $t return $t) return { \"user\": $uid[0].screen-name, \"count\": count((for $generated_uservar_1 in $uid return $generated_uservar_1)) }",
                CleanJsonApi = null,
                //"[{\"count\":{\"int64\":3},\"user\":null},{\"count\":{\"int64\":1},\"user\":null},{\"count\":{\"int64\":1},\"user\":null},{\"count\":{\"int64\":1},\"user\":null},{\"count\":{\"int64\":6},\"user\":null}]",
                QueryResult = null
            },
            //TODO: The ApiResponse is different than expected (Grouping is a known issue).
            new TestQuery("GroupingAndLimits10")
            {
                LinqQuery =
                    dv.TweetMessages.GroupBy(t => t.user.ScreenName)
                        .OrderByDescending(g => g.Count())
                        .Take(3)
                        .Select(uid => new {user = uid.Key, count = uid.Count()}),
                Aql =
                    "for $uid in (for $g in (for $t in dataset TweetMessages group by $t.user.screen-name with $t return $t) order by count((for $generated_uservar_uservar_1 in $g return $generated_uservar_uservar_1)) desc limit 3 return $g) return { \"user\": $uid[0].screen-name, \"count\": count((for $generated_uservar_1 in $uid return $generated_uservar_1)) }",
                CleanJsonApi = null,
                //"[{\"count\":{\"int64\":6},\"user\":null},{\"count\":{\"int64\":3},\"user\":null},{\"count\":{\"int64\":1},\"user\":null}]",
                QueryResult = null,
                EnforceOrder = true
            },
            new TestQuery("LeftOuterFuzzyJoin11")
            {
                LinqQuery =
                    dv.TweetMessages.Where(t => t.tweetid == "12")
                        .Select(
                            t =>
                                new
                                {
                                    tweet = t,
                                    similarTweetCount =
                                        dv.TweetMessages.Where(
                                            t2 =>
                                                t.ReferredTopics.Jaccard(t2.ReferredTopics) > 0.3 &&
                                                t.tweetid != t2.tweetid).Select(t2 => t2.ReferredTopics).Count()
                                }),
                Aql =
                    "for $t in dataset TweetMessages where ($t.tweetid = \"12\") return { \"tweet\": $t, \"similarTweetCount\": count((for $t2 in dataset TweetMessages where ((similarity-jaccard($t.referred-topics, $t2.referred-topics) > 0.3) and ($t.tweetid != $t2.tweetid)) return $t2.referred-topics)) }",
                CleanJsonApi =
                    "[{\"tweet\":{\"tweetid\":\"12\",\"user\":{\"screen-name\":\"OliJackson_512\",\"lang\":\"en\",\"friends_count\":445,\"statuses_count\":164,\"name\":\"Oli Jackson\",\"followers_count\":22649},\"sender-location\":[24.82,94.63],\"send-time\":\"2010-02-13T10:10:00.000Z\",\"referred-topics\":[\"samsung\",\"voice-command\"],\"message-text\":\" like samsung the voice-command is amazing:)\"},\"similarTweetCount\":2}]",
                QueryResult =
                    TinySocialData.TweetMessages.Where(t => t.tweetid == "12")
                        .Select(t => new {tweet = t, similarTweetCount = 2})
            }
        };

#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        private class StringAndEnumerableResult<T>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
        {
            private List<T> _evaluatedEnumerable;
            public string String { get; set; }

            public IEnumerable<T> Enumerable
            {
                get { return _evaluatedEnumerable; }
                set { _evaluatedEnumerable = value.ToList(); }
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                var other = obj as StringAndEnumerableResult<T>;
                if (other == null) return false;
                return String == other.String && Enumerable.SequenceEqual(other.Enumerable);
            }
        }
    }
}