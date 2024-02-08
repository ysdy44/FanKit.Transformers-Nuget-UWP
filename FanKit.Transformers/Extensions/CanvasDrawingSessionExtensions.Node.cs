using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class CanvasDrawingSessionExtensions
    {
        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode(this CanvasDrawingSession drawingSession, Vector2 point)
        {
            drawingSession.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillCircle(point, 8, Windows.UI.Colors.DodgerBlue);
            drawingSession.FillCircle(point, 6, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor)
        {
            drawingSession.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillCircle(point, 8, accentColor);
            drawingSession.FillCircle(point, 6, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ◉.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode2(this CanvasDrawingSession drawingSession, Vector2 point)
        {
            drawingSession.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillCircle(point, 8, Windows.UI.Colors.White);
            drawingSession.FillCircle(point, 6, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Draw a ◉.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode2(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor)
        {
            drawingSession.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillCircle(point, 8, Windows.UI.Colors.White);
            drawingSession.FillCircle(point, 6, accentColor);
        }


        /// <summary>
        /// Draw a ◻.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode3(this CanvasDrawingSession drawingSession, Vector2 point)
        {
            drawingSession.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.DodgerBlue);
            drawingSession.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ◻.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode3(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor)
        {
            drawingSession.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillRectangle(point.X - 6, point.Y - 6, 12, 12, accentColor);
            drawingSession.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ◼.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode4(this CanvasDrawingSession drawingSession, Vector2 point)
        {
            drawingSession.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.White);
            drawingSession.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Draw a ◼.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode4(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor)
        {
            drawingSession.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.White);
            drawingSession.FillRectangle(point.X - 5, point.Y - 5, 10, 10, accentColor);
        }


        /// <summary>
        /// Draw a ◌.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode5(this CanvasDrawingSession drawingSession, Vector2 point)
        {
            drawingSession.FillCircle(point, 6, Windows.UI.Colors.DodgerBlue);
            drawingSession.FillCircle(point, 5, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ◌.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode5(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor)
        {
            drawingSession.FillCircle(point, 6, accentColor);
            drawingSession.FillCircle(point, 5, Windows.UI.Colors.White);
        }
    }
}
