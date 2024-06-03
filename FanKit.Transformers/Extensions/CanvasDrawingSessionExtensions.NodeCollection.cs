using Microsoft.Graphics.Canvas;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class CanvasDrawingSessionExtensions
    {

        /// <summary>
        /// Draw bezier-curve by <see cref="NodeCollection"/>.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="nodeCollection"> The <see cref="NodeCollection"/>. </param>
        public static void DrawNodeCollection(this CanvasDrawingSession drawingSession, NodeCollection nodeCollection)
        {
            foreach (Node node in nodeCollection)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                        {
                            Vector2 vector = node.Point;

                            if (node.IsChecked == false)
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode3(vector, Windows.UI.Colors.Gold);
                                else drawingSession.DrawNode(vector, Windows.UI.Colors.Gold);
                            }
                            else
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode4(vector, Windows.UI.Colors.Gold);
                                else
                                {
                                    //Right
                                    Vector2 rightControlPoint = node.RightControlPoint;
                                    drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                                    drawingSession.DrawNode5(rightControlPoint, Windows.UI.Colors.Gold);

                                    //Left
                                    Vector2 leftControlPoint = node.LeftControlPoint;
                                    drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                                    drawingSession.DrawNode5(leftControlPoint, Windows.UI.Colors.Gold);

                                    drawingSession.DrawNode2(vector, Windows.UI.Colors.Gold);
                                }
                            }
                        }
                        break;
                    case NodeType.Node:
                        {
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
                                    //Right
                                    Vector2 rightControlPoint = node.RightControlPoint;
                                    drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                                    drawingSession.DrawNode5(rightControlPoint);

                                    //Left
                                    Vector2 leftControlPoint = node.LeftControlPoint;
                                    drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                                    drawingSession.DrawNode5(leftControlPoint);

                                    drawingSession.DrawNode2(vector);
                                }
                            }
                        }
                        break;
                    case NodeType.EndFigure:
                        break;
                }
            }
        }

        /// <summary>
        /// Draw bezier-curve by <see cref="NodeCollection"/>.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="nodeCollection"> The <see cref="NodeCollection"/>. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void DrawNodeCollection(this CanvasDrawingSession drawingSession, NodeCollection nodeCollection, Matrix3x2 matrix)
        {
            foreach (Node node in nodeCollection)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                        {
                            Vector2 vector = Vector2.Transform(node.Point, matrix);

                            if (node.IsChecked == false)
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode3(vector, Windows.UI.Colors.Gold);
                                else drawingSession.DrawNode(vector, Windows.UI.Colors.Gold);
                            }
                            else
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode4(vector, Windows.UI.Colors.Gold);
                                else
                                {
                                    //Right
                                    Vector2 rightControlPoint = node.RightControlPoint;
                                    drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                                    drawingSession.DrawNode5(rightControlPoint, Windows.UI.Colors.Gold);

                                    //Left
                                    Vector2 leftControlPoint = node.LeftControlPoint;
                                    drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                                    drawingSession.DrawNode5(leftControlPoint, Windows.UI.Colors.Gold);

                                    drawingSession.DrawNode2(vector, Windows.UI.Colors.Gold);
                                }
                            }
                        }
                        break;
                    case NodeType.Node:
                        {
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
                                    //Right
                                    Vector2 rightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);
                                    drawingSession.DrawLineDodgerBlue(vector, rightControlPoint);
                                    drawingSession.DrawNode5(rightControlPoint);

                                    //Left
                                    Vector2 leftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                                    drawingSession.DrawLineDodgerBlue(vector, leftControlPoint);
                                    drawingSession.DrawNode5(leftControlPoint);

                                    drawingSession.DrawNode2(vector);
                                }
                            }
                        }
                        break;
                    case NodeType.EndFigure:
                        break;
                }
            }
        }

        /// <summary>
        /// Draw bezier-curve by <see cref="NodeCollection"/>.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="nodeCollection"> The <see cref="NodeCollection"/>. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="accentColor"> The accent color. </param>
        public static void DrawNodeCollection(this CanvasDrawingSession drawingSession, NodeCollection nodeCollection, Matrix3x2 matrix, Windows.UI.Color accentColor)
        {
            foreach (Node node in nodeCollection)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                        {
                            Vector2 vector = Vector2.Transform(node.Point, matrix);

                            if (node.IsChecked == false)
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode3(vector, Windows.UI.Colors.Gold);
                                else drawingSession.DrawNode(vector, Windows.UI.Colors.Gold);
                            }
                            else
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode4(vector, Windows.UI.Colors.Gold);
                                else
                                {
                                    //Right
                                    Vector2 rightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);
                                    drawingSession.DrawLine(vector, rightControlPoint, accentColor);
                                    drawingSession.DrawNode5(rightControlPoint, Windows.UI.Colors.Gold);

                                    //Left
                                    Vector2 leftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                                    drawingSession.DrawLine(vector, leftControlPoint, accentColor);
                                    drawingSession.DrawNode5(leftControlPoint, Windows.UI.Colors.Gold);

                                    drawingSession.DrawNode2(vector, Windows.UI.Colors.Gold);
                                }
                            }
                        }
                        break;
                    case NodeType.Node:
                        {
                            Vector2 vector = Vector2.Transform(node.Point, matrix);

                            if (node.IsChecked == false)
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode3(vector, accentColor);
                                else drawingSession.DrawNode(vector, accentColor);
                            }
                            else
                            {
                                if (node.IsSmooth == false) drawingSession.DrawNode4(vector, accentColor);
                                else
                                {
                                    //Right
                                    Vector2 rightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);
                                    drawingSession.DrawLine(vector, rightControlPoint, accentColor);
                                    drawingSession.DrawNode5(rightControlPoint, accentColor);

                                    //Left
                                    Vector2 leftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                                    drawingSession.DrawLine(vector, leftControlPoint, accentColor);
                                    drawingSession.DrawNode5(leftControlPoint, accentColor);

                                    drawingSession.DrawNode2(vector, accentColor);
                                }
                            }
                        }
                        break;
                    case NodeType.EndFigure:
                        break;
                }
            }
        }

    }
}