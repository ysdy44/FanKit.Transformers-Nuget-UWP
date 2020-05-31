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
    public sealed partial class NodeCollection : ICacheTransform, IList<Node>, IEnumerable<Node>
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
                 Point = left,
                 LeftControlPoint = left,
                 RightControlPoint = left,

                 IsChecked = false,
                 IsSmooth = false,
             },
             new Node
             {
                 Point = right,
                 LeftControlPoint = right,
                 RightControlPoint = right,

                 IsChecked = false,
                 IsSmooth = false,
             }
        };

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
            {
                CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
                pathBuilder.BeginFigure(this.Last().Point);

                for (int i = this.Count - 1; i > 0; i--)
                {
                    Node current = this[i];
                    Node preview = this[i - 1];

                    if (current.IsSmooth && preview.IsSmooth)
                        pathBuilder.AddCubicBezier(current.RightControlPoint, preview.LeftControlPoint, preview.Point);
                    else if (current.IsSmooth && preview.IsSmooth == false)
                        pathBuilder.AddCubicBezier(current.RightControlPoint, preview.Point, preview.Point);
                    else if (current.IsSmooth == false && preview.IsSmooth)
                        pathBuilder.AddCubicBezier(current.Point, preview.LeftControlPoint, preview.Point);
                    else
                        pathBuilder.AddLine(preview.Point);
                }

                pathBuilder.EndFigure(CanvasFigureLoop.Open);
                return CanvasGeometry.CreatePath(pathBuilder);
            }

            //Clockwise
            /*
            {
                CanvasPathBuilder pathBuilder = new CanvasPathBuilder(resourceCreator);
                pathBuilder.BeginFigure(this.First().Point);

                for (int i = 0; i < this.Count - 1; i++)
                {
                    Node current = this[i];
                    Node preview = this[i + 1];

                    if (current.IsSmooth && preview.IsSmooth)
                        pathBuilder.AddCubicBezier(current.LeftControlPoint, preview.RightControlPoint, preview.Point);
                    else if (current.IsSmooth && preview.IsSmooth == false)
                        pathBuilder.AddCubicBezier(current.LeftControlPoint, preview.Point, preview.Point);
                    else if (current.IsSmooth == false && preview.IsSmooth)
                        pathBuilder.AddCubicBezier(current.Point, preview.RightControlPoint, preview.Point);
                    else
                        pathBuilder.AddLine(preview.Point);
                }

                pathBuilder.EndFigure(CanvasFigureLoop.Open);
                return CanvasGeometry.CreatePath(pathBuilder);
            }
             */
        }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned NodeCollection. </returns>
        public NodeCollection Clone() => new NodeCollection(from node in this._nodes select node.Clone());

    }
}