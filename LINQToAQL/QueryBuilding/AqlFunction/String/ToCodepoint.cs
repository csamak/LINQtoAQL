using System;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class ToCodepoint : AqlFunctionVisitorBase
    {
        public ToCodepoint(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("ToCharArray", new Type[0]));
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("string-to-codepoint", expression.Object);
        }
    }
}