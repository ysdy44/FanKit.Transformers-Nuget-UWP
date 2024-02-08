// <copyright file="Matrix.cs" company="Math.NET">
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
    ///     Defines the base class for <c>Matrix</c> classes.
    /// </summary>
    /// <typeparam name="double">
    ///     Supported data types are <c>double</c>, <c>single</c>, <seecreComplex, and <seecreComplex32.
    /// </typeparam>
    partial class MathNetNumericsExtensions
    {
        /// <summary>
        ///     Creates a clone of this instance.
        /// </summary>
        /// <returns>
        ///     A clone of the instance.
        /// </returns>
        public static double[] Clone8x8(this double[] data8x8)
        {
            double[] resultData = new double[8 * 8];
            System.Array.Copy(data8x8, 0, resultData, 0, data8x8.Length);
            return resultData;
        }


        /// <summary>
        ///     Returns the transpose of this matrix.
        /// </summary>
        /// <returns>The transpose of this matrix.</returns>
        public static double[] Transpose8x8(this double[] data8x8)
        {
            double[] resultData = new double[8 * 8];
            data8x8.TransposeToUnchecked8x8(resultData);
            return resultData;
        }

        /// <summary>
        ///     Retrieves the requested element without range checking.
        /// </summary>
        public static double At8x8(this double[] data8x8, int row1x8, int column1x8)
        {
            return data8x8[column1x8 * 8 + row1x8];
        }

        /// <summary>
        ///     Sets the element without range checking.
        /// </summary>
        public static void At8x8(this double[] data8x8, int row1x8, int column1x8, double value)
        {
            data8x8[column1x8 * 8 + row1x8] = value;
        }

        public static double[] PseudoInverse8x8(this double[] data8x8)
        {
            Svd8x8 svd = Create(data8x8);
            double[] w = svd.W;
            double[] s = svd.S;
            double tolerance = 8 * svd.L2Norm * System.Math.Pow(2, -53);

            for (int i = 0; i < 8; i++)
            {
                s[i] = s[i] < tolerance ? 0 : 1 / s[i];
            }

            for (int i = 0; i < 8; i++)
            {
                double value = s[i];
                w.At8x8(i, i, value);
            }


            double[] ddd = svd.U.Multiply8x8(w).Multiply8x8(svd.VT);
            return ddd.TransposeToUnchecked8x8();
        }
        
        public static void TransposeToUnchecked8x8(this double[] data8x8, double[] target8x8)
        {
            for (int j = 0; j < 8; j++)
            {
                int index = j * 8;
                for (int i = 0; i < 8; i++) target8x8[i * 8 + j] = data8x8[index + i];
            }
        }

        public static double[] TransposeToUnchecked8x8(this double[] data8x8)
        {
            double[] targetData = new double[8 * 8];
            for (int j = 0; j < 8; j++)
            {
                int index = j * 8;
                for (int i = 0; i < 8; i++) targetData[i * 8 + j] = data8x8[index + i];
            }
            return targetData;
        }

        public static float Multiply8x8(this double[] data8x8, double[] rightSide1x8, int index1x8)
        {
            double[] pseudoInverse = data8x8;
            double s = 0.0;

            for (int k = 0; k < 8; k++)
            {
                double d = pseudoInverse.At8x8(index1x8, k);
                s += d * rightSide1x8[k];
            }

            return (float)s;
        }

        /// <summary>
        ///     Multiplies this matrix with another matrix and returns the result.
        /// </summary>
        /// <param name="other8x8">The matrix to multiply with.</param>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <strong>this.Columns != other.Rows</strong>
        /// .
        /// </exception>
        /// <returns>The result of the multiplication.</returns>
        public static double[] Multiply8x8(this double[] data8x8, double[] other8x8)
        {
            double[] result = new double[8 * 8];

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    double s = 0.0;
                    for (int k = 0; k < 8; k++)
                    {
                        s += data8x8.At8x8(i, k) * other8x8.At8x8(k, j);
                    }
                    result.At8x8(i, j, s);
                }
            }
            return result;
        }
    }
}