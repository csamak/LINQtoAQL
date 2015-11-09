using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LINQToAQL.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LINQToAQL.Deserialization.Json
{
    internal class PointConverter : JsonConverter
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
                JProperty prop;
                try
                {
                    prop = jsonObject.Properties().Single();
                }
                catch (Exception e)
                {
                    throw new Exception("Expected a point type but received: " + jsonObject, e);
                }
                if ("point" == prop.Name && prop.Value.Type == JTokenType.Array)
                {
                    var array = (JArray)prop.Value;
                    return new Point(array[0].Value<double>(), array[1].Value<double>());
                }
            }
            else if (reader.TokenType == JsonToken.Null)
                return null;
            throw new NotSupportedException($"Could not read JSON [{reader.ReadAsString()}] as a numeric type");
        }

        public override bool CanConvert(Type objectType)
        {
            return new[] { typeof(Point) }.Any( t => t == objectType);
        }
    }
}
