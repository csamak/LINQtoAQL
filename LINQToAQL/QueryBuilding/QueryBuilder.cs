using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;
using Remotion.Linq.Clauses;

namespace LINQToAQL.QueryBuilding
{
    public class QueryBuilder
    {
        public QueryBuilder()
        {
            FromParts = new List<string>();
            WhereParts = new List<string>();
            OrderByParts = new List<string>();
        }

        public string SelectPart { get; set; }
        private IList<string> FromParts { get; set; }
        private IList<string> WhereParts { get; set; }
        private IList<string> OrderByParts { get; set; }

        public void AddFromPart(IQuerySource querySource)
        {
            string dataset = querySource.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name);
            FromParts.Add(string.Format("${0} in dataset {1}", querySource.ItemName,
                dataset ?? querySource.ItemType.Name));
        }

        public void AddWherePart(string formatString, params object[] args)
        {
            WhereParts.Add(string.Format(formatString, args));
        }

        public void AddOrderByPart(IEnumerable<string> orderings)
        {
            OrderByParts.Insert(0, string.Join(", ", orderings));
        }

        public string BuildAqlString()
        {
            var stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(SelectPart) || FromParts.Count == 0)
                throw new InvalidOperationException("A query must have a return and at least one from.");
            foreach (string curr in FromParts) //join later
                stringBuilder.AppendFormat("for {0}", curr);
            if (WhereParts.Count > 0)
                stringBuilder.AppendFormat(" where {0}", string.Join(" and ", WhereParts));
            if (OrderByParts.Count > 0)
                stringBuilder.AppendFormat(" order by {0}", string.Join(", ", OrderByParts));
            stringBuilder.AppendFormat(" return {0};", SelectPart);
            return stringBuilder.ToString();
        }
    }
}