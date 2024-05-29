using Microsoft.Graphics.Canvas.Geometry;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FanKit.Transformers
{
    partial class NodeCollection
    {
        public Node this[int index]
        {
            get => this.Nodes[index];
            set => this.Nodes[index] = value;
        }

        public int Count => this.Nodes.Count();
        public bool IsReadOnly => false;

        public void Add(Node item) => this.Nodes.Add(item);
        public void Clear() => this.Nodes.Clear();
        public bool Contains(Node item) => this.Nodes.Contains(item);

        public void CopyTo(Node[] array, int arrayIndex) => this.Nodes.CopyTo(array, arrayIndex);
        public IEnumerator<Node> GetEnumerator() => this.Nodes.GetEnumerator();

        public int IndexOf(Node item) => this.Nodes.IndexOf(item);
        public void Insert(int index, Node item) => this.Nodes.Insert(index, item);
        public bool Remove(Node item) => this.Nodes.Remove(item);
        public void RemoveAt(int index) => this.Nodes.RemoveAt(index);

        IEnumerator IEnumerable.GetEnumerator() => this.Nodes.GetEnumerator();


        public bool PenAdd(Node item)
        {
            Node last = this.Last(n => n.Type == NodeType.EndFigure);
            this.Nodes.Remove(last);

            this.Nodes.Add(item);
            this.Nodes.Add(last);

            return true;
        }

        public bool NodesReplace(IEnumerable<Node> nodes)
        {
            if (nodes.Count() < 3) return false;

            this.Nodes.Clear();
            foreach (Node node in nodes)
            {
                this.Nodes.Add(node);
            }
            return true;
        }


    }
}