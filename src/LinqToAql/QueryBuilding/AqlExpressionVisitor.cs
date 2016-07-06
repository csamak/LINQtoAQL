// Licensed to the Apache Software Foundation (ASF) under one
// or more contributor license agreements.  See the NOTICE file
// distributed with this work for additional information
// regarding copyright ownership.  The ASF licenses this file
// to you under the Apache License, Version 2.0 (the
// "License"); you may not use this file except in compliance
// with the License.  You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing,
// software distributed under the License is distributed on an
// "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
// KIND, either express or implied.  See the License for the
// specific language governing permissions and limitations
// under the License.

using System;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using LinqToAql.DataAnnotations;
using LinqToAql.Extensions;
using LinqToAql.QueryBuilding.AqlConstructors;
using LinqToAql.QueryBuilding.AqlFunctions;
using LinqToAql.Spatial;
using Remotion.Linq.Clauses;
using Remotion.Linq.Clauses.Expressions;
using Remotion.Linq.Clauses.ResultOperators;
using Remotion.Linq.Parsing;
using Point = LinqToAql.Spatial.Point;

namespace LinqToAql.QueryBuilding
{
    internal class AqlExpressionVisitor : ThrowingExpressionVisitor
    {
        private readonly StringBuilder _aqlExpression = new StringBuilder();
        private readonly SingleExpressionVisitorFactory<AqlFunctionVisitor, MethodCallExpression> _aqlFunctionVisitorFactory;
        private readonly SingleExpressionVisitorFactory<AqlConstructorVisitor, NewExpression> _aqlConstructorNewVisitorFactory;
        private readonly SingleExpressionVisitorFactory<AqlConstructorVisitor, ConstantExpression> _aqlConstructorConstantVisitorFactory;

        private AqlExpressionVisitor()
        {
            //When UDF support is added, a public API can be exposed through the context to allow
            //registration to this factory
            _aqlFunctionVisitorFactory = new AqlFunctionFactory();
            _aqlConstructorNewVisitorFactory = new AqlConstructorNewExpressionFactory();
            _aqlConstructorConstantVisitorFactory = new AqlConstructorConstantExpressionFactory();
        }

        public static string GetAqlExpression(Expression linqExpression)
        {
            var visitor = new AqlExpressionVisitor();
            visitor.Visit(linqExpression);
            return visitor.GetAqlExpression();
        }

        private string GetAqlExpression()
        {
            return _aqlExpression.ToString();
        }

        protected override Expression VisitQuerySourceReference(QuerySourceReferenceExpression expression)
        {
            _aqlExpression.Append("$" + expression.ReferencedQuerySource.ItemName);
            return expression;
        }

        protected override Expression VisitBinary(BinaryExpression expression)
        {
            if (expression.Right.NodeType == ExpressionType.Constant &&
                ((ConstantExpression) expression.Right).Value == null)
                VisitNullComparison(expression.NodeType, expression.Left);
            else if (expression.Left.NodeType == ExpressionType.Constant &&
                     ((ConstantExpression) expression.Left).Value == null)
                VisitNullComparison(expression.NodeType, expression.Right);
            else if (expression.NodeType == ExpressionType.ArrayIndex)
            {
                Visit(expression.Left);
                _aqlExpression.Append("[");
                Visit(expression.Right);
                _aqlExpression.Append("]");
            }
            else
            {
                _aqlExpression.Append("(");
                Visit(expression.Left);
                string op;
                if (Operators.Binary.TryGetValue(expression.NodeType, out op))
                    _aqlExpression.Append(op);
                else
                    base.VisitBinary(expression);
                Visit(expression.Right);
                _aqlExpression.Append(")");
            }
            return expression;
        }

        private void VisitNullComparison(ExpressionType comparisonType, Expression toCompare)
        {
            switch (comparisonType)
            {
                case ExpressionType.Equal:
                    _aqlExpression.Append("is-null(");
                    Visit(toCompare);
                    _aqlExpression.Append(")");
                    break;
                case ExpressionType.NotEqual:
                    _aqlExpression.Append("not(is-null(");
                    Visit(toCompare);
                    _aqlExpression.Append("))");
                    break;
                default:
                    throw new NotImplementedException("Can only compare null using != or ==");
            }
        }

        protected override Expression VisitMember(MemberExpression expression)
        {
            if (expression.Member.Name == "Key" &&
                expression.Expression.Type.GetGenericTypeDefinition() == typeof(IGrouping<,>)) //grouping
            {
                var temp =
                    GetKeySelector(
                        (SubQueryExpression)
                            ((MainFromClause)
                                ((QuerySourceReferenceExpression) expression.Expression).ReferencedQuerySource)
                                .FromExpression);
                Visit(expression.Expression);
                var field = temp.Expression.Type.GetTypeInfo().GetMember(temp.Member.Name)
                    .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                    .GetAttributeValue((FieldAttribute f) => f.Name);
                _aqlExpression.Append($"[0].{field ?? temp.Member.Name}");
            }
            else if (expression.Member.Name == "Length" && expression.Member.DeclaringType == typeof(string))
            {
                _aqlExpression.Append("string-length(");
                Visit(expression.Expression);
                _aqlExpression.Append(")");
            }
            else
            {
                Visit(expression.Expression);
                var field = expression.Expression.Type.GetTypeInfo().GetMember(expression.Member.Name)
                    .First(m => m.MemberType == MemberTypes.Property || m.MemberType == MemberTypes.Field)
                    .GetAttributeValue((FieldAttribute f) => f.Name);
                _aqlExpression.Append($".{field ?? expression.Member.Name}");
            }
            return expression;
        }

        protected override Expression VisitMemberInit(MemberInitExpression expression)
        {
            _aqlExpression.Append("{");
            for (var i = 0; i < expression.Bindings.Count; i++)
            {
                var assignment = expression.Bindings[i] as MemberAssignment;
                if (assignment == null)
                    return base.VisitMemberInit(expression);
                _aqlExpression.Append($" \"{assignment.Member.Name}\": ");
                Visit(assignment.Expression);
                if (i < expression.Bindings.Count - 1) //trailing comma not legal
                    _aqlExpression.Append(',');
            }
            _aqlExpression.Append(" }");
            return expression;
        }

        //should we search secondary from clauses?
        private MemberExpression GetKeySelector(SubQueryExpression exp)
        {
            var temp = exp.QueryModel.ResultOperators.FirstOrDefault(r => r is GroupResultOperator);
            return temp == null
                ? GetKeySelector((SubQueryExpression) exp.QueryModel.MainFromClause.FromExpression)
                : (MemberExpression) ((GroupResultOperator) temp).KeySelector;
        }

        protected override Expression VisitConstant(ConstantExpression expression)
        {
            var constructor = _aqlConstructorConstantVisitorFactory.CreateVisitableVisitor(expression, _aqlExpression, this);
            if (constructor != null)
                constructor.Visit(expression);
            else if (expression.Value is IEnumerable)
            {
                _aqlExpression.Append("[");
                var val = ((IEnumerable) expression.Value).Cast<object>().ToList();
                Visit(Expression.Constant(val.First()));
                foreach (var curr in val.Skip(1))
                {
                    _aqlExpression.Append(",");
                    Visit(Expression.Constant(curr));
                }
                _aqlExpression.Append("]");
            }
            else if (expression.Value == null)
                _aqlExpression.Append("null");
            else
                _aqlExpression.Append(expression.Value);
            return expression;
        }

        protected override Expression VisitMethodCall(MethodCallExpression expression)
        {
            var function = _aqlFunctionVisitorFactory.CreateVisitableVisitor(expression, _aqlExpression, this);
            if (function != null)
            {
                function.Visit(expression);
                return expression;
            }
            return base.VisitMethodCall(expression);
        }

        protected override Expression VisitNew(NewExpression expression)
        {
            var constructor = _aqlConstructorNewVisitorFactory.CreateVisitableVisitor(expression, _aqlExpression, this);
            if (constructor != null)
                constructor.Visit(expression);
            else
            {
                _aqlExpression.Append("{");
                for (var i = 0; i < expression.Arguments.Count; i++)
                {
                    _aqlExpression.Append($" \"{expression.Members[i].Name}\": ");
                    Visit(expression.Arguments[i]);
                    if (i < expression.Arguments.Count - 1)
                        _aqlExpression.Append(',');
                }
                _aqlExpression.Append(" }");
            }
            return expression;
        }

        protected override Expression VisitNewArray(NewArrayExpression expression)
        {
            _aqlExpression.Append("[");
            foreach (var curr in expression.Expressions)
            {
                Visit(curr);
                _aqlExpression.Append(", ");
            }
            _aqlExpression.Append("]");
            return expression;
        }

        protected override Expression VisitUnary(UnaryExpression expression)
        {
            if (expression.NodeType == ExpressionType.Convert)
            {
                //are more intelligent converts necessary?
                Visit(expression.Operand);
            }
            else
            {
                _aqlExpression.Append(expression.NodeType.ToString().ToLower());
                _aqlExpression.Append('(');
                Visit(expression.Operand);
                _aqlExpression.Append(')');
            }

            return expression;
        }

        protected override Expression VisitSubQuery(SubQueryExpression expression)
        {
            //is this fragile?
            _aqlExpression.Append(AqlQueryGenerator.GenerateAqlQuery(expression.QueryModel, true));
            return expression;
        }

        protected override Exception CreateUnhandledItemException<T>(T unhandledItem, string visitMethod)
        {
            return
                new NotSupportedException(
                    $"The expression [{unhandledItem}] with type [{typeof(T)}] and method [{visitMethod}] is not supported.");
        }
    }
}