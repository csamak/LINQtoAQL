﻿using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace LINQToAQL.QueryBuilding
{
    internal class AqlFunctionVisitor
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
            if (typeof (Math).GetMethods().Where(m => m.Name == "Abs").Contains(expression.Method))
                SingleArg("numeric-abs", expression.Arguments[0]);
            else if (typeof (Math).GetMethods().Where(m => m.Name == "Ceiling").Contains(expression.Method))
                SingleArg("numeric-ceiling", expression.Arguments[0]);
            else if (typeof (Math).GetMethods().Where(m => m.Name == "Floor").Contains(expression.Method))
                SingleArg("numeric-floor", expression.Arguments[0]);
                //Math.Round defaults to MidpointRounding.ToEven, which is equivalent to numeric-round-half-to-even
            else if (
                new[]
                {
                    typeof (Math).GetMethod("Round", new[] {typeof (double)}),
                    typeof (Math).GetMethod("Round", new[] {typeof (decimal)})
                }.Contains(expression.Method) ||
                (new[]
                {
                    typeof (Math).GetMethod("Round", new[] {typeof (double), typeof (MidpointRounding)}),
                    typeof (Math).GetMethod("Round", new[] {typeof (decimal), typeof (MidpointRounding)})
                }.Contains(
                    expression.Method) &&
                 ((ConstantExpression) expression.Arguments[1]).Value.Equals(MidpointRounding.ToEven)))
                SingleArg("numeric-round-half-to-even", expression.Arguments[0]);
            else if (expression.Method.Equals(typeof (string).GetMethod("ToCharArray", new Type[0])))
                SingleArg("string-to-codepoint", expression.Object);
                //shouldn't use name comparison?
            else if (expression.Method.IsSpecialName && expression.Method.Name == "get_Chars" &&
                     expression.Method.DeclaringType.FullName == "System.String")
            {
                _aqlExpression.Append("string-to-codepoint(");
                _visitor.VisitExpression(expression.Object);
                _aqlExpression.Append(")[");
                _aqlExpression.Append(expression.Arguments[0]);
                _aqlExpression.Append("]");
            }
            else if (expression.Method.Equals(typeof(string).GetMethod("Contains")))
                ManyArgs("contains", expression.Object, expression.Arguments[0]);
            else
                return false;
            return true;
        }

        protected void SingleArg(string name, Expression argument)
        {
            _aqlExpression.AppendFormat("{0}(", name);
            _visitor.VisitExpression(argument);
            _aqlExpression.Append(")");
        }

        protected void ManyArgs(string name, params Expression[] args)
        {
            _aqlExpression.AppendFormat("{0}(", name);
            _visitor.VisitExpression(args[0]);
            foreach (var arg in args.Skip(1))
            {
                _aqlExpression.Append(", ");
                _visitor.VisitExpression(arg);
            }
            _aqlExpression.Append(")");
        }
    }
}