using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {

        /// <summary>
        /// Turn to rectangle.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Rect ToRect() => new Rect(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);

        /// <summary>
        /// Turn to transformer.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Transformer ToTransformer() => new Transformer(this.Left, this.Top, this.Right, this.Bottom);


        /// <summary>
        /// Expands the current border exactly enough to contain the specified point.
        /// </summary>
        /// <param name="point"> The specified point. </param>
        /// <returns> The product border. </returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// Expands the current border exactly enough to contain the specified border.
        /// </summary>
        /// <param name="border"> The specified border. </param>
        /// <returns> The product border. </returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// Creates a border that is exactly large enough to include the specified border and the specified point.
        /// </summary>
        /// <param name="border"> The specified border. </param>
        /// <param name="point"> The specified point. </param>
        /// <returns> The product border. </returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

        /// <summary>
        /// Creates a border that is exactly large enough to contain the two specified borders.
        /// </summary>
        /// <param name="border1"> The specified first border. </param>
        /// <param name="border2"> The specified second border. </param>
        /// <returns> The product border. </returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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