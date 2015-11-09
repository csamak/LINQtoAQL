using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class Substring : AqlFunctionVisitor
    {
        public Substring(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("Substring", new[] {typeof (int)}));
        }

        //AQL uses offset while C# uses index.
        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("substring", expression.Object, Expression.MakeBinary(ExpressionType.Add, expression.Arguments[0], Expression.Constant(1)));
        }
    }
}