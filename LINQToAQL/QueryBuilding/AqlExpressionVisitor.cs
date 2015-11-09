using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;
using LINQToAQL.QueryBuilding.AqlFunction;
using LINQToAQL.Spatial;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing;

namespace LINQToAQL.QueryBuilding
{
    internal class AqlExpressionVisitor : ThrowingExpressionVisitor
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
            visitor.Visit(linqExpression);
            return visitor.GetAqlExpression();
        }

        private string GetAqlExpression()
        {
            return _aqlExpression.ToString();
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            _aqlExpression.Append("$" + expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
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
                Visit(expression.Left);
                _aqlExpression.Append("[");
                Visit(expression.Right);
                _aqlExpression.Append("]");
            }
            else
            {
                _aqlExpression.Append("(");
                Visit(expression.Left); //nested expressions
                string op;
                if (Operators.Binary.TryGetValue(expression.NodeType, out op))
                    _aqlExpression.Append(op);
                else
                    base.VisitBinary(expression);
                Visit(expression.Right);
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
            Visit(toCompare);
            _aqlExpression.Append(comparisonType == ExpressionType.Equal ? ")" : "))");
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (expression.Member.Name == "Key" &&
                expression.Expression.Type.GetGenericTypeDefinition() == typeof (IGrouping<,>)) //grouping
            {
                var temp =
                    GetKeySelector(
                        (SubQueryExpression)
                            ((MainFromClause)
                                ((QuerySourceReferenceExpression) expression.Expression).ReferencedQuerySource)
                                .FromExpression);
                Visit(expression.Expression);
                var field = temp.Expression.Type.GetMember(temp.Member.Name)
                    .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                    .GetAttributeValue((FieldAttribute f) => f.Name);
                _aqlExpression.AppendFormat("[0].{0}", field ?? temp.Member.Name);
            }
            else if (expression.Member.Name == "Length" && expression.Member.DeclaringType == typeof (string))
            {
                _aqlExpression.Append("string-length(");
                Visit(expression.Expression);
                _aqlExpression.Append(")");
            }
            else
            {
                Visit(expression.Expression);
                var field = expression.Expression.Type.GetMember(expression.Member.Name)
                    .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                    .GetAttributeValue((FieldAttribute f) => f.Name);
                _aqlExpression.AppendFormat(".{0}", field ?? expression.Member.Name);
            }
            return expression;
        }

        protected override Expression VisitMemberInit(MemberInitExpression expression)
        {
            _aqlExpression.Append("{");
            for (var i = 0; i < expression.Bindings.Count; i++)
            {
                var assignment = expression.Bindings[i] as MemberAssignment;
                if (assignment == null)
                    return base.VisitMemberInit(expression);
                //what if it isn't named?
                _aqlExpression.AppendFormat(" \"{0}\": ", assignment.Member.Name);
                Visit(assignment.Expression);
                if (i < expression.Bindings.Count - 1) //better way?
                    _aqlExpression.Append(',');
            }
            _aqlExpression.Append(" }");
            return expression;
        }

        //TODO: should we search secondary from clauses?
        private MemberExpression GetKeySelector(SubQueryExpression exp)
        {
            var temp = exp.QueryModel.ResultOperators.FirstOrDefault(r => r is GroupResultOperator);
            return temp == null
                ? GetKeySelector((SubQueryExpression) exp.QueryModel.MainFromClause.FromExpression)
                : (MemberExpression) ((GroupResultOperator) temp).KeySelector;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            //var namedParameter = _parameters.AddParameter(expression.Value);
            //_aqlExpression.AppendFormat(":{0}", namedParameter.Name);
            if (expression.Type == typeof (DateTime))
                _aqlExpression.AppendFormat("datetime('{0}')",
                    ((DateTime) expression.Value).ToString("yyyy-MM-ddTHH:mm:ss.fffzzz"));
            else if (expression.Type == typeof (string))
                _aqlExpression.AppendFormat("\"{0}\"", expression.Value);
            else if (expression.Type == typeof(Point))
            {
                var point = (Point) expression.Value;
                _aqlExpression.Append($"create-point({point.X}, {point.Y})");
            }
            else if (expression.Type == typeof(Line))
            {
                var line = (Line) expression.Value;
                _aqlExpression.Append(
                    $"create-line(create-point({line.First.X}, {line.First.Y}), create-point({line.Second.X}, {line.Second.Y}))");
            }
            else if (expression.Value is IEnumerable)
            {
                _aqlExpression.Append("[");
                var val = ((IEnumerable) expression.Value).Cast<object>().ToList();
                Visit(Expression.Constant(val.First()));
                foreach (var curr in ((IEnumerable) expression.Value).Cast<object>().Skip(1))
                {
                    _aqlExpression.Append(",");
                    Visit(Expression.Constant(curr));
                }
                _aqlExpression.Append("]");
            }
            else if (expression.Value == null)
                _aqlExpression.Append("null");
            else
                _aqlExpression.Append(expression.Value);
            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            foreach (
                var function in
                    _aqlFunctions.Functions.Where(function => function.IsVisitable(expression)))
            {
                function.Visit(expression);
                return expression;
            }
            return base.VisitMethodCall(expression);
        }

        protected override Expression VisitNew(NewExpression expression)
        {
            //TODO: refactor this in the same way as the AqlFunctions
            if (expression.Type == typeof (string))
            {
                _aqlExpression.Append("codepoint-to-string(");
                Visit(expression.Arguments[0]);
                _aqlExpression.Append(")");
            }
            else if (expression.Type == typeof (Point))
            {
                _aqlExpression.Append("create-point(");
                Visit(expression.Arguments[0]);
                _aqlExpression.Append(", ");
                Visit(expression.Arguments[1]);
                _aqlExpression.Append(")");
            }
            else if (expression.Type == typeof (Line))
            {
                _aqlExpression.Append("create-line(");
                Visit(expression.Arguments[0]);
                _aqlExpression.Append(", ");
                Visit(expression.Arguments[1]);
                _aqlExpression.Append(")");
            }
            else
            {
                _aqlExpression.Append("{");
                for (var i = 0; i < expression.Arguments.Count; i++)
                {
                    //what if it isn't named?
                    _aqlExpression.AppendFormat(" \"{0}\": ", expression.Members[i].Name);
                    Visit(expression.Arguments[i]);
                    if (i < expression.Arguments.Count - 1) //better way?
                        _aqlExpression.Append(',');
                }
                _aqlExpression.Append(" }");
            }
            return expression;
        }

        protected override Expression VisitNewArray(NewArrayExpression expression)
        {
            _aqlExpression.Append("[");
            foreach (var curr in expression.Expressions)
            {
                Visit(curr);
                _aqlExpression.Append(", ");
            }
            _aqlExpression.Append("]");
            return expression;
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                //TODO: more intelligent converts?
                Visit(expression.Operand);
            }
            else
            {
                _aqlExpression.Append(expression.NodeType.ToString().ToLower());
                _aqlExpression.Append('(');
                Visit(expression.Operand);
                _aqlExpression.Append(')');
            }

            return expression;
        }

        protected override Expression VisitSubQuery(SubQueryExpression expression)
        {
            //is this fragile?
            _aqlExpression.Append(AqlQueryGenerator.GenerateAqlQuery(expression.QueryModel, true));
            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            return
                new NotSupportedException(
                    $"The expression [{unhandledItem}] with type [{typeof (T)}] and method [{visitMethod}] is not supported.");
        }
    }
}