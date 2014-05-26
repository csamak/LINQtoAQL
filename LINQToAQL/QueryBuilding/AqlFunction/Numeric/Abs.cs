using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.Numeric
{
    internal class Abs : AqlFunctionVisitor
    {
        public Abs(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return typeof (Math).GetMethods().Where(m => m.Name == "Abs").Contains(expression.Method);
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("numeric-abs", expression.Arguments[0]);
        }
    }
}