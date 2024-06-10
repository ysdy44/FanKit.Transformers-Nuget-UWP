namespace FanKit.Transformers
{
    partial class Node
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
            return this.Point.X >= left && this.Point.Y >= top && this.Point.X <= right && this.Point.Y <= bottom;
        }

        /// <summary>
        /// The vector was contained in a rectangle.
        /// </summary>
        /// <param name="transformerRect"> The destination rectangle. </param>
        /// <returns> Return **true** if the vector was contained in rectangle. </returns>
        public bool Contained(TransformerRect transformerRect) => this.Contained(transformerRect.Left, transformerRect.Top, transformerRect.Right, transformerRect.Bottom);

    }
}