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
using System.Linq;
using System.Reflection;
using LINQToAQL.DataAnnotations;
using LINQToAQL.Extensions;

namespace LINQToAQL
{
    /// <summary>
    ///     Creates a context which acts as an entry point for interacting with an <c>AsterixDB</c> dataverse
    /// </summary>
    /// <remarks>
    ///     <see cref="AsterixContext" />s should be lightweight and cheap to create.
    ///     <para />
    ///     When an instance is constructed, public <see cref="IDataset{T}" /> fields and properties with setters are
    ///     initialized to a new <see cref="Dataset{T}" />. The property setter may have any accessibility.
    /// </remarks>
    public abstract class AsterixContext
    {
        private readonly string _dataverse;

        /// <summary>
        ///     Creates an <see cref="AsterixContext" /> and initializes public <see cref="IDataset{T}" /> fields and properties
        ///     with setters.
        /// </summary>
        /// <param name="asterixDbEndpoint">The URI to use when connecting to AsterixDB (e.g. <c>http://localhost:19002/</c>)</param>
        /// <param name="dataverse">The dataverse associated with the context</param>
        protected AsterixContext(Uri asterixDbEndpoint, string dataverse = null)
        {
            AsterixDbEndpoint = asterixDbEndpoint;
            _dataverse = dataverse;
            InitDatasets();
        }

        /// <summary>
        ///     The URI used when connecting to AsterixDB
        /// </summary>
        public Uri AsterixDbEndpoint { get; }

        /// <summary>
        ///     The dataverse name associated with the context. In precedence order, the following is used to determine the
        ///     dataverse name:
        ///     <list type="number">
        ///         <item>
        ///             <description>If provided, the constructor's <c>dataverse</c> parameter</description>
        ///         </item>
        ///         <item>
        ///             <description>
        ///                 If the class is marked with a <see cref="DataverseAttribute" />, the <c>Name</c> property of
        ///                 the attribute
        ///             </description>
        ///         </item>
        ///         <item>
        ///             <description>The name of <c>this</c> class</description>
        ///         </item>
        ///     </list>
        /// </summary>
        public string Dataverse
            => _dataverse ?? GetType().GetAttributeValue((DataverseAttribute d) => d.Name) ?? GetType().Name;

        //For every public Dataset field or settable property that has not been set, assign it a queryable Dataset
        private void InitDatasets()
        {
            foreach (
                FieldInfo field in
                    GetType()
                        .GetFields(BindingFlags.Public | BindingFlags.Instance)
                        .Where(f => f.GetValue(this) == null && f.FieldType.IsGenericType))
            {
                var dataset = GetDatasetForType(field.FieldType);
                if (dataset != null)
                    field.SetValue(this, dataset);
            }
            foreach (
                PropertyInfo prop in
                    GetType()
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Where(p => p.CanWrite && p.GetValue(this) == null && p.PropertyType.IsGenericType))
            {
                var dataset = GetDatasetForType(prop.PropertyType);
                if (dataset != null)
                    prop.SetValue(this, dataset);
            }
        }

        private object GetDatasetForType(Type type)
        {
            Type genType = type.GetGenericTypeDefinition();
            if (genType == typeof (Dataset<>))
                return Activator.CreateInstance(type, AsterixDbEndpoint, Dataverse, this);
            if (genType == typeof (IDataset<>))
            {
                var concreteType = typeof (Dataset<>).MakeGenericType(type.GenericTypeArguments[0]);
                return Activator.CreateInstance(concreteType, AsterixDbEndpoint, Dataverse, this);
            }
            return null;
        }
    }
}