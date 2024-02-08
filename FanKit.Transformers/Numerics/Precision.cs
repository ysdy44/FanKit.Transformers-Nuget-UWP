// <copyright file="Precision.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2015 Math.NET
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

namespace FanKit.Transformers
{
    /// <summary>
    ///     Utilities for working with floating point numbers.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Useful links:
    ///         <list type="bullet">
    ///             <item>
    ///                 http://docs.sun.com/source/806-3568/ncg_goldberg.html#689 - What every computer scientist should know
    ///                 about floating-point arithmetic
    ///             </item>
    ///             <item>
    ///                 http://en.wikipedia.org/wiki/Machine_epsilon - Gives the definition of machine epsilon
    ///             </item>
    ///         </list>
    ///     </para>
    /// </remarks>
    partial class MathNetNumericsExtensions
    {

        /// <summary>
        ///     The number of binary digits used to represent the binary number for a double precision floating
        ///     point value. i.e. there are this many digits used to represent the
        ///     actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        private const int DoubleWidth = 53;

        /// <summary>

        /// <summary>
        ///     Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
        ///     According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// </summary>
        public static readonly double DoublePrecision = System.Math.Pow(2, -DoubleWidth);


        ///     Returns the magnitude of the number.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The magnitude of the number.</returns>
        public static int Magnitude(this double value)
        {
            // Can't do this with zero because the 10-log of zero doesn't exist.
            if (value.Equals(0.0)) return 0;

            // Note that we need the absolute value of the input because Log10 doesn't
            // work for negative numbers (obviously).
            double magnitude = System.Math.Log10(System.Math.Abs(value));
            int truncated = (int)System.Math.Truncate(magnitude);

            // To get the right number we need to know if the value is negative or positive
            // truncating a positive number will always give use the correct magnitude
            // truncating a negative number will give us a magnitude that is off by 1 (unless integer)
            return magnitude < 0d && truncated != magnitude
                ? truncated - 1
                : truncated;
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
            if (System.Math.Abs(a) < DoublePrecision || System.Math.Abs(b) < DoublePrecision)
                // The values are equal if the difference between the two numbers is smaller than
                // 10^(-numberOfDecimalPlaces). We divide by two so that we have half the range
                // on each side of the numbers, e.g. if decimalPlaces == 2,
                // then 0.01 will equal between 0.005 and 0.015, but not 0.02 and not 0.00
                return System.Math.Abs(diff) < System.Math.Pow(10, -decimalPlaces) / 2d;

            // If the magnitudes of the two numbers are equal to within one magnitude the numbers could potentially be equal
            int magnitudeOfFirst = Magnitude(a);
            int magnitudeOfSecond = Magnitude(b);
            int magnitudeOfMax = System.Math.Max(magnitudeOfFirst, magnitudeOfSecond);
            if (magnitudeOfMax > System.Math.Min(magnitudeOfFirst, magnitudeOfSecond) + 1) return false;

            // The values are equal if the difference between the two numbers is smaller than
            // 10^(-numberOfDecimalPlaces). We divide by two so that we have half the range
            // on each side of the numbers, e.g. if decimalPlaces == 2,
            // then 0.01 will equal between 0.00995 and 0.01005, but not 0.0015 and not 0.0095
            return System.Math.Abs(diff) < System.Math.Pow(10, magnitudeOfMax - decimalPlaces) / 2d;
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