namespace FanKit.Transformers
{
    /// <summary> 
    /// <see cref = "CanvasTransformer" />'s inverse matrix mode. 
    /// </summary>
    public enum InverseMatrixTransformerMode
    {
        /// <summary> Control > Virtual > Canvas . </summary>
        ControlToVirtualToCanvas,
        /// <summary> Control > Virtual. </summary>
        ControlToVirtual,
        /// <summary> Virtual > Canvas . </summary>
        VirtualToCanvas,
    }
}