using LINQToAQL.QueryBuilding;
using Remotion.Linq;
using System;
using System.Collections.Generic;

namespace LINQToAQL
{
    internal class AqlQueryExecutor : IQueryExecutor
    {
        private readonly AqlQueryResultRetriever _resultRetriever;

        public AqlQueryExecutor(Uri baseUri, string dataverse)
        {
            _resultRetriever = new AqlQueryResultRetriever(baseUri, dataverse);
        }

        /// <inheritdoc />
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            return _resultRetriever.GetResults<T>(AqlQueryGenerator.GenerateAqlQuery(queryModel));
        }

        /// <inheritdoc />
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            return _resultRetriever.GetScalar<T>(AqlQueryGenerator.GenerateAqlQuery(queryModel));
        }

        /// <inheritdoc />
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            throw new NotImplementedException();
        }
    }
}