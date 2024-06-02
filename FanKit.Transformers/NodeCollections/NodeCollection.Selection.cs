using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    partial class NodeCollection
    {

        /// <summary>
        /// Select only one node.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="matrix"> The matrix. </param>
        public bool SelectionOnlyOne(Vector2 point, Matrix3x2 matrix)
        {
            bool hasIsSelected = false;

            foreach (Node node in this)
            {
                // Unchecked others.
                if (hasIsSelected) node.IsChecked = false;
                else
                {
                    switch (node.Type)
                    {
                        case NodeType.BeginFigure:
                        case NodeType.Node:
                            {
                                Vector2 point2 = Vector2.Transform(node.Point, matrix);
                                bool isSelected = FanKit.Math.InNodeRadius(point, point2);

                                //Check the selected node.
                                if (isSelected)
                                {
                                    hasIsSelected = true;
                                    node.IsChecked = true;
                                }
                                //Unchecked others.
                                else node.IsChecked = false;
                            }
                            break;
                    }
                }
            }

            return hasIsSelected;
        }

        /// <summary>
        /// Deselect.
        /// </summary>
        public void SelectionDeselect()
        {
            foreach (Node node in this)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                    case NodeType.Node:
                        node.IsChecked = false;
                        break;
                }
            }
        }


        /// <summary>
        /// Check node which in the rect.
        /// </summary>
        /// <param name="left"> The destination rectangle's left. </param>
        /// <param name="top"> The destination rectangle's top. </param>
        /// <param name="right"> The destination rectangle's right. </param>
        /// <param name="bottom"> The destination rectangle's bottom. </param>
        public void RectChoose(float left, float top, float right, float bottom)
        {
            foreach (Node node in this)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                    case NodeType.Node:
                        {
                            bool isContained = node.Contained(left, top, right, bottom);
                            if (node.IsChecked != isContained)
                            {
                                node.IsChecked = isContained;
                            }
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Check node which in the rect.
        /// </summary>
        /// <param name="boxRect"> The destination rectangle. </param>
        public void BoxChoose(TransformerRect boxRect) => this.RectChoose(boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Bottom);

    }
}