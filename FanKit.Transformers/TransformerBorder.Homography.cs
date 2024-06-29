using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {

        /// <summary>
        /// Find homography.
        /// </summary>
        /// <param name="source"> The source border. </param>
        /// <param name="destination"> The destination border. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(TransformerBorder source, TransformerBorder destination) 
            => TransformerBorder.FindHomography(source.Left, source.Top, source.Right - source.Left, source.Bottom - source.Top, destination.Left, destination.Top, destination.Right - destination.Left, destination.Bottom - destination.Top);

        /// <summary>
        /// Find homography.
        /// </summary>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination border. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(float sourceWidth, float sourceHeight, TransformerBorder destination) 
            => TransformerBorder.FindHomography(sourceWidth, sourceHeight, destination.Left, destination.Top, destination.Right - destination.Left, destination.Bottom - destination.Top);

        /// <summary>
        /// Find homography.
        /// </summary>
        /// <param name="sourceX"> The x of source rectangle. </param>
        /// <param name="sourceY"> The y of source rectangle. </param>
        /// <param name="sourceWidth"> The width of source rectangle. </param>
        /// <param name="sourceHeight"> The height of source rectangle. </param>
        /// <param name="destination"> The destination border. </param>
        /// <returns> The homologous matrix. </returns>
        public static Matrix3x2 FindHomography(float sourceX, float sourceY, float sourceWidth, float sourceHeight, TransformerBorder destination) 
            => TransformerBorder.FindHomography(sourceX, sourceY, sourceWidth, sourceHeight, destination.Left, destination.Top, destination.Right - destination.Left, destination.Bottom - destination.Top);

        private static Matrix3x2 FindHomography(float sourceWidth, float sourceHeight, float destinationX, float destinationY, float destinationWidth, float destinationHeight) => new Matrix3x2
        {
            M11 = destinationWidth / sourceWidth,
            M22 = destinationHeight / sourceHeight,
            M31 = destinationX,
            M32 = destinationY
        };

        private static Matrix3x2 FindHomography(float sourceX, float sourceY, float sourceWidth, float sourceHeight, float destinationX, float destinationY, float destinationWidth, float destinationHeight)
        {
            float scaleX = destinationWidth / sourceWidth;
            float scaleY = destinationHeight / sourceHeight;

            float offsetX = destinationX - sourceX * scaleX;
            float offsetY = destinationY - sourceY * scaleY;

            return new Matrix3x2
            {
                M11 = scaleX,
                M22 = scaleY,
                M31 = offsetX,
                M32 = offsetY
            };
        }

    }
}