using System;
using System.Collections.Generic;
using System.IO;

namespace LINQToAQL.Deserialization
{
    /// <summary>
    ///     Defines a mechanism for deserializing responses from the AsterixDB API
    /// </summary>
    public interface IResponseDeserializer
    {
        /// <summary>
        ///     Deserialize an AsterixDB API response from a <see cref="TextReader"/>.
        /// </summary>
        /// <typeparam name="T">The expected type in the result array.</typeparam>
        /// <param name="reader">The <see cref="TextReader"/> from which to read the response.</param>
        /// <returns>The deserialized response.</returns>
        IEnumerable<T> DeserializeResponse<T>(TextReader reader);

        /// <summary>
        ///     Deserialize an AsterixDB API response from a <see cref="TextReader"/>.
        /// </summary>
        /// <param name="reader">The <see cref="TextReader"/> from which to read the response.</param>
        /// <param name="type">The expected return type.</param>
        /// <returns>The deserialized response.</returns>
        object DeserializeResponse(TextReader reader, Type type);
    }
}
