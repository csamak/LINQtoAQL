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
        //}
        //{

        //public static ParameterizedQuery CreateQuery(string conString)// /con?
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