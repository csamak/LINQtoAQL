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
                        .Select(t => new TweetWordTokens {tweetid = t.tweetid, wordTokens = t.MessageText.Split(' ')}),
                Aql =
                    $"for $t in dataset TweetMessages where ($t.send-time >= datetime('2012-01-01T00:00:00.000{new DateTime(2012, 1, 1).ToString("zzz")}')) return {{ \"tweetid\": $t.tweetid, \"wordTokens\": word-tokens($t.message-text) }}",
                CleanJsonApi =
                    "[{\"tweetid\":\"9\",\"wordTokens\":[\"love\",\"verizon\",\"its\",\"voicemail\",\"service\",\"is\",\"awesome\"]}]",
                QueryResult =
                    new[]
                    {
                        new TweetWordTokens
                        {
                            tweetid = "9",
                            wordTokens = new[] {"love", "verizon", "its", "voicemail", "service", "is", "awesome"}
                        }
                    }
            },
            new TestQuery("WordTokensExplicitStringOptionsNone")
            {
                LinqQuery =
                    dv.TweetMessages.Where(t => t.SendTime >= new DateTime(2012, 1, 1))
                        .Select(
                            t =>
                                new TweetWordTokens
                                {
                                    tweetid = t.tweetid,
                                    wordTokens = t.MessageText.Split(new[] {" "}, StringSplitOptions.None)
                                }),
                Aql =
                    $"for $t in dataset TweetMessages where ($t.send-time >= datetime('2012-01-01T00:00:00.000{new DateTime(2012, 1, 1).ToString("zzz")}')) return {{ \"tweetid\": $t.tweetid, \"wordTokens\": word-tokens($t.message-text) }}",
                CleanJsonApi =
                    "[{\"tweetid\":\"9\",\"wordTokens\":[\"love\",\"verizon\",\"its\",\"voicemail\",\"service\",\"is\",\"awesome\"]}]",
                QueryResult =
                    new[]
                    {
                        new TweetWordTokens
                        {
                            tweetid = "9",
                            wordTokens = new[] {"love", "verizon", "its", "voicemail", "service", "is", "awesome"}
                        }
                    }
            }
        };
    }

    public class TweetWordTokens
    {
        public string tweetid { get; set; }
        public IEnumerable<string> wordTokens { get; set; } = new List<string>();

        public override bool Equals(object obj)
        {
            var twt = obj as TweetWordTokens;
            if (twt == null) return false;
            return tweetid == twt.tweetid && wordTokens.SequenceEqual(twt.wordTokens);
        }

        public override string ToString()
        {
            return $"tweetid: {tweetid}, wordTokens: {string.Join(", ", wordTokens)}";
        }

        public override int GetHashCode()
        {
            return tweetid.GetHashCode() ^ wordTokens.GetHashCode();
        }
    }
}