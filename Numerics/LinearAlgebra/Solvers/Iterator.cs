// <copyright file="Iterator.cs" company="Math.NET">
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
using System.Collections.Generic;
using System.Linq;

namespace MathNet.Numerics.LinearAlgebra.Solvers
{
    /// <summary>
    ///     An iterator that is used to check if an iterative calculation should continue or stop.
    /// </summary>
    public sealed class Iterator<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        ///     The collection that holds all the stop criteria and the flag indicating if they should be added
        ///     to the child iterators.
        /// </summary>
        private readonly List<IIterationStopCriterion<T>> _stopCriteria;

        /// <summary>
        ///     The status of the iterator.
        /// </summary>
        private IterationStatus _status = IterationStatus.Continue;


        /// <summary>
        ///     Initializes a new instance of the <seecreIterator{ T} class with the specified stop criteria.
        /// </summary>
        /// <param name="stopCriteria">
        ///     The specified stop criteria. Only one stop criterion of each type can be passed in. None
        ///     of the stop criteria will be passed on to child iterators.
        /// </param>
        public Iterator(IEnumerable<IIterationStopCriterion<T>> stopCriteria)
        {
            _stopCriteria = new List<IIterationStopCriterion<T>>(stopCriteria);
        }


        /// <summary>
        ///     Indicates to the iterator that the iterative process has been cancelled.
        /// </summary>
        /// <remarks>
        ///     Does not reset the stop-criteria.
        /// </remarks>
        public void Cancel()
        {
            _status = IterationStatus.Cancelled;
        }
        

        /// <summary>
        ///     Creates a deep clone of the current iterator.
        /// </summary>
        /// <returns>The deep clone of the current iterator.</returns>
        public Iterator<T> Clone()
        {
            return new Iterator<T>(_stopCriteria.Select(sc => sc.Clone()));
        }
    }
}