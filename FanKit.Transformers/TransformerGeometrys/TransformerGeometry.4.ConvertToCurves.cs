using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {

        #region Arrow


        /// <summary>
        /// Convert to curves from arrow.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="isAbsolute"> Is absolute? </param>
        /// <param name="width"> The absolute width. </param>
        /// <param name="value"> The relative value. </param>
        /// <param name="leftTail"> The left-tail. </param>
        /// <param name="rightTail"> The right-tail. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromArrow(ITransformerLTRB transformer, bool isAbsolute = false, float width2 = 10, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
        {
            Vector2 center = transformer.Center;
            Vector2 centerLeft = transformer.CenterLeft;
            Vector2 centerRight = transformer.CenterRight;

            //horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            //vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();

            Vector2 widthVector = TransformerGeometry._getArrowWidthVector(isAbsolute, width2, value, vertical, verticalLength);

            Vector2 focusVector = TransformerGeometry._getArrowFocusVector(verticalLength, horizontalLength, horizontal);
            Vector2 leftFocusTransform = (transformer.CenterLeft + focusVector);
            Vector2 rightFocusTransform = (transformer.CenterRight - focusVector);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._createToCurveFromArrow
                (
                    (widthVector + transformer.Center) - center,

                    //Left
                    centerLeft,
                    transformer.LeftBottom,

                    transformer.LeftTop,
                    (leftFocusTransform - centerLeft),
                    leftFocusTransform,

                    //Right
                    centerRight,
                    transformer.RightBottom,

                    transformer.RightTop,
                    (rightFocusTransform - centerRight),
                    rightFocusTransform,

                    leftTail, rightTail
                )
            };
        }


        /// <summary>
        /// Convert to curves from arrow.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="isAbsolute"> Is absolute? </param>
        /// <param name="width"> The absolute width. </param>
        /// <param name="value"> The relative value. </param>
        /// <param name="leftTail"> The left-tail. </param>
        /// <param name="rightTail"> The right-tail. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromArrow(ITransformerLTRB transformer, Matrix3x2 matrix, bool isAbsolute = false, float width = 10, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
        {
            Vector2 center = Vector2.Transform(transformer.Center, matrix);
            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, matrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, matrix);

            //horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            //vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();

            Vector2 widthVector = TransformerGeometry._getArrowWidthVector(isAbsolute, width, value, vertical, verticalLength);

            Vector2 focusVector = TransformerGeometry._getArrowFocusVector(verticalLength, horizontalLength, horizontal);
            Vector2 leftFocusTransform = Vector2.Transform(transformer.CenterLeft + focusVector, matrix);
            Vector2 rightFocusTransform = Vector2.Transform(transformer.CenterRight - focusVector, matrix);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._createToCurveFromArrow
                (
                    Vector2.Transform(widthVector + transformer.Center, matrix) - center,

                    //Left
                    centerLeft,
                    Vector2.Transform(transformer.LeftBottom, matrix),

                    Vector2.Transform(transformer.LeftTop, matrix),
                    (leftFocusTransform - centerLeft),
                    leftFocusTransform,

                    //Right
                    centerRight,
                    Vector2.Transform(transformer.RightBottom, matrix),

                    Vector2.Transform(transformer.RightTop, matrix),
                    (rightFocusTransform - centerRight),
                    rightFocusTransform,

                    leftTail, rightTail
                )
            };
        }


        private static Vector2 _getArrowFocusVector(float verticalLength, float horizontalLength, Vector2 horizontal)
        {
            if (verticalLength < horizontalLength)
                return 0.5f * (verticalLength / horizontalLength) * horizontal;
            else
                return 0.5f * horizontal;
        }
        private static Vector2 _getArrowWidthVector(bool isAbsolute, float width2, float value, Vector2 vertical, float verticalLength)
        {
            float width = isAbsolute ? width2 : value * verticalLength;
            return vertical * (width / verticalLength) / 2;
        }


        private static IEnumerable<Node> _createToCurveFromArrow(
            Vector2 widthVectorTransform,

            //Left
            Vector2 centerLeft,
            Vector2 leftBottom,

            Vector2 leftTop,
            Vector2 leftVector,
            Vector2 leftFocusTransform,

            //Right
            Vector2 centerRight,
            Vector2 rightBottom,

            Vector2 rightTop,
            Vector2 rightVector,
            Vector2 rightFocusTransform,

           GeometryArrowTailType leftTail, GeometryArrowTailType rightTail)
        {

            if (leftTail == GeometryArrowTailType.Arrow && rightTail == GeometryArrowTailType.Arrow)
            {
                return new List<Node>(10)
                {
                    new Node { Point = centerLeft },//L

                    new Node { Point = leftTop+leftVector },//LT
                    new Node { Point = leftFocusTransform-widthVectorTransform },//C LT

                    new Node { Point = rightFocusTransform-widthVectorTransform },//C RT
                    new Node { Point = rightTop+rightVector },//RT

                    new Node { Point = centerRight },//R
                    
                    new Node { Point = rightBottom+rightVector },//RB
                    new Node { Point = rightFocusTransform+widthVectorTransform },//C RB
                    
                    new Node { Point = leftFocusTransform+widthVectorTransform },//C LB
                    new Node { Point = leftBottom+leftVector },//LB
                };
            }
            else if (leftTail == GeometryArrowTailType.Arrow && rightTail == GeometryArrowTailType.None)
            {
                return new List<Node>(7)
                {
                    new Node { Point = centerLeft },//L
                    
                    new Node { Point = leftTop + leftVector },//LT
                    new Node { Point = leftFocusTransform - widthVectorTransform },//C LT
                    
                    new Node { Point = centerRight - widthVectorTransform },//RT
                    new Node { Point = centerRight + widthVectorTransform },//RB
                    
                    new Node { Point = leftFocusTransform + widthVectorTransform },//C LB
                    new Node { Point = leftBottom + leftVector },//LB

                    new Node { Point = centerLeft },//L
                };
            }
            else if (leftTail == GeometryArrowTailType.None && rightTail == GeometryArrowTailType.Arrow)
            {
                return new List<Node>(7)
                {
                    new Node { Point = centerRight },//R
                    
                    new Node { Point = rightTop + rightVector },//RT
                    new Node { Point = rightFocusTransform - widthVectorTransform },//C RT
                    
                    new Node { Point = centerLeft - widthVectorTransform },//LT
                    new Node { Point = centerLeft + widthVectorTransform },//LB
                    
                    new Node { Point = rightFocusTransform + widthVectorTransform },//C RB
                    new Node { Point = rightBottom + rightVector },//RB
                    
                    new Node { Point = centerRight },//R
                };
            }
            else
            {
                Node leftTop2 = new Node { Point = centerLeft - widthVectorTransform };

                return new List<Node>(4)
                {
                    leftTop2,
                    new Node { Point = centerRight - widthVectorTransform },
                    new Node { Point = centerRight + widthVectorTransform },
                    new Node { Point = centerLeft+ widthVectorTransform },
                    leftTop2,
                };
            }
        }


        #endregion


        #region Capsule


        /// <summary>
        /// Convert to curves from capsule.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromCapsule(ITransformerLTRB transformer)
        {
            Vector2 centerLeft = transformer.CenterLeft;
            Vector2 centerTop = transformer.CenterTop;
            Vector2 centerRight = transformer.CenterRight;
            Vector2 centerBottom = transformer.CenterBottom;

            //Horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            //Vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();

            if (horizontalLength < verticalLength) return TransformerGeometry.ConvertToCurvesFromEllipse(transformer);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._createToCurveFromCapsule
                (
                    verticalLength,
                    horizontalUnit,

                    centerTop,
                    centerLeft,
                    centerRight,
                    centerBottom,

                    transformer.LeftTop,
                    transformer.RightTop,
                    transformer.RightBottom,
                    transformer.LeftBottom
                )
            };
        }

        /// <summary>
        /// Convert to curves from capsule.
        /// </summary>
        /// <param name="transformerLTRB"> The ITransformer-LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromCapsule(ITransformerLTRB transformer, Matrix3x2 matrix)
        {
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, matrix);
            Vector2 centerTop = Vector2.Transform(transformer.CenterTop, matrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, matrix);
            Vector2 centerBottom = Vector2.Transform(transformer.CenterBottom, matrix);

            //Horizontal
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            //Vertical
            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();

            if (horizontalLength < verticalLength) return TransformerGeometry.ConvertToCurvesFromEllipse(transformer, matrix);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._createToCurveFromCapsule
                (
                    verticalLength,
                    horizontalUnit,

                    centerTop,
                    centerLeft,
                    centerRight,
                    centerBottom,

                    leftTop,
                    rightTop,
                    rightBottom,
                    leftBottom
                )
            };
        }

        private static IEnumerable<Node> _createToCurveFromCapsule(
            float verticalLength,
            Vector2 horizontalUnit,

            Vector2 centerTop,
            Vector2 centerLeft,
            Vector2 centerRight,
            Vector2 centerBottom,

            Vector2 leftTop,
            Vector2 rightTop,
            Vector2 rightBottom,
            Vector2 leftBottom)
        {
            //Horizontal
            Vector2 horizontal2 = 0.5f * verticalLength * horizontalUnit;
            Vector2 horizontal448 = horizontal2 * 0.448f;// vector / (1 - 0.552f)
            //Vertical
            Vector2 vertical276 = (centerBottom - centerTop) * 0.276f;// vector / 2 * 0.552f

            //Control
            Vector2 left2 = centerLeft - vertical276;
            Vector2 leftTop_Top = leftTop + horizontal2;
            Vector2 leftTop_Top1 = leftTop + horizontal448;

            Vector2 rightTop_Top = rightTop - horizontal2;
            Vector2 rightTop_Top2 = rightTop - horizontal448;
            Vector2 right1 = centerRight - vertical276;

            Vector2 right2 = centerRight + vertical276;
            Vector2 rightBottom_Bottom = rightBottom - horizontal2;
            Vector2 rightBottom_Bottom1 = rightBottom - horizontal448;

            Vector2 leftBottom_Bottom = leftBottom + horizontal2;
            Vector2 leftBottom_Bottom2 = leftBottom + horizontal448;
            Vector2 left1 = centerLeft + vertical276;


            //curves
            return new List<Node>
            {
                new Node
                {
                    Point = centerLeft,
                    LeftControlPoint = left2,
                    RightControlPoint = centerLeft,
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
                    Point = centerRight,
                    LeftControlPoint = right2,
                    RightControlPoint = right1,
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
                    Point = centerLeft,
                    LeftControlPoint = centerLeft,
                    RightControlPoint = left1,
                    IsSmooth = true,
                },
            };
        }


        #endregion


        #region Heart


        /// <summary>
        /// Convert to curves from heart.
        /// </summary>
        /// <param name="spread"> The spread. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromHeart(ITransformerLTRB transformer, float spread)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._createToCurveFromHeart(spread, oneMatrix)
            };
        }

        /// <summary>
        /// Convert to curves from heart.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="spread"> The spread. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromHeart(ITransformerLTRB transformer, Matrix3x2 matrix, float spread)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._createToCurveFromHeart(spread, oneMatrix2)
            };
        }

        private static IEnumerable<Node> _createToCurveFromHeart(float spread, Matrix3x2 oneMatrix)
        {
            Vector2 bottom = new Vector2(0, 1);

            Vector2 leftBottom = new Vector2(-0.84f, 0.178f);
            Vector2 leftBottom2 = leftBottom + new Vector2(-0.2f, -0.2f);

            Vector2 leftTop = new Vector2(-0.84f, -0.6f);
            Vector2 leftTop1 = leftTop + new Vector2(-0.2f, 0.2f);
            Vector2 leftTop2 = leftTop + new Vector2(0.2f, -0.2f);

            Vector2 top1 = new Vector2(-0.2f, -0.8f);
            Vector2 topSpread = TransformerGeometry._heartTopSpread(spread);
            Vector2 top2 = new Vector2(0.2f, -0.8f);

            Vector2 rightTop = new Vector2(0.84f, -0.6f);
            Vector2 rightTop1 = rightTop + new Vector2(-0.2f, -0.2f);
            Vector2 rightTop2 = rightTop + new Vector2(0.2f, 0.2f);

            Vector2 rightBottom = new Vector2(0.84f, 0.178f);
            Vector2 rightBottom1 = rightBottom + new Vector2(0.2f, -0.2f);


            return new List<Node>
            {
                new Node { Point = Vector2.Transform(bottom, oneMatrix) },
                new Node
                {
                    Point = Vector2.Transform(leftBottom, oneMatrix),
                    LeftControlPoint = Vector2.Transform(leftBottom2, oneMatrix),
                    RightControlPoint = Vector2.Transform(leftBottom, oneMatrix),
                    IsSmooth = true,
                },
                new Node
                {
                    Point = Vector2.Transform(leftTop, oneMatrix),
                    LeftControlPoint = Vector2.Transform(leftTop2, oneMatrix),
                    RightControlPoint = Vector2.Transform(leftTop1, oneMatrix),
                    IsSmooth = true,
                },

                new Node
                {
                    Point = Vector2.Transform(topSpread, oneMatrix),
                    LeftControlPoint = Vector2.Transform(top2, oneMatrix),
                    RightControlPoint = Vector2.Transform(top1, oneMatrix),
                    IsSmooth = true,
                },

                new Node
                {
                    Point = Vector2.Transform(rightTop, oneMatrix),
                    LeftControlPoint = Vector2.Transform(rightTop2, oneMatrix),
                    RightControlPoint = Vector2.Transform(rightTop1, oneMatrix),
                    IsSmooth = true,
                },
                new Node
                {
                    Point = Vector2.Transform(rightBottom, oneMatrix),
                    LeftControlPoint = Vector2.Transform(rightBottom, oneMatrix),
                    RightControlPoint = Vector2.Transform(rightBottom1, oneMatrix),
                    IsSmooth = true,
                },
                new Node { Point = Vector2.Transform(bottom, oneMatrix) },
            };
        }


        #endregion

    }
}