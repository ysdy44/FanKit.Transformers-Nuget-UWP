﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of node objects.
    /// </summary>
    public sealed partial class NodeCollection : ICanvasPathReceiver, ICacheTransform, IList<Node>, IEnumerable<Node>
    {

        List<Node> _nodes;

        /// <summary> Gets or sets the index of the selected item. </summary>
        public int Index { get; set; } = 0;

        //@Constructs
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        public NodeCollection() => this._nodes = new List<Node>();
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        public NodeCollection(IEnumerable<Node> nodes) => this._nodes = nodes.ToList();
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        /// <param name="left"> The frist point. </param>
        /// <param name="right"> The second point. </param>
        public NodeCollection(Vector2 left, Vector2 right) => this._nodes = new List<Node>
        {
             new Node
             {
                 Type = NodeType.BeginFigure,
                 Point = left,
                 LeftControlPoint = left,
                 RightControlPoint = left,

                 IsChecked = false,
                 IsSmooth = false,
             },
             new Node
             {
                 Type = NodeType.Node,
                 Point = right,
                 LeftControlPoint = right,
                 RightControlPoint = right,

                 IsChecked = false,
                 IsSmooth = false,
             },
             new Node
             {
                 Type = NodeType.EndFigure,
                 Point = left,
                 LeftControlPoint = left,
                 RightControlPoint = left,

                 IsChecked = false,
                 IsSmooth = false,
             }
        };
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        /// <param name="geometry"> The geometry. </param>
        public NodeCollection(CanvasGeometry geometry)
        {
            this._nodes = new List<Node>();
            geometry.SendPathTo(this);
        }

        /// <summary> Gets the selected item. </summary>
        public Node SelectedItem => this[this.Index];
        /// <summary> Gets the selected items. </summary>
        public IEnumerable<Node> SelectedItems => from node in this where node.IsChecked select node;

        
        /// <summary>
        /// Creates a new geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The created geometry. </returns>
        public CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            //Counterclockwise
            CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
            pathBuilder.SetFilledRegionDetermination(this.FilledRegionDetermination);
            pathBuilder.SetSegmentOptions(this.FigureSegmentOptions);

            Node preview = null; //this.Last(node => node.Type == NodeType.Node);
            bool isBegin = false;
            for (int i = 0; i < this.Count; i++)
            {
                Node current = this[i];

                switch (current.Type)
                {
                    case NodeType.BeginFigure:
                        pathBuilder.BeginFigure(current.Point, current.FigureFill);
                        isBegin = true;
                        break;
                    case NodeType.Node:
                        if (isBegin)
                        {
                            if (preview == null)
                            {
                                pathBuilder.AddLine(current.Point);
                            }
                            else
                            {
                                if (current.IsSmooth && preview.IsSmooth)
                                    pathBuilder.AddCubicBezier(preview.RightControlPoint, current.LeftControlPoint, current.Point);
                                else if (current.IsSmooth && preview.IsSmooth == false)
                                    pathBuilder.AddCubicBezier(preview.Point, current.LeftControlPoint, current.Point);
                                else if (current.IsSmooth == false && preview.IsSmooth)
                                    pathBuilder.AddCubicBezier(preview.RightControlPoint, current.Point, current.Point);
                                else
                                    pathBuilder.AddLine(current.Point);
                            }
                        }
                        break;
                    case NodeType.EndFigure:
                        pathBuilder.EndFigure(current.FigureLoop);
                        isBegin = false;
                        break;
                }
                               
                preview = current;
            }

            return CanvasGeometry.CreatePath(pathBuilder);
        }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned NodeCollection. </returns>
        public NodeCollection Clone() => new NodeCollection(from node in this._nodes select node.Clone());

    }
}