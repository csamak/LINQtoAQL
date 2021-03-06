﻿// Licensed to the Apache Software Foundation (ASF) under one
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
using LinqToAql.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LinqToAql.Deserialization.Json
{
    internal class LineConverter : JsonConverter
    {
        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartArray)
            {
                var array = JArray.Load(reader);
                if (array.Count == 2)
                    try
                    {
                        //creating new readers shouldn't be necessary; why doesn't it work using the same reader?
                        return new Line(serializer.Deserialize<Point>(new JTokenReader(array[0])),
                            serializer.Deserialize<Point>(new JTokenReader(array[1])));
                    }
                    catch (Exception)
                    {
                        throw new Exception("Expected a line type but received: "); // + array, e);
                    }
            }
            else if (reader.TokenType == JsonToken.Null)
                return null;
            throw new NotSupportedException($"Could not read JSON [{reader.ReadAsString()}] as a line type");
        }

        public override bool CanConvert(Type objectType)
        {
            return new[] { typeof(Line) }.Any(t => t == objectType);
        }
    }
}