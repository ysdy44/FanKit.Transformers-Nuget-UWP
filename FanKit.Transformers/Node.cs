using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial struct Node : ICacheTransform
    {
        /// <summary> Point. </summary>
        public Vector2 Point;
        private Vector2 _oldPoint;

        /// <summary> The specified control point on left. </summary>
        public Vector2 LeftControlPoint;
        private Vector2 _oldLeftControlPoint;

        /// <summary> The specified control point on right. </summary>
        public Vector2 RightControlPoint;
        private Vector2 _oldRightControlPoint;

        /// <summary> Gets or Sets node's IsChecked. </summary>
        public bool IsChecked;
        /// <summary> Gets or Sets whether the node is smooth. </summary>
        public bool IsSmooth;


        #region Move

        /// <summary>
        /// Move the node to the destination.
        /// </summary>
        /// <param name="point"> The destination position. </param>
        /// <returns> The moving node. </returns>
        public Node Move(Vector2 point) => new Node
        {
            Point = point,
            LeftControlPoint = point - this.Point + this.LeftControlPoint,
            RightControlPoint = point - this.Point + this.RightControlPoint,
            IsChecked = this.IsChecked,
            IsSmooth = this.IsSmooth,
        };
        /// <summary>
        /// Move the node to the destination.
        /// </summary>
        /// <param name="point"> The destination position. </param>
        /// <param name="isChecked"> The node's IsChecked. </param>
        /// <param name="isSmooth"> The node's IsSmooth. </param>
        /// <returns> The moving node. </returns>
        public Node Move(Vector2 point, bool isChecked = false, bool isSmooth = false) => new Node
        {
            Point = point,
            LeftControlPoint = point - this.Point + this.LeftControlPoint,
            RightControlPoint = point - this.Point + this.RightControlPoint,
            IsChecked = isChecked,
            IsSmooth = isSmooth,
        };

        #endregion

        #region Contained

        /// <summary>
        /// The vector was contained in the rectangle.
        /// </summary>
        /// <param name="left"> The destination rectangle's left. </param>
        /// <param name="top"> The destination rectangle's top. </param>
        /// <param name="right"> The destination rectangle's right. </param>
        /// <param name="bottom"> The destination rectangle's bottom. </param>
        /// <returns> Return **true** if the vector was contained in rectangle, otherwise **false**. </returns>
        public bool Contained(float left, float top, float right, float bottom)
        {
            if (this.Point.X < left) return false;
            if (this.Point.Y < top) return false;
            if (this.Point.X > right) return false;
            if (this.Point.Y > bottom) return false;

            return true;
        }
        /// <summary>
        /// The vector was contained in a rectangle.
        /// </summary>
        /// <param name="transformerRect"> The destination rectangle. </param>
        /// <returns> Return **true** if the vector was contained in rectangle. </returns>
        public bool Contained(TransformerRect transformerRect) => this.Contained(transformerRect.Left, transformerRect.Top, transformerRect.Right, transformerRect.Bottom);

        #endregion

        #region Equals


        //@Override
        /// <summary>
        /// Returns a boolean indicating whether the given Object is equal to this Node instance.
        /// </summary>
        /// <param name="obj"> The Object to compare against. </param>
        /// <returns> Return **true** if the Object is equal to this Node, otherwise **false**. </returns>
        public override bool Equals(object obj) => base.Equals(obj);

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns> The hash code. </returns>
        public override int GetHashCode() => base.GetHashCode();

        /// <summary>
        /// Returns a String representing this Node instance.
        /// </summary>
        /// <returns> The string representation. </returns>
        public override string ToString() => $"Node [Vector = {this.Point}, LeftControl = {this.LeftControlPoint}, RightControl = {this.RightControlPoint}, IsChecked = {this.IsChecked}]";


        /// <summary>
        /// Returns a boolean indicating whether the given Node is equal to this Node instance.
        /// </summary>
        /// <param name="other"> The Node to compare this instance to. </param>
        /// <returns> Return **true** if the other Node is equal to this instance, otherwise **false**. </returns>
        public bool Equals(Node other)
        {
            if (this.IsChecked != other.IsChecked) return false;
            if (this.Point != other.Point) return false;
            if (this.LeftControlPoint != other.LeftControlPoint) return false;
            if (this.RightControlPoint != other.RightControlPoint) return false;
            return true;
        }


        //@Static
        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified nodes is equal.
        /// </summary>
        /// <param name="left"> The first node to compare. </param>
        /// <param name="right"> The second node to compare. </param>
        /// <returns> Return **true** if left and right are equal, otherwise **false**. </returns>
        public static bool operator ==(Node left, Node right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given nodes are not equal.
        /// </summary>
        /// <param name="left"> The first node to compare. </param>
        /// <param name="right"> The second node to compare. </param>
        /// <returns> Return **true** if the nodes are not equal; False if they are equal. </returns>
        public static bool operator !=(Node left, Node right) => !left.Equals(right);


        #endregion

    }
}