using Microsoft.Graphics.Canvas;
using System.Numerics;


namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {
        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, Windows.UI.Colors.DodgerBlue);
            ds.FillCircle(point, 6, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, accentColor);
            ds.FillCircle(point, 6, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ●.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode2(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, Windows.UI.Colors.White);
            ds.FillCircle(point, 6, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Draw a ●.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode2(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillCircle(point, 10, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillCircle(point, 8, Windows.UI.Colors.White);
            ds.FillCircle(point, 6, accentColor);
        }


        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode3(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.DodgerBlue);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.White);
        }
        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode3(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, accentColor);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.White);
        }


        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        public static void DrawNode4(this CanvasDrawingSession ds, Vector2 point)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.White);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, Windows.UI.Colors.DodgerBlue);
        }
        /// <summary>
        /// Draw a ロ.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point"> The point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNode4(this CanvasDrawingSession ds, Vector2 point, Windows.UI.Color accentColor)
        {
            ds.FillRectangle(point.X - 7, point.Y - 7, 14, 14, Windows.UI.Color.FromArgb(70, 127, 127, 127));
            ds.FillRectangle(point.X - 6, point.Y - 6, 12, 12, Windows.UI.Colors.White);
            ds.FillRectangle(point.X - 5, point.Y - 5, 10, 10, accentColor);
        }
    }
}
