using System;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a border of unchecked nodes for temporary storage.
    /// </summary>
    public class NodeBorder : IDisposable
    {
        internal Node BeginFigure;
        internal IList<Node> Nodes = new List<Node>();
        internal Node EndFigure;


        /// <summary>
        /// Remove mode.
        /// </summary>
        public NodeRemoveMode Mode { get; private set; }
        /// <summary>
        /// Gets the remove mode, sets the <see cref="NodeBorder.Mode"/>.
        /// </summary>
        /// <returns> The product mode. </returns>
        public NodeRemoveMode GetRemoveMode()
        {
            NodeRemoveMode mode = this.GetRemoveModeCore();
            this.Mode = mode;
            return mode;
        }
        private NodeRemoveMode GetRemoveModeCore()
        {
            if (this.BeginFigure.IsChecked)
            {
                int count = this.Nodes.Count(n => n.IsChecked == false);
                if (count < 2) return NodeRemoveMode.RemoveCurve;
            }
            else
            {
                int count = this.Nodes.Count(n => n.IsChecked == false);
                if (count == this.Nodes.Count) return NodeRemoveMode.None;
                if (count < 1) return NodeRemoveMode.RemoveCurve;
            }

            return NodeRemoveMode.RemovedNodes;
        }


        /// <summary>
        /// Gets all unchecked nodes.
        /// </summary>
        /// <returns> The product nodes. </returns>
        public IEnumerable<Node> GetUnCheckedNodes()
        {
            bool isBeginFigureChecked = this.BeginFigure.IsChecked;

            if (isBeginFigureChecked == false)
            {
                yield return this.BeginFigure.Clone();
            }

            foreach (Node node in Nodes)
            {
                if (node.IsChecked == false)
                {
                    if (isBeginFigureChecked)
                    {
                        isBeginFigureChecked = false;

                        Node begin = node.Clone();
                        begin.Type = NodeType.BeginFigure;
                        yield return begin;
                    }
                    else
                    {
                        yield return node.Clone();
                    }
                }
            }

            yield return this.EndFigure.Clone();
        }


        /// <summary>
        /// Execute and release or reset unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.BeginFigure = null;
            this.Nodes.Clear();
            this.EndFigure = null;
        }
    }
}