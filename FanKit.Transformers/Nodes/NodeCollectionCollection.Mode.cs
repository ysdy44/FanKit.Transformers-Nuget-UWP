using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of node collection.
    /// </summary>
    public sealed partial class NodeCollectionCollection : ICacheTransform, IList<NodeCollection>, IEnumerable<NodeCollection>
    {

        /// <summary>
        /// Gets the all points by the NodeCollectionCollection contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="nodeCollectionCollection"> The source NodeCollectionCollection. </param>
        /// <returns> The NodeCollection mode. </returns>
        public static NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, NodeCollectionCollection nodeCollectionCollection)
        {
            for (int i = 0; i < nodeCollectionCollection.Count; i++)
            {
                NodeCollection nodes = nodeCollectionCollection[i];
                NodeCollectionMode mode = NodeCollection.ContainsNodeCollectionMode(point, nodes);

                if (mode != NodeCollectionMode.RectChoose)
                {
                    nodeCollectionCollection.Index = i;
                    return mode;
                }
            }

            return NodeCollectionMode.RectChoose;
        }

        /// <summary>
        /// Gets the all points by the NodeCollection contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="nodeCollectionCollection"> The source NodeCollectionCollection. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The NodeCollection mode. </returns>
        public static NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, NodeCollectionCollection nodeCollectionCollection, Matrix3x2 matrix)
        {
            for (int i = 0; i < nodeCollectionCollection.Count; i++)
            {
                NodeCollection nodes = nodeCollectionCollection[i];
                NodeCollectionMode mode = NodeCollection.ContainsNodeCollectionMode(point, nodes, matrix);

                if (mode != NodeCollectionMode.RectChoose)
                {
                    nodeCollectionCollection.Index = i;
                    return mode;
                }
            }

            return NodeCollectionMode.RectChoose;
        }

    }
}
