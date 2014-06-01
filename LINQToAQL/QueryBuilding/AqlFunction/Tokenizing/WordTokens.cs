using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding.AqlFunction.Tokenizing
{
    internal class WordTokens : AqlFunctionVisitor
    {
        public WordTokens(StringBuilder aqlExpression, AqlExpressionVisitor visitor) : base(aqlExpression, visitor)
        {
        }

        public override bool IsVisitable(MethodCallExpression expression)
        {
            if (expression.Method.Equals(typeof (string).GetMethod("Split", new[] {typeof (char[])})))
            {
                var arg = (IEnumerable<char>) ((ConstantExpression) expression.Arguments[0]).Value;
                return arg.Count() == 1 && arg.First() == ' ';
            }
            if (
                expression.Method.Equals(typeof (string).GetMethod("Split",
                    new[] {typeof (string[]), typeof (StringSplitOptions)})))
            {
                var arg = (IEnumerable<string>) ((ConstantExpression) expression.Arguments[0]).Value;
                return ((ConstantExpression) expression.Arguments[1]).Value.Equals(StringSplitOptions.None) &&
                       arg.Count() == 1 && arg.First() == " ";
            }
            if (
                expression.Method.Equals(typeof (string).GetMethod("Split",
                    new[] {typeof (char[]), typeof (StringSplitOptions)})))
            {
                var arg = (IEnumerable<char>) ((ConstantExpression) expression.Arguments[0]).Value;
                return ((ConstantExpression) expression.Arguments[1]).Value.Equals(StringSplitOptions.None) &&
                       arg.Count() == 1 && arg.First() == ' ';
            }
            return false;
        }

        public override void Visit(MethodCallExpression expression)
        {
            AqlFunction("word-tokens", expression.Object);
        }
    }
}