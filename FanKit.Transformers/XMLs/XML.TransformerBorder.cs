using System.Xml.Linq;

namespace FanKit.Transformers
{
    partial class XML
    {

        public static XElement SaveTransformerBorder(string elementName, TransformerBorder border)
        {
            return new XElement
            (
                elementName,
                new XAttribute("Left", border.Left),
                new XAttribute("Top", border.Top),
                new XAttribute("Right", border.Right),
                new XAttribute("Bottom", border.Bottom)
            );
        }

        public static TransformerBorder LoadTransformerBorder(XElement element)
        {
            TransformerBorder result = new TransformerBorder();
            if (element.Attribute("Left") is XAttribute left) result.Left = (float)left;
            if (element.Attribute("Top") is XAttribute top) result.Top = (float)top;
            if (element.Attribute("Right") is XAttribute right) result.Right = (float)right;
            if (element.Attribute("Bottom") is XAttribute bottom) result.Bottom = (float)bottom;
            return result;
        }

    }
}