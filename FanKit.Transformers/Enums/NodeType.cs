namespace FanKit.Transformers
{
    /// <summary>
    /// Type of <see cref="FanKit.Transformers.Node"/>. 
    /// </summary>
    public enum NodeType
    {
        /// <summary>
        /// The beginning of a figure.
        /// </summary>
        BeginFigure,

        /// <summary>
        /// The node.
        /// </summary>
        Node,

        /// <summary>
        /// The end of a figure.
        /// </summary>
        EndFigure,
    }
}