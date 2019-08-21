using System;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {
        /// <summary> Vector in LeftTop. </summary>
        public Vector2 LeftTop;
        /// <summary> Vector in RightTop. </summary>
        public Vector2 RightTop;
        /// <summary> Vector in RightBottom. </summary>
        public Vector2 RightBottom;
        /// <summary> Vector in LeftBottom. </summary>
        public Vector2 LeftBottom;



        //@Constructs
        /// <summary>
        /// Constructs a transformer.
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
        /// Constructs a transformer.
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
        /// Constructs a transformer.
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
        /// Constructs a transformer.
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
        /// Constructs a transformer.
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



        /// <summary> Gets the center vector. </summary>
        public Vector2 Center => (this.LeftTop + this.RightTop + this.RightBottom + this.LeftBottom) / 4;

        /// <summary> Gets the center left vector. </summary>
        public Vector2 CenterLeft => (this.LeftTop + this.LeftBottom) / 2;
        /// <summary> Gets the center top vector. </summary>
        public Vector2 CenterTop => (this.LeftTop + this.RightTop) / 2;
        /// <summary> Gets the center right vector. </summary>
        public Vector2 CenterRight => (this.RightTop + this.RightBottom) / 2;
        /// <summary> Gets the center bottom vector. </summary>
        public Vector2 CenterBottom => (this.RightBottom + this.LeftBottom) / 2;

        /// <summary> Gets the minimum value on the X-Axis. </summary>
        public float MinX => Math.Min(Math.Min(this.LeftTop.X, this.RightTop.X), Math.Min(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the maximum  value on the X-Axis. </summary>
        public float MaxX => Math.Max(Math.Max(this.LeftTop.X, this.RightTop.X), Math.Max(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the minimum value on the Y-Axis. </summary>
        public float MinY => Math.Min(Math.Min(this.LeftTop.Y, this.RightTop.Y), Math.Min(this.RightBottom.Y, this.LeftBottom.Y));
        /// <summary> Gets the maximum  value on the Y-Axis. </summary>
        public float MaxY => Math.Max(Math.Max(this.LeftTop.Y, this.RightTop.Y), Math.Max(this.RightBottom.Y, this.LeftBottom.Y));

        /// <summary> Gets horizontal vector. </summary>
        public Vector2 Horizontal => (this.RightTop + this.RightBottom - this.LeftTop - this.LeftBottom) / 2;
        /// <summary> Gets vertical vector. </summary>
        public Vector2 Vertical => (this.RightBottom + this.LeftBottom - this.LeftTop - this.RightTop) / 2;


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


        //@Static
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


    }
}