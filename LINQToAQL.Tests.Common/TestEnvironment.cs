using System;
using System.Configuration;
using LINQToAQL.Tests.Common.Model;

namespace LINQToAQL.Tests.Common
{
    public static class TestEnvironment
    {
        public static readonly TinySocial Dataverse = new TinySocial(new Uri(ConfigurationManager.AppSettings["AsterixDBEndpoint"] ?? "http://localhost:19002"));
    }
}
