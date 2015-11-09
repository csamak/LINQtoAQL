using System;
using System.Collections.Generic;
using LINQToAQL.Tests.Common.Model;

namespace LINQToAQL.Tests.Common.Queries
{
    public abstract class QuerySet
    {
        protected readonly TinySocial dv = new TinySocial(new Uri("http://localhost:19002"));

        public abstract IEnumerable<TestQuery> Queries { get; }
    }
}