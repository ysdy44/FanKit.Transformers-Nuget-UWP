using System.Numerics;

namespace FanKit.Transformers
{
    public partial struct Transformer : ITransformerLTRB, ITransformerGeometry
    {

        private static TransformerMode _containsNodeMode(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, bool disabledRadian)
        {
            //Scale2
            if (Math.InNodeRadius(leftTop, point)) return TransformerMode.ScaleLeftTop;
            if (Math.InNodeRadius(rightTop, point)) return TransformerMode.ScaleRightTop;
            if (Math.InNodeRadius(rightBottom, point)) return TransformerMode.ScaleRightBottom;
            if (Math.InNodeRadius(leftBottom, point)) return TransformerMode.ScaleLeftBottom;

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale1
            if (Math.InNodeRadius(centerLeft, point)) return TransformerMode.ScaleLeft;
            if (Math.InNodeRadius(centerTop, point)) return TransformerMode.ScaleTop;
            if (Math.InNodeRadius(centerRight, point)) return TransformerMode.ScaleRight;
            if (Math.InNodeRadius(centerBottom, point)) return TransformerMode.ScaleBottom;

            //Rotation
            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Math.GetOutsidePointInTransformer(centerLeft, centerRight);
                Vector2 outsideTop = Math.GetOutsidePointInTransformer(centerTop, centerBottom);
                Vector2 outsideRight = Math.GetOutsidePointInTransformer(centerRight, centerLeft);
                Vector2 outsideBottom = Math.GetOutsidePointInTransformer(centerBottom, centerTop);

                //Rotation
                if (Math.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

                //Skew
                //if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
                //if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
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
            Vector2 leftTop = (transformer.LeftTop);
            Vector2 rightTop = (transformer.RightTop);
            Vector2 rightBottom = (transformer.RightBottom);
            Vector2 leftBottom = (transformer.LeftBottom);

            return Transformer._containsNodeMode(point, leftTop, rightTop, rightBottom, leftBottom, disabledRadian);
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

            return Transformer._containsNodeMode(point, leftTop, rightTop, rightBottom, leftBottom, disabledRadian);
        }
    }
}