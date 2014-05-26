using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class Contains : AqlFunctionVisitor
    {
        public Contains(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("Contains"));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("contains", expression.Object, expression.Arguments[0]);
        }
    }
}