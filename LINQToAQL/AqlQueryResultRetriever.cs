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
                _client.BaseAddress = baseAddress;
            _dataverse = dataverse;
        }

        //TODO: Read results incrementally
        public IEnumerable<T> GetResults<T>(string query)
        {
            using (
                var stream =
                    _client.PostAsync("query", new StringContent(FullQuery(query)))
                        .Result.Content.ReadAsStreamAsync()
                        .Result)
            using (var sr = new StreamReader(stream))
                return _deserializer.DeserializeResponse<T>(sr);
        }

        public T GetScalar<T>(string query)
        {
            using (
                var stream =
                    _client.PostAsync("query", new StringContent(FullQuery(query)))
                        .Result.Content.ReadAsStreamAsync()
                        .Result)
            using (var sr = new StreamReader(stream))
                return _deserializer.DeserializeResponse<T>(sr).Single();
        }

        private string FullQuery(string query)
        {
            return $"use dataverse {_dataverse}; {query}";
        }
    }
}