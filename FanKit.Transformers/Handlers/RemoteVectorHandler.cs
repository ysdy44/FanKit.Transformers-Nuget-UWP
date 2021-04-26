using System.Numerics;

namespace FanKit.Transformers
{
    //@Delegate
    /// <summary>
    /// Method that represents the handling of the ValueChanged event.
    /// </summary>
    /// <param name="sender"> The object to which the event handler is attached. </param>
    /// <param name="value"> Event data. </param>
    public delegate void RemoteVectorHandler(object sender, Vector2 value);
}