namespace FanKit.Transformers
{
    /// <summary>
    /// Mode of restriction by self control point.
    /// </summary>
    public enum SelfControlPointMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> No change the angle. </summary>
        Angle,
        /// <summary> No change the length. </summary>
        Length,
        /// <summary> Disable control point. </summary>
        Disable,
    }
}
