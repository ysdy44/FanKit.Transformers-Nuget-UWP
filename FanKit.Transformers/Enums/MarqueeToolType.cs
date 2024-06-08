namespace FanKit.Transformers
{
    /// <summary>
    /// Tools of different shapes for marquee-tool.
    /// </summary>
    public enum MarqueeToolType
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> □ </summary>
        Rectangular,
        // <summary> ▢ </summary>
        //RoundedRectangle,
        /// <summary> ◯ </summary>
        Elliptical,
        /// <summary> 🗨 </summary>
        Polygonal,
        /// <summary> 🗯 </summary>
        FreeHand,
    }
}