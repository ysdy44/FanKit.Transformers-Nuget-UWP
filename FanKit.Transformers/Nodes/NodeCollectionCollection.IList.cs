using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of node collection.
    /// </summary>
    public sealed partial class NodeCollectionCollection : ICacheTransform, IList<NodeCollection>, IEnumerable<NodeCollection>
    {
        public NodeCollection this[int index]
        {
            get => this._nodes[index];
            set => this._nodes[index] = value;
        }

        public int Count => this._nodes.Count();
        public bool IsReadOnly => false;

        public void Add(NodeCollection item) => this._nodes.Add(item);
        public void Clear() => this._nodes.Clear();
        public bool Contains(NodeCollection item) => this._nodes.Contains(item);

        public void CopyTo(NodeCollection[] array, int arrayIndex) => this._nodes.CopyTo(array, arrayIndex);
        public IEnumerator<NodeCollection> GetEnumerator() => this._nodes.GetEnumerator();

        public int IndexOf(NodeCollection item) => this._nodes.IndexOf(item);
        public void Insert(int index, NodeCollection item) => this._nodes.Insert(index, item);
        public bool Remove(NodeCollection item) => this._nodes.Remove(item);
        public void RemoveAt(int index) => this._nodes.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => this._nodes.GetEnumerator();
    }
}