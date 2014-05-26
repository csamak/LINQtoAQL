using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Parsing.Structure;

namespace LINQToAQL
{
        /// <summary>
        ///     Evaluates queries remotely on an instance of AsterixDB.
        /// </summary>
        /// <typeparam name="T">The result type of the query</typeparam>
    public class AqlQueryable<T> : QueryableBase<T>
    {
        /// <summary>
        ///     Creates an <see cref="AqlQueryable{T}"/> with a dataverse and AsterixDB connection information.
        /// </summary>
        /// <param name="baseUri"></param>
        /// <param name="dataverse"></param>
        public AqlQueryable(Uri baseUri, string dataverse) :
            base(QueryParser.CreateDefault(), new AqlQueryExecutor(baseUri, dataverse))
        {
        }
        
        //TODO: Support an HttpClient overload, or the equivalent to allow specifying things like proxies

        //called by LINQ
        /// <inheritdoc />
        public AqlQueryable(IQueryProvider provider, Expression expression) : base(provider, expression)
        {
        }
    }
}