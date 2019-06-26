﻿// <copyright file="VectorStorage.Validation.cs" company="Math.NET">
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

namespace MathNet.Numerics.LinearAlgebra.Storage
{
    // ReSharper disable UnusedParameter.Global
    public partial class VectorStorage<T>
    {
        private void ValidateRange(int index)
        {
            if ((uint) index >= (uint) Length) throw new ArgumentOutOfRangeException(nameof(index));
        }

        private void ValidateSubVectorRange(VectorStorage<T> target,
            int sourceIndex, int targetIndex, int count)
        {
            if (count < 1)
            {
                // throw new ArgumentOutOfRangeException(nameof(count), Resources.ArgumentMustBePositive);
            }

            // Verify Source

            if ((uint) sourceIndex >= (uint) Length) throw new ArgumentOutOfRangeException(nameof(sourceIndex));

            var sourceMax = sourceIndex + count;

            if (sourceMax > Length) throw new ArgumentOutOfRangeException(nameof(count));

            // Verify Target

            if ((uint) targetIndex >= (uint) target.Length) throw new ArgumentOutOfRangeException(nameof(targetIndex));

            var targetMax = targetIndex + count;

            if (targetMax > target.Length) throw new ArgumentOutOfRangeException(nameof(count));
        }
    }
    // ReSharper restore UnusedParameter.Global
}