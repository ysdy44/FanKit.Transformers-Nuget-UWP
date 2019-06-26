// <copyright file="Builder.cs" company="Math.NET">
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
using MathNet.Numerics.LinearAlgebra.Storage;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    internal class MatrixBuilder : MatrixBuilder<double>
    {
        public override double Zero => 0d;

        public override double One => 1d;

        public override Matrix<double> Dense(DenseColumnMajorMatrixStorage<double> storage)
        {
            return new DenseMatrix(storage);
        }

        public override Matrix<double> Sparse(SparseCompressedRowMatrixStorage<double> storage)
        {
            return new SparseMatrix(storage);
        }

        public override Matrix<double> Diagonal(DiagonalMatrixStorage<double> storage)
        {
            return new DiagonalMatrix(storage);
        }
    }

    internal class VectorBuilder : VectorBuilder<double>
    {
        public override double Zero => 0d;

        public override double One => 1d;

        public override Vector<double> Dense(DenseVectorStorage<double> storage)
        {
            return new DenseVector(storage);
        }

        public override Vector<double> Sparse(SparseVectorStorage<double> storage)
        {
            return new SparseVector(storage);
        }
    }
}

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    ///     Generic linear algebra type builder, for situations where a matrix or vector
    ///     must be created in a generic way. Usage of generic builders should not be
    ///     required in normal user code.
    /// </summary>
    public abstract class MatrixBuilder<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        ///     Gets the value of <c>0.0</c> for type T.
        /// </summary>
        public abstract T Zero { get; }

        /// <summary>
        ///     Gets the value of <c>1.0</c> for type T.
        /// </summary>
        public abstract T One { get; }

        /// <summary>
        ///     Create a new matrix with the same kind of the provided example.
        /// </summary>
        public Matrix<T> SameAs<TU>(Matrix<TU> example, int rows, int columns, bool fullyMutable = false)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            var storage = example.Storage;
            if (storage is DenseColumnMajorMatrixStorage<T>) return Dense(rows, columns);
            if (storage is DiagonalMatrixStorage<T>)
                return fullyMutable ? Sparse(rows, columns) : Diagonal(rows, columns);
            if (storage is SparseCompressedRowMatrixStorage<T>) return Sparse(rows, columns);
            return Dense(rows, columns);
        }

        /// <summary>
        ///     Create a new matrix with the same kind and dimensions of the provided example.
        /// </summary>
        public Matrix<T> SameAs<TU>(Matrix<TU> example)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            return SameAs(example, example.RowCount, example.ColumnCount);
        }

        /// <summary>
        ///     Create a new matrix with the same kind of the provided example.
        /// </summary>
        public Matrix<T> SameAs(Vector<T> example, int rows, int columns)
        {
            return example.Storage.IsDense ? Dense(rows, columns) : Sparse(rows, columns);
        }

        /// <summary>
        ///     Create a new matrix with a type that can represent and is closest to both provided samples.
        /// </summary>
        public Matrix<T> SameAs(Matrix<T> example, Matrix<T> otherExample, int rows, int columns,
            bool fullyMutable = false)
        {
            var storage1 = example.Storage;
            var storage2 = otherExample.Storage;
            if (storage1 is DenseColumnMajorMatrixStorage<T> || storage2 is DenseColumnMajorMatrixStorage<T>)
                return Dense(rows, columns);
            if (storage1 is DiagonalMatrixStorage<T> && storage2 is DiagonalMatrixStorage<T>)
                return fullyMutable ? Sparse(rows, columns) : Diagonal(rows, columns);
            if (storage1 is SparseCompressedRowMatrixStorage<T> || storage2 is SparseCompressedRowMatrixStorage<T>)
                return Sparse(rows, columns);
            return Dense(rows, columns);
        }

        /// <summary>
        ///     Create a new matrix with a type that can represent and is closest to both provided samples and the dimensions of
        ///     example.
        /// </summary>
        public Matrix<T> SameAs(Matrix<T> example, Matrix<T> otherExample)
        {
            return SameAs(example, otherExample, example.RowCount, example.ColumnCount);
        }


        /// <summary>
        ///     Create a new dense matrix straight from an initialized matrix storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public abstract Matrix<T> Dense(DenseColumnMajorMatrixStorage<T> storage);

        /// <summary>
        ///     Create a new dense matrix with the given number of rows and columns.
        ///     All cells of the matrix will be initialized to zero.
        ///     Zero-length matrices are not supported.
        /// </summary>
        public Matrix<T> Dense(int rows, int columns)
        {
            return Dense(new DenseColumnMajorMatrixStorage<T>(rows, columns));
        }

        /// <summary>
        ///     Create a new dense matrix with the given number of rows and columns directly binding to a raw array.
        ///     The array is assumed to be in column-major order (column by column) and is used directly without copying.
        ///     Very efficient, but changes to the array and the matrix will affect each other.
        /// </summary>
        /// <seealso href="http://en.wikipedia.org/wiki/Row-major_order
        public Matrix<T> Dense(int rows, int columns, T[] storage)
        {
            return Dense(new DenseColumnMajorMatrixStorage<T>(rows, columns, storage));
        }

        /// <summary>
        ///     Create a new dense matrix and initialize each value to the same provided value.
        /// </summary>
        public Matrix<T> Dense(int rows, int columns, T value)
        {
            if (Zero.Equals(value)) return Dense(rows, columns);
            return Dense(DenseColumnMajorMatrixStorage<T>.OfValue(rows, columns, value));
        }

        /// <summary>
        ///     Create a new dense matrix and initialize each value using the provided init function.
        /// </summary>
        public Matrix<T> Dense(int rows, int columns, Func<int, int, T> init)
        {
            return Dense(DenseColumnMajorMatrixStorage<T>.OfInit(rows, columns, init));
        }

        /// <summary>
        ///     Create a new sparse matrix straight from an initialized matrix storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public abstract Matrix<T> Sparse(SparseCompressedRowMatrixStorage<T> storage);

        /// <summary>
        ///     Create a sparse matrix of T with the given number of rows and columns.
        /// </summary>
        /// <param name="rows">The number of rows.</param>
        /// <param name="columns">The number of columns.</param>
        public Matrix<T> Sparse(int rows, int columns)
        {
            return Sparse(new SparseCompressedRowMatrixStorage<T>(rows, columns));
        }

        /// <summary>
        ///     Create a new diagonal matrix straight from an initialized matrix storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public abstract Matrix<T> Diagonal(DiagonalMatrixStorage<T> storage);

        /// <summary>
        ///     Create a new diagonal matrix with the given number of rows and columns.
        ///     All cells of the matrix will be initialized to zero.
        ///     Zero-length matrices are not supported.
        /// </summary>
        public Matrix<T> Diagonal(int rows, int columns)
        {
            return Diagonal(new DiagonalMatrixStorage<T>(rows, columns));
        }
    }

    /// <summary>
    ///     Generic linear algebra type builder, for situations where a matrix or vector
    ///     must be created in a generic way. Usage of generic builders should not be
    ///     required in normal user code.
    /// </summary>
    public abstract class VectorBuilder<T> where T : struct, IEquatable<T>, IFormattable
    {
        /// <summary>
        ///     Gets the value of <c>0.0</c> for type T.
        /// </summary>
        public abstract T Zero { get; }

        /// <summary>
        ///     Gets the value of <c>1.0</c> for type T.
        /// </summary>
        public abstract T One { get; }

        /// <summary>
        ///     Create a new vector with the same kind of the provided example.
        /// </summary>
        public Vector<T> SameAs<TU>(Vector<TU> example, int length)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            return example.Storage.IsDense ? Dense(length) : Sparse(length);
        }

        /// <summary>
        ///     Create a new vector with the same kind and dimension of the provided example.
        /// </summary>
        public Vector<T> SameAs<TU>(Vector<TU> example)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            return example.Storage.IsDense ? Dense(example.Count) : Sparse(example.Count);
        }

        /// <summary>
        ///     Create a new vector with the same kind of the provided example.
        /// </summary>
        public Vector<T> SameAs<TU>(Matrix<TU> example, int length)
            where TU : struct, IEquatable<TU>, IFormattable
        {
            return example.Storage.IsDense ? Dense(length) : Sparse(length);
        }

        /// <summary>
        ///     Create a new vector with a type that can represent and is closest to both provided samples and the dimensions of
        ///     example.
        /// </summary>
        public Vector<T> SameAs(Vector<T> example, Vector<T> otherExample)
        {
            return example.Storage.IsDense || otherExample.Storage.IsDense
                ? Dense(example.Count)
                : Sparse(example.Count);
        }

        /// <summary>
        ///     Create a new vector with a type that can represent and is closest to both provided samples.
        /// </summary>
        public Vector<T> SameAs(Matrix<T> matrix, Vector<T> vector, int length)
        {
            return matrix.Storage.IsDense || vector.Storage.IsDense ? Dense(length) : Sparse(length);
        }

        /// <summary>
        ///     Create a new dense vector straight from an initialized vector storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public abstract Vector<T> Dense(DenseVectorStorage<T> storage);

        /// <summary>
        ///     Create a dense vector of T with the given size.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        public Vector<T> Dense(int size)
        {
            return Dense(new DenseVectorStorage<T>(size));
        }

        /// <summary>
        ///     Create a dense vector of T that is directly bound to the specified array.
        /// </summary>
        public Vector<T> Dense(T[] array)
        {
            return Dense(new DenseVectorStorage<T>(array.Length, array));
        }

        /// <summary>
        ///     Create a new dense vector and initialize each value using the provided value.
        /// </summary>
        public Vector<T> Dense(int length, T value)
        {
            if (Zero.Equals(value)) return Dense(length);
            return Dense(DenseVectorStorage<T>.OfValue(length, value));
        }

        /// <summary>
        ///     Create a new dense vector and initialize each value using the provided init function.
        /// </summary>
        public Vector<T> Dense(int length, Func<int, T> init)
        {
            return Dense(DenseVectorStorage<T>.OfInit(length, init));
        }


        /// <summary>
        ///     Create a new sparse vector straight from an initialized vector storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public abstract Vector<T> Sparse(SparseVectorStorage<T> storage);

        /// <summary>
        ///     Create a sparse vector of T with the given size.
        /// </summary>
        /// <param name="size">The size of the vector.</param>
        public Vector<T> Sparse(int size)
        {
            return Sparse(new SparseVectorStorage<T>(size));
        }
    }

    internal static class BuilderInstance<T> where T : struct, IEquatable<T>, IFormattable
    {
        private static readonly Lazy<Tuple<MatrixBuilder<T>, VectorBuilder<T>>> _singleton =
            new Lazy<Tuple<MatrixBuilder<T>, VectorBuilder<T>>>(Create);

        public static MatrixBuilder<T> Matrix => _singleton.Value.Item1;

        public static VectorBuilder<T> Vector => _singleton.Value.Item2;

        private static Tuple<MatrixBuilder<T>, VectorBuilder<T>> Create()
        {
            if (typeof(T) == typeof(double))
                return new Tuple<MatrixBuilder<T>, VectorBuilder<T>>(
                    (MatrixBuilder<T>) (object) new Double.MatrixBuilder(),
                    (VectorBuilder<T>) (object) new Double.VectorBuilder());

            throw new NotSupportedException(string.Format(
                "Matrices and vectors of type '{0}' are not supported. Only Double, Single, Complex or Complex32 are supported at this point.",
                typeof(T).Name));
        }
    }
}