using System.Linq.Expressions;

namespace LinqToAql.QueryBuilding.AqlConstructors
{
    internal class AqlConstructorConstantExpressionFactory : SingleExpressionVisitorFactory<AqlConstructorVisitor, ConstantExpression>
    {
        public AqlConstructorConstantExpressionFactory()
        {
            RegisterVisitor<DateTimeConstructor>();
            RegisterVisitor<LineConstructor>();
            RegisterVisitor<PointConstructor>();
            RegisterVisitor<StringConstructor>();
        }
    }
}
