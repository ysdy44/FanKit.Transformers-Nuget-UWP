// <copyright file="Matrix.BCL.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2014 Math.NET
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
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MathNet.Numerics.LinearAlgebra
{
    [DebuggerDisplay("Matrix {RowCount}x{ColumnCount}")]
    public abstract partial class Matrix<T>
    {
        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">
        ///     An object to compare with this object.
        /// </param>
        /// <returns>
        ///     <c>true</c> if the current object is equal to the <paramref name="other parameter; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Matrix<T> other)
        {
            return other != null && Storage.Equals(other.Storage);
        }

        /// <summary>
        ///     Returns a string that summarizes this matrix.
        ///     The maximum number of cells can be configured in the
        ///     <seecreControl class.
        ///         The format string is ignored.
        /// </summary>
        public string ToString(string format = null, IFormatProvider formatProvider = null)
        {
            return string.Concat(ToTypeString(), Environment.NewLine, ToMatrixString(format, formatProvider));
        }

        /// <summary>
        ///     Determines whether the specified <seecreSystem.Object is equal to this instance.
        /// </summary>
        /// <param name="obj">The <seecreSystem.Object to compare with this instance.</param>
        /// <returns>
        ///     <c>true</c> if the specified <seecreSystem.Object is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as Matrix<T>;
            return other != null && Storage.Equals(other.Storage);
        }

        /// <summary>
        ///     Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return Storage.GetHashCode();
        }

        /// <summary>
        ///     Returns a string that describes the type, dimensions and shape of this matrix.
        /// </summary>
        public virtual string ToTypeString()
        {
            return string.Format("{0} {1}x{2}-{3}", GetType().Name, RowCount, ColumnCount, typeof(T).Name);
        }


        /// <summary>
        ///     Returns a string 2D array that summarizes the content of this matrix.
        /// </summary>
        public string[,] ToMatrixStringArray(int upperRows, int lowerRows, int minLeftColumns, int rightColumns,
            int maxWidth, int padding,
            string horizontalEllipsis, string verticalEllipsis, string diagonalEllipsis, Func<T, string> formatValue)
        {
            upperRows = Math.Max(upperRows, 1);
            lowerRows = Math.Max(lowerRows, 0);
            minLeftColumns = Math.Max(minLeftColumns, 1);
            maxWidth = Math.Max(maxWidth, 12);

            var upper = RowCount <= upperRows ? RowCount : upperRows;
            var lower = RowCount <= upperRows ? 0 :
                RowCount <= upperRows + lowerRows ? RowCount - upperRows : lowerRows;
            var rowEllipsis = RowCount > upper + lower;
            var rows = rowEllipsis ? upper + lower + 1 : upper + lower;

            var left = ColumnCount <= minLeftColumns ? ColumnCount : minLeftColumns;
            var right = ColumnCount <= minLeftColumns ? 0 :
                ColumnCount <= minLeftColumns + rightColumns ? ColumnCount - minLeftColumns : rightColumns;

            var columnsLeft = new List<Tuple<int, string[]>>();
            for (var j = 0; j < left; j++)
                columnsLeft.Add(FormatColumn(j, rows, upper, lower, rowEllipsis, verticalEllipsis, formatValue));

            var columnsRight = new List<Tuple<int, string[]>>();
            for (var j = 0; j < right; j++)
                columnsRight.Add(FormatColumn(ColumnCount - right + j, rows, upper, lower, rowEllipsis,
                    verticalEllipsis, formatValue));

            var chars = columnsLeft.Sum(t => t.Item1 + padding) + columnsRight.Sum(t => t.Item1 + padding);
            for (var j = left; j < ColumnCount - right; j++)
            {
                var candidate = FormatColumn(j, rows, upper, lower, rowEllipsis, verticalEllipsis, formatValue);
                chars += candidate.Item1 + padding;
                if (chars > maxWidth) break;
                columnsLeft.Add(candidate);
            }

            var cols = columnsLeft.Count + columnsRight.Count;
            var colEllipsis = ColumnCount > cols;
            if (colEllipsis) cols++;

            var array = new string[rows, cols];
            var colIndex = 0;
            foreach (var column in columnsLeft)
            {
                for (var i = 0; i < column.Item2.Length; i++) array[i, colIndex] = column.Item2[i];
                colIndex++;
            }

            if (colEllipsis)
            {
                var rowIndex = 0;
                for (var row = 0; row < upper; row++) array[rowIndex++, colIndex] = horizontalEllipsis;
                if (rowEllipsis) array[rowIndex++, colIndex] = diagonalEllipsis;
                for (var row = RowCount - lower; row < RowCount; row++)
                    array[rowIndex++, colIndex] = horizontalEllipsis;
                colIndex++;
            }

            foreach (var column in columnsRight)
            {
                for (var i = 0; i < column.Item2.Length; i++) array[i, colIndex] = column.Item2[i];
                colIndex++;
            }

            return array;
        }

        private Tuple<int, string[]> FormatColumn(int column, int height, int upper, int lower, bool withEllipsis,
            string ellipsis, Func<T, string> formatValue)
        {
            var c = new string[height];
            var index = 0;
            for (var row = 0; row < upper; row++) c[index++] = formatValue(At(row, column));
            if (withEllipsis) c[index++] = "";
            for (var row = RowCount - lower; row < RowCount; row++) c[index++] = formatValue(At(row, column));
            var w = c.Max(x => x.Length);
            if (withEllipsis) c[upper] = ellipsis;
            return new Tuple<int, string[]>(w, c);
        }

        private static string FormatStringArrayToString(string[,] array, string columnSeparator, string rowSeparator)
        {
            var rows = array.GetLength(0);
            var cols = array.GetLength(1);

            var widths = new int[cols];
            for (var i = 0; i < rows; i++)
            for (var j = 0; j < cols; j++)
                widths[j] = Math.Max(widths[j], array[i, j].Length);

            var sb = new StringBuilder();
            for (var i = 0; i < rows; i++)
            {
                sb.Append(array[i, 0].PadLeft(widths[0]));
                for (var j = 1; j < cols; j++)
                {
                    sb.Append(columnSeparator);
                    sb.Append(array[i, j].PadLeft(widths[j]));
                }

                sb.Append(rowSeparator);
            }

            return sb.ToString();
        }


        public string ToMatrixString(int upperRows, int lowerRows, int minLeftColumns, int rightColumns, int maxWidth,
            string horizontalEllipsis, string verticalEllipsis, string diagonalEllipsis,
            string columnSeparator, string rowSeparator, Func<T, string> formatValue)
        {
            return FormatStringArrayToString(
                ToMatrixStringArray(upperRows, lowerRows, minLeftColumns, rightColumns, maxWidth,
                    columnSeparator.Length, horizontalEllipsis, verticalEllipsis, diagonalEllipsis, formatValue),
                columnSeparator, rowSeparator);
        }


        /// <summary>
        ///     Returns a string that summarizes the content of this matrix.
        /// </summary>
        public string ToMatrixString(string format = null, IFormatProvider provider = null)
        {
            if (format == null) format = "G6";

            return ToMatrixString(8, 4, 5, 2, 76, "..", "..", "..", "  ", Environment.NewLine,
                x => x.ToString(format, provider));
        }

        /// <summary>
        ///     Returns a string that summarizes this matrix.
        ///     The maximum number of cells can be configured in the <seecreControl class.
        /// </summary>
        public sealed override string ToString()
        {
            return string.Concat(ToTypeString(), Environment.NewLine, ToMatrixString());
        }
    }
}