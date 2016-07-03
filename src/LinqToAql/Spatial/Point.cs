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

namespace LinqToAql.Spatial
{
    /// <summary>
    ///     An AQL <c>Point</c>
    /// </summary>
    public sealed class Point : IEquatable<Point>
    {
        /// <summary>
        ///     Creates an AQL <c>Point</c>
        /// </summary>
        /// <param name="x">The x-coordinate</param>
        /// <param name="y">The y-coordinate</param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        ///     Gets the x-coordinate of the <c>Point</c>
        /// </summary>
        /// <returns>The x-coordinate</returns>
        public double X { get; }

        /// <summary>
        ///     Gets the y-coordinate of the <c>Point</c>
        /// </summary>
        /// <returns>The y-coordinate</returns>
        public double Y { get; }

        /// <summary>
        ///     Determines whether the specified <see cref="Point" /> is equal to the current <see cref="Point" />.
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="Point" /> is equal to the current <see cref="Point" />; otherwise, false.
        /// </returns>
        /// <param name="other">The <see cref="Point" /> to compare with the current <see cref="Point" /></param>
        public bool Equals(Point other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Math.Abs(X - X) < 0.000001 && Math.Abs(Y - Y) < 0.000001;
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current <see cref="Point" />.
        /// </summary>
        /// <returns>
        ///     true if the specified object  is equal to the current <see cref="Point" />; otherwise, false.
        /// </returns>
        /// <param name="obj">The object to compare with the current <see cref="Point" /></param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Point && Equals((Point) obj);
        }

        /// <summary>
        ///     Calculates the Euclidian distance between two <c>Point</c>s
        /// </summary>
        /// <param name="other">The other <c>Point</c></param>
        /// <returns>The Euclidian distance</returns>
        public double Distance(Point other)
        {
            throw new AsterixRemoteOnlyException();
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="Point" />.
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode()*397) ^ Y.GetHashCode();
            }
        }

        /// <summary>
        ///     Determines whether the two <see cref="Point" />s are equal.
        /// </summary>
        /// <param name="left">The first <see cref="Point" /> to compare.</param>
        /// <param name="right">The second <see cref="Point" /> to compare.</param>
        /// <returns>Whether the <see cref="Point" />s are equal.</returns>
        public static bool operator ==(Point left, Point right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Determines whether the two <see cref="Point" />s are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="Point" /> to compare.</param>
        /// <param name="right">The second <see cref="Point" /> to compare.</param>
        /// <returns>Whether the <see cref="Point" />s are not equal.</returns>
        public static bool operator !=(Point left, Point right)
        {
            return !Equals(left, right);
        }

        /// <summary>
        ///     Returns a string that represents the current <see cref="Point" />.
        /// </summary>
        /// <returns>
        ///     A string that represents the current <see cref="Point" />.
        /// </returns>
        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}