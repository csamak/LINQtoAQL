using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    class CharIndex : AqlFunctionVisitorBase
    {
        public CharIndex(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("get_Chars"));
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlExpression.Append("string-to-codepoint(");
            Visitor.VisitExpression(expression.Object);
            AqlExpression.Append(")[");
            AqlExpression.Append(expression.Arguments[0]);
            AqlExpression.Append("]");
        }
    }
}
