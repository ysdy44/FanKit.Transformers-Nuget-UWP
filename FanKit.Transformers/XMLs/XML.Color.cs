using System.Xml.Linq;
using Windows.UI;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Color"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="color"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveColor(string elementName, Color color)
        {
            return new XElement
            (
                elementName,
                new XAttribute("A", color.A),
                new XAttribute("R", color.R),
                new XAttribute("G", color.G),
                new XAttribute("B", color.B)
             );
        }

        /// <summary>
        ///  Loads a <see cref="Color"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Color"/>. </returns>
        public static Color LoadColor(XElement element)
        {
            Color color;

            if (element.Attribute("A") is XAttribute a) color.A = (byte)(int)a;
            if (element.Attribute("R") is XAttribute r) color.R = (byte)(int)r;
            if (element.Attribute("G") is XAttribute g) color.G = (byte)(int)g;
            if (element.Attribute("B") is XAttribute b) color.B = (byte)(int)b;

            return color;
        }

    }
}