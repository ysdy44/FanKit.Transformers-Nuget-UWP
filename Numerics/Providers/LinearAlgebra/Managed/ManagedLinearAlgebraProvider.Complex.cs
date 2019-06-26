// <copyright file="ManagedLinearAlgebraProvider.Complex.cs" company="Math.NET">
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

using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.Threading;
using System;
using System.Numerics;

namespace MathNet.Numerics.Providers.LinearAlgebra.Managed
{
    /// <summary>
    ///     The managed linear algebra provider.
    /// </summary>
    internal partial class ManagedLinearAlgebraProvider
    {
        /// <summary>
        ///     Scales an array. Can be used to scale a vector and a matrix.
        /// </summary>
        /// <param name="alpha">The scalar.</param>
        /// <param name="x">The values to scale.</param>
        /// <param name="result">This result of the scaling.</param>
        /// <remarks>This is similar to the SCAL BLAS routine.</remarks>
        public virtual void ScaleArray(Complex alpha, Complex[] x, Complex[] result)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));

            if (alpha.IsZero())
                Array.Clear(result, 0, result.Length);
            else if (alpha.IsOne())
                x.Copy(result);
            else
                for (var i = 0; i < result.Length; i++)
                    result[i] = alpha * x[i];
        }

        /// <summary>
        ///     Computes the dot product of x and y.
        /// </summary>
        /// <param name="x">The vector x.</param>
        /// <param name="y">The vector y.</param>
        /// <returns>The dot product of x and y.</returns>
        /// <remarks>This is equivalent to the DOT BLAS routine.</remarks>
        public virtual Complex DotProduct(Complex[] x, Complex[] y)
        {
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x == null) throw new ArgumentNullException(nameof(x));

            if (y.Length != x.Length)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            var dot = Complex.Zero;
            for (var index = 0; index < y.Length; index++) dot += y[index] * x[index];

            return dot;
        }

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
        public virtual void AddArrays(Complex[] x, Complex[] y, Complex[] result)
        {
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x == null) throw new ArgumentNullException(nameof(x));

            if (result == null) throw new ArgumentNullException(nameof(result));

            if (y.Length != x.Length || y.Length != result.Length)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            for (var i = 0; i < result.Length; i++) result[i] = x[i] + y[i];
        }

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
        public virtual void SubtractArrays(Complex[] x, Complex[] y, Complex[] result)
        {
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x == null) throw new ArgumentNullException(nameof(x));

            if (result == null) throw new ArgumentNullException(nameof(result));

            if (y.Length != x.Length || y.Length != result.Length)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            for (var i = 0; i < result.Length; i++) result[i] = x[i] - y[i];
        }

        /// <summary>
        ///     Does a point wise multiplication of two arrays <c>z = x * y</c>. This can be used
        ///     to multiple elements of vectors or matrices.
        /// </summary>
        /// <param name="x">The array x.</param>
        /// <param name="y">The array y.</param>
        /// <param name="result">The result of the point wise multiplication.</param>
        /// <remarks>
        ///     There is no equivalent BLAS routine, but many libraries
        ///     provide optimized (parallel and/or vectorized) versions of this
        ///     routine.
        /// </remarks>
        public virtual void PointWiseMultiplyArrays(Complex[] x, Complex[] y, Complex[] result)
        {
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x == null) throw new ArgumentNullException(nameof(x));

            if (result == null) throw new ArgumentNullException(nameof(result));

            if (y.Length != x.Length || y.Length != result.Length)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            for (var i = 0; i < result.Length; i++) result[i] = x[i] * y[i];
        }

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
        public virtual void PointWiseDivideArrays(Complex[] x, Complex[] y, Complex[] result)
        {
            if (y == null) throw new ArgumentNullException(nameof(y));

            if (x == null) throw new ArgumentNullException(nameof(x));

            if (result == null) throw new ArgumentNullException(nameof(result));

            if (y.Length != x.Length || y.Length != result.Length)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength);
            }

            CommonParallel.For(0, y.Length, 4096, (a, b) =>
            {
                for (var i = a; i < b; i++) result[i] = x[i] / y[i];
            });
        }

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
        public virtual void MatrixMultiply(Complex[] x, int rowsX, int columnsX, Complex[] y, int rowsY, int columnsY,
            Complex[] result)
        {
            if (x == null) throw new ArgumentNullException(nameof(x));

            if (y == null) throw new ArgumentNullException(nameof(y));

            if (result == null) throw new ArgumentNullException(nameof(result));

            if (columnsX != rowsY)
                throw new ArgumentOutOfRangeException(string.Format("columnsA ({0}) != rowsB ({1})", columnsX, rowsY));

            if (rowsX * columnsX != x.Length)
                throw new ArgumentOutOfRangeException(string.Format("rowsA ({0}) * columnsA ({1}) != a.Length ({2})",
                    rowsX, columnsX, x.Length));

            if (rowsY * columnsY != y.Length)
                throw new ArgumentOutOfRangeException(string.Format("rowsB ({0}) * columnsB ({1}) != b.Length ({2})",
                    rowsY, columnsY, y.Length));

            if (rowsX * columnsY != result.Length)
                throw new ArgumentOutOfRangeException(string.Format("rowsA ({0}) * columnsB ({1}) != c.Length ({2})",
                    rowsX, columnsY, result.Length));

            // handle degenerate cases
            Array.Clear(result, 0, result.Length);

            // Extract column arrays
            var columnDataB = new Complex[columnsY][];
            for (var i = 0; i < columnDataB.Length; i++)
            {
                var column = new Complex[rowsY];
                GetColumn(Transpose.DontTranspose, i, rowsY, columnsY, y, column);
                columnDataB[i] = column;
            }

            var shouldNotParallelize = rowsX + columnsY + columnsX < Control.ParallelizeOrder ||
                                       Control.MaxDegreeOfParallelism < 2;
            if (shouldNotParallelize)
            {
                var row = new Complex[columnsX];
                for (var i = 0; i < rowsX; i++)
                {
                    GetRow(Transpose.DontTranspose, i, rowsX, columnsX, x, row);
                    for (var j = 0; j < columnsY; j++)
                    {
                        var col = columnDataB[j];
                        var sum = Complex.Zero;
                        for (var ii = 0; ii < row.Length; ii++) sum += row[ii] * col[ii];

                        result[j * rowsX + i] += Complex.One * sum;
                    }
                }
            }
            else
            {
                CommonParallel.For(0, rowsX, 1, (u, v) =>
                {
                    var row = new Complex[columnsX];
                    for (var i = u; i < v; i++)
                    {
                        GetRow(Transpose.DontTranspose, i, rowsX, columnsX, x, row);
                        for (var j = 0; j < columnsY; j++)
                        {
                            var column = columnDataB[j];
                            var sum = Complex.Zero;
                            for (var ii = 0; ii < row.Length; ii++) sum += row[ii] * column[ii];

                            result[j * rowsX + i] += Complex.One * sum;
                        }
                    }
                });
            }
        }

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
        public virtual void MatrixMultiplyWithUpdate(Transpose transposeA, Transpose transposeB, Complex alpha,
            Complex[] a, int rowsA, int columnsA, Complex[] b, int rowsB, int columnsB, Complex beta, Complex[] c)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            if (b == null) throw new ArgumentNullException(nameof(b));

            if (c == null) throw new ArgumentNullException(nameof(c));

            if (transposeA != Transpose.DontTranspose)
            {
                var swap = rowsA;
                rowsA = columnsA;
                columnsA = swap;
            }

            if (transposeB != Transpose.DontTranspose)
            {
                var swap = rowsB;
                rowsB = columnsB;
                columnsB = swap;
            }

            if (columnsA != rowsB)
                throw new ArgumentOutOfRangeException(string.Format("columnsA ({0}) != rowsB ({1})", columnsA, rowsB));

            if (rowsA * columnsA != a.Length)
                throw new ArgumentOutOfRangeException(string.Format("rowsA ({0}) * columnsA ({1}) != a.Length ({2})",
                    rowsA, columnsA, a.Length));

            if (rowsB * columnsB != b.Length)
                throw new ArgumentOutOfRangeException(string.Format("rowsB ({0}) * columnsB ({1}) != b.Length ({2})",
                    rowsB, columnsB, b.Length));

            if (rowsA * columnsB != c.Length)
                throw new ArgumentOutOfRangeException(string.Format("rowsA ({0}) * columnsB ({1}) != c.Length ({2})",
                    rowsA, columnsB, c.Length));

            // handle degenerate cases
            if (beta == Complex.Zero)
                Array.Clear(c, 0, c.Length);
            else if (beta != Complex.One) ScaleArray(beta, c, c);

            if (alpha == Complex.Zero) return;

            // Extract column arrays
            var columnDataB = new Complex[columnsB][];
            for (var i = 0; i < columnDataB.Length; i++)
            {
                var column = new Complex[rowsB];
                GetColumn(transposeB, i, rowsB, columnsB, b, column);
                columnDataB[i] = column;
            }

            var shouldNotParallelize = rowsA + columnsB + columnsA < Control.ParallelizeOrder ||
                                       Control.MaxDegreeOfParallelism < 2;
            if (shouldNotParallelize)
            {
                var row = new Complex[columnsA];
                for (var i = 0; i < rowsA; i++)
                {
                    GetRow(transposeA, i, rowsA, columnsA, a, row);
                    for (var j = 0; j < columnsB; j++)
                    {
                        var col = columnDataB[j];
                        var sum = Complex.Zero;
                        for (var ii = 0; ii < row.Length; ii++) sum += row[ii] * col[ii];

                        c[j * rowsA + i] += alpha * sum;
                    }
                }
            }
            else
            {
                CommonParallel.For(0, rowsA, 1, (u, v) =>
                {
                    var row = new Complex[columnsA];
                    for (var i = u; i < v; i++)
                    {
                        GetRow(transposeA, i, rowsA, columnsA, a, row);
                        for (var j = 0; j < columnsB; j++)
                        {
                            var column = columnDataB[j];
                            var sum = Complex.Zero;
                            for (var ii = 0; ii < row.Length; ii++) sum += row[ii] * column[ii];

                            c[j * rowsA + i] += alpha * sum;
                        }
                    }
                });
            }
        }

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
        public virtual void LUFactor(Complex[] data, int order, int[] ipiv)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (ipiv == null) throw new ArgumentNullException(nameof(ipiv));

            if (data.Length != order * order)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(data));
            }

            if (ipiv.Length != order)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(ipiv));
            }

            // Initialize the pivot matrix to the identity permutation.
            for (var i = 0; i < order; i++) ipiv[i] = i;

            var vecLUcolj = new Complex[order];

            // Outer loop.
            for (var j = 0; j < order; j++)
            {
                var indexj = j * order;
                var indexjj = indexj + j;

                // Make a copy of the j-th column to localize references.
                for (var i = 0; i < order; i++) vecLUcolj[i] = data[indexj + i];

                // Apply previous transformations.
                for (var i = 0; i < order; i++)
                {
                    // Most of the time is spent in the following dot product.
                    var kmax = Math.Min(i, j);
                    var s = Complex.Zero;
                    for (var k = 0; k < kmax; k++) s += data[k * order + i] * vecLUcolj[k];

                    data[indexj + i] = vecLUcolj[i] -= s;
                }

                // Find pivot and exchange if necessary.
                var p = j;
                for (var i = j + 1; i < order; i++)
                    if (vecLUcolj[i].Magnitude > vecLUcolj[p].Magnitude)
                        p = i;

                if (p != j)
                {
                    for (var k = 0; k < order; k++)
                    {
                        var indexk = k * order;
                        var indexkp = indexk + p;
                        var indexkj = indexk + j;
                        var temp = data[indexkp];
                        data[indexkp] = data[indexkj];
                        data[indexkj] = temp;
                    }

                    ipiv[j] = p;
                }

                // Compute multipliers.
                if ((j < order) & (data[indexjj] != 0.0))
                    for (var i = j + 1; i < order; i++)
                        data[indexj + i] /= data[indexjj];
            }
        }

        /// <summary>
        ///     Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="columnsOfB">The number of columns of B.</param>
        /// <param name="a">The factored A matrix.</param>
        /// <param name="order">The order of the square matrix <paramref name="a.</param>
        /// <param name="ipiv">The pivot indices of <paramref name="a.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <remarks>This is equivalent to the GETRS LAPACK routine.</remarks>
        public virtual void LUSolveFactored(int columnsOfB, Complex[] a, int order, int[] ipiv, Complex[] b)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            if (ipiv == null) throw new ArgumentNullException(nameof(ipiv));

            if (b == null) throw new ArgumentNullException(nameof(b));

            if (a.Length != order * order)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(a));
            }

            if (ipiv.Length != order)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(ipiv));
            }

            if (b.Length != order * columnsOfB)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                //throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            // Compute the column vector  P*B
            for (var i = 0; i < ipiv.Length; i++)
            {
                if (ipiv[i] == i) continue;

                var p = ipiv[i];
                for (var j = 0; j < columnsOfB; j++)
                {
                    var indexk = j * order;
                    var indexkp = indexk + p;
                    var indexkj = indexk + i;
                    var temp = b[indexkp];
                    b[indexkp] = b[indexkj];
                    b[indexkj] = temp;
                }
            }

            // Solve L*Y = P*B
            for (var k = 0; k < order; k++)
            {
                var korder = k * order;
                for (var i = k + 1; i < order; i++)
                for (var j = 0; j < columnsOfB; j++)
                {
                    var index = j * order;
                    b[i + index] -= b[k + index] * a[i + korder];
                }
            }

            // Solve U*X = Y;
            for (var k = order - 1; k >= 0; k--)
            {
                var korder = k + k * order;
                for (var j = 0; j < columnsOfB; j++) b[k + j * order] /= a[korder];

                korder = k * order;
                for (var i = 0; i < k; i++)
                for (var j = 0; j < columnsOfB; j++)
                {
                    var index = j * order;
                    b[i + index] -= b[k + index] * a[i + korder];
                }
            }
        }

        /// <summary>
        ///     Computes the Cholesky factorization of A.
        /// </summary>
        /// <param name="a">
        ///     On entry, a square, positive definite matrix. On exit, the matrix is overwritten with the
        ///     the Cholesky factorization.
        /// </param>
        /// <param name="order">The number of rows or columns in the matrix.</param>
        /// <remarks>This is equivalent to the POTRF LAPACK routine.</remarks>
        public virtual void CholeskyFactor(Complex[] a, int order)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            var tmpColumn = new Complex[order];

            // Main loop - along the diagonal
            for (var ij = 0; ij < order; ij++)
            {
                // "Pivot" element
                var tmpVal = a[ij * order + ij];

                if (tmpVal.Real > 0.0)
                {
                    tmpVal = tmpVal.SquareRoot();
                    a[ij * order + ij] = tmpVal;
                    tmpColumn[ij] = tmpVal;

                    // Calculate multipliers and copy to local column
                    // Current column, below the diagonal
                    for (var i = ij + 1; i < order; i++)
                    {
                        a[ij * order + i] /= tmpVal;
                        tmpColumn[i] = a[ij * order + i];
                    }

                    // Remaining columns, below the diagonal
                    DoCholeskyStep(a, order, ij + 1, order, tmpColumn, Control.MaxDegreeOfParallelism);
                }

                for (var i = ij + 1; i < order; i++) a[i * order + ij] = 0.0;
            }
        }

        /// <summary>
        ///     Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns in the B matrix.</param>
        /// <remarks>This is equivalent to the POTRS LAPACK routine.</remarks>
        public virtual void CholeskySolveFactored(Complex[] a, int orderA, Complex[] b, int columnsB)
        {
            if (a == null) throw new ArgumentNullException(nameof(a));

            if (b == null) throw new ArgumentNullException(nameof(b));

            if (b.Length != orderA * columnsB)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (ReferenceEquals(a, b))
            {
                //throw new ArgumentException(Resources.ArgumentReferenceDifferent);
            }

            CommonParallel.For(0, columnsB, (u, v) =>
            {
                for (var i = u; i < v; i++) DoCholeskySolve(a, orderA, b, i);
            });
        }

        /// <summary>
        ///     Computes the QR factorization of A.
        /// </summary>
        /// <param name="r">
        ///     On entry, it is the M by N A matrix to factor. On exit,
        ///     it is overwritten with the R matrix of the QR factorization.
        /// </param>
        /// <param name="rowsR">The number of rows in the A matrix.</param>
        /// <param name="columnsR">The number of columns in the A matrix.</param>
        /// <param name="q">
        ///     On exit, A M by M matrix that holds the Q matrix of the
        ///     QR factorization.
        /// </param>
        /// <param name="tau">
        ///     A min(m,n) vector. On exit, contains additional information
        ///     to be used by the QR solve routine.
        /// </param>
        /// <remarks>This is similar to the GEQRF and ORGQR LAPACK routines.</remarks>
        public virtual void QRFactor(Complex[] r, int rowsR, int columnsR, Complex[] q, Complex[] tau)
        {
            if (r == null) throw new ArgumentNullException(nameof(r));

            if (q == null) throw new ArgumentNullException(nameof(q));

            if (r.Length != rowsR * columnsR)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, "rowsR * columnsR"), nameof(r));
            }

            if (tau.Length < Math.Min(rowsR, columnsR))
            {
                //throw new ArgumentException(string.Format(Resources.ArrayTooSmall, "min(m,n)"), nameof(tau));
            }

            if (q.Length != rowsR * rowsR)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, "rowsR * rowsR"), nameof(q));
            }

            var work = columnsR > rowsR ? new Complex[rowsR * rowsR] : new Complex[rowsR * columnsR];

            CommonParallel.For(0, rowsR, (a, b) =>
            {
                for (var i = a; i < b; i++) q[i * rowsR + i] = Complex.One;
            });

            var minmn = Math.Min(rowsR, columnsR);
            for (var i = 0; i < minmn; i++)
            {
                GenerateColumn(work, r, rowsR, i, i);
                ComputeQR(work, i, r, i, rowsR, i + 1, columnsR, Control.MaxDegreeOfParallelism);
            }

            for (var i = minmn - 1; i >= 0; i--)
                ComputeQR(work, i, q, i, rowsR, i, rowsR, Control.MaxDegreeOfParallelism);
        }

        /// <summary>
        ///     Computes the QR factorization of A.
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
        public virtual void ThinQRFactor(Complex[] a, int rowsA, int columnsA, Complex[] r, Complex[] tau)
        {
            if (r == null) throw new ArgumentNullException(nameof(r));

            if (a == null) throw new ArgumentNullException(nameof(a));

            if (a.Length != rowsA * columnsA)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, "rowsR * columnsR"), nameof(a));
            }

            if (tau.Length < Math.Min(rowsA, columnsA))
            {
                //throw new ArgumentException(string.Format(Resources.ArrayTooSmall, "min(m,n)"), nameof(tau));
            }

            if (r.Length != columnsA * columnsA)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, "columnsA * columnsA"), nameof(r));
            }

            var work = new Complex[rowsA * columnsA];

            var minmn = Math.Min(rowsA, columnsA);
            for (var i = 0; i < minmn; i++)
            {
                GenerateColumn(work, a, rowsA, i, i);
                ComputeQR(work, i, a, i, rowsA, i + 1, columnsA, Control.MaxDegreeOfParallelism);
            }

            //copy R
            for (var j = 0; j < columnsA; j++)
            {
                var rIndex = j * columnsA;
                var aIndex = j * rowsA;
                for (var i = 0; i < columnsA; i++) r[rIndex + i] = a[aIndex + i];
            }

            //clear A and set diagonals to 1
            Array.Clear(a, 0, a.Length);
            for (var i = 0; i < columnsA; i++) a[i * rowsA + i] = Complex.One;

            for (var i = minmn - 1; i >= 0; i--)
                ComputeQR(work, i, a, i, rowsA, i, columnsA, Control.MaxDegreeOfParallelism);
        }


        /// <summary>
        ///     Solves A*X=B for X using a previously QR factored matrix.
        /// </summary>
        /// <param name="q">
        ///     The Q matrix obtained by calling <seecreQRFactor( Complex[], int, int, Complex[], Complex[]).</param>
        /// <param name="r">
        ///     The R matrix obtained by calling <seecreQRFactor( Complex[], int, int, Complex[], Complex[]). </param>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="tau">
        ///     Contains additional information on Q. Only used for the native solver
        ///     and can be <c>null</c> for the managed provider.
        /// </param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        /// <param name="method">The type of QR factorization to perform. <seealsocreQRMethod</param>
        /// <remarks>Rows must be greater or equal to columns.</remarks>
        public virtual void QRSolveFactored(Complex[] q, Complex[] r, int rowsA, int columnsA, Complex[] tau,
            Complex[] b, int columnsB, Complex[] x, QRMethod method = QRMethod.Full)
        {
            if (r == null) throw new ArgumentNullException(nameof(r));

            if (q == null) throw new ArgumentNullException(nameof(q));

            if (b == null) throw new ArgumentNullException(nameof(q));

            if (x == null) throw new ArgumentNullException(nameof(q));

            if (rowsA < columnsA)
            {
                //throw new ArgumentException(Resources.RowsLessThanColumns);
            }

            int rowsQ, columnsQ, rowsR, columnsR;
            if (method == QRMethod.Full)
            {
                rowsQ = columnsQ = rowsR = rowsA;
                columnsR = columnsA;
            }
            else
            {
                rowsQ = rowsA;
                columnsQ = rowsR = columnsR = columnsA;
            }

            if (r.Length != rowsR * columnsR)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, rowsR*columnsR), nameof(r));
            }

            if (q.Length != rowsQ * columnsQ)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, rowsQ*columnsQ), nameof(q));
            }

            if (b.Length != rowsA * columnsB)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, rowsA*columnsB), nameof(b));
            }

            if (x.Length != columnsA * columnsB)
            {
                //throw new ArgumentException(string.Format(Resources.ArgumentArrayWrongLength, columnsA*columnsB), nameof(x));
            }

            var sol = new Complex[b.Length];

            // Copy B matrix to "sol", so B data will not be changed
            Array.Copy(b, 0, sol, 0, b.Length);

            // Compute Y = transpose(Q)*B
            var column = new Complex[rowsA];
            for (var j = 0; j < columnsB; j++)
            {
                var jm = j * rowsA;
                Array.Copy(sol, jm, column, 0, rowsA);
                CommonParallel.For(0, columnsA, (u, v) =>
                {
                    for (var i = u; i < v; i++)
                    {
                        var im = i * rowsA;

                        var sum = Complex.Zero;
                        for (var k = 0; k < rowsA; k++) sum += q[im + k].Conjugate() * column[k];

                        sol[jm + i] = sum;
                    }
                });
            }

            // Solve R*X = Y;
            for (var k = columnsA - 1; k >= 0; k--)
            {
                var km = k * rowsR;
                for (var j = 0; j < columnsB; j++) sol[j * rowsA + k] /= r[km + k];

                for (var i = 0; i < k; i++)
                for (var j = 0; j < columnsB; j++)
                {
                    var jm = j * rowsA;
                    sol[jm + i] -= sol[jm + k] * r[km + i];
                }
            }

            // Fill result matrix
            for (var col = 0; col < columnsB; col++) Array.Copy(sol, col * rowsA, x, col * columnsA, columnsR);
        }
        

        /// <summary>
        ///     Solves A*X=B for X using a previously SVD decomposed matrix.
        /// </summary>
        /// <param name="rowsA">The number of rows in the A matrix.</param>
        /// <param name="columnsA">The number of columns in the A matrix.</param>
        /// <param name="s">
        ///     The s values returned by
        ///     <seecreSingularValueDecomposition( bool, Complex[], int, int, Complex[], Complex[], Complex[]).</param>
        /// <param name="u">
        ///     The left singular vectors returned by
        ///     <seecreSingularValueDecomposition( bool, Complex[], int, int, Complex[], Complex[], Complex[]).</param>
        /// <param name="vt">
        ///     The right singular  vectors returned by
        ///     <seecreSingularValueDecomposition( bool, Complex[], int, int, Complex[], Complex[], Complex[]).</param>
        /// <param name="b">The B matrix.</param>
        /// <param name="columnsB">The number of columns of B.</param>
        /// <param name="x">On exit, the solution matrix.</param>
        public virtual void SvdSolveFactored(int rowsA, int columnsA, Complex[] s, Complex[] u, Complex[] vt,
            Complex[] b, int columnsB, Complex[] x)
        {
            if (s == null) throw new ArgumentNullException(nameof(s));

            if (u == null) throw new ArgumentNullException(nameof(u));

            if (vt == null) throw new ArgumentNullException(nameof(vt));

            if (b == null) throw new ArgumentNullException(nameof(b));

            if (x == null) throw new ArgumentNullException(nameof(x));

            if (u.Length != rowsA * rowsA)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(u));
            }

            if (vt.Length != columnsA * columnsA)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(vt));
            }

            if (s.Length != Math.Min(rowsA, columnsA))
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(s));
            }

            if (b.Length != rowsA * columnsB)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            if (x.Length != columnsA * columnsB)
            {
                //throw new ArgumentException(Resources.ArgumentArraysSameLength, nameof(b));
            }

            var mn = Math.Min(rowsA, columnsA);
            var tmp = new Complex[columnsA];

            for (var k = 0; k < columnsB; k++)
            {
                for (var j = 0; j < columnsA; j++)
                {
                    var value = Complex.Zero;
                    if (j < mn)
                    {
                        for (var i = 0; i < rowsA; i++) value += u[j * rowsA + i].Conjugate() * b[k * rowsA + i];

                        value /= s[j];
                    }

                    tmp[j] = value;
                }

                for (var j = 0; j < columnsA; j++)
                {
                    var value = Complex.Zero;
                    for (var i = 0; i < columnsA; i++) value += vt[j * columnsA + i].Conjugate() * tmp[i];

                    x[k * columnsA + j] = value;
                }
            }
        }
        
        /// <summary>
        ///     Calculate Cholesky step
        /// </summary>
        /// <param name="data">Factor matrix</param>
        /// <param name="rowDim">Number of rows</param>
        /// <param name="firstCol">Column start</param>
        /// <param name="colLimit">Total columns</param>
        /// <param name="multipliers">Multipliers calculated previously</param>
        /// <param name="availableCores">Number of available processors</param>
        private static void DoCholeskyStep(Complex[] data, int rowDim, int firstCol, int colLimit,
            Complex[] multipliers, int availableCores)
        {
            var tmpColCount = colLimit - firstCol;

            if (availableCores > 1 && tmpColCount > Control.ParallelizeElements)
            {
                var tmpSplit = firstCol + tmpColCount / 3;
                var tmpCores = availableCores / 2;

                CommonParallel.Invoke(
                    () => DoCholeskyStep(data, rowDim, firstCol, tmpSplit, multipliers, tmpCores),
                    () => DoCholeskyStep(data, rowDim, tmpSplit, colLimit, multipliers, tmpCores));
            }
            else
            {
                for (var j = firstCol; j < colLimit; j++)
                {
                    var tmpVal = multipliers[j];
                    for (var i = j; i < rowDim; i++) data[j * rowDim + i] -= multipliers[i] * tmpVal.Conjugate();
                }
            }
        }

        /// <summary>
        ///     Solves A*X=B for X using a previously factored A matrix.
        /// </summary>
        /// <param name="a">The square, positive definite matrix A. Has to be different than <paramref name="b.</param>
        /// <param name="orderA">The number of rows and columns in A.</param>
        /// <param name="b">On entry the B matrix; on exit the X matrix.</param>
        /// <param name="index">The column to solve for.</param>
        private static void DoCholeskySolve(Complex[] a, int orderA, Complex[] b, int index)
        {
            var cindex = index * orderA;

            // Solve L*Y = B;
            Complex sum;
            for (var i = 0; i < orderA; i++)
            {
                sum = b[cindex + i];
                for (var k = i - 1; k >= 0; k--) sum -= a[k * orderA + i] * b[cindex + k];

                b[cindex + i] = sum / a[i * orderA + i];
            }

            // Solve L'*X = Y;
            for (var i = orderA - 1; i >= 0; i--)
            {
                sum = b[cindex + i];
                var iindex = i * orderA;
                for (var k = i + 1; k < orderA; k++) sum -= a[iindex + k].Conjugate() * b[cindex + k];

                b[cindex + i] = sum / a[iindex + i];
            }
        }

        /// <summary>
        ///     Assumes that <paramref name="numRows and <paramref name="numCols have already been transposed.
        /// 
        /// 
        /// </summary>
        protected static void GetRow(Transpose transpose, int rowindx, int numRows, int numCols, Complex[] matrix,
            Complex[] row)
        {
            if (transpose == Transpose.DontTranspose)
            {
                for (var i = 0; i < numCols; i++) row[i] = matrix[i * numRows + rowindx];
            }
            else if (transpose == Transpose.ConjugateTranspose)
            {
                var offset = rowindx * numCols;
                for (var i = 0; i < row.Length; i++) row[i] = matrix[i + offset].Conjugate();
            }
            else
            {
                Array.Copy(matrix, rowindx * numCols, row, 0, numCols);
            }
        }

        /// <summary>
        ///     Assumes that <paramref name="numRows and <paramref name="numCols have already been transposed.
        /// 
        /// 
        /// </summary>
        protected static void GetColumn(Transpose transpose, int colindx, int numRows, int numCols, Complex[] matrix,
            Complex[] column)
        {
            if (transpose == Transpose.DontTranspose)
                Array.Copy(matrix, colindx * numRows, column, 0, numRows);
            else if (transpose == Transpose.ConjugateTranspose)
                for (var i = 0; i < numRows; i++)
                    column[i] = matrix[i * numCols + colindx].Conjugate();
            else
                for (var i = 0; i < numRows; i++)
                    column[i] = matrix[i * numCols + colindx];
        }


        #region QR Factor Helper functions

        /// <summary>
        ///     Perform calculation of Q or R
        /// </summary>
        /// <param name="work">Work array</param>
        /// <param name="workIndex">Index of column in work array</param>
        /// <param name="a">Q or R matrices</param>
        /// <param name="rowStart">The first row in </param>
        /// <param name="rowCount">The last row</param>
        /// <param name="columnStart">The first column</param>
        /// <param name="columnCount">The last column</param>
        /// <param name="availableCores">Number of available CPUs</param>
        private static void ComputeQR(Complex[] work, int workIndex, Complex[] a, int rowStart, int rowCount,
            int columnStart, int columnCount, int availableCores)
        {
            if (rowStart > rowCount || columnStart > columnCount) return;

            var tmpColCount = columnCount - columnStart;

            if (availableCores > 1 && tmpColCount > 200)
            {
                var tmpSplit = columnStart + tmpColCount / 2;
                var tmpCores = availableCores / 2;

                CommonParallel.Invoke(
                    () => ComputeQR(work, workIndex, a, rowStart, rowCount, columnStart, tmpSplit, tmpCores),
                    () => ComputeQR(work, workIndex, a, rowStart, rowCount, tmpSplit, columnCount, tmpCores));
            }
            else
            {
                for (var j = columnStart; j < columnCount; j++)
                {
                    var scale = Complex.Zero;
                    for (var i = rowStart; i < rowCount; i++)
                        scale += work[workIndex * rowCount + i - rowStart] * a[j * rowCount + i];

                    for (var i = rowStart; i < rowCount; i++)
                        a[j * rowCount + i] -= work[workIndex * rowCount + i - rowStart].Conjugate() * scale;
                }
            }
        }

        /// <summary>
        ///     Generate column from initial matrix to work array
        /// </summary>
        /// <param name="work">Work array</param>
        /// <param name="a">Initial matrix</param>
        /// <param name="rowCount">The number of rows in matrix</param>
        /// <param name="row">The first row</param>
        /// <param name="column">Column index</param>
        private static void GenerateColumn(Complex[] work, Complex[] a, int rowCount, int row, int column)
        {
            var tmp = column * rowCount;
            var index = tmp + row;

            CommonParallel.For(row, rowCount, (u, v) =>
            {
                for (var i = u; i < v; i++)
                {
                    var iIndex = tmp + i;
                    work[iIndex - row] = a[iIndex];
                    a[iIndex] = Complex.Zero;
                }
            });

            var norm = Complex.Zero;
            for (var i = 0; i < rowCount - row; ++i)
            {
                var index1 = tmp + i;
                norm += work[index1].Magnitude * work[index1].Magnitude;
            }

            norm = norm.SquareRoot();
            if (row == rowCount - 1 || norm.Magnitude == 0)
            {
                a[index] = -work[tmp];
                work[tmp] = new Complex(2.0, 0).SquareRoot();
                return;
            }

            if (work[tmp].Magnitude != 0.0) norm = norm.Magnitude * (work[tmp] / work[tmp].Magnitude);

            a[index] = -norm;
            CommonParallel.For(0, rowCount - row, 4096, (u, v) =>
            {
                for (var i = u; i < v; i++) work[tmp + i] /= norm;
            });
            work[tmp] += 1.0;

            var s = (1.0 / work[tmp]).SquareRoot();
            CommonParallel.For(0, rowCount - row, 4096, (u, v) =>
            {
                for (var i = u; i < v; i++) work[tmp + i] = work[tmp + i].Conjugate() * s;
            });
        }

        #endregion
    }
}