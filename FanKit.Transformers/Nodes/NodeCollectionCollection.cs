using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
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

        List<NodeCollection> _nodes;

        /// <summary> Gets or sets the index of the selected item. </summary>
        public int Index { get; set; } = 0;

        //@Constructs
        /// <summary>
        /// Initialize a NodeCollectionCollection.
        /// </summary>
        public NodeCollectionCollection() => this._nodes = new List<NodeCollection>();
        /// <summary>
        /// Initialize a NodeCollectionCollection.
        /// </summary>
        /// <param name="nodess"> The NodeCollections. </param>
        public NodeCollectionCollection(IEnumerable<NodeCollection> nodess) => this._nodes = nodess.ToList();
        /// <summary>
        /// Initialize a NodeCollectionCollection.
        /// </summary>      
        /// <param name="nodess"> The nodess. </param>
        public NodeCollectionCollection(IEnumerable<IEnumerable<Node>> nodess) => this._nodes = 
        (
            from nodes
            in nodess
            select new NodeCollection(nodes)
        ).ToList();

        /// <summary> Gets the selected item. </summary>
        public NodeCollection SelectedItem => this[this.Index];
        /// <summary> Gets the selected items. </summary>
        public IEnumerable<NodeCollection> SelectedItems => from node in this where node.Index != 0 select node;//TODO: bug?

        
        /// <summary>
        /// Creates a new geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The created geometry. </returns>
        public CanvasGeometry CreateGeometry(ICanvasResourceCreator resourceCreator)
        {
            IEnumerable<CanvasGeometry> geometrys =
                 from nodes
                 in this
                 select nodes.CreateGeometry(resourceCreator);

            return CanvasGeometry.CreateGroup(resourceCreator, geometrys.ToArray());
        }


        /// <summary>
        /// Get own copy.
        /// </summary>
        /// <returns> The cloned NodeCollectionCollection. </returns>
        public NodeCollectionCollection Clone() => new NodeCollectionCollection(from node in this._nodes select node);

    }
}
