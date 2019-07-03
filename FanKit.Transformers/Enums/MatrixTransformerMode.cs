namespace FanKit.Transformers
{
    /// <summary>
    /// <see cref = "CanvasTransformer" />'s matrix mode. 
    /// </summary>
    public enum MatrixTransformerMode
    {
        /// <summary> Canvas > Virtual > Control . </summary>
        CanvasToVirtualToControl,
        /// <summary> Canvas > Virtual. </summary>
        CanvasToVirtual,
        /// <summary> Virtual > Control . </summary>
        VirtualToControl,
    }
}