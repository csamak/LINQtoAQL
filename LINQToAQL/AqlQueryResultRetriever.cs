using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LINQToAQL
{
    class AqlQueryResultRetriever
    {
        readonly HttpClient _client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate });

        public AqlQueryResultRetriever(Uri baseAddress)
        {
            _client.BaseAddress = baseAddress;
        }

        public IEnumerable<T> GetResults<T>(string query)
        {
            throw new NotImplementedException();
        }

        public T GetScalar<T>(string query)
        {
            throw new NotImplementedException();
        }
    }
}
