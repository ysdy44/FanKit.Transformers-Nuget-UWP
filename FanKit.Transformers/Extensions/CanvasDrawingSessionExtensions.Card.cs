﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    partial class CanvasDrawingSessionExtensions
    {

        // Card
        /// <summary> The color of the drop shadow. Default (A64 R0 G0 B0). </summary>
        public static Windows.UI.Color ShadowColor = Windows.UI.Color.FromArgb(64, 0, 0, 0);

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void DrawShadowCore(CanvasDrawingSession drawingSession, ICanvasImage image, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset)
        {
            ICanvasImage shadow = new ShadowEffect
            {
                Source = image,
                ShadowColor = shadowColor,
                BlurAmount = shadowBlurAmount,
            };
            drawingSession.DrawImage(shadow, shadowOffset, shadowOffset);
            drawingSession.DrawImage(image);
        }


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void DrawCradCore(CanvasDrawingSession drawingSession, ICanvasImage previousImage, Rect cropRect, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset) => CanvasDrawingSessionExtensions.DrawShadowCore(drawingSession, new CropEffect
        {
            Source = previousImage,
            SourceRectangle = cropRect
        }, shadowColor, shadowBlurAmount, shadowOffset);
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
      
        private static void DrawCradCore(CanvasDrawingSession drawingSession, ICanvasImage previousImage, Rect cropRect, Matrix3x2 matrix, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset) => CanvasDrawingSessionExtensions.DrawShadowCore(drawingSession, new Transform2DEffect
        {
            TransformMatrix = matrix,
            Source = new CropEffect
            {
                Source = previousImage,
                SourceRectangle = cropRect
            }
        }, shadowColor, shadowBlurAmount, shadowOffset);


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void DrawCradCore(CanvasDrawingSession drawingSession, ICanvasImage previousImage, CanvasTransformer canvasTransformer, Windows.UI.Color shadowColor, float shadowBlurAmount, float shadowOffset)
        {
            float width = canvasTransformer.Width * canvasTransformer.Scale;
            float height = canvasTransformer.Height * canvasTransformer.Scale;
            Rect rect = new Rect(-width / 2, -height / 2, width, height);

            Matrix3x2 matrix = canvasTransformer.GetMatrix(MatrixTransformerMode.VirtualToControl);
            CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, rect, matrix, shadowColor, shadowBlurAmount, shadowOffset);
        }


        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="previousImage"> The previous image. </param>
        /// <param name="cropRect"> The image crop rectangle. </param>
        /// <param name="shadowBlurAmount"> The shaodw blur amount. </param>
        /// <param name="shadowOffset"> The shadow offset. </param>
        public static void DrawCrad(this CanvasDrawingSession drawingSession, ICanvasImage previousImage, Rect cropRect, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, cropRect, CanvasDrawingSessionExtensions.ShadowColor, shadowBlurAmount, shadowOffset);
        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="previousImage"> The previous image. </param>
        /// <param name="cropRect"> The image crop rectangle. </param>
        /// <param name="shadowColor"> The shadow color. </param>
        /// <param name="shadowBlurAmount"> The shaodw blur amount. </param>
        /// <param name="shadowOffset"> The shadow offset. </param>
        public static void DrawCrad(this CanvasDrawingSession drawingSession, ICanvasImage previousImage, Rect cropRect, Windows.UI.Color shadowColor, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, cropRect, shadowColor, shadowBlurAmount, shadowOffset);



        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="previousImage"> The previous image. </param>
        /// <param name="cropRect"> The image crop rectangle. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="shadowBlurAmount"> The shaodw blur amount. </param>
        /// <param name="shadowOffset"> The shadow offset. </param>
        public static void DrawCrad(this CanvasDrawingSession drawingSession, ICanvasImage previousImage, Rect cropRect, Matrix3x2 matrix, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, cropRect, matrix, CanvasDrawingSessionExtensions.ShadowColor, shadowBlurAmount, shadowOffset);
        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="previousImage"> The previous image. </param>
        /// <param name="cropRect"> The image crop rectangle. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="shadowColor"> The shadow color. </param>
        /// <param name="shadowBlurAmount"> The shaodw blur amount. </param>
        /// <param name="shadowOffset"> The shadow offset. </param>
        public static void DrawCrad(this CanvasDrawingSession drawingSession, ICanvasImage previousImage, Rect cropRect, Matrix3x2 matrix, Windows.UI.Color shadowColor, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, cropRect, matrix, shadowColor, shadowBlurAmount, shadowOffset);



        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="previousImage"> The previous image. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="shadowBlurAmount"> The shaodw blur amount. </param>
        /// <param name="shadowOffset"> The shadow offset. </param>
        public static void DrawCrad(this CanvasDrawingSession drawingSession, ICanvasImage previousImage, CanvasTransformer canvasTransformer, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, canvasTransformer, CanvasDrawingSessionExtensions.ShadowColor, shadowBlurAmount, shadowOffset);
        /// <summary>
        /// Draw a card.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="previousImage"> The previous image. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="shadowColor"> The shadow color. </param>
        /// <param name="shadowBlurAmount"> The shaodw blur amount. </param>
        /// <param name="shadowOffset"> The shadow offset. </param>
        public static void DrawCard(this CanvasDrawingSession drawingSession, ICanvasImage previousImage, CanvasTransformer canvasTransformer, Windows.UI.Color shadowColor, float shadowBlurAmount = 4.0f, float shadowOffset = 5.0f) => CanvasDrawingSessionExtensions.DrawCradCore(drawingSession, previousImage, canvasTransformer, shadowColor, shadowBlurAmount, shadowOffset);

    }
}