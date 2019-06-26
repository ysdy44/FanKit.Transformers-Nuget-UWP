// <copyright file="Matrix.Solve.cs" company="Math.NET">
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

using MathNet.Numerics.LinearAlgebra.Factorization;

namespace MathNet.Numerics.LinearAlgebra
{
    /// <summary>
    ///     Defines the base class for <c>Matrix</c> classes.
    /// </summary>
    public abstract partial class Matrix<T>
    {
        /// <summary>
        ///     Computes the LU decomposition for a matrix.
        /// </summary>
        /// <returns>The LU decomposition object.</returns>
        public abstract LU<T> LU();

        /// <summary>
        ///     Computes the QR decomposition for a matrix.
        /// </summary>
        /// <param name="method">The type of QR factorization to perform.</param>
        /// <returns>The QR decomposition object.</returns>
        public abstract QR<T> QR(QRMethod method = QRMethod.Thin);


        // Direct Solvers: Full

        /// <summary>
        ///     Solves a system of linear equations, <b>Ax = b</b>, with A QR factorized.
        /// </summary>
        /// <param name="input">The right hand side vector, <b>b</b>.</param>
        /// <param name="result">
        ///     The left hand side <seecreMatrix{ T}, <b>x</b>.
        /// </param>
        public void Solve(Vector<T> input, Vector<T> result)
        {
            if (ColumnCount == RowCount)
            {
                LU().Solve(input, result);
                return;
            }

            QR().Solve(input, result);
        }

        /// <summary>
        ///     Solves a system of linear equations, <b>AX = B</b>, with A QR factorized.
        /// </summary>
        /// <param name="input">
        ///     The right hand side <seecreMatrix{ T}, <b>B</b>.
        /// </param>
        /// <param name="result">
        ///     The left hand side <seecreMatrix{ T}, <b>X</b>.
        /// </param>
        public void Solve(Matrix<T> input, Matrix<T> result)
        {
            if (ColumnCount == RowCount)
            {
                LU().Solve(input, result);
                return;
            }

            QR().Solve(input, result);
        }


        // Direct Solvers: Simple

        /// <summary>
        ///     Solves a system of linear equations, <b>AX = B</b>, with A QR factorized.
        /// </summary>
        /// <param name="input">
        ///     The right hand side <seecreMatrix{ T}, <b>B</b>.
        /// </param>
        /// <returns>
        ///     The left hand side <seecreMatrix{ T}, <b>X</b>.
        /// </returns>
        public Matrix<T> Solve(Matrix<T> input)
        {
            var x = Build.SameAs(this, ColumnCount, input.ColumnCount, true);
            Solve(input, x);
            return x;
        }


        /// <summary>
        ///     Solves a system of linear equations, <b>Ax = b</b>, with A QR factorized.
        /// </summary>
        /// <param name="input">The right hand side vector, <b>b</b>.</param>
        /// <returns>
        ///     The left hand side <seecreVector{ T}, <b>x</b>.
        /// </returns>
        public Vector<T> Solve(Vector<T> input)
        {
            var x = Vector<T>.Build.SameAs(this, ColumnCount);
            Solve(input, x);
            return x;
        }
    }
}