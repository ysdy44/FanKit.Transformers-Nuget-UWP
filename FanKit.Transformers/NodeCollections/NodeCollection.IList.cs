using Microsoft.Graphics.Canvas.Geometry;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformers
{
    public sealed partial class NodeCollection : ICanvasPathReceiver, ICacheTransform, IList<Node>, IEnumerable<Node>
    {
        public Node this[int index]
        {
            get => this._nodes[index];
            set => this._nodes[index] = value;
        }

        public int Count => this._nodes.Count();
        public bool IsReadOnly => false;

        public void Add(Node item) => this._nodes.Add(item);
        public void Clear() => this._nodes.Clear();
        public bool Contains(Node item) => this._nodes.Contains(item);

        public void CopyTo(Node[] array, int arrayIndex) => this._nodes.CopyTo(array, arrayIndex);
        public IEnumerator<Node> GetEnumerator() => this._nodes.GetEnumerator();

        public int IndexOf(Node item) => this._nodes.IndexOf(item);
        public void Insert(int index, Node item) => this._nodes.Insert(index, item);
        public bool Remove(Node item) => this._nodes.Remove(item);
        public void RemoveAt(int index) => this._nodes.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => this._nodes.GetEnumerator();


        public bool PenAdd(Node item)
        {
            Node last = this.Last(n => n.Type == NodeType.EndFigure);
            this._nodes.Remove(last);

            this._nodes.Add(item);
            this._nodes.Add(last);

            return true;
        }

        public bool NodesReplace(IEnumerable<Node> nodes)
        {
            if (nodes.Count() < 3) return false;

            this._nodes.Clear();
            foreach (Node node in nodes)
            {
                this._nodes.Add(node);
            }
            return true;
        }
        

    }
}