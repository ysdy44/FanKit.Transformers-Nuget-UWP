using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of node objects.
    /// </summary>
    public sealed partial class NodeCollection : ICanvasPathReceiver, ICacheTransform, IList<Node>, IEnumerable<Node>
    {
        
        //public StringBuilder StringBuilder = new StringBuilder();

        bool IsBeginFigure = true;
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

            this.IsBeginFigure = true;
            this.IsLastRightControlPoint = true;
            //this.StringBuilder.AppendLine($"BeginFigure: startPoint{startPoint}");
        }


        public void AddArc(Vector2 endPoint, float radiusX, float radiusY, float rotationAngle, CanvasSweepDirection sweepDirection, CanvasArcSize arcSize) { }


        Vector2 LastRightControlPoint = new Vector2();
        bool IsLastRightControlPoint = true;
        public void AddCubicBezier(Vector2 controlPoint1, Vector2 controlPoint2, Vector2 endPoint)
        {
            Vector2 point = endPoint;
            Vector2 leftControlPoint = controlPoint2;
            Vector2 rightControlPoint = controlPoint1;

            if (this.IsBeginFigure)
            {
                this.IsBeginFigure = false;

                Vector2 startPoint = endPoint;
                Node begin = this.Last(n => n.Type == NodeType.BeginFigure);
                if (begin.Point.X == 0 && begin.Point.Y == 0) begin.Point = startPoint;
            }

            if (this.IsLastRightControlPoint)
            {
                this.IsLastRightControlPoint = false;
                this.LastRightControlPoint = rightControlPoint;
            }

            Node last = this.LastOrDefault();
            if (last != null) if (last.IsSmooth) if (last.Type == NodeType.Node) last.RightControlPoint = rightControlPoint;

            this.Add(new Node
            {
                Point = point,
                LeftControlPoint = leftControlPoint,
                RightControlPoint = point,

                IsChecked = true,
                IsSmooth = true,
            });

            //this.StringBuilder.AppendLine($"AddCubicBezier: controlPoint1{controlPoint1} controlPoint2{controlPoint2} endPoint{endPoint}");
        }


        public void AddLine(Vector2 endPoint)
        {
            Vector2 point = endPoint;

            if (this.IsBeginFigure)
            {
                this.IsBeginFigure = false;

                Vector2 startPoint = endPoint;
                Node begin = this.Last(n => n.Type == NodeType.BeginFigure);
                if (begin.Point.X == 0 && begin.Point.Y == 0) begin.Point = startPoint;
            }

            this.Add(new Node
            {
                Point = point,
                LeftControlPoint = point,
                RightControlPoint = point,

                IsChecked = true,
                IsSmooth = false,
            });

            //this.StringBuilder.AppendLine($"AddLine: endPoint{endPoint}");
        }


        public void AddQuadraticBezier(Vector2 controlPoint, Vector2 endPoint)
        {
            //this.StringBuilder.AppendLine($"AddQuadraticBezier: startPoint{controlPoint} startPoint{endPoint}");
        }


        public CanvasFilledRegionDetermination FilledRegionDetermination;
        public void SetFilledRegionDetermination(CanvasFilledRegionDetermination filledRegionDetermination)
        {
            this.FilledRegionDetermination = filledRegionDetermination;
            //this.StringBuilder.AppendLine($"SetFilledRegionDetermination: filledRegionDetermination{filledRegionDetermination}");
        }


        public CanvasFigureSegmentOptions FigureSegmentOptions;
        public void SetSegmentOptions(CanvasFigureSegmentOptions figureSegmentOptions)
        {
            this.FigureSegmentOptions = figureSegmentOptions;
            //this.StringBuilder.AppendLine($"SetSegmentOptions: figureSegmentOptions{figureSegmentOptions}");
        }


        public void EndFigure(CanvasFigureLoop figureLoop)
        {
            if (this.IsLastRightControlPoint == false)
            {
                Node last = this.LastOrDefault();
                if (last != null) if (last.IsSmooth) if (last.Type == NodeType.Node) last.RightControlPoint = this.LastRightControlPoint;
            }

            this.Add(new Node
            {
                Type = NodeType.EndFigure,
                FigureLoop= figureLoop,
            });

            //this.StringBuilder.AppendLine($"EndFigure: figureLoop{figureLoop}");
        }

    }
}