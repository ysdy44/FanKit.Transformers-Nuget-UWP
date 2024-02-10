using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {

        internal static Matrix3x2 FindHomographyFromIdentity(ITransformerLTRB transformer) => new Matrix3x2
        {
            M11 = transformer.RightTop.X - transformer.LeftTop.X,
            M12 = transformer.RightTop.Y - transformer.LeftTop.Y,
            M21 = transformer.LeftBottom.X - transformer.LeftTop.X,
            M22 = transformer.LeftBottom.Y - transformer.LeftTop.Y,
            M31 = transformer.LeftTop.X,
            M32 = transformer.LeftTop.Y,
        };

        internal static Matrix4x4 FindHomographyFromIdentity3D(ITransformerLTRB transformer) => new Matrix4x4
        {
            M11 = transformer.RightTop.X - transformer.LeftTop.X,
            M12 = transformer.RightTop.Y - transformer.LeftTop.Y,
            M21 = transformer.LeftBottom.X - transformer.LeftTop.X,
            M22 = transformer.LeftBottom.Y - transformer.LeftTop.Y,
            M33 = 1,
            M41 = transformer.LeftTop.X,
            M42 = transformer.LeftTop.Y,
            M44 = 1,
        };

        internal static Matrix3x2 FindHomographyFromIdentity(float x, float y, float width, float height) => new Matrix3x2(
                width, // ScaleX
                0, // SkewY
                0, // SkewX
                height, // ScaleY 
                x, // TransX
                y); // TransY

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(float sourceWidth, float sourceHeight, ITransformerLTRB destination) =>
          Matrix3x2.CreateScale(1f / sourceWidth, 1f / sourceHeight) * FindHomographyFromIdentity(destination);

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix4x4 FindHomography3D(float sourceWidth, float sourceHeight, ITransformerLTRB destination)
        {
            Matrix4x4 S = FindHomographyFromIdentity3D(destination);
            return Matrix4x4.CreateScale(1f / sourceWidth, 1f / sourceHeight, 1) * B3D(S, destination.RightBottom) * S;
        }

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="sourceX"> The x of source rectangle. </param>
        /// <param name="sourceY"> The y of source rectangle. </param>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(float sourceX, float sourceY, float sourceWidth, float sourceHeight, ITransformerLTRB destination) => new Matrix3x2
        {
            M11 = 1f / sourceWidth,
            M22 = 1f / sourceHeight,
            M31 = -sourceX / sourceWidth,
            M32 = -sourceY / sourceHeight,
        } * FindHomographyFromIdentity(destination);

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="sourceX"> The x of source rectangle. </param>
        /// <param name="sourceY"> The y of source rectangle. </param>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix4x4 FindHomography3D(float sourceX, float sourceY, float sourceWidth, float sourceHeight, ITransformerLTRB destination)
        {
            Matrix4x4 S = FindHomographyFromIdentity3D(destination);
            return new Matrix4x4
            {
                M11 = 1 / sourceWidth,
                M22 = 1 / sourceHeight,
                M33 = 1,
                M41 = -sourceX / sourceWidth,
                M42 = -sourceY / sourceHeight,
                M44 = 1,
            } * B3D(S, destination.RightBottom) * S;
        }

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="source"> The source Transformer. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <param name="distance">
        /// <para>If the source/destination is not a parallelogram:</para>
        /// <para>Vector2 point = ?</para>
        /// <para>Matrix3x2 matrix = Transformer.FindHomography(source, destination, out Vector2 distance);</para>
        /// <para>Vector2 position = Vector2.Transform(point, matrix) / (1 + Vector2.Dot(point, distance));</para>
        /// </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(ITransformerLTRB source, ITransformerLTRB destination, out Vector2 distance)
        {
            float x0 = source.LeftTop.X, x1 = source.RightTop.X, x2 = source.LeftBottom.X, x3 = source.RightBottom.X;
            float y0 = source.LeftTop.Y, y1 = source.RightTop.Y, y2 = source.LeftBottom.Y, y3 = source.RightBottom.Y;
            float u0 = destination.LeftTop.X, u1 = destination.RightTop.X, u2 = destination.LeftBottom.X, u3 = destination.RightBottom.X;
            float v0 = destination.LeftTop.Y, v1 = destination.RightTop.Y, v2 = destination.LeftBottom.Y, v3 = destination.RightBottom.Y;

            double[] matrix = new double[8 * 8]
            {

                // 0 
                x0,
                0,
                x1,
                0,
                x3,
                0,
                x2,
                0,

                // 1
                y0,
                0,
                y1,
                0,
                y3,
                0,
                y2,
                0,

                // 2
                1,
                0,
                1,
                0,
                1,
                0,
                1,
                0,

                // 3
                0,
                x0,
                0,
                x1,
                0,
                x3,
                0,
                x2,

                // 4
                0,
                y0,
                0,
                y1,
                0,
                y3,
                0,
                y2,

                // 5
                0,
                1,
                0,
                1,
                0,
                1,
                0,
                1,

                // 6
                -u0 * x0,
                -v0 * x0,
                -u1 * x1,
                -v1 * x1,
                -u3 * x3,
                -v3 * x3,
                -u2 * x2,
                -v2 * x2,

                // 7
                -u0 * y0,
                -v0 * y0,
                -u1 * y1,
                -v1 * y1,
                -u3 * y3,
                -v3 * y3,
                -u2 * y2,
                -v2 * y2
            }.PseudoInverse8x8();

            double[] vector = new double[]
            {
                u0, v0, u1, v1,
                u3, v3, u2, v2
            };

            distance = new Vector2(matrix.Multiply8x8(vector, 6), matrix.Multiply8x8(vector, 7));
            return new Matrix3x2
            (
                m11: matrix.Multiply8x8(vector, 0), m12: matrix.Multiply8x8(vector, 3),
                m21: matrix.Multiply8x8(vector, 1), m22: matrix.Multiply8x8(vector, 4),
                m31: matrix.Multiply8x8(vector, 2), m32: matrix.Multiply8x8(vector, 5)
            );
        }

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="source"> The source Transformer. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(ITransformerLTRB source, ITransformerLTRB destination)
        {
            Matrix3x2 m = FindHomographyFromIdentity(source);
            return Matrix3x2.Invert(m, out m) ? m * FindHomographyFromIdentity(destination) : FindHomographyFromIdentity(destination);
        }

        /// <summary>
        /// Find Homography.  
        /// </summary>
        /// <param name="source"> The source Transformer. </param>
        /// <param name="destination"> The destination Transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix4x4 FindHomography3D(TransformerRect source, ITransformerLTRB destination)
            => FindHomography3D(source.Left, source.Top, source.Width, source.Height, destination);

        private static Matrix4x4 B3D(Matrix4x4 A, Vector2 lrt)
        {
            //  A Matrix -> a b
            float den = A.M11 * A.M22 - A.M12 * A.M21;
            float a = (A.M22 * lrt.X - A.M21 * lrt.Y + A.M21 * A.M42 - A.M22 * A.M41) / den;
            float b = (A.M11 * lrt.Y - A.M12 * lrt.X + A.M12 * A.M41 - A.M11 * A.M42) / den;

            // compute B Matrix
            // (0, 0)->(0, 0)
            // (0, 1)->(0, 1)
            // (1, 0)->(1, 0)
            // (1, 1)->(a, b)
            float ab1 = a + b - 1;
            float scaleX = a / ab1;
            float scaleY = b / ab1;
            return new Matrix4x4
            {
                M11 = scaleX,
                M14 = scaleX - 1,
                M22 = scaleY,
                M24 = scaleY - 1,
                M33 = 1,
                M44 = 1,
            };
        }
    }
}