// <copyright file="SparseMatrix.cs" company="Math.NET">
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
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.Providers.LinearAlgebra;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    ///     A Matrix with sparse storage, intended for very large matrices where most of the cells are zero.
    ///     The underlying storage scheme is 3-array compressed-sparse-row (CSR) Format.
    ///     <a href="http://en.wikipedia.org/wiki/Sparse_matrix#Compressed_sparse_row_.28CSR_or_CRS.29">Wikipedia - CSR</a>.
    /// </summary>
    [DebuggerDisplay("SparseMatrix {RowCount}x{ColumnCount}-Double {NonZerosCount}-NonZero")]
    public class SparseMatrix : Matrix
    {
        private readonly SparseCompressedRowMatrixStorage<double> _storage;

        /// <summary>
        ///     Create a new sparse matrix straight from an initialized matrix storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public SparseMatrix(SparseCompressedRowMatrixStorage<double> storage)
            : base(storage)
        {
            _storage = storage;
        }

        /// <summary>
        ///     Create a new sparse matrix with the given number of rows and columns.
        ///     All cells of the matrix will be initialized to zero.
        ///     Zero-length matrices are not supported.
        /// </summary>
        /// <exceptioncreArgumentException">If the row or column count is less than one.
        /// 
        /// </exception>
        public SparseMatrix(int rows, int columns)
            : this(new SparseCompressedRowMatrixStorage<double>(rows, columns))
        {
        }

        /// <summary>
        ///     Gets the number of non zero elements in the matrix.
        /// </summary>
        /// <value>The number of non zero elements.</value>
        public int NonZerosCount => _storage.ValueCount;

        /// <summary>
        ///     Create a new sparse matrix and initialize each value to the same provided value.
        /// </summary>
        public static SparseMatrix Create(int rows, int columns, double value)
        {
            if (value == 0d) return new SparseMatrix(rows, columns);
            return new SparseMatrix(SparseCompressedRowMatrixStorage<double>.OfValue(rows, columns, value));
        }

        /// <summary>
        ///     Create a new sparse matrix and initialize each value using the provided init function.
        /// </summary>
        public static SparseMatrix Create(int rows, int columns, Func<int, int, double> init)
        {
            return new SparseMatrix(SparseCompressedRowMatrixStorage<double>.OfInit(rows, columns, init));
        }

        /// <summary>
        ///     Returns a new matrix containing the lower triangle of this matrix.
        /// </summary>
        /// <returns>The lower triangle of this matrix.</returns>
        public override Matrix<double> LowerTriangle()
        {
            var result = Build.SameAs(this);
            LowerTriangleImpl(result);
            return result;
        }

        /// <summary>
        ///     Puts the lower triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="result is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentException">If the result matrix's dimensions are not the same as this matrix.
        /// 
        /// </exception>
        public void LowerTriangle(Matrix<double> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentException>(this, result, "result");

            if (ReferenceEquals(this, result))
            {
                var tmp = Build.SameAs(result);
                LowerTriangle(tmp);
                tmp.CopyTo(result);
            }
            else
            {
                result.Clear();
                LowerTriangleImpl(result);
            }
        }

        /// <summary>
        ///     Puts the lower triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        private void LowerTriangleImpl(Matrix<double> result)
        {
            var rowPointers = _storage.RowPointers;
            var columnIndices = _storage.ColumnIndices;
            var values = _storage.Values;

            for (var row = 0; row < result.RowCount; row++)
            {
                var endIndex = rowPointers[row + 1];
                for (var j = rowPointers[row]; j < endIndex; j++)
                    if (row >= columnIndices[j])
                        result.At(row, columnIndices[j], values[j]);
            }
        }

        /// <summary>
        ///     Returns a new matrix containing the upper triangle of this matrix.
        /// </summary>
        /// <returns>The upper triangle of this matrix.</returns>
        public override Matrix<double> UpperTriangle()
        {
            var result = Build.SameAs(this);
            UpperTriangleImpl(result);
            return result;
        }

        /// <summary>
        ///     Puts the upper triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="result is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentException">If the result matrix's dimensions are not the same as this matrix.
        /// 
        /// </exception>
        public void UpperTriangle(Matrix<double> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            if (result.RowCount != RowCount || result.ColumnCount != ColumnCount)
                throw DimensionsDontMatch<ArgumentException>(this, result, "result");

            if (ReferenceEquals(this, result))
            {
                var tmp = Build.SameAs(result);
                UpperTriangle(tmp);
                tmp.CopyTo(result);
            }
            else
            {
                result.Clear();
                UpperTriangleImpl(result);
            }
        }

        /// <summary>
        ///     Puts the upper triangle of this matrix into the result matrix.
        /// </summary>
        /// <param name="result">Where to store the lower triangle.</param>
        private void UpperTriangleImpl(Matrix<double> result)
        {
            var rowPointers = _storage.RowPointers;
            var columnIndices = _storage.ColumnIndices;
            var values = _storage.Values;

            for (var row = 0; row < result.RowCount; row++)
            {
                var endIndex = rowPointers[row + 1];
                for (var j = rowPointers[row]; j < endIndex; j++)
                    if (row <= columnIndices[j])
                        result.At(row, columnIndices[j], values[j]);
            }
        }

        /// <summary>
        ///     Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        protected override void DoNegate(Matrix<double> result)
        {
            CopyTo(result);
            DoMultiply(-1, result);
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
            var sparseOther = other as SparseMatrix;
            var sparseResult = result as SparseMatrix;
            if (sparseOther == null || sparseResult == null)
            {
                base.DoAdd(other, result);
                return;
            }

            if (ReferenceEquals(this, other))
            {
                if (!ReferenceEquals(this, result)) CopyTo(result);

                LinearAlgebraControl.Provider.ScaleArray(2.0, sparseResult._storage.Values,
                    sparseResult._storage.Values);
                return;
            }

            SparseMatrix left;

            if (ReferenceEquals(sparseOther, sparseResult))
            {
                left = this;
            }
            else if (ReferenceEquals(this, sparseResult))
            {
                left = sparseOther;
            }
            else
            {
                CopyTo(sparseResult);
                left = sparseOther;
            }

            var leftStorage = left._storage;
            for (var i = 0; i < leftStorage.RowCount; i++)
            {
                var endIndex = leftStorage.RowPointers[i + 1];
                for (var j = leftStorage.RowPointers[i]; j < endIndex; j++)
                {
                    var columnIndex = leftStorage.ColumnIndices[j];
                    var resVal = leftStorage.Values[j] + result.At(i, columnIndex);
                    result.At(i, columnIndex, resVal);
                }
            }
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
            var sparseOther = other as SparseMatrix;
            var sparseResult = result as SparseMatrix;

            if (sparseOther == null || sparseResult == null)
            {
                base.DoSubtract(other, result);
                return;
            }

            if (ReferenceEquals(this, other))
            {
                result.Clear();
                return;
            }

            var otherStorage = sparseOther._storage;

            if (ReferenceEquals(this, sparseResult))
            {
                for (var i = 0; i < otherStorage.RowCount; i++)
                {
                    var endIndex = otherStorage.RowPointers[i + 1];
                    for (var j = otherStorage.RowPointers[i]; j < endIndex; j++)
                    {
                        var columnIndex = otherStorage.ColumnIndices[j];
                        var resVal = sparseResult.At(i, columnIndex) - otherStorage.Values[j];
                        result.At(i, columnIndex, resVal);
                    }
                }
            }
            else
            {
                if (!ReferenceEquals(sparseOther, sparseResult)) sparseOther.CopyTo(sparseResult);

                sparseResult.Negate(sparseResult);

                var rowPointers = _storage.RowPointers;
                var columnIndices = _storage.ColumnIndices;
                var values = _storage.Values;

                for (var i = 0; i < RowCount; i++)
                {
                    var endIndex = rowPointers[i + 1];
                    for (var j = rowPointers[i]; j < endIndex; j++)
                    {
                        var columnIndex = columnIndices[j];
                        var resVal = sparseResult.At(i, columnIndex) + values[j];
                        result.At(i, columnIndex, resVal);
                    }
                }
            }
        }

        /// <summary>
        ///     Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        protected override void DoMultiply(double scalar, Matrix<double> result)
        {
            if (scalar == 1.0)
            {
                CopyTo(result);
                return;
            }

            if (scalar == 0.0 || NonZerosCount == 0)
            {
                result.Clear();
                return;
            }

            var sparseResult = result as SparseMatrix;
            if (sparseResult == null)
            {
                result.Clear();

                var rowPointers = _storage.RowPointers;
                var columnIndices = _storage.ColumnIndices;
                var values = _storage.Values;

                for (var row = 0; row < RowCount; row++)
                {
                    var start = rowPointers[row];
                    var end = rowPointers[row + 1];

                    if (start == end) continue;

                    for (var index = start; index < end; index++)
                    {
                        var column = columnIndices[index];
                        result.At(row, column, values[index] * scalar);
                    }
                }
            }
            else
            {
                if (!ReferenceEquals(this, result)) CopyTo(sparseResult);

                LinearAlgebraControl.Provider.ScaleArray(scalar, sparseResult._storage.Values,
                    sparseResult._storage.Values);
            }
        }

        /// <summary>
        ///     Multiplies this matrix with another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Matrix<double> other, Matrix<double> result)
        {
            var sparseOther = other as SparseMatrix;
            var sparseResult = result as SparseMatrix;
            if (sparseOther != null && sparseResult != null)
            {
                DoMultiplySparse(sparseOther, sparseResult);
                return;
            }

            var diagonalOther = other.Storage as DiagonalMatrixStorage<double>;
            if (diagonalOther != null && sparseResult != null)
            {
                var diagonal = diagonalOther.Data;
                if (other.ColumnCount == other.RowCount)
                {
                    Storage.MapIndexedTo(result.Storage, (i, j, x) => x * diagonal[j], Zeros.AllowSkip,
                        ExistingData.Clear);
                }
                else
                {
                    result.Storage.Clear();
                    Storage.MapSubMatrixIndexedTo(result.Storage, (i, j, x) => x * diagonal[j], 0, 0, RowCount, 0, 0,
                        ColumnCount, Zeros.AllowSkip, ExistingData.AssumeZeros);
                }

                return;
            }

            result.Clear();
            var rowPointers = _storage.RowPointers;
            var columnIndices = _storage.ColumnIndices;
            var values = _storage.Values;

            var denseOther = other.Storage as DenseColumnMajorMatrixStorage<double>;
            if (denseOther != null)
            {
                // in this case we can directly address the underlying data-array
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = rowPointers[row];
                    var endIndex = rowPointers[row + 1];

                    if (startIndex == endIndex) continue;

                    for (var column = 0; column < other.ColumnCount; column++)
                    {
                        var otherColumnStartPosition = column * other.RowCount;
                        var sum = 0d;
                        for (var index = startIndex; index < endIndex; index++)
                            sum += values[index] * denseOther.Data[otherColumnStartPosition + columnIndices[index]];

                        result.At(row, column, sum);
                    }
                }

                return;
            }

            var columnVector = new DenseVector(other.RowCount);
            for (var row = 0; row < RowCount; row++)
            {
                var startIndex = rowPointers[row];
                var endIndex = rowPointers[row + 1];

                if (startIndex == endIndex) continue;

                for (var column = 0; column < other.ColumnCount; column++)
                {
                    // Multiply row of matrix A on column of matrix B
                    other.Column(column, columnVector);

                    var sum = 0d;
                    for (var index = startIndex; index < endIndex; index++)
                        sum += values[index] * columnVector[columnIndices[index]];

                    result.At(row, column, sum);
                }
            }
        }

        private void DoMultiplySparse(SparseMatrix other, SparseMatrix result)
        {
            result.Clear();

            var ax = _storage.Values;
            var ap = _storage.RowPointers;
            var ai = _storage.ColumnIndices;

            var bx = other._storage.Values;
            var bp = other._storage.RowPointers;
            var bi = other._storage.ColumnIndices;

            var rows = RowCount;
            var cols = other.ColumnCount;

            var cp = result._storage.RowPointers;

            var marker = new int[cols];
            for (var ib = 0; ib < cols; ib++) marker[ib] = -1;

            var count = 0;
            for (var i = 0; i < rows; i++)
            {
                // For each row of A
                for (var j = ap[i]; j < ap[i + 1]; j++)
                {
                    // Row number to be added
                    var a = ai[j];
                    for (var k = bp[a]; k < bp[a + 1]; k++)
                    {
                        var b = bi[k];
                        if (marker[b] != i)
                        {
                            marker[b] = i;
                            count++;
                        }
                    }
                }

                // Record non-zero count.
                cp[i + 1] = count;
            }

            var ci = new int[count];
            var cx = new double[count];

            for (var ib = 0; ib < cols; ib++) marker[ib] = -1;

            count = 0;
            for (var i = 0; i < rows; i++)
            {
                var rowStart = cp[i];
                for (var j = ap[i]; j < ap[i + 1]; j++)
                {
                    var a = ai[j];
                    var aEntry = ax[j];
                    for (var k = bp[a]; k < bp[a + 1]; k++)
                    {
                        var b = bi[k];
                        var bEntry = bx[k];
                        if (marker[b] < rowStart)
                        {
                            marker[b] = count;
                            ci[marker[b]] = b;
                            cx[marker[b]] = aEntry * bEntry;
                            count++;
                        }
                        else
                        {
                            cx[marker[b]] += aEntry * bEntry;
                        }
                    }
                }
            }

            result._storage.Values = cx;
            result._storage.ColumnIndices = ci;
            result._storage.Normalize();
        }

        /// <summary>
        ///     Multiplies this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Vector<double> rightSide, Vector<double> result)
        {
            var rowPointers = _storage.RowPointers;
            var columnIndices = _storage.ColumnIndices;
            var values = _storage.Values;

            for (var row = 0; row < RowCount; row++)
            {
                var startIndex = rowPointers[row];
                var endIndex = rowPointers[row + 1];

                if (startIndex == endIndex) continue;

                var sum = 0d;
                for (var index = startIndex; index < endIndex; index++)
                    sum += values[index] * rightSide[columnIndices[index]];

                result[row] = sum;
            }
        }

        /// <summary>
        ///     Multiplies the transpose of this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoTransposeThisAndMultiply(Vector<double> rightSide, Vector<double> result)
        {
            var rowPointers = _storage.RowPointers;
            var columnIndices = _storage.ColumnIndices;
            var values = _storage.Values;

            for (var row = 0; row < RowCount; row++)
            {
                var startIndex = rowPointers[row];
                var endIndex = rowPointers[row + 1];

                if (startIndex == endIndex) continue;

                var rightSideValue = rightSide[row];
                for (var index = startIndex; index < endIndex; index++)
                    result[columnIndices[index]] += values[index] * rightSideValue;
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
            var sparseResult = result as SparseMatrix;
            if (sparseResult == null)
            {
                base.DoRemainder(divisor, result);
                return;
            }

            if (!ReferenceEquals(this, result)) CopyTo(result);

            var resultStorage = sparseResult._storage;
            for (var index = 0; index < resultStorage.Values.Length; index++) resultStorage.Values[index] %= divisor;
        }

        /// <summary>
        ///     Adds two matrices together and returns the results.
        /// </summary>
        /// <remarks>
        ///     This operator will allocate new memory for the result. It will
        ///     choose the representation of either <paramref name="leftSide or 
        ///     
        ///     <paramref name="rightSide depending on which
        /// is denser.
        /// 
        /// </remarks>
        /// <param name="leftSide">The left matrix to add.</param>
        /// <param name="rightSide">The right matrix to add.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide don't have the same dimensions.
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
        public static SparseMatrix operator +(SparseMatrix leftSide, SparseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (leftSide.RowCount != rightSide.RowCount || leftSide.ColumnCount != rightSide.ColumnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(leftSide, rightSide);

            return (SparseMatrix) leftSide.Add(rightSide);
        }

        /// <summary>
        ///     Returns a <strong>Matrix</strong> containing the same values of <paramref name="rightSide.
        /// 
        /// 
        /// </summary>
        /// <param name="rightSide">The matrix to get the values from.</param>
        /// <returns>A matrix containing a the same values as <paramref name="rightSide.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseMatrix operator +(SparseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (SparseMatrix) rightSide.Clone();
        }

        /// <summary>
        ///     Subtracts two matrices together and returns the results.
        /// </summary>
        /// <remarks>
        ///     This operator will allocate new memory for the result. It will
        ///     choose the representation of either <paramref name="leftSide or 
        ///     
        ///     <paramref name="rightSide depending on which
        /// is denser.
        /// 
        /// </remarks>
        /// <param name="leftSide">The left matrix to subtract.</param>
        /// <param name="rightSide">The right matrix to subtract.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide don't have the same dimensions.
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
        public static SparseMatrix operator -(SparseMatrix leftSide, SparseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (leftSide.RowCount != rightSide.RowCount || leftSide.ColumnCount != rightSide.ColumnCount)
                throw DimensionsDontMatch<ArgumentException>(leftSide, rightSide);

            return (SparseMatrix) leftSide.Subtract(rightSide);
        }

        /// <summary>
        ///     Negates each element of the matrix.
        /// </summary>
        /// <param name="rightSide">The matrix to negate.</param>
        /// <returns>A matrix containing the negated values.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseMatrix operator -(SparseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (SparseMatrix) rightSide.Negate();
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> by a constant and returns the result.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The constant to multiply the matrix by.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseMatrix operator *(SparseMatrix leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseMatrix) leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> by a constant and returns the result.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The constant to multiply the matrix by.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseMatrix operator *(double leftSide, SparseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (SparseMatrix) rightSide.Multiply(leftSide);
        }

        /// <summary>
        ///     Multiplies two matrices.
        /// </summary>
        /// <remarks>
        ///     This operator will allocate new memory for the result. It will
        ///     choose the representation of either <paramref name="leftSide or 
        ///     
        ///     <paramref name="rightSide depending on which
        /// is denser.
        /// 
        /// </remarks>
        /// <param name="leftSide">The left matrix to multiply.</param>
        /// <param name="rightSide">The right matrix to multiply.</param>
        /// <returns>The result of multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentException">If the dimensions of 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide don't conform.
        /// 
        /// </exception>
        public static SparseMatrix operator *(SparseMatrix leftSide, SparseMatrix rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide.ColumnCount != rightSide.RowCount)
                throw DimensionsDontMatch<ArgumentException>(leftSide, rightSide);

            return (SparseMatrix) leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> and a Vector.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The vector to multiply.</param>
        /// <returns>The result of multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator *(SparseMatrix leftSide, SparseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseVector) leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a Vector and a <strong>Matrix</strong>.
        /// </summary>
        /// <param name="leftSide">The vector to multiply.</param>
        /// <param name="rightSide">The matrix to multiply.</param>
        /// <returns>The result of multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseVector operator *(SparseVector leftSide, SparseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (SparseVector) rightSide.LeftMultiply(leftSide);
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> by a constant and returns the result.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The constant to multiply the matrix by.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static SparseMatrix operator %(SparseMatrix leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (SparseMatrix) leftSide.Remainder(rightSide);
        }

        public override string ToTypeString()
        {
            return string.Format("SparseMatrix {0}x{1}-Double {2:P2} Filled", RowCount, ColumnCount,
                NonZerosCount / (RowCount * (double) ColumnCount));
        }
    }
}