using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace LINQToAQL.Deserialization.Json
{
    /// <summary>
    ///     Deserializes JSON responses from the AsterixDB API.
    /// </summary>
    public class JsonResponseDeserializer : IResponseDeserializer
    {
        private readonly JsonSerializer _serializer = new JsonSerializer();

        /// <summary>
        ///     Constructs a new <see cref="JsonResponseDeserializer"/>.
        /// </summary>
        public JsonResponseDeserializer()
        {
            _serializer.Converters.Add(new IntConverter());
            _serializer.ContractResolver = new LinqToAqlContractResolver();
        }

        /// <summary>
        ///     Deserialize an AsterixDB API JSON response from a <see cref="TextReader"/>.
        /// </summary>
        /// <typeparam name="T">The expected type in the result array.</typeparam>
        /// <param name="reader">The <see cref="TextReader"/> from which to read the JSON response.</param>
        /// <returns>The deserialized response.</returns>
        public IEnumerable<T> DeserializeResponse<T>(TextReader reader)
        {
            using (var jsonTextReader = new JsonTextReader(reader))
                return _serializer.Deserialize<IEnumerable<T>>(jsonTextReader);
        }

        /// <summary>
        ///     Deserialize an AsterixDB API JSON response from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> from which to read the JSON response.</param>
        /// <param name="type">The expected return type.</param>
        /// <returns>The deserialized response.</returns>
        public object DeserializeResponse(TextReader reader, Type type)
        {
            using (var jsonTextReader = new JsonTextReader(reader))
                return _serializer.Deserialize(jsonTextReader, typeof (IEnumerable<>).MakeGenericType(type));
        }
    }
}