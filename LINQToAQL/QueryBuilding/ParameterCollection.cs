using System.Collections.Generic;
using System.Linq;

namespace LINQToAQL.QueryBuilding
{
    internal class ParameterizedQuery
    {
        public ParameterizedQuery(string statement, Parameter[] namedParameters)
        {
            Statement = statement;
            NamedParameters = namedParameters;
        }

        public string Statement { get; private set; }
        public Parameter[] NamedParameters { get; private set; }

        //public static ParameterizedQuery CreateQuery(string conString)// /con?
        //{
        //}
    }

    internal class Parameter
    {
        public Parameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
    }

    internal class ParameterCollection
    {
        private readonly IList<Parameter> _parameters = new List<Parameter>();

        public Parameter AddParameter(object value)
        {
            var parameter = new Parameter("p" + (_parameters.Count + 1), value);
            _parameters.Add(parameter);
            return parameter;
        }

        public Parameter[] GetParameters()
        {
            return _parameters.ToArray();
        }
    }
}