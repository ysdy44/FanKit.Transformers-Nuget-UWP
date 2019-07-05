using System;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for transformer.
    /// </summary>
    public struct TransformerMath
    {
        /// <summary> 15 degress in angle system. </summary>
        public const float RadiansStep = 0.2617993833333333f;
        /// <summary> 7.5 degress in angle system. </summary>
        public const float RadiansStepHalf = 0.1308996916666667f;
        /// <summary> To find a multiple of the nearest 15. </summary>
        public static float RadiansStepFrequency(float radian) => ((int)((radian + TransformerMath.RadiansStepHalf) / TransformerMath.RadiansStep)) * TransformerMath.RadiansStep;//Get step radians


        /// <summary> The number pi. </summary>
        public const float Pi = 3.1415926535897932384626433832795028841971693993751f;
        /// <summary> The number pi/2. </summary>
        public const float PiOver2 = 1.5707963267948966192313216916397514420985846996876f;
        /// <summary> The number pi/4. </summary>
        public const float PiOver4 = 0.78539816339744830961566084581987572104929234984378f;


        /// <summary>
        /// Get the [Foot Point] of point and LIne.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="lineA"> The first point. </param>
        /// <param name="lineB"> The second point. </param>
        /// <returns> The product vector. </returns>
        public static Vector2 FootPoint(Vector2 point, Vector2 lineA, Vector2 lineB)
        {
            Vector2 lineVector = lineA - lineB;
            Vector2 pointLineA = lineA - point;

            float t = -(pointLineA.Y * lineVector.Y + pointLineA.X * lineVector.X)
                / lineVector.LengthSquared();

            return lineVector * t + lineA;
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
            if (Math.Abs(line1A.X - line1B.X) > float.Epsilon)
            {
                a = (line1B.Y - line1A.Y) / (line1B.X - line1A.X);
                state |= 1;
            }
            if (Math.Abs(line2A.X - line2B.X) > float.Epsilon)
            {
                b = (line2B.Y - line2A.Y) / (line2B.X - line2A.X);
                state |= 2;
            }
            switch (state)
            {
                case 0:
                    if (Math.Abs(line1A.X - line2A.X) < float.Epsilon) return Vector2.Zero;
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
                        if (Math.Abs(a - b) < float.Epsilon) return Vector2.Zero;
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
        /// <param name="center"> The center of coordinate system.  </param>
        /// <param name="length">The length of vector. </param>
        /// <returns> The product vector. </returns>
        public static Vector2 RadiansToVector(float radians, Vector2 center, float length = 40.0f) => new Vector2((float)Math.Cos(radians) * length + center.X, (float)Math.Sin(radians) * length + center.Y);

        /// <summary>
        /// Get radians of the vector in the coordinate system. 
        /// </summary>
        /// <param name="vector"> The vector. </param>
        /// <returns> The product radians. </returns>
        public static float VectorToRadians(Vector2 vector)
        {
            float tan = (float)Math.Atan(Math.Abs(vector.Y / vector.X));

            //First Quantity
            if (vector.X > 0 && vector.Y > 0) return tan;
            //Second Quadrant
            else if (vector.X > 0 && vector.Y < 0) return -tan;
            //Third Quadrant  
            else if (vector.X < 0 && vector.Y > 0) return (float)Math.PI - tan;
            //Fourth Quadrant  
            else return tan - (float)Math.PI;
        }
    }
}