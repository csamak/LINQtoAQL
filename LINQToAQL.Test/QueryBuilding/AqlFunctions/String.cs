using System;
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Test.Model;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    internal class String
    {
        private readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

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

        //AQL like

        [Test]
        public void StartsWith()
        {
            var query = dv.FacebookMessages.Where(i => i.Message.StartsWith(" like")).Select(i => i.Message);
            Assert.AreEqual("for $i in dataset FacebookMessages where starts-with($i.message, \" like\") return $i.message"
                ,GetQueryString(query.Expression));
        }

        [Test]
        public void EndsWith()
        {
            var query = dv.FacebookMessages.Where(i => i.Message.EndsWith(":)")).Select(i => i.Message);
            Assert.AreEqual("for $i in dataset FacebookMessages where ends-with($i.message, \":)\") return $i.message",
                GetQueryString(query.Expression));
        }

        //string-concat

        private class StringJoinClass
        {
            public string[] Messages { get; set; }
        }

        [Test]
        public void StringJoin()
        {
            var query = new AqlQueryable<StringJoinClass>(null, "").Select(m => string.Join(",", m.Messages));
            Assert.AreEqual("for $m in dataset StringJoinClass return string-join($m.Messages, \",\")",
                GetQueryString(query.Expression));
        }

        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}