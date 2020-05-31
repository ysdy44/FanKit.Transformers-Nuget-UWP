using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents an ordered collection of node collection.
    /// </summary>
    public sealed partial class NodeCollectionCollection : ICacheTransform, IList<NodeCollection>, IEnumerable<NodeCollection>
    {

        /// <summary>
        /// Select only one node.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="matrix"> The matrix. </param>
        public bool SelectionOnlyOne(Vector2 point, Matrix3x2 matrix)
        {
            bool hasIsSelected = false;

            foreach (NodeCollection nodes in this)
            {
                //Unchecked others.
                if (hasIsSelected) nodes.SelectionDeselect();
                else
                {
                    bool isSelected = nodes.SelectionOnlyOne(point, matrix);

                    //Check the selected node.
                    if (isSelected)
                    {
                        hasIsSelected = true;
                    }
                    //Unchecked others.
                    else nodes.SelectionDeselect();
                }
            }

            return hasIsSelected;
        }
        /// Select deselect.
        /// </summary>
        public void SelectionDeselect()
        {
            foreach (NodeCollection nodes in this)
            {
                nodes.SelectionDeselect();
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
            foreach (NodeCollection nodes in this)
            {
                nodes.RectChoose(left, top, right, bottom);
            }
        }
        /// <summary>
        /// Check node which in the rect.
        /// </summary>
        /// <param name="boxRect"> The destination rectangle. </param>
        public void BoxChoose(TransformerRect boxRect) => this.RectChoose(boxRect.Left, boxRect.Top, boxRect.Right, boxRect.Bottom);

    }
}
