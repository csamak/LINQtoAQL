using System;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class Lowercase : AqlFunctionVisitor
    {
        public Lowercase(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (string).GetMethod("ToLower", new Type[0]));
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("lowercase", expression.Object);
        }
    }
}