using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Tests.Common;
using LINQToAQL.Tests.Common.Model;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Tests.Unit.QueryBuilding
{
    internal abstract class QueryBuildingBase
    {
        protected readonly TinySocial dv = TestEnvironment.Dataverse;

        public static string GetQueryString(Expression exp)
        {
            return AqlQueryGenerator.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }
    }
}