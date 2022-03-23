using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformers
{
    public sealed partial class NodeCollection : ICanvasPathReceiver, ICacheTransform, IList<Node>, IEnumerable<Node>
    {
        
        /// <summary>
        /// Insert a new point between checked points
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static bool InterpolationCheckedNodes(NodeCollection nodeCollection)
        {
            bool hasIsSelectd = false;

            IList<int> list = new List<int>();

            // Find adjacent two selected nodes.
            for (int i = 0; i < nodeCollection.Count-1; i++)
            {
                Node node = nodeCollection[i];
                if (node.IsChecked)
                {
                    // Add
                    if (hasIsSelectd)
                    {
                        hasIsSelectd = false;
                        list.Add(i);
                    }

                    // hasIsSelectd
                    Node next = nodeCollection[i + 1];
                    if (next.Type != NodeType.EndFigure)
                    {
                        hasIsSelectd = true;
                    }
                }
            }

            if (list.Count == 0) return false;
            
            // Interpolation
            for (int i = 0; i < list.Count; i++)
            {
                int index = list[i] + i;               
                Vector2 point = (nodeCollection[index - 1].Point + nodeCollection[index].Point) / 2;

                Node node = new Node
                {
                    Point = point,
                    LeftControlPoint = point,
                    RightControlPoint = point,
                    IsChecked = true,
                    IsSmooth = false,
                };
                nodeCollection.Insert(index, node);
            }
            return true;
        }

        /// <summary>
        /// Sharpen all checked nodes.
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static bool SharpCheckedNodes(NodeCollection nodeCollection)
        {
            bool hasIsSelectd = false;

            foreach (Node node in nodeCollection)
            {
                if (node.IsChecked)
                {
                    node.Sharp();
                    hasIsSelectd = true;
                }
            }

            return hasIsSelectd;
        }

        /// <summary>
        /// Smoothly all checked nodes.
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static bool SmoothCheckedNodes(NodeCollection nodeCollection)
        {
            if (nodeCollection.Count < 3) return false;
            bool hasIsSelectd = false;


            // First node
            Node first = nodeCollection.First();
            if (first.IsChecked)
            {
                Node second = nodeCollection[1];

                Vector2 space = (second.Point - first.Point) / 3;
                first.Smooth(space);
            }


            // Nodes
            for (int i = 1; i < nodeCollection.Count - 1; i++)
            {
                Node node = nodeCollection[i];

                if (node.IsChecked)
                {
                    Node next = nodeCollection[i + 1];
                    Node previous = nodeCollection[i - 1];

                    switch (next.Type)
                    {
                        case NodeType.EndFigure:
                            {
                                Vector2 space = (previous.Point - node.Point) / 3;
                                node.Smooth(space);
                                hasIsSelectd = true;
                            }
                            break;

                        default:
                            {
                                Vector2 space = (next.Point - previous.Point) / 6;
                                node.Smooth(space);
                                hasIsSelectd = true;
                            }
                            break;
                    }
                }
            }

            return hasIsSelectd;
        }

    }
}