using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {
        
        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1) => ds.DrawLine(point0, point1, Windows.UI.Colors.DodgerBlue);
     
        /// <summary>
        /// Draw a line.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1, Windows.UI.Color accentColor) => ds.DrawLine(point0, point1, accentColor);
     

        /// <summary>
        /// Draw a ——.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="point0"> The frist point. </param>
        /// <param name="point1"> The second point. </param>
        public static void DrawThickLine(this CanvasDrawingSession ds, Vector2 point0, Vector2 point1)
        {
            ds.DrawLine(point0, point1, Windows.UI.Colors.Black, 2);
            ds.DrawLine(point0, point1, Windows.UI.Colors.White);
        }
        


        private static void _DrawBound(CanvasDrawingSession ds, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor)
        {
            ds.DrawLine(leftTop, rightTop, accentColor);
            ds.DrawLine(rightTop, rightBottom, accentColor);
            ds.DrawLine(rightBottom, leftBottom, accentColor);
            ds.DrawLine(leftBottom, leftTop, accentColor);
        }
     
        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer) => CanvasDrawingSessionExtensions._DrawBound(ds, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, Windows.UI.Colors.DodgerBlue);
     
        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix) => CanvasDrawingSessionExtensions._DrawBound(ds, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue);
     
        
        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer,  Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions._DrawBound(ds, transformer.LeftTop, transformer.RightTop, transformer.RightBottom, transformer.LeftBottom, accentColor);
    
        /// <summary>
        /// Draw lines on bound.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawBound(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor) => CanvasDrawingSessionExtensions._DrawBound(ds, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix),accentColor);




        private static void _DrawBoundNodes(CanvasDrawingSession ds, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, Windows.UI.Color accentColor, bool disabledRadian )
        {
            //Line
            ds.DrawThickLine(leftTop, rightTop);
            ds.DrawThickLine(rightTop, rightBottom);
            ds.DrawThickLine(rightBottom, leftBottom);
            ds.DrawThickLine(leftBottom, leftTop);

            //Center
            Vector2 centerLeft = (leftTop + leftBottom) / 2;
            Vector2 centerTop = (leftTop + rightTop) / 2;
            Vector2 centerRight = (rightTop + rightBottom) / 2;
            Vector2 centerBottom = (leftBottom + rightBottom) / 2;

            //Scale2
            ds.DrawNode2(leftTop);
            ds.DrawNode2(rightTop);
            ds.DrawNode2(rightBottom);
            ds.DrawNode2(leftBottom);

            if (disabledRadian == false)
            {
                //Outside
                Vector2 outsideLeft = Transformer.OutsideNode(centerLeft, centerRight);
                Vector2 outsideTop = Transformer.OutsideNode(centerTop, centerBottom);
                Vector2 outsideRight = Transformer.OutsideNode(centerRight, centerLeft);
                Vector2 outsideBottom = Transformer.OutsideNode(centerBottom, centerTop);

                //Radian
                ds.DrawThickLine(outsideTop, centerTop);
                ds.DrawNode(outsideTop);

                //Skew
                //ds.DrawNode2(ds, outsideTop);
                //ds.DrawNode2(ds, outsideLeft);
                ds.DrawNode2(outsideRight);
                ds.DrawNode2(outsideBottom);
            }

            //Scale1
            if (Transformer.OutNodeDistance(centerLeft, centerRight))
            {
                ds.DrawNode2(centerTop);
                ds.DrawNode2(centerBottom);
            }
            if (Transformer.OutNodeDistance(centerTop, centerBottom))
            {
                ds.DrawNode2(centerLeft);
                ds.DrawNode2(centerRight);
            }
        }
      
        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession ds, Transformer transformer,  bool disabledRadian = false) => CanvasDrawingSessionExtensions._DrawBoundNodes(ds, transformer.LeftTop,  transformer.RightTop,  transformer.RightBottom,  transformer.LeftBottom,  Windows.UI.Colors.DodgerBlue, disabledRadian);
     
        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix, bool disabledRadian = false) => CanvasDrawingSessionExtensions._DrawBoundNodes(ds, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), Windows.UI.Colors.DodgerBlue, disabledRadian);
   

        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession ds, Transformer transformer,  Windows.UI.Color accentColor, bool disabledRadian = false) => CanvasDrawingSessionExtensions._DrawBoundNodes(ds, transformer.LeftTop,  transformer.RightTop,  transformer.RightBottom,  transformer.LeftBottom,  accentColor, disabledRadian);
    
        /// <summary>
        /// Draw nodes and lines on bound，just like【由】.
        /// </summary>
        /// <param name="ds"> DrawingSession </param>
        /// <param name="transformer"> transformer </param>
        /// <param name="matrix"> matrix </param>
        /// <param name="accentColor"> The accent color. </param>
        /// <param name="disabledRadian"> Disable the rotation angle. </param>
        public static void DrawBoundNodes(this CanvasDrawingSession ds, Transformer transformer, Matrix3x2 matrix, Windows.UI.Color accentColor, bool disabledRadian = false) => CanvasDrawingSessionExtensions._DrawBoundNodes(ds, Vector2.Transform(transformer.LeftTop, matrix), Vector2.Transform(transformer.RightTop, matrix), Vector2.Transform(transformer.RightBottom, matrix), Vector2.Transform(transformer.LeftBottom, matrix), accentColor, disabledRadian);
    }
}