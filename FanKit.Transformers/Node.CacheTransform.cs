using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial struct Node : ICacheTransform
    {

        /// <summary>
        ///  Cache the node's transformer.
        /// </summary>
        public void CacheTransform()
        {
            this._oldPoint = this.Point;
            this._oldLeftControlPoint = this.LeftControlPoint;
            this._oldRightControlPoint = this.RightControlPoint;
        }
        /// <summary>
        ///  Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector)
        {
            this.Point = this._oldPoint + vector;
            this.LeftControlPoint = this._oldLeftControlPoint + vector;
            this.RightControlPoint = this._oldRightControlPoint + vector;
        }
        /// <summary>
        ///  Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            this.Point = Vector2.Transform(this._oldPoint, matrix);
            this.LeftControlPoint = Vector2.Transform(this._oldLeftControlPoint, matrix);
            this.RightControlPoint = Vector2.Transform(this._oldRightControlPoint, matrix);
        }

    }
}