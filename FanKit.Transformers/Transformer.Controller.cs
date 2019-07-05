using System;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {
        /// <summary>
        /// It controls the transformation of transformer.
        /// </summary>
        /// <param name="mode"> TransformerMode </param>
        /// <param name="startingPoint"> starting point </param>
        /// <param name="point"> point </param>
        /// <param name="startingTransformer"> starting transformer </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isStepFrequency"> Step Frequency when spinning. </param>
        /// <returns> The controlled transformer. </returns>
        public static Transformer Controller(TransformerMode mode, Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isRatio = false, bool isCenter = false, bool isStepFrequency = false)
        {
            switch (mode)
            {
                case TransformerMode.None: return startingTransformer;

                case TransformerMode.Translation: return Transformer.Translation(startingPoint, point, startingTransformer);

                case TransformerMode.Rotation: return Transformer.Rotation(startingPoint, point, startingTransformer, isStepFrequency);

                case TransformerMode.SkewLeft: return Transformer.SkewLeft(startingPoint, point, startingTransformer, isCenter);
                case TransformerMode.SkewTop: return Transformer.SkewTop(startingPoint, point, startingTransformer, isCenter);
                case TransformerMode.SkewRight: return Transformer.SkewRight(startingPoint, point, startingTransformer, isCenter);
                case TransformerMode.SkewBottom: return Transformer.SkewBottom(startingPoint, point, startingTransformer, isCenter);

                case TransformerMode.ScaleLeft: return Transformer.ScaleLeft(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleTop: return Transformer.ScaleTop(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleRight: return Transformer.ScaleRight(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleBottom: return Transformer.ScaleBottom(point, startingTransformer, isRatio, isCenter);

                case TransformerMode.ScaleLeftTop: return Transformer.ScaleLeftTop(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleRightTop: return Transformer.ScaleRightTop(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleRightBottom: return Transformer.ScaleRightBottom(point, startingTransformer, isRatio, isCenter);
                case TransformerMode.ScaleLeftBottom: return Transformer.ScaleLeftBottom(point, startingTransformer, isRatio, isCenter);
            }

            return startingTransformer;
        }
        /// <summary>
        /// It controls the transformation of transformer.
        /// </summary>
        /// <param name="mode"> TransformerMode </param>
        /// <param name="startingPoint"> starting point </param>
        /// <param name="point"> point </param>
        /// <param name="startingTransformer"> starting transformer </param>
        /// <param name="inverseMatrix"> inverse matrix </param>
        /// <param name="isRatio"> Maintain a ratio when scaling.  </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isStepFrequency"> Step Frequency when spinning. </param>
        /// <returns> The controlled transformer. </returns>
        public static Transformer Controller(TransformerMode mode, Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio = false, bool isCenter = false, bool isStepFrequency = false)
        {
            switch (mode)
            {
                case TransformerMode.None: return startingTransformer;

                case TransformerMode.Translation: return Transformer.Translation(startingPoint, point, startingTransformer, inverseMatrix);

                case TransformerMode.Rotation: return Transformer.Rotation(startingPoint, point, startingTransformer, inverseMatrix, isStepFrequency);

                case TransformerMode.SkewLeft: return Transformer.SkewLeft(startingPoint, point, startingTransformer, isCenter);
                case TransformerMode.SkewTop: return Transformer.SkewTop(startingPoint, point, startingTransformer, isCenter);
                case TransformerMode.SkewRight: return Transformer.SkewRight(startingPoint, point, startingTransformer, isCenter);
                case TransformerMode.SkewBottom: return Transformer.SkewBottom(startingPoint, point, startingTransformer, isCenter);

                case TransformerMode.ScaleLeft: return Transformer.ScaleLeft(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleTop: return Transformer.ScaleTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRight: return Transformer.ScaleRight(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleBottom: return Transformer.ScaleBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);

                case TransformerMode.ScaleLeftTop: return Transformer.ScaleLeftTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRightTop: return Transformer.ScaleRightTop(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleRightBottom: return Transformer.ScaleRightBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);
                case TransformerMode.ScaleLeftBottom: return Transformer.ScaleLeftBottom(point, startingTransformer, inverseMatrix, isRatio, isCenter);
            }

            return startingTransformer;
        }



        //Translation
        private static Transformer Translation(Vector2 startingPoint, Vector2 point, Transformer startingTransformer)
        {
            Vector2 canvasStartingPoint = (startingPoint);
            Vector2 canvasPoint = (point);

            return Transformer.Add(startingTransformer, canvasPoint - canvasStartingPoint);
        }
        private static Transformer Translation(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix)
        {
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            return Transformer.Add(startingTransformer, canvasPoint - canvasStartingPoint);
        }



        //Rotation      
        private static Transformer Rotation(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isStepFrequency)
        {
            Vector2 canvasPoint = (point);
            Vector2 canvasStartingPoint = (startingPoint);
            Vector2 center = startingTransformer.Center;

            float canvasRadian = TransformerMath.VectorToRadians(canvasPoint - center);
            if (isStepFrequency) canvasRadian = TransformerMath.RadiansStepFrequency(canvasRadian);

            float canvasStartingRadian = TransformerMath.VectorToRadians(canvasStartingPoint - center);
            float radian = canvasRadian - canvasStartingRadian;

            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(radian, center);
            return Transformer.Multiplies(startingTransformer, rotationMatrix);
        }
        private static Transformer Rotation(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isStepFrequency)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 canvasStartingPoint = Vector2.Transform(startingPoint, inverseMatrix);
            Vector2 center = startingTransformer.Center;

            float canvasRadian = TransformerMath.VectorToRadians(canvasPoint - center);
            if (isStepFrequency) canvasRadian = TransformerMath.RadiansStepFrequency(canvasRadian);

            float canvasStartingRadian = TransformerMath.VectorToRadians(canvasStartingPoint - center);
            float radian = canvasRadian - canvasStartingRadian;

            Matrix3x2 rotationMatrix = Matrix3x2.CreateRotation(radian, center);
            return Transformer.Multiplies(startingTransformer, rotationMatrix);
        }



        //Skew
        private static Vector2 Skew(Vector2 startingPoint, Vector2 point, Vector2 linePoineA, Vector2 linePoineB)
        {
            Vector2 canvasStartingSkewPoint = TransformerMath.FootPoint(startingPoint, linePoineA, linePoineB);
            Vector2 canvasSkewPoint = TransformerMath.FootPoint(point, linePoineA, linePoineB);

            Vector2 vector = canvasSkewPoint - canvasStartingSkewPoint;
            Vector2 halfVector = vector / 2;

            return halfVector;
        }

        private static Transformer SkewLeft(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.LeftTop;
            Vector2 linePoineB = startingTransformer.LeftBottom;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.LeftTop += vector;
            startingTransformer.LeftBottom += vector;

            if (isCenter)
            {
                startingTransformer.RightTop -= vector;
                startingTransformer.RightBottom -= vector;
            }

            return startingTransformer;
        }
        private static Transformer SkewTop(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.LeftTop;
            Vector2 linePoineB = startingTransformer.RightTop;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.LeftTop += vector;
            startingTransformer.RightTop += vector;

            if (isCenter)
            {
                startingTransformer.RightBottom -= vector;
                startingTransformer.LeftBottom -= vector;
            }

            return startingTransformer;
        }
        private static Transformer SkewRight(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.RightTop;
            Vector2 linePoineB = startingTransformer.RightBottom;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.RightTop += vector;
            startingTransformer.RightBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.LeftBottom -= vector;
            }

            return startingTransformer;
        }
        private static Transformer SkewBottom(Vector2 startingPoint, Vector2 point, Transformer startingTransformer, bool isCenter)
        {
            Vector2 linePoineA = startingTransformer.LeftBottom;
            Vector2 linePoineB = startingTransformer.RightBottom;
            Vector2 vector = Transformer.Skew(startingPoint, point, linePoineA, linePoineB);

            startingTransformer.RightBottom += vector;
            startingTransformer.LeftBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.RightTop -= vector;
            }

            return startingTransformer;
        }



        //ScaleAround
        private static Transformer ScaleAround(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Transformer, bool, Vector2, Transformer> _func)
        {
            Vector2 canvasPoint = (point);
            Vector2 footPoint = TransformerMath.FootPoint(canvasPoint, linePoint, lineDiagonalPoint);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;

                LineDistance distance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(LineDistance.Scale(distance), center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 vector = footPoint - linePoint;
                return _func(startingTransformer, isCenter, vector);
            }
        }
        private static Transformer ScaleAround(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Transformer, bool, Vector2, Transformer> _func)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);
            Vector2 footPoint = TransformerMath.FootPoint(canvasPoint, linePoint, lineDiagonalPoint);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;

                LineDistance distance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(LineDistance.Scale(distance), center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 vector = footPoint - linePoint;
                return _func(startingTransformer, isCenter, vector);
            }
        }


        private static Transformer ScaleLeft(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterLeft;
            Vector2 lineDiagonalPoint = startingTransformer.CenterRight;

            return Transformer.ScaleAround(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeft);
        }
        private static Transformer ScaleLeft(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterLeft;
            Vector2 lineDiagonalPoint = startingTransformer.CenterRight;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeft);
        }
        static Transformer _funcScaleLeft(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.LeftTop += vector;
            startingTransformer.LeftBottom += vector;

            if (isCenter)
            {
                startingTransformer.RightTop -= vector;
                startingTransformer.RightBottom -= vector;
            }

            return startingTransformer;
        }

        private static Transformer ScaleTop(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterTop;
            Vector2 lineDiagonalPoint = startingTransformer.CenterBottom;

            return Transformer.ScaleAround(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleTop);
        }
        private static Transformer ScaleTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterTop;
            Vector2 lineDiagonalPoint = startingTransformer.CenterBottom;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleTop);
        }
        static Transformer _funcScaleTop(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.LeftTop += vector;
            startingTransformer.RightTop += vector;

            if (isCenter)
            {
                startingTransformer.LeftBottom -= vector;
                startingTransformer.RightBottom -= vector;
            }

            return startingTransformer;
        }

        private static Transformer ScaleRight(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterRight;
            Vector2 lineDiagonalPoint = startingTransformer.CenterLeft;

            return Transformer.ScaleAround(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRight);
        }
        private static Transformer ScaleRight(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterRight;
            Vector2 lineDiagonalPoint = startingTransformer.CenterLeft;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRight);
        }
        static Transformer _funcScaleRight(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.RightTop += vector;
            startingTransformer.RightBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.LeftBottom -= vector;
            }

            return startingTransformer;
        }

        private static Transformer ScaleBottom(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterBottom;
            Vector2 lineDiagonalPoint = startingTransformer.CenterTop;

            return Transformer.ScaleAround(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleBottom);
        }
        private static Transformer ScaleBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.CenterBottom;
            Vector2 lineDiagonalPoint = startingTransformer.CenterTop;

            return Transformer.ScaleAround(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleBottom);
        }
        static Transformer _funcScaleBottom(Transformer startingTransformer, bool isCenter, Vector2 vector)
        {
            startingTransformer.LeftBottom += vector;
            startingTransformer.RightBottom += vector;

            if (isCenter)
            {
                startingTransformer.LeftTop -= vector;
                startingTransformer.RightTop -= vector;
            }

            return startingTransformer;
        }



        //ScaleCorner
        private static Transformer ScaleCorner(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Vector2, Vector2, Vector2, Vector2, Transformer> _func)
        {
            Vector2 canvasPoint = (point);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;
                Vector2 footPoint = TransformerMath.FootPoint(canvasPoint, linePoint, center);

                LineDistance lineDistance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(LineDistance.Scale(lineDistance), center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 center = isCenter ? startingTransformer.Center * 2 - canvasPoint : lineDiagonalPoint;
                Vector2 horizontal = startingTransformer.CenterRight - startingTransformer.CenterLeft;
                Vector2 vertical = startingTransformer.CenterBottom - startingTransformer.CenterTop;

                Vector2 returnPoint = canvasPoint;
                Vector2 returnDiagonalPoint = center;
                Vector2 returnHorizontalPoint = TransformerMath.IntersectionPoint(canvasPoint, (canvasPoint - horizontal), (center + vertical), center);
                Vector2 returnVerticalPoint = TransformerMath.IntersectionPoint(canvasPoint, (canvasPoint - vertical), (center + horizontal), center);

                return _func(returnPoint, returnDiagonalPoint, returnHorizontalPoint, returnVerticalPoint);
            }
        }
        private static Transformer ScaleCorner(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter, Vector2 linePoint, Vector2 lineDiagonalPoint, Func<Vector2, Vector2, Vector2, Vector2, Transformer> _func)
        {
            Vector2 canvasPoint = Vector2.Transform(point, inverseMatrix);

            if (isRatio)
            {
                Vector2 center = isCenter ? startingTransformer.Center : lineDiagonalPoint;
                Vector2 footPoint = TransformerMath.FootPoint(canvasPoint, linePoint, center);

                LineDistance lineDistance = new LineDistance(footPoint, linePoint, center);
                Matrix3x2 scaleMatrix = Matrix3x2.CreateScale(LineDistance.Scale(lineDistance), center);

                return Transformer.Multiplies(startingTransformer, scaleMatrix);
            }
            else
            {
                Vector2 center = isCenter ? startingTransformer.Center * 2 - canvasPoint : lineDiagonalPoint;
                Vector2 horizontal = startingTransformer.CenterRight - startingTransformer.CenterLeft;
                Vector2 vertical = startingTransformer.CenterBottom - startingTransformer.CenterTop;

                Vector2 returnPoint = canvasPoint;
                Vector2 returnDiagonalPoint = center;
                Vector2 returnHorizontalPoint = TransformerMath.IntersectionPoint(canvasPoint, (canvasPoint - horizontal), (center + vertical), center);
                Vector2 returnVerticalPoint = TransformerMath.IntersectionPoint(canvasPoint, (canvasPoint - vertical), (center + horizontal), center);

                return _func(returnPoint, returnDiagonalPoint, returnHorizontalPoint, returnVerticalPoint);
            }
        }


        private static Transformer ScaleLeftTop(Vector2 point, Transformer startingTransformer, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftTop;
            Vector2 lineDiagonalPoint = startingTransformer.RightBottom;

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeftTop);
        }
        private static Transformer ScaleLeftTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftTop;
            Vector2 lineDiagonalPoint = startingTransformer.RightBottom;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeftTop);
        }
        static Transformer _funcScaleLeftTop(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
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

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRightTop);
        }
        private static Transformer ScaleRightTop(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightTop;
            Vector2 lineDiagonalPoint = startingTransformer.LeftBottom;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRightTop);
        }
        static Transformer _funcScaleRightTop(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
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

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRightBottom);
        }
        private static Transformer ScaleRightBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.RightBottom;
            Vector2 lineDiagonalPoint = startingTransformer.LeftTop;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleRightBottom);
        }
        static Transformer _funcScaleRightBottom(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
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

            return Transformer.ScaleCorner(point, startingTransformer, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeftBottom);
        }
        private static Transformer ScaleLeftBottom(Vector2 point, Transformer startingTransformer, Matrix3x2 inverseMatrix, bool isRatio, bool isCenter)
        {
            Vector2 linePoint = startingTransformer.LeftBottom;
            Vector2 lineDiagonalPoint = startingTransformer.RightTop;

            return Transformer.ScaleCorner(point, startingTransformer, inverseMatrix, isRatio, isCenter, linePoint, lineDiagonalPoint, Transformer._funcScaleLeftBottom);
        }
        static Transformer _funcScaleLeftBottom(Vector2 returnPoint, Vector2 returnDiagonalPoint, Vector2 returnHorizontalPoint, Vector2 returnVerticalPoint)
        {
            return new Transformer
            {
                LeftTop = returnVerticalPoint,
                RightTop = returnDiagonalPoint,
                RightBottom = returnHorizontalPoint,
                LeftBottom = returnPoint,
            };
        }


    }
}