namespace FanKit.Transformers
{
    partial class XML
    {

        /// <summary>
        /// Create a <see cref="NodeType"/> from an string and XElement.
        /// </summary>
        /// <param name="type"> The source string. </param>
        /// <returns> The loaded <see cref="NodeType"/>. </returns>
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