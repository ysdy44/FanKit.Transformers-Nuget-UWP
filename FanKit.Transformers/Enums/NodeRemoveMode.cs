namespace FanKit.Transformers
{
    /// <summary>
    /// State of <see cref="Noder"/>. 
    /// </summary>
    public enum NodeRemoveMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Removed all checked nodes. </summary>
        RemovedNodes,

        /// <summary> Remove the curve. </summary>
        RemoveCurve
,
    }
}