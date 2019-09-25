using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// An interface provides a methon to convert geometry.
    /// </summary>
    public interface ITransformerGeometry
    {
        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>
         CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator);

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
         CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix);


        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns></returns>
         CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator);

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns></returns>
         CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix);

    }
}