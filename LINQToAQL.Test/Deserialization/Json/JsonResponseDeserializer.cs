using LINQToAQL.Deserialization;

namespace LINQToAQL.Test.Deserialization.Json
{
    internal class JsonResponseDeserializer : BaseResponseDeserializer
    {
        protected override IResponseDeserializer Deserializer
            => new LINQToAQL.Deserialization.Json.JsonResponseDeserializer();
    }
}