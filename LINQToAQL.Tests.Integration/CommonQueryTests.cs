using System.Linq;
using LINQToAQL.Tests.Common.Queries;
using NUnit.Framework;

namespace LINQToAQL.Tests.Integration
{
    internal class CommonQueryTests
    {
        [Test, TestCaseSource(typeof (QueryTestCases), nameof(QueryTestCases.EndToEndTestCases))]
        public object TestCommonQueries(IQueryable<object> linqQuery)
        {
            return linqQuery.ToList(); //force remote evaluation
        }
    }
}
