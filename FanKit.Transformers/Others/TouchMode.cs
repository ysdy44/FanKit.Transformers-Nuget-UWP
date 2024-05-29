namespace FanKit.Transformers
{
    /// <summary>
    /// The touch mode of <see cref = "CanvasOperator" />.
    /// </summary>
    public enum TouchMode
    {
        /// <summary> Disabled. </summary>
        Disable,
        /// <summary> <see cref="InputDevice.SingleFinger"/>. </summary>
        SingleFinger,
        /// <summary> <see cref="InputDevice.RightButton"/>. </summary>
        RightButton,
    }
}