using System;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a border collection of unchecked nodes for temporary storage.
    /// </summary>
    public class NodeBorderCollection : IDisposable
    {
        readonly IList<NodeBorder> NodeBorders = new List<NodeBorder>();

        //@Constructs
        /// <summary>
        /// Initialize a NodeBorderCollection.
        /// </summary>
        /// <param name="nodeCollection"> The <see cref="NodeCollection"/>. </param>
        public NodeBorderCollection(NodeCollection nodeCollection)
        {
            foreach (Node node in nodeCollection)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                        this.NodeBorders.Add(new NodeBorder
                        {
                            BeginFigure = node.Clone()
                        });
                        break;
                    case NodeType.Node:
                        {
                            NodeBorder last = this.NodeBorders.Last();
                            last.Nodes.Add(node.Clone());
                        }
                        break;
                    case NodeType.EndFigure:
                        {
                            NodeBorder last = this.NodeBorders.Last();
                            last.EndFigure = node.Clone();
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Gets the remove mode.
        /// </summary>
        /// <returns> The product mode. </returns>
        public NodeRemoveMode GetRemoveMode()
        {
            IEnumerable<NodeRemoveMode> modes = from n
                                                in this.NodeBorders
                                                select n.GetRemoveMode();

            if (modes.All(m => m == NodeRemoveMode.None))
            {
                return NodeRemoveMode.None;
            }

            if (modes.All(m => m == NodeRemoveMode.RemoveCurve))
            {
                return NodeRemoveMode.RemoveCurve;
            }

            return NodeRemoveMode.RemovedNodes;
        }


        /// <summary>
        /// Gets all unchecked nodes.
        /// </summary>
        /// <returns> The product nodes. </returns>
        public IEnumerable<Node> GetUnCheckedNodes()
        {
            foreach (NodeBorder border in this.NodeBorders)
            {
                if (border.Mode == NodeRemoveMode.RemovedNodes)
                {

                    foreach (Node node in border.GetUnCheckedNodes())
                    {
                        if (node.IsChecked == false)
                        {
                            yield return node;
                        }
                    }

                }
            }
        }


        /// <summary>
        /// Execute and release or reset unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.NodeBorders.Clear();
        }
    }
}