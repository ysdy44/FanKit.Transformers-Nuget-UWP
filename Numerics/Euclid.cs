// <copyright file="Euclid.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2014 Math.NET
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
    /// <summary>
    ///     Integer number theory functions.
    /// </summary>
    public static class Euclid
    {
        private static readonly int[] MultiplyDeBruijnBitPosition = new int[32]
        {
            0, 9, 1, 10, 13, 21, 2, 29, 11, 14, 16, 18, 22, 25, 3, 30,
            8, 12, 20, 28, 15, 17, 24, 7, 19, 27, 23, 6, 26, 5, 4, 31
        };


        public static double Modulus(double dividend, double divisor)
        {
            return (dividend % divisor + divisor) % divisor;
        }


        public static double Remainder(double dividend, double divisor)
        {
            return dividend % divisor;
        }



        /// <summary>
        ///     Raises 2 to the provided integer exponent (0 &lt;= exponent &lt; 31).
        /// </summary>
        /// <param name="exponent">The exponent to raise 2 up to.</param>
        /// <returns>2 ^ exponent.</returns>
        /// <exceptioncreArgumentOutOfRangeException" />
        public static int PowerOfTwo(this int exponent)
        {
            if (exponent < 0 || exponent >= 31) throw new ArgumentOutOfRangeException(nameof(exponent));

            return 1 << exponent;
        }


        /// <summary>
        ///     Evaluate the binary logarithm of an integer number.
        /// </summary>
        /// <remarks>Two-step method using a De Bruijn-like sequence table lookup.</remarks>
        public static int Log2(this int number)
        {
            number |= number >> 1;
            number |= number >> 2;
            number |= number >> 4;
            number |= number >> 8;
            number |= number >> 16;

            return MultiplyDeBruijnBitPosition[(uint) (number * 0x07C4ACDDU) >> 27];
        }
    }
}