using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace FanKit.Transformers
{
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="NodeCollection"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="nodes"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveNodeCollection(string elementName, NodeCollection nodes)
        {
            return new XElement
            (
                elementName,
                from node
                in nodes
                select XML.SaveNode("Node", node)
            );
        }

        /// <summary>
        ///  Loads a <see cref="NodeCollection"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="NodeCollection"/>. </returns>
        public static NodeCollection LoadNodeCollection(XElement element)
        {
            return new NodeCollection
            (
                from node
                in element.Elements("Node")
                select XML.LoadNode(node)
            );
        }

    }
}