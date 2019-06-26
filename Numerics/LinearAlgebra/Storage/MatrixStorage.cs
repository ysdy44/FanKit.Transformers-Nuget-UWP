// <copyright file="MatrixStorage.cs" company="Math.NET">
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
using System.Runtime.Serialization;

namespace MathNet.Numerics.LinearAlgebra.Storage
{
    [DataContract(Namespace = "urn:MathNet/Numerics/LinearAlgebra")]
    public abstract partial class MatrixStorage<T> : IEquatable<MatrixStorage<T>>
        where T : struct, IEquatable<T>, IFormattable
    {
        // [ruegg] public fields are OK here

        protected static readonly T Zero = BuilderInstance<T>.Matrix.Zero;

        [DataMember(Order = 2)] public readonly int ColumnCount;

        [DataMember(Order = 1)] public readonly int RowCount;

        protected MatrixStorage(int rowCount, int columnCount)
        {
            if (rowCount <= 0)
            {
                //   throw new ArgumentOutOfRangeException(nameof(rowCount), Resources.MatrixRowsMustBePositive);
            }

            if (columnCount <= 0)
            {
                //  throw new ArgumentOutOfRangeException(nameof(columnCount), Resources.MatrixColumnsMustBePositive);
            }

            RowCount = rowCount;
            ColumnCount = columnCount;
        }

        /// <summary>
        ///     True if the matrix storage format is dense.
        /// </summary>
        public abstract bool IsDense { get; }


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
            get
            {
                ValidateRange(row, column);
                return At(row, column);
            }

            set
            {
                ValidateRange(row, column);
                At(row, column, value);
            }
        }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        ///     An object to compare with this object.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the current object is equal to the <paramref name="other parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(MatrixStorage<T> other)
        {
            // Reject equality when the argument is null or has a different shape.
            if (other == null) return false;
            if (ColumnCount != other.ColumnCount || RowCount != other.RowCount) return false;

            // Accept if the argument is the same object as this.
            if (ReferenceEquals(this, other)) return true;

            // Perform element wise comparison.
            return Find2Unchecked(other, (a, b) => !a.Equals(b), Zeros.AllowSkip) == null;
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
        /// <remarks>Not range-checked.</remarks>
        public abstract T At(int row, int column);

        /// <summary>
        ///     Sets the element without range checking.
        /// </summary>
        /// <param name="row"> The row of the element. </param>
        /// <param name="column"> The column of the element. </param>
        /// <param name="value"> The value to set the element to. </param>
        /// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
        public abstract void At(int row, int column, T value);

        /// <summary>
        ///     Determines whether the specified <seecreT:System.Object is equal to the current <seecreT:System.Object.
        /// </summary>
        /// <returns>
        ///     true if the specified <seecreT:System.Object is equal to the current <seecreT:System.Object; otherwise, false.
        /// </returns>
        /// <param name="obj">The <seecreT:System.Object to compare with the current <seecreT:System.Object. </param>
        public sealed override bool Equals(object obj)
        {
            return Equals(obj as MatrixStorage<T>);
        }
        
        // CLEARING

        public virtual void Clear()
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
                At(i, j, Zero);
        }

        public void Clear(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            if (rowCount < 1 || columnCount < 1) return;

            if (rowIndex + rowCount > RowCount || rowIndex < 0) throw new ArgumentOutOfRangeException(nameof(rowIndex));

            if (columnIndex + columnCount > ColumnCount || columnIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(columnIndex));

            ClearUnchecked(rowIndex, rowCount, columnIndex, columnCount);
        }

        internal virtual void ClearUnchecked(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            for (var i = rowIndex; i < rowIndex + rowCount; i++)
            for (var j = columnIndex; j < columnIndex + columnCount; j++)
                At(i, j, Zero);
        }

        // MATRIX COPY

        public void CopyTo(MatrixStorage<T> target, ExistingData existingData = ExistingData.Clear)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (ReferenceEquals(this, target)) return;

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                //   var message = string.Format(Resources.ArgumentMatrixDimensions2, RowCount + "x" + ColumnCount, target.RowCount + "x" + target.ColumnCount);
                //   throw new ArgumentException(message, nameof(target));
            }

            CopyToUnchecked(target, existingData);
        }

        internal virtual void CopyToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            for (var j = 0; j < ColumnCount; j++)
            for (var i = 0; i < RowCount; i++)
                target.At(i, j, At(i, j));
        }

        public void CopySubMatrixTo(MatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData = ExistingData.Clear)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (rowCount == 0 || columnCount == 0) return;

            if (ReferenceEquals(this, target)) throw new NotSupportedException();

            ValidateSubMatrixRange(target,
                sourceRowIndex, targetRowIndex, rowCount,
                sourceColumnIndex, targetColumnIndex, columnCount);

            CopySubMatrixToUnchecked(target, sourceRowIndex, targetRowIndex, rowCount,
                sourceColumnIndex, targetColumnIndex, columnCount, existingData);
        }

        internal virtual void CopySubMatrixToUnchecked(MatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
                target.At(ii, jj, At(i, j));
        }

        // ROW COPY

        public void CopyRowTo(VectorStorage<T> target, int rowIndex, ExistingData existingData = ExistingData.Clear)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            ValidateRowRange(target, rowIndex);
            CopySubRowToUnchecked(target, rowIndex, 0, 0, ColumnCount, existingData);
        }

        public void CopySubRowTo(VectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData = ExistingData.Clear)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (columnCount == 0) return;

            ValidateSubRowRange(target, rowIndex, sourceColumnIndex, targetColumnIndex, columnCount);
            CopySubRowToUnchecked(target, rowIndex, sourceColumnIndex, targetColumnIndex, columnCount, existingData);
        }

        internal virtual void CopySubRowToUnchecked(VectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount, ExistingData existingData)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
                target.At(jj, At(rowIndex, j));
        }

        // COLUMN COPY

        public void CopyColumnTo(VectorStorage<T> target, int columnIndex,
            ExistingData existingData = ExistingData.Clear)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            ValidateColumnRange(target, columnIndex);
            CopySubColumnToUnchecked(target, columnIndex, 0, 0, RowCount, existingData);
        }

        public void CopySubColumnTo(VectorStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            ExistingData existingData = ExistingData.Clear)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (rowCount == 0) return;

            ValidateSubColumnRange(target, columnIndex, sourceRowIndex, targetRowIndex, rowCount);
            CopySubColumnToUnchecked(target, columnIndex, sourceRowIndex, targetRowIndex, rowCount, existingData);
        }

        internal virtual void CopySubColumnToUnchecked(VectorStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount, ExistingData existingData)
        {
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
                target.At(ii, At(i, columnIndex));
        }

        // TRANSPOSE

        internal virtual void TransposeToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            for (var j = 0; j < ColumnCount; j++)
            for (var i = 0; i < RowCount; i++)
                target.At(j, i, At(i, j));
        }

        // EXTRACT


        // FIND

        public virtual Tuple<int, int, T> Find(Func<T, bool> predicate, Zeros zeros)
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
            {
                var item = At(i, j);
                if (predicate(item)) return new Tuple<int, int, T>(i, j, item);
            }

            return null;
        }


        internal virtual Tuple<int, int, T, TOther> Find2Unchecked<TOther>(MatrixStorage<TOther> other,
            Func<T, TOther, bool> predicate, Zeros zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
            {
                var item = At(i, j);
                var otherItem = other.At(i, j);
                if (predicate(item, otherItem)) return new Tuple<int, int, T, TOther>(i, j, item, otherItem);
            }

            return null;
        }

        // FUNCTIONAL COMBINATORS: MAP

        public virtual void MapInplace(Func<T, T> f, Zeros zeros)
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
                At(i, j, f(At(i, j)));
        }


        public void MapTo<TU>(MatrixStorage<TU> target, Func<T, TU> f, Zeros zeros, ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                // var message = string.Format(Resources.ArgumentMatrixDimensions2, RowCount + "x" + ColumnCount, target.RowCount + "x" + target.ColumnCount);
                // throw new ArgumentException(message, nameof(target));
            }

            MapToUnchecked(target, f, zeros, existingData);
        }

        internal virtual void MapToUnchecked<TU>(MatrixStorage<TU> target, Func<T, TU> f, Zeros zeros,
            ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
                target.At(i, j, f(At(i, j)));
        }

        public void MapIndexedTo<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f, Zeros zeros,
            ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                //     var message = string.Format(Resources.ArgumentMatrixDimensions2, RowCount + "x" + ColumnCount, target.RowCount + "x" + target.ColumnCount);
                //    throw new ArgumentException(message, nameof(target));
            }

            MapIndexedToUnchecked(target, f, zeros, existingData);
        }

        internal virtual void MapIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f, Zeros zeros,
            ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (var j = 0; j < ColumnCount; j++)
            for (var i = 0; i < RowCount; i++)
                target.At(i, j, f(i, j, At(i, j)));
        }

        public void MapSubMatrixIndexedTo<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (rowCount == 0 || columnCount == 0) return;

            if (ReferenceEquals(this, target)) throw new NotSupportedException();

            ValidateSubMatrixRange(target,
                sourceRowIndex, targetRowIndex, rowCount,
                sourceColumnIndex, targetColumnIndex, columnCount);

            MapSubMatrixIndexedToUnchecked(target, f, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex,
                targetColumnIndex, columnCount, zeros, existingData);
        }

        internal virtual void MapSubMatrixIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
                target.At(ii, jj, f(ii, jj, At(i, j)));
        }

        public void Map2To(MatrixStorage<T> target, MatrixStorage<T> other, Func<T, T, T> f, Zeros zeros,
            ExistingData existingData)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            if (other == null) throw new ArgumentNullException(nameof(other));

            if (RowCount != target.RowCount || ColumnCount != target.ColumnCount)
            {
                //     var message = string.Format(Resources.ArgumentMatrixDimensions2, RowCount + "x" + ColumnCount, target.RowCount + "x" + target.ColumnCount);
                //   throw new ArgumentException(message, nameof(target));
            }

            if (RowCount != other.RowCount || ColumnCount != other.ColumnCount)
            {
                //  var message = string.Format(Resources.ArgumentMatrixDimensions2, RowCount + "x" + ColumnCount, other.RowCount + "x" + other.ColumnCount);
                //   throw new ArgumentException(message, nameof(other));
            }

            Map2ToUnchecked(target, other, f, zeros, existingData);
        }

        internal virtual void Map2ToUnchecked(MatrixStorage<T> target, MatrixStorage<T> other, Func<T, T, T> f,
            Zeros zeros, ExistingData existingData)
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
                target.At(i, j, f(At(i, j), other.At(i, j)));
        }

        // FUNCTIONAL COMBINATORS: FOLD


        internal virtual TState Fold2Unchecked<TOther, TState>(MatrixStorage<TOther> other,
            Func<TState, T, TOther, TState> f, TState state, Zeros zeros)
            where TOther : struct, IEquatable<TOther>, IFormattable
        {
            for (var i = 0; i < RowCount; i++)
            for (var j = 0; j < ColumnCount; j++)
                state = f(state, At(i, j), other.At(i, j));

            return state;
        }
    }
}