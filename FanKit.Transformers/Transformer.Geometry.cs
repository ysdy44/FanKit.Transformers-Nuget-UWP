using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    public partial struct Transformer : ITransformerLTRB, ITransformerGeometry
    {


        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator) => TransformerGeometry.CreateRectangle(resourceCreator, this);

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => TransformerGeometry.CreateRectangle(resourceCreator, this, matrix);


        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns></returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator) => TransformerGeometry.CreateEllipse(resourceCreator, this);

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns></returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => TransformerGeometry.CreateEllipse(resourceCreator, this, matrix);

    }
}