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
        
        /// <summary>
        /// Remove all checked nodes.
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static NodeRemoveMode RemoveCheckedNodes(NodeCollection nodeCollection)
        {
            {
                int checkCount = nodeCollection._nodes.Count(node => node.IsChecked);
                if (checkCount == 0) return NodeRemoveMode.None;
            }

            List<Node> unCheckedNodes = new List<Node>();
            foreach (Node node in nodeCollection)
            {
                if (node.IsChecked == false)
                {
                    unCheckedNodes.Add(node);
                }
            }

            // Count all un checked nodes.
            {
                int count = unCheckedNodes.Count();
                if (count <= 2) return NodeRemoveMode.RemoveCurve
;
            }

            nodeCollection._nodes = unCheckedNodes;
            return NodeRemoveMode.RemovedNodes;
        }

        /// <summary>
        /// Insert a new point between checked points
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static void InterpolationCheckedNodes(NodeCollection nodeCollection)
        {
            bool isChecked = false;

            List<int> list = new List<int>();

            for (int i = 0; i < nodeCollection.Count; i++)
            {
                if (isChecked)
                {
                    list.Add(i);
                }

                isChecked = nodeCollection[i].IsChecked;
            }

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
        }

        /// <summary>
        /// Sharpen all checked nodes.
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static void SharpCheckedNodes(NodeCollection nodeCollection)
        {
            foreach (Node node in nodeCollection)
            {
                node.Sharp();
            }
        }

        /// <summary>
        /// Smoothly all checked nodes.
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static void SmoothCheckedNodes(NodeCollection nodeCollection)
        {
            //First     
            {
                int first = 0;
                int next = 1;
                Node firstNode = nodeCollection[first];
                Node nextNode = nodeCollection[next];

                if (firstNode.IsChecked)
                {
                    Vector2 space = (nextNode.Point - firstNode.Point) / 3;
                    firstNode.Smooth(space);
                }
            }

            //Last
            {
                int last = nodeCollection.Count - 1;
                int previous = nodeCollection.Count - 2;
                Node lastNode = nodeCollection[last];
                Node previousNode = nodeCollection[previous];

                if (lastNode.IsChecked)
                {
                    Vector2 space = (lastNode.Point - previousNode.Point) / 3;
                    lastNode.Smooth(space);
                }
            }

            //Nodes
            if (nodeCollection.Count > 2)
            {
                for (int i = 1; i < nodeCollection.Count - 1; i++)
                {
                    Node node = nodeCollection[i];
                    Node nextNode = nodeCollection[i + 1];
                    Node previousNode = nodeCollection[i - 1];

                    if (node.IsChecked)
                    {
                        Vector2 space = (nextNode.Point - previousNode.Point) / 6;
                        node.Smooth(space);
                    }
                }
            }
        }

    }
}