using System;
using System.Collections.Generic;
using System.Linq;
using LINQToAQL.Tests.Common.Queries.AqlFunction;
using NUnit.Framework;

namespace LINQToAQL.Tests.Common.Queries
{
    public class QueryTestCases
    {
        private readonly List<IEnumerable<Tuple<Type, TestQuery>>> _testQueries =
            new List<IEnumerable<Tuple<Type, TestQuery>>>();

        public QueryTestCases()
        {
            RegisterQuerySets(new AdbTutorialQuerySet(), new NumericQuerySet(), new TokenizingQuerySet(),
                new StringQuerySet(), new SimilarityQuerySet(), new SpatialQuerySet());
        }

        public IEnumerable<TestCaseData> QuerySynthesisTestCases
        {
            get
            {
                foreach (
                    var query in
                        _testQueries.SelectMany(x => x)
                            .Select(t => t.Item2.QuerySynthesisTestData.SetCategory(t.Item1.Name)))
                {
                    if (query.Arguments.All(a => a == null) || query.Result == null)
                        query.Ignore("Query test case not complete");
                    yield return query;
                }
            }
        }

        public IEnumerable<TestCaseData> DeserializationTestCases
        {
            get
            {
                foreach (
                    var query in
                        _testQueries.SelectMany(x => x)
                            .Select(t => t.Item2.DeserializationTestData.SetCategory(t.Item1.Name)))
                {
                    if (query.Arguments.All(a => a == null) || query.Result == null)
                        query.Ignore("Query test case not complete");
                    yield return query;
                }
            }
        }

        public IEnumerable<TestCaseData> EndToEndTestCases
        {
            get
            {
                foreach (
                    var query in
                        _testQueries.SelectMany(x => x).Select(t => t.Item2.EndToEndTestData.SetCategory(t.Item1.Name)))
                {
                    if (query.Arguments.All(a => a == null) || query.Result == null)
                        query.Ignore("Query test case not complete");
                    yield return query;
                }
            }
        }

        public void RegisterQuerySets(params QuerySet[] querySets)
        {
            foreach (var querySet in querySets)
                _testQueries.Add(querySet.Queries.Select(t => Tuple.Create(querySet.GetType(), t)));
        }
    }
}