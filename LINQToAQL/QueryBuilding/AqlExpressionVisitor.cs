using System;
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
    internal class AqlExpressionVisitor : ThrowingExpressionTreeVisitor
    {
        private readonly StringBuilder _aqlExpression = new StringBuilder();
        //private readonly ParameterCollection _parameters;
        private readonly AqlFunctionVisitor _functionVisitor;

        private AqlExpressionVisitor() //ParameterCollection parameters)
        {
            //_parameters = parameters;
            _functionVisitor = new AqlFunctionVisitor(_aqlExpression, this);
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
                ((ConstantExpression)expression.Right).Value == null)
                VisitNullComparison(expression.NodeType, expression.Left);
            else if (expression.Left.NodeType == ExpressionType.Constant &&
                     ((ConstantExpression)expression.Left).Value == null)
                VisitNullComparison(expression.NodeType, expression.Right);
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
            if (expression.Member.Name == "Length" && expression.Member.DeclaringType == typeof (string))
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
            if (expression.Type == typeof(DateTime))
                _aqlExpression.AppendFormat("datetime('{0}')", ((DateTime)expression.Value).ToString("O"));
            else if (expression.Type == typeof(string))
                _aqlExpression.AppendFormat("\"{0}\"", expression.Value);
            else if (expression.Value == null)
                _aqlExpression.Append("null");
            else
                _aqlExpression.Append(expression.Value);
            return expression;
        }

        protected override Expression VisitMethodCallExpression(MethodCallExpression expression)
        {
            return !_functionVisitor.VisitAqlFunction(expression)
                ? base.VisitMethodCallExpression(expression)
                : expression;
        }

        protected override Expression VisitNewExpression(NewExpression expression)
        {
            if (expression.Type == typeof(string))
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
            _aqlExpression.Append(AqlQueryModelVisitor.GenerateAqlQuery(expression.QueryModel, true));
            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            return
                new NotSupportedException(string.Format("The expression '{0}' (type: {1}) is not supported.",
                    FormatUnhandledItem(unhandledItem), typeof(T)));
        }

        private static string FormatUnhandledItem<T>(T unhandledItem)
        {
            var exp = unhandledItem as Expression;
            return exp != null ? FormattingExpressionTreeVisitor.Format(exp) : unhandledItem.ToString();
        }
    }
}