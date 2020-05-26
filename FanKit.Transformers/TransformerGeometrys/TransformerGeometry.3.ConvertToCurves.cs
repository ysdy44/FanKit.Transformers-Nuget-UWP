using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {

        #region Dount


        /// <summary>
        /// Convert to curves from dount.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="holeRadius"> The hole-radius. </param>
        /// <returns> The product geometry. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromDount(ITransformerLTRB transformer, float holeRadius)
        {
            bool zeroHoleRadius = holeRadius == 0;

            if (zeroHoleRadius)
                return TransformerGeometry.ConvertToCurvesFromEllipse(transformer);
            else
                return TransformerGeometry._convertToCurvesFromDount
                (
                    holeRadius,
                    transformer.Center,
                    transformer.CenterLeft,
                    transformer.CenterTop,
                    transformer.CenterRight,
                    transformer.CenterBottom
                );
        }

        /// <summary>
        /// Convert to curves from dount.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="holeRadius"> The hole-radius. </param>
        /// <returns> The product geometry. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromDount(ITransformerLTRB transformer, Matrix3x2 matrix, float holeRadius)
        {
            bool zeroHoleRadius = holeRadius == 0;

            if (zeroHoleRadius)
                return TransformerGeometry.ConvertToCurvesFromEllipse(transformer, matrix);
            else
                return TransformerGeometry._convertToCurvesFromDount
                (
                    holeRadius,
                    Vector2.Transform(transformer.Center, matrix),
                    Vector2.Transform(transformer.CenterLeft, matrix),
                    Vector2.Transform(transformer.CenterTop, matrix),
                    Vector2.Transform(transformer.CenterRight, matrix),
                    Vector2.Transform(transformer.CenterBottom, matrix)
                );
        }

        private static IEnumerable<IEnumerable<Node>> _convertToCurvesFromDount(
            float holeRadius, 
            Vector2 center, 
            Vector2 centerLeft, 
            Vector2 centerTop, 
            Vector2 centerRight, 
            Vector2 centerBottom)
        {
            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromEllipse
                (
                    Vector2.Lerp(center, centerLeft, holeRadius),
                    Vector2.Lerp(center, centerTop, holeRadius),
                    Vector2.Lerp(center, centerRight, holeRadius),
                    Vector2.Lerp(center, centerBottom, holeRadius)
                 ),                 
                TransformerGeometry._convertToCurveFromEllipse
                (
                    centerLeft,
                    centerTop,
                    centerRight,
                    centerBottom
                 )
            };
        }


        #endregion


        #region Pie


        /// <summary>
        /// Convert to curves from pie.
        /// </summary>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromPie(ITransformerLTRB transformer, float sweepAngle)
        {
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
                return TransformerGeometry.ConvertToCurvesFromEllipse(transformer);
            else if (sweepAngle == FanKit.Math.PiTwice)
            {
                Node center = new Node { Point = transformer.Center };
                Node right = new Node { Point = transformer.CenterRight };

                return new List<IEnumerable<Node>>
                {
                    new List<Node> { center, right, right, center }
                };
            }
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

                return new List<IEnumerable<Node>>
                {
                    TransformerGeometry._convertToCurveFromPie(oneMatrix, sweepAngle)
                };
            }
        }

        /// <summary>
        /// Convert to curves from pie.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromPie(ITransformerLTRB transformer, Matrix3x2 matrix, float sweepAngle)
        {
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
                return TransformerGeometry.ConvertToCurvesFromEllipse(transformer, matrix);
            else if (sweepAngle == FanKit.Math.PiTwice)
            {
                Node center = new Node { Point = Vector2.Transform(transformer.Center, matrix) };
                Node right = new Node { Point = Vector2.Transform(transformer.CenterRight, matrix) };

                return new List<IEnumerable<Node>>
                {
                    new List<Node> { center, right, right, center }
                };
            }
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
                Matrix3x2 oneMatrix2 = oneMatrix * matrix;

                return new List<IEnumerable<Node>>
                {
                    TransformerGeometry._convertToCurveFromPie(oneMatrix, sweepAngle)
                };
            }
        }

        private static IEnumerable<Node> _convertToCurveFromPie(Matrix3x2 oneMatrix, float sweepAngle)
        {
            float z552 = TransformerGeometry._z552;

            // [Sweep point] distance / [Control point] distance
            float scale = (FanKit.Math.PiTwice - sweepAngle) % FanKit.Math.PiOver2 / FanKit.Math.PiOver2;
            float z552Scale = _z552 * scale;


            ////////////////////////////


            // [Sweep point] and [Tangent point]
            float x = (float)System.Math.Cos(sweepAngle);
            float y = (float)System.Math.Sin(sweepAngle);

            Vector2 sweepUnit = new Vector2(x, y);
            Vector2 tangentUnit = new Vector2(-y, x) * z552Scale + sweepUnit;

            Vector2 sweepPoint = Vector2.Transform(sweepUnit, oneMatrix);
            Vector2 tangentPoint = Vector2.Transform(tangentUnit, oneMatrix);


            Node sweepNode = new Node// [Sweep point]
            {
                Point = sweepPoint,
                LeftControlPoint = sweepPoint,//2
                RightControlPoint = tangentPoint,//1
                IsSmooth = true,
            };


            ////////////// First Quadrant //////////////


            Vector2 rightPoint = Vector2.Transform(new Vector2(1, 0), oneMatrix);

            Vector2 rightControlPoint2 = Vector2.Transform(new Vector2(1, -z552), oneMatrix);//2
            //Vector2 rightControlPoint1 = Vector2.Transform(new Vector2(1, z552), oneMatrix);//1

            Vector2 rightControlPoint2Sweep = Vector2.Transform(new Vector2(1, -z552Scale), oneMatrix);//2
            //Vector2 rightControlPoint1Sweep = Vector2.Transform(new Vector2(1, -z552Scale), oneMatrix);//1

            Node center = new Node { Point = oneMatrix.Translation };// [Center point]
            List<Node> nodes = new List<Node> { center };

            //360 To 270
            if (sweepAngle > FanKit.Math.Pi + FanKit.Math.PiOver2)
            {
                Node rightNodeSweep = new Node//Right: [Starting point] with [Sweep point]
                {
                    Point = rightPoint,
                    LeftControlPoint = rightControlPoint2Sweep,//2
                    RightControlPoint = rightPoint,//1
                    IsSmooth = true,
                };

                nodes.Add(rightNodeSweep);//Right: [Starting point] with [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(center);// [Center point]
                return nodes;
            }


            Node rightNodePerpendicular = new Node//Right: [Perpendicular point]
            {
                Point = rightPoint,
                LeftControlPoint = rightControlPoint2,//2
                RightControlPoint = rightPoint,//1
                IsSmooth = true,
            };
            nodes.Add(rightNodePerpendicular);//Right: [Perpendicular point]


            ////////////// Second Quadrant //////////////


            Vector2 topPoint = Vector2.Transform(new Vector2(0, -1), oneMatrix);

            Vector2 topControlPoint2 = Vector2.Transform(new Vector2(-z552, -1), oneMatrix);//2
            Vector2 topControlPoint1 = Vector2.Transform(new Vector2(z552, -1), oneMatrix);//1

            Vector2 topControlPoint2Sweep = Vector2.Transform(new Vector2(-z552Scale, -1), oneMatrix);
            //Vector2 topControlPoint1Sweep = Vector2.Transform(new Vector2(z552Scale, -1), oneMatrix);


            //270
            if (sweepAngle == FanKit.Math.Pi + FanKit.Math.PiOver2)
            {
                Node topNodePerpendicular = new Node//Top: [Perpendicular point]
                {
                    Point = topPoint,
                    LeftControlPoint = topPoint,//2
                    RightControlPoint = topControlPoint1,//1
                    IsSmooth = true,
                };

                nodes.Add(topNodePerpendicular);//Top: [Perpendicular point]
                nodes.Add(center);// [Center point]
                return nodes;
            }

            //270 To 180
            if (sweepAngle > FanKit.Math.Pi)
            {
                Node topNodeSweep = new Node//Top: [Sweep point]
                {
                    Point = topPoint,
                    LeftControlPoint = topControlPoint2Sweep,//2
                    RightControlPoint = topControlPoint1,//1
                    IsSmooth = true,
                };

                nodes.Add(topNodeSweep);//Top: [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(center);// [Center point]
                return nodes;
            }


            Node topNodePassing = new Node//Top: [Passing point]
            {
                Point = topPoint,
                LeftControlPoint = topControlPoint2,//2
                RightControlPoint = topControlPoint1,//1
                IsSmooth = true,
            };
            nodes.Add(topNodePassing);//Top: [Passing point]


            ////////////// Third Quadrant //////////////


            Vector2 leftPoint = Vector2.Transform(new Vector2(-1, 0), oneMatrix);

            Vector2 leftControlPoint2 = Vector2.Transform(new Vector2(-1, z552), oneMatrix);//2
            Vector2 leftControlPoint1 = Vector2.Transform(new Vector2(-1, -z552), oneMatrix);//1

            Vector2 leftControlPoint2Sweep = Vector2.Transform(new Vector2(-1, z552Scale), oneMatrix);//2
            //Vector2 leftControlPoint1Sweep = Vector2.Transform(new Vector2(-1, -z552Scale), oneMatrix);//1


            //180
            if (sweepAngle == FanKit.Math.Pi)
            {
                Node leftNodePerpendicular = new Node//Left: [Perpendicular point]
                {
                    Point = leftPoint,
                    LeftControlPoint = leftPoint,//2
                    RightControlPoint = leftControlPoint1,//1
                    IsSmooth = true,
                };

                nodes.Add(leftNodePerpendicular);//Left: [Perpendicular point]
                nodes.Add(center);// [Center point]
                return nodes;
            }


            //180 To 90
            if (sweepAngle > FanKit.Math.PiOver2)
            {
                Node leftNodeSweep = new Node//Left: with [Sweep point]
                {
                    Point = leftPoint,
                    LeftControlPoint = leftControlPoint2Sweep,//2
                    RightControlPoint = leftControlPoint1,//1
                    IsSmooth = true,
                };

                nodes.Add(leftNodeSweep);//Left: with [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(center);// [Center point]
                return nodes;
            }


            Node leftNodePassing = new Node//Left: [Passing point]
            {
                Point = leftPoint,
                LeftControlPoint = leftControlPoint2,//2
                RightControlPoint = leftControlPoint1,//1
                IsSmooth = true,
            };
            nodes.Add(leftNodePassing);//Left: [Passing point]


            ////////////// Fourth Quadrant //////////////


            Vector2 bottomPoint = Vector2.Transform(new Vector2(0, 1), oneMatrix);

            //Vector2 bottomControlPoint2 = Vector2.Transform(new Vector2(z552, 1), oneMatrix);//2
            Vector2 bottomControlPoint1 = Vector2.Transform(new Vector2(-z552, 1), oneMatrix);//1

            Vector2 bottomControlPoint2Sweep = Vector2.Transform(new Vector2(z552Scale, 1), oneMatrix);//2
            //Vector2 bottomControlPoint1Sweep = Vector2.Transform(new Vector2(-z552Scale, 1), oneMatrix);//1


            //90
            if (sweepAngle == FanKit.Math.PiOver2)
            {
                Node bottomNodePerpendicular = new Node//Bottom: [Perpendicular point]
                {
                    Point = bottomPoint,
                    LeftControlPoint = bottomPoint,//2
                    RightControlPoint = bottomControlPoint1,//1
                    IsSmooth = true,
                };

                nodes.Add(bottomNodePerpendicular);//Bottom: [Perpendicular point]
                nodes.Add(center);// [Center point]
                return nodes;
            }


            //90 To 0
            if (sweepAngle > 0)
            {
                Node bottomNodeSweep = new Node//Bottom: [Sweep point]
                {
                    Point = bottomPoint,
                    LeftControlPoint = bottomControlPoint2Sweep,//2
                    RightControlPoint = bottomControlPoint1,//1
                    IsSmooth = true,
                };

                nodes.Add(bottomNodeSweep);//Bottom: [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(center);// [Center point]
                return nodes;
            }


            ////////////////////////////


            nodes.Add(center);// [Center point]
            return null;
        }

        #endregion


        #region Cookie


        /// <summary>
        /// Convert to curves from cookie.
        /// </summary>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromCookie(ITransformerLTRB transformer, float innerRadius, float sweepAngle)
        {
            bool zeroInnerRadius = innerRadius == 0;
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
            {
                if (zeroInnerRadius)
                    return TransformerGeometry.ConvertToCurvesFromEllipse(transformer);
                else
                    return TransformerGeometry._convertToCurvesFromDount
                    (
                        innerRadius,
                        transformer.Center,
                        transformer.CenterLeft,
                        transformer.CenterTop,
                        transformer.CenterRight,
                        transformer.CenterBottom
                    );
            }
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

                if (zeroInnerRadius)
                    return new List<IEnumerable<Node>>
                    {
                        TransformerGeometry._convertToCurveFromPie(oneMatrix, sweepAngle)
                    };
                else
                    return new List<IEnumerable<Node>>
                    {
                        TransformerGeometry._convertToCurveFromCookie(oneMatrix, innerRadius, sweepAngle)
                    };
            }
        }

        /// <summary>
        /// Convert to curves from cookie.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromCookie( ITransformerLTRB transformer, Matrix3x2 matrix, float innerRadius, float sweepAngle)
        {
            bool zeroInnerRadius = innerRadius == 0;
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
            {
                if (zeroInnerRadius)
                    return TransformerGeometry.ConvertToCurvesFromEllipse(transformer, matrix);
                else
                    return TransformerGeometry._convertToCurvesFromDount
                    (
                        innerRadius,
                        Vector2.Transform(transformer.Center, matrix),
                        Vector2.Transform(transformer.CenterLeft, matrix),
                        Vector2.Transform(transformer.CenterTop, matrix),
                        Vector2.Transform(transformer.CenterRight, matrix),
                        Vector2.Transform(transformer.CenterBottom, matrix)
                    );
            }
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
                Matrix3x2 oneMatrix2 = oneMatrix * matrix;

                if (zeroInnerRadius)
                    return new List<IEnumerable<Node>>
                    {
                        TransformerGeometry._convertToCurveFromPie(oneMatrix2, sweepAngle)
                    };
                else
                    return new List<IEnumerable<Node>>
                    {
                        TransformerGeometry._convertToCurveFromCookie(oneMatrix2, innerRadius, sweepAngle)
                    };
            }
        }

        private static IEnumerable<Node> _convertToCurveFromCookie(Matrix3x2 oneMatrix, float innerRadius, float sweepAngle)
        {
            float z552 = TransformerGeometry._z552;
            float z552_InnerRadius = TransformerGeometry._z552 * innerRadius;

            // [Sweep point] distance / [Control point] distance
            float scale = (FanKit.Math.PiTwice - sweepAngle) % FanKit.Math.PiOver2 / FanKit.Math.PiOver2;
            float z552Scale = _z552 * scale;
            float z552Scale_InnerRadius = z552Scale * innerRadius;


            ////////////////////////////


            // [Sweep point] and [Tangent point]
            float x = (float)System.Math.Cos(sweepAngle);
            float y = (float)System.Math.Sin(sweepAngle);

            Vector2 sweepUnit = new Vector2(x, y);
            Vector2 tangentUnit = new Vector2(-y, x) * z552Scale + sweepUnit;

            Vector2 sweepPoint = Vector2.Transform(sweepUnit, oneMatrix);
            Vector2 sweepPoint_InnerRadius = Vector2.Transform(sweepUnit * innerRadius, oneMatrix);
            Vector2 tangentPoint = Vector2.Transform(tangentUnit, oneMatrix);
            Vector2 tangentPoint_InnerRadius = Vector2.Transform(tangentUnit * innerRadius, oneMatrix);


            Node sweepNode = new Node// [Sweep point]
            {
                Point = sweepPoint,
                LeftControlPoint = sweepPoint,//2
                RightControlPoint = tangentPoint,//1
                IsSmooth = true,
            };
            Node sweepNode_InnerRadius = new Node// [Sweep point] InnerRadius
            {
                Point = sweepPoint_InnerRadius,
                LeftControlPoint = tangentPoint_InnerRadius,//2
                RightControlPoint = sweepPoint_InnerRadius,//1
                IsSmooth = true,
            };


            ////////////// First Quadrant //////////////


            Vector2 rightPoint = Vector2.Transform(new Vector2(1, 0), oneMatrix);
            Vector2 rightPoint_InnerRadius = Vector2.Transform(new Vector2(innerRadius, 0), oneMatrix);

            Vector2 rightControlPoint2 = Vector2.Transform(new Vector2(1, -z552), oneMatrix);//2
            //Vector2 rightControlPoint2_InnerRadius = Vector2.Transform(new Vector2(innerRadius, z552_InnerRadius), oneMatrix);//1
            //Vector2 rightControlPoint1 = Vector2.Transform(new Vector2(1, z552), oneMatrix);//1
            Vector2 rightControlPoint1_InnerRadius = Vector2.Transform(new Vector2(innerRadius, -z552_InnerRadius), oneMatrix);//2

            Vector2 rightControlPoint2Sweep = Vector2.Transform(new Vector2(1, -z552Scale), oneMatrix);//2
            //Vector2 rightControlPoint2Sweep_InnerRadius = Vector2.Transform(new Vector2(innerRadius, -z552Scale_InnerRadius), oneMatrix);//2
            //Vector2 rightControlPoint1Sweep = Vector2.Transform(new Vector2(1, -z552Scale), oneMatrix);//1
            Vector2 rightControlPoint1Sweep_InnerRadius = Vector2.Transform(new Vector2(innerRadius, -z552Scale_InnerRadius), oneMatrix);//1

            List<Node> nodes = new List<Node> { new Node { Point = rightPoint_InnerRadius } };

            //360 To 270
            if (sweepAngle > FanKit.Math.Pi + FanKit.Math.PiOver2)
            {
                Node rightNodeSweep = new Node//Right: [Starting point] with [Sweep point]
                {
                    Point = rightPoint,
                    LeftControlPoint = rightControlPoint2Sweep,//2
                    RightControlPoint = rightPoint,//1
                    IsSmooth = true,
                };
                Node rightNodeSweep_InnerRadius = new Node//Right: [Starting point] with [Sweep point] InnerRadius
                {
                    Point = rightPoint_InnerRadius,
                    LeftControlPoint = rightPoint_InnerRadius,//2
                    RightControlPoint = rightControlPoint1Sweep_InnerRadius,//1
                    IsSmooth = true,
                };

                nodes.Add(rightNodeSweep);//Right: [Starting point] with [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(sweepNode_InnerRadius);// [Sweep point] InnerRadius
                nodes.Add(rightNodeSweep_InnerRadius);//Right: [Starting point] with [Sweep point] InnerRadius
                return nodes;
            }


            Node rightNodePerpendicular = new Node//Right: [Perpendicular point]
            {
                Point = rightPoint,
                LeftControlPoint = rightControlPoint2,//2
                RightControlPoint = rightPoint,//1
                IsSmooth = true,
            };
            Node rightNodePerpendicular_InnerRadius = new Node//Right: [Perpendicular point] InnerRadius
            {
                Point = rightPoint_InnerRadius,
                LeftControlPoint = rightPoint_InnerRadius,//2
                RightControlPoint = rightControlPoint1_InnerRadius,//1
                IsSmooth = true,
            };
            nodes.Add(rightNodePerpendicular);//Right: [Perpendicular point]


            ////////////// Second Quadrant //////////////


            Vector2 topPoint = Vector2.Transform(new Vector2(0, -1), oneMatrix);
            Vector2 topPoint_InnerRadius = Vector2.Transform(new Vector2(0, -innerRadius), oneMatrix);

            Vector2 topControlPoint2 = Vector2.Transform(new Vector2(-z552, -1), oneMatrix);//2
            Vector2 topControlPoint2_InnerRadius = Vector2.Transform(new Vector2(z552_InnerRadius, -innerRadius), oneMatrix);//2
            Vector2 topControlPoint1 = Vector2.Transform(new Vector2(z552, -1), oneMatrix);//1
            Vector2 topControlPoint1_InnerRadius = Vector2.Transform(new Vector2(-z552_InnerRadius, -innerRadius), oneMatrix);//1

            Vector2 topControlPoint2Sweep = Vector2.Transform(new Vector2(-z552Scale, -1), oneMatrix);
            //Vector2 topControlPoint2Sweep_InnerRadius = Vector2.Transform(new Vector2(z552Scale_InnerRadius, -innerRadius), oneMatrix);
            //Vector2 topControlPoint1Sweep = Vector2.Transform(new Vector2(z552Scale, -1), oneMatrix);
            Vector2 topControlPoint1Sweep_InnerRadius = Vector2.Transform(new Vector2(-z552Scale_InnerRadius, -innerRadius), oneMatrix);


            //270
            if (sweepAngle == FanKit.Math.Pi + FanKit.Math.PiOver2)
            {
                Node topNodePerpendicular = new Node//Top: [Perpendicular point]
                {
                    Point = topPoint,
                    LeftControlPoint = topPoint,//2
                    RightControlPoint = topControlPoint1,//1
                    IsSmooth = true,
                };
                Node topNodePerpendicular_InnerRadius = new Node//Top: [Perpendicular point] InnerRadius
                {
                    Point = topPoint_InnerRadius,
                    LeftControlPoint = topControlPoint2_InnerRadius,//2
                    RightControlPoint = topPoint_InnerRadius,//1
                    IsSmooth = true,
                };

                nodes.Add(topNodePerpendicular);//Top: [Perpendicular point]
                nodes.Add(topNodePerpendicular_InnerRadius);//Top: [Perpendicular point] InnerRadius
                nodes.Add(rightNodePerpendicular_InnerRadius);//Right: [Perpendicular point] InnerRadius
                return nodes;
            }

            //270 To 180
            if (sweepAngle > FanKit.Math.Pi)
            {
                Node topNodeSweep = new Node//Top: [Sweep point]
                {
                    Point = topPoint,
                    LeftControlPoint = topControlPoint2Sweep,//2
                    RightControlPoint = topControlPoint1,//1
                    IsSmooth = true,
                };
                Node topNodeSweep_InnerRadius = new Node//Top: [Sweep point] InnerRadius 
                {
                    Point = topPoint_InnerRadius,
                    LeftControlPoint = topControlPoint2_InnerRadius,//2
                    RightControlPoint = topControlPoint1Sweep_InnerRadius,//1
                    IsSmooth = true,
                };

                nodes.Add(topNodeSweep);//Top: [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(sweepNode_InnerRadius);// [Sweep point] InnerRadius
                nodes.Add(topNodeSweep_InnerRadius);//Top: [Sweep point] InnerRadius 
                nodes.Add(rightNodePerpendicular_InnerRadius);//Right: [Perpendicular point] InnerRadius
                return nodes;
            }


            Node topNodePassing = new Node//Top: [Passing point]
            {
                Point = topPoint,
                LeftControlPoint = topControlPoint2,//2
                RightControlPoint = topControlPoint1,//1
                IsSmooth = true,
            };
            Node topNodePassing_InnerRadius = new Node//Top: [Passing point] InnerRadius
            {
                Point = topPoint_InnerRadius,
                LeftControlPoint = topControlPoint2_InnerRadius,//2
                RightControlPoint = topControlPoint1_InnerRadius,//1
                IsSmooth = true,
            };
            nodes.Add(topNodePassing);//Top: [Passing point]


            ////////////// Third Quadrant //////////////


            Vector2 leftPoint = Vector2.Transform(new Vector2(-1, 0), oneMatrix);
            Vector2 leftPoint_InnerRadius = Vector2.Transform(new Vector2(-innerRadius, 0), oneMatrix);

            Vector2 leftControlPoint2 = Vector2.Transform(new Vector2(-1, z552), oneMatrix);//2
            Vector2 leftControlPoint2_InnerRadius = Vector2.Transform(new Vector2(-innerRadius, -z552_InnerRadius), oneMatrix);//2
            Vector2 leftControlPoint1 = Vector2.Transform(new Vector2(-1, -z552), oneMatrix);//1
            Vector2 leftControlPoint1_InnerRadius = Vector2.Transform(new Vector2(-innerRadius, z552_InnerRadius), oneMatrix);//1

            Vector2 leftControlPoint2Sweep = Vector2.Transform(new Vector2(-1, z552Scale), oneMatrix);//2
            //Vector2 leftControlPoint2Sweep_InnerRadius = Vector2.Transform(new Vector2(-innerRadius, -z552Scale_InnerRadius), oneMatrix);//2
           //Vector2 leftControlPoint1Sweep = Vector2.Transform(new Vector2(-1, -z552Scale), oneMatrix);//1
            Vector2 leftControlPoint1Sweep_InnerRadius = Vector2.Transform(new Vector2(-innerRadius, z552Scale_InnerRadius), oneMatrix);//1


            //180
            if (sweepAngle == FanKit.Math.Pi)
            {
                Node leftNodePerpendicular = new Node//Left: [Perpendicular point]
                {
                    Point = leftPoint,
                    LeftControlPoint = leftPoint,//2
                    RightControlPoint = leftControlPoint1,//1
                    IsSmooth = true,
                };
                Node leftNodePerpendicular_InnerRadius = new Node//Left: [Perpendicular point] InnerRadius
                {
                    Point = leftPoint_InnerRadius,
                    LeftControlPoint = leftControlPoint2_InnerRadius,//2
                    RightControlPoint = leftPoint_InnerRadius,//1
                    IsSmooth = true,
                };

                nodes.Add(leftNodePerpendicular);//Left: [Perpendicular point]
                nodes.Add(leftNodePerpendicular_InnerRadius);//Left: [Perpendicular point] InnerRadius
                nodes.Add(topNodePassing_InnerRadius);//Top: [Passing point] InnerRadius
                nodes.Add(rightNodePerpendicular_InnerRadius);//Right: [Perpendicular point] InnerRadius
                return nodes;
            }


            //180 To 90
            if (sweepAngle > FanKit.Math.PiOver2)
            {
                Node leftNodeSweep = new Node//Left: with [Sweep point]
                {
                    Point = leftPoint,
                    LeftControlPoint = leftControlPoint2Sweep,//2
                    RightControlPoint = leftControlPoint1,//1
                    IsSmooth = true,
                };
                Node leftNodeSweep_InnerRadius = new Node//Left: [Sweep point] InnerRadius
                {
                    Point = leftPoint_InnerRadius,
                    LeftControlPoint = leftControlPoint2_InnerRadius,//2
                    RightControlPoint = leftControlPoint1Sweep_InnerRadius,//1
                    IsSmooth = true,
                };
                
                nodes.Add(leftNodeSweep);//Left: with [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(sweepNode_InnerRadius);// [Sweep point] InnerRadius
                nodes.Add(leftNodeSweep_InnerRadius);//Left: [Sweep point] InnerRadius
                nodes.Add(topNodePassing_InnerRadius);//Top: [Passing point] InnerRadius 
                nodes.Add(rightNodePerpendicular_InnerRadius);//Right: [Perpendicular point] InnerRadius
                return nodes;
            }


            Node leftNodePassing = new Node//Left: [Passing point]
            {
                Point = leftPoint,
                LeftControlPoint = leftControlPoint2,//2
                RightControlPoint = leftControlPoint1,//1
                IsSmooth = true,
            };
            Node leftNodePassing_InnerRadius = new Node//Left: [Passing point] InnerRadius
            {
                Point = leftPoint_InnerRadius,
                LeftControlPoint = leftControlPoint2_InnerRadius,//2
                RightControlPoint = leftControlPoint1_InnerRadius,//1
                IsSmooth = true,
            };
            nodes.Add(leftNodePassing);//Left: [Passing point]


            ////////////// Fourth Quadrant //////////////


            Vector2 bottomPoint = Vector2.Transform(new Vector2(0, 1), oneMatrix);
            Vector2 bottomPoint_InnerRadius = Vector2.Transform(new Vector2(0, innerRadius), oneMatrix);

            //Vector2 bottomControlPoint2 = Vector2.Transform(new Vector2(z552, 1), oneMatrix);//2
            Vector2 bottomControlPoint2_InnerRadius = Vector2.Transform(new Vector2(-z552_InnerRadius, innerRadius), oneMatrix);//2
            Vector2 bottomControlPoint1 = Vector2.Transform(new Vector2(-z552, 1), oneMatrix);//1
            //Vector2 bottomControlPoint1_InnerRadius = Vector2.Transform(new Vector2(z552_InnerRadius, innerRadius), oneMatrix);//1

            Vector2 bottomControlPoint2Sweep = Vector2.Transform(new Vector2(z552Scale,1), oneMatrix);//2
            //Vector2 bottomControlPoint2Sweep_InnerRadius = Vector2.Transform(new Vector2(-z552Scale_InnerRadius, innerRadius), oneMatrix);//2
            //Vector2 bottomControlPoint1Sweep = Vector2.Transform(new Vector2(-z552Scale, 1), oneMatrix);//1
            Vector2 bottomControlPoint1Sweep_InnerRadius = Vector2.Transform(new Vector2(z552Scale_InnerRadius, innerRadius), oneMatrix);//1


            //90
            if (sweepAngle == FanKit.Math.PiOver2)
            {
                Node bottomNodePerpendicular = new Node//Bottom: [Perpendicular point]
                {
                    Point = bottomPoint,
                    LeftControlPoint = bottomPoint,//2
                    RightControlPoint = bottomControlPoint1,//1
                    IsSmooth = true,
                };
                Node bottomNodePerpendicular_InnerRadius = new Node//Bottom: [Perpendicular point] InnerRadius
                {
                    Point = bottomPoint_InnerRadius,
                    LeftControlPoint = bottomControlPoint2_InnerRadius,//2
                    RightControlPoint = bottomPoint_InnerRadius,//1
                    IsSmooth = true,
                };

                nodes.Add(bottomNodePerpendicular);//Bottom: [Perpendicular point]
                nodes.Add(bottomNodePerpendicular_InnerRadius);//Bottom: [Perpendicular point] InnerRadius
                nodes.Add(leftNodePassing_InnerRadius);//Left: [Passing point] InnerRadius
                nodes.Add(topNodePassing_InnerRadius);//Top: [Passing point] InnerRadius
                nodes.Add(rightNodePerpendicular_InnerRadius);//Right: [Perpendicular point] InnerRadius
                return nodes;
            }


            //90 To 0
            if (sweepAngle > 0)
            {
                Node bottomNodeSweep = new Node//Bottom: [Sweep point]
                {
                    Point = bottomPoint,
                    LeftControlPoint = bottomControlPoint2Sweep,//2
                    RightControlPoint = bottomControlPoint1,//1
                    IsSmooth = true,
                };
                Node bottomNodeSweep_InnerRadius = new Node//Bottom: [Sweep point] InnerRadius
                {
                    Point = bottomPoint_InnerRadius,
                    LeftControlPoint = bottomControlPoint2_InnerRadius,//2
                    RightControlPoint = bottomControlPoint1Sweep_InnerRadius,//1
                    IsSmooth = true,
                };

                nodes.Add(bottomNodeSweep);//Bottom: [Sweep point]
                nodes.Add(sweepNode);// [Sweep point]
                nodes.Add(sweepNode_InnerRadius);// [Sweep point] InnerRadius
                nodes.Add(bottomNodeSweep_InnerRadius);//Bottom: [Sweep point] InnerRadius
                nodes.Add(leftNodePassing_InnerRadius);//Left: [Passing point] InnerRadius
                nodes.Add(topNodePassing_InnerRadius);//Top: [Passing point] InnerRadius
                nodes.Add(rightNodePerpendicular_InnerRadius);//Right: [Perpendicular point] InnerRadius
                return nodes;
            }


            ////////////////////////////


            return null;
        }


        #endregion
               
    }
}