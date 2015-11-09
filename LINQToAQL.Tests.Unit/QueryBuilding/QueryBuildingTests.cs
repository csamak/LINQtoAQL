using System;
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.QueryBuilding;
using LINQToAQL.Tests.Common.Model;
using LINQToAQL.Tests.Common.Queries;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL.Tests.Unit.QueryBuilding
{
    internal class QueryBuildingTests
    {
        private static string GetQueryString(Expression exp)
        {
            return AqlQueryGenerator.GenerateAqlQuery(QueryParser.CreateDefault().GetParsedQuery(exp));
        }

        [Test, TestCaseSource(typeof (QueryTestCases), nameof(QueryTestCases.QuerySynthesisTestCases))]
        public string TestCommonQueries(IQueryable<object> query)
        {
            return GetQueryString(query.Expression);
        }

        //Needs to use Expression directly, so cannot be included with the other QueryTestCases.
        [Test, Category("AdbTutorialQuerySet")]
        public void SimpleAggregation8()
        {
            Expression<Func<int>> query = () => new TinySocial(null).FacebookUsers.Count();
            Assert.AreEqual("count(for $generated_1 in dataset FacebookUsers return $generated_1)",
                GetQueryString(query.Body));
        }
    }
}