using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
    }
}