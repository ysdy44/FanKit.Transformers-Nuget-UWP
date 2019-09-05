﻿using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of UIElement objects.
    /// </summary>
    public sealed partial class NodeCollection : ICacheTransform, IList<Node>, IEnumerable<Node>
    {
        List<Node> _nodes;
        /// <summary> Gets or sets the index of the selected item. </summary>
        public int Index { get; set; } = 0;

        //@Constructs
        /// <summary>
        /// Constructs a NodeCollection.
        /// </summary>
        public NodeCollection() => this._nodes = new List<Node>();
        /// <summary>
        /// Constructs a NodeCollection.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        public NodeCollection(IEnumerable<Node> nodes) => this._nodes = nodes.ToList();
        /// <summary>
        /// Constructs a NodeCollection.
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
        /// Select only one node.
        /// </summary>
        /// <param name="selectedIndex"> The index of the selected node. </param>
        public void SelectionOnlyOne(int selectedIndex)
        {
            for (int i = 0; i < this.Count; i++)
            {
                //Check the selected node.
                if (i == selectedIndex)
                {
                    Node node = this[selectedIndex];
                    node.IsChecked = true;
                    this[selectedIndex] = node;
                }
                //Unchecked others.
                else
                {
                    Node node = this[i];
                    if (node.IsChecked)
                    {
                        node.IsChecked = false;
                        this[i] = node;
                    }
                }
            }
        }
               

        /// <summary>
        /// Check node which in the rect.
        /// </summary>
        /// <param name="left"> The destination rectangle's left. </param>
        /// <param name="top"> The destination rectangle's top. </param>
        /// <param name="right"> The destination rectangle's right. </param>
        /// <param name="bottom"> The destination rectangle's bottom. </param>
        public void RectChoose(float left, float top, float right, float bottom)
        {
            for (int i = 0; i < this.Count; i++)
            {
                Node node = this[i];

                bool isContained = node.Contained(left, top, right, bottom);
                if (node.IsChecked != isContained)
                {
                    node.IsChecked = isContained;
                    this[i] = node;
                }
            }
        }
        /// <summary>
        /// Check node which in the rect.
        /// </summary>
        /// <param name="transformerRect"> The destination rectangle. </param>
        public void RectChoose(TransformerRect transformerRect) => this.RectChoose(transformerRect.Left, transformerRect.Top, transformerRect.Right, transformerRect.Bottom);


        /// <summary>
        /// Creates a new geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The created geometry. </returns>
        public CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
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
          
    }
}