using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class SubstringWithLength : AqlFunctionVisitor
    {
        public SubstringWithLength(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
            : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("Substring", new[] {typeof (int), typeof (int)}));
        }

        //AQL uses offset while C# uses index. Also, the length is relative in AQL while it is a final index in C#.
        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("substring", expression.Object, Expression.MakeBinary(ExpressionType.Add, expression.Arguments[0], Expression.Constant(1)), expression.Arguments[1]);
        }
    }
}