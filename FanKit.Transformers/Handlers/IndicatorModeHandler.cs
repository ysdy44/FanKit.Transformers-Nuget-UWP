namespace FanKit.Transformers
{
    //@Delegate
    /// <summary>
    /// Method that represents the handling of the IndicatorModeChange event.
    /// </summary>
    /// <param name="sender"> The object to which the event handler is attached. </param>
    /// <param name="mode"> The indicator-mode. </param>
    public delegate void IndicatorModeHandler(object sender, IndicatorMode mode);
}