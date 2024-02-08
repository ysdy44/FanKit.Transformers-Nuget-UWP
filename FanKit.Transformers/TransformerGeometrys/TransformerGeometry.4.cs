using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Type of arrow tail.
    /// </summary>
    public enum GeometryArrowTailType
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Arrow. </summary>
        Arrow,

        /// <summary> Arrow. </summary>
        // Round,
    }

    partial class TransformerGeometry
    {

        #region Arrow


        /// <summary>
        /// Create a new arrow geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource - creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="isAbsolute"> Is absolute? </param>
        /// <param name="width"> The absolute width. </param>
        /// <param name="value"> The relative value. </param>
        /// <param name="leftTail"> The left - tail. </param>
        /// <param name="rightTail"> The right - tail. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateArrow(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, bool isAbsolute = false, float width = 10, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
        {
            Vector2 center = transformer.Center;
            Vector2 centerLeft = transformer.CenterLeft;
            Vector2 centerRight = transformer.CenterRight;

            // horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            // vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();

            Vector2 widthVector = TransformerGeometry.GetArrowWidthVector(isAbsolute, width, value, vertical, verticalLength);

            Vector2 focusVector = TransformerGeometry.GetArrowFocusVector(verticalLength, horizontalLength, horizontal);
            Vector2 leftFocusTransform = (transformer.CenterLeft + focusVector);
            Vector2 rightFocusTransform = (transformer.CenterRight - focusVector);

            return TransformerGeometry.CreateArrowCore
            (
                resourceCreator,
                widthVector + transformer.Center - center,

                // Left
                centerLeft,
                transformer.LeftBottom,

                transformer.LeftTop,
                leftFocusTransform - centerLeft,
                leftFocusTransform,

                // Right
                centerRight,
                transformer.RightBottom,

                transformer.RightTop,
                rightFocusTransform - centerRight,
                rightFocusTransform,

                leftTail, rightTail
             );
        }


        /// <summary>
        /// Create a new arrow geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource - creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="isAbsolute"> Is absolute? </param>
        /// <param name="width"> The absolute width. </param>
        /// <param name="value"> The relative value. </param>
        /// <param name="leftTail"> The left - tail. </param>
        /// <param name="rightTail"> The right - tail. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateArrow(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix, bool isAbsolute = false, float width = 10, float value = 0.5f, GeometryArrowTailType leftTail = GeometryArrowTailType.None, GeometryArrowTailType rightTail = GeometryArrowTailType.Arrow)
        {
            Vector2 center = Vector2.Transform(transformer.Center, matrix);
            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, matrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, matrix);

            // horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            // vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();

            Vector2 widthVector = TransformerGeometry.GetArrowWidthVector(isAbsolute, width, value, vertical, verticalLength);

            Vector2 focusVector = TransformerGeometry.GetArrowFocusVector(verticalLength, horizontalLength, horizontal);
            Vector2 leftFocusTransform = Vector2.Transform(transformer.CenterLeft + focusVector, matrix);
            Vector2 rightFocusTransform = Vector2.Transform(transformer.CenterRight - focusVector, matrix);

            return TransformerGeometry.CreateArrowCore
            (
                resourceCreator,
                Vector2.Transform(widthVector + transformer.Center, matrix) - center,

                // Left
                centerLeft,
                Vector2.Transform(transformer.LeftBottom, matrix),

                Vector2.Transform(transformer.LeftTop, matrix),
                (leftFocusTransform - centerLeft),
                leftFocusTransform,

                // Right
                centerRight,
                Vector2.Transform(transformer.RightBottom, matrix),

                Vector2.Transform(transformer.RightTop, matrix),
                (rightFocusTransform - centerRight),
                rightFocusTransform,

                leftTail, rightTail
            );
        }


        private static CanvasGeometry CreateArrowCore(ICanvasResourceCreator resourceCreator,
            Vector2 widthVectorTransform,

            // Left
            Vector2 centerLeft,
            Vector2 leftBottom,

            Vector2 leftTop,
            Vector2 leftVector,
            Vector2 leftFocusTransform,

            // Right
            Vector2 centerRight,
            Vector2 rightBottom,

            Vector2 rightTop,
            Vector2 rightVector,
            Vector2 rightFocusTransform,

           GeometryArrowTailType leftTail, GeometryArrowTailType rightTail)
        {
            Vector2[] points;

            if (leftTail == GeometryArrowTailType.Arrow && rightTail == GeometryArrowTailType.Arrow)
            {
                points = new Vector2[10]
                {
                    centerLeft, // L

                    leftTop + leftVector, // LT
                    leftFocusTransform - widthVectorTransform, // C LT

                    rightFocusTransform - widthVectorTransform, // C RT
                    rightTop + rightVector, // RT

                    centerRight, // R

                    rightBottom + rightVector, // RB
                    rightFocusTransform + widthVectorTransform, // C RB

                    leftFocusTransform + widthVectorTransform, // C LB
                    leftBottom + leftVector, // LB
                };
            }
            else if (leftTail == GeometryArrowTailType.Arrow && rightTail == GeometryArrowTailType.None)
            {
                points = new Vector2[7]
                {
                    centerLeft, // L

                    leftTop + leftVector, // LT
                    leftFocusTransform - widthVectorTransform, // C LT
                    
                    centerRight - widthVectorTransform, // RT
                    centerRight + widthVectorTransform, // RB

                    leftFocusTransform + widthVectorTransform, // C LB
                    leftBottom + leftVector, // LB
                };
            }
            else if (leftTail == GeometryArrowTailType.None && rightTail == GeometryArrowTailType.Arrow)
            {
                points = new Vector2[7]
                {
                    centerRight, // R

                    rightTop + rightVector, // RT
                    rightFocusTransform - widthVectorTransform, // C RT

                    centerLeft - widthVectorTransform, // LT
                    centerLeft + widthVectorTransform, // LB

                    rightFocusTransform + widthVectorTransform, // C RB
                    rightBottom + rightVector, // RB
                };
            }
            else
            {
                points = new Vector2[4]
                {
                    centerLeft + widthVectorTransform, // LB
                    centerLeft - widthVectorTransform, // LT
                    centerRight - widthVectorTransform, // RT
                    centerRight + widthVectorTransform, // RB
                };
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        private static Vector2 GetArrowFocusVector(float verticalLength, float horizontalLength, Vector2 horizontal)
        {
            if (verticalLength < horizontalLength)
                return 0.5f * (verticalLength / horizontalLength) * horizontal;
            else
                return 0.5f * horizontal;
        }

        private static Vector2 GetArrowWidthVector(bool isAbsolute, float width2, float value, Vector2 vertical, float verticalLength)
        {
            float width = isAbsolute ? width2 : value * verticalLength;
            return vertical * (width / verticalLength) / 2;
        }


        #endregion


        #region Capsule


        /// <summary>
        /// Create a new capsule geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource - creator. </param>
        /// <param name="transformer"> The ITransformer - LTRB. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateCapsule(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer)
        {
            Vector2 centerLeft = transformer.CenterLeft;
            Vector2 centerTop = transformer.CenterTop;
            Vector2 centerRight = transformer.CenterRight;
            Vector2 centerBottom = transformer.CenterBottom;

            // Horizontal
            Vector2 horizontal = transformer.Horizontal;
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            // Vertical
            Vector2 vertical = transformer.Vertical;
            float verticalLength = vertical.Length();

            if (horizontalLength < verticalLength) return TransformerGeometry.CreateEllipse(resourceCreator, transformer);

            return TransformerGeometry.CreateCapsuleCore(resourceCreator,
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
            );
        }

        /// <summary>
        /// Create a new capsule geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource - creator. </param>
        /// <param name="transformerLTRB"> The ITransformer - LTRB. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateCapsule(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix)
        {
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            Vector2 centerLeft = Vector2.Transform(transformer.CenterLeft, matrix);
            Vector2 centerTop = Vector2.Transform(transformer.CenterTop, matrix);
            Vector2 centerRight = Vector2.Transform(transformer.CenterRight, matrix);
            Vector2 centerBottom = Vector2.Transform(transformer.CenterBottom, matrix);

            // Horizontal
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            // Vertical
            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();

            if (horizontalLength < verticalLength) return TransformerGeometry.CreateEllipse(resourceCreator, transformer, matrix);

            return TransformerGeometry.CreateCapsuleCore(resourceCreator,
                verticalLength,
                horizontalUnit,

                centerTop,
                centerLeft,
                centerRight,
                centerBottom,

                leftTop,
                rightTop,
                rightBottom,
                leftBottom);
        }

        private static CanvasGeometry CreateCapsuleCore(ICanvasResourceCreator resourceCreator,
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
            // Horizontal
            Vector2 horizontal2 = 0.5f * verticalLength * horizontalUnit;
            Vector2 horizontal448 = horizontal2 * 0.448f; // vector / (1 - 0.552f)
            // Vertical
            Vector2 vertical276 = (centerBottom - centerTop) * 0.276f; // vector / 2 * 0.552f

            // Control
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


            // Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            {
                pathBuilder.BeginFigure(centerLeft);

                pathBuilder.AddCubicBezier(left2, leftTop_Top1, leftTop_Top);
                pathBuilder.AddLine(rightTop_Top);

                pathBuilder.AddCubicBezier(rightTop_Top2, right1, centerRight);

                pathBuilder.AddCubicBezier(right2, rightBottom_Bottom1, rightBottom_Bottom);
                pathBuilder.AddLine(leftBottom_Bottom);

                pathBuilder.AddCubicBezier(leftBottom_Bottom2, left1, centerLeft);

                pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            }

            // Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }


        #endregion


        #region Heart


        /// <summary>
        /// Create a new heart geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource - creator. </param>
        /// <param name="spread"> The spread. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateHeart(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, float spread)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return TransformerGeometry.CreateHeartCore(resourceCreator, spread, oneMatrix);
        }

        /// <summary>
        /// Create a new heart geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource - creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="spread"> The spread. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateHeart(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix, float spread)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return TransformerGeometry.CreateHeartCore(resourceCreator, spread, oneMatrix2);
        }

        private static CanvasGeometry CreateHeartCore(ICanvasResourceCreator resourceCreator, float spread, Matrix3x2 oneMatrix)
        {
            Vector2 bottom = new Vector2(0, 1);

            Vector2 leftBottom = new Vector2(-0.84f, 0.178f);
            Vector2 leftBottom2 = leftBottom + new Vector2(-0.2f, -0.2f);

            Vector2 leftTop = new Vector2(-0.84f, -0.6f);
            Vector2 leftTop1 = leftTop + new Vector2(-0.2f, 0.2f);
            Vector2 leftTop2 = leftTop + new Vector2(0.2f, -0.2f);

            Vector2 top1 = new Vector2(-0.2f, -0.8f);
            Vector2 topSpread = TransformerGeometry.HeartTopSpread(spread);
            Vector2 top2 = new Vector2(0.2f, -0.8f);

            Vector2 rightTop = new Vector2(0.84f, -0.6f);
            Vector2 rightTop1 = rightTop + new Vector2(-0.2f, -0.2f);
            Vector2 rightTop2 = rightTop + new Vector2(0.2f, 0.2f);

            Vector2 rightBottom = new Vector2(0.84f, 0.178f);
            Vector2 rightBottom1 = rightBottom + new Vector2(0.2f, -0.2f);


            // Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            {
                pathBuilder.BeginFigure(bottom);
                pathBuilder.AddLine(leftBottom);

                pathBuilder.AddCubicBezier(leftBottom2, leftTop1, leftTop);

                pathBuilder.AddCubicBezier(leftTop2, top1, topSpread);
                pathBuilder.AddCubicBezier(top2, rightTop1, rightTop);

                pathBuilder.AddCubicBezier(rightTop2, rightBottom1, rightBottom);
                pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            }

            // Geometry
            return CanvasGeometry.CreatePath(pathBuilder).Transform(oneMatrix);
        }


        private static Vector2 HeartTopSpread(float spread)
        {
            // Rang
            //   x: 0~1
            //   y: 1.0~ - 0.8
            //  y=1 - 1.8x
            float topSpread = 1f - spread * 1.8f;
            return new Vector2(0, topSpread);
        }


        #endregion

    }
}