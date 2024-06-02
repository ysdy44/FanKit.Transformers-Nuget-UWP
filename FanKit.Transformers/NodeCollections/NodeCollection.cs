using Microsoft.Graphics.Canvas;
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
        readonly List<Node> Nodes;

        /// <summary> Gets or sets the index of the selected item. </summary>
        public int Index { get; set; } = 0;

        //@Constructs
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        public NodeCollection() => this.Nodes = new List<Node>();
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        public NodeCollection(IEnumerable<Node> nodes) => this.Nodes = nodes.ToList();
        /// <summary>
        /// Initialize a NodeCollection.
        /// </summary>
        /// <param name="left"> The frist point. </param>
        /// <param name="right"> The second point. </param>
        public NodeCollection(Vector2 left, Vector2 right) => this.Nodes = new List<Node>
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
            this.Nodes = new List<Node>();
            geometry.SendPathTo(this);
            NodeCollection.ArrangeNodes(this.Nodes);
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
                            Node previous = this[i - 1];

                            if (current.IsSmooth == false && previous.IsSmooth == false)
                                pathBuilder.AddLine(current.Point);
                            else
                                pathBuilder.AddCubicBezier(previous.RightControlPoint, current.LeftControlPoint, current.Point);
                        }
                        break;
                    case NodeType.EndFigure:
                        pathBuilder.EndFigure(current.FigureLoop);
                        isBegin = false;
                        break;
                }
            }

            return CanvasGeometry.CreatePath(pathBuilder);
        }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned NodeCollection. </returns>
        public NodeCollection Clone() => new NodeCollection(from node in this.Nodes select node.Clone());

        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned ndoes. </returns>
        public IEnumerable<Node> NodesClone() => from node in this.Nodes select node.Clone();
        /// <summary>
        /// Get own starting copy (ex: <see cref="Node.StartingIsChecked"/> ).
        /// </summary>
        /// <returns> The cloned ndoes. </returns>
        public IEnumerable<Node> NodesStartingClone() => from node in this.Nodes
                                                         select new Node
                                                         {
                                                             Type = node.Type,
                                                             FigureFill = node.FigureFill,
                                                             FigureLoop = node.FigureLoop,

                                                             Point = node.StartingPoint,
                                                             LeftControlPoint = node.StartingLeftControlPoint,
                                                             RightControlPoint = node.StartingRightControlPoint,

                                                             IsChecked = node.StartingIsChecked,
                                                             IsSmooth = node.StartingIsSmooth,
                                                         };

    }
}
