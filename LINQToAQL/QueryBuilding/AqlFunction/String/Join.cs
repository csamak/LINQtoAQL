using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.String
{
    internal class Join : AqlFunctionVisitor
    {
        public Join(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return
                new[]
                {
                    typeof (string).GetMethod("Join", new[] {typeof (string), typeof (IEnumerable<string>)}),
                    typeof (string).GetMethod("Join", new[] {typeof (string), typeof (string[])})
                }.Contains(
                    expression.Method);
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("string-join", expression.Arguments[1], expression.Arguments[0]);
        }
    }
}