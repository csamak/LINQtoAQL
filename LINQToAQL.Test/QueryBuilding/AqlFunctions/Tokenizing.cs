using System;
using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    internal class Tokenizing : QueryBuildingBase
    {
        [Test]
        public void WordTokens()
        {
            var query =
                dv.TweetMessages.Where(t => t.SendTime >= new DateTime(2012, 1, 1))
                    .Select(t => new {t.tweetid, wordTokens = t.MessageText.Split(' ')});
            Assert.AreEqual(
                "for $t in dataset TweetMessages where ($t.send-time >= datetime('2012-01-01T00:00:00.0000000')) return { \"tweetid\": $t.tweetid, \"wordTokens\": word-tokens($t.message-text) }",
                GetQueryString(query.Expression));
        }

        [Test]
        public void WordTokensRemoveEmptyEntries()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new [] {' '}, StringSplitOptions.RemoveEmptyEntries));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
        }

        [Test]
        public void WordTokensString()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new [] {" "}, StringSplitOptions.None));
            Assert.AreEqual(
                "for $u in dataset FacebookUsers return word-tokens($u.name)",
                GetQueryString(query.Expression));
        }

        [Test]
        public void WordTokensWrongSeparator()
        {
            var query = dv.FacebookUsers.Select(u => u.name.Split(new [] {"wrong"}, StringSplitOptions.None));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
            query = dv.FacebookUsers.Select(u => u.name.Split('x'));
            Assert.Throws<NotSupportedException>(() => GetQueryString(query.Expression));
        }
    }
}