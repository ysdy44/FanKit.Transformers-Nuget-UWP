using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {
			   
        #region Rectangle


        /// <summary>
        ///  Convert to curves from rectangle.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromRectangle(ITransformerLTRB transformerLTRB) => TransformerGeometry._convertToCurvesFromRectangle
        (
            transformerLTRB.LeftTop, 
            transformerLTRB.RightTop, 
            transformerLTRB.RightBottom, 
            transformerLTRB.LeftBottom
        );

        /// <summary>
        ///  Convert to curves from rectangle.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromRectangle(ITransformerLTRB transformerLTRB, Matrix3x2 matrix) => TransformerGeometry.ConvertToCurvesFromRectangle
        (
            Vector2.Transform(transformerLTRB.LeftTop, matrix),
            Vector2.Transform(transformerLTRB.RightTop, matrix),
            Vector2.Transform(transformerLTRB.RightBottom, matrix),
            Vector2.Transform(transformerLTRB.LeftBottom, matrix)
        );

        /// <summary>
        ///  Convert to curves from rectangle.
        /// </summary>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromRectangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom) => TransformerGeometry._convertToCurvesFromRectangle
        (
            leftTop,
            rightTop,
            rightBottom,
            leftBottom
        );

        /// <summary>
        ///  Convert to curves from rectangle.
        /// </summary>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromRectangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix) => TransformerGeometry._convertToCurvesFromRectangle
        (
            Vector2.Transform(leftTop, matrix), 
            Vector2.Transform(rightTop, matrix),
            Vector2.Transform(rightBottom, matrix), 
            Vector2.Transform(leftBottom, matrix)
        );

        private static IEnumerable<IEnumerable<Node>> _convertToCurvesFromRectangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromRectangle(leftTop, rightTop, rightBottom, leftBottom)
            };
        }
        private static IEnumerable<Node> _convertToCurveFromRectangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            return  new List<Node>
            {
                new Node { Point=leftTop },
                new Node { Point=rightTop },
                new Node { Point=rightBottom },
                new Node { Point=leftBottom },
                new Node { Point=leftTop },
            };
        }


        #endregion


        #region Ellipse


        /// <summary>
        ///  Convert to curves from ellipse.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromEllipse(ITransformerLTRB transformerLTRB) => TransformerGeometry._convertToCurvesFromEllipse
        (
            transformerLTRB.CenterLeft, 
            transformerLTRB.CenterTop, 
            transformerLTRB.CenterRight,
            transformerLTRB.CenterBottom
        );

        /// <summary>
        ///  Convert to curves from ellipse.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromEllipse(ITransformerLTRB transformerLTRB, Matrix3x2 matrix) => TransformerGeometry.ConvertToCurvesFromEllipse
        (
            Vector2.Transform(transformerLTRB.CenterLeft, matrix),
            Vector2.Transform(transformerLTRB.CenterTop, matrix),
            Vector2.Transform(transformerLTRB.CenterRight, matrix),
            Vector2.Transform(transformerLTRB.CenterBottom, matrix)
        );

        /// <summary>
        ///  Convert to curves from ellipse.
        /// </summary>
        /// <param name="centerLeft"> The left point. </param>
        /// <param name="centerTop"> The top point. </param>
        /// <param name="centerRight"> The right point. </param>
        /// <param name="centerBottom"> The bottom point. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromEllipse(Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom) => TransformerGeometry._convertToCurvesFromEllipse
        (
            centerLeft,
            centerTop, 
            centerRight, 
            centerBottom
        );

        /// <summary>
        ///  Convert to curves from ellipse.
        /// </summary>
        /// <param name="centerLeft"> The left point. </param>
        /// <param name="centerTop"> The top point. </param>
        /// <param name="centerRight"> The right point. </param>
        /// <param name="centerBottom"> The bottom point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromEllipse(Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom, Matrix3x2 matrix) => TransformerGeometry._convertToCurvesFromEllipse
        (
            Vector2.Transform(centerLeft, matrix), 
            Vector2.Transform(centerTop, matrix), 
            Vector2.Transform(centerRight, matrix), 
            Vector2.Transform(centerBottom, matrix)
        );

        private static IEnumerable<IEnumerable<Node>> _convertToCurvesFromEllipse(Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromEllipse(centerLeft, centerTop, centerRight, centerBottom)
            };
        }
        private static IEnumerable<Node> _convertToCurveFromEllipse(Vector2 centerLeft, Vector2 centerTop, Vector2 centerRight, Vector2 centerBottom)
        {
            // A Ellipse has left, top, right, bottom four curves.
            // 
            // Control points on the left and right sides of the nodes.
            // 
            // The distance of the control point 
            // is 0.552f times
            // the length of the square edge.

            //HV
            Vector2 horizontal = (centerRight - centerLeft);
            Vector2 horizontal276 = horizontal * _z276;// vector * 0.552f / 2

            Vector2 vertical = (centerBottom - centerTop);
            Vector2 vertical276 = vertical * _z276;// vector * 0.552f / 2

            //Control
            Vector2 left1 = centerLeft + vertical276;
            Vector2 left2 = centerLeft - vertical276;
            Vector2 top1 = centerTop - horizontal276;
            Vector2 top2 = centerTop + horizontal276;
            Vector2 right1 = centerRight - vertical276;
            Vector2 right2 = centerRight + vertical276;
            Vector2 bottom1 = centerBottom + horizontal276;
            Vector2 bottom2 = centerBottom - horizontal276;

            //curves
            return new List<Node>
            {
                new Node
                {
                    Point = centerLeft,
                    LeftControlPoint = left2,
                    RightControlPoint = left1,
                    IsSmooth = true
                },
                new Node
                {
                    Point = centerTop,
                    LeftControlPoint = top2,
                    RightControlPoint = top1,
                    IsSmooth = true
                },
                new Node
                {
                    Point = centerRight,
                    LeftControlPoint = right2,
                    RightControlPoint = right1,
                    IsSmooth = true
                },
                new Node
                {
                    Point = centerBottom,
                    LeftControlPoint = bottom2,
                    RightControlPoint = bottom1,
                    IsSmooth = true
                },
                new Node
                {
                    Point = centerLeft,
                    LeftControlPoint = left2,
                    RightControlPoint = left1,
                    IsSmooth = true
                },
            };
        }


        #endregion

    }
}