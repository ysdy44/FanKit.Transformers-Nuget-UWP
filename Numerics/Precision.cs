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
using System.Runtime;
using System.Runtime.InteropServices;

namespace MathNet.Numerics
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
    public static partial class Precision
    {
        /// <summary>
        ///     The number of binary digits used to represent the binary number for a double precision floating
        ///     point value. i.e. there are this many digits used to represent the
        ///     actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        private const int DoubleWidth = 53;

        /// <summary>
        ///     The number of binary digits used to represent the binary number for a single precision floating
        ///     point value. i.e. there are this many digits used to represent the
        ///     actual number, where in a number as: 0.134556 * 10^5 the digits are 0.134556 and the exponent is 5.
        /// </summary>
        private const int SingleWidth = 24;

        /// <summary>
        ///     Standard epsilon, the maximum relative precision of IEEE 754 double-precision floating numbers (64 bit).
        ///     According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// </summary>
        public static readonly double DoublePrecision = Math.Pow(2, -DoubleWidth);

        /// <summary>
        ///     Standard epsilon, the maximum relative precision of IEEE 754 single-precision floating numbers (32 bit).
        ///     According to the definition of Prof. Demmel and used in LAPACK and Scilab.
        /// </summary>
        public static readonly double SinglePrecision = Math.Pow(2, -SingleWidth);


        /// <summary>
        ///     Value representing 10 * 2^(-53) = 1.11022302462516E-15
        /// </summary>
        private static readonly double DefaultDoubleAccuracy = DoublePrecision * 10;

        /// <summary>
        ///     Value representing 10 * 2^(-24) = 5.96046447753906E-07
        /// </summary>
        private static readonly float DefaultSingleAccuracy = (float) (SinglePrecision * 10);

        /// <summary>
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
            var magnitude = Math.Log10(Math.Abs(value));
            var truncated = (int) Truncate(magnitude);

            // To get the right number we need to know if the value is negative or positive
            // truncating a positive number will always give use the correct magnitude
            // truncating a negative number will give us a magnitude that is off by 1 (unless integer)
            return magnitude < 0d && truncated != magnitude
                ? truncated - 1
                : truncated;
        }


        /// <summary>
        ///     Increments a floating point number to the next bigger number representable by the data type.
        /// </summary>
        /// <param name="value">The value which needs to be incremented.</param>
        /// <param name="count">How many times the number should be incremented.</param>
        /// <remarks>
        ///     The incrementation step length depends on the provided value.
        ///     Increment(double.MaxValue) will return positive infinity.
        /// </remarks>
        /// <returns>The next larger floating point value.</returns>
        public static double Increment(this double value, int count = 1)
        {
            if (double.IsInfinity(value) || double.IsNaN(value) || count == 0) return value;

            if (count < 0) return Decrement(value, -count);

            // Translate the bit pattern of the double to an integer.
            // Note that this leads to:
            // double > 0 --> long > 0, growing as the double value grows
            // double < 0 --> long < 0, increasing in absolute magnitude as the double
            //                          gets closer to zero!
            //                          i.e. 0 - double.epsilon will give the largest long value!
            var intValue = BitConverter.DoubleToInt64Bits(value);
            if (intValue < 0)
                intValue -= count;
            else
                intValue += count;

            // Note that long.MinValue has the same bit pattern as -0.0.
            if (intValue == long.MinValue) return 0;

            // Note that not all long values can be translated into double values. There's a whole bunch of them
            // which return weird values like infinity and NaN
            return BitConverter.Int64BitsToDouble(intValue);
        }

        /// <summary>
        ///     Decrements a floating point number to the next smaller number representable by the data type.
        /// </summary>
        /// <param name="value">The value which should be decremented.</param>
        /// <param name="count">How many times the number should be decremented.</param>
        /// <remarks>
        ///     The decrementation step length depends on the provided value.
        ///     Decrement(double.MinValue) will return negative infinity.
        /// </remarks>
        /// <returns>The next smaller floating point value.</returns>
        public static double Decrement(this double value, int count = 1)
        {
            if (double.IsInfinity(value) || double.IsNaN(value) || count == 0) return value;

            if (count < 0) return Decrement(value, -count);

            // Translate the bit pattern of the double to an integer.
            // Note that this leads to:
            // double > 0 --> long > 0, growing as the double value grows
            // double < 0 --> long < 0, increasing in absolute magnitude as the double
            //                          gets closer to zero!
            //                          i.e. 0 - double.epsilon will give the largest long value!
            var intValue = BitConverter.DoubleToInt64Bits(value);

            // If the value is zero then we'd really like the value to be -0. So we'll make it -0
            // and then everything else should work out.
            if (intValue == 0)
                // Note that long.MinValue has the same bit pattern as -0.0.
                intValue = long.MinValue;

            if (intValue < 0)
                intValue += count;
            else
                intValue -= count;

            // Note that not all long values can be translated into double values. There's a whole bunch of them
            // which return weird values like infinity and NaN
            return BitConverter.Int64BitsToDouble(intValue);
        }


        /// <summary>
        ///     Evaluates the minimum distance to the next distinguishable number near the argument value.
        /// </summary>
        /// <param name="value">The value used to determine the minimum distance.</param>
        /// <returns>
        ///     Relative Epsilon (positive double or NaN).
        /// </returns>
        /// <remarks>
        ///     Evaluates the <b>negative</b> epsilon. The more common positive epsilon is equal to two times this negative
        ///     epsilon.
        /// </remarks>
        /// <seealsocrePositiveEpsilonOf( double)" />
        public static double EpsilonOf(this double value)
        {
            if (double.IsInfinity(value) || double.IsNaN(value)) return double.NaN;

            var signed64 = BitConverter.DoubleToInt64Bits(value);
            if (signed64 == 0)
            {
                signed64++;
                return BitConverter.Int64BitsToDouble(signed64) - value;
            }

            if (signed64-- < 0) return BitConverter.Int64BitsToDouble(signed64) - value;
            return value - BitConverter.Int64BitsToDouble(signed64);
        }


        private static double Truncate(double value)
        {
            return Math.Truncate(value);
        }


    }
}