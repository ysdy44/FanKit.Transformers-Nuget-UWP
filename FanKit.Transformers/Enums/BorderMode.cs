namespace FanKit.Transformers
{
    /// <summary>
    /// Mode of <see cref="TransformerBorder"/>. 
    /// </summary>
    public enum BorderMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary>
        /// <see cref="Transformer.MinX"/>.
        /// </summary>
        MinX,
        /// <summary>
        /// <see cref="Transformer.CenterX"/>.
        /// </summary>
        CenterX,
        /// <summary>
        /// <see cref="Transformer.MaxX"/>.
        /// </summary>
        MaxX,

        /// <summary>
        /// <see cref="Transformer.MinY"/>.
        /// </summary>
        MinY,
        /// <summary>
        /// <see cref="Transformer.CenterY"/>.
        /// </summary>
        CenterY,
        /// <summary>
        /// <see cref="Transformer.MaxY"/>.
        /// </summary>
        MaxY,
    }
}