using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using LINQToAQL.Deserialization;
using LINQToAQL.Deserialization.Json;

namespace LINQToAQL
{
    internal class AqlQueryResultRetriever
    {
        private readonly HttpClient _client =
            new HttpClient(new HttpClientHandler
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            });

        private readonly string _dataverse;
        private readonly IResponseDeserializer _deserializer = new JsonResponseDeserializer();

        public AqlQueryResultRetriever(Uri baseAddress, string dataverse)
        {
            if (baseAddress != null)
                _client.BaseAddress = new Uri(baseAddress, "query");
            _dataverse = dataverse;
        }

        public IEnumerable<T> GetResults<T>(string query)
        {
            using (var sr = new StreamReader(_client.GetStreamAsync(QueryString(query)).Result))
                return _deserializer.DeserializeResponse<T>(sr);
        }

        public T GetScalar<T>(string query)
        {
            using (var sr = new StreamReader(_client.GetStreamAsync(QueryString(query)).Result))
                return _deserializer.DeserializeResponse<T>(sr).Single();
        }

        private string QueryString(string query)
        {
            return "?query=" + Uri.EscapeDataString($"use dataverse {_dataverse}; {query}");
        }
    }
}