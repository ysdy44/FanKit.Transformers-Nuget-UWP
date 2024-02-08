using System.Numerics;
using System.Xml.Linq;

namespace FanKit.Transformers
{
    partial class XML
    {

        /// <summary>
        /// Saves the entire <see cref="Vector2"/> to a XElement.
        /// </summary>
        /// <param name="elementName"> The element name. </param>
        /// <param name="vector"> The source data. </param>
        /// <returns> The saved XElement. </returns>
        public static XElement SaveVector2(string elementName, Vector2 vector)
        {
            return new XElement
            (
                elementName,
                new XAttribute("X", vector.X),
                new XAttribute("Y", vector.Y)
            );
        }

        /// <summary>
        ///  Loads a <see cref="Vector2"/> from an XElement.
        /// </summary>
        /// <param name="element"> The source XElement. </param>
        /// <returns> The loaded <see cref="Vector2"/>. </returns>
        public static Vector2 LoadVector2(XElement element)
        {
            Vector2 vector=new Vector2();

            if (element.Attribute("X") is XAttribute x) vector.X = (float)x;
            if (element.Attribute("Y") is XAttribute y) vector.Y = (float)y;

            return vector;
        }

    }
}