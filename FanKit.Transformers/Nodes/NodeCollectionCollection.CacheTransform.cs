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
        ///  Cache the NodeCollection's transformer.
        /// </summary>
        public void CacheTransform()
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.CacheTransform();
            }
        }
        /// <summary>
        ///  Cache the NodeCollection's transformer.
        /// </summary>
        public void CacheTransformOnlySelected()
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.CacheTransformOnlySelected();
            }
        }


        /// <summary>
        ///  Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector)
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.TransformAdd(vector);
            }
        }
        /// <summary>
        ///  Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAddOnlySelected(Vector2 vector)
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.TransformAddOnlySelected(vector);
            }
        }


        /// <summary>
        ///  Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.TransformMultiplies(matrix);
            }
        }
        /// <summary>
        ///  Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>  
        public void TransformMultipliesOnlySelected(Matrix3x2 matrix)
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.TransformMultipliesOnlySelected(matrix);
            }
        }

    }
}