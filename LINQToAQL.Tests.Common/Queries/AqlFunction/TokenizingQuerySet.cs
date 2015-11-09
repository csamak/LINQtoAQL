using System;
using System.Collections.Generic;
using System.Linq;

namespace LINQToAQL.Tests.Common.Queries.AqlFunction
{
    internal class TokenizingQuerySet : QuerySet
    {
        public override IEnumerable<TestQuery> Queries => new[]
        {
            new TestQuery("WordTokens")
            {
                LinqQuery =
                    dv.TweetMessages.Where(t => t.SendTime >= new DateTime(2012, 1, 1))
                        .Select(t => new {t.tweetid, wordTokens = t.MessageText.Split(' ')}),
                Aql =
                    "for $t in dataset TweetMessages where ($t.send-time >= datetime('2012-01-01T00:00:00.0000000')) return { \"tweetid\": $t.tweetid, \"wordTokens\": word-tokens($t.message-text) }",
                ApiResponse =
                    "[{\"tweetid\":\"9\",\"wordTokens\":[\"love\",\"verizon\",\"its\",\"voicemail\",\"service\",\"is\",\"awesome\"]}]",
                QueryResult =
                    new[]
                    {
                        new
                        {
                            tweetid = 10,
                            wordTokens = new[] {"love", "verizon", "its", "voicemail", "service", "is", "awesome"}
                        }
                    }
            },
            new TestQuery("WordTokensExplicitStringOptionsNone")
            {
                LinqQuery =
                    dv.TweetMessages.Where(t => t.SendTime >= new DateTime(2012, 1, 1))
                        .Select(
                            t => new {t.tweetid, wordTokens = t.MessageText.Split(new[] {" "}, StringSplitOptions.None)}),
                Aql =
                    "for $t in dataset TweetMessages where ($t.send-time >= datetime('2012-01-01T00:00:00.0000000')) return { \"tweetid\": $t.tweetid, \"wordTokens\": word-tokens($t.message-text) }",
                ApiResponse =
                    "[{\"tweetid\":\"9\",\"wordTokens\":[\"love\",\"verizon\",\"its\",\"voicemail\",\"service\",\"is\",\"awesome\"]}]",
                QueryResult =
                    new[]
                    {
                        new
                        {
                            tweetid = 10,
                            wordTokens = new[] {"love", "verizon", "its", "voicemail", "service", "is", "awesome"}
                        }
                    }
            }
        };
    }
}