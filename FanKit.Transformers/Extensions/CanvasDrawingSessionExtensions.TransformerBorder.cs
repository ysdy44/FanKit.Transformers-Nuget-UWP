using Microsoft.Graphics.Canvas;

namespace FanKit.Transformers
{
    partial class CanvasDrawingSessionExtensions
    {

        #region DrawBound


        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="border"> The border. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, TransformerBorder border) => CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, border, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="border"> The border. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, TransformerBorder border, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, border, accentColor);


        private static void DrawBoundCore(CanvasDrawingSession drawingSession, TransformerBorder border, Windows.UI.Color accentColor)
        {
            drawingSession.DrawLine(border.Left, border.Top, border.Right, border.Top, accentColor);
            drawingSession.DrawLine(border.Right, border.Top, border.Right, border.Bottom, accentColor);
            drawingSession.DrawLine(border.Right, border.Bottom, border.Left, border.Bottom, accentColor);
            drawingSession.DrawLine(border.Left, border.Bottom, border.Left, border.Top, accentColor);
        }


        #endregion


        #region DrawBoundNodes


        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="border"> The border. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, TransformerBorder border) => CanvasDrawingSessionExtensions.DrawBoundNodesCore(drawingSession, border, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="border"> The border. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, TransformerBorder border, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawBoundNodesCore(drawingSession, border, accentColor);


        private static void DrawBoundNodesCore(CanvasDrawingSession drawingSession, TransformerBorder border, Windows.UI.Color accentColor)
        {
            // Line
            CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, border, accentColor);

            drawingSession.DrawNode2(border.Left, border.Top, accentColor);
            drawingSession.DrawNode2(border.Right, border.Top, accentColor);
            drawingSession.DrawNode2(border.Right, border.Bottom, accentColor);
            drawingSession.DrawNode2(border.Left, border.Bottom, accentColor);

            if (System.MathF.Abs(border.Right - border.Left) > FanKit.Math.NodeDistance)
            {
                drawingSession.DrawNode2(border.CenterX, border.Top, accentColor);
                drawingSession.DrawNode2(border.CenterX, border.Bottom, accentColor);
            }

            if (System.MathF.Abs(border.Bottom - border.Top) > FanKit.Math.NodeDistance)
            {
                drawingSession.DrawNode2(border.Left, border.CenterY, accentColor);
                drawingSession.DrawNode2(border.Right, border.CenterY, accentColor);
            }
        }


        #endregion


        #region DrawCrop


        /// <summary>
        /// Draw box and lines on bound，just like 🔳.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="border"> The transformer. </param>
        public static void DrawCrop(this CanvasDrawingSession drawingSession, TransformerBorder border) => CanvasDrawingSessionExtensions.DrawCropCore(drawingSession, border, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw box and lines on bound，just like 🔳.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="border"> The border. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawCrop(this CanvasDrawingSession drawingSession, TransformerBorder border, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawCropCore(drawingSession, border, accentColor);


        private static void DrawCropCore(CanvasDrawingSession drawingSession, TransformerBorder border, Windows.UI.Color accentColor)
        {
            // Line
            CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, border, accentColor);

            // Scale2
            {
                drawingSession.FillCircle(border.Left, border.Top, 10f, accentColor);
                drawingSession.FillCircle(border.Left, border.Top, 9f, Windows.UI.Colors.White);

                drawingSession.FillCircle(border.Right, border.Top, 10f, accentColor);
                drawingSession.FillCircle(border.Right, border.Top, 9f, Windows.UI.Colors.White);

                drawingSession.FillCircle(border.Right, border.Bottom, 10f, accentColor);
                drawingSession.FillCircle(border.Right, border.Bottom, 9f, Windows.UI.Colors.White);

                drawingSession.FillCircle(border.Left, border.Bottom, 10f, accentColor);
                drawingSession.FillCircle(border.Left, border.Bottom, 9f, Windows.UI.Colors.White);
            }

            // Scale1
            if (System.MathF.Abs(border.Right - border.Left) > FanKit.Math.NodeDistance)
            {
                drawingSession.FillCircle(border.CenterX, border.Top, 10f, accentColor);
                drawingSession.FillCircle(border.CenterX, border.Top, 9f, Windows.UI.Colors.White);
                drawingSession.FillCircle(border.CenterX, border.Bottom, 10f, accentColor);
                drawingSession.FillCircle(border.CenterX, border.Bottom, 9f, Windows.UI.Colors.White);
            }
            if (System.MathF.Abs(border.Bottom - border.Top) > FanKit.Math.NodeDistance)
            {
                drawingSession.FillCircle(border.Left, border.CenterY, 10f, accentColor);
                drawingSession.FillCircle(border.Left, border.CenterY, 9f, Windows.UI.Colors.White);
                drawingSession.FillCircle(border.Right, border.CenterY, 10f, accentColor);
                drawingSession.FillCircle(border.Right, border.CenterY, 9f, Windows.UI.Colors.White);
            }
        }


        #endregion

    }
}