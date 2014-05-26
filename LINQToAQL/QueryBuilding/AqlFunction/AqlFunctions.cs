using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using LINQToAQL.QueryBuilding.AqlFunction.Numeric;
using LINQToAQL.QueryBuilding.AqlFunction.Similarity;
using LINQToAQL.QueryBuilding.AqlFunction.String;

namespace LINQToAQL.QueryBuilding.AqlFunction
{
    internal class AqlFunctions
    {
        public readonly ReadOnlyCollection<AqlFunctionVisitor> Functions;

        public AqlFunctions(StringBuilder aqlExpression, AqlExpressionVisitor visitor)
        {
            Functions =
                new ReadOnlyCollection<AqlFunctionVisitor>(
                    new[]
                    {
                        typeof (Abs), typeof (Ceiling), typeof (Floor), typeof (Round), typeof (CharIndex),
                        typeof (Contains), typeof (EndsWith), typeof (Join), typeof (Lowercase), typeof (StartsWith),
                        typeof (Substring), typeof (SubstringWithLength), typeof (ToCodepoint), typeof (EditDistance),
                        typeof (EditDistanceCheck), typeof (Jaccard), typeof (JaccardCheck)
                    }.Select(
                        t => Activator.CreateInstance(t, aqlExpression, visitor))
                        .Cast<AqlFunctionVisitor>()
                        .ToList());
        }
    }
}