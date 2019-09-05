using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {
        /// <summary> The translucent color of the DodgerBlue. Default (A90 R54 G135 B230).</summary>
        public static readonly Windows.UI.Color TranslucentDodgerBlue = Windows.UI.Color.FromArgb(90, 54, 135, 230);

        /// <summary>
        /// Fill a Rectangular (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        public static void FillRectangleDodgerBlue(this CanvasDrawingSession drawingSession, TransformerRect transformerRect)
        {
            Rect rect = transformerRect.ToRect();
            drawingSession.FillRectangle(rect, CanvasDrawingSessionExtensions.TranslucentDodgerBlue);
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Fill a Rectangular (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void FillRectangleDodgerBlue(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect, Matrix3x2 matrix)
        {
            CanvasGeometry canvasGeometry = TransformerRect.CreateRectangle(resourceCreator, transformerRect, matrix);
            drawingSession.FillGeometry(canvasGeometry, CanvasDrawingSessionExtensions.TranslucentDodgerBlue);
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
        }

        /// <summary>
        /// Fill a Ellipse (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        private static void FillEllipseDodgerBlue(this CanvasDrawingSession drawingSession, TransformerRect transformerRect)
        {
            Vector2 centerPoint = transformerRect.Center;
            float width = transformerRect.Width / 2;
            float height = transformerRect.Height / 2;

            drawingSession.FillEllipse(centerPoint, width, height, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Fill a Ellipse (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        private static void FillEllipseDodgerBlue(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect, Matrix3x2 matrix)
        {
            CanvasGeometry canvasGeometry = TransformerRect.CreateEllipse(resourceCreator, transformerRect, matrix);
            drawingSession.FillGeometry(canvasGeometry, CanvasDrawingSessionExtensions.TranslucentDodgerBlue);
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
        }


        /// <summary>
        /// Draw a thick Rectangular.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        public static void DrawThickRectangle(this CanvasDrawingSession drawingSession, TransformerRect transformerRect)
        {
            Rect rect = transformerRect.ToRect();
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.Black, 2);
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.White);
        }

        /// <summary>
        /// Draw a thick Ellipse.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        public static void DrawThickEllipse(this CanvasDrawingSession drawingSession, TransformerRect transformerRect)
        {
            Vector2 centerPoint = transformerRect.Center;
            float width = transformerRect.Width / 2;
            float height = transformerRect.Height / 2;

            drawingSession.DrawEllipse(centerPoint, width, height, Windows.UI.Colors.Black, 2);
            drawingSession.DrawEllipse(centerPoint, width, height, Windows.UI.Colors.White);
        }
    }
}