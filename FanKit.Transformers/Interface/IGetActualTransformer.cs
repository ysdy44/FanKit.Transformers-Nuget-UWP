using System.Collections.Generic;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides an interface to get the transformation.
    /// </summary>
    public interface IGetActualTransformer
    {
        /// <summary>
        ///  Get the actual transformer.
        /// </summary>
        Transformer GetActualTransformer();
    }
}