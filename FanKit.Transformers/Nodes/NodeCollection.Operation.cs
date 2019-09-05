using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of UIElement objects.
    /// </summary>
    public sealed partial class NodeCollection : ICacheTransform, IList<Node>, IEnumerable<Node>
    {
        /// <summary>
        /// Remove all checked nodes.
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static void RemoveCheckedNodes(NodeCollection nodeCollection)
        {
            int removeIndex = -1;

            do
            {
                if (removeIndex >= 0)
                {
                    if (nodeCollection[removeIndex].IsChecked)
                    {
                        nodeCollection.RemoveAt(removeIndex);
                    }
                }

                removeIndex = -1;
                for (int i = 0; i < nodeCollection.Count; i++)
                {
                    if (nodeCollection[i].IsChecked)
                    {
                        removeIndex = i;
                    }
                }
            }
            while (removeIndex >= 0);
        }

        /// <summary>
        /// Insert a new point between checked points
        /// </summary>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        public static void Interpolation(NodeCollection nodeCollection)
        {
            bool isChecked = false;

            List<int> list = new List<int>();

            for (int i = 0; i < nodeCollection.Count; i++)
            {
                if ( isChecked )
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
            for (int i = 0; i < nodeCollection.Count; i++)
            {
                if (nodeCollection[i].IsChecked)
                {
                    if (nodeCollection[i].IsSmooth)
                    {
                        Vector2 vector = nodeCollection[i].Point;

                        Node node = new Node
                        {
                            Point = vector,
                            LeftControlPoint = vector,
                            RightControlPoint = vector,
                            IsChecked = true,
                            IsSmooth = false,
                        };

                        nodeCollection[i] = node;
                    }
                }
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

                if (nodeCollection[first].IsChecked)
                {
                    Vector2 space = (nodeCollection[next].Point - nodeCollection[first].Point) / 3;
                    NodeCollection. _smoothNode(nodeCollection,first, space);
                }
            }

            //Last
            {
                int last = nodeCollection.Count - 1;
                int previous = nodeCollection.Count - 2;

                if (nodeCollection[last].IsChecked)
                {
                    Vector2 space = (nodeCollection[last].Point - nodeCollection[previous].Point) / 3;
                    NodeCollection._smoothNode(nodeCollection,last, space);
                }
            }

            //Nodes
            if (nodeCollection.Count > 2)
            {
                for (int i = 1; i < nodeCollection.Count - 1; i++)
                {
                    if (nodeCollection[i].IsChecked)
                    {
                        Vector2 space = (nodeCollection[i + 1].Point - nodeCollection[i - 1].Point) / 6;
                        NodeCollection._smoothNode(nodeCollection,i, space);
                    }
                }
            }
        }
        static void _smoothNode(NodeCollection nodeCollection, int index, Vector2 space)
        {
            Vector2 vector = nodeCollection[index].Point;

            Node node = new Node
            {
                Point = vector,
                LeftControlPoint = vector + space,
                RightControlPoint = vector - space,
                IsChecked = true,
                IsSmooth = true,
            };
            nodeCollection[index] = node;
        }

    }
}