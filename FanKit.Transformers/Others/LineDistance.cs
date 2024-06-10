using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// Distance of points on these points in a line: 
    /// ------D[Diagonal Point]、C[Center Point]、P[Point) and F[FootPoint] .
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    internal struct LineDistance
    {
        /// <summary> Distance between [Foot Point] and [Center Point] . </summary>
        public float FC;
        /// <summary> Distance between [Foot Point] and [Point] . </summary>
        public float FP;
        /// <summary> Distance between [Foot Point] and [Diagonal Point] . </summary>
        public float FD;
        /// <summary> Distance between [Point] and [Center Point] . </summary>
        public float PC;


        // These points in a line: 
        //      D[Diagonal Point]、C[Center Point]、P[Point] and F[FootPoint] .
        //
        //                                         2m                                           1m                          1m
        //————•————————————————•————————•————————•————
        //              D                                                          C                                                           P
        public LineDistance(Vector2 footPoint, Vector2 point, Vector2 center)
        {
            Vector2 diagonal = center + center - point;

            this.FC = Vector2.Distance(footPoint, center);
            this.FP = Vector2.Distance(footPoint, point);
            this.FD = Vector2.Distance(footPoint, diagonal);
            this.PC = Vector2.Distance(point, center);
        }

        /// <summary> Scale of [Foot Point] betwwen [Center Point] / scale of [Point] betwwen [Center Point] (may be negative) </summary>
        /// <param name="distance"> The distance </param>
        /// <returns> Scale </returns>
        public static float Scale(LineDistance distance)
        {
            float scale = distance.FC / distance.PC;
            bool isReverse = (distance.FP < distance.FD);
            return isReverse ? scale : -scale;
        }

        public static implicit operator float(LineDistance distance) => LineDistance.Scale(distance);
  
    }
}