using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="NodeCollectionCollection"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="nodess"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveNodeCollectionCollection(string elementName, NodeCollectionCollection nodess)
        {
            return new XElement
            (
                "Nodess",
                from nodes
                in nodess
                select FanKit.Transformers.XML.SaveNodeCollection("Nodes", nodes)
            );
        }

        /// <summary>
        ///  Loads a <see cref="NodeCollectionCollection"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="NodeCollectionCollection"/>. </returns>
        public static NodeCollectionCollection LoadNodeCollectionCollection(XElement element)
        {
            return new NodeCollectionCollection
            (
                from nodes
                in element.Elements("Nodes")
                select FanKit.Transformers.XML.LoadNodeCollection(nodes)
           );
        }

    }
}