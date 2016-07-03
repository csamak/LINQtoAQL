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

namespace LinqToAql.Similarity
{
    /// <summary>
    ///     A collection of edit distance functions that are remotely evaluated by AsterixDB
    /// </summary>
    public static class EditDistanceExtensions
    {
        /// <summary>
        ///     Remotely calculates the edit distance between two <see cref="string" />s or <c>OrderedList</c>s
        /// </summary>
        /// <typeparam name="T">The item type, comparable by AsterixDB</typeparam>
        /// <param name="left">The first <c>OrderedList</c> or <see cref="string" /></param>
        /// <param name="other">The second <c>OrderedList</c> or <see cref="string" /></param>
        /// <remarks>
        ///     The <see cref="IEnumerable{T}" /> is evaluated as the string or homogenous <c>OrderedList</c> referenced in the AQL
        ///     documentation.
        ///     <para />
        ///     There is no suitable replacement interface to enforce ordering without some inconvience.
        ///     <see cref="IOrderedEnumerable{TElement}" /> exists, but is only used in limited cases.
        ///     <para />
        ///     Using this method with a local <see cref="IEnumerable{T}" /> that does not guarantee order (such as a
        ///     <see cref="HashSet{T}" />) is allowed but may have inconsistent behavior.
        ///     <para />
        ///     Using this method with a remote <c>UnorderedList</c> instead of an <c>OrderedList</c> results in the remote error
        ///     <c>AlgebricksException: Incompatible argument types given in edit distance: UNORDEREDLIST ORDEREDLIST</c>.
        /// </remarks>
        /// <returns>The edit distance</returns>
        public static int EditDistance<T>(this IEnumerable<T> left, IEnumerable<T> other)
        {
            throw new AsterixRemoteOnlyException();
        }

        //EditDistanceCheck support was intentionally removed since it is unnecessary with query rewrites.

        /// <summary>
        ///     Remotely checks whether one <see cref="string" /> or <c>OrderedList</c> contains another within a given threshold
        /// </summary>
        /// <typeparam name="T">The item type, comparable by AsterixDB</typeparam>
        /// <param name="left">The first <c>OrderedList</c> or <see cref="string" /></param>
        /// <param name="other">The second <c>OrderedList</c> or <see cref="string" /></param>
        /// <param name="threshold">The threshold (maximum) edit distance for a <c>true</c> result</param>
        /// <remarks>
        ///     The <see cref="IEnumerable{T}" /> is evaluated as the string or homogenous <c>OrderedList</c> referenced in the AQL
        ///     documentation.
        ///     <para />
        ///     There is no suitable replacement interface to enforce ordering without some inconvience.
        ///     <see cref="IOrderedEnumerable{T}" /> exists, but is only used in limited cases.
        ///     <para />
        ///     Using this method with a local <see cref="IEnumerable{T}" /> that does not guarantee order (such as a
        ///     <see cref="HashSet{T}" />) is allowed but may have inconsistent behavior.
        ///     <para />
        ///     Using this method with a remote <c>UnorderedList</c> instead of an <c>OrderedList</c> results in the remote error
        ///     <c>AlgebricksException: Incompatible argument types given in edit distance: UNORDEREDLIST ORDEREDLIST</c>
        /// </remarks>
        /// <returns>An <see cref="EditDistanceContainsResult" /> representing the edit distance and whether it meets the threshold</returns>
        public static EditDistanceContainsResult EditDistanceContains<T>(this IEnumerable<T> left, IEnumerable<T> other,
            int threshold)
        {
            throw new AsterixRemoteOnlyException();
        }
    }
}