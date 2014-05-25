using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Test.Model;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Test.QueryBuilding
{
    class QueryBuildingBase
    {
        protected readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        protected static string GetQueryString(Expression exp)
        {
            return AqlQueryModelVisitor.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}
