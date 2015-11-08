using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class CharIndex : AqlFunctionVisitor
    {
        public CharIndex(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("get_Chars"));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlExpression.Append("string-to-codepoint(");
            Visitor.Visit(expression.Object);
            AqlExpression.Append(")[");
            AqlExpression.Append(expression.Arguments[0]);
            AqlExpression.Append("]");
        }
    }
}