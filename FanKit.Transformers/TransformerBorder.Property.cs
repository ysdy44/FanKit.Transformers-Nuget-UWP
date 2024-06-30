namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {

        /// <summary> Gets the width of the border. </summary>
        public float Width => this.Right - this.Left;
        /// <summary> Gets the height of the border. </summary>
        public float Height => this.Bottom - this.Top;

        /// <summary> Gets the X-axis value of the center of the border. </summary>
        public float CenterX => (this.Left + this.Right) / 2;
        /// <summary> Gets the Y-axis position of the center of the border. </summary>
        public float CenterY => (this.Top + this.Bottom) / 2;
        /*
        /// <summary> Gets the position of the center of the border. </summary>
        public Vector2 Center => new Vector2(this.CenterX, this.CenterY);   
        
        /// <summary> Gets the position of the center of bottom-left and top-left corners of the border. </summary>
        public Vector2 CenterLeft => new Vector2(this.Left, this.CenterY);
        /// <summary> Gets the position of the center of top-left and top-right corners of the border. </summary>
        public Vector2 CenterTop => new Vector2(this.CenterX, this.Top);
        /// <summary> Gets the position of the center of top-right and bottom-right corners of the border. </summary>
        public Vector2 CenterRight => new Vector2(this.Right, this.CenterY);
        /// <summary> Gets the position of the center of bottom-right and bottom-left corners of the border. </summary>
        public Vector2 CenterBottom => new Vector2(this.CenterX, this.Bottom);

        /// <summary> Gets the position of the top-left corner of the border. </summary>
        public Vector2 LeftTop => new Vector2(this.Left, this.Top);
        /// <summary> Gets the position of the top-right corner of the border. </summary>
        public Vector2 RightTop => new Vector2(this.Right, this.Top);
        /// <summary> Gets the position of the bottom-right corner of the border. </summary>
        public Vector2 RightBottom => new Vector2(this.Right, this.Bottom);
        /// <summary> Gets the position of the bottom-left corner of the border. </summary>
        public Vector2 LeftBottom => new Vector2(this.Left, this.Bottom);

        /// <summary> Gets the horizontal vector of the border. </summary>
        public Vector2 Horizontal => this.CenterRight - this.CenterLeft;
        /// <summary> Gets the vertical vector of the border. </summary>
        public Vector2 Vertical => this.CenterBottom - this.CenterTop;
         */


        /// <summary>
        /// Turn to transformer.
        /// </summary>
        public Transformer ToTransformer() => new Transformer(this.Left, this.Top, this.Right, this.Bottom);


        /// <summary>
        /// Gets value by left, right, top, bottom.
        /// </summary>
        /// <param name="borderMode"> The border mode. </param>
        /// <returns> The produced value. </returns>
        public float GetBorderValue(BorderMode borderMode)
        {
            switch (borderMode)
            {
                case BorderMode.MinX: return this.Left;
                case BorderMode.CenterX: return this.CenterX;
                case BorderMode.MaxX: return this.Right;

                case BorderMode.MinY: return this.Top;
                case BorderMode.CenterY: return this.CenterY;
                case BorderMode.MaxY: return this.Bottom;

            }
            return this.Left;
        }

    }
}