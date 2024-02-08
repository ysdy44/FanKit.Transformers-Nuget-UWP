using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class NodeCollection
    {

        /// <summary>
        /// Gets the all points by the NodeCollection contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        /// <returns> The NodeCollection mode. </returns>
        public static NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, NodeCollection nodeCollection)
        {
            if (nodeCollection == null) throw new NullReferenceException("NodeCollection is null.");

            for (int i = 0; i < nodeCollection.Count; i++)
            {
                Node node = nodeCollection[i];
                NodeMode nodeMode = Node.ContainsNodeMode(point, node);

                switch (nodeMode)
                {
                    case NodeMode.None: continue;
                    case NodeMode.PointWithChecked:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.Move;
                        }
                    case NodeMode.PointWithoutChecked:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.MoveSingleNodePoint;
                        }
                    case NodeMode.LeftControlPoint:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.MoveSingleNodeLeftControlPoint;
                        }
                    case NodeMode.RightControlPoint:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.MoveSingleNodeRightControlPoint;
                        }
                }
            }

            return NodeCollectionMode.RectChoose;
        }

        /// <summary>
        /// Gets the all points by the NodeCollection contains the specified point. 
        /// </summary>
        /// <param name="point"> The input point. </param>
        /// <param name="nodeCollection"> The source NodeCollection. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The NodeCollection mode. </returns>
        public static NodeCollectionMode ContainsNodeCollectionMode(Vector2 point, NodeCollection nodeCollection,  Matrix3x2 matrix)
        {
            if (nodeCollection == null) throw new NullReferenceException("NodeCollection is null.");

            for (int i = 0; i < nodeCollection.Count; i++)
            {
                Node node = nodeCollection[i];
                NodeMode nodeMode = Node.ContainsNodeMode(point, node, matrix);

                switch (nodeMode)
                {
                    case NodeMode.None: break;
                    case NodeMode.PointWithChecked:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.Move;
                        }
                    case NodeMode.PointWithoutChecked:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.MoveSingleNodePoint;
                        }
                    case NodeMode.LeftControlPoint:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.MoveSingleNodeLeftControlPoint;
                        }
                    case NodeMode.RightControlPoint:
                        {
                            nodeCollection.Index = i;
                            return NodeCollectionMode.MoveSingleNodeRightControlPoint;
                        }
                }
            }

            return NodeCollectionMode.RectChoose;
        }

    }
}
