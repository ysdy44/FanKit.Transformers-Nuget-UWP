namespace FanKit.Transformers
{
    /// <summary>
    /// Mode of <see cref="Transformer"/>. 
    /// </summary>
    public enum TransformerMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Rotation. </summary>
        Rotation,

        /// <summary> Skew (The node at the left side is dragged). </summary>
        SkewLeft,
        /// <summary> Skew (The node at the top is dragged). </summary>
        SkewTop,
        /// <summary> Skew (The node at the right side is dragged). </summary>
        SkewRight,
        /// <summary> Skew (The node at the bottom is dragged). </summary>
        SkewBottom,

        /// <summary> Scale (The node at the left side is dragged). </summary>
        ScaleLeft,
        /// <summary> Scale (The node at the top is dragged). </summary>
        ScaleTop,
        /// <summary> Scale (The node at the right side is dragged). </summary>
        ScaleRight,
        /// <summary> Scale (The node at the bottom is dragged). </summary>
        ScaleBottom,

        /// <summary> Scale (The node at the top-left corner is dragged). </summary>
        ScaleLeftTop,
        /// <summary> Scale (The node at the top-right corner is dragged). </summary>
        ScaleRightTop,
        /// <summary> Scale (The node at the bottom-right corner is dragged). </summary>
        ScaleRightBottom,
        /// <summary> Scale (The node at the bottom-left corner is dragged). </summary>
        ScaleLeftBottom,
    }
}