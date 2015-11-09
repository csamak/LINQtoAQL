using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace LINQToAQL.Tests.Common.Queries
{
    public class TestQuery
    {
        public TestQuery(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public IQueryable<object> LinqQuery { get; set; }
        public string Aql { get; set; }
        public string CleanJsonApi { get; set; }
        public IEnumerable<object> QueryResult { get; set; }

        public TestCaseData QuerySynthesisTestData => new TestCaseData(LinqQuery).Returns(Aql).SetName(Name);

        public TestCaseData DeserializationTestData
            =>
                new TestCaseData(CleanJsonApi, QueryResultType(QueryResult)).Returns(QueryResult)
                    .SetName(Name);

        public TestCaseData EndToEndTestData => new TestCaseData(LinqQuery).Returns(QueryResult).SetName(Name);

        private static Type QueryResultType(IEnumerable queryResult)
        {
            if (queryResult == null) return null;
            var type = queryResult.GetType();
            return type.IsArray
                ? type.GetElementType()
                : type.GetInterfaces()
                    .Where(t => t.IsGenericType)
                    .SingleOrDefault(t => t.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                    .GetGenericArguments()[0];
        }
    }
}