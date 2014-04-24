using System.Linq;
using LINQToAQL;
using NUnit.Framework;
using Remotion.Linq.Parsing.Structure;

namespace Remotion.Linq.Sample.Tests
{
    [TestFixture]
    public class RelinqSampleTests
    {
        private SampleQueryable<SampleDataSourceItem> items;

        [SetUp]
        public void SetUp()
        {
            var queryParser = QueryParser.CreateDefault();

            // Create our IQueryable instance
            items = new SampleQueryable<SampleDataSourceItem>(queryParser, new SampleQueryExecutor());
        }

        [Test]
        public void Test()
        {
            var results = from i in items select i;

            // force evalution of the statement to prevent assertion from re-evaluating the query.
            var list = results.ToList();

            Assert.That(list.Count, Is.EqualTo(10));
            Assert.That(list[3].Name, Is.EqualTo("Name 3"));
        }
    }
}