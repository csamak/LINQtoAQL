using System;
using System.Linq;
using System.Linq.Expressions;
using Remotion.Linq;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.ResultOperators;

namespace LINQToAQL.QueryBuilding
{
    public class AqlQueryModelVisitor : QueryModelVisitorBase
    {
        public static string GenerateAqlQuery(QueryModel queryModel, bool isSubQuery = false) //better way to handle ; in subqueries?
        {
            var visitor = new AqlQueryModelVisitor();
            visitor.VisitQueryModel(queryModel);
            visitor._queryBuilder.IsSubQuery = isSubQuery;
            return visitor.GetAqlQuery();
        }

        private readonly QueryBuilder _queryBuilder = new QueryBuilder();
        //private readonly ParameterAggregator _parameterAggregator = new ParameterAggregator();

        //public ParameterizedQuery GetAqlQuery()
        //{
        //return new ParameterizedQuery(_queryBuilder.BuildAqlString(), _parameterAggregator.GetParameters());
        //}
        public string GetAqlQuery()
        {
            return _queryBuilder.BuildAqlString();
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
            if (resultOperator is CountResultOperator)
                _queryBuilder.ResultPattern = "count({0})";
            else if (resultOperator is AnyResultOperator) // does count > 1 work?
                _queryBuilder.Existential = true;
            else if (resultOperator is AllResultOperator)
            {
                _queryBuilder.Universal = true;
                _queryBuilder.AddWherePart(GetAqlExpression(((AllResultOperator)resultOperator).Predicate));
            }
            //else if (resultOperator is GroupResultOperator)
            else
                throw new NotSupportedException("Operator not supported!");
            base.VisitResultOperator(resultOperator, queryModel, index);
        }

        public override void VisitMainFromClause(MainFromClause fromClause, QueryModel queryModel)
        {
            //var test = GetAqlExpression(fromClause.FromExpression);
            _queryBuilder.AddFromPart(fromClause);
            base.VisitMainFromClause(fromClause, queryModel);
        }

        public override void VisitSelectClause(SelectClause selectClause, QueryModel queryModel)
        {
            _queryBuilder.SelectPart = GetAqlExpression(selectClause.Selector);
            base.VisitSelectClause(selectClause, queryModel);
        }

        public override void VisitWhereClause(WhereClause whereClause, QueryModel queryModel, int index)
        {
            _queryBuilder.AddWherePart(GetAqlExpression(whereClause.Predicate));
            base.VisitWhereClause(whereClause, queryModel, index);
        }

        public override void VisitOrderByClause(OrderByClause orderByClause, QueryModel queryModel, int index)
        {
            _queryBuilder.AddOrderByPart(orderByClause.Orderings.Select(o => GetAqlExpression(o.Expression)));
            base.VisitOrderByClause(orderByClause, queryModel, index);
        }

        public override void VisitJoinClause(JoinClause joinClause, QueryModel queryModel, int index)
        {
            _queryBuilder.AddFromPart(joinClause); //cross join
            _queryBuilder.AddWherePart("({0} = {1})", GetAqlExpression(joinClause.OuterKeySelector), GetAqlExpression(joinClause.InnerKeySelector));
            base.VisitJoinClause(joinClause, queryModel, index);
        }

        public override void VisitAdditionalFromClause(AdditionalFromClause fromClause, QueryModel queryModel, int index)
        {
            _queryBuilder.AddFromPart(fromClause);
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