using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer : ITransformerLTRB, ITransformerGeometry
    {

        /// <summary>
        /// Create a transformer of aspect ratio fixed.
        /// </summary>
        /// <param name="startingPoint"> The starting-point </param>
        /// <param name="point"> The point. </param>
        /// <param name="sizeWidth"> The source width. </param>
        /// <param name="sizeHeight"> The source height. </param>
        /// <returns> The provided transformer. </returns>
        public static Transformer CreateWithAspectRatio(Vector2 startingPoint, Vector2 point, float sizeWidth, float sizeHeight)
        {
            float lengthSquared = Vector2.DistanceSquared(startingPoint, point);
            const float root2 = 1.4142135623730950488016887242097f;

            //Height not less than 10
            if (sizeWidth > sizeHeight)
            {
                float heightSquared = lengthSquared / (1 + (sizeWidth * sizeWidth) / (sizeHeight * sizeHeight));
                float height = (float)System.Math.Sqrt(heightSquared) / root2;

                if (height < 10) height = 10;
                float width = height * sizeWidth / sizeHeight;

                return Transformer._getRectangleInQuadrant(startingPoint, point, width, height);
            }
            //Width not less than 10
            else if (sizeWidth < sizeHeight)
            {
                float widthSquared = lengthSquared / (1 + (sizeHeight * sizeHeight) / (sizeWidth * sizeWidth));
                float width = (float)System.Math.Sqrt(widthSquared) / root2;

                if (width < 10) width = 10;
                float height = width * sizeHeight / sizeWidth;

                return Transformer._getRectangleInQuadrant(startingPoint, point, width, height);
            }
            //Width equals height
            else
            {
                float spare = (float)System.Math.Sqrt(lengthSquared) / root2;

                return Transformer._getRectangleInQuadrant(startingPoint, point, spare, spare);
            }
        }

        // Get a rectangle corresponding to the 1, 2, 3 or 4 quadrant.
        private static Transformer _getRectangleInQuadrant(Vector2 startingPoint, Vector2 point, float width, float height)
        {
            bool xAxis = (point.X >= startingPoint.X);
            bool yAxis = (point.Y >= startingPoint.Y);

            //Fourth Quadrant  
            if (xAxis && yAxis)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X + width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X + width, startingPoint.Y + height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y + height),
                };
            }

            //Third Quadrant  
            else if (xAxis == false && yAxis)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X - width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X - width, startingPoint.Y + height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y + height),
                };
            }

            //First Quantity
            else if (xAxis && yAxis == false)
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X + width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X + width, startingPoint.Y - height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y - height),
                };
            }

            //Second Quadrant
            else
            {
                return new Transformer
                {
                    LeftTop = new Vector2(startingPoint.X, startingPoint.Y),
                    RightTop = new Vector2(startingPoint.X - width, startingPoint.Y),
                    RightBottom = new Vector2(startingPoint.X - width, startingPoint.Y - height),
                    LeftBottom = new Vector2(startingPoint.X, startingPoint.Y - height),
                };
            }
        }

    }
}