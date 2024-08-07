﻿using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class CanvasDrawingSessionExtensions
    {

        #region DrawBound


        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer) => CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix) => CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, accentColor);

        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), accentColor);


        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static void DrawBoundCore(CanvasDrawingSession drawingSession, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor)
        {
            drawingSession.DrawLine(leftTop, rightTop, accentColor);
            drawingSession.DrawLine(rightTop, rightBottom, accentColor);
            drawingSession.DrawLine(rightBottom, leftBottom, accentColor);
            drawingSession.DrawLine(leftBottom, leftTop, accentColor);
        }


        #endregion


        #region DrawBoundNodes


        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, bool disabledRadian = false) => CanvasDrawingSessionExtensions.DrawBoundNodesCore(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, Windows.UI.Colors.DodgerBlue, disabledRadian);

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false) => CanvasDrawingSessionExtensions.DrawBoundNodesCore(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue, disabledRadian);

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, Windows.UI.Color accentColor, bool disabledRadian = false) => CanvasDrawingSessionExtensions.DrawBoundNodesCore(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, accentColor, disabledRadian);

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor, bool disabledRadian = false) => CanvasDrawingSessionExtensions.DrawBoundNodesCore(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), accentColor, disabledRadian);


        private static void DrawBoundNodesCore(CanvasDrawingSession drawingSession, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor, bool disabledRadian)
        {
            // Line
            CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, leftTop, rightTop, rightBottom, leftBottom, accentColor);

            // Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            // Scale2
            drawingSession.DrawNode2(leftTop, accentColor);
            drawingSession.DrawNode2(rightTop, accentColor);
            drawingSession.DrawNode2(rightBottom, accentColor);
            drawingSession.DrawNode2(leftBottom, accentColor);

            if (disabledRadian == false)
            {
                // Outside
                Vector2 outsideLeft = Math.GetOutsidePointInTransformer(centerLeft, centerRight);
                Vector2 outsideTop = Math.GetOutsidePointInTransformer(centerTop, centerBottom);
                Vector2 outsideRight = Math.GetOutsidePointInTransformer(centerRight, centerLeft);
                Vector2 outsideBottom = Math.GetOutsidePointInTransformer(centerBottom, centerTop);

                // Radian
                drawingSession.DrawThickLine(outsideTop, centerTop);
                drawingSession.DrawNode(outsideTop, accentColor);

                // Skew
                // drawingSession.DrawNode2(outsideTop, accentColor);
                // drawingSession.DrawNode2(outsideLeft, accentColor);
                drawingSession.DrawNode2(outsideRight, accentColor);
                drawingSession.DrawNode2(outsideBottom, accentColor);
            }

            // Scale1
            if (Math.OutNodeDistance(centerLeft, centerRight))
            {
                drawingSession.DrawNode2(centerTop, accentColor);
                drawingSession.DrawNode2(centerBottom, accentColor);
            }
            if (Math.OutNodeDistance(centerTop, centerBottom))
            {
                drawingSession.DrawNode2(centerLeft, accentColor);
                drawingSession.DrawNode2(centerRight, accentColor);
            }
        }


        #endregion


        #region DrawCrop


        /// <summary>
        /// Draw box and lines on bound，just like 🔳.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        public static void DrawCrop(this CanvasDrawingSession drawingSession, Transformer transformer) => CanvasDrawingSessionExtensions.DrawCropCore(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw box and lines on bound，just like 🔳.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void DrawCrop(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix) => CanvasDrawingSessionExtensions.DrawCropCore(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue);

        /// <summary>
        /// Draw box and lines on bound，just like 🔳.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawCrop(this CanvasDrawingSession drawingSession, Transformer transformer, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawCropCore(drawingSession, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, accentColor);

        /// <summary>
        /// Draw box and lines on bound，just like 🔳.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="transformer"> The transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawCrop(this CanvasDrawingSession drawingSession, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions.DrawCropCore(drawingSession, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), accentColor);


        private static void DrawCropCore(CanvasDrawingSession drawingSession, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor)
        {
            // Line
            CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, leftTop, rightTop, rightBottom, leftBottom, accentColor);

            // Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            // Vertical Horizontal
            /*
            Vector2 vertical = centerBottom - centerTop;
            Vector2 horizontal = centerRight - centerLeft;

            Vector2 verticalUnit = vertical / vertical.Length();
            Vector2 horizontalUnit = horizontal / horizontal.Length();

            const float length = 10;
            Vector2 verticalLength = verticalUnit * length;
            Vector2 horizontalLength = horizontalUnit * length;

            const float space = 2;
            Vector2 verticalSpace = verticalUnit * space;
            Vector2 horizontalSpace = horizontalUnit * space;
             */

            // Scale2
            {
                drawingSession.FillCircle(leftTop, 10, accentColor);
                drawingSession.FillCircle(leftTop, 9, Windows.UI.Colors.White);

                drawingSession.FillCircle(rightTop, 10, accentColor);
                drawingSession.FillCircle(rightTop, 9, Windows.UI.Colors.White);

                drawingSession.FillCircle(rightBottom, 10, accentColor);
                drawingSession.FillCircle(rightBottom, 9, Windows.UI.Colors.White);

                drawingSession.FillCircle(leftBottom, 10, accentColor);
                drawingSession.FillCircle(leftBottom, 9, Windows.UI.Colors.White);
            }

            // Scale1
            if (FanKit.Math.OutNodeDistance(centerLeft, centerRight))
            {
                drawingSession.FillCircle(centerTop, 10, accentColor);
                drawingSession.FillCircle(centerTop, 9, Windows.UI.Colors.White);

                drawingSession.FillCircle(centerBottom, 10, accentColor);
                drawingSession.FillCircle(centerBottom, 9, Windows.UI.Colors.White);
            }
            if (FanKit.Math.OutNodeDistance(centerTop, centerBottom))
            {
                drawingSession.FillCircle(centerLeft, 10, accentColor);
                drawingSession.FillCircle(centerLeft, 9, Windows.UI.Colors.White);

                drawingSession.FillCircle(centerRight, 10, accentColor);
                drawingSession.FillCircle(centerRight, 9, Windows.UI.Colors.White);
            }
        }


        //private static void DrawCrop2Core(CanvasDrawingSession drawingSession, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor)
        //{
        //    // Line
        //    CanvasDrawingSessionExtensions.DrawBoundCore(drawingSession, leftTop, rightTop, rightBottom, leftBottom, accentColor);

        //    // Center
        //    Vector2 centerLeft = (leftTop + leftBottom) / 2;
        //    Vector2 centerTop = (leftTop + rightTop) / 2;
        //    Vector2 centerRight = (rightTop + rightBottom) / 2;
        //    Vector2 centerBottom = (leftBottom + rightBottom) / 2;

        //    // Vertical Horizontal
        //    Vector2 vertical = centerBottom - centerTop;
        //    Vector2 horizontal = centerRight - centerLeft;

        //    Vector2 verticalUnit = vertical / vertical.Length();
        //    Vector2 horizontalUnit = horizontal / horizontal.Length();

        //    const float length = 10;
        //    Vector2 verticalLength = verticalUnit * length;
        //    Vector2 horizontalLength = horizontalUnit * length;

        //    const float space = 2;
        //    Vector2 verticalSpace = verticalUnit * space;
        //    Vector2 horizontalSpace = horizontalUnit * space;

        //    // Scale2
        //    {
        //        const float strokeWidth = 2;
        //        Vector2 leftTopOutside = leftTop - verticalSpace - horizontalSpace;
        //        Vector2 rightTopOutside = rightTop - verticalSpace + horizontalSpace;
        //        Vector2 rightBottomOutside = rightBottom + verticalSpace + horizontalSpace;
        //        Vector2 leftBottomOutside = leftBottom + verticalSpace - horizontalSpace;

        //        drawingSession.DrawLine(leftTopOutside, leftTopOutside + horizontalLength, accentColor, strokeWidth);
        //        drawingSession.DrawLine(leftTopOutside, leftTopOutside + verticalLength, accentColor, strokeWidth);

        //        drawingSession.DrawLine(rightTopOutside, rightTopOutside - horizontalLength, accentColor, strokeWidth);
        //        drawingSession.DrawLine(rightTopOutside, rightTopOutside + verticalLength, accentColor, strokeWidth);

        //        drawingSession.DrawLine(rightBottomOutside, rightBottomOutside - horizontalLength, accentColor, strokeWidth);
        //        drawingSession.DrawLine(rightBottomOutside, rightBottomOutside - verticalLength, accentColor, strokeWidth);

        //        drawingSession.DrawLine(leftBottomOutside, leftBottomOutside + horizontalLength, accentColor, strokeWidth);
        //        drawingSession.DrawLine(leftBottomOutside, leftBottomOutside - verticalLength, accentColor, strokeWidth);
        //    }

        //    // Scale1
        //    if (FanKit.Math.OutNodeDistance(centerLeft, centerRight))
        //    {
        //        Vector2 centerTopOutside = centerTop - verticalSpace;
        //        Vector2 centerBottomOutside = centerBottom + verticalSpace;
        //        drawingSession.DrawLine(centerTopOutside + horizontalLength, centerTopOutside - horizontalLength, accentColor, 2);
        //        drawingSession.DrawLine(centerBottomOutside + horizontalLength, centerBottomOutside - horizontalLength, accentColor, 2);
        //    }
        //    if (FanKit.Math.OutNodeDistance(centerTop, centerBottom))
        //    {
        //        Vector2 centerLeftOutside = centerLeft - horizontalSpace;
        //        Vector2 centerRightOutside = centerRight + horizontalSpace;
        //        drawingSession.DrawLine(centerLeftOutside + verticalLength, centerLeftOutside - verticalLength, accentColor, 2);
        //        drawingSession.DrawLine(centerRightOutside + verticalLength, centerRightOutside - verticalLength, accentColor, 2);
        //    }
        //}


        #endregion

    }
}