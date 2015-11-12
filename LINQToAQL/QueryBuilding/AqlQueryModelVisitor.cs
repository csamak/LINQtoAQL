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
using System.Linq;
using System.Linq.Expressions;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;

namespace LINQToAQL.QueryBuilding
{
    /// <summary>
    ///     Generates AQL query strings
    /// </summary>
    public static class AqlQueryGenerator //exists to allow public method below
    {
        /// <summary>
        ///     Generates AQL query strings given a <see cref="QueryModel" />.
        /// </summary>
        /// <param name="queryModel">The query model that represents the query to generate</param>
        /// <param name="isSubQuery">Whether the query is a subquery</param>
        /// <returns>The AQL query string</returns>
        public static string GenerateAqlQuery(QueryModel queryModel, bool isSubQuery = false)
            //better way to handle ; in subqueries?
        {
            var visitor = new AqlQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            visitor.QueryBuilder.IsSubQuery = isSubQuery;
            return visitor.GetAqlQuery();
        }
    }

    internal class AqlQueryModelVisitor : QueryModelVisitorBase
    {
        internal readonly QueryBuilder QueryBuilder = new QueryBuilder();

        public string GetAqlQuery()
        {
            return QueryBuilder.BuildAqlString();
        }

        public override void VisitQueryModel(QueryModel queryModel)
        {
            queryModel.SelectClause.Accept(this, queryModel);
            queryModel.MainFromClause.Accept(this, queryModel);
            VisitBodyClauses(queryModel.BodyClauses, queryModel);
            VisitResultOperators(queryModel.ResultOperators, queryModel);
        }

        public override void VisitResultOperator(ResultOperatorBase resultOperator, QueryModel queryModel, int index)
        {
            //TODO: make sure we shouldn't be using sql-sum, etc.
            if (resultOperator is CountResultOperator)
                QueryBuilder.ResultPattern = "count({0})";
            else if (resultOperator is AnyResultOperator) // does count > 1 work?
                QueryBuilder.Existential = true;
            else if (resultOperator is AllResultOperator)
            {
                QueryBuilder.Universal = true;
                QueryBuilder.AddWherePart(GetAqlExpression(((AllResultOperator) resultOperator).Predicate));
            }
            else if (resultOperator is AverageResultOperator)
                QueryBuilder.ResultPattern = "avg({0})";
            else if (resultOperator is SumResultOperator)
                QueryBuilder.ResultPattern = "sum({0})";
            else if (resultOperator is MaxResultOperator)
                QueryBuilder.ResultPattern = "max({0})";
            else if (resultOperator is MinResultOperator)
                QueryBuilder.ResultPattern = "min({0})";
            else if (resultOperator is TakeResultOperator)
                QueryBuilder.LimitPart = " limit " +
                                         AqlExpressionVisitor.GetAqlExpression(
                                             ((TakeResultOperator) resultOperator).Count);
            else if (resultOperator is GroupResultOperator)
            {
                var groupResult = (GroupResultOperator) resultOperator;
                QueryBuilder.GroupPart = string.Format(" group by {0} with {1}",
                    AqlExpressionVisitor.GetAqlExpression(groupResult.KeySelector),
                    AqlExpressionVisitor.GetAqlExpression(groupResult.ElementSelector));
            }
            else
                throw new NotSupportedException("Operator not supported!");
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            VisitFromClause(fromClause, queryModel);
            base.VisitMainFromClause(fromClause, queryModel);
        }

        private void VisitFromClause(FromClauseBase fromClause, QueryModel queryModel)
        {
            if (fromClause.FromExpression is SubQueryExpression)
                //QueryBuilder.NestedFrom = AqlQueryGenerator.GenerateAqlQuery(((SubQueryExpression) fromClause.FromExpression).QueryModel, true);
                //QueryBuilder.AddFromPart(new MainFromClause("GroupFrom", fromClause.FromExpression.Type, fromClause.FromExpression));
                QueryBuilder.AddFromPart(fromClause.ItemName,
                    AqlQueryGenerator.GenerateAqlQuery(((SubQueryExpression) fromClause.FromExpression).QueryModel, true));
            else
            {
                //string dataset = querySource.FromExpression.NodeType == ExpressionType.MemberAccess
                //? AqlExpressionVisitor.GetAqlExpression(querySource.FromExpression)
                //: querySource.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name);
                //FromParts.Add(Tuple.Create(querySource.ItemName, dataset ?? querySource.ItemType.Name));
                string source;
                if (fromClause.FromExpression.NodeType == ExpressionType.Constant)
                    source = "dataset " + (fromClause.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name) ??
                                           fromClause.ItemType.Name);
                else
                    source = AqlExpressionVisitor.GetAqlExpression(fromClause.FromExpression);
                //should used referencedquerysource instead of itemname?
                QueryBuilder.AddFromPart(fromClause.ItemName, source);
            }
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            QueryBuilder.SelectPart = GetAqlExpression(selectClause.Selector);
            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            QueryBuilder.AddWherePart(GetAqlExpression(whereClause.Predicate));
            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            QueryBuilder.AddOrderByPart(
                orderByClause.Orderings.Select(
                    o =>
                        Tuple.Create(GetAqlExpression(o.Expression),
                            o.OrderingDirection == OrderingDirection.Asc ? "asc" : "desc")));
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            //TODO: index
            //cross join
            QueryBuilder.AddFromPart(joinClause.ItemName,
                "dataset " + (joinClause.ItemType.GetAttributeValue((DatasetAttribute d) => d.Name) ??
                              joinClause.ItemType.Name));
            QueryBuilder.AddWherePart("({0} = {1})", GetAqlExpression(joinClause.OuterKeySelector),
                GetAqlExpression(joinClause.InnerKeySelector));
            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            VisitFromClause(fromClause, queryModel);
            base.VisitAdditionalFromClause(fromClause, queryModel, index);
        }

        public override void VisitGroupJoinClause(GroupJoinClause groupJoinClause, QueryModel queryModel, int index)
        {
            throw new NotSupportedException("group join not supported!");
        }

        private string GetAqlExpression(Expression expression)
        {
            return AqlExpressionVisitor.GetAqlExpression(expression); //, _parameterAggregator);
        }
    }
}