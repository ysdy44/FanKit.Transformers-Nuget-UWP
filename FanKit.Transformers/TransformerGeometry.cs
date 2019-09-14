using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    internal static class TransformerGeometry
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

    }
}