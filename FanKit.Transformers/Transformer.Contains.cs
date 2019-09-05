using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {
        /// <summary>
        /// Returns whether the area filled by the transformer contains the specified point.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns> Return **true** if the area filled by the transformer contains the specified point, otherwise **false**. </returns>
        public bool FillContainsPoint(Vector2 point) => Math.InQuadrangle(point, this.LeftTop, this.RightTop, this.RightBottom, this.LeftBottom);
        
        /// <summary>
        /// Returns whether the transformer was contained in the rectangle.
        /// </summary>
        /// <param name="left"> The destination rectangle's left. </param>
        /// <param name="top"> The destination rectangle's top. </param>
        /// <param name="right"> The destination rectangle's right. </param>
        /// <param name="bottom"> The destination rectangle's bottom. </param>
        /// <returns> Return **true** if the transformer was contained in rectangle, otherwise **false**. </returns>
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
        /// <param name="transformerRect"> The destination rectangle. </param>
        /// <returns> Return **true** if the transformer was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(TransformerRect transformerRect) => this.Contained(transformerRect.Left, transformerRect.Top, transformerRect.Right, transformerRect.Bottom);

    }
}