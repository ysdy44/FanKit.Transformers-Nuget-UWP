using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {
        public TransformerBorder Union(Vector2 point)
        {
            return this.IsEmpty ? TransformerBorder.Empty : new TransformerBorder
            {
                Left = System.Math.Min(this.Left, point.X),
                Top = System.Math.Min(this.Top, point.Y),
                Right = System.Math.Max(this.Right, point.X),
                Bottom = System.Math.Max(this.Bottom, point.Y)
            };
        }

        public TransformerBorder Union(TransformerBorder border)
        {
            return this.IsEmpty ? border : new TransformerBorder
            {
                Left = System.Math.Min(this.Left, border.Left),
                Top = System.Math.Min(this.Top, border.Top),
                Right = System.Math.Max(this.Right, border.Right),
                Bottom = System.Math.Max(this.Bottom, border.Bottom)
            };
        }

        public static TransformerBorder Union(TransformerBorder border, Vector2 point)
        {
            return border.IsEmpty ? TransformerBorder.Empty : new TransformerBorder
            {
                Left = System.Math.Min(border.Left, point.X),
                Top = System.Math.Min(border.Top, point.Y),
                Right = System.Math.Max(border.Right, point.X),
                Bottom = System.Math.Max(border.Bottom, point.Y)
            };
        }

        public static TransformerBorder Union(TransformerBorder border1, TransformerBorder border2)
        {
            return border1.IsEmpty ? border2 : new TransformerBorder
            {
                Left = System.Math.Min(border1.Left, border2.Left),
                Top = System.Math.Min(border1.Top, border2.Top),
                Right = System.Math.Max(border1.Right, border2.Right),
                Bottom = System.Math.Max(border1.Bottom, border2.Bottom)
            };
        }
    }
}