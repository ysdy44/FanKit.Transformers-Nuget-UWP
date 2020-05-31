namespace FanKit.Transformers
{
    /// <summary>
    /// Mode of <see cref="Node"/>. 
    /// </summary>
    public enum NodeMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Checked node's point. </summary>
        PointWithChecked,
        /// <summary> Unchecked node's point. </summary>
        PointWithoutChecked,

        /// <summary> Left-control-point. </summary>
        LeftControlPoint,
        /// <summary> Right-control-point. </summary>
        RightControlPoint,
    }
}