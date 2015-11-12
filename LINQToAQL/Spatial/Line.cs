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

namespace LINQToAQL.Spatial
{
    /// <summary>
    ///     An AQL <c>Line</c>
    /// </summary>
    public sealed class Line : IEquatable<Line>
    {
        /// <summary>
        ///     Creates an AQL <c>Line</c>
        /// </summary>
        /// <param name="first">The first of two points describing the line</param>
        /// <param name="second">The second of two points describing the line</param>
        public Line(Point first, Point second)
        {
            First = first;
            Second = second;
        }

        /// <summary>
        ///     Gets the first point describing the line
        /// </summary>
        /// <returns>The first point describing the line</returns>
        public Point First { get; }

        /// <summary>
        ///     Gets the second point describing the line
        /// </summary>
        /// <returns>The second point describing the line</returns>
        public Point Second { get; }

        /// <summary>
        ///     Determines whether the specified <see cref="Line" /> is equal to the current <see cref="Line" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="Line" /> is equal to the current <see cref="Line" />; otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="Line" /> to compare with the current <see cref="Line" /></param>
        public bool Equals(Line other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(First, other.First) && Equals(Second, other.Second);
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current <see cref="Line" />.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current <see cref="Line" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current <see cref="Line" /></param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Line && Equals((Line) obj);
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="Line" />.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((First?.GetHashCode() ?? 0)*397) ^ (Second?.GetHashCode() ?? 0);
            }
        }

        /// <summary>
        ///     Determines whether the two <see cref="Line" />s are equal.
        /// </summary>
        /// <param name="left">The first <see cref="Line" /> to compare.</param>
        /// <param name="right">The second <see cref="Line" /> to compare.</param>
        /// <returns>Whether the <see cref="Line" />s are equal.</returns>
        public static bool operator ==(Line left, Line right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Determines whether the two <see cref="Line" />s are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="Line" /> to compare.</param>
        /// <param name="right">The second <see cref="Line" /> to compare.</param>
        /// <returns>Whether the <see cref="Line" />s are not equal.</returns>
        public static bool operator !=(Line left, Line right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="Line" />.
        /// </summary>
        /// <returns>
        ///     A string that represents the current <see cref="Line" />.
        /// </returns>
        public override string ToString()
        {
            return $"First Point: [{First}], Second Point: [{Second}]";
        }
    }
}