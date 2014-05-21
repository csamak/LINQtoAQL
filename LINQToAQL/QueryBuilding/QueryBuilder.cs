using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            FromParts = new List<Tuple<string, string>>();
            WhereParts = new List<string>();
            OrderByParts = new List<string>();
        }

        public string SelectPart { get; set; }
        public bool Existential { get; set; }
        public bool Universal { get; set; }
        private IList<Tuple<string, string>> FromParts { get; set; }
        private IList<string> WhereParts { get; set; }
        private IList<string> OrderByParts { get; set; }

        public bool IsSubQuery { get; set; }

        public void AddFromPart(FromClauseBase querySource)
        {
            string dataset = querySource.FromExpression.NodeType == ExpressionType.MemberAccess
                ? AqlExpressionVisitor.GetAqlExpression(querySource.FromExpression as MemberExpression)
                : querySource.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name);
            FromParts.Add(Tuple.Create(querySource.ItemName, dataset ?? querySource.ItemType.Name));
        }

        public void AddFromPart(JoinClause querySource)
        {
            FromParts.Add(Tuple.Create(querySource.ItemName,
                querySource.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name) ?? querySource.ItemType.Name));
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
            if (IsSubQuery) stringBuilder.Append('(');
            if (Existential)
                stringBuilder.Append(string.Join(" ",
                    FromParts.Select(f => string.Format("some ${0} in {1}", f.Item1, f.Item2))));
            else if (Universal)
                stringBuilder.Append(string.Join(" ",
                    FromParts.Select(f => string.Format("every ${0} in {1}", f.Item1, f.Item2))));
            else
                stringBuilder.Append(string.Join(" ",
                    FromParts.Select(f => string.Format("for ${0} in dataset {1}", f.Item1, f.Item2))));
            if (WhereParts.Count > 0)
                stringBuilder.AppendFormat(" {0} {1}", (Existential || Universal) ? "satisfies" : "where",
                    string.Join(" and ", WhereParts));
            if (OrderByParts.Count > 0)
                stringBuilder.AppendFormat(" order by {0}", string.Join(", ", OrderByParts));
            if (!(Existential || Universal))
                stringBuilder.AppendFormat(" return {0}", SelectPart);
            if (IsSubQuery) stringBuilder.Append(')');
            return stringBuilder.ToString();
        }
    }
}