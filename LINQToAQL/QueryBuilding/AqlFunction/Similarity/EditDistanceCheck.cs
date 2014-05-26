using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LINQToAQL.Similarity;

namespace LINQToAQL.QueryBuilding.AqlFunction.Similarity
{
    internal class EditDistanceCheck : AqlFunctionVisitorBase
    {
        public EditDistanceCheck(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
            : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return
                typeof (EditDistanceExtensions).GetMethods()
                    .Where(m => m.Name == "EditDistanceCheck")
                    .Contains(expression.Method);
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("edit-distance-check", expression.Arguments[0], expression.Arguments[1], expression.Arguments[2]);
        }
    }
}