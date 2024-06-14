﻿using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a maximum boundary in a plane right-angle coordinate system,
    /// including the left right top bottom and center.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TransformerBorder : ITransformerWH //, ITransformerLTRB
    {
        //@Constructs
        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="left"> The X-axis value of the left side of the bounds. </param>
        /// <param name="top"> The Y-axis position of the top of the bounds. </param>
        /// <param name="right"> The X-axis value of the right side of the bounds. </param>
        /// <param name="bottom"> The Y-axis position of the bottom of the bounds. </param>
        public TransformerBorder(float left, float top, float right, float bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        public TransformerBorder(float width, float height)
        {
            this.Left = 0;
            this.Top = 0;
            this.Right = width;
            this.Bottom = height;
        }

        /// <summary>
        /// Initialize a <see cref="Transformer"/>.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        public TransformerBorder(ITransformerLTRB transformer)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            this.Init(transformer.LeftTop);
            this.Init(transformer.RightTop);
            this.Init(transformer.RightBottom);
            this.Init(transformer.LeftBottom);
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="transformers"> The transformers. </param>
        public TransformerBorder(IEnumerable<Transformer> transformers)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Transformer item in transformers)
            {
                this.Init(item.LeftTop);
                this.Init(item.RightTop);
                this.Init(item.RightBottom);
                this.Init(item.LeftBottom);
            }
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="getActualTransformers"> The IGetActualTransformer. </param>
        public TransformerBorder(IEnumerable<IGetActualTransformer> getActualTransformers)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (IGetActualTransformer item in getActualTransformers)
            {
                Transformer transformer = item.GetActualTransformer();

                this.Init(transformer.LeftTop);
                this.Init(transformer.RightTop);
                this.Init(transformer.RightBottom);
                this.Init(transformer.LeftBottom);
            }
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        public TransformerBorder(IEnumerable<Node> nodes)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Node item in nodes)
            {
                switch (item.Type)
                {
                    case NodeType.BeginFigure:
                    case NodeType.Node:
                        this.Init(item.Point);
                        break;
                }
            }
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="nodess"> The <see cref="NodeCollection"/> collection. </param>
        public TransformerBorder(IList<NodeCollection> nodess)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (NodeCollection item in nodess)
            {
                foreach (Node node in item)
                {
                    switch (node.Type)
                    {
                        case NodeType.BeginFigure:
                        case NodeType.Node:
                            this.Init(node.Point);
                            break;
                    }
                }
            }
        }

        private void Init(Vector2 vector)
        {
            if (this.Left > vector.X) this.Left = vector.X;
            if (this.Top > vector.Y) this.Top = vector.Y;
            if (this.Right < vector.X) this.Right = vector.X;
            if (this.Bottom < vector.Y) this.Bottom = vector.Y;
        }


        //@Static
        /// <summary>
        /// Adds border and vector together.
        /// </summary>
        /// <param name="border"> The source border. </param>
        /// <param name="vector"> The added vector. </param>
        /// <returns> The resulting border. </returns>
        public static TransformerBorder Add(TransformerBorder border, Vector2 vector) => new TransformerBorder
        {
            Left = border.Left + vector.X,
            Top = border.Top + vector.Y,
            Right = border.Right + vector.X,
            Bottom = border.Bottom + vector.Y
        };


        /// <summary>
        /// Adds border and vector together.
        /// </summary>
        /// <param name="value1"> The source border. </param>
        /// <param name="value2"> The source vector. </param>
        /// <returns> The summed border. </returns>
        public static TransformerBorder operator +(TransformerBorder value1, Vector2 value2) => TransformerBorder.Add(value1, value2);


        /// <summary>
        /// Returns a boolean indicating whether the given TransformerBorder is equal to this TransformerBorder instance.
        /// </summary>
        /// <param name="other"> The TransformerBorder to compare this instance to. </param>
        /// <returns> Return **true** if the other TransformerBorder is equal to this instance, otherwise **false**. </returns>
        public bool Equals(TransformerBorder other)
        {
            return this.Left == other.Left && this.Top == other.Top && this.Right == other.Right && this.Bottom == other.Bottom;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns> A 32-bit signed integer that is the hash code for this instance. </returns>
        public override int GetHashCode()
        {
            int hashCode = 2045187211;
            hashCode = hashCode * -1521134295 + this.Left.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Top.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Right.GetHashCode();
            hashCode = hashCode * -1521134295 + this.Bottom.GetHashCode();
            hashCode = hashCode * -1521134295 + this.CenterX.GetHashCode();
            hashCode = hashCode * -1521134295 + this.CenterY.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.Center.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterLeft.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterTop.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterRight.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterBottom.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.LeftTop.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.RightTop.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.RightBottom.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.LeftBottom.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.Horizontal.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.Vertical.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified nodes is equal.
        /// </summary>
        /// <param name="left"> The first border to compare. </param>
        /// <param name="right"> The second border to compare. </param>
        /// <returns> Return **true** if left and right are equal, otherwise **false**. </returns>
        public static bool operator ==(TransformerBorder left, TransformerBorder right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given nodes are not equal.
        /// </summary>
        /// <param name="left"> The first border to compare. </param>
        /// <param name="right"> The second border to compare. </param>
        /// <returns> Return **true** if the nodes are not equal; False if they are equal. </returns>
        public static bool operator !=(TransformerBorder left, TransformerBorder right) => !left.Equals(right);

    }
}