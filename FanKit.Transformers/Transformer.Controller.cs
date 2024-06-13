using System;
using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {
        /// <summary>
        /// It controls the transformation of transformer.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        /// <param name="startingTransformer"> The starting transformer. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isSnapToTick"> Snap to tick when spinning. </param>
        /// <returns> The controlled transformer. </returns>
        public static Transformer Controller(TransformerMode mode, Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isRatio = false, bool isCenter = false, bool isSnapToTick = false)
        {
            switch (mode)
            {
                case TransformerMode.None:
                    return startingTransformer;

                case TransformerMode.Rotation:
                    return Transformer.Rotate(
                        startingPoint,
                        point,
                        startingTransformer,
                        isSnapToTick);

                case TransformerMode.SkewLeft:
                    Vector2 vector1 = Transformer.Skew(startingPoint, point, startingTransformer.LeftTop, startingTransformer.LeftBottom);
                    return isCenter ? Transformer.ScaleLeftCenter(startingTransformer, vector1) : Transformer.ScaleLeft(startingTransformer, vector1);
                case TransformerMode.SkewTop:
                    Vector2 vector2 = Transformer.Skew(startingPoint, point, startingTransformer.LeftTop, startingTransformer.RightTop);
                    return isCenter ? Transformer.ScaleTopCenter(startingTransformer, vector2) : Transformer.ScaleTop(startingTransformer, vector2);
                case TransformerMode.SkewRight:
                    Vector2 vector3 = Transformer.Skew(startingPoint, point, startingTransformer.RightTop, startingTransformer.RightBottom);
                    return isCenter ? Transformer.ScaleRightCenter(startingTransformer, vector3) : Transformer.ScaleRight(startingTransformer, vector3);
                case TransformerMode.SkewBottom:
                    Vector2 vector4 = Transformer.Skew(startingPoint, point, startingTransformer.LeftBottom, startingTransformer.RightBottom);
                    return isCenter ? Transformer.ScaleBottomCenter(startingTransformer, vector4) : Transformer.ScaleBottom(startingTransformer, vector4);

                case TransformerMode.ScaleLeft:
                    LinePD1C line1 = new LinePD1C
                    {
                        Point = startingTransformer.CenterLeft,
                        DiagonalPoint = startingTransformer.CenterRight,
                        CanvasPoint = point
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line1);
                        else return Transformer.ScaleRatio(startingTransformer, line1);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line1);
                        if (isCenter) return Transformer.ScaleLeftCenter(startingTransformer, vector);
                        else return Transformer.ScaleLeft(startingTransformer, vector);
                    }
                case TransformerMode.ScaleTop:
                    LinePD1C line2 = new LinePD1C
                    {
                        Point = startingTransformer.CenterTop,
                        DiagonalPoint = startingTransformer.CenterBottom,
                        CanvasPoint = point
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line2);
                        else return Transformer.ScaleRatio(startingTransformer, line2);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line2);
                        if (isCenter) return Transformer.ScaleTopCenter(startingTransformer, vector);
                        else return Transformer.ScaleTop(startingTransformer, vector);
                    }
                case TransformerMode.ScaleRight:
                    LinePD1C line3 = new LinePD1C
                    {
                        Point = startingTransformer.CenterRight,
                        DiagonalPoint = startingTransformer.CenterLeft,
                        CanvasPoint = point
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line3);
                        else return Transformer.ScaleRatio(startingTransformer, line3);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line3);
                        if (isCenter) return Transformer.ScaleRightCenter(startingTransformer, vector);
                        else return Transformer.ScaleRight(startingTransformer, vector);
                    }
                case TransformerMode.ScaleBottom:
                    LinePD1C line4 = new LinePD1C
                    {
                        Point = startingTransformer.CenterBottom,
                        DiagonalPoint = startingTransformer.CenterTop,
                        CanvasPoint = point
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line4);
                        else return Transformer.ScaleRatio(startingTransformer, line4);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line4);
                        if (isCenter) return Transformer.ScaleBottomCenter(startingTransformer, vector);
                        else return Transformer.ScaleBottom(startingTransformer, vector);
                    }

                case TransformerMode.ScaleLeftTop:
                    return Transformer.ScaleLeftTop(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleRightTop:
                    return Transformer.ScaleRightTop(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleRightBottom:
                    return Transformer.ScaleRightBottom(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleLeftBottom:
                    return Transformer.ScaleLeftBottom(point, startingTransformer, isRatio, isCenter);

                default:
                    return startingTransformer;
            }
        }

        /// <summary>
        /// It controls the transformation of transformer.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        /// <param name="startingTransformer"> The starting transformer. </param>
        /// <param name="inverseMatrix"> The inverse matrix. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isSnapToTick"> Snap to tick when spinning. </param>
        /// <returns> The controlled transformer. </returns>
        public static Transformer Controller(TransformerMode mode, Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio = false, bool isCenter = false, bool isSnapToTick = false)
        {
            switch (mode)
            {
                case TransformerMode.None:
                    return startingTransformer;

                case TransformerMode.Rotation:
                    return Transformer.Rotate(
                        Vector2.Transform(startingPoint, inverseMatrix),
                        Vector2.Transform(point, inverseMatrix),
                        startingTransformer,
                        isSnapToTick);

                case TransformerMode.SkewLeft:
                    Vector2 vector1 = Transformer.Skew(startingPoint, point, startingTransformer.LeftTop, startingTransformer.LeftBottom);
                    return isCenter ? Transformer.ScaleLeftCenter(startingTransformer, vector1) : Transformer.ScaleLeft(startingTransformer, vector1);
                case TransformerMode.SkewTop:
                    Vector2 vector2 = Transformer.Skew(startingPoint, point, startingTransformer.LeftTop, startingTransformer.RightTop);
                    return isCenter ? Transformer.ScaleTopCenter(startingTransformer, vector2) : Transformer.ScaleTop(startingTransformer, vector2);
                case TransformerMode.SkewRight:
                    Vector2 vector3 = Transformer.Skew(startingPoint, point, startingTransformer.RightTop, startingTransformer.RightBottom);
                    return isCenter ? Transformer.ScaleRightCenter(startingTransformer, vector3) : Transformer.ScaleRight(startingTransformer, vector3);
                case TransformerMode.SkewBottom:
                    Vector2 vector4 = Transformer.Skew(startingPoint, point, startingTransformer.LeftBottom, startingTransformer.RightBottom);
                    return isCenter ? Transformer.ScaleBottomCenter(startingTransformer, vector4) : Transformer.ScaleBottom(startingTransformer, vector4);

                case TransformerMode.ScaleLeft:
                    LinePD1C line1 = new LinePD1C
                    {
                        Point = startingTransformer.CenterLeft,
                        DiagonalPoint = startingTransformer.CenterRight,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line1);
                        else return Transformer.ScaleRatio(startingTransformer, line1);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line1);
                        if (isCenter) return Transformer.ScaleLeftCenter(startingTransformer, vector);
                        else return Transformer.ScaleLeft(startingTransformer, vector);
                    }
                case TransformerMode.ScaleTop:
                    LinePD1C line2 = new LinePD1C
                    {
                        Point = startingTransformer.CenterTop,
                        DiagonalPoint = startingTransformer.CenterBottom,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line2);
                        else return Transformer.ScaleRatio(startingTransformer, line2);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line2);
                        if (isCenter) return Transformer.ScaleTopCenter(startingTransformer, vector);
                        else return Transformer.ScaleTop(startingTransformer, vector);
                    }
                case TransformerMode.ScaleRight:
                    LinePD1C line3 = new LinePD1C
                    {
                        Point = startingTransformer.CenterRight,
                        DiagonalPoint = startingTransformer.CenterLeft,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line3);
                        else return Transformer.ScaleRatio(startingTransformer, line3);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line3);
                        if (isCenter) return Transformer.ScaleRightCenter(startingTransformer, vector);
                        else return Transformer.ScaleRight(startingTransformer, vector);
                    }
                case TransformerMode.ScaleBottom:
                    LinePD1C line4 = new LinePD1C
                    {
                        Point = startingTransformer.CenterBottom,
                        DiagonalPoint = startingTransformer.CenterTop,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    };
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleAroundRatioCenter(startingTransformer, line4);
                        else return Transformer.ScaleRatio(startingTransformer, line4);
                    }
                    else
                    {
                        Vector2 vector = Transformer.ScaleVector(line4);
                        if (isCenter) return Transformer.ScaleBottomCenter(startingTransformer, vector);
                        else return Transformer.ScaleBottom(startingTransformer, vector);
                    }

                case TransformerMode.ScaleLeftTop:
                    return Transformer.ScaleLeftTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRightTop:
                    return Transformer.ScaleRightTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRightBottom:
                    return Transformer.ScaleRightBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleLeftBottom:
                    return Transformer.ScaleLeftBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);

                default:
                    return startingTransformer;
            }
        }



        // Rotation      
        private static Transformer Rotate(Vector2 canvasStartingPoint, Vector2 canvasPoint, Transformer startingTransformer, bool isSnapToTick)
        {
            Vector2 center = startingTransformer.Center;

            float canvasRadian = Math.VectorToRadians(canvasPoint - center);
            if (isSnapToTick) canvasRadian = Math.RadiansStepFrequency(canvasRadian);

            float canvasStartingRadian = Math.VectorToRadians(canvasStartingPoint - center);
            float radian = canvasRadian - canvasStartingRadian;

            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(radian, center);
            return Transformer.Multiplies(startingTransformer, rotationMatrix);
        }



        // Skew
        private static Vector2 Skew(Vector2 startingPoint, Vector2 point, Vector2 linePoineA, Vector2 linePoineB)
        {
            Vector2 canvasStartingSkewPoint = Math.FootPoint(startingPoint, linePoineA, linePoineB);
            Vector2 canvasSkewPoint = Math.FootPoint(point, linePoineA, linePoineB);

            Vector2 vector = canvasSkewPoint - canvasStartingSkewPoint;
            Vector2 halfVector = vector / 2;

            return halfVector;
        }



        // ScaleAround
        private static Transformer ScaleAroundRatioCenter(Transformer startingTransformer, LinePD1C line)
        {
            Vector2 center = startingTransformer.Center;
            Vector2 footPoint = Math.FootPoint(line.CanvasPoint, line.Point, line.DiagonalPoint);

            float distance = new LineDistance(footPoint, line.Point, center);
            Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(distance, center);

            return Transformer.Multiplies(startingTransformer, scaleMatrix);
        }



        // ScaleCorner
        private static Transformer ScaleCorner(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Vector2, Vector2, Vector2, Vector2, Transformer> func)
        {
            Vector2 canvasPoint = point;

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;
                Vector2 footPoint = Math.FootPoint(canvasPoint, linePoint, center);

                float distance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(distance, center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 center = isCenter ? startingTransformer.Center * 2 - canvasPoint : lineDiagonalPoint;
                Vector2 horizontal = startingTransformer.CenterRight - startingTransformer.CenterLeft;
                Vector2 vertical = startingTransformer.CenterBottom - startingTransformer.CenterTop;

                Vector2 returnPoint = canvasPoint;
                Vector2 returnDiagonalPoint = center;
                Vector2 returnHorizontalPoint = Math.IntersectionPoint(canvasPoint, (canvasPoint - horizontal), (center + vertical), center);
                Vector2 returnVerticalPoint = Math.IntersectionPoint(canvasPoint, (canvasPoint - vertical), (center + horizontal), center);

                return func(returnPoint, returnDiagonalPoint, returnHorizontalPoint, returnVerticalPoint);
            }
        }
        private static Transformer ScaleCorner(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Vector2, Vector2, Vector2, Vector2, Transformer> func)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;
                Vector2 footPoint = Math.FootPoint(canvasPoint, linePoint, center);

                float distance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(distance, center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 center = isCenter ? startingTransformer.Center * 2 - canvasPoint : lineDiagonalPoint;
                Vector2 horizontal = startingTransformer.CenterRight - startingTransformer.CenterLeft;
                Vector2 vertical = startingTransformer.CenterBottom - startingTransformer.CenterTop;

                Vector2 returnPoint = canvasPoint;
                Vector2 returnDiagonalPoint = center;
                Vector2 returnHorizontalPoint = Math.IntersectionPoint(canvasPoint, (canvasPoint - horizontal), (center + vertical), center);
                Vector2 returnVerticalPoint = Math.IntersectionPoint(canvasPoint, (canvasPoint - vertical), (center + horizontal), center);

                return func(returnPoint, returnDiagonalPoint, returnHorizontalPoint, returnVerticalPoint);
            }
        }


        private static Transformer ScaleLeftTop(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftTop;
            Vector2 lineDiagonalPoint = startingTransformer.RightBottom;

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleLeftTop);
        }
        private static Transformer ScaleLeftTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftTop;
            Vector2 lineDiagonalPoint = startingTransformer.RightBottom;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleLeftTop);
        }
        static Transformer FuncScaleLeftTop(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnPoint,
                RightTop = returnHorizontalPoint,
                RightBottom = returnDiagonalPoint,
                LeftBottom = returnVerticalPoint,
            };
        }

        private static Transformer ScaleRightTop(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightTop;
            Vector2 lineDiagonalPoint = startingTransformer.LeftBottom;

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleRightTop);
        }
        private static Transformer ScaleRightTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightTop;
            Vector2 lineDiagonalPoint = startingTransformer.LeftBottom;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleRightTop);
        }
        static Transformer FuncScaleRightTop(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnHorizontalPoint,
                RightTop = returnPoint,
                RightBottom = returnVerticalPoint,
                LeftBottom = returnDiagonalPoint,
            };
        }

        private static Transformer ScaleRightBottom(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightBottom;
            Vector2 lineDiagonalPoint = startingTransformer.LeftTop;

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleRightBottom);
        }
        private static Transformer ScaleRightBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightBottom;
            Vector2 lineDiagonalPoint = startingTransformer.LeftTop;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleRightBottom);
        }
        static Transformer FuncScaleRightBottom(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnDiagonalPoint,
                RightTop = returnVerticalPoint,
                RightBottom = returnPoint,
                LeftBottom = returnHorizontalPoint,
            };
        }

        private static Transformer ScaleLeftBottom(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftBottom;
            Vector2 lineDiagonalPoint = startingTransformer.RightTop;

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleLeftBottom);
        }
        private static Transformer ScaleLeftBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftBottom;
            Vector2 lineDiagonalPoint = startingTransformer.RightTop;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer.FuncScaleLeftBottom);
        }
        static Transformer FuncScaleLeftBottom(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnVerticalPoint,
                RightTop = returnDiagonalPoint,
                RightBottom = returnHorizontalPoint,
                LeftBottom = returnPoint,
            };
        }



        // Scale
        private static Transformer ScaleRatio(Transformer startingTransformer, LinePD1C line)
        {
            Vector2 center = line.DiagonalPoint;
            Vector2 footPoint = Math.FootPoint(line.CanvasPoint, line.Point, center);

            float distance = new LineDistance(footPoint, line.Point, center);
            Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(distance, center);

            return Transformer.Multiplies(startingTransformer, scaleMatrix);
        }

        private static Vector2 ScaleVector(LinePD1C line)
        {
            Vector2 footPoint = Math.FootPoint(line.CanvasPoint, line.Point, line.DiagonalPoint);
            return footPoint - line.Point;
        }

    }
}