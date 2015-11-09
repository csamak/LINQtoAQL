using LINQToAQL.Deserialization;

namespace LINQToAQL.Tests.Unit.Deserialization.Json
{
    internal class JsonResponseDeserializerTests : BaseResponseDeserializerTests
    {
        protected override IResponseDeserializer Deserializer
            => new LINQToAQL.Deserialization.Json.JsonResponseDeserializer();
    }
}