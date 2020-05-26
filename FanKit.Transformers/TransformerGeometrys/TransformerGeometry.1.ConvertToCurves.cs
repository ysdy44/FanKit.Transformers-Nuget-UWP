using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {
               
        #region RoundRect


        /// <summary>
        /// Convert to curves from round rect.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="corner"> The corner. </param>
        /// <returns> The product geometry. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromRoundRect(ITransformerLTRB transformer, float corner)
        {       
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromRoundRect
                (
                    transformer.LeftTop,
                    transformer.RightTop,
                    transformer.RightBottom,
                    transformer.LeftBottom,

                    transformer.CenterLeft,
                    transformer.CenterTop,
                    transformer.CenterRight,
                    transformer.CenterBottom,

                    corner
                )
            };
        }

        /// <summary>
        /// Convert to curves from round rect.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="corner"> The corner. </param>
        /// <returns> The product geometry. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromRoundRect(ITransformerLTRB transformer, Matrix3x2 matrix, float corner)
        {
            return new List<IEnumerable<Node>>
            {
                 TransformerGeometry._convertToCurveFromRoundRect
                 (
                     Vector2.Transform(transformer.LeftTop, matrix),
                     Vector2.Transform(transformer.RightTop, matrix),
                     Vector2.Transform(transformer.RightBottom, matrix),
                     Vector2.Transform(transformer.LeftBottom, matrix),

                     Vector2.Transform(transformer.CenterLeft, matrix),
                     Vector2.Transform(transformer.CenterTop, matrix),
                     Vector2.Transform(transformer.CenterRight, matrix),
                     Vector2.Transform(transformer.CenterBottom, matrix),

                     corner
                 )
            };
        }

        private static IEnumerable<Node> _convertToCurveFromRoundRect(
            Vector2 leftTop,
            Vector2 rightTop,
            Vector2 rightBottom,
            Vector2 leftBottom,

            Vector2 centerLeft,
            Vector2 centerTop,
            Vector2 centerRight,
            Vector2 centerBottom,

            float corner)
        {
            //Horizontal
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            //Vertical
            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();
            Vector2 verticalUnit = vertical / verticalLength;


            //Control
            float minLength = System.Math.Min(horizontalLength, verticalLength);
            float minLength2 = corner * minLength;

            Vector2 horizontal2 = minLength2 * horizontalUnit;
            Vector2 horizontal448 = horizontal2 * 0.448f;// vector / (1 - 4 * 0.552f)
            Vector2 vertical2 = minLength2 * verticalUnit;
            Vector2 vertical448 = vertical2 * 0.448f;// vector /  (1 - 4 * 0.552f)


            Vector2 leftTop_Left = leftTop + vertical2;
            Vector2 leftTop_Left2 = leftTop + vertical448;
            Vector2 leftTop_Top = leftTop + horizontal2;
            Vector2 leftTop_Top1 = leftTop + horizontal448;

            Vector2 rightTop_Top = rightTop - horizontal2;
            Vector2 rightTop_Top2 = rightTop - horizontal448;
            Vector2 rightTop_Right = rightTop + vertical2;
            Vector2 rightTop_Right1 = rightTop + vertical448;

            Vector2 rightBottom_Right = rightBottom - vertical2;
            Vector2 rightBottom_Right2 = rightBottom - vertical448;
            Vector2 rightBottom_Bottom = rightBottom - horizontal2;
            Vector2 rightBottom_Bottom1 = rightBottom - horizontal448;

            Vector2 leftBottom_Bottom = leftBottom + horizontal2;
            Vector2 leftBottom_Bottom2 = leftBottom + horizontal448;
            Vector2 leftBottom_Left = leftBottom - vertical2;
            Vector2 leftBottom_Left1 = leftBottom - vertical448;


            //curves
            return new List<Node>
            {
                new Node
                {
                    Point = leftTop_Left,
                    LeftControlPoint = leftTop_Left2,
                    RightControlPoint = leftTop_Left,
                    IsSmooth = true,
                },
                new Node
                {
                    Point = leftTop_Top,
                    LeftControlPoint = leftTop_Top,
                    RightControlPoint = leftTop_Top1,
                    IsSmooth = true,
                },

                new Node
                {
                    Point = rightTop_Top,
                    LeftControlPoint = rightTop_Top2,
                    RightControlPoint = rightTop_Top,
                    IsSmooth = true,
                },
                new Node
                {
                    Point = rightTop_Right,
                    LeftControlPoint = rightTop_Right,
                    RightControlPoint = rightTop_Right1,
                    IsSmooth = true,
                },

                new Node
                {
                    Point = rightBottom_Right,
                    LeftControlPoint = rightBottom_Right2,
                    RightControlPoint = rightBottom_Right,
                    IsSmooth = true,
                },
                new Node
                {
                    Point = rightBottom_Bottom,
                    LeftControlPoint = rightBottom_Bottom,
                    RightControlPoint = rightBottom_Bottom1,
                    IsSmooth = true,
                },

                new Node
                {
                    Point = leftBottom_Bottom,
                    LeftControlPoint = leftBottom_Bottom2,
                    RightControlPoint = leftBottom_Bottom,
                    IsSmooth = true,
                },
                new Node
                {
                    Point = leftBottom_Left,
                    LeftControlPoint = leftBottom_Left,
                    RightControlPoint = leftBottom_Left1,
                    IsSmooth = true,
                },

                 new Node{Point=leftTop_Left}
            };
        }


        #endregion


        #region Triangle


        /// <summary>
        ///  Convert to curves from triangle.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromTriangle(ITransformerLTRB transformerLTRB, float center) => TransformerGeometry._convertToCurvesFromTriangle(transformerLTRB.LeftTop, transformerLTRB.RightTop, transformerLTRB.RightBottom, transformerLTRB.LeftBottom, center);

        /// <summary>
        ///  Convert to curves from triangle.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromTriangle(ITransformerLTRB transformerLTRB, Matrix3x2 matrix, float center) => TransformerGeometry._convertToCurvesFromTriangle(Vector2.Transform(transformerLTRB.LeftTop, matrix), Vector2.Transform(transformerLTRB.RightTop, matrix), Vector2.Transform(transformerLTRB.RightBottom, matrix), Vector2.Transform(transformerLTRB.LeftBottom, matrix), center);

        /// <summary>
        ///  Convert to curves from triangle.
        /// </summary>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromTriangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center) => TransformerGeometry._convertToCurvesFromTriangle(leftTop, rightTop, rightBottom, leftBottom, center);

        /// <summary>
        ///  Convert to curves from triangle.
        /// </summary>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromTriangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix, float center) => TransformerGeometry._convertToCurvesFromTriangle(Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix), center);

        private static IEnumerable<IEnumerable<Node>> _convertToCurvesFromTriangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
        {
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromTriangle(leftTop, rightTop, rightBottom, leftBottom, center)
            };
        }
        private static IEnumerable<Node> _convertToCurveFromTriangle(Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
        {
            float minusValue = 1.0f - center;
            Vector2 center2 = leftTop * minusValue + rightTop * center;

            //nodes
            return  new List<Node>
            {
                new Node{ Point = center2 },
                new Node{ Point = rightBottom },
                new Node{ Point = leftBottom },
                new Node{ Point = center2 },
            };
        }


        #endregion


        #region Diamond


        /// <summary>
        ///  Convert to curves from diamond.
        /// </summary>
        /// <param name="mid"> The mid value. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromDiamond(ITransformerLTRB transformer, float mid)
        {
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromDiamond
                (
                    transformer.LeftTop,
                    transformer.RightTop,
                    transformer.RightBottom,
                    transformer.LeftBottom,

                    transformer.CenterLeft,
                    transformer.CenterRight,

                    mid
                )
            };
        }

        /// <summary>
        ///  Convert to curves from diamond.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="mid"> The mid value. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromDiamond(ITransformerLTRB transformer, Matrix3x2 matrix, float mid)
        {     
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromDiamond
                (
                    Vector2.Transform(transformer.LeftTop, matrix),
                    Vector2.Transform(transformer.RightTop, matrix),
                    Vector2.Transform(transformer.RightBottom, matrix),
                    Vector2.Transform(transformer.LeftBottom, matrix),

                    Vector2.Transform(transformer.CenterLeft, matrix),
                    Vector2.Transform(transformer.CenterRight, matrix),

                    mid
                )
            };
        }

        private static IEnumerable<Node> _convertToCurveFromDiamond(
            Vector2 leftTop,
            Vector2 rightTop,
            Vector2 rightBottom,
            Vector2 leftBottom,

            Vector2 centerLeft,
            Vector2 centerRight,

            float mid)
        {
            float minusValue = 1.0f - mid;
            Vector2 top = leftTop * minusValue + rightTop * mid;
            Vector2 bottom = leftBottom * minusValue + rightBottom * mid;

            //curves
            return new List<Node>
            {
                new Node{ Point = centerLeft },
                new Node{ Point = top },
                new Node{ Point = centerRight },
                new Node{ Point = bottom },
                new Node{ Point = centerLeft },
            };
        }


        #endregion
        
    }
}