using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

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
        private readonly JsonSerializer _serializer = new JsonSerializer();

        public AqlQueryResultRetriever(Uri baseAddress, string dataverse)
        {
            _client.BaseAddress = baseAddress;
            _dataverse = dataverse;
        }

        public IEnumerable<T> GetResults<T>(string query)
        {
            using (
                var sr =
                    new StreamReader(
                        _client.GetStreamAsync("/query?query=" +
                                               Uri.EscapeDataString(string.Format("use dataverse {0}; {1}", _dataverse,
                                                   query))).Result))
            using (var jsonTextReader = new JsonTextReader(sr))
                return ParseResults<T>(_serializer.Deserialize<Results>(jsonTextReader));
        }

        public T GetScalar<T>(string query)
        {
            using (
                var sr =
                    new StreamReader(
                        _client.GetStreamAsync(
                            new Uri("/query?query=" +
                                    Uri.EscapeDataString(string.Format("use dataverse {0}; {1}", _dataverse, query))))
                            .Result))
            using (var jsonTextReader = new JsonTextReader(sr))
                return ParseResults<T>(_serializer.Deserialize<Results>(jsonTextReader)).First();
        }

        private IEnumerable<T> ParseResults<T>(Results result)
        {
            return result.results.Select(JsonConvert.DeserializeObject<T>);
        }
    }

    internal class Results
    {
        public IEnumerable<string> results { get; set; }
    }
}