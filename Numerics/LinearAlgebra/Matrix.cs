// <copyright file="Matrix.cs" company="Math.NET">
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
using System.Runtime;
using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    ///     Defines the base class for <c>Matrix</c> classes.
    /// </summary>
    /// <typeparam name="T">
    ///     Supported data types are <c>double</c>, <c>single</c>, <seecreComplex, and <seecreComplex32.
    /// </typeparam>
    public abstract partial class Matrix<T> :
        IFormattable, IEquatable<Matrix<T>>
        where T : struct, IEquatable<T>, IFormattable
    {
        public static readonly MatrixBuilder<T> Build = BuilderInstance<T>.Matrix;

        /// <summary>
        ///     Initializes a new instance of the Matrix class.
        /// </summary>
        protected Matrix(MatrixStorage<T> storage)
        {
            Storage = storage;
            RowCount = storage.RowCount;
            ColumnCount = storage.ColumnCount;
        }

        /// <summary>
        ///     Gets the raw matrix data storage.
        /// </summary>
        public MatrixStorage<T> Storage { get; }

        /// <summary>
        ///     Gets the number of columns.
        /// </summary>
        /// <value>The number of columns.</value>
        public int ColumnCount { get; }

        /// <summary>
        ///     Gets the number of rows.
        /// </summary>
        /// <value>The number of rows.</value>
        public int RowCount { get; }

        /// <summary>
        ///     Gets or sets the value at the given row and column, with range checking.
        /// </summary>
        /// <param name="row">
        ///     The row of the element.
        /// </param>
        /// <param name="column">
        ///     The column of the element.
        /// </param>
        /// <value>The value to get or set.</value>
        /// <remarks>
        ///     This method is ranged checked. <seecreAt( int, int) and
        ///     <seecreAt( int, int, T)
        ///         to get and set values without range checking.
        /// </remarks>
        public T this[int row, int column]
        {
#if !NET40
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get { return Storage[row, column]; }

#if !NET40
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            set { Storage[row, column] = value; }
        }

        /// <summary>
        ///     Retrieves the requested element without range checking.
        /// </summary>
        /// <param name="row">
        ///     The row of the element.
        /// </param>
        /// <param name="column">
        ///     The column of the element.
        /// </param>
        /// <returns>
        ///     The requested element.
        /// </returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public T At(int row, int column)
        {
            return Storage.At(row, column);
        }

        /// <summary>
        ///     Sets the value of the given element without range checking.
        /// </summary>
        /// <param name="row">
        ///     The row of the element.
        /// </param>
        /// <param name="column">
        ///     The column of the element.
        /// </param>
        /// <param name="value">
        ///     The value to set the element to.
        /// </param>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void At(int row, int column, T value)
        {
            Storage.At(row, column, value);
        }

        /// <summary>
        ///     Sets all values to zero.
        /// </summary>
        public void Clear()
        {
            Storage.Clear();
        }


        /// <summary>
        ///     Sets all values of a sub-matrix to zero.
        /// </summary>
        public void ClearSubMatrix(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            Storage.Clear(rowIndex, rowCount, columnIndex, columnCount);
        }

        /// <summary>
        ///     Creates a clone of this instance.
        /// </summary>
        /// <returns>
        ///     A clone of the instance.
        /// </returns>
        public Matrix<T> Clone()
        {
            var result = Build.SameAs(this);
            Storage.CopyToUnchecked(result.Storage, ExistingData.AssumeZeros);
            return result;
        }

        /// <summary>
        ///     Copies the elements of this matrix to the given matrix.
        /// </summary>
        /// <param name="target">
        ///     The matrix to copy values into.
        /// </param>
        /// <exceptioncreArgumentNullException">
        /// If target is 
        /// 
        /// <see langword="null.
        /// 
        /// 
        /// </exception>
        /// <exceptioncreArgumentException">
        /// If this and the target matrix do not have the same dimensions..
        /// 
        /// 
        /// </exception>
        public void CopyTo(Matrix<T> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            Storage.CopyTo(target.Storage);
        }

        /// <summary>
        ///     Copies a row into an Vector.
        /// </summary>
        /// <param name="index">The row to copy.</param>
        /// <returns>A Vector containing the copied elements.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="index is negative,
        /// or greater than or equal to the number of rows.
        /// 
        /// </exception>
        public Vector<T> Row(int index)
        {
            if ((uint) index >= (uint) RowCount) throw new ArgumentOutOfRangeException(nameof(index));

            var ret = Vector<T>.Build.SameAs(this, ColumnCount);
            Storage.CopySubRowToUnchecked(ret.Storage, index, 0, 0, ColumnCount, ExistingData.AssumeZeros);
            return ret;
        }

        /// <summary>
        ///     Copies a row into to the given Vector.
        /// </summary>
        /// <param name="index">The row to copy.</param>
        /// <param name="result">The Vector to copy the row into.</param>
        /// <exceptioncreArgumentNullException">If the result vector is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="index is negative,
        /// or greater than or equal to the number of rows.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <b>this.Columns != result.Count</b>
        /// .
        /// </exception>
        public void Row(int index, Vector<T> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            Storage.CopyRowTo(result.Storage, index);
        }

        /// <summary>
        ///     Copies the requested row elements into a new Vector.
        /// </summary>
        /// <param name="rowIndex">The row to copy elements from.</param>
        /// <param name="columnIndex">The column to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <returns>A Vector containing the requested elements.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If:
        /// 
        /// 
        /// <list>
        ///     <item>
        ///         <paramref name="rowIndex is negative,
        /// or greater than or equal to the number of rows.
        ///     
        ///     </item>
        ///     <item>
        ///         <paramref name="columnIndex is negative,
        /// or greater than or equal to the number of columns.
        ///     
        ///     </item>
        ///     <item>
        ///         <c>(columnIndex + length) &gt;= Columns.</c>
        ///     </item>
        /// </list>
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="length is not positive.
        /// 
        /// </exception>
        public Vector<T> Row(int rowIndex, int columnIndex, int length)
        {
            var ret = Vector<T>.Build.SameAs(this, length);
            Storage.CopySubRowTo(ret.Storage, rowIndex, columnIndex, 0, length);
            return ret;
        }

        /// <summary>
        ///     Copies the requested row elements into a new Vector.
        /// </summary>
        /// <param name="rowIndex">The row to copy elements from.</param>
        /// <param name="columnIndex">The column to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <param name="result">The Vector to copy the column into.</param>
        /// <exceptioncreArgumentNullException">If the result Vector is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="rowIndex is negative,
        /// or greater than or equal to the number of columns.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="columnIndex is negative,
        /// or greater than or equal to the number of rows.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="columnIndex + 
        /// 
        /// <paramref name="length
        /// is greater than or equal to the number of rows.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="length is not positive.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <strong>result.Count &lt; length</strong>
        /// .
        /// </exception>
        public void Row(int rowIndex, int columnIndex, int length, Vector<T> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            Storage.CopySubRowTo(result.Storage, rowIndex, columnIndex, 0, length);
        }

        /// <summary>
        ///     Copies a column into a new Vector>.
        /// </summary>
        /// <param name="index">The column to copy.</param>
        /// <returns>A Vector containing the copied elements.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="index is negative,
        /// or greater than or equal to the number of columns.
        /// 
        /// </exception>
        public Vector<T> Column(int index)
        {
            if ((uint) index >= (uint) ColumnCount) throw new ArgumentOutOfRangeException(nameof(index));

            var ret = Vector<T>.Build.SameAs(this, RowCount);
            Storage.CopySubColumnToUnchecked(ret.Storage, index, 0, 0, RowCount, ExistingData.AssumeZeros);
            return ret;
        }

        /// <summary>
        ///     Copies a column into to the given Vector.
        /// </summary>
        /// <param name="index">The column to copy.</param>
        /// <param name="result">The Vector to copy the column into.</param>
        /// <exceptioncreArgumentNullException">If the result Vector is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="index is negative,
        /// or greater than or equal to the number of columns.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <b>this.Rows != result.Count</b>
        /// .
        /// </exception>
        public void Column(int index, Vector<T> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            Storage.CopyColumnTo(result.Storage, index);
        }
        

        /// <summary>
        ///     Copies the requested column elements into the given vector.
        /// </summary>
        /// <param name="columnIndex">The column to copy elements from.</param>
        /// <param name="rowIndex">The row to start copying from.</param>
        /// <param name="length">The number of elements to copy.</param>
        /// <param name="result">The Vector to copy the column into.</param>
        /// <exceptioncreArgumentNullException">If the result Vector is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="columnIndex is negative,
        /// or greater than or equal to the number of columns.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="rowIndex is negative,
        /// or greater than or equal to the number of rows.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="rowIndex + 
        /// 
        /// <paramref name="length
        /// is greater than or equal to the number of rows.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="length is not positive.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <strong>result.Count &lt; length</strong>
        /// .
        /// </exception>
        public void Column(int columnIndex, int rowIndex, int length, Vector<T> result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            Storage.CopySubColumnTo(result.Storage, columnIndex, rowIndex, 0, length);
        }

        /// <summary>
        ///     Returns a new matrix containing the upper triangle of this matrix.
        /// </summary>
        /// <returns>The upper triangle of this matrix.</returns>
        public virtual Matrix<T> UpperTriangle()
        {
            var result = Build.SameAs(this);
            for (var row = 0; row < RowCount; row++)
            for (var column = row; column < ColumnCount; column++)
                result.At(row, column, At(row, column));
            return result;
        }

        /// <summary>
        ///     Returns a new matrix containing the lower triangle of this matrix.
        /// </summary>
        /// <returns>The lower triangle of this matrix.</returns>
        public virtual Matrix<T> LowerTriangle()
        {
            var result = Build.SameAs(this);
            for (var row = 0; row < RowCount; row++)
            for (var column = 0; column <= row && column < ColumnCount; column++)
                result.At(row, column, At(row, column));
            return result;
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
        public virtual Matrix<T> SubMatrix(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            var result = Build.SameAs(this, rowCount, columnCount);
            Storage.CopySubMatrixTo(result.Storage, rowIndex, 0, rowCount, columnIndex, 0, columnCount,
                ExistingData.AssumeZeros);
            return result;
        }


        /// <summary>
        ///     Copies the values of the given Vector to the diagonal.
        /// </summary>
        /// <param name="source">
        ///     The vector to copy the values from. The length of the vector should be
        ///     Min(Rows, Columns).
        /// </param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="source is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentException">If the length of 
        /// 
        /// <paramref name="source does not
        /// equal Min(Rows, Columns).
        /// 
        /// </exception>
        /// <remarks>For non-square matrices, the elements of <paramref name="source are copied to
        /// this[i,i].</remarks>
        public virtual void SetDiagonal(Vector<T> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var min = Math.Min(RowCount, ColumnCount);

            if (source.Count != min)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(source));
            }

            for (var i = 0; i < min; i++) At(i, i, source.At(i));
        }

        /// <summary>
        ///     Returns the transpose of this matrix.
        /// </summary>
        /// <returns>The transpose of this matrix.</returns>
        public Matrix<T> Transpose()
        {
            var result = Build.SameAs(this, ColumnCount, RowCount);
            Storage.TransposeToUnchecked(result.Storage, ExistingData.AssumeZeros);
            return result;
        }


        /// <summary>
        ///     Applies a function to each value of this matrix and replaces the value in the result matrix.
        ///     If forceMapZero is not set to true, zero values may or may not be skipped depending
        ///     on the actual data storage implementation (relevant mostly for sparse matrices).
        /// </summary>
        public void Map(Func<T, T> f, Matrix<T> result, Zeros zeros = Zeros.AllowSkip)
        {
            if (ReferenceEquals(this, result))
                Storage.MapInplace(f, zeros);
            else
                Storage.MapTo(result.Storage, f, zeros,
                    zeros == Zeros.Include ? ExistingData.AssumeZeros : ExistingData.Clear);
        }


        /// <summary>
        ///     Applies a function to each value pair of two matrices and replaces the value in the result vector.
        /// </summary>
        public void Map2(Func<T, T, T> f, Matrix<T> other, Matrix<T> result, Zeros zeros = Zeros.AllowSkip)
        {
            Storage.Map2To(result.Storage, other.Storage, f, zeros, ExistingData.Clear);
        }


        /// <summary>
        ///     Returns a tuple with the index and value of the first element satisfying a predicate, or null if none is found.
        ///     Zero elements may be skipped on sparse data structures if allowed (default).
        /// </summary>
        public Tuple<int, int, T> Find(Func<T, bool> predicate, Zeros zeros = Zeros.AllowSkip)
        {
            return Storage.Find(predicate, zeros);
        }


        /// <summary>
        ///     Returns true if all elements satisfy a predicate.
        ///     Zero elements may be skipped on sparse data structures if allowed (default).
        /// </summary>
        public bool ForAll(Func<T, bool> predicate, Zeros zeros = Zeros.AllowSkip)
        {
            return Storage.Find(x => !predicate(x), zeros) == null;
        }
    }
}