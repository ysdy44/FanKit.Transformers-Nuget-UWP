namespace FanKit.Transformers
{
    /// <summary>
    /// The matrix mode of <see cref="CanvasTransformer"/>. 
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