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
    class Numeric
    {
        private readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        [Test]
        public void NumericAbs()
        {
            //numeric-abs
            var query = dv.FacebookUsers.Where(u => Math.Abs(u.id) > 32);
            Assert.AreEqual("for $u in dataset FacebookUsers where (numeric-abs($u.id) > 32) return $u",
                GetQueryString(query.Expression));
        }
        private static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}
