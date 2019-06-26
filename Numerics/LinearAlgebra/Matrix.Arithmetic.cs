// <copyright file="Matrix.Arithmetic.cs" company="Math.NET">
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
using System.Linq;

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    ///     Defines the base class for <c>Matrix</c> classes.
    /// </summary>
    public abstract partial class Matrix<T>
    {
        /// <summary>
        ///     The value of 1.0.
        /// </summary>
        public static readonly T One = BuilderInstance<T>.Matrix.One;

        /// <summary>
        ///     The value of 0.0.
        /// </summary>
        public static readonly T Zero = BuilderInstance<T>.Matrix.Zero;

        /// <summary>
        ///     Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        protected abstract void DoNegate(Matrix<T> result);


        /// <summary>
        ///     Add a scalar to each element of the matrix and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        protected abstract void DoAdd(T scalar, Matrix<T> result);

        /// <summary>
        ///     Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        protected abstract void DoAdd(Matrix<T> other, Matrix<T> result);

        /// <summary>
        ///     Subtracts a scalar from each element of the matrix and stores the result in the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected abstract void DoSubtract(T scalar, Matrix<T> result);

        /// <summary>
        ///     Subtracts each element of the matrix from a scalar and stores the result in the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to subtract from.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected void DoSubtractFrom(T scalar, Matrix<T> result)
        {
            DoNegate(result);
            result.DoAdd(scalar, result);
        }

        /// <summary>
        ///     Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected abstract void DoSubtract(Matrix<T> other, Matrix<T> result);

        /// <summary>
        ///     Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        protected abstract void DoMultiply(T scalar, Matrix<T> result);

        /// <summary>
        ///     Multiplies this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected abstract void DoMultiply(Vector<T> rightSide, Vector<T> result);

        /// <summary>
        ///     Multiplies this matrix with another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected abstract void DoMultiply(Matrix<T> other, Matrix<T> result);


        /// <summary>
        ///     Multiplies the transpose of this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected abstract void DoTransposeThisAndMultiply(Vector<T> rightSide, Vector<T> result);


        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given divisor each element of the matrix.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">Matrix to store the results in.</param>
        protected abstract void DoRemainder(T divisor, Matrix<T> result);

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given dividend for each element of the matrix.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected abstract void DoRemainderByThis(T dividend, Matrix<T> result);

        /// <summary>
        ///     Pointwise remainder (% operator), where the result has the sign of the dividend,
        ///     of this matrix with another matrix and stores the result into the result matrix.
        /// </summary>
        /// <param name="divisor">The pointwise denominator matrix to use</param>
        /// <param name="result">The result of the modulus.</param>
        protected abstract void DoPointwiseRemainder(Matrix<T> divisor, Matrix<T> result);

        /// <summary>
        ///     Pointwise applies the exponential function to each value and stores the result into the result matrix.
        /// </summary>
        /// <param name="result">The matrix to store the result.</param>
        protected abstract void DoPointwiseExp(Matrix<T> result);

        protected abstract void DoPointwiseAbs(Matrix<T> result);
        protected abstract void DoPointwiseAtan(Matrix<T> result);
        protected abstract void DoPointwiseRound(Matrix<T> result);

        protected abstract void DoPointwiseTan(Matrix<T> result);


        /// <summary>
        ///     Adds a scalar to each element of the matrix.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        public Matrix<T> Add(T scalar)
        {
            if (scalar.Equals(Zero)) return Clone();

            var result = Build.SameAs(this);
            DoAdd(scalar, result);
            return result;
        }

        /// <summary>
        ///     Adds a scalar to each element of the matrix and stores the result in the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        public void Add(T scalar, Matrix<T> result)
        {
            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, result, "result");

            if (scalar.Equals(Zero))
            {
                CopyTo(result);
                return;
            }

            DoAdd(scalar, result);
        }

        /// <summary>
        ///     Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        public Matrix<T> Add(Matrix<T> other)
        {
            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, other);

            var result = Build.SameAs(this, other, RowCount, ColumnCount);
            DoAdd(other, result);
            return result;
        }

        /// <summary>
        ///     Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        public void Add(Matrix<T> other, Matrix<T> result)
        {
            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, other, "other");

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, result, "result");

            DoAdd(other, result);
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the matrix.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <returns>A new matrix containing the subtraction of this matrix and the scalar.</returns>
        public Matrix<T> Subtract(T scalar)
        {
            if (scalar.Equals(Zero)) return Clone();

            var result = Build.SameAs(this);
            DoSubtract(scalar, result);
            return result;
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the matrix and stores the result in the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        /// <exceptioncreArgumentException">If this matrix and 
        /// 
        /// <paramref name="result are not the same size.
        /// 
        /// </exception>
        public void Subtract(T scalar, Matrix<T> result)
        {
            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, result, "result");

            if (scalar.Equals(Zero))
            {
                CopyTo(result);
                return;
            }

            DoSubtract(scalar, result);
        }

        /// <summary>
        ///     Subtracts each element of the matrix from a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to subtract from.</param>
        /// <returns>A new matrix containing the subtraction of the scalar and this matrix.</returns>
        public Matrix<T> SubtractFrom(T scalar)
        {
            var result = Build.SameAs(this);
            DoSubtractFrom(scalar, result);
            return result;
        }


        /// <summary>
        ///     Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        public Matrix<T> Subtract(Matrix<T> other)
        {
            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, other);

            var result = Build.SameAs(this, other, RowCount, ColumnCount);
            DoSubtract(other, result);
            return result;
        }

        /// <summary>
        ///     Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        public void Subtract(Matrix<T> other, Matrix<T> result)
        {
            if (other.RowCount != RowCount || other.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, other, "other");

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(this, result, "result");

            DoSubtract(other, result);
        }

        /// <summary>
        ///     Multiplies each element of this matrix with a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to multiply with.</param>
        /// <returns>The result of the multiplication.</returns>
        public Matrix<T> Multiply(T scalar)
        {
            if (scalar.Equals(One)) return Clone();

            if (scalar.Equals(Zero)) return Build.SameAs(this);

            var result = Build.SameAs(this);
            DoMultiply(scalar, result);
            return result;
        }


        /// <summary>
        ///     Multiplies this matrix by a vector and returns the result.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <c>this.ColumnCount != rightSide.Count</c>
        /// .
        /// </exception>
        public Vector<T> Multiply(Vector<T> rightSide)
        {
            if (ColumnCount != rightSide.Count)
                throw DimensionsDontMatch<ArgumentException>(this, rightSide, "rightSide");

            var ret = Vector<T>.Build.SameAs(this, rightSide, RowCount);
            DoMultiply(rightSide, ret);
            return ret;
        }


        /// <summary>
        ///     Left multiply a matrix with a vector ( = vector * matrix ).
        /// </summary>
        /// <param name="leftSide">The vector to multiply with.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <strong>this.RowCount != <paramref name="leftSide.Count</strong>
        /// .
        /// </exception>
        public Vector<T> LeftMultiply(Vector<T> leftSide)
        {
            if (RowCount != leftSide.Count) throw DimensionsDontMatch<ArgumentException>(this, leftSide, "leftSide");

            var ret = Vector<T>.Build.SameAs(this, leftSide, ColumnCount);
            DoLeftMultiply(leftSide, ret);
            return ret;
        }

        /// <summary>
        ///     Left multiply a matrix with a vector ( = vector * matrix ) and place the result in the result vector.
        /// </summary>
        /// <param name="leftSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected void DoLeftMultiply(Vector<T> leftSide, Vector<T> result)
        {
            DoTransposeThisAndMultiply(leftSide, result);
        }

        /// <summary>
        ///     Multiplies this matrix with another matrix and returns the result.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <strong>this.Columns != other.Rows</strong>
        /// .
        /// </exception>
        /// <returns>The result of the multiplication.</returns>
        public Matrix<T> Multiply(Matrix<T> other)
        {
            if (ColumnCount != other.RowCount) throw DimensionsDontMatch<ArgumentException>(this, other);

            var result = Build.SameAs(this, other, RowCount, other.ColumnCount);
            DoMultiply(other, result);
            return result;
        }


        /// <summary>
        ///     Negate each element of this matrix.
        /// </summary>
        /// <returns>A matrix containing the negated values.</returns>
        public Matrix<T> Negate()
        {
            var result = Build.SameAs(this);
            DoNegate(result);
            return result;
        }

        /// <summary>
        ///     Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        /// <exceptioncreArgumentException">if the result matrix's dimensions are not the same as this matrix.
        /// 
        /// </exception>
        public void Negate(Matrix<T> result)
        {
            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentException>(this, result);

            DoNegate(result);
        }


        /// <summary>
        ///     Computes the remainder (matrix % divisor), where the result has the sign of the dividend,
        ///     for each element of the matrix.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <returns>A matrix containing the results.</returns>
        public Matrix<T> Remainder(T divisor)
        {
            var result = Build.SameAs(this);
            DoRemainder(divisor, result);
            return result;
        } 

        /// <summary>
        ///     Computes the remainder (dividend % matrix), where the result has the sign of the dividend,
        ///     for each element of the matrix.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <returns>A matrix containing the results.</returns>
        public Matrix<T> RemainderByThis(T dividend)
        {
            var result = Build.SameAs(this);
            DoRemainderByThis(dividend, result);
            return result;
        }


        /// <summary>
        ///     Pointwise remainder (% operator), where the result has the sign of the dividend,
        ///     of this matrix by another matrix.
        /// </summary>
        /// <param name="divisor">The pointwise denominator matrix to use.</param>
        /// <exceptioncreArgumentException">If this matrix and 
        /// 
        /// <paramref name="divisor are not the same size.
        /// 
        /// </exception>
        public Matrix<T> PointwiseRemainder(Matrix<T> divisor)
        {
            if (ColumnCount != divisor.ColumnCount || RowCount != divisor.RowCount)
                throw DimensionsDontMatch<ArgumentException>(this, divisor);

            var result = Build.SameAs(this, divisor);
            DoPointwiseRemainder(divisor, result);
            return result;
        }


        /// <summary>
        ///     Helper function to apply a unary function to a matrix. The function
        ///     f modifies the matrix given to it in place.  Before its
        ///     called, a copy of the 'this' matrix is first created, then passed to
        ///     f.  The copy is then returned as the result
        /// </summary>
        /// <param name="f">Function which takes a matrix, modifies it in place and returns void</param>
        /// <returns>New instance of matrix which is the result</returns>
        protected Matrix<T> PointwiseUnary(Action<Matrix<T>> f)
        {
            var result = Build.SameAs(this);
            f(result);
            return result;
        }


        /// <summary>
        ///     Pointwise applies the exponent function to each value.
        /// </summary>
        public Matrix<T> PointwiseExp()
        {
            return PointwiseUnary(DoPointwiseExp);
        }


        /// <summary>
        ///     Pointwise applies the abs function to each value
        /// </summary>
        public Matrix<T> PointwiseAbs()
        {
            return PointwiseUnary(DoPointwiseAbs);
        }


        /// <summary>
        ///     Pointwise applies the atan function to each value
        /// </summary>
        public Matrix<T> PointwiseAtan()
        {
            return PointwiseUnary(DoPointwiseAtan);
        }


        /// <summary>
        ///     Pointwise applies the round function to each value
        /// </summary>
        public Matrix<T> PointwiseRound()
        {
            return PointwiseUnary(DoPointwiseRound);
        }

        


        /// <summary>
        ///     Pointwise applies the tan function to each value
        /// </summary>
        public Matrix<T> PointwiseTan()
        {
            return PointwiseUnary(DoPointwiseTan);
        }


        /// <summary>
        ///     Computes the trace of this matrix.
        /// </summary>
        /// <returns>The trace of this matrix</returns>
        /// <exceptioncreArgumentException">If the matrix is not square
        /// 
        /// </exception>
        public abstract T Trace();
        
        


        #region Exceptions - possibly move elsewhere?

        internal static Exception DimensionsDontMatch<TException>(Matrix<T> left, Matrix<T> right,
            string paramName = null)
            where TException : Exception
        {
            //   var message = string.Format(Resources.ArgumentMatrixDimensions2, left.RowCount + "x" + left.ColumnCount, right.RowCount + "x" + right.ColumnCount);
            return CreateException<TException>("", paramName);
        }

        internal static Exception DimensionsDontMatch<TException>(Matrix<T> matrix)
            where TException : Exception
        {
            //   var message = string.Format(Resources.ArgumentMatrixDimensions1, matrix.RowCount + "x" + matrix.ColumnCount);
            return CreateException<TException>("");
        }


        internal static Exception DimensionsDontMatch<TException>(Matrix<T> left, Vector<T> right,
            string paramName = null)
            where TException : Exception
        {
            return DimensionsDontMatch<TException>(left, right.ToColumnMatrix(), paramName);
        }

        internal static Exception DimensionsDontMatch<TException>(Vector<T> left, Matrix<T> right,
            string paramName = null)
            where TException : Exception
        {
            return DimensionsDontMatch<TException>(left.ToColumnMatrix(), right, paramName);
        }

        private static Exception CreateException<TException>(string message, string paramName = null)
            where TException : Exception
        {
            if (typeof(TException) == typeof(ArgumentException)) return new ArgumentException(message, paramName);
            if (typeof(TException) == typeof(ArgumentOutOfRangeException))
                return new ArgumentOutOfRangeException(paramName, message);
            return new Exception(message);
        }

        #endregion
    }
}