namespace FanKit.Transformers
{
    /// <summary>
    /// Mode of <see cref="IndicatorControl"/>.
    /// </summary>
    public enum IndicatorMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary>
        /// <see cref="Transformer.LeftTop"/>.
        /// </summary>
        LeftTop,
        /// <summary>
        /// <see cref="Transformer.RightTop"/>.
        /// </summary>
        RightTop,
        /// <summary>
        /// <see cref="Transformer.RightBottom"/>.
        /// </summary>
        RightBottom,
        /// <summary>
        /// <see cref="Transformer.LeftBottom"/>.
        /// </summary>
        LeftBottom,

        /// <summary>
        /// <see cref="Transformer.CenterLeft"/>.
        /// </summary>
        Left,
        /// <summary>
        /// <see cref="Transformer.CenterTop"/>.
        /// </summary>
        Top,
        /// <summary>
        /// <see cref="Transformer.CenterRight"/>.
        /// </summary>
        Right,
        /// <summary>
        /// <see cref="Transformer.CenterBottom"/>.
        /// </summary>
        Bottom,

        /// <summary>
        /// <see cref="Transformer.Center"/>.
        /// </summary>
        Center,
    }
}