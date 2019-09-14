using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer
    {

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator)
        {
            //LTRB
            Vector2 leftTop = this.LeftTop;
            Vector2 rightTop = this.RightTop;
            Vector2 rightBottom = this.RightBottom;
            Vector2 leftBottom = this.LeftBottom;

            return TransformerGeometry.CreateRectangle(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);
        }

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(this.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(this.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(this.LeftBottom, matrix);

            return TransformerGeometry.CreateRectangle(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);
        }
            
        
        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns></returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator)
        {
            //LTRB
            Vector2 leftTop = this.LeftTop;
            Vector2 rightTop = this.RightTop;
            Vector2 rightBottom = this.RightBottom;
            Vector2 leftBottom = this.LeftBottom;

            return TransformerGeometry.CreateEllipse(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);
        }

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns></returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(this.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(this.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(this.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(this.LeftBottom, matrix);

            return TransformerGeometry.CreateEllipse(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);
        }

    }
}