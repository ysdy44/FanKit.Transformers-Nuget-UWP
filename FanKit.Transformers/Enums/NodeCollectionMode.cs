namespace FanKit.Transformers
{
    /// <summary>
    /// State of <see cref="NodeCollection"/>. 
    /// </summary>
    public enum NodeCollectionMode
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> Preview a line before creating a layer. </summary>
        Preview,
        /// <summary> Add a node to nodes in curve layer. </summary>
        Add,
        /// <summary> Move multiple nodes in curve layer. </summary>
        Move,

        /// <summary> Move a node's point. </summary>
        MoveSingleNodePoint,
        /// <summary> Move a node's left-control-point. </summary>
        MoveSingleNodeLeftControlPoint,
        /// <summary> Move a node's right-control-point. </summary>
        MoveSingleNodeRightControlPoint,

        /// <summary> Fill a choose rectangle. </summary>
        RectChoose,
    }
}