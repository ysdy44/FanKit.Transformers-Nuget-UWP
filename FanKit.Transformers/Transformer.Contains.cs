using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {
        private static TransformerMode ContainsNodeModeCore(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, bool disabledRadian)
        {
            // Scale2
            if (Math.InNodeRadius(leftTop, point)) return TransformerMode.ScaleLeftTop;
            if (Math.InNodeRadius(rightTop, point)) return TransformerMode.ScaleRightTop;
            if (Math.InNodeRadius(rightBottom, point)) return TransformerMode.ScaleRightBottom;
            if (Math.InNodeRadius(leftBottom, point)) return TransformerMode.ScaleLeftBottom;

            // Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            // Scale1
            if (Math.InNodeRadius(centerLeft, point)) return TransformerMode.ScaleLeft;
            if (Math.InNodeRadius(centerTop, point)) return TransformerMode.ScaleTop;
            if (Math.InNodeRadius(centerRight, point)) return TransformerMode.ScaleRight;
            if (Math.InNodeRadius(centerBottom, point)) return TransformerMode.ScaleBottom;

            // Rotation
            if (disabledRadian == false)
            {
                // Outside
                Vector2 outsideLeft = Math.GetOutsidePointInTransformer(centerLeft, centerRight);
                Vector2 outsideTop = Math.GetOutsidePointInTransformer(centerTop, centerBottom);
                Vector2 outsideRight = Math.GetOutsidePointInTransformer(centerRight, centerLeft);
                Vector2 outsideBottom = Math.GetOutsidePointInTransformer(centerBottom, centerTop);

                // Rotation
                if (Math.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

                // Skew
                // if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
                // if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
                if (Math.InNodeRadius(outsideRight, point)) return TransformerMode.SkewRight;
                if (Math.InNodeRadius(outsideBottom, point)) return TransformerMode.SkewBottom;
            }

            return TransformerMode.None;
        }

        /// <summary>
        /// Gets the all nodes by the transformer contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="disabledRadian"> The disabled radian. </param>
        /// <returns> The transformer-mode. </returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, bool disabledRadian = false)
        {
            Vector2 leftTop = transformer.LeftTop;
            Vector2 rightTop = transformer.RightTop;
            Vector2 rightBottom = transformer.RightBottom;
            Vector2 leftBottom = transformer.LeftBottom;

            return Transformer.ContainsNodeModeCore(point, leftTop, rightTop, rightBottom, leftBottom, disabledRadian);
        }

        /// <summary>
        /// Gets the all nodes by the transformer contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="disabledRadian"> The disabled radian. </param>
        /// <returns> The transformer-mode. </returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false)
        {
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            return Transformer.ContainsNodeModeCore(point, leftTop, rightTop, rightBottom, leftBottom, disabledRadian);
        }


        /// <summary>
        /// Returns whether the area filled by the transformer contains the specified point.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns> Return **true** if the area filled by the transformer contains the specified point, otherwise **false**. </returns>
        public bool FillContainsPoint(Vector2 point) => Math.InQuadrangle(point, this.LeftTop, this.RightTop, this.RightBottom, this.LeftBottom);

        /// <summary>
        /// Returns whether the transformer was contained in the rectangle.
        /// </summary>
        /// <param name="left"> The X-axis value of the left side of the destination rectangle. </param>
        /// <param name="top"> The Y-axis position of the top of the destination rectangle. </param>
        /// <param name="right"> The X-axis value of the right side of the destination rectangle. </param>
        /// <param name="bottom"> The Y-axis position of the bottom of the destination rectangle. </param>
        /// <returns> Return **true** if the transformer was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(float left, float top, float right, float bottom)
        {
            return this.MinX >= left && this.MinY >= top && this.MaxX <= right && this.MaxY <= bottom;
        }

        /// <summary>
        /// The transformer was contained in a rectangle.
        /// </summary>
        /// <param name="transformerRect"> The destination rectangle. </param>
        /// <returns> Return **true** if the transformer was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(TransformerRect transformerRect) => this.Contained(transformerRect.Left, transformerRect.Top, transformerRect.Right, transformerRect.Bottom);

    }
}