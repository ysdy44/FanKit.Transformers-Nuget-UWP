using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class TransformerGeometry
    {

        #region Pentagon


        /// <summary>
        /// Create a new pentagon geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="points"> The points count. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreatePentagon(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, int points)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return TransformerGeometry.CreatePentagonCore(resourceCreator, points, oneMatrix);
        }

        /// <summary>
        /// Create a new pentagon geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="points"> The points count. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreatePentagon(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix, int points)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return TransformerGeometry.CreatePentagonCore(resourceCreator, points, oneMatrix2);
        }

        private static CanvasGeometry CreatePentagonCore(ICanvasResourceCreator resourceCreator, int points, Matrix3x2 oneMatrix)
        {
            float rotation = TransformerGeometry.StartingRotation;
            float angle = FanKit.Math.Pi * 2.0f / points;

            Vector2[] array = new Vector2[points];
            for (int i = 0; i < points; i++)
            {
                int index = i;

                // Outer
                Vector2 outer = TransformerGeometry.GetRotationVector(rotation);
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                array[index] = outerTransform;
                rotation += angle;
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, array);
        }


        #endregion


        #region Star


        /// <summary>
        /// Create a new star geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="points"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateStar(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, int points, float innerRadius)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return TransformerGeometry.CreateStarCore(resourceCreator, points, innerRadius, oneMatrix);
        }

        /// <summary>
        /// Create a new star geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="points"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateStar(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix, int points, float innerRadius)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return TransformerGeometry.CreateStarCore(resourceCreator, points, innerRadius, oneMatrix2);
        }

        private static CanvasGeometry CreateStarCore(ICanvasResourceCreator resourceCreator, int points, float innerRadius, Matrix3x2 oneMatrix)
        {
            float rotation = TransformerGeometry.StartingRotation;
            float angle = FanKit.Math.Pi / points;

            Vector2[] array = new Vector2[points * 2];
            for (int i = 0; i < points; i++)
            {
                int index = i * 2;

                // Outer
                Vector2 outer = TransformerGeometry.GetRotationVector(rotation);
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                array[index] = outerTransform;
                rotation += angle;

                // Inner
                Vector2 inner = TransformerGeometry.GetRotationVector(rotation);
                Vector2 inner2 = inner * innerRadius;
                Vector2 inner2Transform = Vector2.Transform(inner2, oneMatrix);
                array[index + 1] = inner2Transform;
                rotation += angle;
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, array);
        }


        #endregion


        #region Cog


        /// <summary>
        /// Create a new cog geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="count"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="tooth"> The tooth. </param>
        /// <param name="notch"> The notch. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateCog(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, int count, float innerRadius, float tooth, float notch)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return TransformerGeometry.CreateCogCore(resourceCreator, count, innerRadius, tooth, notch, oneMatrix);
        }

        /// <summary>
        /// Create a new cog geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="count"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="tooth"> The tooth. </param>
        /// <param name="notch"> The notch. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateCog(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix, int count, float innerRadius, float tooth, float notch)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return TransformerGeometry.CreateCogCore(resourceCreator, count, innerRadius, tooth, notch, oneMatrix2);
        }

        private static CanvasGeometry CreateCogCore(ICanvasResourceCreator resourceCreator, int count, float innerRadius, float tooth, float notch, Matrix3x2 oneMatrix)
        {
            float angle = FanKit.Math.Pi * 2f / count; // angle
            float angleTooth = angle * tooth; // angle tooth
            float angleNotch = angle * notch; // angle notch
            float angleDiffHalf = (angleNotch - angleTooth) / 2; // Half the angle difference between the tooth and the notch

            float rotation = 0; // Start angle is zero
            int countQuadra = count * 4;
            Vector2[] points = new Vector2[countQuadra];

            for (int i = 0; i < countQuadra; i++)
            {
                Vector2 vector = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
                int remainder = i % 4; // remainder

                if (remainder == 0) // 凸 left-bottom point
                {
                    // Inner
                    Vector2 inner = vector * innerRadius;
                    Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                    points[i] = innerTransform;
                    rotation += angleDiffHalf;
                }
                else if (remainder == 1) // 凸 left-top point
                {
                    // Outer
                    Vector2 outer = vector;
                    Vector2 outerTransform = Vector2.Transform(vector, oneMatrix);
                    points[i] = outerTransform;
                    rotation += angleTooth;
                }
                else if (remainder == 2) // 凸 right-top point
                {
                    // Outer
                    Vector2 outer = vector;
                    Vector2 outerTransform = Vector2.Transform(vector, oneMatrix);
                    points[i] = outerTransform;
                    rotation += angleDiffHalf;
                }
                else if (remainder == 3) // 凸 right-bottom point
                {
                    // Inner
                    Vector2 inner = vector * innerRadius;
                    Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                    points[i] = innerTransform;
                    rotation += angle - angleNotch;
                }
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }


        #endregion

    }
}