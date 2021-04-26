using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    public sealed partial class NodeCollection : ICanvasPathReceiver, ICacheTransform, IList<Node>, IEnumerable<Node>
    {

        /// <summary>
        /// Arrange all node righyt control point.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        private static void ArrangeNodes(IList<Node> nodes)
        {
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                Node node = nodes[i];
                Node next = nodes[i + 1];

                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                    case NodeType.Node:
                        {
                            if (next.Type == NodeType.EndFigure)
                            {
                                node.RightControlPoint = node.Point;
                            }
                            else
                            {
                                if (next.IsSmooth)
                                    node.RightControlPoint = next.RightControlPoint;
                                else
                                    node.RightControlPoint = next.Point;
                            }
                        }
                        break;
                    case NodeType.EndFigure:
                        break;
                    default:
                        break;
                }
            }
        }



        /// <summary>
        /// Specifies the method used to determine which points are inside the geometry described
        /// by this path builder, and which points are outside.
        /// </summary>
        /// <param name="filledRegionDetermination"> The determination for filled region.</param>
        public void SetFilledRegionDetermination(CanvasFilledRegionDetermination filledRegionDetermination)
        {
            this.FilledRegionDetermination = filledRegionDetermination;
        }
        CanvasFilledRegionDetermination FilledRegionDetermination;

        /// <summary>
        /// Specifies the method used to determine which points are inside the geometry described
        /// by this path builder, and which points are outside.
        /// </summary>
        /// <param name="figureSegmentOptions"> The options for figure segment.</param>
        public void SetSegmentOptions(CanvasFigureSegmentOptions figureSegmentOptions)
        {
            this.FigureSegmentOptions = figureSegmentOptions;
        }
        CanvasFigureSegmentOptions FigureSegmentOptions;



        /// <summary>
        ///  Starts a new figure at the specified point, with the specified figure fill option.
        /// </summary>
        /// <param name="startPoint"> The start-point. </param>
        /// <param name="figureFill"> The figure-fill. </param>
        public void BeginFigure(Vector2 startPoint, CanvasFigureFill figureFill)
        {
            this.Add(new Node
            {
                Type = NodeType.BeginFigure,
                FigureFill = figureFill,

                Point = startPoint,
                LeftControlPoint = startPoint,
                RightControlPoint = startPoint,

                IsChecked = true,
                IsSmooth = true,
            });
        }

        /// <summary>
        /// Ends the current figure; optionally, closes it.
        /// </summary>
        /// <param name="figureLoop"> The figure-loop. </param>
        public void EndFigure(CanvasFigureLoop figureLoop)
        {
            this.Add(new Node
            {
                Type = NodeType.EndFigure,
                FigureLoop = figureLoop,
            });
        }


        /// <summary>
        /// Adds a line segment to the path, with the specified end point.
        /// </summary>
        /// <param name="endPoint"> The end-point. </param>
        public void AddLine(Vector2 endPoint)
        {
            Vector2 point = endPoint;

            this.Add(new Node
            {
                Point = point,
                LeftControlPoint = point,
                RightControlPoint = point,

                IsChecked = true,
                IsSmooth = false,
            });
        }

        /// <summary>
        /// Adds a quadratic bezier to the path. The bezier starts where the path left off,
        /// and has the specified control point and end point.
        /// </summary>
        /// <param name="controlPoint"> The control-point. </param>
        /// <param name="endPoint"> The end-point. </param>
        public void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint)
        {
        }


        /// <summary>
        /// Adds a single arc to the path, specified by start and end points through which
        /// an ellipse will be fitted.
        /// </summary>
        /// <param name="endPoint"> The end-point. </param>
        /// <param name="radiusX"> The radius X. </param>
        /// <param name="radiusY"> The radius Y. </param>
        /// <param name="rotationAngle"> The angle of rotation. </param>
        /// <param name="sweepDirection"> The direction for sweep. </param>
        /// <param name="arcSize"> The size of arc. </param>
        public void AddArc(Vector2 endPoint, float radiusX, float radiusY, float rotationAngle, CanvasSweepDirection sweepDirection, CanvasArcSize arcSize) { }

        /// <summary>
        /// Adds a cubic bezier to the path. The bezier starts where the path left off, and
        /// has the specified control points and end point.
        /// </summary>
        /// <param name="controlPoint1"> The first control-point. </param>
        /// <param name="controlPoint2"> The sencond control-point. </param>
        /// <param name="endPoint"> The end-point. </param>
        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            Vector2 point = endPoint;
            Vector2 leftControlPoint = controlPoint2;
            Vector2 rightControlPoint = controlPoint1;

            this.Add(new Node
            {
                Point = point,
                LeftControlPoint = leftControlPoint,
                RightControlPoint = rightControlPoint,

                IsChecked = true,
                IsSmooth = true,
            });
        }


    }
}