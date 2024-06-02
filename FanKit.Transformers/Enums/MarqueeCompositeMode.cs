namespace FanKit.Transformers
{
    /// <summary>
    /// The composite mode used for the marquee-tool.
    /// </summary>
    public enum MarqueeCompositeMode
    {
        /// <summary> New bitmap. </summary>
        New,
        /// <summary> Union of source and destination bitmap. </summary>
        Add,
        /// <summary> Region of the source bitmap. </summary>
        Subtract,
        /// <summary> Intersection of source and destination bitmap. </summary>
        Intersect,

        /// <summary> Union of source and destination bitmaps with xor function for pixels that overlap. </summary>
        Xor,
    }
}