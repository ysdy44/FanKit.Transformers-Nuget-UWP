using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    public sealed partial class NodeCollection : ICanvasPathReceiver, ICacheTransform, IList<Node>, IEnumerable<Node>
    {

        /// <summary>
        /// Cache the NodeCollection's transformer.
        /// </summary>
        public void CacheTransform()
        {
            foreach (Node node in this)
            {
                node.CacheTransform();
            }
        }
        /// <summary>
        /// Cache the NodeCollection's transformer.
        /// </summary>
        public void CacheTransformOnlySelected()
        {
            foreach (Node node in this)
            {
                if (node.IsChecked)
                {
                    node.CacheTransform();
                }
            }
        }


        /// <summary>
        /// Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector)
        {
            foreach (Node node in this)
            {
                node.TransformAdd(vector);
            }
        }
        /// <summary>
        /// Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAddOnlySelected(Vector2 vector)
        {
            foreach (Node node in this)
            {
                if (node.IsChecked)
                {
                    node.TransformAdd(vector);
                }
            }
        }


        /// <summary>
        /// Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix)
        {
            foreach (Node node in this)
            {
                node.TransformMultiplies(matrix);
            }
        }
        /// <summary>
        /// Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>  
        public void TransformMultipliesOnlySelected(Matrix3x2 matrix)
        {
            foreach (Node node in this)
            {
                if (node.IsChecked)
                {
                    node.TransformMultiplies(matrix);
                }
            }
        }

    }
}