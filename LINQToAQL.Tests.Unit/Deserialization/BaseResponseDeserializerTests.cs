using System;
using System.IO;
using System.Linq;
using LINQToAQL.Deserialization;
using LINQToAQL.Tests.Common.Queries;
using NUnit.Framework;

namespace LINQToAQL.Tests.Unit.Deserialization
{
    internal abstract class BaseResponseDeserializerTests
    {
        protected abstract IResponseDeserializer Deserializer { get; }
        
        [Test, TestCaseSource(typeof(QueryTestCases), nameof(QueryTestCases.DeserializationTestCases))]
        public object TestCommonQueries(string apiResponse, Type expectedReturnType)
        {
            using (var reader = new StringReader(apiResponse))
                return Deserializer.DeserializeResponse(reader, expectedReturnType);
        }

        [Test]
        public void IntInsideJsonObject()
        {
            Assert.AreEqual(37,
                Deserializer.DeserializeResponse<int>(new StringReader("[ { \"int64\": 37 } ]")).Single());
        }
    }
}