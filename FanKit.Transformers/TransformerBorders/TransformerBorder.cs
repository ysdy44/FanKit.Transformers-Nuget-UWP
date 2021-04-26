using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a maximum boundary in a plane right-angle coordinate system,
    /// including the left right top bottom and center.
    /// </summary>
    public partial struct TransformerBorder
    {
        //@Constructs
        /// <summary>
        /// Initialize a <see cref = "TransformerBorder" />.
        /// </summary>
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        public TransformerBorder(float left, float top, float right, float bottom)
        {
            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.CenterX = (left + right) / 2;
            this.CenterY = (top + bottom) / 2;
        }

        /// <summary>
        /// Initialize a <see cref = "TransformerBorder" />.
        /// </summary>
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        public TransformerBorder(float width, float height)
        {
            this.Left = 0;
            this.Top = 0;
            this.Right = width;
            this.Bottom = height;
            this.CenterX = width / 2;
            this.CenterY = height / 2;
        }

        /// <summary>
        /// Initialize a <see cref = "Transformer" />.
        /// </summary>
        /// <param name="transformer"> The transformer. </param>
        public TransformerBorder(ITransformerLTRB transformer)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            void aaa(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            aaa(transformer.LeftTop);
            aaa(transformer.RightTop);
            aaa(transformer.RightBottom);
            aaa(transformer.LeftBottom);

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.CenterX = (left + right) / 2;
            this.CenterY = (Top + bottom) / 2;
        }

        /// <summary>
        /// Initialize a <see cref = "TransformerBorder" />.
        /// </summary>
        /// <param name="transformers"> The transformers. </param>
        public TransformerBorder(IEnumerable<Transformer> transformers)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            void aaa(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            foreach (Transformer transformer in transformers)
            {
                aaa(transformer.LeftTop);
                aaa(transformer.RightTop);
                aaa(transformer.RightBottom);
                aaa(transformer.LeftBottom);
            }

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.CenterX = (left + right) / 2;
            this.CenterY = (Top + bottom) / 2;
        }

        /// <summary>
        /// Initialize a <see cref = "TransformerBorder" />.
        /// </summary>
        /// <param name="getActualTransformers"> The IGetActualTransformer. </param>
        public TransformerBorder(IEnumerable<IGetActualTransformer> getActualTransformers)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            void aaa(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            foreach (IGetActualTransformer getActualTransformer in getActualTransformers)
            {
                Transformer transformer = getActualTransformer.GetActualTransformer();

                aaa(transformer.LeftTop);
                aaa(transformer.RightTop);
                aaa(transformer.RightBottom);
                aaa(transformer.LeftBottom);
            }

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.CenterX = (left + right) / 2;
            this.CenterY = (Top + bottom) / 2;
        }

        /// <summary>
        /// Initialize a <see cref = "TransformerBorder" />.
        /// </summary>
        /// <param name="nodes"> The nodes. </param>
        public TransformerBorder(IEnumerable<Node> nodes)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            void aaa(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            foreach (Node node in nodes)
            {
                switch (node.Type)
                {
                    case NodeType.BeginFigure:
                    case NodeType.Node:
                        aaa(node.Point);
                        break;
                }
            }

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.CenterX = (left + right) / 2;
            this.CenterY = (Top + bottom) / 2;
        }

        /// <summary>
        /// Initialize a <see cref = "TransformerBorder" />.
        /// </summary>
        /// <param name="nodess"> The NodeCollectionCollection. </param>
        public TransformerBorder(IList<NodeCollection> nodess)
        {
            float left = float.MaxValue;
            float top = float.MaxValue;
            float right = float.MinValue;
            float bottom = float.MinValue;

            void aaa(Vector2 vector)
            {
                if (left > vector.X) left = vector.X;
                if (top > vector.Y) top = vector.Y;
                if (right < vector.X) right = vector.X;
                if (bottom < vector.Y) bottom = vector.Y;
            }

            foreach (NodeCollection nodes in nodess)
            {
                foreach (Node node in nodes)
                {
                    switch (node.Type)
                    {
                        case NodeType.BeginFigure:
                        case NodeType.Node:
                            aaa(node.Point);
                            break;
                    }
                }
            }

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
            this.CenterX = (left + right) / 2;
            this.CenterY = (Top + bottom) / 2;
        }



        //@Static
        /// <summary>
        /// Adds border and vector together.
        /// </summary>
        /// <param name="border"> The source border. </param>
        /// <param name="vector"> The added vector. </param>
        /// <returns> The resulting border. </returns>
        public static TransformerBorder Add(TransformerBorder border, Vector2 vector) => new TransformerBorder
        (
            left: border.Left + vector.X,
            top: border.Top + vector.Y,
            right: border.Right + vector.X,
            bottom: border.Bottom + vector.Y
        );


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
            if (this.Left != other.Left) return false;
            if (this.Top != other.Top) return false;
            if (this.Right != other.Right) return false;
            if (this.Bottom != other.Bottom) return false;
            return true;
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