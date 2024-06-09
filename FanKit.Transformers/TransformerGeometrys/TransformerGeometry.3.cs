using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class TransformerGeometry
    {

        #region Donut


        /// <summary>
        /// Create a new donut geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="holeRadius"> The hole-radius. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateDonut(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, float holeRadius)
        {
            bool zeroHoleRadius = holeRadius == 0;
            CanvasGeometry outter = TransformerGeometry.CreateEllipse(resourceCreator, bounds);

            if (zeroHoleRadius)
                return outter;
            else
            {
                Vector2 center = bounds.Center;

                return TransformerGeometry.CreateDonutCore(outter, holeRadius, center);
            }
        }

        /// <summary>
        /// Create a new donut geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="holeRadius"> The hole-radius. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateDonut(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, Matrix3x2 matrix, float holeRadius)
        {
            bool zeroHoleRadius = holeRadius == 0;
            CanvasGeometry outter = TransformerGeometry.CreateEllipse(resourceCreator, bounds, matrix);

            if (zeroHoleRadius)
                return outter;
            else
            {
                Vector2 center = Vector2.Transform(bounds.Center, matrix);

                return TransformerGeometry.CreateDonutCore(outter, holeRadius, center);
            }
        }

        private static CanvasGeometry CreateDonutCore(CanvasGeometry outter, float holeRadius, Vector2 center)
        {
            // Donut
            Matrix3x2 holeMatrix = Matrix3x2.CreateTranslation(-center) * Matrix3x2.CreateScale(holeRadius) * Matrix3x2.CreateTranslation(center);
            return outter.CombineWith(outter, holeMatrix, CanvasGeometryCombine.Exclude);
        }


        #endregion


        #region Pie


        /// <summary>
        /// Create a new pie geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreatePie(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, float sweepAngle)
        {
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
                return TransformerGeometry.CreateEllipse(resourceCreator, bounds);
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomographyFromIdentity(bounds);

                return TransformerGeometry.CreatePieCore(resourceCreator, oneMatrix, sweepAngle);
            }
        }

        /// <summary>
        /// Create a new pie geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreatePie(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, Matrix3x2 matrix, float sweepAngle)
        {
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
                return TransformerGeometry.CreateEllipse(resourceCreator, bounds, matrix);
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomographyFromIdentity(bounds);
                Matrix3x2 oneMatrix2 = oneMatrix * matrix;

                return TransformerGeometry.CreatePieCore(resourceCreator, oneMatrix2, sweepAngle);
            }
        }

        private static CanvasGeometry CreatePieCore(ICanvasResourceCreator resourceCreator, Matrix3x2 oneMatrix, float sweepAngle)
        {
            // start tooth
            Vector2 startTooth = new Vector2(1, 0);
            // end tooth
            Vector2 endTooth = TransformerGeometry.GetRotationVector(sweepAngle);

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            CanvasArcSize canvasArcSize = (sweepAngle < System.Math.PI) ? CanvasArcSize.Large : CanvasArcSize.Small;
            {
                pathBuilder.BeginFigure(Vector2.Zero);

                // end notch point
                pathBuilder.AddLine(endTooth);

                // end tooth point
                pathBuilder.AddArc(startTooth, 1, 1, sweepAngle, CanvasSweepDirection.Clockwise, canvasArcSize);

                pathBuilder.EndFigure(CanvasFigureLoop.Closed);
            }
            return CanvasGeometry.CreatePath(pathBuilder).Transform(oneMatrix);
        }


        #endregion


        #region Cookie


        /// <summary>
        /// Create a new cookie geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateCookie(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, float innerRadius, float sweepAngle)
        {
            bool zeroInnerRadius = innerRadius == 0;
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
            {
                CanvasGeometry ellipse = TransformerGeometry.CreateEllipse(resourceCreator, bounds);

                if (zeroInnerRadius)
                    return ellipse;
                else
                {
                    Vector2 center = bounds.Center;

                    return TransformerGeometry.CreateDonutCore(ellipse, innerRadius, center);
                }
            }
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomographyFromIdentity(bounds);

                if (zeroInnerRadius)
                    return TransformerGeometry.CreatePieCore(resourceCreator, oneMatrix, sweepAngle);
                else
                    return TransformerGeometry.CreateCookieCore(resourceCreator, oneMatrix, innerRadius, sweepAngle);
            }
        }

        /// <summary>
        /// Create a new cookie geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="sweepAngle"> The sweep-angle. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateCookie(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, Matrix3x2 matrix, float innerRadius, float sweepAngle)
        {
            bool zeroInnerRadius = innerRadius == 0;
            bool zeroSweepAngle = sweepAngle == 0;

            if (zeroSweepAngle)
            {
                CanvasGeometry ellipse = TransformerGeometry.CreateEllipse(resourceCreator, bounds, matrix);

                if (zeroInnerRadius)
                    return ellipse;
                else
                {
                    Vector2 center = Vector2.Transform(bounds.Center, matrix);

                    return TransformerGeometry.CreateDonutCore(ellipse, innerRadius, center);
                }
            }
            else
            {
                Matrix3x2 oneMatrix = Transformer.FindHomographyFromIdentity(bounds);
                Matrix3x2 oneMatrix2 = oneMatrix * matrix;

                if (zeroInnerRadius)
                    return TransformerGeometry.CreatePieCore(resourceCreator, oneMatrix2, sweepAngle);
                else
                    return TransformerGeometry.CreateCookieCore(resourceCreator, oneMatrix2, innerRadius, sweepAngle);
            }
        }

        private static CanvasGeometry CreateCookieCore(ICanvasResourceCreator resourceCreator, Matrix3x2 oneMatrix, float innerRadius, float sweepAngle)
        {
            // start tooth
            Vector2 startTooth = new Vector2(1, 0);
            // end tooth
            Vector2 endTooth = TransformerGeometry.GetRotationVector(sweepAngle);

            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            CanvasArcSize canvasArcSize = (sweepAngle < System.Math.PI) ? CanvasArcSize.Large : CanvasArcSize.Small;
            {
                // DonutAndCookie
                // start notch
                Vector2 startNotch = startTooth * innerRadius;
                // end notch
                Vector2 endNotch = endTooth * innerRadius;

                // start tooth point
                pathBuilder.BeginFigure(startNotch);
                // start notch point
                pathBuilder.AddArc(endNotch, innerRadius, innerRadius, sweepAngle, CanvasSweepDirection.CounterClockwise, canvasArcSize);
            }

            // end notch point
            pathBuilder.AddLine(endTooth);

            // end tooth point
            pathBuilder.AddArc(startTooth, 1, 1, sweepAngle, CanvasSweepDirection.Clockwise, canvasArcSize);

            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            return CanvasGeometry.CreatePath(pathBuilder).Transform(oneMatrix);
        }


        #endregion

    }
}