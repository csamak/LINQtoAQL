using System;
using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Test.Model;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Test.QueryBuilding
{
    internal class QueryBuildingBase
    {
        protected readonly TinySocial dv = new TinySocial(new Uri("http://33.0.0.2:19002"));

        protected static string GetQueryString(Expression exp)
        {
            return AqlQueryGenerator.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}