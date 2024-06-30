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

        /// <summary>
        /// Find homography.  
        /// </summary>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(float sourceWidth, float sourceHeight, ITransformerLTRB destination) =>
          Matrix3x2.CreateScale(1f / sourceWidth, 1f / sourceHeight) * FindHomographyFromIdentity(destination);

        /// <summary>
        /// Find homography.  
        /// </summary>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix4x4 FindHomography3D(float sourceWidth, float sourceHeight, ITransformerLTRB destination)
        {
            Matrix4x4 S = FindHomographyFromIdentity3D(destination);
            return Matrix4x4.CreateScale(1f / sourceWidth, 1f / sourceHeight, 1) * B3D(S, destination.RightBottom) * S;
        }

        /// <summary>
        /// Find homography.  
        /// </summary>
        /// <param name="sourceX"> The x of source rectangle. </param>
        /// <param name="sourceY"> The y of source rectangle. </param>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(float sourceX, float sourceY, float sourceWidth, float sourceHeight, ITransformerLTRB destination) => new Matrix3x2
        {
            M11 = 1f / sourceWidth,
            M22 = 1f / sourceHeight,
            M31 = -sourceX / sourceWidth,
            M32 = -sourceY / sourceHeight,
        } * FindHomographyFromIdentity(destination);

        /// <summary>
        /// Find homography.  
        /// </summary>
        /// <param name="sourceX"> The x of source rectangle. </param>
        /// <param name="sourceY"> The y of source rectangle. </param>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination transformer. </param>
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
        /// Find homography.  
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(TransformerRect source, ITransformerLTRB destination)
        {
            return FindHomography(source.Left, source.Top, source.Width, source.Height, destination);
        }

        /// <summary>
        /// Find homography.  
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix4x4 FindHomography3D(TransformerRect source, ITransformerLTRB destination)
        {
            return FindHomography3D(source.Left, source.Top, source.Width, source.Height, destination);
        }

        /// <summary>
        /// Find homography.  
        /// </summary>
        /// <param name="source"> The source transformer. </param>
        /// <param name="destination"> The destination transformer. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(ITransformerLTRB source, ITransformerLTRB destination)
        {
            Matrix3x2 m = FindHomographyFromIdentity(source);
            return Matrix3x2.Invert(m, out m) ? m * FindHomographyFromIdentity(destination) : FindHomographyFromIdentity(destination);
        }

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