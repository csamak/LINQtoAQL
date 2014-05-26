using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.Numeric
{
    internal class Ceiling : AqlFunctionVisitorBase
    {
        public Ceiling(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return typeof (Math).GetMethods().Where(m => m.Name == "Ceiling").Contains(expression.Method);
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("numeric-ceiling", expression.Arguments[0]);
        }
    }
}