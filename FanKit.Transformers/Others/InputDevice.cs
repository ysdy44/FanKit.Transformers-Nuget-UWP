namespace FanKit.Transformers
{
    /// <summary>
    /// The input device type of <see cref = "CanvasOperator" />.
    /// </summary>
    public enum InputDevice
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Indeterminacy. </summary>
        Indeterminacy,

        /// <summary>Holding. </summary>
        Holding,
        /// <summary> One Finger. </summary>
        SingleFinger,
        /// <summary> Two Fingers. </summary>
        DoubleFinger,

        /// <summary> Pen. </summary>
        Pen,
        /// <summary> Eraser. </summary>
        Eraser,

        /// <summary> Mouse-Left-Button. </summary>
        LeftButton,
        /// <summary> Mouse-Right-Button. </summary>
        RightButton,
    }
}