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
        public static void DrawNode(this CanvasDrawingSession drawingSession, Vector2 point) => CanvasDrawingSessionExtensions.DrawNode1Core(drawingSession, point.X, point.Y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        public static void DrawNode(this CanvasDrawingSession drawingSession, float x, float y) => CanvasDrawingSessionExtensions.DrawNode1Core(drawingSession, x, y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode1Core(drawingSession, point.X, point.Y, accentColor);

        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        public static void DrawNode(this CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode1Core(drawingSession, x, y, accentColor);

        private static void DrawNode1Core(CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor)
        {
            drawingSession.FillCircle(x, y, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillCircle(x, y, 8, accentColor);
            drawingSession.FillCircle(x, y, 6, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ◉.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode2(this CanvasDrawingSession drawingSession, Vector2 point) => CanvasDrawingSessionExtensions.DrawNode2Core(drawingSession, point.X, point.Y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◉.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        public static void DrawNode2(this CanvasDrawingSession drawingSession, float x, float y) => CanvasDrawingSessionExtensions.DrawNode2Core(drawingSession, x, y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◉.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode2(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode2Core(drawingSession, point.X, point.Y, accentColor);

        /// <summary>
        /// Draw a ◉.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode2(this CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode2Core(drawingSession, x, y, accentColor);

        private static void DrawNode2Core(CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor)
        {
            drawingSession.FillCircle(x, y, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillCircle(x, y, 8, Windows.UI.Colors.White);
            drawingSession.FillCircle(x, y, 6, accentColor);
        }


        /// <summary>
        /// Draw a ◻.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode3(this CanvasDrawingSession drawingSession, Vector2 point) => CanvasDrawingSessionExtensions.DrawNode3Core(drawingSession, point.X, point.Y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◻.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        public static void DrawNode3(this CanvasDrawingSession drawingSession, float x, float y) => CanvasDrawingSessionExtensions.DrawNode3Core(drawingSession, x, y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◻.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode3(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode3Core(drawingSession, point.X, point.Y, accentColor);

        /// <summary>
        /// Draw a ◻.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode3(this CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode3Core(drawingSession, x, y, accentColor);

        private static void DrawNode3Core(CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor)
        {
            drawingSession.FillRectangle(x - 7, y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillRectangle(x - 6, y - 6, 12, 12, accentColor);
            drawingSession.FillRectangle(x - 5, y - 5, 10, 10, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ◼.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode4(this CanvasDrawingSession drawingSession, Vector2 point) => CanvasDrawingSessionExtensions.DrawNode4Core(drawingSession, point.X, point.Y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◼.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        public static void DrawNode4(this CanvasDrawingSession drawingSession, float x, float y) => CanvasDrawingSessionExtensions.DrawNode4Core(drawingSession, x, y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◼.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode4(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode4Core(drawingSession, point.X, point.Y, accentColor);

        /// <summary>
        /// Draw a ◼.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode4(this CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode4Core(drawingSession, x, y, accentColor);

        private static void DrawNode4Core(CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor)
        {
            drawingSession.FillRectangle(x - 7, y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            drawingSession.FillRectangle(x - 6, y - 6, 12, 12, Windows.UI.Colors.White);
            drawingSession.FillRectangle(x - 5, y - 5, 10, 10, accentColor);
        }


        /// <summary>
        /// Draw a ◌.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode5(this CanvasDrawingSession drawingSession, Vector2 point) => CanvasDrawingSessionExtensions.DrawNode5Core(drawingSession, point.X, point.Y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◌.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        public static void DrawNode5(this CanvasDrawingSession drawingSession, float x, float y) => CanvasDrawingSessionExtensions.DrawNode5Core(drawingSession, x, y, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw a ◌.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode5(this CanvasDrawingSession drawingSession, Vector2 point, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode5Core(drawingSession, point.X, point.Y, accentColor);

        /// <summary>
        /// Draw a ◌.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="x"> The X-axis value of the point. </param>
        /// <param name="y"> The Y-axis position of the point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode5(this CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawNode5Core(drawingSession, x, y, accentColor);

        private static void DrawNode5Core(CanvasDrawingSession drawingSession, float x, float y, Windows.UI.Color accentColor)
        {
            drawingSession.FillCircle(x, y, 6, accentColor);
            drawingSession.FillCircle(x, y, 5, Windows.UI.Colors.White);
        }
    }
}