using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four range values (Left, Top, Right, Bottom). 
    /// </summary>
    public partial struct TransformerRect
    {

        //@Constructs
        /// <summary>
        /// Constructs a <see cref = "TransformerRect" />.
        /// </summary>
        /// <param name="pointA"> The frist point of rectangle. </param>
        /// <param name="pointB"> The second point of rectangle. </param>
        public TransformerRect(Vector2 pointA, Vector2 pointB)
        {
            float left = System.Math.Min(pointA.X, pointB.X);
            float top = System.Math.Min(pointA.Y, pointB.Y);
            float right = System.Math.Max(pointA.X, pointB.X);
            float bottom = System.Math.Max(pointA.Y, pointB.Y);

            float width = System.Math.Abs(pointA.X - pointB.X);
            float height = System.Math.Abs(pointA.Y - pointB.Y);
            float centerX = (pointA.X + pointB.X) / 2;
            float centerY = (pointA.Y + pointB.Y) / 2;

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;

            this.Width = width;
            this.Height = height;
            this.Center = new Vector2(centerX, centerY);
            this.CenterX = centerX;
            this.CenterY = centerY;

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }

        /// <summary>
        /// Constructs a <see cref = "TransformerRect" />.
        /// </summary>
        /// <param name="pointA"> The frist point of rectangle. </param>
        /// <param name="pointB"> The second point of rectangle. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isSquare"> Equal in width and height. </param>
        public TransformerRect(Vector2 pointA, Vector2 pointB, bool isCenter, bool isSquare)
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

            float width = System.Math.Abs(pointA.X - pointB.X);
            float height = System.Math.Abs(pointA.Y - pointB.Y);
            float centerX = (pointA.X + pointB.X) / 2;
            float centerY = (pointA.Y + pointB.Y) / 2;

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;

            this.Width = width;
            this.Height = height;
            this.Center = new Vector2(centerX, centerY);
            this.CenterX = centerX;
            this.CenterY = centerY;

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
        
        /// <summary>
        /// Constructs a <see cref = "TransformerRect" />.
        /// </summary>
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        /// <param name="postion"> The postion. </param>
        public TransformerRect(float width, float height, Vector2 postion)
        {
            float left = postion.X;
            float top = postion.Y;
            float right = width + postion.X;
            float bottom = height + postion.Y;

            float centerX = postion.X + width / 2;
            float centerY = postion.Y + height / 2;

            this.Left = left;
            this.Top = top;
            this.Right = right;
            this.Bottom = bottom;

            this.Width = width;
            this.Height = height;
            this.Center = new Vector2(centerX, centerY);
            this.CenterX = centerX;
            this.CenterY = centerY;

            this.LeftTop = new Vector2(left, top);
            this.RightTop = new Vector2(right, top);
            this.RightBottom = new Vector2(right, bottom);
            this.LeftBottom = new Vector2(left, bottom);
        }
   

        /// <summary>
        /// Turn to Rect.
        /// </summary>
        public Rect ToRect() => new Rect(this.Left, this.Top, this.Right - this.Left, this.Bottom - this.Top);


        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator) => TransformerRect.CreateRectangle(resourceCreator, this);

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns> The product geometry. </returns>
        public CanvasGeometry ToRectangle(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => TransformerRect.CreateRectangle(resourceCreator, this, matrix);


        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <returns></returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator) => TransformerRect.CreateEllipse(resourceCreator, this);

        /// <summary>
        /// Turn to geometry.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <returns></returns>
        public CanvasGeometry ToEllipse(ICanvasResourceCreator resourceCreator, Matrix3x2 matrix) => TransformerRect.CreateEllipse(resourceCreator, this, matrix);
        
    }
}