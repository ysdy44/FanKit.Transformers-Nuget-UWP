using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {

        //Card
        static Windows.UI.Color ShadowColor = Windows.UI.Color.FromArgb(64, 0, 0, 0);

        private static void _DrawShadow(CanvasDrawingSession ds, ICanvasImage image, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset)
        {
            ICanvasImage shadow = new ShadowEffect
            {
                Source = image,
                ShadowColor = shadowColor,
                BlurAmount = shadowBlurAmount,
            };
            ds.DrawImage(shadow, shadowOffset, shadowOffset);
            ds.DrawImage(image);
        }


        private static void _DrawCrad(CanvasDrawingSession ds, ICanvasImage previousImage, Rect cropRect, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset) => CanvasDrawingSessionExtensions._DrawShadow(ds, new CropEffect
        {
            Source = previousImage,
            SourceRectangle = cropRect
        }, shadowColor, shadowBlurAmount, shadowOffset);
        private static void _DrawCrad(CanvasDrawingSession ds, ICanvasImage previousImage, Rect cropRect, Matrix3x2 matrix, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset) => CanvasDrawingSessionExtensions._DrawShadow(ds, new Transform2DEffect
        {
            TransformMatrix = matrix,
            Source = new CropEffect
            {
                Source = previousImage,
                SourceRectangle = cropRect
            }
        }, shadowColor, shadowBlurAmount, shadowOffset);


        private static void _DrawCrad(CanvasDrawingSession ds, ICanvasImage previousImage, CanvasTransformer canvasTransformer, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset)
        {
            float width = canvasTransformer.Width * canvasTransformer.Scale;
            float height = canvasTransformer.Height * canvasTransformer.Scale;
            Rect rect = new Rect(-width / 2, -height / 2, width, height);
            Matrix3x2 matrix = canvasTransformer.GetMatrix(MatrixTransformerMode.VirtualToControl);
            CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, rect, matrix, shadowColor, shadowBlurAmount, shadowOffset);
        }


        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="previousImage"> previous image </param>
        /// <param name="cropRect"> image crop rectangle</param>
        /// <param name="shadowBlurAmount"> shaodw blur amount </param>
        /// <param name="shadowOffset"> shadow offset</param>
        public static void DrawCrad(this CanvasDrawingSession ds, ICanvasImage previousImage, Rect cropRect, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, cropRect, CanvasDrawingSessionExtensions.ShadowColor, shadowBlurAmount, shadowOffset);
        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="previousImage"> previous image </param>
        /// <param name="cropRect"> image crop rectangle</param>
        /// <param name="shadowColor"> shadow color </param>
        /// <param name="shadowBlurAmount"> shaodw blur amount </param>
        /// <param name="shadowOffset"> shadow offset</param>
        public static void DrawCrad(this CanvasDrawingSession ds, ICanvasImage previousImage, Rect cropRect, Windows.UI.Color shadowColor, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, cropRect, shadowColor, shadowBlurAmount, shadowOffset);



        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="previousImage"> previous image </param>
        /// <param name="cropRect"> image crop rectangle</param>
        /// <param name="matrix"> matrix </param>
        /// <param name="shadowBlurAmount"> shaodw blur amount </param>
        /// <param name="shadowOffset"> shadow offset</param>
        public static void DrawCrad(this CanvasDrawingSession ds, ICanvasImage previousImage, Rect cropRect, Matrix3x2 matrix, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, cropRect, matrix, CanvasDrawingSessionExtensions.ShadowColor, shadowBlurAmount, shadowOffset);
        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="previousImage"> previous image </param>
        /// <param name="cropRect"> image crop rectangle</param>
        /// <param name="matrix"> matrix </param>
        /// <param name="shadowColor"> shadow color </param>
        /// <param name="shadowBlurAmount"> shaodw blur amount </param>
        /// <param name="shadowOffset"> shadow offset</param>
        public static void DrawCrad(this CanvasDrawingSession ds, ICanvasImage previousImage, Rect cropRect, Matrix3x2 matrix, Windows.UI.Color shadowColor, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, cropRect, matrix, shadowColor, shadowBlurAmount, shadowOffset);



        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="previousImage"> previous image </param>
        /// <param name="canvasTransformer"></param>
        /// <param name="shadowBlurAmount"> shaodw blur amount </param>
        /// <param name="shadowOffset"> shadow offset</param>
        public static void DrawCrad(this CanvasDrawingSession ds, ICanvasImage previousImage, CanvasTransformer canvasTransformer, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, canvasTransformer, CanvasDrawingSessionExtensions.ShadowColor, shadowBlurAmount, shadowOffset);
        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="ds"> CanvasDrawingSession </param>
        /// <param name="previousImage"> previous image </param>
        /// <param name="canvasTransformer"></param>
        /// <param name="shadowColor"> shadow color </param>
        /// <param name="shadowBlurAmount"> shaodw blur amount </param>
        /// <param name="shadowOffset"> shadow offset</param>
        public static void DrawCrad(this CanvasDrawingSession ds, ICanvasImage previousImage, CanvasTransformer canvasTransformer, Windows.UI.Color shadowColor, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions._DrawCrad(ds, previousImage, canvasTransformer, shadowColor, shadowBlurAmount, shadowOffset);

    }
}