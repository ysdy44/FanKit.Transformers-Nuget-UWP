using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {

        /// <summary>
        /// Draw a line (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawLineDodgerBlue(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1) => drawingSession.DrawLine(point0, point1, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ——.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawThickLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1)
        {
            drawingSession.DrawLine(point0, point1, Windows.UI.Colors.Black, 2);
            drawingSession.DrawLine(point0, point1, Windows.UI.Colors.White);
        }



     


        /// <summary>
        /// Fill a Rectangular (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        public static void FillRectDodgerBlue(this CanvasDrawingSession drawingSession, TransformerRect transformerRect)
        {
            Rect rect = transformerRect.ToRect();
            drawingSession.FillRectangle(rect, Windows.UI.Color.FromArgb(90, 54, 135, 230));
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.DodgerBlue);
        }

        /// <summary>
        /// Fill a Rectangular (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void FillRectDodgerBlue(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformerRect.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformerRect.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformerRect.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformerRect.LeftBottom, matrix);

            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom
            };

            //Geometry
            CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(resourceCreator, points);
            drawingSession.FillGeometry(canvasGeometry, Windows.UI.Color.FromArgb(90, 54, 135, 230));
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
        }

    }
}