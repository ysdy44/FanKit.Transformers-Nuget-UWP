namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a GeometryArrowTailType from the string.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created type. </returns>
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