// <copyright file="Precision.Equality.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2013 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;
using System.Numerics;

namespace MathNet.Numerics
{
    // TODO PERF: Cache/Precompute 10^x terms

    public static partial class Precision
    {
        /// <summary>
        ///     Compares two doubles and determines if they are equal
        ///     within the specified maximum absolute error.
        /// </summary>
        /// <param name="a">The norm of the first value (can be negative).</param>
        /// <param name="b">The norm of the second value (can be negative).</param>
        /// <param name="diff">The norm of the difference of the two values (can be negative).</param>
        /// <param name="maximumAbsoluteError">The absolute accuracy required for being almost equal.</param>
        /// <returns>True if both doubles are almost equal up to the specified maximum absolute error, false otherwise.</returns>
        public static bool AlmostEqualNorm(this double a, double b, double diff, double maximumAbsoluteError)
        {
            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b)) return a == b;

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b)) return false;

            return Math.Abs(diff) < maximumAbsoluteError;
        }


        /// <summary>
        ///     Checks whether two real numbers are almost equal.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>true if the two values differ by no more than 10 * 2^(-52); false otherwise.</returns>
        public static bool AlmostEqual(this double a, double b)
        {
            return AlmostEqualNorm(a, b, a - b, DefaultDoubleAccuracy);
        }

        /// <summary>
        ///     Checks whether two real numbers are almost equal.
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>true if the two values differ by no more than 10 * 2^(-52); false otherwise.</returns>
        public static bool AlmostEqual(this float a, float b)
        {
            return AlmostEqualNorm(a, b, a - b, DefaultSingleAccuracy);
        } 

        /// <summary>
        ///     Compares two doubles and determines if they are equal to within the specified number of decimal places or not. If
        ///     the numbers
        ///     are very close to zero an absolute difference is compared, otherwise the relative difference is compared.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         The values are equal if the difference between the two numbers is smaller than 10^(-numberOfDecimalPlaces). We
        ///         divide by
        ///         two so that we have half the range on each side of the numbers, e.g. if <paramref name="decimalPlaces" /> == 2,
        ///         then 0.01 will equal between
        ///         0.005 and 0.015, but not 0.02 and not 0.00
        ///     </para>
        /// </remarks>
        /// <param name="a">The norm of the first value (can be negative).</param>
        /// <param name="b">The norm of the second value (can be negative).</param>
        /// <param name="diff">The norm of the difference of the two values (can be negative).</param>
        /// <param name="decimalPlaces">The number of decimal places.</param>
        /// <exceptioncreArgumentOutOfRangeException">Thrown if 
        /// 
        /// <paramref name="decimalPlaces" />
        /// is smaller than zero.
        /// </exception>
        public static bool AlmostEqualNormRelative(this double a, double b, double diff, int decimalPlaces)
        {
            if (decimalPlaces < 0)
                // Can't have a negative number of decimal places
                throw new ArgumentOutOfRangeException(nameof(decimalPlaces));

            // If A or B are a NAN, return false. NANs are equal to nothing,
            // not even themselves.
            if (double.IsNaN(a) || double.IsNaN(b)) return false;

            // If A or B are infinity (positive or negative) then
            // only return true if they are exactly equal to each other -
            // that is, if they are both infinities of the same sign.
            if (double.IsInfinity(a) || double.IsInfinity(b)) return a == b;

            // If both numbers are equal, get out now. This should remove the possibility of both numbers being zero
            // and any problems associated with that.
            if (a.Equals(b)) return true;

            // If one is almost zero, fall back to absolute equality
            if (Math.Abs(a) < DoublePrecision || Math.Abs(b) < DoublePrecision)
                // The values are equal if the difference between the two numbers is smaller than
                // 10^(-numberOfDecimalPlaces). We divide by two so that we have half the range
                // on each side of the numbers, e.g. if decimalPlaces == 2,
                // then 0.01 will equal between 0.005 and 0.015, but not 0.02 and not 0.00
                return Math.Abs(diff) < Math.Pow(10, -decimalPlaces) / 2d;

            // If the magnitudes of the two numbers are equal to within one magnitude the numbers could potentially be equal
            var magnitudeOfFirst = Magnitude(a);
            var magnitudeOfSecond = Magnitude(b);
            var magnitudeOfMax = Math.Max(magnitudeOfFirst, magnitudeOfSecond);
            if (magnitudeOfMax > Math.Min(magnitudeOfFirst, magnitudeOfSecond) + 1) return false;

            // The values are equal if the difference between the two numbers is smaller than
            // 10^(-numberOfDecimalPlaces). We divide by two so that we have half the range
            // on each side of the numbers, e.g. if decimalPlaces == 2,
            // then 0.01 will equal between 0.00995 and 0.01005, but not 0.0015 and not 0.0095
            return Math.Abs(diff) < Math.Pow(10, magnitudeOfMax - decimalPlaces) / 2d;
        }


        /// <summary>
        ///     Compares two doubles and determines if they are equal to within the specified number of decimal places or not. If
        ///     the numbers
        ///     are very close to zero an absolute difference is compared, otherwise the relative difference is compared.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <param name="decimalPlaces">The number of decimal places.</param>
        public static bool AlmostEqualRelative(this double a, double b, int decimalPlaces)
        {
            return AlmostEqualNormRelative(a, b, a - b, decimalPlaces);
        }
    }
}