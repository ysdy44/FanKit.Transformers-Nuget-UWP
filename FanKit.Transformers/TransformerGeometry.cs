using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static class TransformerGeometry
    {
        //@Static
        
        #region Rectangle


        /// <summary>
        ///  Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformerLTRB) => TransformerGeometry._createRectangle(resourceCreator, transformerLTRB.LeftTop, transformerLTRB.RightTop, transformerLTRB.RightBottom, transformerLTRB.LeftBottom);

        /// <summary>
        ///  Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformerLTRB, Matrix3x2 matrix) => TransformerGeometry._createRectangle(resourceCreator, Vector2.Transform(transformerLTRB.LeftTop, matrix), Vector2.Transform(transformerLTRB.RightTop, matrix), Vector2.Transform(transformerLTRB.RightBottom, matrix), Vector2.Transform(transformerLTRB.LeftBottom, matrix));

        /// <summary>
        ///  Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom) => TransformerGeometry._createRectangle(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);

        /// <summary>
        ///  Create a new rectangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix) => TransformerGeometry._createRectangle(resourceCreator, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix));

        private static CanvasGeometry _createRectangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom,
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        #endregion


        #region Ellipse


        /// <summary>
        ///  Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformerLTRB) => TransformerGeometry._createEllipse(resourceCreator, transformerLTRB.CenterLeft, transformerLTRB.CenterTop, transformerLTRB.CenterRight, transformerLTRB.CenterBottom);

        /// <summary>
        ///  Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformerLTRB, Matrix3x2 matrix) => TransformerGeometry._createEllipse(resourceCreator, Vector2.Transform(transformerLTRB.CenterLeft, matrix), Vector2.Transform(transformerLTRB.CenterTop, matrix), Vector2.Transform(transformerLTRB.CenterRight, matrix), Vector2.Transform(transformerLTRB.CenterBottom, matrix));

        /// <summary>
        ///  Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="centerLeft"> The left point. </param>
        /// <param name="centerTop"> The top point. </param>
        /// <param name="centerRight"> The right point. </param>
        /// <param name="centerBottom"> The bottom point. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom) => TransformerGeometry._createEllipse(resourceCreator, centerLeft, centerTop, centerRight, centerBottom);

        /// <summary>
        ///  Create a new ellipse geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="centerLeft"> The left point. </param>
        /// <param name="centerTop"> The top point. </param>
        /// <param name="centerRight"> The right point. </param>
        /// <param name="centerBottom"> The bottom point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator,             Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom, Matrix3x2 matrix)             => TransformerGeometry._createEllipse(resourceCreator, Vector2.Transform(centerLeft, matrix), Vector2.Transform(centerTop, matrix), Vector2.Transform(centerRight, matrix), Vector2.Transform(centerBottom, matrix));

        private static CanvasGeometry _createEllipse(ICanvasResourceCreator resourceCreator, Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            // A Ellipse has left, top, right, bottom four nodes.
            // 
            // Control points on the left and right sides of the node.
            // 
            // The distance of the control point 
            // is 0.552f times
            // the length of the square edge.

            //HV
            Vector2 horizontal = (centerRight - centerLeft);
            Vector2 horizontal276 = horizontal * 0.276f;// vector * 0.552f / 2

            Vector2 vertical = (centerBottom - centerTop) ;
            Vector2 vertical276 = vertical * 0.276f;// vector * 0.552f / 2

            //Control
            Vector2 left1 = centerLeft - vertical276;
            Vector2 left2 = centerLeft + vertical276;
            Vector2 top1 = centerTop + horizontal276;
            Vector2 top2 = centerTop - horizontal276;
            Vector2 right1 = centerRight + vertical276;
            Vector2 right2 = centerRight - vertical276;
            Vector2 bottom1 = centerBottom - horizontal276;
            Vector2 bottom2 = centerBottom + horizontal276;

            //Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(centerBottom);
            pathBuilder.AddCubicBezier(bottom1, left2, centerLeft);
            pathBuilder.AddCubicBezier(left1, top2, centerTop);
            pathBuilder.AddCubicBezier(top1, right2, centerRight);
            pathBuilder.AddCubicBezier(right1, bottom2, centerBottom);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }


        #endregion

    }
}