using System.Xml.Linq;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Node"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="node"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveNode(string elementName, Node node)
        {
            if (node.IsSmooth)
            {
                return new XElement
                (
                    "Node",
                    XML.SaveVector2("Point", node.Point),
                    XML.SaveVector2("LeftControlPoint", node.LeftControlPoint),
                    XML.SaveVector2("RightControlPoint", node.RightControlPoint),
                    new XAttribute("IsSmooth", true)
               );
            }
            else
            {
                return new XElement
                (
                    "Node",
                    XML.SaveVector2("Point", node.Point),
                    new XAttribute("IsSmooth", false)
               );
            }
        }

        /// <summary>
        ///  Loads a <see cref="Node"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Node"/>. </returns>
        public static Node LoadNode(XElement element)
        {
            bool isSmooth = (bool)element.Attribute("IsSmooth");

            if (isSmooth)
            {
                return new Node
                {
                    Point = XML.LoadVector2(element.Element("Point")),
                    LeftControlPoint = XML.LoadVector2(element.Element("LeftControlPoint")),
                    RightControlPoint = XML.LoadVector2(element.Element("RightControlPoint")),
                    IsSmooth = true,
                };
            }
            else
            {
                return new Node
                {
                    Point = XML.LoadVector2(element.Element("Point")),
                    IsSmooth = false,
                };
            }           
        }

    }
}