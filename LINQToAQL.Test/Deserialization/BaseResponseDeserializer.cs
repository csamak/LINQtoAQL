using System.IO;
using System.Linq;
using LINQToAQL.Deserialization;
using NUnit.Framework;

namespace LINQToAQL.Test.Deserialization
{
    internal abstract class BaseResponseDeserializer
    {
        protected abstract IResponseDeserializer Deserializer { get; }

        [Test]
        public void IntInsideJsonObject()
        {
            Assert.AreEqual(37,
                Deserializer.DeserializeResponse<int>(new StringReader("[ { \"int64\": 37 } ]")).Single());
        }
    }
}