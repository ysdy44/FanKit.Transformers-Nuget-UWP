using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class TransformerGeometry
    {

        #region RoundRect


        /// <summary>
        /// Create a new round rect geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="corner"> The corner. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRoundRect(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, float corner)
        {
            return TransformerGeometry.CreateRoundRectCore
            (
                resourceCreator,

                bounds.LeftTop,
                bounds.RightTop,
                bounds.RightBottom,
                bounds.LeftBottom,

                bounds.CenterLeft,
                bounds.CenterTop,
                bounds.CenterRight,
                bounds.CenterBottom,

                corner
            );
        }

        /// <summary>
        /// Create a new round rect geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="corner"> The corner. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRoundRect(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, Matrix3x2 matrix, float corner)
        {
            return TransformerGeometry.CreateRoundRectCore
            (
                resourceCreator,

                Vector2.Transform(bounds.LeftTop, matrix),
                Vector2.Transform(bounds.RightTop, matrix),
                Vector2.Transform(bounds.RightBottom, matrix),
                Vector2.Transform(bounds.LeftBottom, matrix),

                Vector2.Transform(bounds.CenterLeft, matrix),
                Vector2.Transform(bounds.CenterTop, matrix),
                Vector2.Transform(bounds.CenterRight, matrix),
                Vector2.Transform(bounds.CenterBottom, matrix),

                corner
            );
        }

        private static CanvasGeometry CreateRoundRectCore(ICanvasResourceCreator resourceCreator,
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
            // Horizontal
            Vector2 horizontal = (centerRight - centerLeft);
            float horizontalLength = horizontal.Length();
            Vector2 horizontalUnit = horizontal / horizontalLength;
            // Vertical
            Vector2 vertical = (centerBottom - centerTop);
            float verticalLength = vertical.Length();
            Vector2 verticalUnit = vertical / verticalLength;


            // Control
            float minLength = System.Math.Min(horizontalLength, verticalLength);
            float minLength2 = corner * minLength;

            Vector2 horizontal2 = minLength2 * horizontalUnit;
            Vector2 horizontal448 = horizontal2 * 0.448f; // vector / (1 - 4 * 0.552f)
            Vector2 vertical2 = minLength2 * verticalUnit;
            Vector2 vertical448 = vertical2 * 0.448f; // vector /  (1 - 4 * 0.552f)


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


            // Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(leftTop_Left);

            pathBuilder.AddCubicBezier(leftTop_Left2, leftTop_Top1, leftTop_Top);
            pathBuilder.AddLine(rightTop_Top);

            pathBuilder.AddCubicBezier(rightTop_Top2, rightTop_Right1, rightTop_Right);
            pathBuilder.AddLine(rightBottom_Right);

            pathBuilder.AddCubicBezier(rightBottom_Right2, rightBottom_Bottom1, rightBottom_Bottom);
            pathBuilder.AddLine(leftBottom_Bottom);

            pathBuilder.AddCubicBezier(leftBottom_Bottom2, leftBottom_Left1, leftBottom_Left);
            pathBuilder.AddLine(leftBottom_Left);

            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            // Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }


        #endregion


        #region Triangle


        /// <summary>
        /// Create a new triangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="center"> The value of center. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateTriangle(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, float center) => TransformerGeometry.CreateTriangleCore(resourceCreator, bounds.LeftTop, bounds.RightTop, bounds.RightBottom, bounds.LeftBottom, center);

        /// <summary>
        /// Create a new triangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="center"> The value of center. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateTriangle(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, Matrix3x2 matrix, float center) => TransformerGeometry.CreateTriangleCore(resourceCreator, Vector2.Transform(bounds.LeftTop, matrix), Vector2.Transform(bounds.RightTop, matrix), Vector2.Transform(bounds.RightBottom, matrix), Vector2.Transform(bounds.LeftBottom, matrix), center);

        /// <summary>
        /// Create a new triangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <param name="center"> The value of center. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateTriangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center) => TransformerGeometry.CreateTriangleCore(resourceCreator, leftTop, rightTop, rightBottom, leftBottom, center);

        /// <summary>
        /// Create a new triangle geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="center"> The value of center. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateTriangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Matrix3x2 matrix, float center) => TransformerGeometry.CreateTriangleCore(resourceCreator, Vector2.Transform(leftTop, matrix), Vector2.Transform(rightTop, matrix), Vector2.Transform(rightBottom, matrix), Vector2.Transform(leftBottom, matrix), center);

        private static CanvasGeometry CreateTriangleCore(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, float center)
        {
            float minusValue = 1.0f - center;
            Vector2 center2 = leftTop * minusValue + rightTop * center;

            // Points
            Vector2[] points = new Vector2[]
            {
                center2,
                rightBottom,
                leftBottom,
            };

            // Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        #endregion


        #region Diamond


        /// <summary>
        /// Create a new diamond geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="mid"> The mid value. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateDiamond(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, float mid)
        {
            return TransformerGeometry.CreateDiamondCore(resourceCreator,
                 bounds.LeftTop,
                 bounds.RightTop,
                 bounds.RightBottom,
                 bounds.LeftBottom,

                 bounds.CenterLeft,
                 bounds.CenterRight,

                 mid
            );
        }

        /// <summary>
        /// Create a new diamond geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="bounds"> The bounds. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="mid"> The mid value. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateDiamond(ICanvasResourceCreator resourceCreator, ITransformerLTRB bounds, Matrix3x2 matrix, float mid)
        {
            return TransformerGeometry.CreateDiamondCore(resourceCreator,
               Vector2.Transform(bounds.LeftTop, matrix),
               Vector2.Transform(bounds.RightTop, matrix),
               Vector2.Transform(bounds.RightBottom, matrix),
               Vector2.Transform(bounds.LeftBottom, matrix),

               Vector2.Transform(bounds.CenterLeft, matrix),
               Vector2.Transform(bounds.CenterRight, matrix),

               mid
            );
        }

        private static CanvasGeometry CreateDiamondCore(ICanvasResourceCreator resourceCreator,
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

            // Points
            Vector2[] points = new Vector2[]
            {
                centerLeft,
                top,
                centerRight,
                bottom,
            };

            // Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        #endregion

    }
}