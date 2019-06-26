// <copyright file="DenseColumnMajorMatrixStorage.cs" company="Math.NET">
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
using System.Collections.Generic;
using System.Runtime.Serialization;
using MathNet.Numerics.Threading;

namespace MathNet.Numerics.LinearAlgebra.Storage
{
    [DataContract(Namespace = "urn:MathNet/Numerics/LinearAlgebra")]
    public class DenseColumnMajorMatrixStorage<T> : MatrixStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        // [ruegg] public fields are OK here

        [DataMember(Order = 1)] public readonly T[] Data;

        internal DenseColumnMajorMatrixStorage(int rows, int columns)
            : base(rows, columns)
        {
            Data = new T[rows * columns];
        }

        internal DenseColumnMajorMatrixStorage(int rows, int columns, T[] data)
            : base(rows, columns)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (data.Length != rows * columns)
            {
                // throw new ArgumentOutOfRangeException(nameof(data), string.Format(Resources.ArgumentArrayWrongLength, rows*columns));
            }

            Data = data;
        }

        /// <summary>
        ///     True if the matrix storage format is dense.
        /// </summary>
        public override bool IsDense => true;


        /// <summary>
        ///     Retrieves the requested element without range checking.
        /// </summary>
        public override T At(int row, int column)
        {
            return Data[column * RowCount + row];
        }

        /// <summary>
        ///     Sets the element without range checking.
        /// </summary>
        public override void At(int row, int column, T value)
        {
            Data[column * RowCount + row] = value;
        }

        /// <summary>
        ///     Evaluate the row and column at a specific data index.
        /// </summary>
        private void RowColumnAtIndex(int index, out int row, out int column)
        {
#if NETSTANDARD1_3
            row = index % RowCount;
            column = index / RowCount;
#else
            column = index / RowCount;
            row = index % RowCount;
#endif
        }

        // CLEARING

        public override void Clear()
        {
            Array.Clear(Data, 0, Data.Length);
        }

        internal override void ClearUnchecked(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            if (rowIndex == 0 && columnIndex == 0 && rowCount == RowCount && columnCount == ColumnCount)
            {
                Array.Clear(Data, 0, Data.Length);
                return;
            }

            for (var j = columnIndex; j < columnIndex + columnCount; j++)
                Array.Clear(Data, j * RowCount + rowIndex, rowCount);
        }
        

        // INITIALIZATION


        public static DenseColumnMajorMatrixStorage<T> OfValue(int rows, int columns, T value)
        {
            var storage = new DenseColumnMajorMatrixStorage<T>(rows, columns);
            var data = storage.Data;
            CommonParallel.For(0, data.Length, 4096, (a, b) =>
            {
                for (var i = a; i < b; i++) data[i] = value;
            });
            return storage;
        }

        public static DenseColumnMajorMatrixStorage<T> OfInit(int rows, int columns, Func<int, int, T> init)
        {
            var storage = new DenseColumnMajorMatrixStorage<T>(rows, columns);
            var index = 0;
            for (var j = 0; j < columns; j++)
            for (var i = 0; i < rows; i++)
                storage.Data[index++] = init(i, j);
            return storage;
        }

        public static DenseColumnMajorMatrixStorage<T> OfArray(T[,] array)
        {
            var storage = new DenseColumnMajorMatrixStorage<T>(array.GetLength(0), array.GetLength(1));
            var index = 0;
            for (var j = 0; j < storage.ColumnCount; j++)
            for (var i = 0; i < storage.RowCount; i++)
                storage.Data[index++] = array[i, j];
            return storage;
        }

        // MATRIX COPY

        internal override void CopyToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            var denseTarget = target as DenseColumnMajorMatrixStorage<T>;
            if (denseTarget != null)
            {
                CopyToUnchecked(denseTarget);
                return;
            }

            // FALL BACK

            for (int j = 0, offset = 0; j < ColumnCount; j++, offset += RowCount)
            for (var i = 0; i < RowCount; i++)
                target.At(i, j, Data[i + offset]);
        }

        private void CopyToUnchecked(DenseColumnMajorMatrixStorage<T> target)
        {
            //Buffer.BlockCopy(Data, 0, target.Data, 0, Data.Length * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
            Array.Copy(Data, 0, target.Data, 0, Data.Length);
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
                    targetColumnIndex, columnCount);
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                var index = sourceRowIndex + j * RowCount;
                for (var ii = targetRowIndex; ii < targetRowIndex + rowCount; ii++) target.At(ii, jj, Data[index++]);
            }
        }

        private void CopySubMatrixToUnchecked(DenseColumnMajorMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount)
        {
            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
                //Buffer.BlockCopy(Data, j*RowCount + sourceRowIndex, target.Data, jj*target.RowCount + targetRowIndex, rowCount * System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)));
                Array.Copy(Data, j * RowCount + sourceRowIndex, target.Data, jj * target.RowCount + targetRowIndex,
                    rowCount);
        }

        // ROW COPY

        internal override void CopySubRowToUnchecked(VectorStorage<T> target, int rowIndex, int sourceColumnIndex,
            int targetColumnIndex, int columnCount,
            ExistingData existingData)
        {
            var targetDense = target as DenseVectorStorage<T>;
            if (targetDense != null)
            {
                for (var j = 0; j < columnCount; j++)
                    targetDense.Data[j + targetColumnIndex] = Data[(j + sourceColumnIndex) * RowCount + rowIndex];
                return;
            }

            // FALL BACK

            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
                target.At(jj, Data[j * RowCount + rowIndex]);
        }

        // COLUMN COPY

        internal override void CopySubColumnToUnchecked(VectorStorage<T> target, int columnIndex, int sourceRowIndex,
            int targetRowIndex, int rowCount,
            ExistingData existingData)
        {
            var targetDense = target as DenseVectorStorage<T>;
            if (targetDense != null)
            {
                Array.Copy(Data, columnIndex * RowCount + sourceRowIndex, targetDense.Data, targetRowIndex, rowCount);
                return;
            }

            // FALL BACK

            var offset = columnIndex * RowCount;
            for (int i = sourceRowIndex, ii = targetRowIndex; i < sourceRowIndex + rowCount; i++, ii++)
                target.At(ii, Data[offset + i]);
        }

        // TRANSPOSE

        internal override void TransposeToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            var denseTarget = target as DenseColumnMajorMatrixStorage<T>;
            if (denseTarget != null)
            {
                TransposeToUnchecked(denseTarget);
                return;
            }

            var sparseTarget = target as SparseCompressedRowMatrixStorage<T>;
            if (sparseTarget != null)
            {
                TransposeToUnchecked(sparseTarget);
                return;
            }

            // FALL BACK

            for (int j = 0, offset = 0; j < ColumnCount; j++, offset += RowCount)
            for (var i = 0; i < RowCount; i++)
                target.At(j, i, Data[i + offset]);
        }

        private void TransposeToUnchecked(DenseColumnMajorMatrixStorage<T> target)
        {
            for (var j = 0; j < ColumnCount; j++)
            {
                var index = j * RowCount;
                for (var i = 0; i < RowCount; i++) target.Data[i * ColumnCount + j] = Data[index + i];
            }
        }

        private void TransposeToUnchecked(SparseCompressedRowMatrixStorage<T> target)
        {
            var rowPointers = target.RowPointers;
            var columnIndices = new List<int>();
            var values = new List<T>();

            for (var j = 0; j < ColumnCount; j++)
            {
                rowPointers[j] = values.Count;
                var index = j * RowCount;
                for (var i = 0; i < RowCount; i++)
                    if (!Zero.Equals(Data[index + i]))
                    {
                        values.Add(Data[index + i]);
                        columnIndices.Add(i);
                    }
            }

            rowPointers[ColumnCount] = values.Count;
            target.ColumnIndices = columnIndices.ToArray();
            target.Values = values.ToArray();
        }


        // FIND

        public override Tuple<int, int, T> Find(Func<T, bool> predicate, Zeros zeros)
        {
            for (var i = 0; i < Data.Length; i++)
                if (predicate(Data[i]))
                {
                    int row, column;
                    RowColumnAtIndex(i, out row, out column);
                    return new Tuple<int, int, T>(row, column, Data[i]);
                }

            return null;
        }

        internal override Tuple<int, int, T, TOther> Find2Unchecked<TOther>(MatrixStorage<TOther> other,
            Func<T, TOther, bool> predicate, Zeros zeros)
        {
            var denseOther = other as DenseColumnMajorMatrixStorage<TOther>;
            if (denseOther != null)
            {
                var otherData = denseOther.Data;
                for (var i = 0; i < Data.Length; i++)
                    if (predicate(Data[i], otherData[i]))
                    {
                        int row, column;
                        RowColumnAtIndex(i, out row, out column);
                        return new Tuple<int, int, T, TOther>(row, column, Data[i], otherData[i]);
                    }

                return null;
            }

            var diagonalOther = other as DiagonalMatrixStorage<TOther>;
            if (diagonalOther != null)
            {
                var otherData = diagonalOther.Data;
                var otherZero = BuilderInstance<TOther>.Matrix.Zero;
                var k = 0;
                for (var j = 0; j < ColumnCount; j++)
                for (var i = 0; i < RowCount; i++)
                {
                    if (predicate(Data[k], i == j ? otherData[i] : otherZero))
                        return new Tuple<int, int, T, TOther>(i, j, Data[k], i == j ? otherData[i] : otherZero);
                    k++;
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
                var k = 0;
                for (var row = 0; row < RowCount; row++)
                for (var col = 0; col < ColumnCount; col++)
                    if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                    {
                        if (predicate(Data[col * RowCount + row], otherValues[k]))
                            return new Tuple<int, int, T, TOther>(row, col, Data[col * RowCount + row], otherValues[k]);
                        k++;
                    }
                    else
                    {
                        if (predicate(Data[col * RowCount + row], otherZero))
                            return new Tuple<int, int, T, TOther>(row, col, Data[col * RowCount + row], otherValues[k]);
                    }

                return null;
            }

            // FALL BACK

            return base.Find2Unchecked(other, predicate, zeros);
        }

        // FUNCTIONAL COMBINATORS: MAP

        public override void MapInplace(Func<T, T> f, Zeros zeros)
        {
            CommonParallel.For(0, Data.Length, 4096, (a, b) =>
            {
                for (var i = a; i < b; i++) Data[i] = f(Data[i]);
            });
        }

        internal override void MapToUnchecked<TU>(MatrixStorage<TU> target, Func<T, TU> f,
            Zeros zeros, ExistingData existingData)
        {
            var denseTarget = target as DenseColumnMajorMatrixStorage<TU>;
            if (denseTarget != null)
            {
                CommonParallel.For(0, Data.Length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) denseTarget.Data[i] = f(Data[i]);
                });
                return;
            }

            // FALL BACK

            var index = 0;
            for (var j = 0; j < ColumnCount; j++)
            for (var i = 0; i < RowCount; i++)
                target.At(i, j, f(Data[index++]));
        }

        internal override void MapIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            Zeros zeros, ExistingData existingData)
        {
            var denseTarget = target as DenseColumnMajorMatrixStorage<TU>;
            if (denseTarget != null)
            {
                CommonParallel.For(0, ColumnCount, Math.Max(4096 / RowCount, 32), (a, b) =>
                {
                    var index = a * RowCount;
                    for (var j = a; j < b; j++)
                    for (var i = 0; i < RowCount; i++)
                    {
                        denseTarget.Data[index] = f(i, j, Data[index]);
                        index++;
                    }
                });
                return;
            }

            // FALL BACK

            var index2 = 0;
            for (var j = 0; j < ColumnCount; j++)
            for (var i = 0; i < RowCount; i++)
                target.At(i, j, f(i, j, Data[index2++]));
        }

        internal override void MapSubMatrixIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
        {
            var denseTarget = target as DenseColumnMajorMatrixStorage<TU>;
            if (denseTarget != null)
            {
                CommonParallel.For(0, columnCount, Math.Max(4096 / rowCount, 32), (a, b) =>
                {
                    for (var j = a; j < b; j++)
                    {
                        var sourceIndex = sourceRowIndex + (j + sourceColumnIndex) * RowCount;
                        var targetIndex = targetRowIndex + (j + targetColumnIndex) * target.RowCount;
                        for (var i = 0; i < rowCount; i++)
                            denseTarget.Data[targetIndex++] = f(targetRowIndex + i, targetColumnIndex + j,
                                Data[sourceIndex++]);
                    }
                });
                return;
            }

            // TODO: Proper Sparse Implementation

            // FALL BACK

            for (int j = sourceColumnIndex, jj = targetColumnIndex; j < sourceColumnIndex + columnCount; j++, jj++)
            {
                var index = sourceRowIndex + j * RowCount;
                for (var ii = targetRowIndex; ii < targetRowIndex + rowCount; ii++)
                    target.At(ii, jj, f(ii, jj, Data[index++]));
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
                for (var i = 0; i < Data.Length; i++) state = f(state, Data[i], otherData[i]);
                return state;
            }

            var diagonalOther = other as DiagonalMatrixStorage<TOther>;
            if (diagonalOther != null)
            {
                var otherData = diagonalOther.Data;
                var otherZero = BuilderInstance<TOther>.Matrix.Zero;
                var k = 0;
                for (var j = 0; j < ColumnCount; j++)
                for (var i = 0; i < RowCount; i++)
                {
                    state = f(state, Data[k], i == j ? otherData[i] : otherZero);
                    k++;
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
                var k = 0;
                for (var row = 0; row < RowCount; row++)
                for (var col = 0; col < ColumnCount; col++)
                    if (k < otherRowPointers[row + 1] && otherColumnIndices[k] == col)
                        state = f(state, Data[col * RowCount + row], otherValues[k++]);
                    else
                        state = f(state, Data[col * RowCount + row], otherZero);
                return state;
            }

            // FALL BACK

            return base.Fold2Unchecked(other, f, state, zeros);
        }
    }
}