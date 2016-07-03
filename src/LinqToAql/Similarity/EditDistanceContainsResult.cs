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

namespace LinqToAql.Similarity
{
    /// <summary>
    ///     Represents a strongly typed version of the <c>OrderedList</c> result from <c>edit-distance-contains</c>.
    /// </summary>
    /// <seealso cref="EditDistanceExtensions" />
    public struct EditDistanceContainsResult
    {
        /// <summary>
        ///     Creates an <see cref="EditDistanceContainsResult" />
        /// </summary>
        /// <param name="contains">Whether the first expression contains the second within the threshold</param>
        /// <param name="editDistance">If <paramref name="contains" /> is true, the edit distance between the two, otherwise 0</param>
        public EditDistanceContainsResult(bool contains, int editDistance)
        {
            if (contains && editDistance != 0)
                throw new ArgumentException("editDistance must be 0 if contains is false");
            Contains = contains;
            EditDistance = editDistance;
        }

        /// <summary>
        ///     Whether the first expression contains the second within the threshold
        /// </summary>
        public readonly bool Contains;

        /// <summary>
        ///     The edit distance between the expressions when <see cref="Contains" /> is true
        /// </summary>
        public readonly int EditDistance;
    }
}