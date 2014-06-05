using System.Linq.Expressions;
using System.Text;
using LINQToAQL.Spatial;

namespace LINQToAQL.QueryBuilding.AqlFunction.Spatial
{
    internal class SpatialDistance : AqlFunctionVisitor
    {
        public SpatialDistance(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Equals(typeof (Point).GetMethod("Distance"));
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("spatial-distance", expression.Object, expression.Arguments[0]);
        }
    }
}