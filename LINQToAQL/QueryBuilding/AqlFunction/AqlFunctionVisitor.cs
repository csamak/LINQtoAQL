using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction
{
    internal abstract class AqlFunctionVisitor
    {
        protected readonly StringBuilder AqlExpression;
        protected AqlExpressionVisitor Visitor;

        protected AqlFunctionVisitor(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
        {
            AqlExpression = aqlExpression;
            Visitor = visitor;
        }

        public abstract bool IsVisitable(MethodCallExpression expression);
        public abstract void Visit(MethodCallExpression expression);

        protected void AqlFunction(string name, params Expression[] args)
        {
            AqlExpression.AppendFormat("{0}(", name);
            Visitor.Visit(args[0]);
            foreach (Expression arg in args.Skip(1))
            {
                AqlExpression.Append(", ");
                Visitor.Visit(arg);
            }
            AqlExpression.Append(")");
        }
    }
}