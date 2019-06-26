// <copyright file="Generate.cs" company="Math.NET">
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

namespace MathNet.Numerics
{
    public static class Generate
    {
        /// <summary>
        ///     Generate a linearly spaced sample vector within the inclusive interval (start, stop) and step 1.
        ///     Equivalent to MATLAB colon operator (:).
        /// </summary>
        public static double[] LinearRange(int start, int stop)
        {
            if (start == stop) return new double[] {start};
            if (start < stop)
            {
                var data = new double[stop - start + 1];
                for (var i = 0; i < data.Length; i++) data[i] = start + i;
                return data;
            }
            else
            {
                var data = new double[start - stop + 1];
                for (var i = 0; i < data.Length; i++) data[i] = start - i;
                return data;
            }
        }


        /// <summary>
        ///     Generate a linearly spaced sample vector within the inclusive interval (start, stop) and the provided step.
        ///     The start value is aways included as first value, but stop is only included if it stop-start is a multiple of step.
        ///     Equivalent to MATLAB double colon operator (::).
        /// </summary>
        public static double[] LinearRange(int start, int step, int stop)
        {
            if (start == stop) return new double[] {start};
            if (start < stop && step < 0 || start > stop && step > 0 || step == 0d) return new double[0];

            var data = new double[(stop - start) / step + 1];
            for (var i = 0; i < data.Length; i++) data[i] = start + i * step;
            return data;
        }

        /// <summary>
        ///     Generate a linearly spaced sample vector within the inclusive interval (start, stop) and the provide step.
        ///     The start value is aways included as first value, but stop is only included if it stop-start is a multiple of step.
        ///     Equivalent to MATLAB double colon operator (::).
        /// </summary>
        public static double[] LinearRange(double start, double step, double stop)
        {
            if (start == stop) return new[] {start};
            if (start < stop && step < 0 || start > stop && step > 0 || step == 0d) return new double[0];

            var data = new double[(int) Math.Floor((stop - start) / step + 1d)];
            for (var i = 0; i < data.Length; i++) data[i] = start + i * step;
            return data;
        }


        /// <summary>
        ///     Create a periodic wave.
        /// </summary>
        /// <param name="length">The number of samples to generate.</param>
        /// <param name="map">The function to apply to each of the values and evaluate the resulting sample.</param>
        /// <param name="samplingRate">
        ///     Samples per time unit (Hz). Must be larger than twice the frequency to satisfy the Nyquist
        ///     criterion.
        /// </param>
        /// <param name="frequency">Frequency in periods per time unit (Hz).</param>
        /// <param name="amplitude">
        ///     The length of the period when sampled at one sample per time unit. This is the interval of the
        ///     periodic domain, a typical value is 1.0, or 2*Pi for angular functions.
        /// </param>
        /// <param name="phase">Optional phase offset.</param>
        /// <param name="delay">Optional delay, relative to the phase.</param>
        public static T[] PeriodicMap<T>(int length, Func<double, T> map, double samplingRate, double frequency,
            double amplitude = 1.0, double phase = 0.0, int delay = 0)
        {
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));

            var step = frequency / samplingRate * amplitude;
            phase = Euclid.Modulus(phase - delay * step, amplitude);

            var data = new T[length];
            for (int i = 0, k = 0; i < data.Length; i++, k++)
            {
                var x = phase + k * step;
                if (x >= amplitude)
                {
                    x %= amplitude;
                    phase = x;
                    k = 0;
                }

                data[i] = map(x);
            }

            return data;
        }


        /// <summary>
        ///     Create a periodic square wave, starting with the high phase.
        /// </summary>
        /// <param name="length">The number of samples to generate.</param>
        /// <param name="highDuration">Number of samples of the high phase.</param>
        /// <param name="lowDuration">Number of samples of the low phase.</param>
        /// <param name="lowValue">Sample value to be emitted during the low phase.</param>
        /// <param name="highValue">Sample value to be emitted during the high phase.</param>
        /// <param name="delay">Optional delay.</param>
        public static double[] Square(int length, int highDuration, int lowDuration, double lowValue, double highValue,
            int delay = 0)
        {
            var duration = highDuration + lowDuration;
            return PeriodicMap(length, x => x < highDuration ? highValue : lowValue, 1.0, 1.0 / duration, duration, 0.0,
                delay);
        }
    }
}