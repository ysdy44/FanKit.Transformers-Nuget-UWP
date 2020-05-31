using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial class Node : ICacheTransform
    {

        /// <summary>
        /// The vector was contained in the rectangle.
        /// </summary>
        /// <param name="left"> The destination rectangle's left. </param>
        /// <param name="top"> The destination rectangle's top. </param>
        /// <param name="right"> The destination rectangle's right. </param>
        /// <param name="bottom"> The destination rectangle's bottom. </param>
        /// <returns> Return **true** if the vector was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(float left, float top, float right, float bottom)
        {
            if (this.Point.X < left) return false;
            if (this.Point.Y < top) return false;
            if (this.Point.X > right) return false;
            if (this.Point.Y > bottom) return false;

            return true;
        }
        /// <summary>
        /// The vector was contained in a rectangle.
        /// </summary>
        /// <param name="transformerRect"> The destination rectangle. </param>
        /// <returns> Return **true** if the vector was contained in rectangle. </returns>
        public bool Contained(TransformerRect transformerRect) => this.Contained(transformerRect.Left, transformerRect.Top, transformerRect.Right, transformerRect.Bottom);

    }
}