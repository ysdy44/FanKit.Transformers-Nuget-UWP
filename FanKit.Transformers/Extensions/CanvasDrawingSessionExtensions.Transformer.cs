using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {

        private static void _drawBound(CanvasDrawingSession drawingSession, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor)
        {
            drawingSession.DrawLine(leftTop, rightTop, accentColor);
            drawingSession.DrawLine(rightTop, rightBottom, accentColor);
            drawingSession.DrawLine(rightBottom, leftBottom, accentColor);
            drawingSession.DrawLine(leftBottom, leftTop, accentColor);
        }

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer) => CanvasDrawingSessionExtensions._drawBound(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix) => CanvasDrawingSessionExtensions._drawBound(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue);


        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions._drawBound(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, accentColor);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions._drawBound(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), accentColor);



        private static void _drawBoundNodes(CanvasDrawingSession drawingSession, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor, bool disabledRadian)
        {
            //Line
            drawingSession.DrawThickLine(leftTop, rightTop);
            drawingSession.DrawThickLine(rightTop, rightBottom);
            drawingSession.DrawThickLine(rightBottom, leftBottom);
            drawingSession.DrawThickLine(leftBottom, leftTop);

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale2
            drawingSession.DrawNode2(leftTop);
            drawingSession.DrawNode2(rightTop);
            drawingSession.DrawNode2(rightBottom);
            drawingSession.DrawNode2(leftBottom);

            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Math.GetOutsidePointInTransformer(centerLeft, centerRight);
                Vector2 outsideTop = Math.GetOutsidePointInTransformer(centerTop, centerBottom);
                Vector2 outsideRight = Math.GetOutsidePointInTransformer(centerRight, centerLeft);
                Vector2 outsideBottom = Math.GetOutsidePointInTransformer(centerBottom, centerTop);

                //Radian
                drawingSession.DrawThickLine(outsideTop, centerTop);
                drawingSession.DrawNode(outsideTop);

                //Skew
                //drawingSession.DrawNode2(drawingSession, outsideTop);
                //drawingSession.DrawNode2(drawingSession, outsideLeft);
                drawingSession.DrawNode2(outsideRight);
                drawingSession.DrawNode2(outsideBottom);
            }

            //Scale1
            if (Math.OutNodeDistance(centerLeft, centerRight))
            {
                drawingSession.DrawNode2(centerTop);
                drawingSession.DrawNode2(centerBottom);
            }
            if (Math.OutNodeDistance(centerTop, centerBottom))
            {
                drawingSession.DrawNode2(centerLeft);
                drawingSession.DrawNode2(centerRight);
            }
        }

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, bool disabledRadian = false) => CanvasDrawingSessionExtensions._drawBoundNodes(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, Windows.UI.Colors.DodgerBlue, disabledRadian);

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false) => CanvasDrawingSessionExtensions._drawBoundNodes(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue, disabledRadian);


        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, Windows.UI.Color accentColor, bool disabledRadian = false) => CanvasDrawingSessionExtensions._drawBoundNodes(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, accentColor, disabledRadian);

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor, bool disabledRadian = false) => CanvasDrawingSessionExtensions._drawBoundNodes(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), accentColor, disabledRadian);

    }
}