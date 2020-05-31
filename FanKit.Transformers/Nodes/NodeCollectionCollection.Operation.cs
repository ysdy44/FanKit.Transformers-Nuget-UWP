using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of node collection.
    /// </summary>
    public sealed partial class NodeCollectionCollection : ICacheTransform, IList<NodeCollection>, IEnumerable<NodeCollection>
    {
        /// <summary>
        /// Remove all checked nodes.
        /// </summary>
        /// <param name="nodeCollectionCollection"> The source NodeCollectionCollection. </param>
        public static NodeRemoveMode RemoveCheckedNodes(NodeCollectionCollection nodeCollectionCollection)
        {
            int removeCount = 0;
            IList<NodeCollection> removeNodeCollection = new List<NodeCollection>();

            foreach (NodeCollection nodes in nodeCollectionCollection)
            {
                NodeRemoveMode removeMode = NodeCollection.RemoveCheckedNodes(nodes);

                switch (removeMode)
                {
                    case NodeRemoveMode.RemovedNodes:
                        removeCount++;
                        break;
                    case NodeRemoveMode.RemoveCurve:
                        removeNodeCollection.Add(nodes);
                        break;
                }
            }


            if (removeCount == 0)
                if (removeNodeCollection.Count == 0)
                    return NodeRemoveMode.None;


            if (removeNodeCollection.Count == nodeCollectionCollection.Count)
                return NodeRemoveMode.RemoveCurve;


            foreach (NodeCollection remove in removeNodeCollection)
            {
                nodeCollectionCollection.Remove(remove);
            }
            return NodeRemoveMode.RemovedNodes;
        }

        /// <summary>
        /// Insert a new point between checked points
        /// </summary>
        /// <param name="nodeCollections"> The source NodeCollectionCollection. </param>
        public static void InterpolationCheckedNodes(NodeCollectionCollection nodeCollections)
        {
            foreach (NodeCollection nodes in nodeCollections)
            {
                NodeCollection.InterpolationCheckedNodes(nodes);
            }
        }

        /// <summary>
        /// Sharpen all checked nodes.
        /// </summary>
        /// <param name="nodeCollections"> The source NodeCollectionCollection. </param>
        public static void SharpCheckedNodes(NodeCollectionCollection nodeCollections)
        {
            foreach (NodeCollection nodes in nodeCollections)
            {
                NodeCollection.SharpCheckedNodes(nodes);
            }
        }

        /// <summary>
        /// Smoothly all checked nodes.
        /// </summary>
        /// <param name="nodeCollections"> The source NodeCollectionCollection. </param>
        public static void SmoothCheckedNodes(NodeCollectionCollection nodeCollections)
        {
            foreach (NodeCollection nodes in nodeCollections)
            {
                NodeCollection.SmoothCheckedNodes(nodes);
            }
        }

    }
}