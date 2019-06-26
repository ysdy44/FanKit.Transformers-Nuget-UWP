// <copyright file="DivergenceStopCriterion.cs" company="Math.NET">
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

namespace MathNet.Numerics.LinearAlgebra.Solvers
{
    /// <summary>
    ///     Monitors an iterative calculation for signs of divergence.
    /// </summary>
    public sealed class DivergenceStopCriterion<T> : IIterationStopCriterion<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        ///     The maximum relative increase the residual may experience without triggering a divergence warning.
        /// </summary>
        private readonly double _maximumRelativeIncrease;

        /// <summary>
        ///     The number of iterations over which a residual increase should be tracked before issuing a divergence warning.
        /// </summary>
        private readonly int _minimumNumberOfIterations;

        /// <summary>
        ///     The iteration number of the last iteration.
        /// </summary>
        private int _lastIteration = -1;

        /// <summary>
        ///     The array that holds the tracking information.
        /// </summary>
        private double[] _residualHistory;

        /// <summary>
        ///     The status of the calculation
        /// </summary>
        private IterationStatus _status = IterationStatus.Continue;

        /// <summary>
        ///     Initializes a new instance of the
        ///     <seecreDivergenceStopCriterion{ T} class with the specified maximum
        ///         relative increase and the specified minimum number of tracking iterations.
        /// </summary>
        /// <param name="maximumRelativeIncrease">
        ///     The maximum relative increase that the residual may experience before a
        ///     divergence warning is issued.
        /// </param>
        /// <param name="minimumIterations">
        ///     The minimum number of iterations over which the residual must grow before a divergence
        ///     warning is issued.
        /// </param>
        public DivergenceStopCriterion(double maximumRelativeIncrease = 0.08, int minimumIterations = 10)
        {
            if (maximumRelativeIncrease <= 0) throw new ArgumentOutOfRangeException(nameof(maximumRelativeIncrease));

            // There must be at least three iterations otherwise we can't calculate the relative increase
            if (minimumIterations < 3) throw new ArgumentOutOfRangeException(nameof(minimumIterations));

            _maximumRelativeIncrease = maximumRelativeIncrease;
            _minimumNumberOfIterations = minimumIterations;
        }


        /// <summary>
        ///     Clones the current <seecreDivergenceStopCriterion{ T} and its settings.
        /// </summary>
        /// <returns>A new instance of the <seecreDivergenceStopCriterion{ T} class.</returns>
        public IIterationStopCriterion<T> Clone()
        {
            return new DivergenceStopCriterion<T>(_maximumRelativeIncrease, _minimumNumberOfIterations);
        }
    }
}