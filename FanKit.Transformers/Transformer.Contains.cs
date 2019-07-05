using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {
        /// <summary> Radius of node'. Defult 12. </summary>
        private const float NodeRadius = 12.0f;
        /// <summary> Whether the distance exceeds [NodeRadius]. Defult: 144. </summary>
        public static bool InNodeRadius(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() < 144.0f;// Transformer.NodeRadius * Transformer.NodeRadius;


        /// <summary> Minimum distance between two nodes. Defult 20. </summary>
        private const float NodeDistance = 20.0f;
        /// <summary> Double [NodeDistance]. Defult 40. </summary>
        private const float NodeDistanceDouble = 40.0f;
        /// <summary> Whether the distance'LengthSquared exceeds [NodeDistance]. Defult: 400. </summary>
        public static bool OutNodeDistance(Vector2 node0, Vector2 node1) => (node0 - node1).LengthSquared() > 400.0f;// Transformer.NodeDistance * Transformer.NodeDistance;


        /// <summary> Get outside node. </summary>
        internal static Vector2 OutsideNode(Vector2 nearNode, Vector2 farNode) => nearNode - Vector2.Normalize(farNode - nearNode) * Transformer.NodeDistanceDouble;

        /// <summary>
        /// Point inside the Quadrangle
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <returns></returns>
        public static bool InQuadrangle(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            float a = (leftTop.X - leftBottom.X) * (point.Y - leftBottom.Y) - (leftTop.Y - leftBottom.Y) * (point.X - leftBottom.X);
            float b = (rightTop.X - leftTop.X) * (point.Y - leftTop.Y) - (rightTop.Y - leftTop.Y) * (point.X - leftTop.X);
            float c = (rightBottom.X - rightTop.X) * (point.Y - rightTop.Y) - (rightBottom.Y - rightTop.Y) * (point.X - rightTop.X);
            float d = (leftBottom.X - rightBottom.X) * (point.Y - rightBottom.Y) - (leftBottom.Y - rightBottom.Y) * (point.X - rightBottom.X);
            return (a > 0 && b > 0 && c > 0 && d > 0) || (a < 0 && b < 0 && c < 0 && d < 0);
        }
        /// <summary>
        /// Point inside the transformer's quadrangle.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <returns></returns>
        public static bool InQuadrangle(Vector2 point, Transformer transformer) => Transformer.InQuadrangle(point, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom);
        /// <summary>
        /// Point inside the transformer's quadrangle.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns></returns>
        public bool InQuadrangle(Vector2 point) => Transformer.InQuadrangle(point, this.LeftTop, this.RightTop, this.RightBottom, this.LeftBottom);



        /// <summary>
        /// The transformer was contained in a rectangle.
        /// </summary>
        /// <param name="left"> The destination rectangle's left. </param>
        /// <param name="top"> The destination rectangle's top. </param>
        /// <param name="right"> The destination rectangle's right. </param>
        /// <param name="bottom"> The destination rectangle's bottom. </param>
        /// <returns></returns>
        public bool Contained(float left, float top, float right, float bottom)
        {
            if (this.MinX < left) return false;
            if (this.MinY < top) return false;
            if (this.MaxX > right) return false;
            if (this.MaxY > bottom) return false;

            return true;
        }
        /// <summary>
        /// The transformer was contained in a rectangle.
        /// </summary>
        /// <param name="rect"> The destination rectangle. </param>
        /// <returns></returns>
        public bool Contained(TransformerRect rect) => this.Contained(rect.Left, rect.Top, rect.Right, rect.Bottom);

    }
}