using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;
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
                //else if (resultOperator is GroupResultOperator)
            else
                throw new NotSupportedException("Operator not supported!");
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            //var test = GetAqlExpression(fromClause.FromExpression);
            QueryBuilder.AddFromPart(fromClause);
            base.VisitMainFromClause(fromClause, queryModel);
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
            QueryBuilder.AddOrderByPart(orderByClause.Orderings.Select(o => GetAqlExpression(o.Expression)));
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            QueryBuilder.AddFromPart(joinClause); //cross join
            QueryBuilder.AddWherePart("({0} = {1})", GetAqlExpression(joinClause.OuterKeySelector),
                GetAqlExpression(joinClause.InnerKeySelector));
            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            QueryBuilder.AddFromPart(fromClause);
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