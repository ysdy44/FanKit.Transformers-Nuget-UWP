// <copyright file="ILinearAlgebraProvider.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2018 Math.NET
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

using System.Numerics;
using MathNet.Numerics.LinearAlgebra.Factorization;

namespace MathNet.Numerics.Providers.LinearAlgebra
{
    /// <summary>
    ///     How to transpose a matrix.
    /// </summary>
    public enum Transpose
    {
        /// <summary>
        ///     Don't transpose a matrix.
        /// </summary>
        DontTranspose = 111,

        /// <summary>
        ///     Transpose a matrix.
        /// </summary>
        Transpose = 112,

        /// <summary>
        ///     Conjugate transpose a complex matrix.
        /// </summary>
        /// <remarks>If a conjugate transpose is used with a real matrix, then the matrix is just transposed.</remarks>
        ConjugateTranspose = 113
    }



    /// <summary>
    ///     Interface to linear algebra algorithms that work off 1-D arrays.
    /// </summary>
    public interface ILinearAlgebraProvider :
        ILinearAlgebraProvider<double>,
        ILinearAlgebraProvider<Complex>,
        ILinearAlgebraProvider<Complex32>
    {
        /// <summary>
        ///     Initialize and verify that the provided is indeed available. If not, fall back to alternatives like the managed
        ///     provider
        /// </summary>
        void InitializeVerify();
    }

    /// <summary>
    ///     Interface to linear algebra algorithms that work off 1-D arrays.
    /// </summary>
    /// <typeparam name="T">Supported data types are Double, Single, Complex, and Complex32.</typeparam>
    public interface ILinearAlgebraProvider<T>
        where T : struct
    {
        /*/// <summary>
        /// Queries the provider for the optimal, workspace block size
        /// for the given routine.
        /// </summary>
        /// <param name="methodName">Name of the method to query.</param>
        /// <returns>-1 if the provider cannot compute the workspace size; otherwise
        /// the suggested block size.</returns>
        int QueryWorkspaceBlockSize(string methodName);*/

        /// <summary>
        ///     Scales an array. Can be used to scale a vector and a matrix.
        /// </summary>
        /// <param name="alpha">The scalar.</param>
        /// <param name="x">The values to scale.</param>
        /// <param name="result">This result of the scaling.</param>
        /// <remarks>This is similar to the SCAL BLAS routine.</remarks>
        void ScaleArray(T alpha, T[] x, T[] result);

        /// <summary>
        ///     Computes the dot product of x and y.
        /// </summary>
        /// <param name="x">The vector x.</param>
        /// <param name="y">The vector y.</param>
        /// <returns>The dot product of x and y.</returns>
        /// <remarks>This is equivalent to the DOT BLAS routine.</remarks>
        T DotProduct(T[] x, T[] y);

        /// <summary>
        ///     Does a point wise add of two arrays <c>z = x + y</c>. This can be used
        ///     to add vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the addition.</param>
        /// <remarks>
        ///     There is no equivalent BLAS routine, but many libraries
        ///     provide optimized (parallel and/or vectorized) versions of this
        ///     routine.
        /// </remarks>
        void AddArrays(T[] x, T[] y, T[] result);

        /// <summary>
        ///     Does a point wise subtraction of two arrays <c>z = x - y</c>. This can be used
        ///     to subtract vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the subtraction.</param>
        /// <remarks>
        ///     There is no equivalent BLAS routine, but many libraries
        ///     provide optimized (parallel and/or vectorized) versions of this
        ///     routine.
        /// </remarks>
        void SubtractArrays(T[] x, T[] y, T[] result);

        /// <summary>
        ///     Does a point wise multiplication of two arrays <c>z = x * y</c>. This can be used
        ///     to multiply elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise multiplication.</param>
        /// <remarks>
        ///     There is no equivalent BLAS routine, but many libraries
        ///     provide optimized (parallel and/or vectorized) versions of this
        ///     routine.
        /// </remarks>
        void PointWiseMultiplyArrays(T[] x, T[] y, T[] result);

        /// <summary>
        ///     Does a point wise division of two arrays <c>z = x / y</c>. This can be used
        ///     to divide elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise division.</param>
        /// <remarks>
        ///     There is no equivalent BLAS routine, but many libraries
        ///     provide optimized (parallel and/or vectorized) versions of this
        ///     routine.
        /// </remarks>
        void PointWiseDivideArrays(T[] x, T[] y, T[] result);

        /// <summary>
        ///     Multiples two matrices. <c>result = x * y</c>
        /// </summary>
        /// <param name="x">The x matrix.</param>
        /// <param name="rowsX">The number of rows in the x matrix.</param>
        /// <param name="columnsX">The number of columns in the x matrix.</param>
        /// <param name="y">The y matrix.</param>
        /// <param name="rowsY">The number of rows in the y matrix.</param>
        /// <param name="columnsY">The number of columns in the y matrix.</param>
        /// <param name="result">Where to store the result of the multiplication.</param>
        /// <remarks>
        ///     This is a simplified version of the BLAS GEMM routine with alpha
        ///     set to 1.0 and beta set to 0.0, and x and y are not transposed.
        /// </remarks>
        void MatrixMultiply(T[] x, int rowsX, int columnsX, T[] y, int rowsY, int columnsY, T[] result);

        /// <summary>
        ///     Multiplies two matrices and updates another with the result. <c>c = alpha*op(a)*op(b) + beta*c</c>
        /// </summary>
        /// <param name="transposeA">How to transpose the <paramref name="a matrix.</param>
        /// <param name="transposeB">How to transpose the <paramref name="b matrix.</param>
        /// <param name="alpha">The value to scale <paramref name="a matrix.</param>
        /// <param name="a">The a matrix.</param>
        /// <param name="rowsA">The number of rows in the <paramref name="a matrix.</param>
        /// <param name="columnsA">The number of columns in the <paramref name="a matrix.</param>
        /// <param name="b">The b matrix</param>
        /// <param name="rowsB">The number of rows in the <paramref name="b matrix.</param>
        /// <param name="columnsB">The number of columns in the <paramref name="b matrix.</param>
        /// <param name="beta">The value to scale the <paramref name="c matrix.</param>
        /// <param name="c">The c matrix.</param>
        void MatrixMultiplyWithUpdate(Transpose transposeA, Transpose transposeB, T alpha, T[] a, int rowsA,
            int columnsA, T[] b, int rowsB, int columnsB, T beta, T[] c);

        /// <summary>
        ///     Computes the LUP factorization of A. P*A = L*U.
        /// </summary>
        /// <param name="data">
        ///     An <paramref name="order by 
        ///     
        ///     <paramref
        ///         name="order matrix. The matrix is overwritten with the
        /// the LU factorization on exit. The lower triangular factor L is stored in under the diagonal of 
        ///     
        ///     <paramref
        ///         name="data (the diagonal is always 1.0
        /// for the L factor). The upper triangular factor U is stored on and above the diagonal of 
        ///     
        ///     <paramref name="data.
        /// 
        /// </param>
        /// <param name="order">The order of the square matrix <paramref name="data.</param>
        /// <param name="ipiv">On exit, it contains the pivot indices. The size of the array must be <paramref name="order.</param>
        /// <remarks>This is equivalent to the GETRF LAPACK routine.</remarks>
        void LUFactor(T[] data, int order, int[] ipiv);


        /// <summary>
        ///     Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The factored A matrix.</param>
        /// <param name="order">The order of the square matrix <paramref name="a.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <remarks>This is equivalent to the GETRS LAPACK routine.</remarks>
        void LUSolveFactored(int columnsOfB, T[] a, int order, int[] ipiv, T[] b);

        /// <summary>
        ///     Computes the Cholesky factorization of A.
        /// </summary>
        /// <param name="a">
        ///     On entry, a square, positive definite matrix. On exit, the matrix is overwritten with the
        ///     the Cholesky factorization.
        /// </param>
        /// <param name="order">The number of rows or columns in the matrix.</param>
        /// <remarks>This is equivalent to the POTRF LAPACK routine.</remarks>
        void CholeskyFactor(T[] a, int order);


        /// <summary>
        ///     Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRS LAPACK routine.</remarks>
        void CholeskySolveFactored(T[] a, int orderA, T[] b, int columnsB);

        /// <summary>
        ///     Computes the full QR factorization of A.
        /// </summary>
        /// <param name="a">
        ///     On entry, it is the M by N A matrix to factor. On exit,
        ///     it is overwritten with the R matrix of the QR factorization.
        /// </param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="q">
        ///     On exit, A M by M matrix that holds the Q matrix of the
        ///     QR factorization.
        /// </param>
        /// <param name="tau">
        ///     A min(m,n) vector. On exit, contains additional information
        ///     to be used by the QR solve routine.
        /// </param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        void QRFactor(T[] a, int rowsA, int columnsA, T[] q, T[] tau);

        /// <summary>
        ///     Computes the thin QR factorization of A where M &gt; N.
        /// </summary>
        /// <param name="a">
        ///     On entry, it is the M by N A matrix to factor. On exit,
        ///     it is overwritten with the Q matrix of the QR factorization.
        /// </param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="r">
        ///     On exit, A N by N matrix that holds the R matrix of the
        ///     QR factorization.
        /// </param>
        /// <param name="tau">
        ///     A min(m,n) vector. On exit, contains additional information
        ///     to be used by the QR solve routine.
        /// </param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        void ThinQRFactor(T[] a, int rowsA, int columnsA, T[] r, T[] tau);


        /// <summary>
        ///     Solves A*X=B for X using a previously QR factored matrix.
        /// </summary>
        /// <param name="q">
        ///     The Q matrix obtained by QR factor. This is only used for the managed provider and can be
        ///     <c>null</c> for the native provider. The native provider uses the Q portion stored in the R matrix.
        /// </param>
        /// <param name="r">
        ///     The R matrix obtained by calling <seecreQRFactor( T[], int, int, T[], T[]). </param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="tau">
        ///     Contains additional information on Q. Only used for the native solver
        ///     and can be <c>null</c> for the managed provider.
        /// </param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        /// <remarks>Rows must be greater or equal to columns.</remarks>
        /// <param name="method">The type of QR factorization to perform. <seealsocreQRMethod</param>
        void QRSolveFactored(T[] q, T[] r, int rowsA, int columnsA, T[] tau, T[] b, int columnsB, T[] x,
            QRMethod method = QRMethod.Full);
    }
}