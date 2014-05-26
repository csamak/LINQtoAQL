using System.Linq;
using System.Linq.Expressions;
using System.Text;
using LINQToAQL.Similarity;

namespace LINQToAQL.QueryBuilding.AqlFunction.Similarity
{
    internal class EditDistance : AqlFunctionVisitor
    {
        public EditDistance(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return
                typeof (EditDistanceExtensions).GetMethods()
                    .Where(m => m.Name == "EditDistance")
                    .Contains(expression.Method) ||
                (expression.Method.IsGenericMethod &&
                 typeof (EditDistanceExtensions).GetMethods()
                     .Where(m => m.Name == "EditDistance")
                     .Contains(expression.Method.GetGenericMethodDefinition()));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("edit-distance", expression.Arguments[0], expression.Arguments[1]);
        }
    }
}