﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {

        /// <summary> 0.27614f </summary>
        const float Z276 = 0.276114f;
        /// <summary> 0.55228f </summary>
        const float Z552 = 0.55228f;

        /// <summary> Gets top vector. </summary>
        public const float StartingRotation = -FanKit.Math.Pi / 2.0f;

        /// <summary>
        /// Get unit vectors from an rotation.
        /// </summary>
        /// <param name="rotation"> The source rotation. </param>
        /// <returns> The unit vector. </returns>
        public static Vector2 GetRotationVector(float rotation) => new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));


        #region Rectangle


        /// <summary>
        /// Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Transformer bounds) => TransformerGeometry.CreateRectangleCore(resourceCreator, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom);

        /// <summary>
        /// Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Transformer bounds, Matrix3x2 matrix) => TransformerGeometry.CreateRectangle(resourceCreator, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom, matrix);

        /// <summary>
        /// Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The position of the top-left corner of the bounds. </param>
        /// <param name="rightTop"> The position of the top-right corner of the bounds. </param>
        /// <param name="rightBottom"> The position of the bottom-right corner of the bounds. </param>
        /// <param name="leftBottom"> The position of the bottom-left corner of the bounds. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom) => TransformerGeometry.CreateRectangleCore(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);

        /// <summary>
        /// Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The position of the top-left corner of the bounds. </param>
        /// <param name="rightTop"> The position of the top-right corner of the bounds. </param>
        /// <param name="rightBottom"> The position of the bottom-right corner of the bounds. </param>
        /// <param name="leftBottom"> The position of the bottom-left corner of the bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix) => TransformerGeometry.CreateRectangleCore(resourceCreator, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix));

        private static CanvasGeometry CreateRectangleCore(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            // Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom,
            };

            // Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        #endregion


        #region Ellipse


        /// <summary>
        /// Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, Transformer bounds) => TransformerGeometry.CreateEllipseCore(resourceCreator, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom);

        /// <summary>
        /// Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, Transformer bounds, Matrix3x2 matrix) => TransformerGeometry.CreateEllipse(resourceCreator, bounds.CenterLeft, bounds.CenterTop, bounds.CenterRight, bounds.CenterBottom, matrix);

        /// <summary>
        /// Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="centerLeft"> The position of the center of bottom-left and top-left corners of the bounds. </param>
        /// <param name="centerTop"> The position of the center of top-left and top-right corners of the bounds. </param>
        /// <param name="centerRight"> The position of the center of top-right and bottom-right corners of the bounds. </param>
        /// <param name="centerBottom"> The position of the center of bottom-right and bottom-left corners of the bounds. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom) => TransformerGeometry.CreateEllipseCore(resourceCreator, centerLeft, centerTop, centerRight, centerBottom);

        /// <summary>
        /// Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="centerLeft"> The position of the center of bottom-left and top-left corners of the bounds. </param>
        /// <param name="centerTop"> The position of the center of top-left and top-right corners of the bounds. </param>
        /// <param name="centerRight"> The position of the center of top-right and bottom-right corners of the bounds. </param>
        /// <param name="centerBottom"> The position of the center of bottom-right and bottom-left corners of the bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom, Matrix3x2 matrix) => TransformerGeometry.CreateEllipseCore(resourceCreator, Vector2.Transform(centerLeft, matrix), Vector2.Transform(centerTop, matrix), Vector2.Transform(centerRight, matrix), Vector2.Transform(centerBottom, matrix));

        private static CanvasGeometry CreateEllipseCore(ICanvasResourceCreator resourceCreator, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            // A Ellipse has left, top, right, bottom four nodes.
            // 
            // Control points on the left and right sides of the node.
            // 
            // The distance of the control point 
            // is 0.552f times
            // the length of the square edge.

            // HV
            Vector2 horizontal = (centerRight - centerLeft);
            Vector2 horizontal276 = horizontal * Z276; // vector * Z552 / 2

            Vector2 vertical = (centerBottom - centerTop);
            Vector2 vertical276 = vertical * Z276; // vector * Z552 / 2

            // Control
            Vector2 left1 = centerLeft + vertical276;
            Vector2 left2 = centerLeft - vertical276;
            Vector2 top1 = centerTop - horizontal276;
            Vector2 top2 = centerTop + horizontal276;
            Vector2 right1 = centerRight - vertical276;
            Vector2 right2 = centerRight + vertical276;
            Vector2 bottom1 = centerBottom + horizontal276;
            Vector2 bottom2 = centerBottom - horizontal276;

            // Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(centerBottom);
            pathBuilder.AddCubicBezier(bottom2, left1, centerLeft);
            pathBuilder.AddCubicBezier(left2, top1, centerTop);
            pathBuilder.AddCubicBezier(top2, right1, centerRight);
            pathBuilder.AddCubicBezier(right2, bottom1, centerBottom);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            // Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }


        #endregion

    }
}