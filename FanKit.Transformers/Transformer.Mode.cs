﻿using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// Represents a Transformer (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {

        private static TransformerMode _ContainsNodeMode(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, bool disabledRadian)
        {
            //Scale2
            if (Transformer.InNodeRadius(leftTop, point)) return TransformerMode.ScaleLeftTop;
            if (Transformer.InNodeRadius(rightTop, point)) return TransformerMode.ScaleRightTop;
            if (Transformer.InNodeRadius(rightBottom, point)) return TransformerMode.ScaleRightBottom;
            if (Transformer.InNodeRadius(leftBottom, point)) return TransformerMode.ScaleLeftBottom;

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale1
            if (Transformer.InNodeRadius(centerLeft, point)) return TransformerMode.ScaleLeft;
            if (Transformer.InNodeRadius(centerTop, point)) return TransformerMode.ScaleTop;
            if (Transformer.InNodeRadius(centerRight, point)) return TransformerMode.ScaleRight;
            if (Transformer.InNodeRadius(centerBottom, point)) return TransformerMode.ScaleBottom;

            //Rotation
            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Rotation
                if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.Rotation;

                //Skew
                //if (Transformer.InNodeRadius(outsideLeft, point)) return TransformerMode.SkewLeft;
                //if (Transformer.InNodeRadius(outsideTop, point)) return TransformerMode.SkewTop;
                if (Transformer.InNodeRadius(outsideRight, point)) return TransformerMode.SkewRight;
                if (Transformer.InNodeRadius(outsideBottom, point)) return TransformerMode.SkewBottom;
            }

            //Translation
            if (Transformer.InQuadrangle(point, leftTop, rightTop, rightBottom, leftBottom))
            {
                return TransformerMode.Translation;
            }

            return TransformerMode.None;
        }

        /// <summary>
        /// Gets the radian area filled by the skew node contains the specified point. 
        /// </summary>
        /// <param name="point"> Input point. </param>
        /// <param name="transformer"> Layer's Transformer. </param>
        /// <param name="disabledRadian"> disabled radian </param>
        /// <returns></returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, bool disabledRadian = false)
        {
            Vector2 leftTop = (transformer.LeftTop);
            Vector2 rightTop = (transformer.RightTop);
            Vector2 rightBottom = (transformer.RightBottom);
            Vector2 leftBottom = (transformer.LeftBottom);

            return Transformer._ContainsNodeMode(point, leftTop, rightTop, rightBottom, leftBottom, disabledRadian);
        }

        /// <summary>
        /// Gets the radian area filled by the skew node contains the specified point. 
        /// </summary>
        /// <param name="point"> Input point. </param>
        /// <param name="transformer"> Layer's Transformer. </param>
        /// <param name="matrix"></param>
        /// <param name="disabledRadian"> disabled radian </param>
        /// <returns></returns>
        public static TransformerMode ContainsNodeMode(Vector2 point, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false)
        {
            Vector2 leftTop = Vector2.Transform(transformer.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformer.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformer.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformer.LeftBottom, matrix);

            return Transformer._ContainsNodeMode(point, leftTop, rightTop, rightBottom, leftBottom, disabledRadian);
        }
    }
}