using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {
        /// <summary> Gets or sets the position of the top-left corner of the transformer. </summary>
        public Vector2 LeftTop { get; set; }
        /// <summary> Gets or sets the position of the top-right corner of the transformer. </summary>
        public Vector2 RightTop { get; set; }
        /// <summary> Gets or sets the position of the bottom-right corner of the transformer. </summary>
        public Vector2 RightBottom { get; set; }
        /// <summary> Gets or sets the position of the bottom-left corner of the transformer. </summary>
        public Vector2 LeftBottom { get; set; }


        /// <summary> Gets the position of the center of the transformer. </summary>
        public Vector2 Center => (this.LeftTop + this.RightTop + this.RightBottom + this.LeftBottom) / 4;

        /// <summary> Gets the position of the center of bottom-left and top-left corners of the transformer. </summary>
        public Vector2 CenterLeft => (this.LeftTop + this.LeftBottom) / 2;
        /// <summary> Gets the position of the center of top-left and top-right corners of the transformer. </summary>
        public Vector2 CenterTop => (this.LeftTop + this.RightTop) / 2;
        /// <summary> Gets the position of the center of top-right and bottom-right corners of the transformer. </summary>
        public Vector2 CenterRight => (this.RightTop + this.RightBottom) / 2;
        /// <summary> Gets the position of the center of bottom-right and bottom-left corners of the transformer. </summary>
        public Vector2 CenterBottom => (this.RightBottom + this.LeftBottom) / 2;

        /// <summary> Gets the minimum value of the X-axis value for all corners of the transformer. </summary>
        public float MinX => System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the maximum value of the X-axis value for all corners of the transformer. </summary>
        public float MaxX => System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the minimum position of the Y-axis position for all corners of the transformer. </summary>
        public float MinY => System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y));
        /// <summary> Gets the maximum position of the Y-axis position for all corners of the transformer. </summary>
        public float MaxY => System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y));

        /// <summary> Gets the horizontal vector of the transformer. </summary>
        public Vector2 Horizontal => this.CenterRight - this.CenterLeft;
        /// <summary> Gets the vertical vector of the transformer. </summary>
        public Vector2 Vertical => this.CenterBottom - this.CenterTop;


        /// <summary>
        /// Gets value by left, right, top, bottom.
        /// </summary>
        /// <param name="borderMode"> The border mode. </param>
        /// <returns> The produced value. </returns>
        public float GetBorderValue(BorderMode borderMode)
        {
            switch (borderMode)
            {
                case BorderMode.MinX: return this.MinX;
                case BorderMode.CenterX: return this.Center.X;
                case BorderMode.MaxX: return this.MaxX;

                case BorderMode.MinY: return this.MinY;
                case BorderMode.CenterY: return this.Center.Y;
                case BorderMode.MaxY: return this.MaxY;

            }
            return this.MinX;
        }

        /// <summary>
        /// Gets vector by left, right, top, bottom.
        /// </summary>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <returns> The produced vector. </returns>
        public Vector2 GetIndicatorVector(IndicatorMode indicatorMode)
        {
            switch (indicatorMode)
            {
                case IndicatorMode.LeftTop: return this.LeftTop;
                case IndicatorMode.RightTop: return this.RightTop;
                case IndicatorMode.RightBottom: return this.RightBottom;
                case IndicatorMode.LeftBottom: return this.LeftBottom;

                case IndicatorMode.Left: return this.CenterLeft;
                case IndicatorMode.Top: return this.CenterTop;
                case IndicatorMode.Right: return this.CenterRight;
                case IndicatorMode.Bottom: return this.CenterBottom;

                case IndicatorMode.Center: return this.Center;
            }
            return this.LeftTop;
        }
    }
}