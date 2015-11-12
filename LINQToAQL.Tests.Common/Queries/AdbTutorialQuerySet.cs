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
                QueryResult = TinySocialData.FacebookUsersByIds(8)
            },
            new TestQuery("RangeScan0B")
            {
                LinqQuery = from user in dv.FacebookUsers where user.id >= 2 && user.id <= 4 select user,
                Aql =
                    "for $user in dataset FacebookUsers where (($user.id >= 2) and ($user.id <= 4)) return $user",
                CleanJsonApi =
                    "[{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult = TinySocialData.FacebookUsersByIds(2, 4, 3)
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
                QueryResult = TinySocialData.FacebookUsersByIds(2, 4, 10, 3)
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
                QueryResult = null
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
                QueryResult = null
            },
            new TestQuery("NestedOuterJoin3")
            {
                LinqQuery = from user in dv.FacebookUsers
                    select
                        new
                        {
                            uname = user.name,
                            messages =
                                from message in dv.FacebookMessages
                                where message.AuthorId == user.id
                                select message.Message
                        },
                Aql =
                    "for $user in dataset FacebookUsers return { \"uname\": $user.name, \"messages\": (for $message in dataset FacebookMessages where ($message.author-id = $user.id) return $message.message) }",
                CleanJsonApi =
                    "[{\"uname\":\"WillisWynne\",\"messages\":{\"orderedlist\":[\" love sprint the customization is mind-blowing\"]}},{\"uname\":\"MargaritaStoddard\",\"messages\":{\"orderedlist\":[\" dislike iphone its touch-screen is horrible\",\" can't stand at&t the network is horrible:(\",\" like verizon the 3G is awesome:)\",\" can't stand motorola the touch-screen is terrible\",\" can't stand at&t its plan is terrible\"]}},{\"uname\":\"IsbelDull\",\"messages\":{\"orderedlist\":[\" like t-mobile its platform is mind-blowing\",\" like samsung the plan is amazing\"]}},{\"uname\":\"NicholasStroh\",\"messages\":{\"orderedlist\":[]}},{\"uname\":\"NilaMilliron\",\"messages\":{\"orderedlist\":[]}},{\"uname\":\"WoodrowNehling\",\"messages\":{\"orderedlist\":[\" love at&t its 3G is good:)\"]}},{\"uname\":\"BramHatch\",\"messages\":{\"orderedlist\":[\" dislike iphone the voice-command is bad:(\",\" can't stand t-mobile its voicemail-service is OMG:(\"]}},{\"uname\":\"EmoryUnk\",\"messages\":{\"orderedlist\":[\" love sprint its shortcut-menu is awesome:)\",\" love verizon its wireless is good\"]}},{\"uname\":\"VonKemble\",\"messages\":{\"orderedlist\":[\" dislike sprint the speed is horrible\"]}},{\"uname\":\"SuzannaTillson\",\"messages\":{\"orderedlist\":[\" like iphone the voicemail-service is awesome\"]}}]",
                QueryResult = null
            },
            new TestQuery("ThetaJoin4")
            {
                LinqQuery =
                    dv.TweetMessages.Select(
                        t =>
                            new
                            {
                                message = t.MessageText,
                                nearbyMessages =
                                    dv.TweetMessages.Where(t2 => t.SenderLocation.Distance(t2.SenderLocation) <= 1)
                                        .Select(t2 => new {msgtxt = t2.MessageText})
                            }),
                Aql =
                    "for $t in dataset TweetMessages return { \"message\": $t.message-text, \"nearbyMessages\": (for $t2 in dataset TweetMessages where (spatial-distance($t.sender-location, $t2.sender-location) <= 1) return { \"msgtxt\": $t2.message-text }) }",
                CleanJsonApi =
                    "[{\"message\":\" hate verizon its voice-clarity is OMG:(\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" hate verizon its voice-clarity is OMG:(\"},{\"msgtxt\":\" like motorola the speed is good:)\"}]}},{\"message\":\" like iphone the voice-clarity is good:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" like iphone the voice-clarity is good:)\"}]}},{\"message\":\" like samsung the platform is good\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" like samsung the platform is good\"}]}},{\"message\":\" love t-mobile its customization is good:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" love t-mobile its customization is good:)\"}]}},{\"message\":\" like samsung the voice-command is amazing:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" like samsung the voice-command is amazing:)\"}]}},{\"message\":\" like motorola the speed is good:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" hate verizon its voice-clarity is OMG:(\"},{\"msgtxt\":\" like motorola the speed is good:)\"}]}},{\"message\":\" love verizon its voicemail-service is awesome\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" love verizon its voicemail-service is awesome\"}]}},{\"message\":\" can't stand motorola its speed is terrible:(\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" can't stand motorola its speed is terrible:(\"}]}},{\"message\":\" like t-mobile the shortcut-menu is awesome:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" like t-mobile the shortcut-menu is awesome:)\"}]}},{\"message\":\" can't stand iphone its platform is terrible\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" can't stand iphone its platform is terrible\"}]}},{\"message\":\" like verizon its shortcut-menu is awesome:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" like verizon its shortcut-menu is awesome:)\"}]}},{\"message\":\" like sprint the voice-command is mind-blowing:)\",\"nearbyMessages\":{\"orderedlist\":[{\"msgtxt\":\" like sprint the voice-command is mind-blowing:)\"}]}}]",
                QueryResult = null
            },
            //TODO: fox edit-distance-check. It does not return a boolean.
            //new TestQuery
            //{
            //    Name = "FuzzyJoin5",
            //    LinqQuery = dv.FacebookUsers.Select(fbu => new {id = fbu.id, name = fbu.name, similarUsers = dv.TweetMessages.Where(t => t.user.name.EditDistanceCheck(fbu.name, 3)).Select(t => new {twitterScreenname = t.user.ScreenName, twitterName = t.user.name})}),
            //    Aql = "for $fbu in dataset FacebookUsers return { \"id\": $fbu.id, \"name\": $fbu.name, \"similarUsers\": (for $t in dataset TweetMessages where edit-distance-check($t.user.name, $fbu.name, 3) return { \"twitterScreenname\": $t.user.screen-name, \"twitterName\": $t.user.name }) }",
            //    ApiResponse = "",
            //    QueryResult = null
            //}
            new TestQuery("ExistentialQuantification6")
            {
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.Any(e => e.EndDate == null) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies is-null($e.end-date)) return $fbu",
                CleanJsonApi =
                    "[{\"id\":6,\"alias\":\"Willis\",\"name\":\"WillisWynne\",\"user-since\":\"2005-01-17T10:10:00.000Z\",\"friend-ids\":[1,3,7],\"employment\":[{\"organization-name\":\"jaydax\",\"start-date\":\"2009-05-15\",\"end-date\":null}]},{\"id\":1,\"alias\":\"Margarita\",\"name\":\"MargaritaStoddard\",\"user-since\":\"2012-08-20T10:10:00.000Z\",\"friend-ids\":[2,3,6,10],\"employment\":[{\"organization-name\":\"Codetechno\",\"start-date\":\"2006-08-06\",\"end-date\":null}]},{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":8,\"alias\":\"Nila\",\"name\":\"NilaMilliron\",\"user-since\":\"2008-01-01T10:10:00.000Z\",\"friend-ids\":[3],\"employment\":[{\"organization-name\":\"Plexlane\",\"start-date\":\"2010-02-28\",\"end-date\":null}]},{\"id\":5,\"alias\":\"Von\",\"name\":\"VonKemble\",\"user-since\":\"2010-01-05T10:10:00.000Z\",\"friend-ids\":[3,6,10],\"employment\":[{\"organization-name\":\"Kongreen\",\"start-date\":\"2010-11-27\",\"end-date\":null}]},{\"id\":7,\"alias\":\"Suzanna\",\"name\":\"SuzannaTillson\",\"user-since\":\"2012-08-07T10:10:00.000Z\",\"friend-ids\":[6],\"employment\":[{\"organization-name\":\"Labzatron\",\"start-date\":\"2011-04-19\",\"end-date\":null}]}]",
                QueryResult = TinySocialData.FacebookUsersByIds(6, 1, 2, 4, 8, 5, 7)
            },
            new TestQuery("ExistentialQuantification6DoubleNegation")
            {
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.Any(e => !e.EndDate.HasValue) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (some $e in $fbu.employment satisfies not(not(is-null($e.end-date)))) return $fbu",
                CleanJsonApi =
                    "[{\"id\":6,\"alias\":\"Willis\",\"name\":\"WillisWynne\",\"user-since\":\"2005-01-17T10:10:00.000Z\",\"friend-ids\":[1,3,7],\"employment\":[{\"organization-name\":\"jaydax\",\"start-date\":\"2009-05-15\",\"end-date\":null}]},{\"id\":1,\"alias\":\"Margarita\",\"name\":\"MargaritaStoddard\",\"user-since\":\"2012-08-20T10:10:00.000Z\",\"friend-ids\":[2,3,6,10],\"employment\":[{\"organization-name\":\"Codetechno\",\"start-date\":\"2006-08-06\",\"end-date\":null}]},{\"id\":2,\"alias\":\"Isbel\",\"name\":\"IsbelDull\",\"user-since\":\"2011-01-22T10:10:00.000Z\",\"friend-ids\":[1,4],\"employment\":[{\"organization-name\":\"Hexviafind\",\"start-date\":\"2010-04-27\",\"end-date\":null}]},{\"id\":4,\"alias\":\"Nicholas\",\"name\":\"NicholasStroh\",\"user-since\":\"2010-12-27T10:10:00.000Z\",\"friend-ids\":[2],\"employment\":[{\"organization-name\":\"Zamcorporation\",\"start-date\":\"2010-06-08\",\"end-date\":null}]},{\"id\":8,\"alias\":\"Nila\",\"name\":\"NilaMilliron\",\"user-since\":\"2008-01-01T10:10:00.000Z\",\"friend-ids\":[3],\"employment\":[{\"organization-name\":\"Plexlane\",\"start-date\":\"2010-02-28\",\"end-date\":null}]},{\"id\":5,\"alias\":\"Von\",\"name\":\"VonKemble\",\"user-since\":\"2010-01-05T10:10:00.000Z\",\"friend-ids\":[3,6,10],\"employment\":[{\"organization-name\":\"Kongreen\",\"start-date\":\"2010-11-27\",\"end-date\":null}]},{\"id\":7,\"alias\":\"Suzanna\",\"name\":\"SuzannaTillson\",\"user-since\":\"2012-08-07T10:10:00.000Z\",\"friend-ids\":[6],\"employment\":[{\"organization-name\":\"Labzatron\",\"start-date\":\"2011-04-19\",\"end-date\":null}]}]",
                QueryResult = TinySocialData.FacebookUsersByIds(6, 1, 2, 4, 8, 5, 7)
            },
            new TestQuery("UniversalQuantification7DateNotNull")
            {
                Name = "UniversalQuantification7DateNotNull",
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.All(e => e.EndDate != null) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (every $e in $fbu.employment satisfies not(is-null($e.end-date))) return $fbu",
                CleanJsonApi =
                    "[{\"id\":9,\"alias\":\"Woodrow\",\"name\":\"WoodrowNehling\",\"user-since\":\"2005-09-20T10:10:00.000Z\",\"friend-ids\":[3,10],\"employment\":[{\"organization-name\":\"Zuncan\",\"start-date\":\"2003-04-22\",\"end-date\":\"2009-12-13\"}]},{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult = TinySocialData.FacebookUsersByIds(9, 10, 3)
            },
            new TestQuery("UniversalQuantification7DateHasValue")
            {
                LinqQuery = from fbu in dv.FacebookUsers where fbu.employment.All(e => e.EndDate.HasValue) select fbu,
                Aql =
                    "for $fbu in dataset FacebookUsers where (every $e in $fbu.employment satisfies not(is-null($e.end-date))) return $fbu",
                CleanJsonApi =
                    "[{\"id\":9,\"alias\":\"Woodrow\",\"name\":\"WoodrowNehling\",\"user-since\":\"2005-09-20T10:10:00.000Z\",\"friend-ids\":[3,10],\"employment\":[{\"organization-name\":\"Zuncan\",\"start-date\":\"2003-04-22\",\"end-date\":\"2009-12-13\"}]},{\"id\":10,\"alias\":\"Bram\",\"name\":\"BramHatch\",\"user-since\":\"2010-10-16T10:10:00.000Z\",\"friend-ids\":[1,5,9],\"employment\":[{\"organization-name\":\"physcane\",\"start-date\":\"2007-06-05\",\"end-date\":\"2011-11-05\"}]},{\"id\":3,\"alias\":\"Emory\",\"name\":\"EmoryUnk\",\"user-since\":\"2012-07-10T10:10:00.000Z\",\"friend-ids\":[1,5,8,9],\"employment\":[{\"organization-name\":\"geomedia\",\"start-date\":\"2010-06-17\",\"end-date\":\"2010-01-26\"}]}]",
                QueryResult = TinySocialData.FacebookUsersByIds(9, 10, 3)
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
                QueryResult = null
            },
            new TestQuery("LeftOuterFuzzyJoin11")
            {
                LinqQuery =
                    dv.TweetMessages.Select(
                        t =>
                            new
                            {
                                tweet = t,
                                similarTweets =
                                    dv.TweetMessages.Where(
                                        t2 =>
                                            (bool) t.ReferredTopics.JaccardCheck(t2.ReferredTopics, 0.3)[0] &&
                                            t.tweetid != t2.tweetid).Select(t2 => t2.ReferredTopics)
                            }),
                Aql =
                    "for $t in dataset TweetMessages return { \"tweet\": $t, \"similarTweets\": (for $t2 in dataset TweetMessages where (similarity-jaccard-check($t.referred-topics, $t2.referred-topics, 0.3)[0] and ($t.tweetid != $t2.tweetid)) return $t2.referred-topics) }",
                CleanJsonApi =
                    "[{\"tweet\":{\"tweetid\":\"10\",\"user\":{\"screen-name\":\"ColineGeyer@63\",\"lang\":\"en\",\"friends_count\":{\"int32\":121},\"statuses_count\":{\"int32\":362},\"name\":\"Coline Geyer\",\"followers_count\":{\"int32\":17159}},\"sender-location\":{\"point\":[29.15,76.53]},\"send-time\":{\"datetime\":1201342200000},\"referred-topics\":{\"unorderedlist\":[\"verizon\",\"voice-clarity\"]},\"message-text\":\" hate verizon its voice-clarity is OMG:(\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"6\",\"user\":{\"screen-name\":\"ColineGeyer@63\",\"lang\":\"en\",\"friends_count\":{\"int32\":121},\"statuses_count\":{\"int32\":362},\"name\":\"Coline Geyer\",\"followers_count\":{\"int32\":17159}},\"sender-location\":{\"point\":[47.51,83.99]},\"send-time\":{\"datetime\":1273227000000},\"referred-topics\":{\"unorderedlist\":[\"iphone\",\"voice-clarity\"]},\"message-text\":\" like iphone the voice-clarity is good:)\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"7\",\"user\":{\"screen-name\":\"ChangEwing_573\",\"lang\":\"en\",\"friends_count\":{\"int32\":182},\"statuses_count\":{\"int32\":394},\"name\":\"Chang Ewing\",\"followers_count\":{\"int32\":32136}},\"sender-location\":{\"point\":[36.21,72.6]},\"send-time\":{\"datetime\":1314267000000},\"referred-topics\":{\"unorderedlist\":[\"samsung\",\"platform\"]},\"message-text\":\" like samsung the platform is good\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"1\",\"user\":{\"screen-name\":\"NathanGiesen@211\",\"lang\":\"en\",\"friends_count\":{\"int32\":39339},\"statuses_count\":{\"int32\":473},\"name\":\"Nathan Giesen\",\"followers_count\":{\"int32\":49416}},\"sender-location\":{\"point\":[47.44,80.65]},\"send-time\":{\"datetime\":1209204600000},\"referred-topics\":{\"unorderedlist\":[\"t-mobile\",\"customization\"]},\"message-text\":\" love t-mobile its customization is good:)\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"12\",\"user\":{\"screen-name\":\"OliJackson_512\",\"lang\":\"en\",\"friends_count\":{\"int32\":445},\"statuses_count\":{\"int32\":164},\"name\":\"Oli Jackson\",\"followers_count\":{\"int32\":22649}},\"sender-location\":{\"point\":[24.82,94.63]},\"send-time\":{\"datetime\":1266055800000},\"referred-topics\":{\"unorderedlist\":[\"samsung\",\"voice-command\"]},\"message-text\":\" like samsung the voice-command is amazing:)\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"3\",\"user\":{\"screen-name\":\"NathanGiesen@211\",\"lang\":\"en\",\"friends_count\":{\"int32\":39339},\"statuses_count\":{\"int32\":473},\"name\":\"Nathan Giesen\",\"followers_count\":{\"int32\":49416}},\"sender-location\":{\"point\":[29.72,75.8]},\"send-time\":{\"datetime\":1162635000000},\"referred-topics\":{\"unorderedlist\":[\"motorola\",\"speed\"]},\"message-text\":\" like motorola the speed is good:)\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"9\",\"user\":{\"screen-name\":\"NathanGiesen@211\",\"lang\":\"en\",\"friends_count\":{\"int32\":39339},\"statuses_count\":{\"int32\":473},\"name\":\"Nathan Giesen\",\"followers_count\":{\"int32\":49416}},\"sender-location\":{\"point\":[36.86,74.62]},\"send-time\":{\"datetime\":1342865400000},\"referred-topics\":{\"unorderedlist\":[\"verizon\",\"voicemail-service\"]},\"message-text\":\" love verizon its voicemail-service is awesome\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"5\",\"user\":{\"screen-name\":\"NathanGiesen@211\",\"lang\":\"en\",\"friends_count\":{\"int32\":39339},\"statuses_count\":{\"int32\":473},\"name\":\"Nathan Giesen\",\"followers_count\":{\"int32\":49416}},\"sender-location\":{\"point\":[40.09,92.69]},\"send-time\":{\"datetime\":1154686200000},\"referred-topics\":{\"unorderedlist\":[\"motorola\",\"speed\"]},\"message-text\":\" can't stand motorola its speed is terrible:(\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"8\",\"user\":{\"screen-name\":\"NathanGiesen@211\",\"lang\":\"en\",\"friends_count\":{\"int32\":39339},\"statuses_count\":{\"int32\":473},\"name\":\"Nathan Giesen\",\"followers_count\":{\"int32\":49416}},\"sender-location\":{\"point\":[46.05,93.34]},\"send-time\":{\"datetime\":1129284600000},\"referred-topics\":{\"unorderedlist\":[\"t-mobile\",\"shortcut-menu\"]},\"message-text\":\" like t-mobile the shortcut-menu is awesome:)\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"11\",\"user\":{\"screen-name\":\"NilaMilliron_tw\",\"lang\":\"en\",\"friends_count\":{\"int32\":445},\"statuses_count\":{\"int32\":164},\"name\":\"Nila Milliron\",\"followers_count\":{\"int32\":22649}},\"sender-location\":{\"point\":[37.59,68.42]},\"send-time\":{\"datetime\":1205057400000},\"referred-topics\":{\"unorderedlist\":[\"iphone\",\"platform\"]},\"message-text\":\" can't stand iphone its platform is terrible\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"2\",\"user\":{\"screen-name\":\"ColineGeyer@63\",\"lang\":\"en\",\"friends_count\":{\"int32\":121},\"statuses_count\":{\"int32\":362},\"name\":\"Coline Geyer\",\"followers_count\":{\"int32\":17159}},\"sender-location\":{\"point\":[32.84,67.14]},\"send-time\":{\"datetime\":1273745400000},\"referred-topics\":{\"unorderedlist\":[\"verizon\",\"shortcut-menu\"]},\"message-text\":\" like verizon its shortcut-menu is awesome:)\"},\"similarTweets\":{\"orderedlist\":[]}},{\"tweet\":{\"tweetid\":\"4\",\"user\":{\"screen-name\":\"NathanGiesen@211\",\"lang\":\"en\",\"friends_count\":{\"int32\":39339},\"statuses_count\":{\"int32\":473},\"name\":\"Nathan Giesen\",\"followers_count\":{\"int32\":49416}},\"sender-location\":{\"point\":[39.28,70.48]},\"send-time\":{\"datetime\":1324894200000},\"referred-topics\":{\"unorderedlist\":[\"sprint\",\"voice-command\"]},\"message-text\":\" like sprint the voice-command is mind-blowing:)\"},\"similarTweets\":{\"orderedlist\":[]}}]",
                QueryResult = null
            }
        };
    }
}