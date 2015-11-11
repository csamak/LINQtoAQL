using System.Linq;
using LINQToAQL.Tests.Common;
using LINQToAQL.Tests.Common.Model;
using LINQToAQL.Tests.Common.Queries;
using NUnit.Framework;

namespace LINQToAQL.Tests.Integration
{
    internal class CommonQueryTests
    {
        private readonly TinySocial _dv = TestEnvironment.Dataverse;

        [Test, TestCaseSource(typeof (QueryTestCases), nameof(QueryTestCases.EndToEndTestCases))]
        public object TestCommonQueries(IQueryable<object> linqQuery)
        {
            return linqQuery.ToList(); //force remote evaluation
        }

        [Test]
        public void CountScalar()
        {
            //TODO: support LongCount()
            Assert.AreEqual(_dv.FacebookUsers.Count(), 10);
        }
    }
}
