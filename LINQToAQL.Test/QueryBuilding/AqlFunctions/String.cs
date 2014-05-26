﻿using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    internal class String : QueryBuildingBase
    {
        [Test]
        public void StringToCodepoint()
        {
            IQueryable<char[]> query = dv.FacebookUsers.Select(u => u.name.ToCharArray());
            Assert.AreEqual("for $u in dataset FacebookUsers return string-to-codepoint($u.name)",
                GetQueryString(query.Expression));
            IQueryable<char> indexer = dv.FacebookUsers.Select(u => u.name[0]);
            Assert.AreEqual("for $u in dataset FacebookUsers return string-to-codepoint($u.name)[0]",
                GetQueryString(indexer.Expression));
        }

        [Test]
        public void CodepointToString()
        {
            IQueryable<string> query = dv.FacebookUsers.Select(u => new string(u.name.ToCharArray()));
            Assert.AreEqual("for $u in dataset FacebookUsers return codepoint-to-string(string-to-codepoint($u.name))",
                GetQueryString(query.Expression));
        }

        [Test]
        public void Contains()
        {
            var query =
                dv.FacebookMessages.Where(i => i.Message.Contains("phone"))
                    .Select(i => new {mid = i.Id, message = i.Message});
            Assert.AreEqual(
                "for $i in dataset FacebookMessages where contains($i.message, \"phone\") return { \"mid\": $i.message-id, \"message\": $i.message }",
                GetQueryString(query.Expression));
        }

        //TODO: AQL like

        [Test]
        public void StartsWith()
        {
            IQueryable<string> query =
                dv.FacebookMessages.Where(i => i.Message.StartsWith(" like")).Select(i => i.Message);
            Assert.AreEqual(
                "for $i in dataset FacebookMessages where starts-with($i.message, \" like\") return $i.message"
                , GetQueryString(query.Expression));
        }

        [Test]
        public void EndsWith()
        {
            IQueryable<string> query = dv.FacebookMessages.Where(i => i.Message.EndsWith(":)")).Select(i => i.Message);
            Assert.AreEqual("for $i in dataset FacebookMessages where ends-with($i.message, \":)\") return $i.message",
                GetQueryString(query.Expression));
        }

        //TODO: string-concat

        [Test]
        public void StringJoin()
        {
            IQueryable<string> query =
                new AqlQueryable<StringJoinClass>(null, "").Select(m => string.Join(",", m.Messages));
            Assert.AreEqual("for $m in dataset StringJoinClass return string-join($m.Messages, \",\")",
                GetQueryString(query.Expression));
        }

        [Test]
        public void ToLower()
        {
            IQueryable<string> query = dv.FacebookUsers.Select(u => u.name.ToLower());
            Assert.AreEqual("for $u in dataset FacebookUsers return lowercase($u.name)",
                GetQueryString(query.Expression));
        }

        //TODO: matches
        //TODO: replace

        [Test]
        public void StringLength()
        {
            var query = dv.FacebookMessages.Select(i => new {mid = i.Id, mlen = i.Message.Length});
            Assert.AreEqual(
                "for $i in dataset FacebookMessages return { \"mid\": $i.message-id, \"mlen\": string-length($i.message) }",
                GetQueryString(query.Expression));
        }

        [Test]
        public void Substring()
        {
            IQueryable<string> query = dv.FacebookMessages.Select(i => i.Message.Substring(50));
            Assert.AreEqual("for $i in dataset FacebookMessages return substring($i.message, 50)",
                GetQueryString(query.Expression));
            query = dv.FacebookMessages.Select(i => i.Message.Substring(50, 10));
            Assert.AreEqual("for $i in dataset FacebookMessages return substring($i.message, 50, 10)",
                GetQueryString(query.Expression));
        }

        private class StringJoinClass
        {
            public string[] Messages { get; set; }
        }

        //TODO: substring-before
        //TODO: substring-after
    }
}