using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerRect
    {

        public float Left { get; private set; }
        public float Top { get; private set; }
        public float Right { get; private set; }
        public float Bottom { get; private set; }

        public float Width { get; private set; }
        public float Height { get; private set; }

        public float CenterX { get; private set; }
        public float CenterY { get; private set; }
        public Vector2 Center { get; private set; }

        public Vector2 CenterLeft { get; private set; }
        public Vector2 CenterTop { get; private set; }
        public Vector2 CenterRight { get; private set; }
        public Vector2 CenterBottom { get; private set; }

        public Vector2 LeftTop { get; private set; }
        public Vector2 RightTop { get; private set; }
        public Vector2 RightBottom { get; private set; }
        public Vector2 LeftBottom { get; private set; }

        public Vector2 Horizontal => this.CenterRight - this.CenterLeft;
        public Vector2 Vertical => this.CenterBottom - this.CenterTop;
    }
}