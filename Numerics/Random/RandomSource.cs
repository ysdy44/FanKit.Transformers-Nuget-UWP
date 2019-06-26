// <copyright file="AbstractRandomNumberGenerator.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2016 Math.NET
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
using System.Runtime.Serialization;

namespace MathNet.Numerics.Random
{
    /// <summary>
    ///     Base class for random number generators. This class introduces a layer between
    ///     <seecreSystem.Random
    ///         and the Math.Net Numerics random number generators to provide thread safety.
    ///         When used directly it use the System.Random as random number source.
    /// </summary>
    [DataContract(Namespace = "urn:MathNet/Numerics/Random")]
    public abstract class RandomSource : System.Random
    {
        private readonly object _lock = new object();
        private readonly bool _threadSafe;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <seecreRandomSource class using
        ///         the value of
        ///     <seecreControl.ThreadSafeRandomNumberGenerators to set whether
        ///         the instance is thread safe or not.
        /// </summary>
        protected RandomSource() : base(RandomSeed.Robust())
        {
            _threadSafe = Control.ThreadSafeRandomNumberGenerators;
        }

        /// <summary>
        ///     Initializes a new instance of the <seecreRandomSource class.
        /// </summary>
        /// <param name="threadSafe">if set to <c>true</c> , the class is thread safe.</param>
        /// <remarks>
        ///     Thread safe instances are two and half times slower than non-thread
        ///     safe classes.
        /// </remarks>
        protected RandomSource(bool threadSafe) : base(RandomSeed.Robust())
        {
            _threadSafe = threadSafe;
        }


        /// <summary>
        ///     Returns a random 32-bit signed integer greater than or equal to zero and less than <seecreF:System.Int32.MaxValue.
        /// </summary>
        public sealed override int Next()
        {
            if (_threadSafe)
                lock (_lock)
                {
                    return DoSampleInteger();
                }

            return DoSampleInteger();
        }

        /// <summary>
        ///     Returns a random number less then a specified maximum.
        /// </summary>
        /// <param name="maxExclusive">The exclusive upper bound of the random number returned. Range: maxExclusive ≥ 1.</param>
        /// <returns>A 32-bit signed integer less than <paramref name="maxExclusive.</returns>
        /// <exceptioncreT:System.ArgumentOutOfRangeException">
        /// 
        /// <paramref name="maxExclusive is zero or negative.
        /// 
        /// </exception>
        public sealed override int Next(int maxExclusive)
        {
            // Invalid case: Zero and less are not valid use cases.
            if (maxExclusive <= 0)
            {
                //throw new ArgumentException(Resources.ArgumentMustBePositive);
            }

            // Fast case: Only zero is allowed to be returned. No sampling is needed.
            if (maxExclusive == 1) return 0;

            // Simple case: standard range
            if (maxExclusive == int.MaxValue) return Next();

            // Sample with maxExclusive ≥ 2
            if (_threadSafe)
                lock (_lock)
                {
                    return DoSampleInteger(maxExclusive);
                }

            return DoSampleInteger(maxExclusive);
        }

        /// <summary>
        ///     Returns a random number within a specified range.
        /// </summary>
        /// <param name="minInclusive">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxExclusive">The exclusive upper bound of the random number returned. Range: maxExclusive > minExclusive.</param>
        /// <returns>
        ///     A 32-bit signed integer greater than or equal to <paramref name="minInclusive and less than 
        ///     
        ///     <paramref name="maxExclusive; that is, the range of return values includes 
        ///     <paramref name="minInclusive but not 
        ///     <paramref name="maxExclusive. If <paramref name="minInclusive equals 
        ///     <paramref name="maxExclusive, 
        ///     <paramref name="minInclusive is returned.
        /// 
        /// 
        /// </returns>
        /// <exceptioncreT:System.ArgumentOutOfRangeException">
        /// 
        /// <paramref name="minInclusive is greater than 
        /// 
        /// <paramref name="maxExclusive. 
        /// 
        /// </exception>
        public sealed override int Next(int minInclusive, int maxExclusive)
        {
            // Invalid case: empty range.
            if (minInclusive >= maxExclusive)
            {
                //throw new ArgumentException(Resources.ArgumentMaxExclusiveMustBeLargerThanMinInclusive);
            }

            // Fast case: Only minInclusive is allowed to be returned. No sampling is needed.
            if (maxExclusive == minInclusive + 1) return minInclusive;

            // Simple case: simple range
            if (minInclusive == 0)
            {
                // Simple case: standard range
                if (maxExclusive == int.MaxValue) return Next();

                return Next(maxExclusive);
            }

            // Sample with maxExclusive ≥ minExclusive + 2
            if (_threadSafe)
                lock (_lock)
                {
                    return DoSampleInteger(minInclusive, maxExclusive);
                }

            return DoSampleInteger(minInclusive, maxExclusive);
        }


        /// <summary>
        ///     Fills the elements of a specified array of bytes with random numbers.
        /// </summary>
        /// <param name="buffer">An array of bytes to contain random numbers.</param>
        /// <exceptioncreT:System.ArgumentNullException">
        /// 
        /// <paramref name="buffer is null. 
        /// 
        /// </exception>
        public sealed override void NextBytes(byte[] buffer)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));

            if (_threadSafe)
            {
                lock (_lock)
                {
                    DoSampleBytes(buffer);
                }

                return;
            }

            DoSampleBytes(buffer);
        }

        /// <summary>
        ///     Returns a random number between 0.0 and 1.0.
        /// </summary>
        /// <returns>A double-precision floating point number greater than or equal to 0.0, and less than 1.0.</returns>
        protected sealed override double Sample()
        {
            if (_threadSafe)
                lock (_lock)
                {
                    return DoSample();
                }

            return DoSample();
        }

        /// <summary>
        ///     Returns a random double-precision floating point number greater than or equal to 0.0, and less than 1.0.
        /// </summary>
        protected abstract double DoSample();

        /// <summary>
        ///     Returns a random 32-bit signed integer greater than or equal to zero and less than 2147483647 (
        ///     <seecreF:System.Int32.MaxValue).
        /// </summary>
        protected virtual int DoSampleInteger()
        {
            return (int) (DoSample() * int.MaxValue);
        }

        /// <summary>
        ///     Fills the elements of a specified array of bytes with random numbers in full range, including zero and 255 (
        ///     <seecreF:System.Byte.MaxValue).
        /// </summary>
        protected virtual void DoSampleBytes(byte[] buffer)
        {
            for (var i = 0; i < buffer.Length; i++) buffer[i] = (byte) (DoSampleInteger() % 256);
        }

        /// <summary>
        ///     Returns a random N-bit signed integer greater than or equal to zero and less than 2^N.
        ///     N (bit count) is expected to be greater than zero and less than 32 (not verified).
        /// </summary>
        protected virtual int DoSampleInt32WithNBits(int bitCount)
        {
            // Fast case: Only 0 is allowed to be returned
            // No random call is needed
            if (bitCount == 0) return 0;

            var bytes = new byte[4];
            DoSampleBytes(bytes);

            // every bit with independent uniform distribution
            var uint32 = BitConverter.ToUInt32(bytes, 0);

            // the least significant N bits with independent uniform distribution and the remaining bits zero
            var uintN = uint32 >> (32 - bitCount);
            return (int) uintN;
        }


        /// <summary>
        ///     Returns a random 32-bit signed integer within the specified range.
        /// </summary>
        /// <param name="maxExclusive">
        ///     The exclusive upper bound of the random number returned. Range: maxExclusive ≥ 2 (not
        ///     verified, must be ensured by caller).
        /// </param>
        protected virtual int DoSampleInteger(int maxExclusive)
        {
            // non-biased implementation
            // (biased: return (int)(DoSample() * maxExclusive);)

            var bitCount = maxExclusive.Log2();
            var range = bitCount.PowerOfTwo();

            // Fast case: maxExclusive is a power of two
            if (range == maxExclusive) return DoSampleInt32WithNBits(bitCount);

            // Rejection case: we need to use rejection to avoid introducing bias
            bitCount++;
            int sample;
            do
            {
                sample = DoSampleInt32WithNBits(bitCount);
            } while (sample >= maxExclusive);

            return sample;
        }

        /// <summary>
        ///     Returns a random 32-bit signed integer within the specified range.
        /// </summary>
        /// <param name="minInclusive">The inclusive lower bound of the random number returned.</param>
        /// <param name="maxExclusive">
        ///     The exclusive upper bound of the random number returned. Range: maxExclusive ≥ minExclusive
        ///     + 2 (not verified, must be ensured by caller).
        /// </param>
        protected virtual int DoSampleInteger(int minInclusive, int maxExclusive)
        {
            // Sample with maxExclusive ≥ 2
            return DoSampleInteger(maxExclusive - minInclusive) + minInclusive;
        }
    }
}