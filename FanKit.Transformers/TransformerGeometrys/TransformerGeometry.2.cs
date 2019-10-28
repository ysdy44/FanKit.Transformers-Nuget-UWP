using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {

        #region Pentagon


        /// <summary>
        ///  Create a new pentagon geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="points"> The points count. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreatePentagon(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, int points)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            CanvasGeometry geometry = TransformerGeometry._createPentagon(resourceCreator, points);

            return geometry.Transform(oneMatrix);
        }

        /// <summary>
        ///  Create a new pentagon geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="points"> The points count. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreatePentagon(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, Matrix3x2 matrix, int points)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            CanvasGeometry geometry = TransformerGeometry._createPentagon(resourceCreator, points);

            return geometry.Transform(oneMatrix2);
        }

        private static CanvasGeometry _createPentagon(ICanvasResourceCreator resourceCreator, int points)
        {
            float rotation = TransformerGeometry.StartingRotation;
            float angle = FanKit.Math.Pi * 2.0f / points;

            Vector2[] array = new Vector2[points];
            for (int i = 0; i < points; i++)
            {
                int index = i;

                //Outer
                Vector2 outer = TransformerGeometry.GetRotationVector(rotation);
                array[index] = outer;
                rotation += angle;
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, array);
        }


        #endregion


        #region Star


        /// <summary>
        ///  Create a new star geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="points"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateStar(ICanvasResourceCreator resourceCreator, ITransformerLTRB transformer, int points, float innerRadius)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            CanvasGeometry geometry = TransformerGeometry._createStar(resourceCreator, points, innerRadius);

            return geometry.Transform(oneMatrix);
        }

        /// <summary>
        ///  Create a new star geometry.
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

            CanvasGeometry geometry = TransformerGeometry._createStar(resourceCreator, points, innerRadius);

            return geometry.Transform(oneMatrix2);
        }

        private static CanvasGeometry _createStar(ICanvasResourceCreator resourceCreator, int points, float innerRadius)
        {
            float rotation = TransformerGeometry.StartingRotation;
            float angle = FanKit.Math.Pi / points;

            Vector2[] array = new Vector2[points * 2];
            for (int i = 0; i < points; i++)
            {
                int index = i * 2;

                //Outer
                Vector2 outer = TransformerGeometry.GetRotationVector(rotation);
                array[index] = outer;
                rotation += angle;

                //Inner
                Vector2 inner = TransformerGeometry.GetRotationVector(rotation);
                array[index + 1] = inner * innerRadius;
                rotation += angle;
            }

            return CanvasGeometry.CreatePolygon(resourceCreator, array);
        }


        #endregion

    }
}