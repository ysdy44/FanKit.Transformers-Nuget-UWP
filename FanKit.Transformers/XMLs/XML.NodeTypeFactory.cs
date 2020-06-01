namespace FanKit.Transformers
{
    /// <summary>
    /// Provide constant and static methods for XElement.
    /// </summary>
    public static partial class XML
    {

        /// <summary>
        /// Create a node type from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The created Visibility. </returns>
        public static NodeType CreateNodeType(string type)
        {
            switch (type)
            {
                case "BeginFigure": return NodeType.BeginFigure;
                case "EndFigure": return NodeType.EndFigure;
                default: return NodeType.Node;
            }
        }

    }
}