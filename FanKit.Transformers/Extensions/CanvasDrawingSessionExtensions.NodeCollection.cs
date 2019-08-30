using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace FanKit.Transformers.Extensions
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {

        /// <summary>
        /// Draw bezier-curve by NodeCollection.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="nodeCollection"> The NodeCollection. </param>
        public static void DrawNodeCollection(this CanvasDrawingSession drawingSession, NodeCollection nodeCollection)
        {
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                Node node = nodeCollection[i];
                Vector2 vector = node.Point;

                if (node.IsChecked == false)
                {
                    if (node.IsSmooth == false) drawingSession.DrawNode3(vector);
                    else drawingSession.DrawNode(vector);
                }
                else
                {
                    if (node.IsSmooth == false) drawingSession.DrawNode4(vector);
                    else
                    {
                        //Ignoring the right-control-point of the first point.
                        if (i != 0)
                        {
                            Vector2 rightControlPoint = node.RightControlPoint;
                            drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                            drawingSession.DrawNode5(rightControlPoint);
                        }

                        //Ignoring the left-control-point of the last point.
                        if (i != nodeCollection.Count - 1)
                        {
                            Vector2 leftControlPoint = node.LeftControlPoint;
                            drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                            drawingSession.DrawNode5(leftControlPoint);
                        }

                        drawingSession.DrawNode2(vector);
                    }
                }
            }
        }

        /// <summary>
        /// Draw bezier-curve by NodeCollection.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="nodeCollection"> The NodeCollection. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void DrawNodeCollection(this CanvasDrawingSession drawingSession, NodeCollection nodeCollection, Matrix3x2 matrix)
        {
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                Node node = nodeCollection[i];
                Vector2 vector = Vector2.Transform(node.Point, matrix);

                if (node.IsChecked == false)
                {
                    if (node.IsSmooth == false) drawingSession.DrawNode3(vector);
                    else drawingSession.DrawNode(vector);
                }
                else
                {
                    if (node.IsSmooth == false) drawingSession.DrawNode4(vector);
                    else
                    {
                        //Ignoring the right-control-point of the first point.
                        if (i != 0)
                        {
                            Vector2 rightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);
                            drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                            drawingSession.DrawNode5(rightControlPoint);
                        }

                        //Ignoring the left-control-point of the last point.
                        if (i != nodeCollection.Count - 1)
                        {
                            Vector2 leftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                            drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                            drawingSession.DrawNode5(leftControlPoint);
                        }

                        drawingSession.DrawNode2(vector);
                    }
                }
            }
        }

    }
}