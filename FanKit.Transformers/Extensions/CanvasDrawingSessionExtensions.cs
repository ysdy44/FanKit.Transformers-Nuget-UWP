using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref="CanvasDrawingSession"/>.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {
        /// <summary> The translucent color of the DodgerBlue. Default (A90 R54 G135 B230). </summary>
        public static readonly Windows.UI.Color TranslucentDodgerBlue = Windows.UI.Color.FromArgb(90, 54, 135, 230);


        /// <summary>
        /// Draw a line and the DodgerBlue color.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point0"> The first point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawLineDodgerBlue(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1) => drawingSession.DrawLine(point0, point1, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a line and the DodgerBlue color.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x0"> The X-axis value of the first point. </param>
        /// <param name="y0"> The Y-axis position of the first point. </param>
        /// <param name="x1"> The X-axis value of the second point. </param>
        /// <param name="y1"> The Y-axis position of the second point. </param>
        public static void DrawLineDodgerBlue(this CanvasDrawingSession drawingSession, float x0, float y0, float x1, float y1) => drawingSession.DrawLine(x0, y0, x1, y1, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a thick line.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point0"> The first point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawThickLine(this CanvasDrawingSession drawingSession, Vector2 point0, Vector2 point1)
        {
            drawingSession.DrawLine(point0, point1, Windows.UI.Colors.Black, 2);
            drawingSession.DrawLine(point0, point1, Windows.UI.Colors.White);
        }

        /// <summary>
        /// Draw a thick line.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x0"> The X-axis value of the first point. </param>
        /// <param name="y0"> The Y-axis position of the first point. </param>
        /// <param name="x1"> The X-axis value of the second point. </param>
        /// <param name="y1"> The Y-axis position of the second point. </param>
        public static void DrawThickLine(this CanvasDrawingSession drawingSession, float x0, float y0, float x1, float y1)
        {
            drawingSession.DrawLine(x0, y0, x1, y1, Windows.UI.Colors.Black, 2);
            drawingSession.DrawLine(x0, y0, x1, y1, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ellipse and the DodgerBlue color.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="radiusX"> The X-axis radius of the ellipse. </param>
        /// <param name="radiusY"> The Y-axis radius of the ellipse. </param>
        public static void DrawEllipseDodgerBlue(this CanvasDrawingSession drawingSession, Vector2 centerPoint, float radiusX, float radiusY)
        {
            drawingSession.DrawEllipse(centerPoint, radiusX, radiusY, Windows.UI.Colors.DodgerBlue);
            drawingSession.FillEllipse(centerPoint, radiusX, radiusY, CanvasDrawingSessionExtensions.TranslucentDodgerBlue);
        }

        /// <summary>
        /// Draw a thick ellipse.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="radiusX"> The X-axis radius of the ellipse. </param>
        /// <param name="radiusY"> The Y-axis radius of the ellipse. </param>
        public static void DrawThickEllipse(this CanvasDrawingSession drawingSession, Vector2 centerPoint, float radiusX, float radiusY)
        {
            drawingSession.DrawEllipse(centerPoint, radiusX, radiusY, Windows.UI.Colors.Black, 2);
            drawingSession.DrawEllipse(centerPoint, radiusX, radiusY, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a rectangle and the DodgerBlue color.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="rect"> The rectangle. </param>
        public static void DrawRectangleDodgerBlue(this CanvasDrawingSession drawingSession, Rect rect)
        {
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.DodgerBlue);
            drawingSession.FillRectangle(rect, CanvasDrawingSessionExtensions.TranslucentDodgerBlue);
        }

        /// <summary>
        /// Draw a thick rectangle.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="rect"> The rectangle. </param>
        public static void DrawThickRectangle(this CanvasDrawingSession drawingSession, Rect rect)
        {
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.Black, 2);
            drawingSession.DrawRectangle(rect, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a geometry and the DodgerBlue color.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasGeometry"> The canvas-geometry. </param>
        public static void DrawGeometryDodgerBlue(this CanvasDrawingSession drawingSession, CanvasGeometry canvasGeometry)
        {
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
            drawingSession.FillGeometry(canvasGeometry, CanvasDrawingSessionExtensions.TranslucentDodgerBlue);
        }

        /// <summary>
        /// Draw a thick geometry.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasGeometry"> The canvas-geometry. </param>
        public static void DrawThickGeometry(this CanvasDrawingSession drawingSession, CanvasGeometry canvasGeometry)
        {
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.Black, 2);
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.White);
        }

    }
}