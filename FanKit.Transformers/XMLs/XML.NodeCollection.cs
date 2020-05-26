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
        /// Saves the entire <see cref="NodeCollection"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="childrenElementName"> The children element name. </param>
        /// <param name="nodes"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveNodeCollection(string elementName, string childrenElementName, NodeCollection nodes)
        {
            return new XElement
            (
                elementName,
                from node
                in nodes
                select FanKit.Transformers.XML.SaveNode(childrenElementName, node)
            );
        }

        /// <summary>
        ///  Loads a <see cref="NodeCollection"/> from an XElement.
        /// </summary>
        /// <param name="childrenElementName"> The children element name. </param>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="NodeCollection"/>. </returns>
        public static NodeCollection LoadNodeCollection(string childrenElementName, XElement element)
        {
            if (element.Elements(childrenElementName) is IEnumerable<XElement> children)
            {
                return new NodeCollection
                (
                    from node
                    in children
                    select XML.LoadNode(node)
                );
            }
            else
            {
                return new NodeCollection();
            }
        }

    }
}