using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial class Node : ICacheTransform
    {

        /// <summary>
        /// Gets the all points by the node contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="node"> The source node. </param>
        /// <returns> The node-mode. </returns>
        public static NodeMode ContainsNodeMode(Vector2 point, Node node)
        {
            switch (node.Type)
            {
                case NodeType.BeginFigure:
                case NodeType.Node:
                    {
                        if (node.IsChecked)
                        {
                            //When you click on a checked node point ...
                            Vector2 point2 = node.Point;
                            if (Math.InNodeRadius(point, point2))
                            {
                                return NodeMode.PointWithChecked;
                            }

                            if (node.IsSmooth)
                            {
                                Vector2 leftControlPoint = node.LeftControlPoint;
                                Vector2 rightControlPoint = node.RightControlPoint;

                                if (Math.InNodeRadius(point, leftControlPoint))
                                {
                                    return NodeMode.LeftControlPoint;
                                }
                                else if (Math.InNodeRadius(point, rightControlPoint))
                                {
                                    return NodeMode.RightControlPoint;
                                }
                            }
                        }
                        else
                        {
                            //When you click on a unchecked node point ...      
                            Vector2 point2 = node.Point;
                            if (Math.InNodeRadius(point, point2))
                            {
                                return NodeMode.PointWithoutChecked;
                            }
                        }
                    }
                    break;

                default:
                    return NodeMode.None;
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
            switch (node.Type)
            {
                case NodeType.BeginFigure:
                case NodeType.Node:
                    {
                        if (node.IsChecked)
                        {
                            //When you click on a checked node point ...
                            Vector2 point2 = Vector2.Transform(node.Point, matrix);
                            if (Math.InNodeRadius(point, point2))
                            {
                                return NodeMode.PointWithChecked;
                            }

                            if (node.IsSmooth)
                            {
                                Vector2 leftControlPoint = Vector2.Transform(node.LeftControlPoint, matrix);
                                Vector2 rightControlPoint = Vector2.Transform(node.RightControlPoint, matrix);

                                if (Math.InNodeRadius(point, leftControlPoint))
                                {
                                    return NodeMode.LeftControlPoint;
                                }
                                else if (Math.InNodeRadius(point, rightControlPoint))
                                {
                                    return NodeMode.RightControlPoint;
                                }
                            }
                        }
                        else
                        {
                            //When you click on a unchecked node point ...      
                            Vector2 point2 = Vector2.Transform(node.Point, matrix);
                            if (Math.InNodeRadius(point, point2))
                            {
                                return NodeMode.PointWithoutChecked;
                            }
                        }
                    }
                    break;

                default:
                    return NodeMode.None;
            }

            return NodeMode.None;
        }

    }
}