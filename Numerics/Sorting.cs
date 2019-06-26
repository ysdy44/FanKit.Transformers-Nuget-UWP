// <copyright file="Sorting.cs" company="Math.NET">
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

namespace MathNet.Numerics
{
    /// <summary>
    ///     Sorting algorithms for single, tuple and triple lists.
    /// </summary>
    public static class Sorting
    {
        /// <summary>
        ///     Sort a list of keys, in place using the quick sort algorithm using the quick sort algorithm.
        /// </summary>
        /// <typeparam name="T">The type of elements in the key list.</typeparam>
        /// <param name="keys">List to sort.</param>
        /// <param name="comparer">Comparison, defining the sort order.</param>
        public static void Sort<T>(IList<T> keys, IComparer<T> comparer = null)
        {
            var count = keys.Count;
            if (count <= 1) return;

            if (null == comparer) comparer = Comparer<T>.Default;

            if (count == 2)
            {
                if (comparer.Compare(keys[0], keys[1]) > 0) Swap(keys, 0, 1);
                return;
            }

            // insertion sort
            if (count <= 10)
            {
                for (var i = 1; i < count; i++)
                {
                    var key = keys[i];
                    var j = i - 1;
                    while (j >= 0 && comparer.Compare(keys[j], key) > 0)
                    {
                        keys[j + 1] = keys[j];
                        j--;
                    }

                    keys[j + 1] = key;
                }

                return;
            }

            // array case
            var keysArray = keys as T[];
            if (null != keysArray)
            {
                Array.Sort(keysArray, comparer);
                return;
            }

            // generic list case
            var keysList = keys as List<T>;
            if (null != keysList)
            {
                keysList.Sort(comparer);
                return;
            }

            // local sort implementation
            QuickSort(keys, comparer, 0, count - 1);
        }

        /// <summary>
        ///     Sort a list of keys and items with respect to the keys, in place using the quick sort algorithm.
        /// </summary>
        /// <typeparam name="TKey">The type of elements in the key list.</typeparam>
        /// <typeparam name="TItem">The type of elements in the item list.</typeparam>
        /// <param name="keys">List to sort.</param>
        /// <param name="items">List to permute the same way as the key list.</param>
        /// <param name="comparer">Comparison, defining the sort order.</param>
        public static void Sort<TKey, TItem>(IList<TKey> keys, IList<TItem> items, IComparer<TKey> comparer = null)
        {
            var count = keys.Count;
            if (count <= 1) return;

            if (null == comparer) comparer = Comparer<TKey>.Default;

            if (count == 2)
            {
                if (comparer.Compare(keys[0], keys[1]) > 0)
                {
                    Swap(keys, 0, 1);
                    Swap(items, 0, 1);
                }

                return;
            }

            // insertion sort
            if (count <= 10)
            {
                for (var i = 1; i < count; i++)
                {
                    var key = keys[i];
                    var item = items[i];
                    var j = i - 1;
                    while (j >= 0 && comparer.Compare(keys[j], key) > 0)
                    {
                        keys[j + 1] = keys[j];
                        items[j + 1] = items[j];
                        j--;
                    }

                    keys[j + 1] = key;
                    items[j + 1] = item;
                }

                return;
            }

            // array case
            var keysArray = keys as TKey[];
            var itemsArray = items as TItem[];
            if (null != keysArray && null != itemsArray)
            {
                Array.Sort(keysArray, itemsArray, comparer);
                return;
            }

            // local sort implementation
            QuickSort(keys, items, comparer, 0, count - 1);
        }

        /// <summary>
        ///     Sort a list of keys, items1 and items2 with respect to the keys, in place using the quick sort algorithm.
        /// </summary>
        /// <typeparam name="TKey">The type of elements in the key list.</typeparam>
        /// <typeparam name="TItem1">The type of elements in the first item list.</typeparam>
        /// <typeparam name="TItem2">The type of elements in the second item list.</typeparam>
        /// <param name="keys">List to sort.</param>
        /// <param name="items1">First list to permute the same way as the key list.</param>
        /// <param name="items2">Second list to permute the same way as the key list.</param>
        /// <param name="comparer">Comparison, defining the sort order.</param>
        public static void Sort<TKey, TItem1, TItem2>(IList<TKey> keys, IList<TItem1> items1, IList<TItem2> items2,
            IComparer<TKey> comparer = null)
        {
            var count = keys.Count;
            if (count <= 1) return;

            if (null == comparer) comparer = Comparer<TKey>.Default;

            if (count == 2)
            {
                if (comparer.Compare(keys[0], keys[1]) > 0)
                {
                    Swap(keys, 0, 1);
                    Swap(items1, 0, 1);
                    Swap(items2, 0, 1);
                }

                return;
            }

            // insertion sort
            if (count <= 10)
            {
                for (var i = 1; i < count; i++)
                {
                    var key = keys[i];
                    var item1 = items1[i];
                    var item2 = items2[i];
                    var j = i - 1;
                    while (j >= 0 && comparer.Compare(keys[j], key) > 0)
                    {
                        keys[j + 1] = keys[j];
                        items1[j + 1] = items1[j];
                        items2[j + 1] = items2[j];
                        j--;
                    }

                    keys[j + 1] = key;
                    items1[j + 1] = item1;
                    items2[j + 1] = item2;
                }

                return;
            }

            // local sort implementation
            QuickSort(keys, items1, items2, comparer, 0, count - 1);
        }

        /// <summary>
        ///     Sort a range of a list of keys, in place using the quick sort algorithm.
        /// </summary>
        /// <typeparam name="T">The type of element in the list.</typeparam>
        /// <param name="keys">List to sort.</param>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">Comparison, defining the sort order.</param>
        public static void Sort<T>(IList<T> keys, int index, int count, IComparer<T> comparer = null)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0 || index + count > keys.Count) throw new ArgumentOutOfRangeException(nameof(count));

            if (count <= 1) return;

            if (null == comparer) comparer = Comparer<T>.Default;

            if (count == 2)
            {
                if (comparer.Compare(keys[index], keys[index + 1]) > 0) Swap(keys, index, index + 1);
                return;
            }

            // insertion sort
            if (count <= 10)
            {
                var to = index + count;
                for (var i = index + 1; i < to; i++)
                {
                    var key = keys[i];
                    var j = i - 1;
                    while (j >= index && comparer.Compare(keys[j], key) > 0)
                    {
                        keys[j + 1] = keys[j];
                        j--;
                    }

                    keys[j + 1] = key;
                }

                return;
            }

            // array case
            var keysArray = keys as T[];
            if (null != keysArray)
            {
                Array.Sort(keysArray, index, count, comparer);
                return;
            }

            // generic list case
            var keysList = keys as List<T>;
            if (null != keysList)
            {
                keysList.Sort(index, count, comparer);
                return;
            }

            // fall back: local sort implementation
            QuickSort(keys, comparer, index, count - 1);
        }

        /// <summary>
        ///     Sort a list of keys and items with respect to the keys, in place using the quick sort algorithm.
        /// </summary>
        /// <typeparam name="TKey">The type of elements in the key list.</typeparam>
        /// <typeparam name="TItem">The type of elements in the item list.</typeparam>
        /// <param name="keys">List to sort.</param>
        /// <param name="items">List to permute the same way as the key list.</param>
        /// <param name="index">The zero-based starting index of the range to sort.</param>
        /// <param name="count">The length of the range to sort.</param>
        /// <param name="comparer">Comparison, defining the sort order.</param>
        public static void Sort<TKey, TItem>(IList<TKey> keys, IList<TItem> items, int index, int count,
            IComparer<TKey> comparer = null)
        {
            if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0 || index + count > keys.Count) throw new ArgumentOutOfRangeException(nameof(count));

            if (count <= 1) return;

            if (null == comparer) comparer = Comparer<TKey>.Default;

            if (count == 2)
            {
                if (comparer.Compare(keys[index], keys[index + 1]) > 0)
                {
                    Swap(keys, index, index + 1);
                    Swap(items, index, index + 1);
                }

                return;
            }

            // insertion sort
            if (count <= 10)
            {
                var to = index + count;
                for (var i = index + 1; i < to; i++)
                {
                    var key = keys[i];
                    var item = items[i];
                    var j = i - 1;
                    while (j >= index && comparer.Compare(keys[j], key) > 0)
                    {
                        keys[j + 1] = keys[j];
                        items[j + 1] = items[j];
                        j--;
                    }

                    keys[j + 1] = key;
                    items[j + 1] = item;
                }

                return;
            }

            // array case
            var keysArray = keys as TKey[];
            var itemsArray = items as TItem[];
            if (null != keysArray && null != itemsArray)
            {
                Array.Sort(keysArray, itemsArray, index, count, comparer);
                return;
            }

            // fall back: local sort implementation
            QuickSort(keys, items, comparer, index, count - 1);
        }

        /// <summary>
        ///     Recursive implementation for an in place quick sort on a list.
        /// </summary>
        /// <typeparam name="T">The type of the list on which the quick sort is performed.</typeparam>
        /// <param name="keys">The list which is sorted using quick sort.</param>
        /// <param name="comparer">The method with which to compare two elements of the quick sort.</param>
        /// <param name="left">The left boundary of the quick sort.</param>
        /// <param name="right">The right boundary of the quick sort.</param>
        private static void QuickSort<T>(IList<T> keys, IComparer<T> comparer, int left, int right)
        {
            do
            {
                // Pivoting
                var a = left;
                var b = right;
                var p = a + ((b - a) >> 1); // midpoint

                if (comparer.Compare(keys[a], keys[p]) > 0) Swap(keys, a, p);

                if (comparer.Compare(keys[a], keys[b]) > 0) Swap(keys, a, b);

                if (comparer.Compare(keys[p], keys[b]) > 0) Swap(keys, p, b);

                var pivot = keys[p];

                // Hoare Partitioning
                do
                {
                    while (comparer.Compare(keys[a], pivot) < 0) a++;

                    while (comparer.Compare(pivot, keys[b]) < 0) b--;

                    if (a > b) break;

                    if (a < b) Swap(keys, a, b);

                    a++;
                    b--;
                } while (a <= b);

                // In order to limit the recursion depth to log(n), we sort the
                // shorter partition recursively and the longer partition iteratively.
                if (b - left <= right - a)
                {
                    if (left < b) QuickSort(keys, comparer, left, b);

                    left = a;
                }
                else
                {
                    if (a < right) QuickSort(keys, comparer, a, right);

                    right = b;
                }
            } while (left < right);
        }

        /// <summary>
        ///     Recursive implementation for an in place quick sort on a list while reordering one other list accordingly.
        /// </summary>
        /// <typeparam name="T">The type of the list on which the quick sort is performed.</typeparam>
        /// <typeparam name="TItems">The type of the list which is automatically reordered accordingly.</typeparam>
        /// <param name="keys">The list which is sorted using quick sort.</param>
        /// <param name="items">The list which is automatically reordered accordingly.</param>
        /// <param name="comparer">The method with which to compare two elements of the quick sort.</param>
        /// <param name="left">The left boundary of the quick sort.</param>
        /// <param name="right">The right boundary of the quick sort.</param>
        private static void QuickSort<T, TItems>(IList<T> keys, IList<TItems> items, IComparer<T> comparer, int left,
            int right)
        {
            do
            {
                // Pivoting
                var a = left;
                var b = right;
                var p = a + ((b - a) >> 1); // midpoint

                if (comparer.Compare(keys[a], keys[p]) > 0)
                {
                    Swap(keys, a, p);
                    Swap(items, a, p);
                }

                if (comparer.Compare(keys[a], keys[b]) > 0)
                {
                    Swap(keys, a, b);
                    Swap(items, a, b);
                }

                if (comparer.Compare(keys[p], keys[b]) > 0)
                {
                    Swap(keys, p, b);
                    Swap(items, p, b);
                }

                var pivot = keys[p];

                // Hoare Partitioning
                do
                {
                    while (comparer.Compare(keys[a], pivot) < 0) a++;

                    while (comparer.Compare(pivot, keys[b]) < 0) b--;

                    if (a > b) break;

                    if (a < b)
                    {
                        Swap(keys, a, b);
                        Swap(items, a, b);
                    }

                    a++;
                    b--;
                } while (a <= b);

                // In order to limit the recursion depth to log(n), we sort the
                // shorter partition recursively and the longer partition iteratively.
                if (b - left <= right - a)
                {
                    if (left < b) QuickSort(keys, items, comparer, left, b);

                    left = a;
                }
                else
                {
                    if (a < right) QuickSort(keys, items, comparer, a, right);

                    right = b;
                }
            } while (left < right);
        }

        /// <summary>
        ///     Recursive implementation for an in place quick sort on one list while reordering two other lists accordingly.
        /// </summary>
        /// <typeparam name="T">The type of the list on which the quick sort is performed.</typeparam>
        /// <typeparam name="TItems1">The type of the first list which is automatically reordered accordingly.</typeparam>
        /// <typeparam name="TItems2">The type of the second list which is automatically reordered accordingly.</typeparam>
        /// <param name="keys">The list which is sorted using quick sort.</param>
        /// <param name="items1">The first list which is automatically reordered accordingly.</param>
        /// <param name="items2">The second list which is automatically reordered accordingly.</param>
        /// <param name="comparer">The method with which to compare two elements of the quick sort.</param>
        /// <param name="left">The left boundary of the quick sort.</param>
        /// <param name="right">The right boundary of the quick sort.</param>
        private static void QuickSort<T, TItems1, TItems2>(
            IList<T> keys, IList<TItems1> items1, IList<TItems2> items2,
            IComparer<T> comparer,
            int left, int right)
        {
            do
            {
                // Pivoting
                var a = left;
                var b = right;
                var p = a + ((b - a) >> 1); // midpoint

                if (comparer.Compare(keys[a], keys[p]) > 0)
                {
                    Swap(keys, a, p);
                    Swap(items1, a, p);
                    Swap(items2, a, p);
                }

                if (comparer.Compare(keys[a], keys[b]) > 0)
                {
                    Swap(keys, a, b);
                    Swap(items1, a, b);
                    Swap(items2, a, b);
                }

                if (comparer.Compare(keys[p], keys[b]) > 0)
                {
                    Swap(keys, p, b);
                    Swap(items1, p, b);
                    Swap(items2, p, b);
                }

                var pivot = keys[p];

                // Hoare Partitioning
                do
                {
                    while (comparer.Compare(keys[a], pivot) < 0) a++;

                    while (comparer.Compare(pivot, keys[b]) < 0) b--;

                    if (a > b) break;

                    if (a < b)
                    {
                        Swap(keys, a, b);
                        Swap(items1, a, b);
                        Swap(items2, a, b);
                    }

                    a++;
                    b--;
                } while (a <= b);

                // In order to limit the recursion depth to log(n), we sort the
                // shorter partition recursively and the longer partition iteratively.
                if (b - left <= right - a)
                {
                    if (left < b) QuickSort(keys, items1, items2, comparer, left, b);

                    left = a;
                }
                else
                {
                    if (a < right) QuickSort(keys, items1, items2, comparer, a, right);

                    right = b;
                }
            } while (left < right);
        }


        /// <summary>
        ///     Performs an in place swap of two elements in a list.
        /// </summary>
        /// <typeparam name="T">The type of elements stored in the list.</typeparam>
        /// <param name="keys">The list in which the elements are stored.</param>
        /// <param name="a">The index of the first element of the swap.</param>
        /// <param name="b">The index of the second element of the swap.</param>
        private static void Swap<T>(IList<T> keys, int a, int b)
        {
            if (a == b) return;

            var local = keys[a];
            keys[a] = keys[b];
            keys[b] = local;
        }
    }
}