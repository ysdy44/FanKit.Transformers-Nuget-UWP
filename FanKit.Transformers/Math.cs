using System.Numerics;

namespace FanKit
{
    /// <summary>
    /// Provide constant and static methods for transformer.
    /// </summary>
    public static class Math
    {
        /// <summary> 15 degress in angle system. </summary>
        public const float RadiansStep = System.MathF.PI / 12f;
        /// <summary> 7.5 degress in angle system. </summary>
        public const float RadiansStepHalf = System.MathF.PI / 24f;
        /// <summary> To find a multiple of the nearest 15. </summary>
        public static float RadiansStepFrequency(float radian) => ((int)((radian + Math.RadiansStepHalf) / Math.RadiansStep)) * Math.RadiansStep; // Get step radians


        /// <summary> The number pi*2. </summary>
        public const float PiTwice = System.MathF.PI * 2f;
        /// <summary> The number pi. </summary>
        public const float Pi = System.MathF.PI;
        /// <summary> The number pi/2. </summary>
        public const float PiOver2 = System.MathF.PI / 2f;
        /// <summary> The number pi/4. </summary>
        public const float PiOver4 = System.MathF.PI / 4f;


        /// <summary> Radius of node'. Default 12. </summary>
        public const float NodeRadius = 12.0f;
        private const float NodeRadiusSquare = Math.NodeRadius * Math.NodeRadius;

        /// <summary> Minimum distance between two nodes. Default 20. </summary>
        public const float NodeDistance = 20.0f;
        private const float NodeDistanceSquare = Math.NodeDistance * Math.NodeDistance;
        private const float NodeDistanceDouble = Math.NodeDistance + Math.NodeDistance;

        internal static float LengthSquared(float x, float y) => x * x + y * y;

        internal static float DistanceSquared(float x0, float y0, float x1, float y1) => Math.LengthSquared(x1 - x0, y1 - y0);

        /// <summary>
        /// Whether the distance exceeds [NodeRadius]. Default: 144.
        /// </summary>
        /// <param name="point0"> The first point. </param>
        /// <param name="point1"> The second point. </param>
        /// <returns> Return **true** if the distance exceeds [NodeRadius], otherwise **false**. </returns>
        public static bool InNodeRadius(Vector2 point0, Vector2 point1) => Math.DistanceSquared(point0.X, point0.Y, point1.X, point1.Y) < Math.NodeRadiusSquare;

        /// <summary>
        /// Whether the distance exceeds [NodeRadius]. Default: 144.
        /// </summary>
        /// <param name="x0"> The X-axis value of the first point. </param>
        /// <param name="y0"> The Y-axis position of the first point. </param>
        /// <param name="x1"> The X-axis value of the second point. </param>
        /// <param name="y1"> The Y-axis position of the second point. </param>
        /// <returns> Return **true** if the distance exceeds [NodeRadius], otherwise **false**. </returns>
        public static bool InNodeRadius(float x0, float y0, float x1, float y1) => Math.DistanceSquared(x0, y0, x1, y1) < Math.NodeRadiusSquare;

        /// <summary>
        /// Whether the distance's LengthSquared exceeds [NodeDistance]. Default: 400.
        /// </summary>
        /// <param name="point0"> The first point. </param>
        /// <param name="point1"> The second point. </param>
        /// <returns> Return **true** if the distance's LengthSquared exceeds [NodeDistance], otherwise **false**. </returns>
        public static bool OutNodeDistance(Vector2 point0, Vector2 point1) => Math.DistanceSquared(point0.X, point0.Y, point1.X, point1.Y) > Math.NodeDistanceSquare;

        /// <summary>
        /// Whether the distance's LengthSquared exceeds [NodeDistance]. Default: 400.
        /// </summary>
        /// <param name="x0"> The X-axis value of the first point. </param>
        /// <param name="y0"> The Y-axis position of the first point. </param>
        /// <param name="x1"> The X-axis value of the second point. </param>
        /// <param name="y1"> The Y-axis position of the second point. </param>
        /// <returns> Return **true** if the distance's LengthSquared exceeds [NodeDistance], otherwise **false**. </returns>
        public static bool OutNodeDistance(float x0, float y0, float x1, float y1) => Math.DistanceSquared(x0, y0, x1, y1) > Math.NodeDistanceSquare;

        /// <summary>
        /// Get outside point in a line on a transformer.
        /// </summary>
        /// <param name="nearPoint"> The nearest point to outside point in a line on a transformer. </param>
        /// <param name="farPoint"> The farthest point to outside point in a line on a transformer. </param>
        /// <returns> The product point. </returns>
        public static Vector2 GetOutsidePointInTransformer(Vector2 nearPoint, Vector2 farPoint) => nearPoint - Vector2.Normalize(farPoint - nearPoint) * Math.NodeDistanceDouble;


        /// <summary>
        /// Returns whether the quadrangle contains the specified point.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="leftTop"> The position of the top-left corner of the quadrangle. </param>
        /// <param name="rightTop"> The position of the top-right corner of the quadrangle. </param>
        /// <param name="rightBottom"> The position of the bottom-right corner of the quadrangle. </param>
        /// <param name="leftBottom"> The position of the bottom-left corner of the quadrangle. </param>
        /// <returns> Return **true** if the quadrangle contains the specified point, otherwise **false**. </returns>
        public static bool InQuadrangle(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            float a = (leftTop.X - leftBottom.X) * (point.Y - leftBottom.Y) - (leftTop.Y - leftBottom.Y) * (point.X - leftBottom.X);
            float b = (rightTop.X - leftTop.X) * (point.Y - leftTop.Y) - (rightTop.Y - leftTop.Y) * (point.X - leftTop.X);
            float c = (rightBottom.X - rightTop.X) * (point.Y - rightTop.Y) - (rightBottom.Y - rightTop.Y) * (point.X - rightTop.X);
            float d = (leftBottom.X - rightBottom.X) * (point.Y - rightBottom.Y) - (leftBottom.Y - rightBottom.Y) * (point.X - rightBottom.X);
            return (a > 0 && b > 0 && c > 0 && d > 0) || (a < 0 && b < 0 && c < 0 && d < 0);
        }

        /// <summary>
        /// Move the position of the point of the quadrilateral so that the quadrilateral is a convex quadrilateral. 
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="diagonal"> The point on the diagonal. </param>
        /// <param name="left"> The point to left of the diagonal. </param>
        /// <param name="right"> The point to right of the diagonal. </param>
        /// <param name="margin"> The margin of the triangle formed by other three points. </param>
        /// <returns> The product point. </returns>
        public static Vector2 MovePointOfConvexQuadrilateral(Vector2 point, Vector2 diagonal, Vector2 left, Vector2 right, int margin = 8)
        {
            if (margin > 0)
            {
                Vector2 m = margin * Vector2.Normalize(diagonal + diagonal - left - right);
                diagonal += m;
                left -= m;
                right -= m;
            }

            int i = 0;
            do
            {
                Vector2 lineA;
                Vector2 lineB;
                switch (i)
                {
                    case 1:
                        lineA = diagonal;
                        lineB = right;
                        break;
                    case 2:
                        lineA = left;
                        lineB = diagonal;
                        break;
                    default:
                        lineA = left;
                        lineB = right;
                        break;
                }

                float bx = lineA.X - lineB.X;
                float by = lineA.Y - lineB.Y;
                float px = lineA.X - point.X;
                float py = lineA.Y - point.Y;

                if (bx * py - by * px < 0f)
                {
                    float s = bx * bx + by * by;
                    float p = py * by + px * bx;

                    point = new Vector2
                    {
                        X = lineA.X - bx * p / s,
                        Y = lineA.Y - by * p / s
                    };
                }
                i++;
            }
            while (i < 4);

            return point;
        }


        /// <summary>
        /// Get the [Foot Point] of point and LIne.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="point0"> The first point on the line. </param>
        /// <param name="point1"> The second point on the line. </param>
        /// <returns> The product vector. </returns>
        public static Vector2 FootPoint(Vector2 point, Vector2 point0, Vector2 point1)
        {
            float x = point0.X - point1.X;
            float y = point0.Y - point1.Y;

            float s = x * x + y * y;
            float p = (point0.Y - point.Y) * y + (point0.X - point.X) * x;

            return new Vector2
            {
                X = point0.X - x * p / s,
                Y = point0.Y - y * p / s
            };
        }

        /// <summary>
        /// Get the intersection-point of Line1 and LIne2.
        /// </summary>
        /// <param name="line1A"> The first line first point. </param>
        /// <param name="line1B"> The first line second point. </param>
        /// <param name="line2A"> The second line first point. </param>
        /// <param name="line2B"> The second line second point. </param>
        /// <returns> The product vector. </returns>
        public static Vector2 IntersectionPoint(Vector2 line1A, Vector2 line1B, Vector2 line2A, Vector2 line2B)
        {
            float a = 0, b = 0;
            int state = 0;
            if (System.Math.Abs(line1A.X - line1B.X) > float.Epsilon)
            {
                a = (line1B.Y - line1A.Y) / (line1B.X - line1A.X);
                state |= 1;
            }
            if (System.Math.Abs(line2A.X - line2B.X) > float.Epsilon)
            {
                b = (line2B.Y - line2A.Y) / (line2B.X - line2A.X);
                state |= 2;
            }
            switch (state)
            {
                case 0:
                    if (System.Math.Abs(line1A.X - line2A.X) < float.Epsilon) return Vector2.Zero;
                    else return Vector2.Zero;
                case 1:
                    {
                        float x = line2A.X;
                        float y = (line1A.X - x) * (-a) + line1A.Y;
                        return new Vector2(x, y);
                    }
                case 2:
                    {
                        float x = line1A.X;
                        float y = (line2A.X - x) * (-b) + line2A.Y;
                        return new Vector2(x, y);
                    }
                case 3:
                    {
                        if (System.Math.Abs(a - b) < float.Epsilon) return Vector2.Zero;
                        float x = (a * line1A.X - b * line2A.X - line1A.Y + line2A.Y) / (a - b);
                        float y = a * x - a * line1A.X + line1A.Y;
                        return new Vector2(x, y);
                    }
            }
            return Vector2.Zero;
        }


        /// <summary>
        /// Get vector of the radians in the coordinate system. 
        /// </summary>
        /// <param name="radians"> The radians. </param>
        /// <param name="center"> The center of coordinate system. </param>
        /// <param name="length"> The length of vector. </param>
        /// <returns> The product vector. </returns>
        public static Vector2 RadiansToVector(float radians, Vector2 center, float length = 40.0f) => new Vector2(System.MathF.Cos(radians) * length + center.X, System.MathF.Sin(radians) * length + center.Y);

        /// <summary>
        /// Get radians of the vector in the coordinate system. 
        /// </summary>
        /// <param name="vector"> The vector. </param>
        /// <returns> The product radians. </returns>
        public static float VectorToRadians(Vector2 vector)
        {
            float tan = System.MathF.Atan(System.Math.Abs(vector.Y / vector.X));

            // First Quantity
            if (vector.X > 0 && vector.Y > 0) return tan;
            // Second Quadrant
            else if (vector.X > 0 && vector.Y < 0) return -tan;
            // Third Quadrant  
            else if (vector.X < 0 && vector.Y > 0) return System.MathF.PI - tan;
            // Fourth Quadrant  
            else return tan - System.MathF.PI;
        }


        /// <summary>
        /// Transforms a vector by the specified rotation value.
        /// </summary>
        /// <param name="value"> The vector to rotate. </param>
        /// <param name="cos"> The cos value of rotation to apply. </param>
        /// <param name="sin"> The sin value of rotation to apply. </param>
        /// <returns> The transformed vector. </returns>
        public static Vector2 Rotate(Vector2 value, float cos, float sin)
        {
            float x = cos * value.X - sin * value.Y;
            float y = sin * value.X + cos * value.Y;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Transforms a vector by the specified rotation value.
        /// </summary>
        /// <param name="value"> The vector to rotate. </param>
        /// <param name="rotation"> The rotation to apply. </param>
        /// <returns> The transformed vector. </returns>
        public static Vector2 Rotate(Vector2 value, Vector2 rotation)
        {
            float x = rotation.X * value.X - rotation.Y * value.Y;
            float y = rotation.Y * value.X + rotation.X * value.Y;
            return new Vector2(x, y);
        }

        /// <summary>
        /// Creates a rotation vector using the given rotation in radians.
        /// </summary>
        /// <param name="radians"> The amount of rotation, in radians. </param>
        /// <returns> The rotation vector. </returns>
        public static Vector2 CreateRotation(float radians)
        {
            radians = System.MathF.IEEERemainder(radians, System.MathF.PI * 2);

            const float epsilon = 0.001f * System.MathF.PI / 180f;     // 0.1% of a degree

            if (radians > -epsilon && radians < epsilon)
            {
                // Exact case for zero rotation.
                return new Vector2(1, 0);
            }
            else if (radians > System.Math.PI / 2 - epsilon && radians < System.Math.PI / 2 + epsilon)
            {
                // Exact case for 90 degree rotation.
                return new Vector2(0, 1);
            }
            else if (radians < -System.Math.PI + epsilon || radians > System.Math.PI - epsilon)
            {
                // Exact case for 180 degree rotation.
                return new Vector2(-1, 0);
            }
            else if (radians > -System.Math.PI / 2 - epsilon && radians < -System.Math.PI / 2 + epsilon)
            {
                // Exact case for 270 degree rotation.
                return new Vector2(0, -1);
            }
            else
            {
                // Arbitrary rotation.
                float c = System.MathF.Cos(radians);
                float s = System.MathF.Sin(radians);
                return new Vector2(c, s);
            }
        }
    }
}