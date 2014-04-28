using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ExpressionTreeVisitors;
using Remotion.Linq.Parsing;

namespace LINQToAQL.QueryBuilding
{
    public class AqlExpressionVisitor : ThrowingExpressionTreeVisitor
    {
        public static string GetAqlExpression(Expression linqExpression)//, ParameterCollection parameters)
        {
            //var visitor = new AqlExpressionVisitor(parameters);
            var visitor = new AqlExpressionVisitor();
            visitor.VisitExpression(linqExpression);
            return visitor.GetAqlExpression();
        }

        private readonly StringBuilder _aqlExpression = new StringBuilder();
        //private readonly ParameterCollection _parameters;

        private AqlExpressionVisitor()//ParameterCollection parameters)
        {
            //_parameters = parameters;
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
            _aqlExpression.Append("(");
            VisitExpression(expression.Left); //nested expressions
            switch (expression.NodeType)
            {
                case ExpressionType.Equal:
                    _aqlExpression.Append(" = ");
                    break;
                case ExpressionType.AndAlso:
                case ExpressionType.And:
                    _aqlExpression.Append(" and ");
                    break;
                case ExpressionType.OrElse:
                case ExpressionType.Or:
                    _aqlExpression.Append(" or ");
                    break;
                case ExpressionType.Add:
                    _aqlExpression.Append(" + ");
                    break;
                case ExpressionType.Subtract:
                    _aqlExpression.Append(" - ");
                    break;
                case ExpressionType.Multiply:
                    _aqlExpression.Append(" * ");
                    break;
                case ExpressionType.Divide:
                    _aqlExpression.Append(" / ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _aqlExpression.Append(" >= ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _aqlExpression.Append(" <= ");
                    break;
                default:
                    base.VisitBinaryExpression(expression);
                    break;
            }
            VisitExpression(expression.Right);
            _aqlExpression.Append(")");
            return expression;
        }

        protected override Expression VisitMemberExpression(MemberExpression expression)
        {
            VisitExpression(expression.Expression);
            string field = expression.Expression.Type.GetMember(expression.Member.Name)
                .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                .GetAttributeValue((FieldAttribute f) => f.Name);
            _aqlExpression.AppendFormat(".{0}", field ?? expression.Member.Name);
            return expression;
        }

        protected override Expression VisitConstantExpression(ConstantExpression expression)
        {
            //var namedParameter = _parameters.AddParameter(expression.Value);
            //_aqlExpression.AppendFormat(":{0}", namedParameter.Name);
            if (expression.Type.FullName == "System.DateTime") //should we check expression.Value instead?
                _aqlExpression.AppendFormat("datetime('{0}')", ((DateTime)expression.Value).ToString("O"));
            else
                _aqlExpression.Append(expression.Value);
            return expression;
        }

        protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
        {
            if (expression.Method.Equals(typeof(string).GetMethod("Contains")))
            {
                _aqlExpression.Append("(");
                VisitExpression(expression.Object);
                _aqlExpression.Append(" like '%'+");
                VisitExpression(expression.Arguments[0]);
                _aqlExpression.Append("+'%')");
                return expression;
            }
            return base.VisitMethodCallExpression(expression);
        }

        protected override Expression VisitNewExpression(NewExpression expression)
        {
            _aqlExpression.Append("{");
            for (int i = 0; i < expression.Arguments.Count; i++)
            {
                _aqlExpression.AppendFormat(" \"{0}\": ", expression.Members[i].Name);
                VisitExpression(expression.Arguments[i]);
                if (i < expression.Arguments.Count - 1) //better way?
                    _aqlExpression.Append(',');
            }
            _aqlExpression.Append(" }");
            return expression;
        }

        protected override Expression VisitUnaryExpression(UnaryExpression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                //TODO: more intelligent converts?
                VisitExpression(expression.Operand);
                return expression;
            }
            _aqlExpression.Append(expression.NodeType);
            _aqlExpression.Append('(');
            VisitExpression(expression.Operand);
            _aqlExpression.Append(')');

            return expression;
        }

        protected override Expression VisitSubQueryExpression(SubQueryExpression expression)
        {
            //is this fragile?
            _aqlExpression.Append(AqlQueryModelVisitor.GenerateAqlQuery(expression.QueryModel, true));
            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            return new NotSupportedException(string.Format("The expression '{0}' (type: {1}) is not supported.", FormatUnhandledItem(unhandledItem), typeof(T)));
        }

        private static string FormatUnhandledItem<T>(T unhandledItem)
        {
            var exp = unhandledItem as Expression;
            return exp != null ? FormattingExpressionTreeVisitor.Format(exp) : unhandledItem.ToString();
        }
    }
}