using System.Xml.Linq;

namespace FanKit.Transformers
{
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
            Transformer transformer = new Transformer();

            if (element.Element("LeftTop") is XElement leftTop) transformer.LeftTop = XML.LoadVector2(leftTop);
            if (element.Element("RightTop") is XElement rightTop) transformer.RightTop = XML.LoadVector2(rightTop);
            if (element.Element("RightBottom") is XElement rightBottom) transformer.RightBottom = XML.LoadVector2(rightBottom);
            if (element.Element("LeftBottom") is XElement leftBottom) transformer.LeftBottom = XML.LoadVector2(leftBottom);

            return transformer;
        }

    }
}