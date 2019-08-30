﻿using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {
        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="source"> The source Transformer. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(Transformer source, Transformer destination)
        {

            float x0 = source.LeftTop.X, x1 = source.RightTop.X, x2 = source.LeftBottom.X, x3 = source.RightBottom.X;
            float y0 = source.LeftTop.Y, y1 = source.RightTop.Y, y2 = source.LeftBottom.Y, y3 = source.RightBottom.Y;
            float u0 = destination.LeftTop.X, u1 = destination.RightTop.X, u2 = destination.LeftBottom.X, u3 = destination.RightBottom.X;
            float v0 = destination.LeftTop.Y, v1 = destination.RightTop.Y, v2 = destination.LeftBottom.Y, v3 = destination.RightBottom.Y;

            MathNet.Numerics.LinearAlgebra.Double.DenseMatrix matrix = MathNet.Numerics.LinearAlgebra.Double.DenseMatrix.OfArray(new double[008, 008]
            {
                { x0, y0, 1, 0, 0, 0, -u0* x0, -u0* y0 },
                { 0, 0, 0, x0, y0, 1, -v0* x0, -v0* y0 },
                { x1, y1, 1, 0, 0, 0, -u1* x1, -u1* y1 },
                { 0, 0, 0, x1, y1, 1, -v1* x1, -v1* y1 },
                { x3, y3, 1, 0, 0, 0, -u3* x3, -u3* y3 },
                { 0, 0, 0, x3, y3, 1, -v3* x3, -v3* y3 },
                { x2, y2, 1, 0, 0, 0, -u2* x2, -u2* y2 },
                { 0, 0, 0, x2, y2, 1, -v2* x2, -v2* y2 },
            });

            MathNet.Numerics.LinearAlgebra.Double.DenseVector vector = new MathNet.Numerics.LinearAlgebra.Double.DenseVector(new double[008]
            {
                u0, v0, u1, v1,
                u3, v3, u2, v2
            });

            MathNet.Numerics.LinearAlgebra.Matrix<double> PseudoInverse = Transformer._pseudoInverse2(matrix);

            MathNet.Numerics.LinearAlgebra.Vector<double> ret = Transformer._multiply2(PseudoInverse, vector);

            return new Matrix3x2
            (
                m11: (float)ret[0], m12: (float)ret[3],
                m21: (float)ret[1], m22: (float)ret[4],
                m31: (float)ret[2], m32: (float)ret[5]
            );
        }

        private static MathNet.Numerics.LinearAlgebra.Matrix<double> _pseudoInverse2(MathNet.Numerics.LinearAlgebra.Double.Matrix matrix)
        {
            MathNet.Numerics.LinearAlgebra.Factorization.Svd<double> svd = MathNet.Numerics.LinearAlgebra.Double.Factorization.UserSvd.Create(matrix, true);
            MathNet.Numerics.LinearAlgebra.Matrix<double> w = svd.W;
            MathNet.Numerics.LinearAlgebra.Vector<double> s = svd.S;
            double tolerance = 008 * svd.L2Norm * System.Math.Pow(2, -53);

            for (int i = 0; i < s.Count; i++)
            {
                s[i] = s[i] < tolerance ? 0 : 1 / s[i];
            }

            for (var i = 0; i < 008; i++)
            {
                double value = s.At(i);
                w.Storage.At(i, i, value);
            }

            MathNet.Numerics.LinearAlgebra.Matrix<double> ddd = svd.U * w * svd.VT;
            return ddd.Transpose();
        }

        private static MathNet.Numerics.LinearAlgebra.Vector<double> _multiply2(MathNet.Numerics.LinearAlgebra.Matrix<double> matrix, MathNet.Numerics.LinearAlgebra.Vector<double> rightSide)
        {
            MathNet.Numerics.LinearAlgebra.VectorBuilder<double> build = MathNet.Numerics.LinearAlgebra.Vector<double>.Build;
            MathNet.Numerics.LinearAlgebra.Vector<double> ret = build.SameAs(matrix, rightSide, matrix.RowCount);

            Transformer._doMultiply(matrix, rightSide, ret);

            return ret;
        }

        private static MathNet.Numerics.LinearAlgebra.Matrix<double> _doMultiply(MathNet.Numerics.LinearAlgebra.Matrix<double> matrix, MathNet.Numerics.LinearAlgebra.Vector<double> rightSide, MathNet.Numerics.LinearAlgebra.Vector<double> result)
        {
            for (var i = 0; i < matrix.RowCount; i++)
            {
                double s = 0.0;
                for (var j = 0; j < 008; j++)
                {
                    double ddd = matrix.Storage.At(i, j);
                    s += ddd * rightSide[j];
                }
                result[i] = s;
            }

            return matrix;
        }


        // TODO: bug
        /*
         
        /// <summary> Find Homography. </summary>
        public static Matrix3x2 FindHomography22(Transformer source, Transformer destination)
        {
            float x0 = source.LeftTop.X, x1 = source.RightTop.X, x2 = source.LeftBottom.X, x3 = source.RightBottom.X;
            float y0 = source.LeftTop.Y, y1 = source.RightTop.Y, y2 = source.LeftBottom.Y, y3 = source.RightBottom.Y;
            float u0 = destination.LeftTop.X, u1 = destination.RightTop.X, u2 = destination.LeftBottom.X, u3 = destination.RightBottom.X;
            float v0 = destination.LeftTop.Y, v1 = destination.RightTop.Y, v2 = destination.LeftBottom.Y, v3 = destination.RightBottom.Y;
            float[,] A = new float[8, 9]
            {
                    { x0, y0, 1, 0, 0, 0, -x0* u0, -y0* u0, u0 },
                    { x1, y1, 1, 0, 0, 0, -x1* u1, -y1* u1, u1 },
                    { x2, y2, 1, 0, 0, 0, -x2* u2, -y2* u2, u2 },
                    { x3, y3, 1, 0, 0, 0, -x3* u3, -y3* u3, u3 },
                    { 0, 0, 0, x0, y0, 1, -x0* v0, -y0* v0, v0 },
                    { 0, 0, 0, x1, y1, 1, -x1* v1, -y1* v1, v1 },
                    { 0, 0, 0, x2, y2, 1, -x2* v2, -y2* v2, v2 },
                    { 0, 0, 0, x3, y3, 1, -x3* v3, -y3* v3, v3 },
            };


            //epu:A's row  var:A's col-1
            int row, column;
            for (row = 0, column = 0; column < 8 && row < 8; column++, row++)
            {
                int max_r = row;
                for (int i = row + 1; i < 8; i++)
                {
                    if ((1e-12) < Math.Abs(A[i, column]) - Math.Abs(A[max_r, column]))
                    {
                        max_r = i;
                    }
                }

                if (max_r != row)
                {
                    for (int j = 0; j < 8 + 1; j++)
                    {
                        var a = A[row, j];
                        var b = A[max_r, j];
                        A[row, j] = b;
                        A[max_r, j] = a;
                    }
                }

                for (int i = row + 1; i < 8; i++)
                {
                    if (Math.Abs(A[i, column]) < (1e-12)) continue;

                    float tmp = -A[i, column] / A[row, column];

                    for (int j = column; j < 8 + 1; j++)
                    {
                        A[i, j] += tmp * A[row, j];
                    }
                }
            }


            float[] ret = new float[6];
            for (int i = 8 - 1; i >= 0; i--)
            {
                //Calculate Unique Solutions
                float tmp = 0;
                for (int j = i + 1; j < 8; j++)
                {
                    float n = ret[j % 6];
                    tmp += A[i, j] * n;
                }
                ret[i % 6] = (A[i, 8] - tmp) / A[i, i];
            }

            return new Matrix3x2
            (
                m11: ret[0], m12: ret[3],
                m21: ret[1], m22: ret[4],
                m31: ret[2], m32: ret[5]
            );
        }


         
         */

    }
}