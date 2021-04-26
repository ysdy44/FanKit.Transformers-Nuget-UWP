namespace FanKit.Transformers
{
    /// <summary>
    /// State of <see cref="Node"/>. 
    /// </summary>
    public enum NodeRemoveMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Remove all checked nodes. </summary>
        RemovedNodes,

        /// <summary> Remove the curve. </summary>
        RemoveCurve
,
    }
}