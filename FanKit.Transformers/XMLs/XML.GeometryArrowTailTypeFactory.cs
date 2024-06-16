namespace FanKit.Transformers
{
    partial class XML
    {

        /// <summary>
        /// Create a <see cref="GeometryArrowTailType"/> from the string.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created <see cref="GeometryArrowTailType"/>. </returns>
        public static GeometryArrowTailType CreateGeometryArrowTailType(string type)
        {
            switch (type)
            {
                case "Arrow": return GeometryArrowTailType.Arrow;
                default: return GeometryArrowTailType.None;
            }
        }

    }
}