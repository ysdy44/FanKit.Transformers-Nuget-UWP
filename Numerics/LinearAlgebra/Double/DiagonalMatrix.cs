// <copyright file="DiagonalMatrix.cs" company="Math.NET">
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
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.Providers.LinearAlgebra;
using MathNet.Numerics.Threading;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    ///     A matrix type for diagonal matrices.
    /// </summary>
    /// <remarks>
    ///     Diagonal matrices can be non-square matrices but the diagonal always starts
    ///     at element 0,0. A diagonal matrix will throw an exception if non diagonal
    ///     entries are set. The exception to this is when the off diagonal elements are
    ///     0.0 or NaN; these settings will cause no change to the diagonal matrix.
    /// </remarks>
    [DebuggerDisplay("DiagonalMatrix {RowCount}x{ColumnCount}-Double")]
    public class DiagonalMatrix : Matrix
    {
        /// <summary>
        ///     Gets the matrix's data.
        /// </summary>
        /// <value>The matrix's data.</value>
        private readonly double[] _data;

        /// <summary>
        ///     Create a new diagonal matrix straight from an initialized matrix storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public DiagonalMatrix(DiagonalMatrixStorage<double> storage)
            : base(storage)
        {
            _data = storage.Data;
        }


        /// <summary>
        ///     Create a new diagonal matrix with the given number of rows and columns.
        ///     All cells of the matrix will be initialized to zero.
        ///     Zero-length matrices are not supported.
        /// </summary>
        /// <exceptioncreArgumentException">If the row or column count is less than one.
        /// 
        /// </exception>
        public DiagonalMatrix(int rows, int columns)
            : this(new DiagonalMatrixStorage<double>(rows, columns))
        {
        }

        /// <summary>
        ///     Create a new diagonal matrix and initialize each diagonal value using the provided init function.
        /// </summary>
        public static DiagonalMatrix Create(int rows, int columns, Func<int, double> init)
        {
            return new DiagonalMatrix(DiagonalMatrixStorage<double>.OfInit(rows, columns, init));
        }


        /// <summary>
        ///     Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        protected override void DoNegate(Matrix<double> result)
        {
            var diagResult = result as DiagonalMatrix;
            if (diagResult != null)
            {
                LinearAlgebraControl.Provider.ScaleArray(-1, _data, diagResult._data);
                return;
            }

            result.Clear();
            for (var i = 0; i < _data.Length; i++) result.At(i, i, -_data[i]);
        }

        /// <summary>
        ///     Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        protected override void DoAdd(Matrix<double> other, Matrix<double> result)
        {
            // diagonal + diagonal = diagonal
            var diagOther = other as DiagonalMatrix;
            var diagResult = result as DiagonalMatrix;
            if (diagOther != null && diagResult != null)
            {
                LinearAlgebraControl.Provider.AddArrays(_data, diagOther._data, diagResult._data);
                return;
            }

            other.CopyTo(result);
            for (var i = 0; i < _data.Length; i++) result.At(i, i, result.At(i, i) + _data[i]);
        }

        /// <summary>
        ///     Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        protected override void DoSubtract(Matrix<double> other, Matrix<double> result)
        {
            // diagonal - diagonal = diagonal
            var diagOther = other as DiagonalMatrix;
            var diagResult = result as DiagonalMatrix;
            if (diagOther != null && diagResult != null)
            {
                LinearAlgebraControl.Provider.SubtractArrays(_data, diagOther._data, diagResult._data);
                return;
            }

            other.Negate(result);
            for (var i = 0; i < _data.Length; i++) result.At(i, i, result.At(i, i) + _data[i]);
        }

        /// <summary>
        ///     Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        /// <exceptioncreArgumentException">If the result matrix's dimensions are not the same as this matrix.
        /// 
        /// </exception>
        protected override void DoMultiply(double scalar, Matrix<double> result)
        {
            if (scalar == 0.0)
            {
                result.Clear();
                return;
            }

            if (scalar == 1.0)
            {
                CopyTo(result);
                return;
            }

            var diagResult = result as DiagonalMatrix;
            if (diagResult == null)
                base.DoMultiply(scalar, result);
            else
                LinearAlgebraControl.Provider.ScaleArray(scalar, _data, diagResult._data);
        }

        /// <summary>
        ///     Multiplies this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Vector<double> rightSide, Vector<double> result)
        {
            var d = Math.Min(ColumnCount, RowCount);
            if (d < RowCount) result.ClearSubVector(ColumnCount, RowCount - ColumnCount);

            if (d == ColumnCount)
            {
                var denseOther = rightSide.Storage as DenseVectorStorage<double>;
                var denseResult = result.Storage as DenseVectorStorage<double>;
                if (denseOther != null && denseResult != null)
                {
                    LinearAlgebraControl.Provider.PointWiseMultiplyArrays(_data, denseOther.Data, denseResult.Data);
                    return;
                }
            }

            for (var i = 0; i < d; i++) result.At(i, _data[i] * rightSide.At(i));
        }

        /// <summary>
        ///     Multiplies this matrix with another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Matrix<double> other, Matrix<double> result)
        {
            var diagonalOther = other as DiagonalMatrix;
            var diagonalResult = result as DiagonalMatrix;
            if (diagonalOther != null && diagonalResult != null)
            {
                var thisDataCopy = new double[diagonalResult._data.Length];
                var otherDataCopy = new double[diagonalResult._data.Length];
                Array.Copy(_data, 0, thisDataCopy, 0,
                    diagonalResult._data.Length > _data.Length ? _data.Length : diagonalResult._data.Length);
                Array.Copy(diagonalOther._data, 0, otherDataCopy, 0,
                    diagonalResult._data.Length > diagonalOther._data.Length
                        ? diagonalOther._data.Length
                        : diagonalResult._data.Length);
                LinearAlgebraControl.Provider.PointWiseMultiplyArrays(thisDataCopy, otherDataCopy,
                    diagonalResult._data);
                return;
            }

            var denseOther = other.Storage as DenseColumnMajorMatrixStorage<double>;
            if (denseOther != null)
            {
                var dense = denseOther.Data;
                var diagonal = _data;
                var d = Math.Min(denseOther.RowCount, RowCount);
                if (d < RowCount)
                    result.ClearSubMatrix(denseOther.RowCount, RowCount - denseOther.RowCount, 0,
                        denseOther.ColumnCount);
                var index = 0;
                for (var i = 0; i < denseOther.ColumnCount; i++)
                {
                    for (var j = 0; j < d; j++)
                    {
                        result.At(j, i, dense[index] * diagonal[j]);
                        index++;
                    }

                    index += denseOther.RowCount - d;
                }

                return;
            }

            if (ColumnCount == RowCount)
            {
                other.Storage.MapIndexedTo(result.Storage, (i, j, x) => x * _data[i], Zeros.AllowSkip,
                    ExistingData.Clear);
            }
            else
            {
                result.Clear();
                other.Storage.MapSubMatrixIndexedTo(result.Storage, (i, j, x) => x * _data[i], 0, 0, other.RowCount, 0,
                    0, other.ColumnCount, Zeros.AllowSkip, ExistingData.AssumeZeros);
            }
        }

        /// <summary>
        ///     Multiplies the transpose of this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoTransposeThisAndMultiply(Vector<double> rightSide, Vector<double> result)
        {
            var d = Math.Min(ColumnCount, RowCount);
            if (d < ColumnCount) result.ClearSubVector(RowCount, ColumnCount - RowCount);

            if (d == RowCount)
            {
                var denseOther = rightSide.Storage as DenseVectorStorage<double>;
                var denseResult = result.Storage as DenseVectorStorage<double>;
                if (denseOther != null && denseResult != null)
                {
                    LinearAlgebraControl.Provider.PointWiseMultiplyArrays(_data, denseOther.Data, denseResult.Data);
                    return;
                }
            }

            for (var i = 0; i < d; i++) result.At(i, _data[i] * rightSide.At(i));
        }

        /// <summary>
        ///     Divides each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="divisor">The scalar to divide the matrix with.</param>
        /// <param name="result">The matrix to store the result of the division.</param>
        protected override void DoDivide(double divisor, Matrix<double> result)
        {
            if (divisor == 1.0)
            {
                CopyTo(result);
                return;
            }

            var diagResult = result as DiagonalMatrix;
            if (diagResult != null)
            {
                LinearAlgebraControl.Provider.ScaleArray(1.0 / divisor, _data, diagResult._data);
                return;
            }

            result.Clear();
            for (var i = 0; i < _data.Length; i++) result.At(i, i, _data[i] / divisor);
        }


        /// <summary>
        ///     Copies the values of the given <seecreVector{ T} to the diagonal.
        /// </summary>
        /// <param name="source">
        ///     The vector to copy the values from. The length of the vector should be
        ///     Min(Rows, Columns).
        /// </param>
        /// <exceptioncreArgumentException">If the length of 
        /// 
        /// <paramref name="source does not
        /// equal Min(Rows, Columns).
        /// 
        /// </exception>
        /// <remarks>For non-square matrices, the elements of <paramref name="source are copied to
        /// this[i,i].</remarks>
        public override void SetDiagonal(Vector<double> source)
        {
            var denseSource = source as DenseVector;
            if (denseSource == null)
            {
                base.SetDiagonal(source);
                return;
            }

            if (_data.Length != denseSource.Values.Length)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(source));
            }

            Buffer.BlockCopy(denseSource.Values, 0, _data, 0, denseSource.Values.Length * Constants.SizeOfDouble);
        }
        
        /// <summary>
        ///     Returns a new matrix containing the lower triangle of this matrix.
        /// </summary>
        /// <returns>The lower triangle of this matrix.</returns>
        public override Matrix<double> LowerTriangle()
        {
            return Clone();
        }
        

        /// <summary>
        ///     Returns a new matrix containing the upper triangle of this matrix.
        /// </summary>
        /// <returns>The upper triangle of this matrix.</returns>
        public override Matrix<double> UpperTriangle()
        {
            return Clone();
        }

        /// <summary>
        ///     Creates a matrix that contains the values from the requested sub-matrix.
        /// </summary>
        /// <param name="rowIndex">The row to start copying from.</param>
        /// <param name="rowCount">The number of rows to copy. Must be positive.</param>
        /// <param name="columnIndex">The column to start copying from.</param>
        /// <param name="columnCount">The number of columns to copy. Must be positive.</param>
        /// <returns>The requested sub-matrix.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If: 
        /// 
        /// <list>
        ///     <item>
        ///         <paramref name="rowIndex is
        /// negative, or greater than or equal to the number of rows.
        ///     
        ///     </item>
        ///     <item>
        ///         <paramref name="columnIndex is negative, or greater than or equal to the number
        /// of columns.
        ///     
        ///     </item>
        ///     <item>
        ///         <c>(columnIndex + columnLength) &gt;= Columns</c>
        ///     </item>
        ///     <item>
        ///         <c>(rowIndex + rowLength) &gt;= Rows</c>
        ///     </item>
        /// </list>
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="rowCount or 
        /// 
        /// <paramref name="columnCount
        /// is not positive.
        /// 
        /// </exception>
        public override Matrix<double> SubMatrix(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            var target = rowIndex == columnIndex
                ? (Matrix<double>) new DiagonalMatrix(rowCount, columnCount)
                : new SparseMatrix(rowCount, columnCount);

            Storage.CopySubMatrixTo(target.Storage, rowIndex, 0, rowCount, columnIndex, 0, columnCount,
                ExistingData.AssumeZeros);
            return target;
        }


        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given divisor each element of the matrix.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">Matrix to store the results in.</param>
        protected override void DoRemainder(double divisor, Matrix<double> result)
        {
            var diagonalResult = result as DiagonalMatrix;
            if (diagonalResult == null)
            {
                base.DoRemainder(divisor, result);
                return;
            }

            CommonParallel.For(0, _data.Length, 4096, (a, b) =>
            {
                var r = diagonalResult._data;
                for (var i = a; i < b; i++) r[i] = _data[i] % divisor;
            });
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given dividend for each element of the matrix.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoRemainderByThis(double dividend, Matrix<double> result)
        {
            var diagonalResult = result as DiagonalMatrix;
            if (diagonalResult == null)
            {
                base.DoRemainderByThis(dividend, result);
                return;
            }

            CommonParallel.For(0, _data.Length, 4096, (a, b) =>
            {
                var r = diagonalResult._data;
                for (var i = a; i < b; i++) r[i] = dividend % _data[i];
            });
        }
    }
}