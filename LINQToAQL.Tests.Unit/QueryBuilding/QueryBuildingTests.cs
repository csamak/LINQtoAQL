using System;
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.Tests.Common.Model;
using LINQToAQL.Tests.Common.Queries;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.QueryBuilding
{
    internal class QueryBuildingTests
    {
        [Test, TestCaseSource(typeof (QueryTestCases), nameof(QueryTestCases.QuerySynthesisTestCases))]
        public string TestCommonQueries(IQueryable<object> query)
        {
            return QueryBuildingBase.GetQueryString(query.Expression);
        }

        //Needs to use Expression directly, so cannot be included with the other QueryTestCases.
        [Test, Category("AdbTutorialQuerySet")]
        public void SimpleAggregation8()
        {
            Expression<Func<int>> query = () => new TinySocial(null).FacebookUsers.Count();
            Assert.AreEqual("count(for $generated_1 in dataset FacebookUsers return $generated_1)",
                QueryBuildingBase.GetQueryString(query.Body));
        }
    }
}