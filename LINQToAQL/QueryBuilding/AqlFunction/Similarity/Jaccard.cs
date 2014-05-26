using System.Linq.Expressions;
using System.Text;
using LINQToAQL.Similarity;

namespace LINQToAQL.QueryBuilding.AqlFunction.Similarity
{
    internal class Jaccard : AqlFunctionVisitor
    {
        public Jaccard(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Name == "Jaccard" && expression.Method.IsGenericMethod &&
                   expression.Method.GetGenericMethodDefinition() ==
                   typeof (JaccardExtensions).GetMethod("Jaccard");
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("similarity-jaccard", expression.Arguments[0], expression.Arguments[1]);
        }
    }
}