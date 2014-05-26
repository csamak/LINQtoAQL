using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class SubstringWithLength : AqlFunctionVisitorBase
    {
        public SubstringWithLength(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
            : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("Substring", new[] {typeof (int), typeof (int)}));
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("substring", expression.Object, expression.Arguments[0], expression.Arguments[1]);
        }
    }
}