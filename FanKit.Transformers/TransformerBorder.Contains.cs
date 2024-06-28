using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {
        public static TransformerMode ContainsNodeMode(Vector2 point, TransformerBorder border)
        {
            // Scale2
            if (Math.InNodeRadius(border.Left, border.Top, point.X, point.Y)) return TransformerMode.ScaleLeftTop;
            if (Math.InNodeRadius(border.Right, border.Top, point.X, point.Y)) return TransformerMode.ScaleRightTop;
            if (Math.InNodeRadius(border.Right, border.Bottom, point.X, point.Y)) return TransformerMode.ScaleRightBottom;
            if (Math.InNodeRadius(border.Left, border.Bottom, point.X, point.Y)) return TransformerMode.ScaleLeftBottom;

            // Center
            float cx = border.CenterX;
            float cy = border.CenterY;

            // Scale1
            if (Math.InNodeRadius(border.Left, cy, point.X, point.Y)) return TransformerMode.ScaleLeft;
            if (Math.InNodeRadius(cx, border.Top, point.X, point.Y)) return TransformerMode.ScaleTop;
            if (Math.InNodeRadius(border.Right, cy, point.X, point.Y)) return TransformerMode.ScaleRight;
            if (Math.InNodeRadius(cx, border.Bottom, point.X, point.Y)) return TransformerMode.ScaleBottom;

            return TransformerMode.None;
        }

        public bool FillContainsPoint(Vector2 point)
        {
            return point.X > this.Left && point.X < this.Right && point.Y > this.Top && point.Y < this.Bottom;
        }

        public bool Contained(float left, float top, float right, float bottom)
        {
            return this.Left >= left && this.Top >= top && this.Right <= right && this.Bottom <= bottom;
        }

        public bool Contained(TransformerBorder rect)
        {
            return this.Left >= rect.Left && this.Top >= rect.Top && this.Right <= rect.Right && this.Bottom <= rect.Bottom;
        }
    }
}