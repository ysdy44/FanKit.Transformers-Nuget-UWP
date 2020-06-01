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
            switch (node.Type)
            {
                case NodeType.BeginFigure:
                    return new XElement
                    (
                        "Node",
                        new XAttribute("Type", NodeType.BeginFigure),
                        XML.SaveVector2("StartPoint", node.Point)
                   );
                case NodeType.EndFigure:
                    return new XElement
                    (
                        "Node",
                        new XAttribute("Type", NodeType.EndFigure)
                   );
                default://case NodeType.Node:
                    {
                        if (node.IsSmooth)
                        {
                            return new XElement
                            (
                                "Node",
                                new XAttribute("Type", NodeType.Node),
                                new XAttribute("IsSmooth", true),
                                XML.SaveVector2("Point", node.Point),
                                XML.SaveVector2("LeftControlPoint", node.LeftControlPoint),
                                XML.SaveVector2("RightControlPoint", node.RightControlPoint)
                           );
                        }
                        else
                        {
                            return new XElement
                            (
                                "Node",
                                new XAttribute("Type", NodeType.Node),
                                new XAttribute("IsSmooth", false),
                                XML.SaveVector2("Point", node.Point)
                           );
                        }
                    }
            }
        }

        /// <summary>
        ///  Loads a <see cref="Node"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Node"/>. </returns>
        public static Node LoadNode(XElement element)
        {
            Node node = new Node();
            
            if (element.Attribute("Type") is XAttribute type) node.Type = XML.CreateNodeType(type.Value);

            switch (node.Type)
            {
                case NodeType.BeginFigure:
                    if (element.Element("StartPoint") is XElement startPoint) node.Point = XML.LoadVector2(startPoint);
                    break;
                case NodeType.EndFigure:
                    break;
                default://case NodeType.Node:
                    {
                        if (element.Attribute("IsSmooth") is XAttribute isSmooth) node.IsSmooth = (bool)isSmooth;

                        if (element.Element("Point") is XElement point) node.Point = XML.LoadVector2(point);

                        if (node.IsSmooth)
                        {
                            if (element.Element("LeftControlPoint") is XElement left) node.LeftControlPoint = XML.LoadVector2(left);
                            if (element.Element("RightControlPoint") is XElement right) node.RightControlPoint = XML.LoadVector2(right);
                        }
                    }
                    break;
            }
            return node;
        }

    }
}