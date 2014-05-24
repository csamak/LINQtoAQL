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

namespace LINQToAQL.Test.QueryBuilding
{
    internal class Grouping
    {
        private readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        [Test]
        public void SingleReturnKey()
        {
            var query = dv.FacebookUsers.GroupBy(u => u.id).Select(g => g.Key);
            Assert.AreEqual("for $u in dataset FacebookUsers group by $g := $u.id with $u return $g",
                GetQueryString(query.Expression));
        }

        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}
