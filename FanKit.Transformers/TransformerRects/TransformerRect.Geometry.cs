using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerRect
    {

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator) => TransformerGeometry.CreateRectangle(resourceCreator, new Transformer(this));

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => TransformerGeometry.CreateRectangle(resourceCreator, new Transformer(this), matrix);


        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator) => TransformerGeometry.CreateEllipse(resourceCreator, new Transformer(this));

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => TransformerGeometry.CreateEllipse(resourceCreator, new Transformer(this), matrix);

    }
}