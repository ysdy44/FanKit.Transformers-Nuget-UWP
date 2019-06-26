// <copyright file="Vector.cs" company="Math.NET">
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
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using System.Runtime.CompilerServices;
using MathNet.Numerics.LinearAlgebra.Storage;

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    ///     Defines the generic class for <c>Vector</c> classes.
    /// </summary>
    /// <typeparam name="T">Supported data types are double, single, <seecreComplex, and <seecreComplex32.</typeparam>
    public abstract partial class Vector<T> :
        IFormattable, IEquatable<Vector<T>>, IList, IList<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        public static readonly VectorBuilder<T> Build = BuilderInstance<T>.Vector;

        /// <summary>
        ///     Initializes a new instance of the Vector class.
        /// </summary>
        protected Vector(VectorStorage<T> storage)
        {
            Storage = storage;
            Count = storage.Length;
        }

        /// <summary>
        ///     Gets the raw vector data storage.
        /// </summary>
        public VectorStorage<T> Storage { get; }

        /// <summary>
        ///     Gets the length or number of dimensions of this vector.
        /// </summary>
        public int Count { get; }

        /// <summary>
        ///     Resets all values to zero.
        /// </summary>
        public void Clear()
        {
            Storage.Clear();
        }

        /// <summary>Gets or sets the value at the given <paramref name="index.</summary>
        /// <param name="index">The index of the value to get or set.</param>
        /// <returns>The value of the vector at the given <paramref name="index.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="index is negative or
        /// greater than the size of the vector.
        /// 
        /// </exception>
        public T this[int index]
        {
#if !NET40
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            get { return Storage[index]; }

#if !NET40
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
            set { Storage[index] = value; }
        }

        /// <summary>Gets the value at the given <paramref name="index without range checking..</summary>
        /// <param name="index">The index of the value to get or set.</param>
        /// <returns>The value of the vector at the given <paramref name="index.</returns>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public T At(int index)
        {
            return Storage.At(index);
        }

        /// <summary>Sets the <paramref name="value at the given <paramref name="index without range checking..</summary>
        /// <param name="index">The index of the value to get or set.</param>
        /// <param name="value">The value to set.</param>
#if !NET40
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
#endif
        public void At(int index, T value)
        {
            Storage.At(index, value);
        }

        /// <summary>
        ///     Sets all values of a subvector to zero.
        /// </summary>
        public void ClearSubVector(int index, int count)
        {
            if (count < 1)
            {
                //  throw new ArgumentOutOfRangeException(nameof(count), Resources.ArgumentMustBePositive);
            }

            if (index + count > Count || index < 0) throw new ArgumentOutOfRangeException(nameof(index));

            Storage.Clear(index, count);
        }


        /// <summary>
        ///     Returns a deep-copy clone of the vector.
        /// </summary>
        /// <returns>A deep-copy clone of the vector.</returns>
        public Vector<T> Clone()
        {
            var result = Build.SameAs(this);
            Storage.CopyToUnchecked(result.Storage, ExistingData.AssumeZeros);
            return result;
        }


        /// <summary>
        ///     Copies the values of this vector into the target vector.
        /// </summary>
        /// <param name="target">The vector to copy elements into.</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="target is 
        /// 
        /// <see langword="null.
        /// 
        /// </exception>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="target is not the same size as this vector.
        /// 
        /// </exception>
        public void CopyTo(Vector<T> target)
        {
            if (target == null) throw new ArgumentNullException(nameof(target));

            Storage.CopyTo(target.Storage);
        }


        /// <summary>
        ///     Create a matrix based on this vector in column form (one single column).
        /// </summary>
        /// <returns>
        ///     This vector as a column matrix.
        /// </returns>
        public Matrix<T> ToColumnMatrix()
        {
            var result = Matrix<T>.Build.SameAs(this, Count, 1);
            Storage.CopyToColumnUnchecked(result.Storage, 0, ExistingData.AssumeZeros);
            return result;
        }


        /// <summary>
        ///     Returns an IEnumerable that can be used to iterate through all values of the vector.
        /// </summary>
        /// <remarks>
        ///     The enumerator will include all values, even if they are zero.
        /// </remarks>
        public IEnumerable<T> Enumerate()
        {
            return Storage.Enumerate();
        }


        /// <summary>
        ///     Applies a function to each value of this vector and replaces the value in the result vector.
        ///     If forceMapZero is not set to true, zero values may or may not be skipped depending
        ///     on the actual data storage implementation (relevant mostly for sparse vectors).
        /// </summary>
        public void Map(Func<T, T> f, Vector<T> result, Zeros zeros = Zeros.AllowSkip)
        {
            if (ReferenceEquals(this, result))
                Storage.MapInplace(f, zeros);
            else
                Storage.MapTo(result.Storage, f, zeros,
                    zeros == Zeros.Include ? ExistingData.AssumeZeros : ExistingData.Clear);
        }


        /// <summary>
        ///     Applies a function to each value pair of two vectors and replaces the value in the result vector.
        /// </summary>
        public void Map2(Func<T, T, T> f, Vector<T> other, Vector<T> result, Zeros zeros = Zeros.AllowSkip)
        {
            Storage.Map2To(result.Storage, other.Storage, f, zeros, ExistingData.Clear);
        }


        /// <summary>
        ///     Returns a tuple with the index and value of the first element satisfying a predicate, or null if none is found.
        ///     Zero elements may be skipped on sparse data structures if allowed (default).
        /// </summary>
        public Tuple<int, T> Find(Func<T, bool> predicate, Zeros zeros = Zeros.AllowSkip)
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