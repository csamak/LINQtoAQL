﻿// Licensed to the Apache Software Foundation (ASF) under one
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

namespace LinqToAql.DataAnnotations
{
    /// <summary>
    ///     An <see cref="Attribute" /> that describes how to handle a dataset when generating AQL
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class DatasetAttribute : Attribute
    {
        /// <summary>
        ///     The dataset name to use when generating an AQL query
        /// </summary>
        /// <returns>The dataset name</returns>
        public string Name { get; set; }

        /// <summary>
        ///     Whether the type associated with the dataset is an open type
        /// </summary>
        /// <returns>Whether the type is open</returns>
        public bool Open { get; set; }
    }
}