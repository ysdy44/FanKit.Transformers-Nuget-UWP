// <copyright file="SparseVector.cs" company="Math.NET">
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
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.Providers.LinearAlgebra;
using MathNet.Numerics.Threading;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    ///     A vector with sparse storage, intended for very large vectors where most of the cells are zero.
    /// </summary>
    /// <remarks>The sparse vector is not thread safe.</remarks>
    [DebuggerDisplay("SparseVector {Count}-Double {NonZerosCount}-NonZero")]
    public class SparseVector : Vector
    {
        private readonly SparseVectorStorage<double> _storage;

        /// <summary>
        ///     Create a new sparse vector straight from an initialized vector storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public SparseVector(SparseVectorStorage<double> storage)
            : base(storage)
        {
            _storage = storage;
        }

        /// <summary>
        ///     Gets the number of non zero elements in the vector.
        /// </summary>
        /// <value>The number of non zero elements.</value>
        public int NonZerosCount => _storage.ValueCount;


        /// <summary>
        ///     Create a new sparse vector and initialize each value using the provided value.
        /// </summary>
        public static SparseVector Create(int length, double value)
        {
            return new SparseVector(SparseVectorStorage<double>.OfValue(length, value));
        }

        /// <summary>
        ///     Create a new sparse vector and initialize each value using the provided init function.
        /// </summary>
        public static SparseVector Create(int length, Func<int, double> init)
        {
            return new SparseVector(SparseVectorStorage<double>.OfInit(length, init));
        }

        /// <summary>
        ///     Adds a scalar to each element of the vector and stores the result in the result vector.
        ///     Warning, the new 'sparse vector' with a non-zero scalar added to it will be a 100% filled
        ///     sparse vector and very inefficient. Would be better to work with a dense vector instead.
        /// </summary>
        /// <param name="scalar">
        ///     The scalar to add.
        /// </param>
        /// <param name="result">
        ///     The vector to store the result of the addition.
        /// </param>
        protected override void DoAdd(double scalar, Vector<double> result)
        {
            if (scalar == 0.0)
            {
                if (!ReferenceEquals(this, result)) CopyTo(result);

                return;
            }

            if (ReferenceEquals(this, result))
            {
                // populate a new vector with the scalar
                var vnonZeroValues = new double[Count];
                var vnonZeroIndices = new int[Count];
                for (var index = 0; index < Count; index++)
                {
                    vnonZeroIndices[index] = index;
                    vnonZeroValues[index] = scalar;
                }

                // populate the non zero values from this
                var indices = _storage.Indices;
                var values = _storage.Values;
                for (var j = 0; j < _storage.ValueCount; j++) vnonZeroValues[indices[j]] = values[j] + scalar;

                // assign this vectors array to the new arrays.
                _storage.Values = vnonZeroValues;
                _storage.Indices = vnonZeroIndices;
                _storage.ValueCount = Count;
            }
            else
            {
                for (var index = 0; index < Count; index++) result.At(index, At(index) + scalar);
            }
        }

        /// <summary>
        ///     Adds another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">
        ///     The vector to add to this one.
        /// </param>
        /// <param name="result">
        ///     The vector to store the result of the addition.
        /// </param>
        protected override void DoAdd(Vector<double> other, Vector<double> result)
        {
            var otherSparse = other as SparseVector;
            if (otherSparse == null)
            {
                base.DoAdd(other, result);
                return;
            }

            var resultSparse = result as SparseVector;
            if (resultSparse == null)
            {
                base.DoAdd(other, result);
                return;
            }

            // TODO (ruegg, 2011-10-11): Options to optimize?

            var otherStorage = otherSparse._storage;
            if (ReferenceEquals(this, resultSparse))
            {
                int i = 0, j = 0;
                while (j < otherStorage.ValueCount)
                    if (i >= _storage.ValueCount || _storage.Indices[i] > otherStorage.Indices[j])
                    {
                        var otherValue = otherStorage.Values[j];
                        if (otherValue != 0.0)
                            _storage.InsertAtIndexUnchecked(i++, otherStorage.Indices[j], otherValue);
                        j++;
                    }
                    else if (_storage.Indices[i] == otherStorage.Indices[j])
                    {
                        // TODO: result can be zero, remove?
                        _storage.Values[i++] += otherStorage.Values[j++];
                    }
                    else
                    {
                        i++;
                    }
            }
            else
            {
                result.Clear();
                int i = 0, j = 0, last = -1;
                while (i < _storage.ValueCount || j < otherStorage.ValueCount)
                    if (j >= otherStorage.ValueCount ||
                        i < _storage.ValueCount && _storage.Indices[i] <= otherStorage.Indices[j])
                    {
                        var next = _storage.Indices[i];
                        if (next != last)
                        {
                            last = next;
                            result.At(next, _storage.Values[i] + otherSparse.At(next));
                        }

                        i++;
                    }
                    else
                    {
                        var next = otherStorage.Indices[j];
                        if (next != last)
                        {
                            last = next;
                            result.At(next, At(next) + otherStorage.Values[j]);
                        }

                        j++;
                    }
            }
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">
        ///     The scalar to subtract.
        /// </param>
        /// <param name="result">
        ///     The vector to store the result of the subtraction.
        /// </param>
        protected override void DoSubtract(double scalar, Vector<double> result)
        {
            DoAdd(-scalar, result);
        }

        /// <summary>
        ///     Subtracts another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">
        ///     The vector to subtract from this one.
        /// </param>
        /// <param name="result">
        ///     The vector to store the result of the subtraction.
        /// </param>
        protected override void DoSubtract(Vector<double> other, Vector<double> result)
        {
            if (ReferenceEquals(this, other))
            {
                result.Clear();
                return;
            }

            var otherSparse = other as SparseVector;
            if (otherSparse == null)
            {
                base.DoSubtract(other, result);
                return;
            }

            var resultSparse = result as SparseVector;
            if (resultSparse == null)
            {
                base.DoSubtract(other, result);
                return;
            }

            // TODO (ruegg, 2011-10-11): Options to optimize?

            var otherStorage = otherSparse._storage;
            if (ReferenceEquals(this, resultSparse))
            {
                int i = 0, j = 0;
                while (j < otherStorage.ValueCount)
                    if (i >= _storage.ValueCount || _storage.Indices[i] > otherStorage.Indices[j])
                    {
                        var otherValue = otherStorage.Values[j];
                        if (otherValue != 0.0)
                            _storage.InsertAtIndexUnchecked(i++, otherStorage.Indices[j], -otherValue);
                        j++;
                    }
                    else if (_storage.Indices[i] == otherStorage.Indices[j])
                    {
                        // TODO: result can be zero, remove?
                        _storage.Values[i++] -= otherStorage.Values[j++];
                    }
                    else
                    {
                        i++;
                    }
            }
            else
            {
                result.Clear();
                int i = 0, j = 0, last = -1;
                while (i < _storage.ValueCount || j < otherStorage.ValueCount)
                    if (j >= otherStorage.ValueCount ||
                        i < _storage.ValueCount && _storage.Indices[i] <= otherStorage.Indices[j])
                    {
                        var next = _storage.Indices[i];
                        if (next != last)
                        {
                            last = next;
                            result.At(next, _storage.Values[i] - otherSparse.At(next));
                        }

                        i++;
                    }
                    else
                    {
                        var next = otherStorage.Indices[j];
                        if (next != last)
                        {
                            last = next;
                            result.At(next, At(next) - otherStorage.Values[j]);
                        }

                        j++;
                    }
            }
        }

        /// <summary>
        ///     Negates vector and saves result to <paramref name="result
        /// 
        /// 
        /// </summary>
        /// <param name="result">Target vector</param>
        protected override void DoNegate(Vector<double> result)
        {
            var sparseResult = result as SparseVector;
            if (sparseResult == null)
            {
                result.Clear();
                for (var index = 0; index < _storage.ValueCount; index++)
                    result.At(_storage.Indices[index], -_storage.Values[index]);
                return;
            }

            if (!ReferenceEquals(this, result))
            {
                sparseResult._storage.ValueCount = _storage.ValueCount;
                sparseResult._storage.Indices = new int[_storage.ValueCount];
                Buffer.BlockCopy(_storage.Indices, 0, sparseResult._storage.Indices, 0,
                    _storage.ValueCount * Constants.SizeOfInt);
                sparseResult._storage.Values = new double[_storage.ValueCount];
                Array.Copy(_storage.Values, 0, sparseResult._storage.Values, 0, _storage.ValueCount);
            }

            LinearAlgebraControl.Provider.ScaleArray(-1.0d, sparseResult._storage.Values, sparseResult._storage.Values);
        }

        /// <summary>
        ///     Multiplies a scalar to each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">
        ///     The scalar to multiply.
        /// </param>
        /// <param name="result">
        ///     The vector to store the result of the multiplication.
        /// </param>
        protected override void DoMultiply(double scalar, Vector<double> result)
        {
            var sparseResult = result as SparseVector;
            if (sparseResult == null)
            {
                result.Clear();
                for (var index = 0; index < _storage.ValueCount; index++)
                    result.At(_storage.Indices[index], scalar * _storage.Values[index]);
            }
            else
            {
                if (!ReferenceEquals(this, result))
                {
                    sparseResult._storage.ValueCount = _storage.ValueCount;
                    sparseResult._storage.Indices = new int[_storage.ValueCount];
                    Buffer.BlockCopy(_storage.Indices, 0, sparseResult._storage.Indices, 0,
                        _storage.ValueCount * Constants.SizeOfInt);
                    sparseResult._storage.Values = new double[_storage.ValueCount];
                    Array.Copy(_storage.Values, 0, sparseResult._storage.Values, 0, _storage.ValueCount);
                }

                LinearAlgebraControl.Provider.ScaleArray(scalar, sparseResult._storage.Values,
                    sparseResult._storage.Values);
            }
        }

        /// <summary>
        ///     Computes the dot product between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The sum of a[i]*b[i] for all i.</returns>
        protected override double DoDotProduct(Vector<double> other)
        {
            var result = 0d;
            if (ReferenceEquals(this, other))
                for (var i = 0; i < _storage.ValueCount; i++)
                    result += _storage.Values[i] * _storage.Values[i];
            else
                for (var i = 0; i < _storage.ValueCount; i++)
                    result += _storage.Values[i] * other.At(_storage.Indices[i]);
            return result;
        }

        /// <summary>
        ///     Computes the canonical modulus, where the result has the sign of the divisor,
        ///     for each element of the vector for the given divisor.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoModulus(double divisor, Vector<double> result)
        {
            if (ReferenceEquals(this, result))
            {
                for (var index = 0; index < _storage.ValueCount; index++)
                    _storage.Values[index] = Euclid.Modulus(_storage.Values[index], divisor);
            }
            else
            {
                result.Clear();
                for (var index = 0; index < _storage.ValueCount; index++)
                    result.At(_storage.Indices[index], Euclid.Modulus(_storage.Values[index], divisor));
            }
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for each element of the vector for the given divisor.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoRemainder(double divisor, Vector<double> result)
        {
            if (ReferenceEquals(this, result))
            {
                for (var index = 0; index < _storage.ValueCount; index++) _storage.Values[index] %= divisor;
            }
            else
            {
                result.Clear();
                for (var index = 0; index < _storage.ValueCount; index++)
                    result.At(_storage.Indices[index], _storage.Values[index] % divisor);
            }
        }

        /// <summary>
        ///     Adds two <strong>Vectors</strong> together and returns the results.
        /// </summary>
        /// <param name="leftSide">One of the vectors to add.</param>
        /// <param name="rightSide">The other vector to add.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator +(SparseVector leftSide, SparseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseVector) leftSide.Add(rightSide);
        }

        /// <summary>
        ///     Returns a <strong>Vector</strong> containing the negated values of <paramref name="rightSide.
        /// 
        /// 
        /// </summary>
        /// <param name="rightSide">The vector to get the values from.</param>
        /// <returns>A vector containing the negated values as <paramref name="rightSide.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator -(SparseVector rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (SparseVector) rightSide.Negate();
        }

        /// <summary>
        ///     Subtracts two <strong>Vectors</strong> and returns the results.
        /// </summary>
        /// <param name="leftSide">The vector to subtract from.</param>
        /// <param name="rightSide">The vector to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator -(SparseVector leftSide, SparseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseVector) leftSide.Subtract(rightSide);
        }

        /// <summary>
        ///     Multiplies a vector with a scalar.
        /// </summary>
        /// <param name="leftSide">The vector to scale.</param>
        /// <param name="rightSide">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator *(SparseVector leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseVector) leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a vector with a scalar.
        /// </summary>
        /// <param name="leftSide">The scalar value.</param>
        /// <param name="rightSide">The vector to scale.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator *(double leftSide, SparseVector rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (SparseVector) rightSide.Multiply(leftSide);
        }

        /// <summary>
        ///     Computes the dot product between two <strong>Vectors</strong>.
        /// </summary>
        /// <param name="leftSide">The left row vector.</param>
        /// <param name="rightSide">The right column vector.</param>
        /// <returns>The dot product between the two vectors.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static double operator *(SparseVector leftSide, SparseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return leftSide.DotProduct(rightSide);
        }

        /// <summary>
        ///     Divides a vector with a scalar.
        /// </summary>
        /// <param name="leftSide">The vector to divide.</param>
        /// <param name="rightSide">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator /(SparseVector leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseVector) leftSide.Divide(rightSide);
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     of each element of the vector of the given divisor.
        /// </summary>
        /// <param name="leftSide">The vector whose elements we want to compute the modulus of.</param>
        /// <param name="rightSide">The divisor to use,</param>
        /// <returns>The result of the calculation</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator %(SparseVector leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseVector) leftSide.Remainder(rightSide);
        }

        /// <summary>
        ///     Returns the index of the maximum element.
        /// </summary>
        /// <returns>The index of maximum element.</returns>
        public override int MaximumIndex()
        {
            if (_storage.ValueCount == 0) return 0;

            var index = 0;
            var max = _storage.Values[0];
            for (var i = 1; i < _storage.ValueCount; i++)
                if (max < _storage.Values[i])
                {
                    index = i;
                    max = _storage.Values[i];
                }

            return _storage.Indices[index];
        }

        /// <summary>
        ///     Computes the sum of the vector's elements.
        /// </summary>
        /// <returns>The sum of the vector's elements.</returns>
        public override double Sum()
        {
            double result = 0;
            for (var i = 0; i < _storage.ValueCount; i++) result += _storage.Values[i];
            return result;
        }

        /// <summary>
        ///     Calculates the L1 norm of the vector, also known as Manhattan norm.
        /// </summary>
        /// <returns>The sum of the absolute values.</returns>
        public double L1Norm()
        {
            var result = 0d;
            for (var i = 0; i < _storage.ValueCount; i++) result += Math.Abs(_storage.Values[i]);
            return result;
        }

        /// <summary>
        ///     Calculates the infinity norm of the vector.
        /// </summary>
        /// <returns>The maximum absolute value.</returns>
        public double InfinityNorm()
        {
            return CommonParallel.Aggregate(0, _storage.ValueCount, i => Math.Abs(_storage.Values[i]), Math.Max, 0d);
        }

        /// <summary>
        ///     Computes the p-Norm.
        /// </summary>
        /// <param name="p">The p value.</param>
        /// <returns>Scalar <c>ret = ( ∑|this[i]|^p )^(1/p)</c></returns>
        public override double Norm(double p)
        {
            if (p < 0d)
            {
            } //throw new ArgumentOutOfRangeException(nameof(p));

            if (_storage.ValueCount == 0) return 0d;

            if (p == 1d) return L1Norm();
            if (p == 2d) return L2Norm();
            if (double.IsPositiveInfinity(p)) return InfinityNorm();

            var sum = 0d;
            for (var index = 0; index < _storage.ValueCount; index++)
                sum += Math.Pow(Math.Abs(_storage.Values[index]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        #region Parse Functions

        #endregion

        public override string ToTypeString()
        {
            return string.Format("SparseVector {0}-Double {1:P2} Filled", Count, NonZerosCount / (double) Count);
        }
    }
}