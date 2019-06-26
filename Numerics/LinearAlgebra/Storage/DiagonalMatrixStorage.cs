// <copyright file="DiagonalMatrixStorage.cs" company="Math.NET">
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
using System.Linq;
using System.Runtime.Serialization;
using MathNet.Numerics.Threading;

namespace MathNet.Numerics.LinearAlgebra.Storage
{
    [DataContract(Namespace = "urn:MathNet/Numerics/LinearAlgebra")]
    public class DiagonalMatrixStorage<T> : MatrixStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        // [ruegg] public fields are OK here

        [DataMember(Order = 1)] public readonly T[] Data;

        internal DiagonalMatrixStorage(int rows, int columns)
            : base(rows, columns)
        {
            Data = new T[Math.Min(rows, columns)];
        }

        internal DiagonalMatrixStorage(int rows, int columns, T[] data)
            : base(rows, columns)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (data.Length != Math.Min(rows, columns))
            {
                // throw new ArgumentOutOfRangeException(nameof(data), string.Format(Resources.ArgumentArrayWrongLength, Math.Min(rows, columns)));
            }

            Data = data;
        }

        /// <summary>
        ///     True if the matrix storage format is dense.
        /// </summary>
        public override bool IsDense => false;

        /// <summary>
        ///     Retrieves the requested element without range checking.
        /// </summary>
        public override T At(int row, int column)
        {
            return row == column ? Data[row] : Zero;
        }

        /// <summary>
        ///     Sets the element without range checking.
        /// </summary>
        public override void At(int row, int column, T value)
        {
            if (row == column)
                Data[row] = value;
            else if (!Zero.Equals(value))
                throw new IndexOutOfRangeException("Cannot set an off-diagonal element in a diagonal matrix.");
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var hashNum = Math.Min(Data.Length, 25);
            var hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++) hash = hash * 31 + Data[i].GetHashCode();
            }

            return hash;
        }

        // CLEARING

        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        internal override void ClearUnchecked(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            var beginInclusive = Math.Max(rowIndex, columnIndex);
            var endExclusive = Math.Min(rowIndex + rowCount, columnIndex + columnCount);
            if (endExclusive > beginInclusive) Array.Clear(Data, beginInclusive, endExclusive - beginInclusive);
        }


        // INITIALIZATION

        public static DiagonalMatrixStorage<T> OfInit(int rows, int columns, Func<int, T> init)
        {
            var storage = new DiagonalMatrixStorage<T>(rows, columns);
            for (var i = 0; i < storage.Data.Length; i++) storage.Data[i] = init(i);
            return storage;
        }

        // MATRIX COPY

        internal override void CopyToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            var diagonalTarget = target as DiagonalMatrixStorage<T>;
            if (diagonalTarget != null)
            {
                CopyToUnchecked(diagonalTarget);
                return;
            }

            var denseTarget = target as DenseColumnMajorMatrixStorage<T>;
            if (denseTarget != null)
            {
                CopyToUnchecked(denseTarget, existingData);
                return;
            }

            var sparseTarget = target as SparseCompressedRowMatrixStorage<T>;
            if (sparseTarget != null)
            {
                CopyToUnchecked(sparseTarget, existingData);
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear) target.Clear();

            for (var i = 0; i < Data.Length; i++) target.At(i, i, Data[i]);
        }

        private void CopyToUnchecked(DiagonalMatrixStorage<T> target)
        {
            //Buffer.BlockCopy(Data, 0, target.Data, 0, Data.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
            Array.Copy(Data, 0, target.Data, 0, Data.Length);
        }

        private void CopyToUnchecked(SparseCompressedRowMatrixStorage<T> target, ExistingData existingData)
        {
            if (existingData == ExistingData.Clear) target.Clear();

            for (var i = 0; i < Data.Length; i++) target.At(i, i, Data[i]);
        }

        private void CopyToUnchecked(DenseColumnMajorMatrixStorage<T> target, ExistingData existingData)
        {
            if (existingData == ExistingData.Clear) target.Clear();

            for (var i = 0; i < Data.Length; i++) target.Data[i * (target.RowCount + 1)] = Data[i];
        }

        internal override void CopySubMatrixToUnchecked(MatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData)
        {
            var denseTarget = target as DenseColumnMajorMatrixStorage<T>;
            if (denseTarget != null)
            {
                CopySubMatrixToUnchecked(denseTarget, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex,
                    targetColumnIndex, columnCount, existingData);
                return;
            }

            var diagonalTarget = target as DiagonalMatrixStorage<T>;
            if (diagonalTarget != null)
            {
                CopySubMatrixToUnchecked(diagonalTarget, sourceRowIndex, targetRowIndex, rowCount, sourceColumnIndex,
                    targetColumnIndex, columnCount);
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            if (existingData == ExistingData.Clear)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            if (sourceRowIndex == sourceColumnIndex)
            {
                for (var i = 0; i < Math.Min(columnCount, rowCount); i++)
                    target.At(targetRowIndex + i, targetColumnIndex + i, Data[sourceRowIndex + i]);
            }
            else if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
            {
                // column by column, but skip resulting zero columns at the beginning
                var columnInit = sourceRowIndex - sourceColumnIndex;
                for (var i = 0; i < Math.Min(columnCount - columnInit, rowCount); i++)
                    target.At(targetRowIndex + i, columnInit + targetColumnIndex + i, Data[sourceRowIndex + i]);
            }
            else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
            {
                // row by row, but skip resulting zero rows at the beginning
                var rowInit = sourceColumnIndex - sourceRowIndex;
                for (var i = 0; i < Math.Min(columnCount, rowCount - rowInit); i++)
                    target.At(rowInit + targetRowIndex + i, targetColumnIndex + i, Data[sourceColumnIndex + i]);
            }
        }

        private void CopySubMatrixToUnchecked(DiagonalMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount)
        {
            if (sourceRowIndex - sourceColumnIndex != targetRowIndex - targetColumnIndex)
            {
                if (Data.Any(x => !Zero.Equals(x)))
                {
                    //throw new NotSupportedException();
                }

                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);
                return;
            }

            var beginInclusive = Math.Max(sourceRowIndex, sourceColumnIndex);
            var endExclusive = Math.Min(sourceRowIndex + rowCount, sourceColumnIndex + columnCount);
            if (endExclusive > beginInclusive)
            {
                var beginTarget = Math.Max(targetRowIndex, targetColumnIndex);
                Array.Copy(Data, beginInclusive, target.Data, beginTarget, endExclusive - beginInclusive);
            }
        }

        private void CopySubMatrixToUnchecked(DenseColumnMajorMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData)
        {
            if (existingData == ExistingData.Clear)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
            {
                // column by column, but skip resulting zero columns at the beginning

                var columnInit = sourceRowIndex - sourceColumnIndex;
                var offset = (columnInit + targetColumnIndex) * target.RowCount + targetRowIndex;
                var step = target.RowCount + 1;
                var end = Math.Min(columnCount - columnInit, rowCount) + sourceRowIndex;

                for (int i = sourceRowIndex, j = offset; i < end; i++, j += step) target.Data[j] = Data[i];
            }
            else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
            {
                // row by row, but skip resulting zero rows at the beginning

                var rowInit = sourceColumnIndex - sourceRowIndex;
                var offset = targetColumnIndex * target.RowCount + rowInit + targetRowIndex;
                var step = target.RowCount + 1;
                var end = Math.Min(columnCount, rowCount - rowInit) + sourceColumnIndex;

                for (int i = sourceColumnIndex, j = offset; i < end; i++, j += step) target.Data[j] = Data[i];
            }
            else
            {
                var offset = targetColumnIndex * target.RowCount + targetRowIndex;
                var step = target.RowCount + 1;
                var end = Math.Min(columnCount, rowCount) + sourceRowIndex;

                for (int i = sourceRowIndex, j = offset; i < end; i++, j += step) target.Data[j] = Data[i];
            }
        }

        // ROW COPY

        internal override void CopySubRowToUnchecked(VectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount, ExistingData existingData)
        {
            if (existingData == ExistingData.Clear) target.Clear(targetColumnIndex, columnCount);

            if (rowIndex >= sourceColumnIndex && rowIndex < sourceColumnIndex + columnCount && rowIndex < Data.Length)
                target.At(rowIndex - sourceColumnIndex + targetColumnIndex, Data[rowIndex]);
        }

        // COLUMN COPY

        internal override void CopySubColumnToUnchecked(VectorStorage<T> target, int columnIndex,
            int sourceRowIndex, int targetRowIndex, int rowCount, ExistingData existingData)
        {
            if (existingData == ExistingData.Clear) target.Clear(targetRowIndex, rowCount);

            if (columnIndex >= sourceRowIndex && columnIndex < sourceRowIndex + rowCount && columnIndex < Data.Length)
                target.At(columnIndex - sourceRowIndex + targetRowIndex, Data[columnIndex]);
        }

        // TRANSPOSE

        internal override void TransposeToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            CopyToUnchecked(target, existingData);
        }

        // EXTRACT

        // ENUMERATION

        // FIND

        public override Tuple<int, int, T> Find(Func<T, bool> predicate, Zeros zeros)
        {
            for (var i = 0; i < Data.Length; i++)
                if (predicate(Data[i]))
                    return new Tuple<int, int, T>(i, i, Data[i]);
            if (zeros == Zeros.Include && (RowCount > 1 || ColumnCount > 1))
                if (predicate(Zero))
                    return new Tuple<int, int, T>(RowCount > 1 ? 1 : 0, RowCount > 1 ? 0 : 1, Zero);
            return null;
        }

        internal override Tuple<int, int, T, TOther> Find2Unchecked<TOther>(MatrixStorage<TOther> other,
            Func<T, TOther, bool> predicate, Zeros zeros)
        {
            var denseOther = other as DenseColumnMajorMatrixStorage<TOther>;
            if (denseOther != null)
            {
                var otherData = denseOther.Data;
                var k = 0;
                for (var j = 0; j < ColumnCount; j++)
                for (var i = 0; i < RowCount; i++)
                {
                    if (predicate(i == j ? Data[i] : Zero, otherData[k]))
                        return new Tuple<int, int, T, TOther>(i, j, i == j ? Data[i] : Zero, otherData[k]);
                    k++;
                }

                return null;
            }

            var diagonalOther = other as DiagonalMatrixStorage<TOther>;
            if (diagonalOther != null)
            {
                var otherData = diagonalOther.Data;
                for (var i = 0; i < Data.Length; i++)
                    if (predicate(Data[i], otherData[i]))
                        return new Tuple<int, int, T, TOther>(i, i, Data[i], otherData[i]);
                if (zeros == Zeros.Include && (RowCount > 1 || ColumnCount > 1))
                {
                    var otherZero = BuilderInstance<TOther>.Matrix.Zero;
                    if (predicate(Zero, otherZero))
                        return new Tuple<int, int, T, TOther>(RowCount > 1 ? 1 : 0, RowCount > 1 ? 0 : 1, Zero,
                            otherZero);
                }

                return null;
            }

            var sparseOther = other as SparseCompressedRowMatrixStorage<TOther>;
            if (sparseOther != null)
            {
                var otherRowPointers = sparseOther.RowPointers;
                var otherColumnIndices = sparseOther.ColumnIndices;
                var otherValues = sparseOther.Values;
                var otherZero = BuilderInstance<TOther>.Matrix.Zero;
                for (var row = 0; row < RowCount; row++)
                {
                    var diagonal = false;
                    var startIndex = otherRowPointers[row];
                    var endIndex = otherRowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                        if (otherColumnIndices[j] == row)
                        {
                            diagonal = true;
                            if (predicate(Data[row], otherValues[j]))
                                return new Tuple<int, int, T, TOther>(row, row, Data[row], otherValues[j]);
                        }
                        else
                        {
                            if (predicate(Zero, otherValues[j]))
                                return new Tuple<int, int, T, TOther>(row, otherColumnIndices[j], Zero, otherValues[j]);
                        }

                    if (!diagonal && row < ColumnCount)
                        if (predicate(Data[row], otherZero))
                            return new Tuple<int, int, T, TOther>(row, row, Data[row], otherZero);
                }

                if (zeros == Zeros.Include && sparseOther.ValueCount < RowCount * ColumnCount)
                    if (predicate(Zero, otherZero))
                    {
                        var k = 0;
                        for (var row = 0; row < RowCount; row++)
                        for (var col = 0; col < ColumnCount; col++)
                            if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                                k++;
                            else if (row != col) return new Tuple<int, int, T, TOther>(row, col, Zero, otherZero);
                    }

                return null;
            }

            // FALL BACK

            return base.Find2Unchecked(other, predicate, zeros);
        }

        // FUNCTIONAL COMBINATORS: MAP

        public override void MapInplace(Func<T, T> f, Zeros zeros)
        {
            if (zeros == Zeros.Include)
                throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");

            CommonParallel.For(0, Data.Length, 4096, (a, b) =>
            {
                for (var i = a; i < b; i++) Data[i] = f(Data[i]);
            });
        }


        internal override void MapToUnchecked<TU>(MatrixStorage<TU> target, Func<T, TU> f,
            Zeros zeros, ExistingData existingData)
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(Zero));

            var diagonalTarget = target as DiagonalMatrixStorage<TU>;
            if (diagonalTarget != null)
            {
                if (processZeros)
                {
                    //throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
                }

                CommonParallel.For(0, Data.Length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) diagonalTarget.Data[i] = f(Data[i]);
                });
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear && !processZeros) target.Clear();

            if (processZeros)
                for (var j = 0; j < ColumnCount; j++)
                for (var i = 0; i < RowCount; i++)
                    target.At(i, j, f(i == j ? Data[i] : Zero));
            else
                for (var i = 0; i < Data.Length; i++)
                    target.At(i, i, f(Data[i]));
        }

        internal override void MapIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            Zeros zeros, ExistingData existingData)
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(0, 1, Zero));

            var diagonalTarget = target as DiagonalMatrixStorage<TU>;
            if (diagonalTarget != null)
            {
                if (processZeros)
                {
                    //throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");
                }

                CommonParallel.For(0, Data.Length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) diagonalTarget.Data[i] = f(i, i, Data[i]);
                });
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear && !processZeros) target.Clear();

            if (processZeros)
                for (var j = 0; j < ColumnCount; j++)
                for (var i = 0; i < RowCount; i++)
                    target.At(i, j, f(i, j, i == j ? Data[i] : Zero));
            else
                for (var i = 0; i < Data.Length; i++)
                    target.At(i, i, f(i, i, Data[i]));
        }

        internal override void MapSubMatrixIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
        {
            var diagonalTarget = target as DiagonalMatrixStorage<TU>;
            if (diagonalTarget != null)
            {
                MapSubMatrixIndexedToUnchecked(diagonalTarget, f, sourceRowIndex, targetRowIndex, rowCount,
                    sourceColumnIndex, targetColumnIndex, columnCount, zeros);
                return;
            }

            var denseTarget = target as DenseColumnMajorMatrixStorage<TU>;
            if (denseTarget != null)
            {
                MapSubMatrixIndexedToUnchecked(denseTarget, f, sourceRowIndex, targetRowIndex, rowCount,
                    sourceColumnIndex, targetColumnIndex, columnCount, zeros, existingData);
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            if (existingData == ExistingData.Clear)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            if (sourceRowIndex == sourceColumnIndex)
            {
                var targetRow = targetRowIndex;
                var targetColumn = targetColumnIndex;
                for (var i = 0; i < Math.Min(columnCount, rowCount); i++)
                {
                    target.At(targetRow, targetColumn, f(targetRow, targetColumn, Data[sourceRowIndex + i]));
                    targetRow++;
                    targetColumn++;
                }
            }
            else if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
            {
                // column by column, but skip resulting zero columns at the beginning
                var columnInit = sourceRowIndex - sourceColumnIndex;
                var targetRow = targetRowIndex;
                var targetColumn = targetColumnIndex + columnInit;
                for (var i = 0; i < Math.Min(columnCount - columnInit, rowCount); i++)
                {
                    target.At(targetRow, targetColumn, f(targetRow, targetColumn, Data[sourceRowIndex + i]));
                    targetRow++;
                    targetColumn++;
                }
            }
            else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
            {
                // row by row, but skip resulting zero rows at the beginning
                var rowInit = sourceColumnIndex - sourceRowIndex;
                var targetRow = targetRowIndex + rowInit;
                var targetColumn = targetColumnIndex;
                for (var i = 0; i < Math.Min(columnCount, rowCount - rowInit); i++)
                {
                    target.At(targetRow, targetColumn, f(targetRow, targetColumn, Data[sourceColumnIndex + i]));
                    targetRow++;
                    targetColumn++;
                }
            }
        }

        private void MapSubMatrixIndexedToUnchecked<TU>(DiagonalMatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(0, 1, Zero));
            if (processZeros || sourceRowIndex - sourceColumnIndex != targetRowIndex - targetColumnIndex)
                throw new NotSupportedException("Cannot map non-zero off-diagonal values into a diagonal matrix");

            var beginInclusive = Math.Max(sourceRowIndex, sourceColumnIndex);
            var count = Math.Min(sourceRowIndex + rowCount, sourceColumnIndex + columnCount) - beginInclusive;
            if (count > 0)
            {
                var beginTarget = Math.Max(targetRowIndex, targetColumnIndex);
                CommonParallel.For(0, count, 4096, (a, b) =>
                {
                    var targetIndex = beginTarget + a;
                    for (var i = a; i < b; i++)
                    {
                        target.Data[targetIndex] = f(targetIndex, targetIndex, Data[beginInclusive + i]);
                        targetIndex++;
                    }
                });
            }
        }

        private void MapSubMatrixIndexedToUnchecked<TU>(DenseColumnMajorMatrixStorage<TU> target,
            Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(0, 1, Zero));
            if (existingData == ExistingData.Clear && !processZeros)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            if (processZeros)
            {
                CommonParallel.For(0, columnCount, Math.Max(4096 / rowCount, 32), (a, b) =>
                {
                    var sourceColumn = sourceColumnIndex + a;
                    var targetColumn = targetColumnIndex + a;
                    for (var j = a; j < b; j++)
                    {
                        var targetIndex = targetRowIndex + (j + targetColumnIndex) * target.RowCount;
                        var sourceRow = sourceRowIndex;
                        var targetRow = targetRowIndex;
                        for (var i = 0; i < rowCount; i++)
                            target.Data[targetIndex++] = f(targetRow++, targetColumn,
                                sourceRow++ == sourceColumn ? Data[sourceColumn] : Zero);
                        sourceColumn++;
                        targetColumn++;
                    }
                });
            }
            else
            {
                if (sourceRowIndex > sourceColumnIndex && sourceColumnIndex + columnCount > sourceRowIndex)
                {
                    // column by column, but skip resulting zero columns at the beginning

                    var columnInit = sourceRowIndex - sourceColumnIndex;
                    var offset = (columnInit + targetColumnIndex) * target.RowCount + targetRowIndex;
                    var step = target.RowCount + 1;
                    var count = Math.Min(columnCount - columnInit, rowCount);

                    for (int k = 0, j = offset; k < count; j += step, k++)
                        target.Data[j] = f(targetRowIndex + k, targetColumnIndex + columnInit + k,
                            Data[sourceRowIndex + k]);
                }
                else if (sourceRowIndex < sourceColumnIndex && sourceRowIndex + rowCount > sourceColumnIndex)
                {
                    // row by row, but skip resulting zero rows at the beginning

                    var rowInit = sourceColumnIndex - sourceRowIndex;
                    var offset = targetColumnIndex * target.RowCount + rowInit + targetRowIndex;
                    var step = target.RowCount + 1;
                    var count = Math.Min(columnCount, rowCount - rowInit);

                    for (int k = 0, j = offset; k < count; j += step, k++)
                        target.Data[j] = f(targetRowIndex + rowInit + k, targetColumnIndex + k,
                            Data[sourceColumnIndex + k]);
                }
                else
                {
                    var offset = targetColumnIndex * target.RowCount + targetRowIndex;
                    var step = target.RowCount + 1;
                    var count = Math.Min(columnCount, rowCount);

                    for (int k = 0, j = offset; k < count; j += step, k++)
                        target.Data[j] = f(targetRowIndex + k, targetColumnIndex + k, Data[sourceRowIndex + k]);
                }
            }
        }

        // FUNCTIONAL COMBINATORS: FOLD

        internal override TState Fold2Unchecked<TOther, TState>(MatrixStorage<TOther> other,
            Func<TState, T, TOther, TState> f, TState state, Zeros zeros)
        {
            var denseOther = other as DenseColumnMajorMatrixStorage<TOther>;
            if (denseOther != null)
            {
                var otherData = denseOther.Data;
                var k = 0;
                for (var j = 0; j < ColumnCount; j++)
                for (var i = 0; i < RowCount; i++)
                {
                    state = f(state, i == j ? Data[i] : Zero, otherData[k]);
                    k++;
                }

                return state;
            }

            var diagonalOther = other as DiagonalMatrixStorage<TOther>;
            if (diagonalOther != null)
            {
                var otherData = diagonalOther.Data;
                for (var i = 0; i < Data.Length; i++) state = f(state, Data[i], otherData[i]);

                // Do we really need to do this?
                if (zeros == Zeros.Include)
                {
                    var otherZero = BuilderInstance<TOther>.Matrix.Zero;
                    var count = RowCount * ColumnCount - Data.Length;
                    for (var i = 0; i < count; i++) state = f(state, Zero, otherZero);
                }

                return state;
            }

            var sparseOther = other as SparseCompressedRowMatrixStorage<TOther>;
            if (sparseOther != null)
            {
                var otherRowPointers = sparseOther.RowPointers;
                var otherColumnIndices = sparseOther.ColumnIndices;
                var otherValues = sparseOther.Values;
                var otherZero = BuilderInstance<TOther>.Matrix.Zero;

                if (zeros == Zeros.Include)
                {
                    var k = 0;
                    for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColumnCount; col++)
                        if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                            state = f(state, row == col ? Data[row] : Zero, otherValues[k++]);
                        else
                            state = f(state, row == col ? Data[row] : Zero, otherZero);
                    return state;
                }

                for (var row = 0; row < RowCount; row++)
                {
                    var diagonal = false;

                    var startIndex = otherRowPointers[row];
                    var endIndex = otherRowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                        if (otherColumnIndices[j] == row)
                        {
                            diagonal = true;
                            state = f(state, Data[row], otherValues[j]);
                        }
                        else
                        {
                            state = f(state, Zero, otherValues[j]);
                        }

                    if (!diagonal && row < ColumnCount) state = f(state, Data[row], otherZero);
                }

                return state;
            }

            // FALL BACK

            return base.Fold2Unchecked(other, f, state, zeros);
        }
    }
}