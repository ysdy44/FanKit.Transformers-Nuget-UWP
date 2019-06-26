﻿// <copyright file="Svd.cs" company="Math.NET">
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

namespace MathNet.Numerics.LinearAlgebra.Factorization
{
    /// <summary>
    ///     <para>A class which encapsulates the functionality of the singular value decomposition (SVD).</para>
    ///     <para>
    ///         Suppose M is an m-by-n matrix whose entries are real numbers.
    ///         Then there exists a factorization of the form M = UΣVT where:
    ///         - U is an m-by-m unitary matrix;
    ///         - Σ is m-by-n diagonal matrix with nonnegative real numbers on the diagonal;
    ///         - VT denotes transpose of V, an n-by-n unitary matrix;
    ///         Such a factorization is called a singular-value decomposition of M. A common convention is to order the
    ///         diagonal
    ///         entries Σ(i,i) in descending order. In this case, the diagonal matrix Σ is uniquely determined
    ///         by M (though the matrices U and V are not). The diagonal entries of Σ are known as the singular values of M.
    ///     </para>
    /// </summary>
    /// <remarks>
    ///     The computation of the singular value decomposition is done at construction time.
    /// </remarks>
    /// <typeparam name="T">Supported data types are double, single, <seecreComplex, and <seecreComplex32.</typeparam>
    public abstract class Svd<T> : ISolver<T>
        where T : struct, IEquatable<T>, IFormattable
    {
        private readonly Lazy<Matrix<T>> _lazyW;

        /// <summary>Indicating whether U and VT matrices have been computed during SVD factorization.</summary>
        protected readonly bool VectorsComputed;

        protected Svd(Vector<T> s, Matrix<T> u, Matrix<T> vt, bool vectorsComputed)
        {
            S = s;
            U = u;
            VT = vt;

            VectorsComputed = vectorsComputed;

            _lazyW = new Lazy<Matrix<T>>(ComputeW);
        }

        /// <summary>
        ///     Gets the singular values (Σ) of matrix in ascending value.
        /// </summary>
        public Vector<T> S { get; }

        /// <summary>
        ///     Gets the left singular vectors (U - m-by-m unitary matrix)
        /// </summary>
        public Matrix<T> U { get; }

        /// <summary>
        ///     Gets the transpose right singular vectors (transpose of V, an n-by-n unitary matrix)
        /// </summary>
        public Matrix<T> VT { get; }

        /// <summary>
        ///     Returns the singular values as a diagonal <seecreMatrix{ T}.
        /// </summary>
        /// <returns>
        ///     The singular values as a diagonal <seecreMatrix{ T}.</returns>
        public Matrix<T> W => _lazyW.Value;

        /// <summary>
        ///     Gets the two norm of the <seecreMatrix{ T}.
        /// </summary>
        /// <returns>
        ///     The 2-norm of the <seecreMatrix{ T}.</returns>
        public abstract double L2Norm { get; }

        /// <summary>
        ///     Solves a system of linear equations, <b>AX = B</b>, with A SVD factorized.
        /// </summary>
        /// <param name="input">
        ///     The right hand side <seecreMatrix{ T}, <b>B</b>.
        /// </param>
        /// <returns>
        ///     The left hand side <seecreMatrix{ T}, <b>X</b>.
        /// </returns>
        public virtual Matrix<T> Solve(Matrix<T> input)
        {
            if (!VectorsComputed)
            {
                // throw new InvalidOperationException(Resources.SingularVectorsNotComputed);
            }

            var x = Matrix<T>.Build.SameAs(U, VT.ColumnCount, input.ColumnCount, true);
            Solve(input, x);
            return x;
        }

        /// <summary>
        ///     Solves a system of linear equations, <b>AX = B</b>, with A SVD factorized.
        /// </summary>
        /// <param name="input">
        ///     The right hand side <seecreMatrix{ T}, <b>B</b>.
        /// </param>
        /// <param name="result">
        ///     The left hand side <seecreMatrix{ T}, <b>X</b>.
        /// </param>
        public abstract void Solve(Matrix<T> input, Matrix<T> result);

        /// <summary>
        ///     Solves a system of linear equations, <b>Ax = b</b>, with A SVD factorized.
        /// </summary>
        /// <param name="input">The right hand side vector, <b>b</b>.</param>
        /// <returns>
        ///     The left hand side <seecreVector{ T}, <b>x</b>.
        /// </returns>
        public virtual Vector<T> Solve(Vector<T> input)
        {
            if (!VectorsComputed)
            {
                // throw new InvalidOperationException(Resources.SingularVectorsNotComputed);
            }

            var x = Vector<T>.Build.SameAs(U, VT.ColumnCount);
            Solve(input, x);
            return x;
        }

        /// <summary>
        ///     Solves a system of linear equations, <b>Ax = b</b>, with A SVD factorized.
        /// </summary>
        /// <param name="input">The right hand side vector, <b>b</b>.</param>
        /// <param name="result">
        ///     The left hand side <seecreMatrix{ T}, <b>x</b>.
        /// </param>
        public abstract void Solve(Vector<T> input, Vector<T> result);

        private Matrix<T> ComputeW()
        {
            var rows = U.RowCount;
            var columns = VT.ColumnCount;
            var result = Matrix<T>.Build.SameAs(U, rows, columns);
            for (var i = 0; i < rows; i++)
            for (var j = 0; j < columns; j++)
                if (i == j)
                    result.At(i, i, S[i]);

            return result;
        }
    }
}