using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.Numeric
{
    //Math.Round defaults to MidpointRounding.ToEven, which is equivalent to numeric-round-half-to-even
    internal class Round : AqlFunctionVisitor
    {
        public Round(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return
                new[]
                {
                    typeof (Math).GetMethod("Round", new[] {typeof (double)}),
                    typeof (Math).GetMethod("Round", new[] {typeof (decimal)})
                }.Contains(expression.Method) ||
                (new[]
                {
                    typeof (Math).GetMethod("Round", new[] {typeof (double), typeof (MidpointRounding)}),
                    typeof (Math).GetMethod("Round", new[] {typeof (decimal), typeof (MidpointRounding)})
                }.Contains(
                    expression.Method) &&
                 ((ConstantExpression) expression.Arguments[1]).Value.Equals(MidpointRounding.ToEven));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("numeric-round-half-to-even", expression.Arguments[0]);
        }
    }
}