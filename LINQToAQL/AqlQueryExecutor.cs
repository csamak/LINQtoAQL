using System;
using System.Collections.Generic;
using Remotion.Linq;

namespace LINQToAQL
{
    internal class AqlQueryExecutor : IQueryExecutor
    {
        private string _conString;

        public AqlQueryExecutor(string conString)
        {
            _conString = conString;
        }

        /// <inheritdoc />
        public IEnumerable<T> ExecuteCollection<T>(QueryModel queryModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T ExecuteScalar<T>(QueryModel queryModel)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public T ExecuteSingle<T>(QueryModel queryModel, bool returnDefaultWhenEmpty)
        {
            throw new NotImplementedException();
        }
    }
}