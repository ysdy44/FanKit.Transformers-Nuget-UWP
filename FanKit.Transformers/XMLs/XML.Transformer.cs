using System.Xml.Linq;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Transformer"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="transformer"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveTransformer(string elementName, Transformer transformer)
        {
            return new XElement
            (
                elementName,
                XML.SaveVector2("LeftTop", transformer.LeftTop),
                XML.SaveVector2("RightTop", transformer.RightTop),
                XML.SaveVector2("RightBottom", transformer.RightBottom),
                XML.SaveVector2("LeftBottom", transformer.LeftBottom)
            );
        }

        /// <summary>
        ///  Loads a <see cref="Transformer"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Transformer"/>. </returns>
        public static Transformer LoadTransformer(XElement element)
        {
            return new Transformer
            {
                LeftTop= XML.LoadVector2(element.Element("LeftTop")),
                RightTop = XML.LoadVector2(element.Element("RightTop")),
                RightBottom = XML.LoadVector2(element.Element("RightBottom")),
                LeftBottom = XML.LoadVector2(element.Element("LeftBottom")),
            };
        }

    }
}