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
        /// Draw a geometry.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point0"> The canvas-geometry. </param>
        public static void DrawThickGeometry(this CanvasDrawingSession drawingSession, CanvasGeometry canvasGeometry)
        {
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.Black, 2);
            drawingSession.DrawGeometry(canvasGeometry, Windows.UI.Colors.White);
        }

        
    }
}