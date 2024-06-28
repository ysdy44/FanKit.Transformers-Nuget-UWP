using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {
        /// <summary>
        /// Gets the all nodes by the border contains the specified point.
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="border"> The source border. </param>
        /// <returns> The border-mode. </returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, TransformerBorder border)
        {
            // Scale2
            if (Math.InNodeRadius(border.Left, border.Top, point.X, point.Y)) return TransformerMode.ScaleLeftTop;
            if (Math.InNodeRadius(border.Right, border.Top, point.X, point.Y)) return TransformerMode.ScaleRightTop;
            if (Math.InNodeRadius(border.Right, border.Bottom, point.X, point.Y)) return TransformerMode.ScaleRightBottom;
            if (Math.InNodeRadius(border.Left, border.Bottom, point.X, point.Y)) return TransformerMode.ScaleLeftBottom;

            // Center
            float cx = border.CenterX;
            float cy = border.CenterY;

            // Scale1
            if (Math.InNodeRadius(border.Left, cy, point.X, point.Y)) return TransformerMode.ScaleLeft;
            if (Math.InNodeRadius(cx, border.Top, point.X, point.Y)) return TransformerMode.ScaleTop;
            if (Math.InNodeRadius(border.Right, cy, point.X, point.Y)) return TransformerMode.ScaleRight;
            if (Math.InNodeRadius(cx, border.Bottom, point.X, point.Y)) return TransformerMode.ScaleBottom;

            return TransformerMode.None;
        }

        /// <summary>
        /// Returns whether the area filled by the border contains the specified point.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns> Return **true** if the area filled by the border contains the specified point, otherwise **false**. </returns>
        public bool FillContainsPoint(Vector2 point)
        {
            return point.X > this.Left && point.X < this.Right && point.Y > this.Top && point.Y < this.Bottom;
        }

        /// <summary>
        /// Returns whether the border was contained in the rectangle.
        /// </summary>
        /// <param name="left"> The X-axis value of the left side of the destination rectangle. </param>
        /// <param name="top"> The Y-axis position of the top of the destination rectangle. </param>
        /// <param name="right"> The X-axis value of the right side of the destination rectangle. </param>
        /// <param name="bottom"> The Y-axis position of the bottom of the destination rectangle. </param>
        /// <returns> Return **true** if the border was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(float left, float top, float right, float bottom)
        {
            return this.Left >= left && this.Top >= top && this.Right <= right && this.Bottom <= bottom;
        }

        /// <summary>
        /// The border was contained in a rectangle.
        /// </summary>
        /// <param name="rect"> The destination rectangle. </param>
        /// <returns> Return **true** if the border was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(TransformerBorder rect)
        {
            return this.Left >= rect.Left && this.Top >= rect.Top && this.Right <= rect.Right && this.Bottom <= rect.Bottom;
        }
    }
}