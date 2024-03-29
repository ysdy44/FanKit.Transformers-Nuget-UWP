﻿using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer : ITransformerLTRB, ITransformerGeometry
    {

        //@Static
        /// <summary>
        /// Returns the transformer (-1,-1) (1,-1) (1,1) (-1,1).
        /// </summary>
        public static readonly Transformer One = new Transformer
        {
            LeftTop = new Vector2(-1f, -1f),
            RightTop = new Vector2(1f, -1f),
            RightBottom = new Vector2(1f, 1f),
            LeftBottom = new Vector2(-1f, 1f),
        };

        //@Constructs
        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="left"> The left. </param>
        /// <param name="top"> The top. </param>
        /// <param name="right"> The right. </param>
        /// <param name="bottom"> The bottom. </param>
        public Transformer(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="transformerRect"> The initial rectangle. </param>
        public Transformer(TransformerRect transformerRect)
        {
            this.LeftTop = transformerRect.LeftTop;
            this.RightTop = transformerRect.RightTop;
            this.RightBottom = transformerRect.RightBottom;
            this.LeftBottom = transformerRect.LeftBottom;
        }

        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="pointA"> The frist point of transformer. </param>
        /// <param name="pointB"> The second point of transformer. </param>
        public Transformer(Vector2 pointA, Vector2 pointB)
        {
            TransformerRect rect = new TransformerRect(pointA, pointB);

            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="pointA"> The frist point of transformer. </param>
        /// <param name="pointB"> The second point of transformer. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        public Transformer(Vector2 pointA, Vector2 pointB, bool isCenter, bool isRatio)
        {
            TransformerRect rect = new TransformerRect(pointA, pointB, isCenter, isRatio);

            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        /// <param name="postion"> The postion. </param>
        public Transformer(float width, float height, Vector2 postion)
        {
            this.LeftTop = postion;
            this.RightTop = new Vector2(postion.X + width, postion.Y);
            this.RightBottom = new Vector2(postion.X + width, postion.Y + height);
            this.LeftBottom = new Vector2(postion.X, postion.Y + height);
        }


        //@Static
        /// <summary>
        /// Adds transformer and vector together.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="vector"> The added vector. </param>
        /// <returns> The resulting transformer. </returns>
        public static Transformer Add(Transformer transformer, Vector2 vector) => new Transformer
        {
            LeftTop = transformer.LeftTop + vector,
            RightTop = transformer.RightTop + vector,
            RightBottom = transformer.RightBottom + vector,
            LeftBottom = transformer.LeftBottom + vector,
        };

        /// <summary>
        /// Multiplies transformer and vector  and returns the resulting transformer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The transformation matrix. </param>
        /// <returns> The product transformer. </returns>
        public static Transformer Multiplies(Transformer transformer, Matrix3x2 matrix) => new Transformer
        {
            LeftTop = Vector2.Transform(transformer.LeftTop, matrix),
            RightTop = Vector2.Transform(transformer.RightTop, matrix),
            RightBottom = Vector2.Transform(transformer.RightBottom, matrix),
            LeftBottom = Vector2.Transform(transformer.LeftBottom, matrix)
        };


        /// <summary>
        /// Adds transformer and vector together.
        /// </summary>
        /// <param name="value1"> The source transformer. </param>
        /// <param name="value2"> The source vector. </param>
        /// <returns> The summed transformer. </returns>
        public static Transformer operator +(Transformer value1, Vector2 value2) => Transformer.Add(value1, value2);

        /// <summary>
        /// Multiplies transformer and vector  and returns the resulting transformer.
        /// </summary>
        /// <param name="value1">  The source transformer. </param>
        /// <param name="value2"> The scaling value to use. </param>
        /// <returns> The resulting transformer. </returns>
        public static Transformer operator *(Transformer value1, Matrix3x2 value2) => Transformer.Multiplies(value1, value2);


        /// <summary>
        /// Returns a boolean indicating whether the given Transformer is equal to this Transformer instance.
        /// </summary>
        /// <param name="other"> The Transformer to compare this instance to. </param>
        /// <returns> Return **true** if the other Transformer is equal to this instance, otherwise **false**. </returns>
        public bool Equals(Transformer other)
        {
            if (this.LeftTop != other.LeftTop) return false;
            if (this.RightTop != other.RightTop) return false;
            if (this.RightBottom != other.RightBottom) return false;
            if (this.LeftBottom != other.LeftBottom) return false;
            return true;
        }

        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified nodes is equal.
        /// </summary>
        /// <param name="left"> The first transformer to compare. </param>
        /// <param name="right"> The second transformer to compare. </param>
        /// <returns> Return **true** if left and right are equal, otherwise **false**. </returns>
        public static bool operator ==(Transformer left, Transformer right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given nodes are not equal.
        /// </summary>
        /// <param name="left"> The first transformer to compare. </param>
        /// <param name="right"> The second transformer to compare. </param>
        /// <returns> Return **true** if the nodes are not equal; False if they are equal. </returns>
        public static bool operator !=(Transformer left, Transformer right) => !left.Equals(right);

    }
}