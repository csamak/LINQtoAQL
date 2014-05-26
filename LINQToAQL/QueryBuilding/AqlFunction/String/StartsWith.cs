using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class StartsWith : AqlFunctionVisitor
    {
        public StartsWith(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("StartsWith", new[] {typeof (string)}));
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("starts-with", expression.Object, expression.Arguments[0]);
        }
    }
}