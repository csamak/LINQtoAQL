using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class EndsWith : AqlFunctionVisitor
    {
        public EndsWith(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("EndsWith", new[] {typeof (string)}));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("ends-with", expression.Object, expression.Arguments[0]);
        }
    }
}