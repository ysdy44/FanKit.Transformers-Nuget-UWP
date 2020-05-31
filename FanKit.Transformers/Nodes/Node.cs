using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial class Node : ICacheTransform
    {

        /// <summary> Point. </summary>
        public Vector2 Point;
        /// <summary> The cache of <see cref="Node.Point"/>. </summary>
        public Vector2 StartingPoint { get; private set; }

        /// <summary> The specified control point on left. </summary>
        public Vector2 LeftControlPoint;
        /// <summary> The cache of <see cref="Node.LeftControlPoint"/>. </summary>
        public Vector2 StartingLeftControlPoint { get; private set; }

        /// <summary> The specified control point on right. </summary>
        public Vector2 RightControlPoint;
        /// <summary> The cache of <see cref="Node.RightControlPoint"/>. </summary>
        public Vector2 StartingRightControlPoint { get; private set; }


        /// <summary> Gets or Sets noder's IsChecked. </summary>
        public bool IsChecked { get; set; }
        /// <summary> The cache of <see cref="Node.IsChecked"/>. </summary>
        public bool StartingIsChecked { get; private set; }

        /// <summary> Gets or Sets whether the noder is smooth. </summary>
        public bool IsSmooth { get; set; }
        /// <summary> The cache of <see cref="Node.IsSmooth"/>. </summary>
        public bool StartingIsSmooth { get; private set; }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned node. </returns>
        public Node Clone()
        {
            return new Node
            {
                Point = this.Point,
                StartingPoint = this.StartingPoint,

                LeftControlPoint = this.LeftControlPoint,
                StartingLeftControlPoint = this.StartingLeftControlPoint,

                RightControlPoint = this.RightControlPoint,
                StartingRightControlPoint = this.StartingRightControlPoint,


                IsChecked = this.IsChecked,
                StartingIsChecked = this.StartingIsChecked,

                IsSmooth = this.IsSmooth,
                StartingIsSmooth = this.StartingIsSmooth,
            };
        }

        /// <summary>
        ///  Cache the node's transformer.
        /// </summary>
        public void CacheTransform()
        {
            this.StartingPoint = this.Point;
            this.StartingLeftControlPoint = this.LeftControlPoint;
            this.StartingRightControlPoint = this.RightControlPoint;

            this.StartingIsChecked = this.IsChecked;
            this.StartingIsSmooth = this.IsSmooth;
        }
        /// <summary>
        ///  Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector)
        {
            this.Point = this.StartingPoint + vector;
            this.LeftControlPoint = this.StartingLeftControlPoint + vector;
            this.RightControlPoint = this.StartingRightControlPoint + vector;
        }
        /// <summary>
        ///  Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Point = Vector2.Transform(this.StartingPoint, matrix);
            this.LeftControlPoint = Vector2.Transform(this.StartingLeftControlPoint, matrix);
            this.RightControlPoint = Vector2.Transform(this.StartingRightControlPoint, matrix);
        }

    }
}