using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerRect
    {

        /// <summary> Gets the X-axis value of the left side of the rectangle. </summary>
        public float Left { get; private set; }
        /// <summary> Gets the Y-axis position of the top of the rectangle. </summary>
        public float Top { get; private set; }
        /// <summary> Gets the X-axis value of the right side of the rectangle. </summary>
        public float Right { get; private set; }
        /// <summary> Gets the Y-axis position of the bottom of the rectangle. </summary>
        public float Bottom { get; private set; }

        /// <summary> Gets the width of the rectangle. </summary>
        public float Width { get; private set; }
        /// <summary> Gets the height of the rectangle. </summary>
        public float Height { get; private set; }

        /// <summary> Gets the X-axis value of the center of the rectangle. </summary>
        public float CenterX { get; private set; }
        /// <summary> Gets the Y-axis position of the center of the rectangle. </summary>
        public float CenterY { get; private set; }
        /// <summary> Gets the position of the center of the rectangle. </summary>
        public Vector2 Center { get; private set; }

        /// <summary> Gets the position of the center of bottom-left and top-left corners of the rectangle. </summary>
        public Vector2 CenterLeft { get; private set; }
        /// <summary> Gets the position of the center of top-left and top-right corners of the rectangle. </summary>
        public Vector2 CenterTop { get; private set; }
        /// <summary> Gets the position of the center of top-right and bottom-right corners of the rectangle. </summary>
        public Vector2 CenterRight { get; private set; }
        /// <summary> Gets the position of the center of bottom-right and bottom-left corners of the rectangle. </summary>
        public Vector2 CenterBottom { get; private set; }

        /// <summary> Gets the position of the top-left corner of the rectangle. </summary>
        public Vector2 LeftTop { get; private set; }
        /// <summary> Gets the position of the top-right corner of the rectangle. </summary>
        public Vector2 RightTop { get; private set; }
        /// <summary> Gets the position of the bottom-right corner of the rectangle. </summary>
        public Vector2 RightBottom { get; private set; }
        /// <summary> Gets the position of the bottom-left corner of the rectangle. </summary>
        public Vector2 LeftBottom { get; private set; }

        /// <summary> Gets the horizontal vector of the rectangle. </summary>
        public Vector2 Horizontal => this.CenterRight - this.CenterLeft;
        /// <summary> Gets the vertical vector of the rectangle. </summary>
        public Vector2 Vertical => this.CenterBottom - this.CenterTop;
    }
}