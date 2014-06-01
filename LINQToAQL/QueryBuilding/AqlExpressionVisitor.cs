using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;
using LINQToAQL.QueryBuilding.AqlFunction;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing;

namespace LINQToAQL.QueryBuilding
{
    internal class AqlExpressionVisitor : ThrowingExpressionTreeVisitor
    {
        private readonly StringBuilder _aqlExpression = new StringBuilder();
        //private readonly ParameterCollection _parameters;
        private readonly AqlFunctions _aqlFunctions;

        private AqlExpressionVisitor() //ParameterCollection parameters)
        {
            //_parameters = parameters;
            _aqlFunctions = new AqlFunctions(_aqlExpression, this);
        }

        public static string GetAqlExpression(Expression linqExpression) //, ParameterCollection parameters)
        {
            //var visitor = new AqlExpressionVisitor(parameters);
            var visitor = new AqlExpressionVisitor();
            visitor.VisitExpression(linqExpression);
            return visitor.GetAqlExpression();
        }

        public string GetAqlExpression()
        {
            return _aqlExpression.ToString();
        }

        protected override Expression VisitQuerySourceReferenceExpression(QuerySourceReferenceExpression expression)
        {
            _aqlExpression.Append("$" + expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitBinaryExpression(BinaryExpression expression)
        {
            //as keyword would reduce these to 1
            if (expression.Right.NodeType == ExpressionType.Constant &&
                ((ConstantExpression) expression.Right).Value == null)
                VisitNullComparison(expression.NodeType, expression.Left);
            else if (expression.Left.NodeType == ExpressionType.Constant &&
                     ((ConstantExpression) expression.Left).Value == null)
                VisitNullComparison(expression.NodeType, expression.Right);
            else if (expression.NodeType == ExpressionType.ArrayIndex)
            {
                VisitExpression(expression.Left);
                _aqlExpression.Append("[");
                VisitExpression(expression.Right);
                _aqlExpression.Append("]");
            }
            else
            {
                _aqlExpression.Append("(");
                VisitExpression(expression.Left); //nested expressions
                string op;
                if (Operators.Binary.TryGetValue(expression.NodeType, out op))
                    _aqlExpression.Append(op);
                else
                    base.VisitBinaryExpression(expression);
                VisitExpression(expression.Right);
                _aqlExpression.Append(")");
            }
            return expression;
        }

        private void VisitNullComparison(ExpressionType comparisonType, Expression toCompare)
        {
            switch (comparisonType)
            {
                case ExpressionType.Equal:
                    _aqlExpression.Append("is-null(");
                    break;
                case ExpressionType.NotEqual:
                    _aqlExpression.Append("not(is-null(");
                    break;
                default:
                    throw new NotImplementedException("Can only compare null using != or ==");
            }
            VisitExpression(toCompare);
            _aqlExpression.Append(comparisonType == ExpressionType.Equal ? ")" : "))");
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            if (expression.Member.Name == "Key" &&
                expression.Expression.Type.GetGenericTypeDefinition() == typeof (IGrouping<,>)) //grouping
            {
                var temp =
                    (MemberExpression)
                        ((GroupResultOperator)
                            ((SubQueryExpression)
                                ((MainFromClause)
                                    ((QuerySourceReferenceExpression) expression.Expression).ReferencedQuerySource)
                                    .FromExpression).QueryModel.ResultOperators[0]).KeySelector;
                VisitExpression(expression.Expression);
                string field = temp.Expression.Type.GetMember(temp.Member.Name)
                    .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                    .GetAttributeValue((FieldAttribute f) => f.Name);
                _aqlExpression.AppendFormat("[0].{0}", field ?? temp.Member.Name);
            }
            else if (expression.Member.Name == "Length" && expression.Member.DeclaringType == typeof (string))
            {
                _aqlExpression.Append("string-length(");
                VisitExpression(expression.Expression);
                _aqlExpression.Append(")");
            }
            else
            {
                VisitExpression(expression.Expression);
                string field = expression.Expression.Type.GetMember(expression.Member.Name)
                    .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                    .GetAttributeValue((FieldAttribute f) => f.Name);
                _aqlExpression.AppendFormat(".{0}", field ?? expression.Member.Name);
            }
            return expression;
        }

        protected override Expression VisitConstantExpression(ConstantExpression expression)
        {
            //var namedParameter = _parameters.AddParameter(expression.Value);
            //_aqlExpression.AppendFormat(":{0}", namedParameter.Name);
            if (expression.Type == typeof (DateTime))
                _aqlExpression.AppendFormat("datetime('{0}')", ((DateTime) expression.Value).ToString("O"));
            else if (expression.Type == typeof (string))
                _aqlExpression.AppendFormat("\"{0}\"", expression.Value);
            else if (expression.Value is IEnumerable)
            {
                _aqlExpression.Append("[");
                List<object> val = ((IEnumerable) expression.Value).Cast<object>().ToList();
                VisitExpression(Expression.Constant(val.First()));
                foreach (object curr in ((IEnumerable) expression.Value).Cast<object>().Skip(1))
                {
                    _aqlExpression.Append(",");
                    VisitExpression(Expression.Constant(curr));
                }
                _aqlExpression.Append("]");
            }
            else if (expression.Value == null)
                _aqlExpression.Append("null");
            else
                _aqlExpression.Append(expression.Value);
            return expression;
        }

        protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
        {
            foreach (
                AqlFunctionVisitor function in
                    _aqlFunctions.Functions.Where(function => function.IsVisitable(expression)))
            {
                function.Visit(expression);
                return expression;
            }
            return base.VisitMethodCallExpression(expression);
        }

        protected override Expression VisitNewExpression(NewExpression expression)
        {
            if (expression.Type == typeof (string))
            {
                _aqlExpression.Append("codepoint-to-string(");
                VisitExpression(expression.Arguments[0]);
                _aqlExpression.Append(")");
            }
            else
            {
                _aqlExpression.Append("{");
                for (int i = 0; i < expression.Arguments.Count; i++)
                {
                    //what if it isn't named?
                    _aqlExpression.AppendFormat(" \"{0}\": ", expression.Members[i].Name);
                    VisitExpression(expression.Arguments[i]);
                    if (i < expression.Arguments.Count - 1) //better way?
                        _aqlExpression.Append(',');
                }
                _aqlExpression.Append(" }");
            }
            return expression;
        }

        protected override Expression VisitUnaryExpression(UnaryExpression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                //TODO: more intelligent converts?
                VisitExpression(expression.Operand);
            }
            else
            {
                _aqlExpression.Append(expression.NodeType.ToString().ToLower());
                _aqlExpression.Append('(');
                VisitExpression(expression.Operand);
                _aqlExpression.Append(')');
            }

            return expression;
        }

        protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
        {
            //is this fragile?
            _aqlExpression.Append(AqlQueryGenerator.GenerateAqlQuery(expression.QueryModel, true));
            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            return
                new NotSupportedException(string.Format("The expression '{0}' (type: {1}) is not supported.",
                    FormatUnhandledItem(unhandledItem), typeof (T)));
        }

        private static string FormatUnhandledItem<T>(T unhandledItem)
        {
            var exp = unhandledItem as Expression;
            return exp != null ? FormattingExpressionTreeVisitor.Format(exp) : unhandledItem.ToString();
        }
    }
}