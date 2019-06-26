﻿// <copyright file="SystemCrypto.cs" company="Math.NET">
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


using System.Runtime;
using System.Runtime.Serialization;
#if !NETSTANDARD1_3
using System;

#endif

namespace MathNet.Numerics.Random
{
    /// <summary>
    ///     A random number generator based on the <seecreSystem.Random class in the . NET library.
    /// </summary>
    [DataContract(Namespace = "urn:MathNet/Numerics/Random")]
    public class SystemRandomSource : RandomSource
    {
        [DataMember(Order = 1)] private readonly System.Random _random;

        /// <summary>
        ///     Construct a new random number generator with a random seed.
        /// </summary>
        public SystemRandomSource() : this(RandomSeed.Robust())
        {
        }

        /// <summary>
        ///     Construct a new random number generator with random seed.
        /// </summary>
        /// <param name="threadSafe">if set to <c>true</c> , the class is thread safe.</param>
        public SystemRandomSource(bool threadSafe) : this(RandomSeed.Robust(), threadSafe)
        {
        }

        /// <summary>
        ///     Construct a new random number generator with random seed.
        /// </summary>
        /// <param name="seed">The seed value.</param>
        public SystemRandomSource(int seed)
        {
            _random = new System.Random(seed);
        }

        /// <summary>
        ///     Construct a new random number generator with random seed.
        /// </summary>
        /// <param name="seed">The seed value.</param>
        /// <param name="threadSafe">if set to <c>true</c> , the class is thread safe.</param>
        public SystemRandomSource(int seed, bool threadSafe) : base(threadSafe)
        {
            _random = new System.Random(seed);
        }


        /// <summary>
        ///     Returns a random double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        protected sealed override double DoSample()
        {
            return _random.NextDouble();
        }

        /// <summary>
        ///     Returns a random 32-bit signed integer greater than or equal to zero and less than <seecreF:System.Int32.MaxValue
        /// </summary>
        protected override int DoSampleInteger()
        {
            return _random.Next();
        }

        /// <summary>
        ///     Returns a random 32-bit signed integer within the specified range.
        /// </summary>
        /// <param name="maxExclusive">
        ///     The exclusive upper bound of the random number returned. Range: maxExclusive ≥ 2 (not
        ///     verified, must be ensured by caller).
        /// </param>
        protected override int DoSampleInteger(int maxExclusive)
        {
            return _random.Next(maxExclusive);
        }

        /// <summary>
        ///     Returns a random 32-bit signed integer within the specified range.
        /// </summary>
        /// <param name="minInclusive">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxExclusive">
        ///     The exclusive upper bound of the random number returned. Range: maxExclusive ≥ minExclusive
        ///     + 2 (not verified, must be ensured by caller).
        /// </param>
        protected override int DoSampleInteger(int minInclusive, int maxExclusive)
        {
            return _random.Next(minInclusive, maxExclusive);
        }

        /// <summary>
        ///     Fills the elements of a specified array of bytes with random numbers in full range, including zero and 255 (
        ///     <seecreF:System.Byte.MaxValue).
        /// </summary>
        protected override void DoSampleBytes(byte[] buffer)
        {
            _random.NextBytes(buffer);
        }


        /// <summary>
        ///     Fills an array with random numbers greater than or equal to 0.0 and less than 1.0.
        /// </summary>
        /// <remarks>Supports being called in parallel from multiple threads.</remarks>
        public static void Doubles(double[] values, int seed)
        {
            var rnd = new System.Random(seed);
            for (var i = 0; i < values.Length; i++) values[i] = rnd.NextDouble();
        }

        /// <summary>
        ///     Returns an array of random numbers greater than or equal to 0.0 and less than 1.0.
        /// </summary>
        /// <remarks>Supports being called in parallel from multiple threads.</remarks>
        public static double[] Doubles(int length, int seed)
        {
            var data = new double[length];
            Doubles(data, seed);
            return data;
        }
    }
}