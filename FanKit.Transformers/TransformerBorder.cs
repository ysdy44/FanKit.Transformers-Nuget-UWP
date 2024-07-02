using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a maximum boundary in a plane right-angle coordinate system.
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct TransformerBorder : ITransformerGeometry
    {

        /// <summary> The X-axis value of the left side of the border. </summary>
        public float Left;
        /// <summary> The Y-axis position of the top of the border. </summary>
        public float Top;
        /// <summary> The X-axis value of the right side of the border. </summary>
        public float Right;
        /// <summary> The Y-axis position of the bottom of the border. </summary>
        public float Bottom;

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
        /// <param name="width"> The width of the border. </param>
        /// <param name="height"> The height of the border. </param>
        public TransformerBorder(float width, float height)
        {
            this.Left = 0;
            this.Top = 0;
            this.Right = width;
            this.Bottom = height;
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="width"> The width of the rectangle. </param>
        /// <param name="height"> The height of the rectangle. </param>
        /// <param name="postion"> The position of the top-left corner of the rectangle. </param>
        public TransformerBorder(float width, float height, Vector2 postion)
        {
            float left = postion.X;
            float top = postion.Y;
            float right = width + postion.X;
            float bottom = height + postion.Y;

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="pointA"> The first point that new rectangle must contain. </param>
        /// <param name="pointB"> The second point that new rectangle must contain. </param>
        public TransformerBorder(Vector2 pointA, Vector2 pointB)
        {
            float left = System.Math.Min(pointA.X, pointB.X);
            float top = System.Math.Min(pointA.Y, pointB.Y);
            float right = System.Math.Max(pointA.X, pointB.X);
            float bottom = System.Math.Max(pointA.Y, pointB.Y);

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="pointA"> The first point that new rectangle must contain. </param>
        /// <param name="pointB"> The second point that new rectangle must contain. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isSquare"> Equal in width and height. </param>
        public TransformerBorder(Vector2 pointA, Vector2 pointB, bool isCenter, bool isSquare)
        {
            if (isSquare)
            {
                float square = Vector2.Distance(pointA, pointB) / 1.4142135623730950488016887242097f;

                pointB = pointA + new Vector2((pointB.X > pointA.X) ? square : -square, (pointB.Y > pointA.Y) ? square : -square);
            }

            if (isCenter)
            {
                pointA = pointA + pointA - pointB;
            }

            float left = System.Math.Min(pointA.X, pointB.X);
            float top = System.Math.Min(pointA.Y, pointB.Y);
            float right = System.Math.Max(pointA.X, pointB.X);
            float bottom = System.Math.Max(pointA.Y, pointB.Y);

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="bounds"> The bounds. </param>
        public TransformerBorder(Transformer bounds)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            this.Init(bounds.LeftTop);
            this.Init(bounds.RightTop);
            this.Init(bounds.RightBottom);
            this.Init(bounds.LeftBottom);
        }

        /// <summary>
        /// Initialize a <see cref="TransformerBorder"/>.
        /// </summary>
        /// <param name="bounds"> The bounds. </param>
        public TransformerBorder(IEnumerable<Transformer> bounds)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (Transformer item in bounds)
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
        /// <param name="bounds"> The bounds. </param>
        public TransformerBorder(IEnumerable<IGetActualTransformer> bounds)
        {
            this.Left = float.MaxValue;
            this.Top = float.MaxValue;
            this.Right = float.MinValue;
            this.Bottom = float.MinValue;

            foreach (IGetActualTransformer item in bounds)
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
        /// <param name="left"> The border to add. </param>
        /// <param name="right"> The vector to add. </param>
        /// <returns> The summed border. </returns>
        public static TransformerBorder Add(TransformerBorder left, Vector2 right) => left + right;

        /// <summary>
        /// Subtracts the vector from the border.
        /// </summary>
        /// <param name="left"> The border. </param>
        /// <param name="right"> The vector. </param>
        /// <returns> The difference border. </returns>
        public static TransformerBorder Subtract(TransformerBorder left, Vector2 right) => left - right;

        /// <summary>
        /// Multiplies a border by a specified scalar.
        /// </summary>
        /// <param name="left"> The border to multiply. </param>
        /// <param name="right"> The scalar value. </param>
        /// <returns> The scaled border. </returns>
        public static TransformerBorder Multiply(TransformerBorder left, float right) => left * right;

        /// <summary>
        /// Divides the specified border by a specified scalar value.
        /// </summary>
        /// <param name="left"> The border. </param>
        /// <param name="divisor"> The scalar value. </param>
        /// <returns> The border that results from the division. </returns>
        public static TransformerBorder Divide(TransformerBorder left, float divisor) => left / divisor;


        private TransformerBorder Scale(float scale)
        {
            return new TransformerBorder
            {
                Left = this.Left * scale,
                Top = this.Top * scale,
                Right = this.Right * scale,
                Bottom = this.Bottom * scale
            };
        }

        private TransformerBorder Scale(float scaleX, float scaleY)
        {
            return new TransformerBorder
            {
                Left = this.Left * scaleX,
                Top = this.Top * scaleY,
                Right = this.Right * scaleX,
                Bottom = this.Bottom * scaleY
            };
        }

        private TransformerBorder Scale(float scaleX, float scaleY, float centerPointX, float centerPointY)
        {
            float sx = centerPointX - centerPointX * scaleX;
            float sy = centerPointY - centerPointY * scaleY;
            return new TransformerBorder
            {
                Left = this.Left * scaleX + sx,
                Top = this.Top * scaleY + sy,
                Right = this.Right * scaleX + sx,
                Bottom = this.Bottom * scaleY + sy
            };
        }


        /// <summary>
        /// Adds border and vector together.
        /// </summary>
        /// <param name="left"> The border to add. </param>
        /// <param name="right"> The vector to add. </param>
        /// <returns> The summed border. </returns>
        public static TransformerBorder operator +(TransformerBorder left, Vector2 right) => new TransformerBorder
        {
            Left = left.Left + right.X,
            Top = left.Top + right.Y,
            Right = left.Right + right.X,
            Bottom = left.Bottom + right.Y
        };

        /// <summary>
        /// Subtracts the vector from the border.
        /// </summary>
        /// <param name="left"> The border. </param>
        /// <param name="right"> The vector. </param>
        /// <returns> The border that results from subtracting vector from border. </returns>
        public static TransformerBorder operator -(TransformerBorder left, Vector2 right) => new TransformerBorder
        {
            Left = left.Left - right.X,
            Top = left.Top - right.Y,
            Right = left.Right - right.X,
            Bottom = left.Bottom - right.Y
        };

        /// <summary>
        /// Multiples the specified border by the specified scalar value.
        /// </summary>
        /// <param name="left"> The border. </param>
        /// <param name="right"> The scalar value. </param>
        /// <returns> The scaled border. </returns>
        public static TransformerBorder operator *(TransformerBorder left, float right) => new TransformerBorder
        {
            Left = left.Left * right,
            Top = left.Top * right,
            Right = left.Right * right,
            Bottom = left.Bottom * right
        };

        /// <summary>
        /// Divides the specified border by a specified scalar value.
        /// </summary>
        /// <param name="left"> The border. </param>
        /// <param name="right"> The scalar value. </param>
        /// <returns> The result of the division. </returns>
        public static TransformerBorder operator /(TransformerBorder left, float right) => new TransformerBorder
        {
            Left = left.Left / right,
            Top = left.Top / right,
            Right = left.Right / right,
            Bottom = left.Bottom / right
        };


        /// <summary>
        /// Returns a boolean indicating whether the given border is equal to this border instance.
        /// </summary>
        /// <param name="other"> The border to compare this instance to. </param>
        /// <returns> Return **true** if the other border is equal to this instance, otherwise **false**. </returns>
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
        public static bool operator !=(TransformerBorder left, TransformerBorder right) => !(left == right);

    }
}