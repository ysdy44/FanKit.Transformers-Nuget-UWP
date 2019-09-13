using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four range values (Left, Top, Right, Bottom). 
    /// </summary>
    public partial struct TransformerRect
    {

        //@Static
        /// <summary>
        ///  Creates a new rectangle geometry object with the specified points.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="leftTop"> The left-top point. </param>
        /// <param name="rightTop"> The right-top point. </param>
        /// <param name="rightBottom"> The right-bottom point. </param>
        /// <param name="leftBottom"> The left-bottom point. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom)
        {
            //Points
            Vector2[] points = new Vector2[]
            {
                leftTop,
                rightTop,
                rightBottom,
                leftBottom,
            };

            //Geometry
            return CanvasGeometry.CreatePolygon(resourceCreator, points);
        }

            /// <summary>
        ///  Creates a new rectangle geometry object with the specified transformer-rect.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, TransformerRect transformerRect)
        {
            //LTRB
            Vector2 leftTop = transformerRect.LeftTop;
            Vector2 rightTop = transformerRect.RightTop;
            Vector2 rightBottom = transformerRect.RightBottom;
            Vector2 leftBottom = transformerRect.LeftBottom;

            return TransformerRect.CreateRectangle(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);
        }

        /// <summary>
        ///  Creates a new rectangle geometry object with the specified transformer-rect.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateRectangle(ICanvasResourceCreator resourceCreator, TransformerRect transformerRect, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 leftTop = Vector2.Transform(transformerRect.LeftTop, matrix);
            Vector2 rightTop = Vector2.Transform(transformerRect.RightTop, matrix);
            Vector2 rightBottom = Vector2.Transform(transformerRect.RightBottom, matrix);
            Vector2 leftBottom = Vector2.Transform(transformerRect.LeftBottom, matrix);

            return TransformerRect.CreateRectangle(resourceCreator, leftTop, rightTop, rightBottom, leftBottom);
        }


        /// <summary>
        ///  Creates a new ellipse geometry object with the specified points.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="left"> The left point. </param>
        /// <param name="top"> The top point. </param>
        /// <param name="right"> The right point. </param>
        /// <param name="bottom"> The bottom point. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, Vector2 left, Vector2 top, Vector2 right, Vector2 bottom)
        {
            /// <summary>
            /// A Ellipse has left, top, right, bottom four nodes.
            /// 
            /// Control points on the left and right sides of the node.
            /// 
            /// The distance of the control point 
            /// is 0.552f times
            /// the length of the square edge.
            /// <summary>

            //HV
            Vector2 horizontal = (right - left) * 0.276f;// vector / 2 * 0.552f
            Vector2 vertical = (bottom - top) * 0.276f;// vector / 2 * 0.552f

            //Control
            Vector2 left1 = left - vertical;
            Vector2 left2 = left + vertical;
            Vector2 top1 = top + horizontal;
            Vector2 top2 = top - horizontal;
            Vector2 right1 = right + vertical;
            Vector2 right2 = right - vertical;
            Vector2 bottom1 = bottom - horizontal;
            Vector2 bottom2 = bottom + horizontal;

            //Path
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.BeginFigure(bottom);
            pathBuilder.AddCubicBezier(bottom1, left2, left);
            pathBuilder.AddCubicBezier(left1, top2, top);
            pathBuilder.AddCubicBezier(top1, right2, right);
            pathBuilder.AddCubicBezier(right1, bottom2, bottom);
            pathBuilder.EndFigure(CanvasFigureLoop.Closed);

            //Geometry
            return CanvasGeometry.CreatePath(pathBuilder);
        }

        /// <summary>
        ///  Creates a new ellipse geometry object with the specified transformer-rect.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, TransformerRect transformerRect)
        {
            //LTRB
            Vector2 left = new Vector2(transformerRect.Left, transformerRect.CenterY);
            Vector2 top = new Vector2(transformerRect.CenterX, transformerRect.Top);
            Vector2 right = new Vector2(transformerRect.Right, transformerRect.CenterY);
            Vector2 bottom = new Vector2(transformerRect.CenterX, transformerRect.Bottom);

            return TransformerRect.CreateEllipse(resourceCreator, left, top, right, bottom);
        }

        /// <summary>
        ///  Creates a new ellipse geometry object with the specified transformer-rect.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public static CanvasGeometry CreateEllipse(ICanvasResourceCreator resourceCreator, TransformerRect transformerRect, Matrix3x2 matrix)
        {
            //LTRB
            Vector2 left = Vector2.Transform(new Vector2(transformerRect.Left, transformerRect.CenterY), matrix);
            Vector2 top = Vector2.Transform(new Vector2(transformerRect.CenterX, transformerRect.Top), matrix);
            Vector2 right = Vector2.Transform(new Vector2(transformerRect.Right, transformerRect.CenterY), matrix);
            Vector2 bottom = Vector2.Transform(new Vector2(transformerRect.CenterX, transformerRect.Bottom), matrix);

            return TransformerRect.CreateEllipse(resourceCreator, left, top, right, bottom);
        }
        
    }
}