namespace FanKit.Transformers
{
    /// <summary> 
    /// The inverse matrix mode of <see cref="CanvasTransformer"/>. 
    /// </summary>
    public enum InverseMatrixTransformerMode
    {
        /// <summary> Control > Virtual > Canvas. </summary>
        ControlToVirtualToCanvas,
        /// <summary> Control > Virtual. </summary>
        ControlToVirtual,
        /// <summary> Virtual > Canvas. </summary>
        VirtualToCanvas,
    }
}