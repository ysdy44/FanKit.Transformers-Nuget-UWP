using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerRect
    {

        /// <summary> Gets rectangle's left. </summary>
        public float Left { get; private set; }
        /// <summary> Gets rectangle's top. </summary>
        public float Top { get; private set; }
        /// <summary> Gets rectangle's right. </summary>
        public float Right { get; private set; }
        /// <summary> Gets rectangle's bottom. </summary>
        public float Bottom { get; private set; }

        /// <summary> Gets rectangle's width. </summary>
        public float Width { get; private set; }
        /// <summary> Gets rectangle's height. </summary>
        public float Height { get; private set; }

        /// <summary> Gets rectangle's center of X. </summary>
        public float CenterX { get; private set; }
        /// <summary> Gets rectangle's center of Y. </summary>
        public float CenterY { get; private set; }
        /// <summary> Gets rectangle's center point. </summary>
        public Vector2 Center { get; private set; }

        /// <summary> Gets the center left vector. </summary>
        public Vector2 CenterLeft { get; private set; }
        /// <summary> Gets the center top vector. </summary>
        public Vector2 CenterTop { get; private set; }
        /// <summary> Gets the center right vector. </summary>
        public Vector2 CenterRight { get; private set; }
        /// <summary> Gets the center bottom vector. </summary>
        public Vector2 CenterBottom { get; private set; }

        /// <summary> Gets the position of the top-left corner of the rectangle. </summary>
        public Vector2 LeftTop { get; private set; }
        /// <summary> Gets the position of the top-right corner of the rectangle. </summary>
        public Vector2 RightTop { get; private set; }
        /// <summary> Gets the position of the bottom-right corner of the rectangle. </summary>
        public Vector2 RightBottom { get; private set; }
        /// <summary> Gets the position of the bottom-left corner of the rectangle. </summary>
        public Vector2 LeftBottom { get; private set; }

        /// <summary> Gets horizontal vector. </summary>
        public Vector2 Horizontal => this.CenterRight - this.CenterLeft;
        /// <summary> Gets vertical vector. </summary>
        public Vector2 Vertical => this.CenterBottom - this.CenterTop;
    }
}