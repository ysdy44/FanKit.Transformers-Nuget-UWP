// <copyright file="Matrix.cs" company="Math.NET">
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
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    ///     <c>double</c> version of the <seecreMatrix{ T} class.
    /// </summary>
    public abstract class Matrix : Matrix<double>
    {
        /// <summary>
        ///     Initializes a new instance of the Matrix class.
        /// </summary>
        protected Matrix(MatrixStorage<double> storage)
            : base(storage)
        {

        }

        /// <summary>
        ///     Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        protected override void DoNegate(Matrix<double> result)
        {
            Map(x => -x, result);
        }

        /// <summary>
        ///     Add a scalar to each element of the matrix and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        protected override void DoAdd(double scalar, Matrix<double> result)
        {
            Map(x => x + scalar, result, Zeros.Include);
        }

        /// <summary>
        ///     Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        /// <exceptioncreArgumentNullException">If the other matrix is 
        /// 
        /// <see langword="null.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        protected override void DoAdd(Matrix<double> other, Matrix<double> result)
        {
            Map2((x, y) => x + y, other, result);
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected override void DoSubtract(double scalar, Matrix<double> result)
        {
            Map(x => x - scalar, result, Zeros.Include);
        }

        /// <summary>
        ///     Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract to this matrix.</param>
        /// <param name="result">The matrix to store the result of subtraction.</param>
        /// <exceptioncreArgumentNullException">If the other matrix is 
        /// 
        /// <see langword="null.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        protected override void DoSubtract(Matrix<double> other, Matrix<double> result)
        {
            Map2((x, y) => x - y, other, result);
        }

        /// <summary>
        ///     Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        protected override void DoMultiply(double scalar, Matrix<double> result)
        {
            Map(x => x * scalar, result);
        }

        /// <summary>
        ///     Multiplies this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Vector<double> rightSide, Vector<double> result)
        {
            for (var i = 0; i < RowCount; i++)
            {
                var s = 0.0;
                for (var j = 0; j < ColumnCount; j++) s += At(i, j) * rightSide[j];
                result[i] = s;
            }
        }

        /// <summary>
        ///     Divides each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="divisor">The scalar to divide the matrix with.</param>
        /// <param name="result">The matrix to store the result of the division.</param>
        protected virtual void DoDivide(double divisor, Matrix<double> result)
        {
            Map(x => x / divisor, result, divisor == 0.0 ? Zeros.Include : Zeros.AllowSkip);
        }

        /// <summary>
        ///     Multiplies this matrix with another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Matrix<double> other, Matrix<double> result)
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < other.ColumnCount; j++)
            {
                var s = 0.0;
                for (var k = 0; k < ColumnCount; k++) s += At(i, k) * other.At(k, j);
                result.At(i, j, s);
            }
        }
        
        
        
        /// <summary>
        ///     Multiplies the transpose of this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoTransposeThisAndMultiply(Vector<double> rightSide, Vector<double> result)
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                var s = 0.0;
                for (var i = 0; i < RowCount; i++) s += At(i, j) * rightSide[i];
                result[j] = s;
            }
        }
        

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given divisor each element of the matrix.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">Matrix to store the results in.</param>
        protected override void DoRemainder(double divisor, Matrix<double> result)
        {
            Map(x => Euclid.Remainder(x, divisor), result, Zeros.Include);
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given dividend for each element of the matrix.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoRemainderByThis(double dividend, Matrix<double> result)
        {
            Map(x => Euclid.Remainder(dividend, x), result, Zeros.Include);
        }

        /// <summary>
        ///     Pointwise remainder (% operator), where the result has the sign of the dividend,
        ///     of this matrix with another matrix and stores the result into the result matrix.
        /// </summary>
        /// <param name="divisor">The pointwise denominator matrix to use</param>
        /// <param name="result">The result of the modulus.</param>
        protected override void DoPointwiseRemainder(Matrix<double> divisor, Matrix<double> result)
        {
            Map2(Euclid.Remainder, divisor, result, Zeros.Include);
        }

        /// <summary>
        ///     Pointwise applies the exponential function to each value and stores the result into the result matrix.
        /// </summary>
        /// <param name="result">The matrix to store the result.</param>
        protected override void DoPointwiseExp(Matrix<double> result)
        {
            Map(Math.Exp, result, Zeros.Include);
        }
        protected override void DoPointwiseAbs(Matrix<double> result)
        {
            Map(Math.Abs, result);
        }
        
        protected override void DoPointwiseAtan(Matrix<double> result)
        {
            Map(Math.Atan, result);
        }
        
        protected override void DoPointwiseRound(Matrix<double> result)
        {
            Map(Math.Round, result);
        }
        

        protected override void DoPointwiseTan(Matrix<double> result)
        {
            Map(Math.Tan, result);
        }

        /// <summary>
        ///     Computes the trace of this matrix.
        /// </summary>
        /// <returns>The trace of this matrix</returns>
        /// <exceptioncreArgumentException">If the matrix is not square
        /// 
        /// </exception>
        public override double Trace()
        {
            if (RowCount != ColumnCount)
            {
                //throw new ArgumentException(Resources.ArgumentMatrixSquare);
            }

            var sum = 0.0;
            for (var i = 0; i < RowCount; i++) sum += At(i, i);

            return sum;
        }
        
        public override LU<double> LU()
        {
            return UserLU.Create(this);
        }

        public override QR<double> QR(QRMethod method = QRMethod.Thin)
        {
            return UserQR.Create(this, method);
        }
        
    }
}