using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four positions (top-left, top-right, bottom-right, bottom-left corners). 
    /// </summary>
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public partial struct Transformer : ITransformerXY, ITransformerLTRB, ITransformerGeometry
    {

        //@Static
        /// <summary>
        /// Returns the transformer (-1,-1) (1,-1) (1,1) (-1,1).
        /// </summary>
        public static Transformer One => new Transformer
        {
            LeftTop = new Vector2(-1f, -1f),
            RightTop = new Vector2(1f, -1f),
            RightBottom = new Vector2(1f, 1f),
            LeftBottom = new Vector2(-1f, 1f),
        };

        //@Constructs
        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="left"> The X-axis value of the left side of the bounds. </param>
        /// <param name="top"> The Y-axis position of the top of the bounds. </param>
        /// <param name="right"> The X-axis value of the right side of the bounds. </param>
        /// <param name="bottom"> The Y-axis position of the bottom of the bounds. </param>
        public Transformer(float left, float top, float right, float bottom)
        {
            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="rect"> The bounding rectangle. </param>
        public Transformer(TransformerRect rect)
        {
            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="pointA"> The first point that new transformer must contain. </param>
        /// <param name="pointB"> The second point that new transformer must contain. </param>
        public Transformer(Vector2 pointA, Vector2 pointB)
        {
            TransformerRect rect = new TransformerRect(pointA, pointB);

            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="pointA"> The first point that new transformer must contain. </param>
        /// <param name="pointB"> The second point that new transformer must contain. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        public Transformer(Vector2 pointA, Vector2 pointB, bool isCenter, bool isRatio)
        {
            TransformerRect rect = new TransformerRect(pointA, pointB, isCenter, isRatio);

            this.LeftTop = rect.LeftTop;
            this.RightTop = rect.RightTop;
            this.RightBottom = rect.RightBottom;
            this.LeftBottom = rect.LeftBottom;
        }

        /// <summary>
        /// Initialize a transformer.
        /// </summary>
        /// <param name="width"> The width of the transformer. </param>
        /// <param name="height"> The height of the transformer. </param>
        /// <param name="postion"> The position of the top-left corner of the transformer. </param>
        public Transformer(float width, float height, Vector2 postion)
        {
            this.LeftTop = postion;
            this.RightTop = new Vector2(postion.X + width, postion.Y);
            this.RightBottom = new Vector2(postion.X + width, postion.Y + height);
            this.LeftBottom = new Vector2(postion.X, postion.Y + height);
        }


        //@Static
        /// <summary>
        /// Adds transformer and vector together.
        /// </summary>
        /// <param name="left"> The transformer to add. </param>
        /// <param name="right"> The vector to add. </param>
        /// <returns> The summed transformer. </returns>
        public static Transformer Add(Transformer left, Vector2 right) => left + right;

        /// <summary>
        /// Subtracts the vector from the transformer.
        /// </summary>
        /// <param name="left"> The transformer. </param>
        /// <param name="right"> The vector. </param>
        /// <returns> The difference transformer. </returns>
        public static Transformer Subtract(Transformer left, Vector2 right) => left - right;

        /// <summary>
        /// Multiplies transformer and vector  and returns the resulting transformer.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="matrix"> The transformation matrix. </param>
        /// <returns> The product transformer. </returns>
        public static Transformer Multiplies(Transformer transformer, Matrix3x2 matrix) => transformer * matrix;

        public static Transformer Multiply(Transformer left, float right) => left * right;


        /// <summary>
        /// Adds transformer and vector together.
        /// </summary>
        /// <param name="left"> The transformer to add. </param>
        /// <param name="right"> The vector to add. </param>
        /// <returns> The summed transformer. </returns>
        public static Transformer operator +(Transformer left, Vector2 right) => new Transformer
        {
            LeftTop = left.LeftTop + right,
            RightTop = left.RightTop + right,
            RightBottom = left.RightBottom + right,
            LeftBottom = left.LeftBottom + right,
        };

        /// <summary>
        /// Subtracts the vector from the transformer.
        /// </summary>
        /// <param name="left"> The transformer. </param>
        /// <param name="right"> The vector. </param>
        /// <returns> The transformer that results from subtracting vector from transformer. </returns>
        public static Transformer operator -(Transformer left, Vector2 right) => new Transformer
        {
            LeftTop = Vector2.Subtract(left.LeftTop, right),
            RightTop = Vector2.Subtract(left.RightTop, right),
            RightBottom = Vector2.Subtract(left.RightBottom, right),
            LeftBottom = Vector2.Subtract(left.LeftBottom, right),
        };

        /// <summary>
        /// Multiplies transformer and vector  and returns the resulting transformer.
        /// </summary>
        /// <param name="left"> The source transformer. </param>
        /// <param name="right"> The scaling value to use. </param>
        /// <returns> The resulting transformer. </returns>
        public static Transformer operator *(Transformer left, Matrix3x2 right) => new Transformer
        {
            LeftTop = Vector2.Transform(left.LeftTop, right),
            RightTop = Vector2.Transform(left.RightTop, right),
            RightBottom = Vector2.Transform(left.RightBottom, right),
            LeftBottom = Vector2.Transform(left.LeftBottom, right)
        };

        public static Transformer operator *(Transformer left, float right) => new Transformer
        {
            LeftTop = Vector2.Multiply(left.LeftTop, right),
            RightTop = Vector2.Multiply(left.RightTop, right),
            RightBottom = Vector2.Multiply(left.RightBottom, right),
            LeftBottom = Vector2.Multiply(left.LeftBottom, right),
        };


        /// <summary>
        /// Returns a boolean indicating whether the given Transformer is equal to this Transformer instance.
        /// </summary>
        /// <param name="other"> The Transformer to compare this instance to. </param>
        /// <returns> Return **true** if the other Transformer is equal to this instance, otherwise **false**. </returns>
        public bool Equals(Transformer other)
        {
            return this.LeftTop == other.LeftTop && this.RightTop == other.RightTop && this.RightBottom == other.RightBottom && this.LeftBottom == other.LeftBottom;
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns> A 32-bit signed integer that is the hash code for this instance. </returns>
        public override int GetHashCode()
        {
            int hashCode = 848733247;
            hashCode = hashCode * -1521134295 + this.LeftTop.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightTop.GetHashCode();
            hashCode = hashCode * -1521134295 + this.RightBottom.GetHashCode();
            hashCode = hashCode * -1521134295 + this.LeftBottom.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.Center.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterLeft.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterTop.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterRight.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.CenterBottom.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.MinX.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.MaxX.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.MinY.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.MaxY.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.Horizontal.GetHashCode();
            //hashCode = hashCode * -1521134295 + this.Vertical.GetHashCode();
            return hashCode;
        }

        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified nodes is equal.
        /// </summary>
        /// <param name="left"> The first transformer to compare. </param>
        /// <param name="right"> The second transformer to compare. </param>
        /// <returns> Return **true** if left and right are equal, otherwise **false**. </returns>
        public static bool operator ==(Transformer left, Transformer right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given nodes are not equal.
        /// </summary>
        /// <param name="left"> The first transformer to compare. </param>
        /// <param name="right"> The second transformer to compare. </param>
        /// <returns> Return **true** if the nodes are not equal; False if they are equal. </returns>
        public static bool operator !=(Transformer left, Transformer right) => !(left == right);

    }
}