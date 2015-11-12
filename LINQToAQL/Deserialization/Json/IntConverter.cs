// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

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
            else if (reader.TokenType == JsonToken.Integer)
                return Convert.ChangeType(JToken.Load(reader).Value<long>(), objectType);
            else if (reader.TokenType == JsonToken.Null)
                return null;
            throw new NotSupportedException($"Could not read JSON [{reader.ReadAsString()}] as a numeric type");
        }

        public override bool CanConvert(Type objectType)
        {
            return
                new[] {typeof (short), typeof (int), typeof (long), typeof (ushort), typeof (uint), typeof (ulong)}.Any(
                    t => t == objectType);
        }
    }
}