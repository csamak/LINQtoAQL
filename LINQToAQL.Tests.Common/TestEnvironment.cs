using System;
using System.Configuration;
using System.Net.Http;
using LINQToAQL.Tests.Common.Model;

namespace LINQToAQL.Tests.Common
{
    public static class TestEnvironment
    {
        private static readonly Uri AsterixDbEnpoint =
            new Uri(ConfigurationManager.AppSettings["AsterixDBEndpoint"] ?? "http://localhost:19002");

        private static readonly HttpClient Client = new HttpClient();

        public static readonly TinySocial Dataverse = new TinySocial(AsterixDbEnpoint);

        public static string ExecuteAql(string aql)
        {
            return
                Client.PostAsync(new Uri(AsterixDbEnpoint, "aql"), new StringContent(aql))
                    .Result.Content.ReadAsStringAsync()
                    .Result;
        }
    }
}