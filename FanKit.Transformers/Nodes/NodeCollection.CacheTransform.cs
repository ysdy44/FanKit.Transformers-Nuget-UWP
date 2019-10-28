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
        ///  Cache the NodeCollection's transformer.
        /// </summary>
        public void CacheTransform() => this._cacheTransform(false);
        /// <summary>
        ///  Cache the NodeCollection's transformer.
        /// </summary>
        /// <param name="isOnlySelected"> Whether only the selected nodes. </param>
        public void CacheTransform(bool isOnlySelected) => this._cacheTransform(isOnlySelected);
        private void _cacheTransform(bool isOnlySelected = false)
        {
            if (isOnlySelected == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Node node = this[i];
                    node.CacheTransform();
                    this[i] = node;
                }
            }
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Node node = this[i];
                    if (node.IsChecked)
                    {
                        node.CacheTransform();
                        this[i] = node;
                    }
                }
            }
        }


        /// <summary>
        ///  Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector) => this._transformAdd(vector, false);
        /// <summary>
        ///  Transforms the node by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        /// <param name="isOnlySelected"> Whether only the selected nodes. </param>
        public void TransformAdd(Vector2 vector, bool isOnlySelected) => this._transformAdd(vector, isOnlySelected);
        private void _transformAdd(Vector2 vector, bool isOnlySelected = false)
        {
            if (isOnlySelected == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Node node = this[i];
                    node.TransformAdd(vector);
                    this[i] = node;
                }
            }
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Node node = this[i];
                    if (node.IsChecked)
                    {
                        node.TransformAdd(vector);
                        this[i] = node;
                    }
                }
            }
        }


        /// <summary>
        ///  Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix) => this._transformMultiplies(matrix, false);
        /// <summary>
        ///  Transforms the node by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>  
        /// <param name="isOnlySelected"> Whether only the selected nodes. </param>
        public void TransformMultiplies(Matrix3x2 matrix, bool isOnlySelected) => this._transformMultiplies(matrix, isOnlySelected);
        private void _transformMultiplies(Matrix3x2 matrix, bool isOnlySelected)
        {
            if (isOnlySelected == false)
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Node node = this[i];
                    node.TransformMultiplies(matrix);
                    this[i] = node;
                }
            }
            else
            {
                for (int i = 0; i < this.Count; i++)
                {
                    Node node = this[i];
                    if (node.IsChecked)
                    {
                        node.TransformMultiplies(matrix);
                        this[i] = node;
                    }
                }
            }
        }

    }
}