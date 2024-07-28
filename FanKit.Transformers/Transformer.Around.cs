using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleLeft(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            RightTop = startingTransformer.RightTop,
            RightBottom = startingTransformer.RightBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleLeftCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            RightTop = startingTransformer.RightTop - vector,
            RightBottom = startingTransformer.RightBottom - vector,
        };

        #endregion

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleTop(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            RightTop = startingTransformer.RightTop + vector,

            RightBottom = startingTransformer.RightBottom,
            LeftBottom = startingTransformer.LeftBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleTopCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            RightTop = startingTransformer.RightTop + vector,

            RightBottom = startingTransformer.RightBottom - vector,
            LeftBottom = startingTransformer.LeftBottom - vector,
        };

        #endregion

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleRight(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightTop = startingTransformer.RightTop + vector,
            RightBottom = startingTransformer.RightBottom + vector,

            LeftTop = startingTransformer.LeftTop,
            LeftBottom = startingTransformer.LeftBottom
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleRightCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightTop = startingTransformer.RightTop + vector,
            RightBottom = startingTransformer.RightBottom + vector,

            LeftTop = startingTransformer.LeftTop - vector,
            LeftBottom = startingTransformer.LeftBottom - vector,
        };

        #endregion

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleBottom(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightBottom = startingTransformer.RightBottom + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            LeftTop = startingTransformer.LeftTop,
            RightTop = startingTransformer.RightTop
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleBottomCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightBottom = startingTransformer.RightBottom + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            LeftTop = startingTransformer.LeftTop - vector,
            RightTop = startingTransformer.RightTop - vector
        };

        #endregion

    }
}