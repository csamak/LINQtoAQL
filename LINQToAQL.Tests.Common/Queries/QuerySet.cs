using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using LINQToAQL.Tests.Common.Model;

namespace LINQToAQL.Tests.Common.Queries
{
    public abstract class QuerySet
    {
        protected readonly TinySocial dv = TestEnvironment.Dataverse;

        public abstract IEnumerable<TestQuery> Queries { get; }
    }
}