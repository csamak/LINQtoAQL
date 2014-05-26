using System.Linq.Expressions;
using System.Text;
using LINQToAQL.Similarity;

namespace LINQToAQL.QueryBuilding.AqlFunction.Similarity
{
    internal class EditDistanceCheck : AqlFunctionVisitor
    {
        public EditDistanceCheck(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
            : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return
                typeof (EditDistanceExtensions).GetMethod("EditDistanceCheck",
                    new[] {typeof (string), typeof (string), typeof (int)}).Equals(expression.Method) ||
                expression.Method.Name == "EditDistanceCheck" && expression.Method.IsGenericMethod &&
                expression.Method.GetGenericMethodDefinition() ==
                typeof (EditDistanceExtensions).GetMethod("EditDistanceCheck");
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("edit-distance-check", expression.Arguments[0], expression.Arguments[1], expression.Arguments[2]);
        }
    }
}