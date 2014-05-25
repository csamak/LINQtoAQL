using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Test.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Test.QueryBuilding.AqlFunctions
{
    class String
    {
        private readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        [Test]
        public void StringToCodepoint()
        {
            var query = dv.FacebookUsers.Select(u => u.name.ToCharArray());
            Assert.AreEqual("for $u in dataset FacebookUsers return string-to-codepoint($u.name)",
                GetQueryString(query.Expression));
            var indexer = dv.FacebookUsers.Select(u => u.name[0]);
            Assert.AreEqual("for $u in dataset FacebookUsers return string-to-codepoint($u.name)[0]",
                GetQueryString(indexer.Expression));
        }

        [Test]


        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}
