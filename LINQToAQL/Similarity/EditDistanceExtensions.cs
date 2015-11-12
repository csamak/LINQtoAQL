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
    ///     A collection of edit distance functions that are remotely evaluated by AsterixDB in <see cref="LINQToAQL" />
    /// </summary>
    /// <remarks>
    ///     See http://asterixdb.ics.uci.edu/documentation/aql/functions.html for more information about individual
    ///     functions.
    /// </remarks>
    public static class EditDistanceExtensions
    {
        /// <summary>
        ///     Remotely calculates the edit distance between the two <see cref="System.String" />s
        /// </summary>
        /// <param name="str">The first <see cref="System.String" /></param>
        /// <param name="other">The second <see cref="System.String" /></param>
        /// <returns>The edit distance</returns>
        public static int EditDistance(this string str, string other)
        {
            throw new AsterixRemoteOnlyException();
        }

        /// <summary>
        ///     Remotely calculates the edit distance between two <c>OrderedList</c>s
        /// </summary>
        /// <typeparam name="T">The item type, comparable by AsterixDB</typeparam>
        /// <param name="left">The first <c>OrderedList</c></param>
        /// <param name="other">The second <c>OrderedList</c></param>
        /// <remarks>
        ///     The <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> is evaluated as the <c>OrderedList</c>
        ///     referenced in the AQL documentation
        /// </remarks>
        /// <returns>The edit distance</returns>
        public static int EditDistance<T>(this IEnumerable<T> left, IEnumerable<T> other)
        {
            throw new AsterixRemoteOnlyException();
        }

        /// <summary>
        ///     Remotely checks whether the edit distance between two <see cref="System.String" />s is within a given threshold
        /// </summary>
        /// <param name="str">The first <see cref="System.String" /></param>
        /// <param name="other">The second <see cref="System.String" /></param>
        /// <param name="threshold">The threshold (maximum) edit distance for a <c>true</c> result</param>
        /// <returns>Whether the edit distance was within the threshold</returns>
        public static bool EditDistanceCheck(this string str, string other, int threshold)
        {
            throw new AsterixRemoteOnlyException();
        }

        /// <summary>
        ///     Remotely checks whether the edit distance between two <c>OrderedLists</c>s is within a given threshold
        /// </summary>
        /// <typeparam name="T">The item type, comparable by AsterixDB</typeparam>
        /// <param name="left">The first <c>OrderedList</c></param>
        /// <param name="other">The second <c>OrderedList</c></param>
        /// <param name="threshold">The threshold (maximum) edit distance for a <c>true</c> result</param>
        /// <remarks>
        ///     The <see cref="IEnumerable{T}" /> of <typeparamref name="T" /> is evaluated as the <c>OrderedList</c>
        ///     referenced in the AQL documentation
        /// </remarks>
        /// <returns>Whether the edit distance was within the threshold</returns>
        public static bool EditDistanceCheck<T>(this IEnumerable<T> left, IEnumerable<T> other, int threshold)
        {
            throw new AsterixRemoteOnlyException();
        }
    }
}