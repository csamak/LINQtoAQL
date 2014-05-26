using System.Linq.Expressions;
using System.Text;
using LINQToAQL.Similarity;

namespace LINQToAQL.QueryBuilding.AqlFunction.Similarity
{
    internal class JaccardCheck : AqlFunctionVisitor
    {
        public JaccardCheck(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            return expression.Method.Name == "JaccardCheck" && expression.Method.IsGenericMethod &&
                   expression.Method.GetGenericMethodDefinition() ==
                   typeof (JaccardExtensions).GetMethod("JaccardCheck");
        }

        public override void VisitAqlFunction(MethodCallExpression expression)
        {
            AqlFunction("similarity-jaccard-check", expression.Arguments[0], expression.Arguments[1],
                expression.Arguments[2]);
        }
    }
}