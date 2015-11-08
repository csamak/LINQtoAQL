using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LINQToAQL.Deserialization.Json
{
    internal class IntConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                var jsonObject = JObject.Load(reader);
                JProperty intProperty;
                try
                {
                    intProperty = jsonObject.Properties().Single();
                }
                catch (Exception e)
                {
                    throw new Exception("Expected an int type but received: " + jsonObject, e);
                }
                //This will allow implicit conversions (e.g. int64 to int32). Should this instead enforce
                //the same int size (and therefore require using LongCount instead of Count etc)?
                //Bounds errors are possible!
                if (new[] {"int8", "int16", "int32", "int64"}.Any(n => n == intProperty.Name) &&
                    intProperty.Value.Type == JTokenType.Integer)
                {
                    return Convert.ChangeType(intProperty.Value.ToString(), objectType);
                }
            }
            return serializer.Deserialize(reader);
        }

        public override bool CanConvert(Type objectType)
        {
            return
                new[] {typeof (short), typeof (int), typeof (long), typeof (ushort), typeof (uint), typeof (ulong)}.Any(
                    t => t == objectType);
        }
    }
}