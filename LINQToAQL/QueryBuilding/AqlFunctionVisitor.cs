using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LINQToAQL.QueryBuilding
{
    class AqlFunctionVisitor
    {
        private readonly StringBuilder _aqlExpression;
        private readonly AqlExpressionVisitor _visitor;
        public AqlFunctionVisitor(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
        {
            _aqlExpression = aqlExpression;
            _visitor = visitor;
        }
        public bool VisitAqlFunction(MethodCallExpression expression)
        {
            if (typeof(Math).GetMethods().Where(m => m.Name == "Abs").Contains(expression.Method))
                SingleArg("numeric-abs", expression);
            else if (typeof(Math).GetMethods().Where(m => m.Name == "Ceiling").Contains(expression.Method))
                SingleArg("numeric-ceiling", expression);
            else if (typeof(Math).GetMethods().Where(m => m.Name == "Floor").Contains(expression.Method))
                SingleArg("numeric-floor", expression);
            else
                return false;
            return true;
        }

        protected void SingleArg(string name, MethodCallExpression expression)
        {
            _aqlExpression.Append(name);
            _aqlExpression.Append("(");
            _visitor.VisitExpression(expression.Arguments[0]);
            _aqlExpression.Append(")");
        }
    }
}
