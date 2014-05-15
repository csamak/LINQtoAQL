using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL
{
    /// <inheritdoc />
    public class AqlQueryable<T> : QueryableBase<T>
    {
        public AqlQueryable(Uri baseUri, string dataverse) :
            base(QueryParser.CreateDefault(), new AqlQueryExecutor(baseUri, dataverse))
        {
        }

        //called by LINQ
        public AqlQueryable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }
}