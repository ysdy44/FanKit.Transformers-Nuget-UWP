// <copyright file="UserSvd.cs" company="Math.NET">
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

namespace FanKit.Transformers
{
    /// <summary>
    ///     <para>
    ///         A class which encapsulates the functionality of the singular value decomposition (SVD) for <seecreMatrix{ double}.</para>
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
    internal sealed partial class Svd8x8
    {
        /// <summary>
        ///     Gets the two norm of the <seecreMatrix{ double}.
        /// </summary>
        /// <returns>
        ///     The 2-norm of the <seecreMatrix{ double}.</returns>
        public double L2Norm => System.Math.Abs(S[0]);

        public Svd8x8(double[] s, double[] u, double[] vt)
        {
            S = s;
            U = u;
            VT = vt;

            const int rows = 8;
            const int columns = 8;
            double[] result = new double[8 * 8];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    if (i == j)
                        result.At8x8(i, i, S[i]);

            W = result;
        }

        /// <summary>
        ///     Gets the singular values (Σ) of matrix in ascending value.
        /// </summary>
        public double[] S { get; }

        /// <summary>
        ///     Gets the left singular vectors (U - m-by-m unitary matrix)
        /// </summary>
        public double[] U { get; }

        /// <summary>
        ///     Gets the transpose right singular vectors (transpose of V, an n-by-n unitary matrix)
        /// </summary>
        public double[] VT { get; }

        /// <summary>
        ///     Returns the singular values as a diagonal <seecreMatrix{ double}.
        /// </summary>
        /// <returns>
        ///     The singular values as a diagonal <seecreMatrix{ double}.</returns>
        public double[] W { get; }
    }

    internal static partial class MathNetNumericsExtensions
    {
        /// <summary>
        ///     Initializes a new instance of the
        ///     <seecreUserSvd class. This object will compute the
        ///         the singular value decomposition when the constructor is called and cache it's decomposition.
        /// 
        /// 
        /// </summary>
        /// <param name="matrix">The matrix to factor.</param>
        /// <param name="computeVectors">Compute the singular U and VT vectors or not.</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="matrix is 
        /// 
        /// <c>null</c>
        /// .
        /// </exception>
        public static Svd8x8 Create(double[] matrix8x8)
        {
            const bool computeVectors = true;

            int nm = System.Math.Min(8 + 1, 8);
            double[] matrixCopy = matrix8x8.Clone8x8();

            double[] s = new double[nm];
            double[] u = new double[8 * 8];
            double[] vt = new double[8 * 8];

            const int maxiter = 1000;
            double[] e = new double[8];
            double[] work = new double[8];

            int i, j;
            int l, lp1;
            double t;

            const int ncu = 8;

            // Reduce matrixCopy to bidiagonal form, storing the diagonal elements
            // In s and the super-diagonal elements in e.
            int nct = System.Math.Min(8 - 1, 8);
            int nrt = System.Math.Max(0, System.Math.Min(8 - 2, 8));
            int lu = System.Math.Max(nct, nrt);
            for (l = 0; l < lu; l++)
            {
                lp1 = l + 1;
                if (l < nct)
                {
                    // Compute the transformation for the l-th column and place the l-th diagonal in VectorS[l].
                    double xnorm = Dnrm2Column(matrixCopy, l, l);
                    s[l] = xnorm;
                    if (s[l] != 0.0)
                    {
                        if (matrixCopy.At8x8(l, l) != 0.0) s[l] = Dsign(s[l], matrixCopy.At8x8(l, l));

                        DscalColumn(matrixCopy, l, l, 1.0 / s[l]);
                        matrixCopy.At8x8(l, l, 1.0 + matrixCopy.At8x8(l, l));
                    }

                    s[l] = -s[l];
                }

                for (j = lp1; j < 8; j++)
                {
                    if (l < nct)
                        if (s[l] != 0.0)
                        {
                            // Apply the transformation.
                            t = -Ddot(matrixCopy, l, j, l) / matrixCopy.At8x8(l, l);
                            for (int ii = l; ii < 8; ii++)
                                matrixCopy.At8x8(ii, j, matrixCopy.At8x8(ii, j) + t * matrixCopy.At8x8(ii, l));
                        }

                    // Place the l-th row of matrixCopy into  e for the
                    // Subsequent calculation of the row transformation.
                    e[j] = matrixCopy.At8x8(l, j);
                }

                if (computeVectors && l < nct)
                    // Place the transformation in u for subsequent back multiplication.
                    for (i = l; i < 8; i++)
                        u.At8x8(i, l, matrixCopy.At8x8(i, l));

                if (l >= nrt) continue;

                // Compute the l-th row transformation and place the l-th super-diagonal in e(l).
                double enorm = Dnrm2Vector(e, lp1);
                e[l] = enorm;
                if (e[l] != 0.0)
                {
                    if (e[lp1] != 0.0) e[l] = Dsign(e[l], e[lp1]);

                    DscalVector(e, lp1, 1.0 / e[l]);
                    e[lp1] = 1.0 + e[lp1];
                }

                e[l] = -e[l];
                if (lp1 < 8 && e[l] != 0.0)
                {
                    // Apply the transformation.
                    for (i = lp1; i < 8; i++) work[i] = 0.0;

                    for (j = lp1; j < 8; j++)
                        for (int ii = lp1; ii < 8; ii++)
                            work[ii] += e[j] * matrixCopy.At8x8(ii, j);

                    for (j = lp1; j < 8; j++)
                    {
                        double ww = -e[j] / e[lp1];
                        for (int ii = lp1; ii < 8; ii++)
                            matrixCopy.At8x8(ii, j, matrixCopy.At8x8(ii, j) + ww * work[ii]);
                    }
                }

                if (computeVectors)
                    // Place the transformation in v for subsequent back multiplication.
                    for (i = lp1; i < 8; i++)
                        vt.At8x8(i, l, e[i]);
            }

            // Set up the final bidiagonal matrixCopy or order m.
            int m = System.Math.Min(8, 8 + 1);
            int nctp1 = nct + 1;
            int nrtp1 = nrt + 1;
            if (nct < 8) s[nctp1 - 1] = matrixCopy.At8x8(nctp1 - 1, nctp1 - 1);

            if (8 < m) s[m - 1] = 0.0;

            if (nrtp1 < m) e[nrtp1 - 1] = matrixCopy.At8x8(nrtp1 - 1, m - 1);

            e[m - 1] = 0.0;

            // If required, generate u.
            if (computeVectors)
            {
                for (j = nctp1 - 1; j < ncu; j++)
                {
                    for (i = 0; i < 8; i++) u.At8x8(i, j, 0.0);

                    u.At8x8(j, j, 1.0);
                }

                for (l = nct - 1; l >= 0; l--)
                    if (s[l] != 0.0)
                    {
                        for (j = l + 1; j < ncu; j++)
                        {
                            t = -Ddot(u, l, j, l) / u.At8x8(l, l);
                            for (int ii = l; ii < 8; ii++) u.At8x8(ii, j, u.At8x8(ii, j) + t * u.At8x8(ii, l));
                        }

                        DscalColumn(u, l, l, -1.0);
                        u.At8x8(l, l, 1.0 + u.At8x8(l, l));
                        for (i = 0; i < l; i++) u.At8x8(i, l, 0.0);
                    }
                    else
                    {
                        for (i = 0; i < 8; i++) u.At8x8(i, l, 0.0);

                        u.At8x8(l, l, 1.0);
                    }
            }

            // If it is required, generate v.
            if (computeVectors)
                for (l = 8 - 1; l >= 0; l--)
                {
                    lp1 = l + 1;
                    if (l < nrt)
                        if (e[l] != 0.0)
                            for (j = lp1; j < 8; j++)
                            {
                                t = -Ddot(vt, l, j, lp1) / vt.At8x8(lp1, l);
                                for (int ii = l; ii < 8; ii++)
                                    vt.At8x8(ii, j, vt.At8x8(ii, j) + t * vt.At8x8(ii, l));
                            }

                    for (i = 0; i < 8; i++) vt.At8x8(i, l, 0.0);

                    vt.At8x8(l, l, 1.0);
                }

            // Transform s and e so that they are  double .
            for (i = 0; i < m; i++)
            {
                double r;
                if (s[i] != 0.0)
                {
                    t = s[i];
                    r = s[i] / t;
                    s[i] = t;
                    if (i < m - 1) e[i] = e[i] / r;

                    if (computeVectors) DscalColumn(u, i, 0, r);
                }

                // Exit
                if (i == m - 1) break;

                if (e[i] != 0.0)
                {
                    t = e[i];
                    r = t / e[i];
                    e[i] = t;
                    s[i + 1] = s[i + 1] * r;
                    if (computeVectors) DscalColumn(vt, i + 1, 0, r);
                }
            }

            // Main iteration loop for the singular values.
            int mn = m;
            int iter = 0;

            while (m > 0)
            {
                // Quit if all the singular values have been found. If too many iterations have been performed,
                // throw exception that Convergence Failed
                if (iter >= maxiter)
                {
                    //throw new NonConvergenceException();
                }

                // This section of the program inspects for negligible elements in the s and e arrays. On
                // completion the variables case and l are set as follows.
                // Case = 1     if VectorS[m] and e[l-1] are negligible and l < m
                // Case = 2     if VectorS[l] is negligible and l < m
                // Case = 3     if e[l-1] is negligible, l < m, and VectorS[l, ..., VectorS[m] are not negligible (qr step).
                // Case = 4     if e[m-1] is negligible (convergence).
                double ztest;
                double test;
                for (l = m - 2; l >= 0; l--)
                {
                    test = System.Math.Abs(s[l]) + System.Math.Abs(s[l + 1]);
                    ztest = test + System.Math.Abs(e[l]);
                    if (ztest.AlmostEqualRelative(test, 15))
                    {
                        e[l] = 0.0;
                        break;
                    }
                }

                int kase;
                if (l == m - 2)
                {
                    kase = 4;
                }
                else
                {
                    int ls;
                    for (ls = m - 1; ls > l; ls--)
                    {
                        test = 0.0;
                        if (ls != m - 1) test = test + System.Math.Abs(e[ls]);

                        if (ls != l + 1) test = test + System.Math.Abs(e[ls - 1]);

                        ztest = test + System.Math.Abs(s[ls]);
                        if (ztest.AlmostEqualRelative(test, 15))
                        {
                            s[ls] = 0.0;
                            break;
                        }
                    }

                    if (ls == l)
                    {
                        kase = 3;
                    }
                    else if (ls == m - 1)
                    {
                        kase = 1;
                    }
                    else
                    {
                        kase = 2;
                        l = ls;
                    }
                }

                l = l + 1;

                // Perform the task indicated by case.
                int k;
                double f;
                double sn;
                double cs;
                switch (kase)
                {
                    // Deflate negligible VectorS[m].
                    case 1:
                        f = e[m - 2];
                        e[m - 2] = 0.0;
                        double t1;
                        for (int kk = l; kk < m - 1; kk++)
                        {
                            k = m - 2 - kk + l;
                            t1 = s[k];
                            Drotg(ref t1, ref f, out cs, out sn);
                            s[k] = t1;
                            if (k != l)
                            {
                                f = -sn * e[k - 1];
                                e[k - 1] = cs * e[k - 1];
                            }

                            if (computeVectors) Drot(vt, k, m - 1, cs, sn);
                        }

                        break;

                    // Split at negligible VectorS[l].
                    case 2:
                        f = e[l - 1];
                        e[l - 1] = 0.0;
                        for (k = l; k < m; k++)
                        {
                            t1 = s[k];
                            Drotg(ref t1, ref f, out cs, out sn);
                            s[k] = t1;
                            f = -sn * e[k];
                            e[k] = cs * e[k];
                            if (computeVectors) Drot(u, k, l - 1, cs, sn);
                        }

                        break;

                    // Perform one qr step.
                    case 3:
                        // Calculate the shift.
                        double scale = 0.0;
                        scale = System.Math.Max(scale, System.Math.Abs(s[m - 1]));
                        scale = System.Math.Max(scale, System.Math.Abs(s[m - 2]));
                        scale = System.Math.Max(scale, System.Math.Abs(e[m - 2]));
                        scale = System.Math.Max(scale, System.Math.Abs(s[l]));
                        scale = System.Math.Max(scale, System.Math.Abs(e[l]));
                        double sm = s[m - 1] / scale;
                        double smm1 = s[m - 2] / scale;
                        double emm1 = e[m - 2] / scale;
                        double sl = s[l] / scale;
                        double el = e[l] / scale;
                        double b = ((smm1 + sm) * (smm1 - sm) + emm1 * emm1) / 2.0;
                        double c = sm * emm1 * (sm * emm1);
                        double shift = 0.0;
                        if (b != 0.0 || c != 0.0)
                        {
                            shift = System.Math.Sqrt(b * b + c);
                            if (b < 0.0) shift = -shift;

                            shift = c / (b + shift);
                        }

                        f = (sl + sm) * (sl - sm) + shift;
                        double g = sl * el;

                        // Chase zeros.
                        for (k = l; k < m - 1; k++)
                        {
                            Drotg(ref f, ref g, out cs, out sn);
                            if (k != l) e[k - 1] = f;

                            f = cs * s[k] + sn * e[k];
                            e[k] = cs * e[k] - sn * s[k];
                            g = sn * s[k + 1];
                            s[k + 1] = cs * s[k + 1];
                            if (computeVectors) Drot(vt, k, k + 1, cs, sn);

                            Drotg(ref f, ref g, out cs, out sn);
                            s[k] = f;
                            f = cs * e[k] + sn * s[k + 1];
                            s[k + 1] = -sn * e[k] + cs * s[k + 1];
                            g = sn * e[k + 1];
                            e[k + 1] = cs * e[k + 1];
                            if (computeVectors && k < 8)
                                Drot(u, k, k + 1, cs, sn);
                        }

                        e[m - 2] = f;
                        iter = iter + 1;
                        break;

                    // Convergence.
                    case 4:
                        // Make the singular value  positive
                        if (s[l] < 0.0)
                        {
                            s[l] = -s[l];
                            if (computeVectors) DscalColumn(vt, l, 0, -1.0);
                        }

                        // Order the singular value.
                        while (l != mn - 1)
                        {
                            if (s[l] >= s[l + 1]) break;

                            t = s[l];
                            s[l] = s[l + 1];
                            s[l + 1] = t;
                            if (computeVectors && l < 8)
                                Dswap(vt, l, l + 1);

                            if (computeVectors && l < 8) Dswap(u, l, l + 1);

                            l = l + 1;
                        }

                        iter = 0;
                        m = m - 1;
                        break;
                }
            }

            if (computeVectors) vt = vt.Transpose8x8();

            // Adjust the size of s if rows < columns. We are using ported copy of linpack's svd code and it uses
            // a singular vector of length mRows+1 when mRows < mColumns. The last element is not used and needs to be removed.
            // we should port lapack's svd routine to remove this problem.
            /*
            if (8 < 8)
            {
                nm--;
                double[] tmp = (new double[nm]);
                for (i = 0; i < nm; i++) tmp[i] = s[i];

                s = tmp;
            }
             */

            return new Svd8x8(s, u, vt);
        }

        /// <summary>
        ///     Calculates absolute value of <paramref name="z1 multiplied on signum function of <paramref name="z2
        /// 
        /// 
        /// </summary>
        /// <param name="z1">Double value z1</param>
        /// <param name="z2">Double value z2</param>
        /// <returns>Result multiplication of signum function and absolute value</returns>
        private static double Dsign(double z1, double z2)
        {
            return System.Math.Abs(z1) * (z2 / System.Math.Abs(z2));
        }

        /// <summary>
        ///     Swap column  <paramref name="columnA1x8  and  <paramref name="columnB1x8
        /// 
        /// 
        /// </summary>
        /// <param name="a8x8">Source matrix</param>
        /// <param name="8">The number of rows in <paramref name="a8x8</param>
        /// <param name="columnA1x8">Column A index to swap</param>
        /// <param name="columnB1x8">Column B index to swap</param>
        private static void Dswap(double[] a8x8, int columnA1x8, int columnB1x8)
        {
            for (int i = 0; i < 8; i++)
            {
                double z = a8x8.At8x8(i, columnA1x8);
                a8x8.At8x8(i, columnA1x8, a8x8.At8x8(i, columnB1x8));
                a8x8.At8x8(i, columnB1x8, z);
            }
        }

        /// <summary>
        ///     Scale column <paramref name="columna1x8 by <paramref name="z starting from row <paramref name="rowStart
        /// 
        /// 
        /// </summary>
        /// <param name="a8x8">Source matrix</param>
        /// <param name="8">The number of rows in <paramref name="a8x8 </param>
        /// <param name="columna1x8">Column to scale</param>
        /// <param name="rowStart">Row to scale from</param>
        /// <param name="z">Scale value</param>
        private static void DscalColumn(double[] a8x8, int columna1x8, int rowStart, double z)
        {
            for (int i = rowStart; i < 8; i++) a8x8.At8x8(i, columna1x8, a8x8.At8x8(i, columna1x8) * z);
        }

        /// <summary>
        ///     Scale vector <paramref name="a1x8 by <paramref name="z starting from index <paramref name="start
        /// 
        /// 
        /// </summary>
        /// <param name="a1x8">Source vector</param>
        /// <param name="start">Row to scale from</param>
        /// <param name="z">Scale value</param>
        private static void DscalVector(double[] a1x8, int start, double z)
        {
            for (int i = start; i < a1x8.Length; i++) a1x8[i] = a1x8[i] * z;
        }

        /// <summary>
        ///     Given the Cartesian coordinates (da, db) of a point p, these function return the parameters da, db, c, and s
        ///     associated with the Givens rotation that zeros the y-coordinate of the point.
        /// </summary>
        /// <param name="da">
        ///     Provides the x-coordinate of the point p. On exit contains the parameter r associated with the Givens
        ///     rotation
        /// </param>
        /// <param name="db">
        ///     Provides the y-coordinate of the point p. On exit contains the parameter z associated with the Givens
        ///     rotation
        /// </param>
        /// <param name="c">Contains the parameter c associated with the Givens rotation</param>
        /// <param name="s">Contains the parameter s associated with the Givens rotation</param>
        /// <remarks>This is equivalent to the DROTG LAPACK routine.</remarks>
        private static void Drotg(ref double da, ref double db, out double c, out double s)
        {
            double r, z;

            double roe = db;
            double absda = System.Math.Abs(da);
            double absdb = System.Math.Abs(db);
            if (absda > absdb) roe = da;

            double scale = absda + absdb;
            if (scale == 0.0)
            {
                c = 1.0;
                s = 0.0;
                r = 0.0;
                z = 0.0;
            }
            else
            {
                double sda = da / scale;
                double sdb = db / scale;
                r = scale * System.Math.Sqrt(sda * sda + sdb * sdb);
                if (roe < 0.0) r = -r;

                c = da / r;
                s = db / r;
                z = 1.0;
                if (absda > absdb) z = s;

                if (absdb >= absda && c != 0.0) z = 1.0 / c;
            }

            da = r;
            db = z;
        }

        /// <summary>
        ///     Calculate Norm 2 of the column <paramref name="column1x8 in matrix <paramref name="a8x8 starting from row 
        ///     
        ///     <paramref name="rowStart
        /// 
        /// 
        /// </summary>
        /// <param name="a8x8">Source matrix</param>
        /// <param name="8">The number of rows in <paramref name="a8x8</param>
        /// <param name="column1x8">Column index</param>
        /// <param name="rowStart">Start row index</param>
        /// <returns>Norm2 (Euclidean norm) of the column</returns>
        private static double Dnrm2Column(double[] a8x8, int column1x8, int rowStart)
        {
            double s = 0;
            for (int i = rowStart; i < 8; i++) s += a8x8.At8x8(i, column1x8) * a8x8.At8x8(i, column1x8);

            return System.Math.Sqrt(s);
        }

        /// <summary>
        ///     Calculate Norm 2 of the vector <paramref name="a1x8 starting from index <paramref name="rowStart
        /// 
        /// 
        /// </summary>
        /// <param name="a1x8">Source vector</param>
        /// <param name="rowStart">Start index</param>
        /// <returns>Norm2 (Euclidean norm) of the vector</returns>
        private static double Dnrm2Vector(double[] a1x8, int rowStart)
        {
            double s = 0;
            for (int i = rowStart; i < a1x8.Length; i++) s += a1x8[i] * a1x8[i];

            return System.Math.Sqrt(s);
        }

        /// <summary>
        ///     Calculate dot product of <paramref name="columnA1x8 and <paramref name="columnB1x8
        /// 
        /// 
        /// </summary>
        /// <param name="a8x8">Source matrix</param>
        /// <param name="8">The number of rows in <paramref name="a8x8</param>
        /// <param name="columnA1x8">Index of column A</param>
        /// <param name="columnB1x8">Index of column B</param>
        /// <param name="rowStart">Starting row index</param>
        /// <returns>Dot product value</returns>
        private static double Ddot(double[] a8x8, int columnA1x8, int columnB1x8, int rowStart)
        {
            double z = 0.0;
            for (int i = rowStart; i < 8; i++) z += a8x8.At8x8(i, columnB1x8) * a8x8.At8x8(i, columnA1x8);

            return z;
        }

        /// <summary>
        ///     Performs rotation of points in the plane. Given two vectors x <paramref name="columnA and y 
        ///     
        ///     <paramref
        ///         name="columnB,
        /// each vector element of these vectors is replaced as follows: x(i) = c*x(i) + s*y(i); y(i) = c*y(i) - s*x(i)
        /// 
        /// 
        /// </summary>
        /// <param name="a8x8">Source matrix</param>
        /// <param name="8">The number of rows in <paramref name="a8x8</param>
        /// <param name="columnA">Index of column A</param>
        /// <param name="columnB">Index of column B</param>
        /// <param name="c">Scalar "c" value</param>
        /// <param name="s">Scalar "s" value</param>
        private static void Drot(double[] a8x8, int columnA, int columnB, double c, double s)
        {
            for (int i = 0; i < 8; i++)
            {
                double z = c * a8x8.At8x8(i, columnA) + s * a8x8.At8x8(i, columnB);
                double tmp = c * a8x8.At8x8(i, columnB) - s * a8x8.At8x8(i, columnA);
                a8x8.At8x8(i, columnB, tmp);
                a8x8.At8x8(i, columnA, z);
            }
        }

    }
}