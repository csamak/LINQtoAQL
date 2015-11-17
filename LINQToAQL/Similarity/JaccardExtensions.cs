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

namespace LINQToAQL.Similarity
{
    /// <summary>
    ///     A collection of Jaccard similarity functions that are remotely evaluated by AsterixDB
    /// </summary>
    /// <remarks>
    ///     See http://asterixdb.ics.uci.edu/documentation/aql/functions.html for more information about individual
    ///     functions.
    /// </remarks>
    public static class JaccardExtensions
    {
        /// <summary>
        ///     Remotely calculates the Jaccard similarity between two <c>UnorderedList</c>s or <c>OrderedList</c>s
        /// </summary>
        /// <typeparam name="T">The type of the lists</typeparam>
        /// <param name="first">The first <c>UnorderedList</c> or <c>OrderedList</c></param>
        /// <param name="second">The second <c>UnorderedList</c> or <c>OrderedList</c></param>
        /// <returns>The Jaccard similarity</returns>
        public static double Jaccard<T>(this IEnumerable<T> first, IEnumerable<T> second)
        {
            throw new AsterixRemoteOnlyException();
        }

        //JaccardCheck support was intentionally removed since it is unnecessary with query rewrites.
    }
}