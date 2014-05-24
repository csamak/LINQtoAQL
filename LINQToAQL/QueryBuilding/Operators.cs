using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace LINQToAQL.QueryBuilding
{
    internal class Operators
    {
        public static readonly ReadOnlyDictionary<ExpressionType, string> Binary =
            new ReadOnlyDictionary<ExpressionType, string>(new Dictionary<ExpressionType, string>
            {
                {ExpressionType.Equal, " = "},
                {ExpressionType.AndAlso, " and "},
                {ExpressionType.And, " and "},
                {ExpressionType.OrElse, " or "},
                {ExpressionType.Or, " or "},
                {ExpressionType.Add, " + "},
                {ExpressionType.Subtract, " - "},
                {ExpressionType.Multiply, " * "},
                {ExpressionType.Divide, " / "},
                {ExpressionType.GreaterThan, " > "},
                {ExpressionType.LessThan, " < "},
                {ExpressionType.GreaterThanOrEqual, " >= "},
                {ExpressionType.LessThanOrEqual, " <= "},
                {ExpressionType.NotEqual, " != "},
            });
    }
}