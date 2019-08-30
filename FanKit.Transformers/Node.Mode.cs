using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial struct Node : ICacheTransform
    {

        /// <summary>
        /// Gets the all points by the node contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="node"> The source node. </param>
        /// <returns> The node-mode. </returns>
        public static NodeMode ContainsNodeMode(Vector2 point, Node node)
        {
            if (node.IsChecked)
            {
                if (node.IsSmooth)
                {
                    Vector2 nodeLeftControlPoint = node.LeftControlPoint;
                    Vector2 nodeRightControlPoint = node.RightControlPoint;

                    if (Math.InNodeRadius(point, nodeLeftControlPoint))
                    {
                        return NodeMode.LeftControlPoint;
                    }
                    else if (Math.InNodeRadius(point, nodeRightControlPoint))
                    {
                        return NodeMode.RightControlPoint;
                    }
                }
                else
                {
                    //When you click on a checked node point ...
                    Vector2 nodePoint = node.Point;
                    if (Math.InNodeRadius(point, nodePoint))
                    {
                        return NodeMode.PointWithChecked;
                    }
                }
            }
            else
            {
                //When you click on a unchecked node point ...      
                Vector2 nodePoint = node.Point;
                if (Math.InNodeRadius(point, nodePoint))
                {
                    return NodeMode.PointWithoutChecked;
                }
            }

            return NodeMode.None;
        }

        /// <summary>
        /// Gets the all points by the node contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="node"> The source node. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The node-mode. </returns>
        public static NodeMode ContainsNodeMode(Vector2 point, Node node, Matrix3x2 matrix)
        {
            if (node.IsChecked)
            {
                if (node.IsSmooth)
                {
                    Vector2 nodeLeftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                    Vector2 nodeRightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);

                    if (Math.InNodeRadius(point, nodeLeftControlPoint))
                    {
                        return NodeMode.LeftControlPoint;
                    }
                    else if (Math.InNodeRadius(point, nodeRightControlPoint))
                    {
                        return NodeMode.RightControlPoint;
                    }
                }
                else
                {
                    //When you click on a checked node point ...
                    Vector2 nodePoint = Vector2.Transform(node.Point, matrix);
                    if (Math.InNodeRadius(point, nodePoint))
                    {
                        return NodeMode.PointWithChecked;
                    }
                }
            }
            else
            {
                //When you click on a unchecked node point ...      
                Vector2 nodePoint = Vector2.Transform(node.Point, matrix);
                if (Math.InNodeRadius(point, nodePoint))
                {
                    return NodeMode.PointWithoutChecked;
                }
            }

            return NodeMode.None;
        }

    }
}