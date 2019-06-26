// <copyright file="SparseCompressedRowMatrixStorage.cs" company="Math.NET">
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

namespace MathNet.Numerics.LinearAlgebra.Storage
{
    [DataContract(Namespace = "urn:MathNet/Numerics/LinearAlgebra")]
    public class SparseCompressedRowMatrixStorage<T> : MatrixStorage<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        // [ruegg] public fields are OK here

        /// <summary>
        ///     The array containing the row indices of the existing rows. Element "i" of the array gives the index of the
        ///     element in the
        ///     <seecreValues array that is first non-zero element in a row "i".
        ///         The last value is equal to ValueCount, so that the number of non-zero entries in row "i" is always
        ///         given by RowPointers[ i+ i] - RowPointers[ i]. This array thus has length RowCount+1.
        /// </summary>
        [DataMember(Order = 1)] public readonly int[] RowPointers;

        /// <summary>
        ///     An array containing the column indices of the non-zero values. Element "j" of the array
        ///     is the number of the column in matrix that contains the j-th value in the <seecreValues array.
        /// </summary>
        [DataMember(Order = 2)] public int[] ColumnIndices;

        /// <summary>
        ///     Array that contains the non-zero elements of matrix. Values of the non-zero elements of matrix are mapped into the
        ///     values
        ///     array using the row-major storage mapping described in a compressed sparse row (CSR) format.
        /// </summary>
        [DataMember(Order = 3)] public T[] Values;

        internal SparseCompressedRowMatrixStorage(int rows, int columns)
            : base(rows, columns)
        {
            RowPointers = new int[rows + 1];
            ColumnIndices = new int[0];
            Values = new T[0];
        }

        /// <summary>
        ///     Gets the number of non zero elements in the matrix.
        /// </summary>
        /// <value>The number of non zero elements.</value>
        public int ValueCount => RowPointers[RowCount];

        /// <summary>
        ///     True if the matrix storage format is dense.
        /// </summary>
        public override bool IsDense => false;


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
        public override T At(int row, int column)
        {
            var index = FindItem(row, column);
            return index >= 0 ? Values[index] : Zero;
        }

        /// <summary>
        ///     Sets the element without range checking.
        /// </summary>
        /// <param name="row"> The row of the element. </param>
        /// <param name="column"> The column of the element. </param>
        /// <param name="value"> The value to set the element to. </param>
        /// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks.</remarks>
        public override void At(int row, int column, T value)
        {
            var index = FindItem(row, column);
            if (index >= 0)
            {
                // Non-zero item found in matrix
                if (Zero.Equals(value))
                    // Delete existing item
                    RemoveAtIndexUnchecked(index, row);
                else
                    // Update item
                    Values[index] = value;
            }
            else
            {
                // Item not found. Add new value
                if (Zero.Equals(value)) return;

                index = ~index;
                var valueCount = RowPointers[RowPointers.Length - 1];

                // Check if the storage needs to be increased
                if (valueCount == Values.Length && valueCount < (long) RowCount * ColumnCount)
                {
                    // Value array is completely full so we increase the size
                    // Determine the increase in size. We will not grow beyond the size of the matrix
                    var size = Math.Min(Values.Length + GrowthSize(), (long) RowCount * ColumnCount);
                    if (size > int.MaxValue)
                    {
                        //throw new NotSupportedException("");
                    }

                    Array.Resize(ref Values, (int) size);
                    Array.Resize(ref ColumnIndices, (int) size);
                }

                // Move all values (with a position larger than index) in the value array to the next position
                // move all values (with a position larger than index) in the columIndices array to the next position
                Array.Copy(Values, index, Values, index + 1, valueCount - index);
                Array.Copy(ColumnIndices, index, ColumnIndices, index + 1, valueCount - index);

                // Add the value and the column index
                Values[index] = value;
                ColumnIndices[index] = column;

                // add 1 to all the row indices for rows bigger than rowIndex
                // so that they point to the correct part of the value array again.
                for (var i = row + 1; i < RowPointers.Length; i++) RowPointers[i] += 1;
            }
        }

        /// <summary>
        ///     Delete value from internal storage
        /// </summary>
        /// <param name="itemIndex">Index of value in nonZeroValues array</param>
        /// <param name="row">Row number of matrix</param>
        /// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks</remarks>
        private void RemoveAtIndexUnchecked(int itemIndex, int row)
        {
            var valueCount = RowPointers[RowPointers.Length - 1];

            // Move all values (with a position larger than index) in the value array to the previous position
            // move all values (with a position larger than index) in the columIndices array to the previous position
            Array.Copy(Values, itemIndex + 1, Values, itemIndex, valueCount - itemIndex - 1);
            Array.Copy(ColumnIndices, itemIndex + 1, ColumnIndices, itemIndex, valueCount - itemIndex - 1);

            // Decrease value in Row
            for (var i = row + 1; i < RowPointers.Length; i++) RowPointers[i] -= 1;

            valueCount -= 1;

            // Check whether we need to shrink the arrays. This is reasonable to do if
            // there are a lot of non-zero elements and storage is two times bigger
            if (valueCount > 1024 && valueCount < Values.Length / 2)
            {
                Array.Resize(ref Values, valueCount);
                Array.Resize(ref ColumnIndices, valueCount);
            }
        }

        /// <summary>
        ///     Find item Index in nonZeroValues array
        /// </summary>
        /// <param name="row">Matrix row index</param>
        /// <param name="column">Matrix column index</param>
        /// <returns>Item index</returns>
        /// <remarks>WARNING: This method is not thread safe. Use "lock" with it and be sure to avoid deadlocks</remarks>
        public int FindItem(int row, int column)
        {
            // Determine bounds in columnIndices array where this item should be searched (using rowIndex)
            return Array.BinarySearch(ColumnIndices, RowPointers[row], RowPointers[row + 1] - RowPointers[row], column);
        }

        /// <summary>
        ///     Calculates the amount with which to grow the storage array's if they need to be
        ///     increased in size.
        /// </summary>
        /// <returns>The amount grown.</returns>
        private int GrowthSize()
        {
            int delta;
            if (Values.Length > 1024)
            {
                delta = Values.Length / 4;
            }
            else
            {
                if (Values.Length > 256)
                    delta = 512;
                else
                    delta = Values.Length > 64 ? 128 : 32;
            }

            return delta;
        }

        public void Normalize()
        {
            NormalizeOrdering();
            NormalizeZeros();
        }

        public void NormalizeOrdering()
        {
            for (var i = 0; i < RowCount; i++)
            {
                var index = RowPointers[i];
                var count = RowPointers[i + 1] - index;
                if (count > 1) Sorting.Sort(ColumnIndices, Values, index, count);
            }
        }

        public void NormalizeZeros()
        {
            MapInplace(x => x, Zeros.AllowSkip);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            var values = Values;
            var hashNum = Math.Min(ValueCount, 25);
            var hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++) hash = hash * 31 + values[i].GetHashCode();
            }

            return hash;
        }

        // CLEARING

        public override void Clear()
        {
            Array.Clear(RowPointers, 0, RowPointers.Length);
        }

        internal override void ClearUnchecked(int rowIndex, int rowCount, int columnIndex, int columnCount)
        {
            if (rowIndex == 0 && columnIndex == 0 && rowCount == RowCount && columnCount == ColumnCount)
            {
                Clear();
                return;
            }

            var valueCount = RowPointers[RowPointers.Length - 1];

            for (var row = rowIndex + rowCount - 1; row >= rowIndex; row--)
            {
                var startIndex = RowPointers[row];
                var endIndex = RowPointers[row + 1];

                // empty row
                if (startIndex == endIndex) continue;

                // multiple entries in row
                var first = Array.BinarySearch(ColumnIndices, startIndex, endIndex - startIndex, columnIndex);
                var last = Array.BinarySearch(ColumnIndices, startIndex, endIndex - startIndex,
                    columnIndex + columnCount - 1);
                if (first < 0) first = ~first;
                if (last < 0) last = ~last - 1;
                var count = last - first + 1;

                if (count > 0)
                {
                    // Move all values (with a position larger than index) in the value array to the previous position
                    // move all values (with a position larger than index) in the columIndices array to the previous position
                    Array.Copy(Values, first + count, Values, first, valueCount - first - count);
                    Array.Copy(ColumnIndices, first + count, ColumnIndices, first, valueCount - first - count);

                    // Decrease value in Row
                    for (var k = row + 1; k < RowPointers.Length; k++) RowPointers[k] -= count;

                    valueCount -= count;
                }
            }

            // Check whether we need to shrink the arrays. This is reasonable to do if
            // there are a lot of non-zero elements and storage is two times bigger
            if (valueCount > 1024 && valueCount < Values.Length / 2)
            {
                Array.Resize(ref Values, valueCount);
                Array.Resize(ref ColumnIndices, valueCount);
            }
        }

        // INITIALIZATION


        public static SparseCompressedRowMatrixStorage<T> OfValue(int rows, int columns, T value)
        {
            if (Zero.Equals(value)) return new SparseCompressedRowMatrixStorage<T>(rows, columns);

            var storage = new SparseCompressedRowMatrixStorage<T>(rows, columns);

            var values = new T[rows * columns];
            for (var i = 0; i < values.Length; i++) values[i] = value;

            var rowPointers = storage.RowPointers;
            for (var i = 0; i <= rows; i++) rowPointers[i] = i * columns;

            var columnIndices = new int[values.Length];
            for (var row = 0; row < rows; row++)
            {
                var offset = row * columns;
                for (var col = 0; col < columns; col++) columnIndices[offset + col] = col;
            }

            rowPointers[rows] = values.Length;
            storage.ColumnIndices = columnIndices;
            storage.Values = values;
            return storage;
        }


        public static SparseCompressedRowMatrixStorage<T> OfInit(int rows, int columns, Func<int, int, T> init)
        {
            var storage = new SparseCompressedRowMatrixStorage<T>(rows, columns);
            var rowPointers = storage.RowPointers;
            var columnIndices = new List<int>();
            var values = new List<T>();

            for (var row = 0; row < rows; row++)
            {
                rowPointers[row] = values.Count;
                for (var col = 0; col < columns; col++)
                {
                    var x = init(row, col);
                    if (!Zero.Equals(x))
                    {
                        values.Add(x);
                        columnIndices.Add(col);
                    }
                }
            }

            rowPointers[rows] = values.Count;
            storage.ColumnIndices = columnIndices.ToArray();
            storage.Values = values.ToArray();
            return storage;
        }

        // MATRIX COPY

        internal override void CopyToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            var sparseTarget = target as SparseCompressedRowMatrixStorage<T>;
            if (sparseTarget != null)
            {
                CopyToUnchecked(sparseTarget);
                return;
            }

            var denseTarget = target as DenseColumnMajorMatrixStorage<T>;
            if (denseTarget != null)
            {
                CopyToUnchecked(denseTarget, existingData);
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear) target.Clear();

            if (ValueCount != 0)
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++) target.At(row, ColumnIndices[j], Values[j]);
                }
        }

        private void CopyToUnchecked(SparseCompressedRowMatrixStorage<T> target)
        {
            target.Values = new T[ValueCount];
            target.ColumnIndices = new int[ValueCount];

            if (ValueCount != 0)
            {
                Array.Copy(Values, 0, target.Values, 0, ValueCount);
                Buffer.BlockCopy(ColumnIndices, 0, target.ColumnIndices, 0, ValueCount * Constants.SizeOfInt);
                Buffer.BlockCopy(RowPointers, 0, target.RowPointers, 0, (RowCount + 1) * Constants.SizeOfInt);
            }
        }

        private void CopyToUnchecked(DenseColumnMajorMatrixStorage<T> target, ExistingData existingData)
        {
            if (existingData == ExistingData.Clear) target.Clear();

            // TODO: proper implementation

            if (ValueCount != 0)
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++) target.At(row, ColumnIndices[j], Values[j]);
                }
        }

        internal override void CopySubMatrixToUnchecked(MatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            var sparseTarget = target as SparseCompressedRowMatrixStorage<T>;
            if (sparseTarget != null)
            {
                CopySubMatrixToUnchecked(sparseTarget,
                    sourceRowIndex, targetRowIndex, rowCount,
                    sourceColumnIndex, targetColumnIndex, columnCount,
                    existingData);
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            for (int i = sourceRowIndex, row = 0; i < sourceRowIndex + rowCount; i++, row++)
            {
                var startIndex = RowPointers[i];
                var endIndex = RowPointers[i + 1];

                for (var j = startIndex; j < endIndex; j++)
                    // check if the column index is in the range
                    if (ColumnIndices[j] >= sourceColumnIndex && ColumnIndices[j] < sourceColumnIndex + columnCount)
                    {
                        var column = ColumnIndices[j] - sourceColumnIndex;
                        target.At(targetRowIndex + row, targetColumnIndex + column, Values[j]);
                    }
            }
        }

        private void CopySubMatrixToUnchecked(SparseCompressedRowMatrixStorage<T> target,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            ExistingData existingData)
        {
            var rowOffset = targetRowIndex - sourceRowIndex;
            var columnOffset = targetColumnIndex - sourceColumnIndex;

            // special case for empty target - much faster
            if (target.ValueCount == 0)
            {
                // note: ValueCount is maximum resulting ValueCount (just using max to avoid internal copying)
                // resulting arrays will likely be smaller - unless all values fit in the chosen range.
                var values = new List<T>(ValueCount);
                var columnIndices = new List<int>(ValueCount);
                var rowPointers = target.RowPointers;

                for (var i = sourceRowIndex; i < sourceRowIndex + rowCount; i++)
                {
                    rowPointers[i + rowOffset] = values.Count;

                    var startIndex = RowPointers[i];
                    var endIndex = RowPointers[i + 1];

                    // note: we might be able to replace this loop with Array.Copy (perf)
                    for (var k = startIndex; k < endIndex; k++)
                        // check if the column index is in the range
                        if (ColumnIndices[k] >= sourceColumnIndex && ColumnIndices[k] < sourceColumnIndex + columnCount)
                        {
                            values.Add(Values[k]);
                            columnIndices.Add(ColumnIndices[k] + columnOffset);
                        }
                }

                for (var i = targetRowIndex + rowCount; i < rowPointers.Length; i++) rowPointers[i] = values.Count;

                target.RowPointers[target.RowCount] = values.Count;
                target.Values = values.ToArray();
                target.ColumnIndices = columnIndices.ToArray();

                return;
            }

            if (existingData == ExistingData.Clear)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            // NOTE: potential for more efficient implementation
            for (int i = sourceRowIndex, row = 0; row < rowCount; i++, row++)
            {
                var startIndex = RowPointers[i];
                var endIndex = RowPointers[i + 1];

                for (var j = startIndex; j < endIndex; j++)
                    // check if the column index is in the range
                    if (ColumnIndices[j] >= sourceColumnIndex && ColumnIndices[j] < sourceColumnIndex + columnCount)
                    {
                        var column = ColumnIndices[j] - sourceColumnIndex;
                        target.At(targetRowIndex + row, targetColumnIndex + column, Values[j]);
                    }
            }
        }

        // ROW COPY

        internal override void CopySubRowToUnchecked(VectorStorage<T> target, int rowIndex,
            int sourceColumnIndex, int targetColumnIndex, int columnCount, ExistingData existingData)
        {
            // Determine bounds in columnIndices array where this item should be searched (using rowIndex)
            var startIndexOfRow = RowPointers[rowIndex];
            var endIndexOfRow = RowPointers[rowIndex + 1];

            if (startIndexOfRow == endIndexOfRow)
            {
                if (existingData == ExistingData.Clear) target.Clear(targetColumnIndex, columnCount);
                return;
            }

            var targetSparse = target as SparseVectorStorage<T>;
            if (targetSparse != null)
            {
                if (sourceColumnIndex == 0 && targetColumnIndex == 0 && columnCount == ColumnCount &&
                    ColumnCount == targetSparse.Length)
                {
                    // rebuild of the values, indices, no clean necessary
                    targetSparse.ValueCount = endIndexOfRow - startIndexOfRow;
                    targetSparse.Values = new T[targetSparse.ValueCount];
                    targetSparse.Indices = new int[targetSparse.ValueCount];
                    Array.Copy(ColumnIndices, startIndexOfRow, targetSparse.Indices, 0, targetSparse.ValueCount);
                    Array.Copy(Values, startIndexOfRow, targetSparse.Values, 0, targetSparse.ValueCount);
                }
                else
                {
                    var sourceStartPos = Array.BinarySearch(ColumnIndices, startIndexOfRow,
                        endIndexOfRow - startIndexOfRow, sourceColumnIndex);
                    if (sourceStartPos < 0) sourceStartPos = ~sourceStartPos;
                    var sourceEndPos = Array.BinarySearch(ColumnIndices, startIndexOfRow,
                        endIndexOfRow - startIndexOfRow, sourceColumnIndex + columnCount);
                    if (sourceEndPos < 0) sourceEndPos = ~sourceEndPos;
                    var positionsToCopy = sourceEndPos - sourceStartPos;
                    if (positionsToCopy > 0)
                    {
                        // rebuild the target (no clean necessary)
                        var targetStartPos = Array.BinarySearch(targetSparse.Indices, 0, targetSparse.ValueCount,
                            targetColumnIndex);
                        if (targetStartPos < 0) targetStartPos = ~targetStartPos;
                        var targetEndPos = Array.BinarySearch(targetSparse.Indices, 0, targetSparse.ValueCount,
                            targetColumnIndex + columnCount);
                        if (targetEndPos < 0) targetEndPos = Math.Max(~targetEndPos, targetStartPos);
                        var newValueCount = targetSparse.ValueCount - (targetEndPos - targetStartPos) + positionsToCopy;
                        var newValues = new T[newValueCount];
                        var newIndices = new int[newValueCount];
                        // copy before
                        Array.Copy(targetSparse.Indices, 0, newIndices, 0, targetStartPos);
                        Array.Copy(targetSparse.Values, 0, newValues, 0, targetStartPos);
                        // copy values themselves, with new positions
                        var shiftRight = targetColumnIndex - sourceColumnIndex;
                        for (var i = 0; i < positionsToCopy; ++i)
                            newIndices[targetStartPos + i] = ColumnIndices[sourceStartPos + i] + shiftRight;
                        Array.Copy(Values, sourceStartPos, newValues, targetStartPos, positionsToCopy);
                        // copy after
                        Array.Copy(targetSparse.Indices, targetEndPos, newIndices, positionsToCopy + targetStartPos,
                            targetSparse.ValueCount - targetEndPos);
                        Array.Copy(targetSparse.Values, targetEndPos, newValues, positionsToCopy + targetStartPos,
                            targetSparse.ValueCount - targetEndPos);
                        targetSparse.Values = newValues;
                        targetSparse.Indices = newIndices;
                        targetSparse.ValueCount = newValueCount;
                    }
                    else
                    {
                        // although there are no values to copy, we still need to clean the existing values (if necessary)
                        if (existingData == ExistingData.Clear) target.Clear(targetColumnIndex, columnCount);
                    }
                }

                return;
            }

            // FALLBACK
            if (existingData == ExistingData.Clear) target.Clear(targetColumnIndex, columnCount);
            // If there are non-zero elements use base class implementation
            for (int i = sourceColumnIndex, j = 0; i < sourceColumnIndex + columnCount; i++, j++)
            {
                var index = FindItem(rowIndex, i);
                target.At(j, index >= 0 ? Values[index] : Zero);
            }
        }

        // TRANSPOSE

        internal override void TransposeToUnchecked(MatrixStorage<T> target, ExistingData existingData)
        {
            var sparseTarget = target as SparseCompressedRowMatrixStorage<T>;
            if (sparseTarget != null)
            {
                TransposeToUnchecked(sparseTarget);
                return;
            }

            var denseTarget = target as DenseColumnMajorMatrixStorage<T>;
            if (denseTarget != null)
            {
                TransposeToUnchecked(denseTarget, existingData);
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear) target.Clear();

            if (ValueCount != 0)
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++) target.At(ColumnIndices[j], row, Values[j]);
                }
        }

        private void TransposeToUnchecked(SparseCompressedRowMatrixStorage<T> target)
        {
            target.Values = new T[ValueCount];
            target.ColumnIndices = new int[ValueCount];
            var cx = target.Values;
            var cp = target.RowPointers;
            var ci = target.ColumnIndices;

            // Column counts
            var w = new int[ColumnCount];
            for (var p = 0; p < RowPointers[RowCount]; p++) w[ColumnIndices[p]]++;

            // Column pointers
            var nz = 0;
            for (var i = 0; i < ColumnCount; i++)
            {
                cp[i] = nz;
                nz += w[i];
                w[i] = cp[i];
            }

            cp[ColumnCount] = nz;

            for (var i = 0; i < RowCount; i++)
            for (var p = RowPointers[i]; p < RowPointers[i + 1]; p++)
            {
                var j = w[ColumnIndices[p]]++;

                // Place A(i,j) as entry C(j,i)
                ci[j] = i;
                cx[j] = Values[p];
            }
        }

        private void TransposeToUnchecked(DenseColumnMajorMatrixStorage<T> target, ExistingData existingData)
        {
            if (existingData == ExistingData.Clear) target.Clear();

            if (ValueCount != 0)
                for (var row = 0; row < RowCount; row++)
                {
                    var targetIndex = row * ColumnCount;
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++) target.Data[targetIndex + ColumnIndices[j]] = Values[j];
                }
        }

        // EXTRACT

        // ENUMERATION

        // FIND

        public override Tuple<int, int, T> Find(Func<T, bool> predicate, Zeros zeros)
        {
            for (var row = 0; row < RowCount; row++)
            {
                var startIndex = RowPointers[row];
                var endIndex = RowPointers[row + 1];
                for (var j = startIndex; j < endIndex; j++)
                    if (predicate(Values[j]))
                        return new Tuple<int, int, T>(row, ColumnIndices[j], Values[j]);
            }

            if (zeros == Zeros.Include && ValueCount < RowCount * ColumnCount)
                if (predicate(Zero))
                {
                    var k = 0;
                    for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColumnCount; col++)
                        if (k < RowPointers[row + 1] && ColumnIndices[k] == col)
                            k++;
                        else
                            return new Tuple<int, int, T>(row, col, Zero);
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
                var k = 0;
                for (var row = 0; row < RowCount; row++)
                for (var col = 0; col < ColumnCount; col++)
                {
                    var available = k < RowPointers[row + 1] && ColumnIndices[k] == col;
                    if (predicate(available ? Values[k++] : Zero, otherData[col * RowCount + row]))
                        return new Tuple<int, int, T, TOther>(row, col, available ? Values[k - 1] : Zero,
                            otherData[col * RowCount + row]);
                }

                return null;
            }

            var diagonalOther = other as DiagonalMatrixStorage<TOther>;
            if (diagonalOther != null)
            {
                var otherData = diagonalOther.Data;
                var otherZero = BuilderInstance<TOther>.Matrix.Zero;

                // Full Scan
                if (zeros == Zeros.Include && predicate(Zero, otherZero))
                {
                    var k = 0;
                    for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColumnCount; col++)
                    {
                        var available = k < RowPointers[row + 1] && ColumnIndices[k] == col;
                        if (predicate(available ? Values[k++] : Zero, row == col ? otherData[row] : otherZero))
                            return new Tuple<int, int, T, TOther>(row, col, available ? Values[k - 1] : Zero,
                                row == col ? otherData[row] : otherZero);
                    }

                    return null;
                }

                // Sparse Scan
                for (var row = 0; row < RowCount; row++)
                {
                    var diagonal = false;
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                        if (ColumnIndices[j] == row)
                        {
                            diagonal = true;
                            if (predicate(Values[j], otherData[row]))
                                return new Tuple<int, int, T, TOther>(row, row, Values[j], otherData[row]);
                        }
                        else
                        {
                            if (predicate(Values[j], otherZero))
                                return new Tuple<int, int, T, TOther>(row, ColumnIndices[j], Values[j], otherZero);
                        }

                    if (!diagonal && row < ColumnCount)
                        if (predicate(Zero, otherData[row]))
                            return new Tuple<int, int, T, TOther>(row, row, Zero, otherData[row]);
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

                if (zeros == Zeros.Include)
                {
                    int k = 0, otherk = 0;
                    for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColumnCount; col++)
                    {
                        var available = k < RowPointers[row + 1] && ColumnIndices[k] == col;
                        var otherAvailable = otherk < otherRowPointers[row + 1] && otherColumnIndices[otherk] == col;
                        if (predicate(available ? Values[k++] : Zero,
                            otherAvailable ? otherValues[otherk++] : otherZero))
                            return new Tuple<int, int, T, TOther>(row, col, available ? Values[k - 1] : Zero,
                                otherAvailable ? otherValues[otherk - 1] : otherZero);
                    }

                    return null;
                }

                for (var row = 0; row < RowCount; row++)
                {
                    var endIndex = RowPointers[row + 1];
                    var otherEndIndex = otherRowPointers[row + 1];
                    var k = RowPointers[row];
                    var otherk = otherRowPointers[row];
                    while (k < endIndex || otherk < otherEndIndex)
                        if (k == endIndex || otherk < otherEndIndex && ColumnIndices[k] > otherColumnIndices[otherk])
                        {
                            if (predicate(Zero, otherValues[otherk++]))
                                return new Tuple<int, int, T, TOther>(row, otherColumnIndices[otherk - 1], Zero,
                                    otherValues[otherk - 1]);
                        }
                        else if (otherk == otherEndIndex || ColumnIndices[k] < otherColumnIndices[otherk])
                        {
                            if (predicate(Values[k++], otherZero))
                                return new Tuple<int, int, T, TOther>(row, ColumnIndices[k - 1], Values[k - 1],
                                    otherZero);
                        }
                        else
                        {
                            if (predicate(Values[k++], otherValues[otherk++]))
                                return new Tuple<int, int, T, TOther>(row, ColumnIndices[k - 1], Values[k - 1],
                                    otherValues[otherk - 1]);
                        }
                }

                return null;
            }

            // FALL BACK

            return base.Find2Unchecked(other, predicate, zeros);
        }

        // FUNCTIONAL COMBINATORS: MAP

        public override void MapInplace(Func<T, T> f, Zeros zeros)
        {
            if (zeros == Zeros.Include || !Zero.Equals(f(Zero)))
            {
                var newRowPointers = RowPointers;
                var newColumnIndices = new List<int>(ColumnIndices.Length);
                var newValues = new List<T>(Values.Length);

                var k = 0;
                for (var row = 0; row < RowCount; row++)
                {
                    newRowPointers[row] = newValues.Count;
                    for (var col = 0; col < ColumnCount; col++)
                    {
                        var item = k < RowPointers[row + 1] && ColumnIndices[k] == col ? f(Values[k++]) : f(Zero);
                        if (!Zero.Equals(item))
                        {
                            newValues.Add(item);
                            newColumnIndices.Add(col);
                        }
                    }
                }

                ColumnIndices = newColumnIndices.ToArray();
                Values = newValues.ToArray();
                newRowPointers[RowCount] = newValues.Count;
            }
            else
            {
                // we can safely do this in-place:
                var nonZero = 0;
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    RowPointers[row] = nonZero;
                    for (var j = startIndex; j < endIndex; j++)
                    {
                        var item = f(Values[j]);
                        if (!Zero.Equals(item))
                        {
                            Values[nonZero] = item;
                            ColumnIndices[nonZero] = ColumnIndices[j];
                            nonZero++;
                        }
                    }
                }

                Array.Resize(ref ColumnIndices, nonZero);
                Array.Resize(ref Values, nonZero);
                RowPointers[RowCount] = nonZero;
            }
        }

        internal override void MapToUnchecked<TU>(MatrixStorage<TU> target, Func<T, TU> f, Zeros zeros,
            ExistingData existingData)
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(Zero));

            var sparseTarget = target as SparseCompressedRowMatrixStorage<TU>;
            if (sparseTarget != null)
            {
                var newRowPointers = sparseTarget.RowPointers;
                var newColumnIndices = new List<int>(ColumnIndices.Length);
                var newValues = new List<TU>(Values.Length);

                if (processZeros)
                {
                    var k = 0;
                    for (var row = 0; row < RowCount; row++)
                    {
                        newRowPointers[row] = newValues.Count;
                        for (var col = 0; col < ColumnCount; col++)
                        {
                            var item = k < RowPointers[row + 1] && ColumnIndices[k] == col ? f(Values[k++]) : f(Zero);
                            if (!Zero.Equals(item))
                            {
                                newValues.Add(item);
                                newColumnIndices.Add(col);
                            }
                        }
                    }
                }
                else
                {
                    for (var row = 0; row < RowCount; row++)
                    {
                        newRowPointers[row] = newValues.Count;
                        var startIndex = RowPointers[row];
                        var endIndex = RowPointers[row + 1];
                        for (var j = startIndex; j < endIndex; j++)
                        {
                            var item = f(Values[j]);
                            if (!Zero.Equals(item))
                            {
                                newValues.Add(item);
                                newColumnIndices.Add(ColumnIndices[j]);
                            }
                        }
                    }
                }

                sparseTarget.ColumnIndices = newColumnIndices.ToArray();
                sparseTarget.Values = newValues.ToArray();
                newRowPointers[RowCount] = newValues.Count;
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear && !processZeros) target.Clear();

            if (processZeros)
                for (var row = 0; row < RowCount; row++)
                {
                    var index = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = 0; j < ColumnCount; j++)
                        if (index < endIndex && j == ColumnIndices[index])
                        {
                            target.At(row, j, f(Values[index]));
                            index = Math.Min(index + 1, endIndex);
                        }
                        else
                        {
                            target.At(row, j, f(Zero));
                        }
                }
            else
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++) target.At(row, ColumnIndices[j], f(Values[j]));
                }
        }

        internal override void MapIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f, Zeros zeros,
            ExistingData existingData)
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(0, 1, Zero));

            var sparseTarget = target as SparseCompressedRowMatrixStorage<TU>;
            if (sparseTarget != null)
            {
                var newRowPointers = sparseTarget.RowPointers;
                var newColumnIndices = new List<int>(ColumnIndices.Length);
                var newValues = new List<TU>(Values.Length);

                if (processZeros)
                {
                    var k = 0;
                    for (var row = 0; row < RowCount; row++)
                    {
                        newRowPointers[row] = newValues.Count;
                        for (var col = 0; col < ColumnCount; col++)
                        {
                            var item = k < RowPointers[row + 1] && ColumnIndices[k] == col
                                ? f(row, col, Values[k++])
                                : f(row, col, Zero);
                            if (!Zero.Equals(item))
                            {
                                newValues.Add(item);
                                newColumnIndices.Add(col);
                            }
                        }
                    }
                }
                else
                {
                    for (var row = 0; row < RowCount; row++)
                    {
                        newRowPointers[row] = newValues.Count;
                        var startIndex = RowPointers[row];
                        var endIndex = RowPointers[row + 1];
                        for (var j = startIndex; j < endIndex; j++)
                        {
                            var item = f(row, ColumnIndices[j], Values[j]);
                            if (!Zero.Equals(item))
                            {
                                newValues.Add(item);
                                newColumnIndices.Add(ColumnIndices[j]);
                            }
                        }
                    }
                }

                sparseTarget.ColumnIndices = newColumnIndices.ToArray();
                sparseTarget.Values = newValues.ToArray();
                newRowPointers[RowCount] = newValues.Count;
                return;
            }

            // FALL BACK

            if (existingData == ExistingData.Clear && !processZeros) target.Clear();

            if (processZeros)
                for (var row = 0; row < RowCount; row++)
                {
                    var index = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = 0; j < ColumnCount; j++)
                        if (index < endIndex && j == ColumnIndices[index])
                        {
                            target.At(row, j, f(row, j, Values[index]));
                            index = Math.Min(index + 1, endIndex);
                        }
                        else
                        {
                            target.At(row, j, f(row, j, Zero));
                        }
                }
            else
                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                        target.At(row, ColumnIndices[j], f(row, ColumnIndices[j], Values[j]));
                }
        }

        internal override void MapSubMatrixIndexedToUnchecked<TU>(MatrixStorage<TU> target, Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
        {
            var sparseTarget = target as SparseCompressedRowMatrixStorage<TU>;
            if (sparseTarget != null)
            {
                MapSubMatrixIndexedToUnchecked(sparseTarget, f, sourceRowIndex, targetRowIndex, rowCount,
                    sourceColumnIndex, targetColumnIndex, columnCount, zeros, existingData);
                return;
            }

            // FALL BACK

            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(0, 1, Zero));
            if (existingData == ExistingData.Clear && !processZeros)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            if (processZeros)
            {
                for (int sr = sourceRowIndex, tr = targetRowIndex; sr < sourceRowIndex + rowCount; sr++, tr++)
                {
                    var index = RowPointers[sr];
                    var endIndex = RowPointers[sr + 1];

                    // move forward to our sub-range
                    for (; ColumnIndices[index] < sourceColumnIndex && index < endIndex; index++)
                    {
                    }

                    for (int sc = sourceColumnIndex, tc = targetColumnIndex;
                        sc < sourceColumnIndex + columnCount;
                        sc++, tc++)
                        if (index < endIndex && sc == ColumnIndices[index])
                        {
                            target.At(tr, tc, f(tr, tc, Values[index]));
                            index = Math.Min(index + 1, endIndex);
                        }
                        else
                        {
                            target.At(tr, tc, f(tr, tc, Zero));
                        }
                }
            }
            else
            {
                var columnOffset = targetColumnIndex - sourceColumnIndex;
                for (int sr = sourceRowIndex, tr = targetRowIndex; sr < sourceRowIndex + rowCount; sr++, tr++)
                {
                    var startIndex = RowPointers[sr];
                    var endIndex = RowPointers[sr + 1];
                    for (var k = startIndex; k < endIndex; k++)
                        // check if the column index is in the range
                        if (ColumnIndices[k] >= sourceColumnIndex && ColumnIndices[k] < sourceColumnIndex + columnCount)
                        {
                            var tc = ColumnIndices[k] + columnOffset;
                            target.At(tr, tc, f(tr, tc, Values[k]));
                        }
                }
            }
        }

        private void MapSubMatrixIndexedToUnchecked<TU>(SparseCompressedRowMatrixStorage<TU> target,
            Func<int, int, T, TU> f,
            int sourceRowIndex, int targetRowIndex, int rowCount,
            int sourceColumnIndex, int targetColumnIndex, int columnCount,
            Zeros zeros, ExistingData existingData)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var processZeros = zeros == Zeros.Include || !Zero.Equals(f(0, 1, Zero));
            if (existingData == ExistingData.Clear && !processZeros)
                target.ClearUnchecked(targetRowIndex, rowCount, targetColumnIndex, columnCount);

            var rowOffset = targetRowIndex - sourceRowIndex;
            var columnOffset = targetColumnIndex - sourceColumnIndex;
            var zero = Matrix<TU>.Zero;

            // special case for empty target - much faster
            if (target.ValueCount == 0)
            {
                var values = new List<TU>(ValueCount);
                var columnIndices = new List<int>(ValueCount);
                var rowPointers = target.RowPointers;

                if (processZeros)
                    for (var sr = sourceRowIndex; sr < sourceRowIndex + rowCount; sr++)
                    {
                        var tr = sr + rowOffset;
                        rowPointers[tr] = values.Count;

                        var index = RowPointers[sr];
                        var endIndex = RowPointers[sr + 1];

                        // move forward to our sub-range
                        for (; ColumnIndices[index] < sourceColumnIndex && index < endIndex; index++)
                        {
                        }

                        for (int sc = sourceColumnIndex, tc = targetColumnIndex;
                            sc < sourceColumnIndex + columnCount;
                            sc++, tc++)
                            if (index < endIndex && sc == ColumnIndices[index])
                            {
                                var item = f(tr, tc, Values[index]);
                                if (!zero.Equals(item))
                                {
                                    values.Add(item);
                                    columnIndices.Add(tc);
                                }

                                index = Math.Min(index + 1, endIndex);
                            }
                            else
                            {
                                var item = f(tr, tc, Zero);
                                if (!zero.Equals(item))
                                {
                                    values.Add(item);
                                    columnIndices.Add(tc);
                                }
                            }
                    }
                else
                    for (var sr = sourceRowIndex; sr < sourceRowIndex + rowCount; sr++)
                    {
                        var tr = sr + rowOffset;
                        rowPointers[tr] = values.Count;

                        var startIndex = RowPointers[sr];
                        var endIndex = RowPointers[sr + 1];

                        for (var k = startIndex; k < endIndex; k++)
                            // check if the column index is in the range
                            if (ColumnIndices[k] >= sourceColumnIndex &&
                                ColumnIndices[k] < sourceColumnIndex + columnCount)
                            {
                                var tc = ColumnIndices[k] + columnOffset;
                                var item = f(tr, tc, Values[k]);
                                if (!zero.Equals(item))
                                {
                                    values.Add(item);
                                    columnIndices.Add(tc);
                                }
                            }
                    }

                for (var i = targetRowIndex + rowCount; i < rowPointers.Length; i++) rowPointers[i] = values.Count;

                target.RowPointers[target.RowCount] = values.Count;
                target.Values = values.ToArray();
                target.ColumnIndices = columnIndices.ToArray();
                return;
            }

            // TODO: proper general sparse case - the following is essentially a fall back, not leveraging the target data structure

            if (processZeros)
                for (int sr = sourceRowIndex, tr = targetRowIndex; sr < sourceRowIndex + rowCount; sr++, tr++)
                {
                    var index = RowPointers[sr];
                    var endIndex = RowPointers[sr + 1];

                    // move forward to our sub-range
                    for (; ColumnIndices[index] < sourceColumnIndex && index < endIndex; index++)
                    {
                    }

                    for (int sc = sourceColumnIndex, tc = targetColumnIndex;
                        sc < sourceColumnIndex + columnCount;
                        sc++, tc++)
                        if (index < endIndex && sc == ColumnIndices[index])
                        {
                            target.At(tr, tc, f(tr, tc, Values[index]));
                            index = Math.Min(index + 1, endIndex);
                        }
                        else
                        {
                            target.At(tr, tc, f(tr, tc, Zero));
                        }
                }
            else
                for (int sr = sourceRowIndex, tr = targetRowIndex; sr < sourceRowIndex + rowCount; sr++, tr++)
                {
                    var startIndex = RowPointers[sr];
                    var endIndex = RowPointers[sr + 1];
                    for (var k = startIndex; k < endIndex; k++)
                        // check if the column index is in the range
                        if (ColumnIndices[k] >= sourceColumnIndex && ColumnIndices[k] < sourceColumnIndex + columnCount)
                        {
                            var tc = ColumnIndices[k] + columnOffset;
                            target.At(tr, tc, f(tr, tc, Values[k]));
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
                for (var row = 0; row < RowCount; row++)
                for (var col = 0; col < ColumnCount; col++)
                {
                    var available = k < RowPointers[row + 1] && ColumnIndices[k] == col;
                    state = f(state, available ? Values[k++] : Zero, otherData[col * RowCount + row]);
                }

                return state;
            }

            var diagonalOther = other as DiagonalMatrixStorage<TOther>;
            if (diagonalOther != null)
            {
                var otherData = diagonalOther.Data;
                var otherZero = BuilderInstance<TOther>.Matrix.Zero;

                if (zeros == Zeros.Include)
                {
                    var k = 0;
                    for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColumnCount; col++)
                    {
                        var available = k < RowPointers[row + 1] && ColumnIndices[k] == col;
                        state = f(state, available ? Values[k++] : Zero, row == col ? otherData[row] : otherZero);
                    }

                    return state;
                }

                for (var row = 0; row < RowCount; row++)
                {
                    var diagonal = false;

                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    for (var j = startIndex; j < endIndex; j++)
                        if (ColumnIndices[j] == row)
                        {
                            diagonal = true;
                            state = f(state, Values[j], otherData[row]);
                        }
                        else
                        {
                            state = f(state, Values[j], otherZero);
                        }

                    if (!diagonal && row < ColumnCount) state = f(state, Zero, otherData[row]);
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
                    int k = 0, otherk = 0;
                    for (var row = 0; row < RowCount; row++)
                    for (var col = 0; col < ColumnCount; col++)
                    {
                        var available = k < RowPointers[row + 1] && ColumnIndices[k] == col;
                        var otherAvailable = otherk < otherRowPointers[row + 1] && otherColumnIndices[otherk] == col;
                        state = f(state, available ? Values[k++] : Zero,
                            otherAvailable ? otherValues[otherk++] : otherZero);
                    }

                    return state;
                }

                for (var row = 0; row < RowCount; row++)
                {
                    var startIndex = RowPointers[row];
                    var endIndex = RowPointers[row + 1];
                    var otherStartIndex = otherRowPointers[row];
                    var otherEndIndex = otherRowPointers[row + 1];

                    var j1 = startIndex;
                    var j2 = otherStartIndex;

                    while (j1 < endIndex || j2 < otherEndIndex)
                        if (j1 == endIndex || j2 < otherEndIndex && ColumnIndices[j1] > otherColumnIndices[j2])
                            state = f(state, Zero, otherValues[j2++]);
                        else if (j2 == otherEndIndex || ColumnIndices[j1] < otherColumnIndices[j2])
                            state = f(state, Values[j1++], otherZero);
                        else
                            state = f(state, Values[j1++], otherValues[j2++]);
                }

                return state;
            }

            // FALL BACK

            return base.Fold2Unchecked(other, f, state, zeros);
        }
    }
}