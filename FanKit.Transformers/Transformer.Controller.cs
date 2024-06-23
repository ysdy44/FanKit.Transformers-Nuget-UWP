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
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.LeftTop,
                            CanvasPoint = point
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.LeftTop,
                            DiagonalPoint = startingTransformer.RightBottom,
                            CanvasPoint = point
                        });
                    }
                    else if (isCenter) return Transformer.FromLeftTop(startingTransformer, point);
                    else return Transformer.FromLeftTop(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.RightBottom,
                        CanvasPoint = point
                    });
                case TransformerMode.ScaleRightTop:
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.RightTop,
                            CanvasPoint = point
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.RightTop,
                            DiagonalPoint = startingTransformer.LeftBottom,
                            CanvasPoint = point
                        });
                    }
                    else if (isCenter) return Transformer.FromRightTop(startingTransformer, point);
                    else return Transformer.FromRightTop(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.LeftBottom,
                        CanvasPoint = point
                    });
                case TransformerMode.ScaleRightBottom:
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.RightBottom,
                            CanvasPoint = point
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.RightBottom,
                            DiagonalPoint = startingTransformer.LeftTop,
                            CanvasPoint = point
                        });
                    }
                    else if (isCenter) return Transformer.FromRightBottom(startingTransformer, point);
                    else return Transformer.FromRightBottom(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.LeftTop,
                        CanvasPoint = point
                    });
                case TransformerMode.ScaleLeftBottom:
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.LeftBottom,
                            CanvasPoint = point
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.LeftBottom,
                            DiagonalPoint = startingTransformer.RightTop,
                            CanvasPoint = point
                        });
                    }
                    else if (isCenter) return Transformer.FromLeftBottom(startingTransformer, point);
                    else return Transformer.FromLeftBottom(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.RightTop,
                        CanvasPoint = point
                    });

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
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.LeftTop,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.LeftTop,
                            DiagonalPoint = startingTransformer.RightBottom,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                    }
                    else if (isCenter) return Transformer.FromLeftTop(startingTransformer, Vector2.Transform(point, inverseMatrix));
                    else return Transformer.FromLeftTop(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.RightBottom,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    });
                case TransformerMode.ScaleRightTop:
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.RightTop,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.RightTop,
                            DiagonalPoint = startingTransformer.LeftBottom,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                    }
                    else if (isCenter) return Transformer.FromRightTop(startingTransformer, Vector2.Transform(point, inverseMatrix));
                    else return Transformer.FromRightTop(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.LeftBottom,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    });
                case TransformerMode.ScaleRightBottom:
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.RightBottom,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.RightBottom,
                            DiagonalPoint = startingTransformer.LeftTop,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                    }
                    else if (isCenter) return Transformer.FromRightBottom(startingTransformer, Vector2.Transform(point, inverseMatrix));
                    else return Transformer.FromRightBottom(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.LeftTop,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    });
                case TransformerMode.ScaleLeftBottom:
                    if (isRatio)
                    {
                        if (isCenter) return Transformer.ScaleCornerRatioCenter(startingTransformer, new LineP1C
                        {
                            Point = startingTransformer.LeftBottom,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                        else return Transformer.ScaleRatio(startingTransformer, new LinePD1C
                        {
                            Point = startingTransformer.LeftBottom,
                            DiagonalPoint = startingTransformer.RightTop,
                            CanvasPoint = Vector2.Transform(point, inverseMatrix)
                        });
                    }
                    else if (isCenter) return Transformer.FromLeftBottom(startingTransformer, Vector2.Transform(point, inverseMatrix));
                    else return Transformer.FromLeftBottom(startingTransformer, new LineD1C
                    {
                        DiagonalPoint = startingTransformer.RightTop,
                        CanvasPoint = Vector2.Transform(point, inverseMatrix)
                    });

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

            return startingTransformer.Rotate(radian, center);
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
            return startingTransformer.Scale(distance, center);
        }

        // ScaleCorner
        private static Transformer ScaleCornerRatioCenter(Transformer startingTransformer, LineP1C line)
        {
            Vector2 center = startingTransformer.Center;
            Vector2 footPoint = Math.FootPoint(line.CanvasPoint, line.Point, center);

            float distance = new LineDistance(footPoint, line.Point, center);
            return startingTransformer.Scale(distance, center);
        }

        // Scale
        private static Transformer ScaleRatio(Transformer startingTransformer, LinePD1C line)
        {
            Vector2 center = line.DiagonalPoint;
            Vector2 footPoint = Math.FootPoint(line.CanvasPoint, line.Point, center);

            float distance = new LineDistance(footPoint, line.Point, center);
            return startingTransformer.Scale(distance, center);
        }

        private static Vector2 ScaleVector(LinePD1C line)
        {
            Vector2 footPoint = Math.FootPoint(line.CanvasPoint, line.Point, line.DiagonalPoint);
            return footPoint - line.Point;
        }

    }
}