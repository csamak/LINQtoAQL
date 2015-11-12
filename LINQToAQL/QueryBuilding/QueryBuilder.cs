// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LINQToAQL.QueryBuilding
{
    internal class QueryBuilder
    {
        public QueryBuilder()
        {
            FromParts = new List<Tuple<string, string>>();
            WhereParts = new List<string>();
            OrderByParts = new List<string>();
            ResultPattern = "{0}";
        }

        public string SelectPart { get; set; }
        public string SubFrom { get; set; }
        public string NestedFrom { get; set; }
        public string GroupPart { get; set; }
        public string LimitPart { get; set; }
        public bool Existential { get; set; }
        public bool Universal { get; set; }
        public string ResultPattern { get; set; }
        private IList<Tuple<string, string>> FromParts { get; set; }
        private IList<string> WhereParts { get; set; }
        private IList<string> OrderByParts { get; set; }

        public bool IsSubQuery { get; set; }

        public void AddFromPart(string var, string source)
        {
            FromParts.Add(Tuple.Create(var, source));
            //need to handle subqueries!
            //string dataset = querySource.FromExpression.NodeType == ExpressionType.MemberAccess
            //? AqlExpressionVisitor.GetAqlExpression(querySource.FromExpression)
            //: querySource.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name);
            //FromParts.Add(Tuple.Create(querySource.ItemName, dataset ?? querySource.ItemType.Name));
        }

        //public void AddFromPart(JoinClause querySource)
        //{
        //FromParts.Add(Tuple.Create(querySource.ItemName,
        //querySource.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name) ?? querySource.ItemType.Name));
        //}

        public void AddWherePart(string formatString, params object[] args)
        {
            WhereParts.Add(string.Format(formatString, args));
        }

        //expression, ordering
        public void AddOrderByPart(IEnumerable<Tuple<string, string>> orderings)
        {
            OrderByParts.Insert(0, string.Join(", ", orderings.Select(t => t.Item1 + " " + t.Item2)));
        }

        public string BuildAqlString()
        {
            var stringBuilder = new StringBuilder();
            if (string.IsNullOrEmpty(SelectPart) || FromParts.Count == 0)
                throw new InvalidOperationException("A query must have a return and at least one from.");
            if (IsSubQuery) stringBuilder.Append('(');
            if (!string.IsNullOrEmpty(NestedFrom))
                stringBuilder.AppendFormat("({0})", NestedFrom);
            else
            {
                if (Existential)
                    stringBuilder.Append(string.Join(" ",
                        FromParts.Select(f => string.Format("some ${0} in {1}", f.Item1, f.Item2))));
                else if (Universal)
                    stringBuilder.Append(string.Join(" ",
                        FromParts.Select(f => string.Format("every ${0} in {1}", f.Item1, f.Item2))));
                else
                    stringBuilder.Append(string.Join(" ",
                        FromParts.Select(f => string.Format("for ${0} in {1}", f.Item1, f.Item2))));
            }
            stringBuilder.Append(GroupPart);
            if (WhereParts.Count > 0)
                stringBuilder.AppendFormat(" {0} {1}", Existential || Universal ? "satisfies" : "where",
                    string.Join(" and ", WhereParts));
            if (OrderByParts.Count > 0)
                stringBuilder.AppendFormat(" order by {0}", string.Join(", ", OrderByParts));
            stringBuilder.Append(LimitPart);
            if (!(Existential || Universal))
                stringBuilder.AppendFormat(" return {0}", SelectPart);
            if (IsSubQuery) stringBuilder.Append(')');
            return string.Format(ResultPattern, FormatGenerated(stringBuilder.ToString()));
        }

        /// <summary>
        ///     This is necesary because relinq generates variables identified by &lt;generated&gt;_number. AQL does not allow &lt;
        ///     or &gt; in variable identifiers. Rather than modifying the parser and introducing more complexity in this project,
        ///     the below is a workaround to remove the special characters. Later to avoid issues in corner cases, this should be
        ///     moved to the member expression visitor or a custom parser should be used.
        /// </summary>
        private static string FormatGenerated(string aql)
        {
            //TODO: use quotes to escape in AQL
            return aql.Replace("$generated_", "$generated_uservar_").Replace("$<generated>_", "$generated_");
        }
    }
}