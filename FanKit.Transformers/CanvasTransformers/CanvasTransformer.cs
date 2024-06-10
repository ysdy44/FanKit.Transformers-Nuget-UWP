using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Imaging;

namespace FanKit.Transformers
{
    /// <summary>
    /// Transformer: 
    /// Provide matrix by Position、Scale、Radians.
    /// 
    /// 
    /// TODO:
    /// Canvas Layer:
    ///    The original size of the layer.
    /// Virtual Layer:
    ///    Render all layers together and make their center points coincide with the origin (0,0) and then zoom;
    /// Control Layer:
    ///    Rotate around the origin first, then shift. (The canvas has a rotation angle)
    ///    
    /// </summary>
    public partial class CanvasTransformer
    {

        /// <summary> <see cref="CanvasTransformer"/>'s width. </summary>
        public int Width { get; set; } = 1000;
        /// <summary> <see cref="CanvasTransformer"/>'s height. </summary>
        public int Height { get; set; } = 1000;

        /// <summary> <see cref="CanvasTransformer"/>'s scale. </summary>
        public float Scale { get; set; } = 1.0f;

        /// <summary> CanvasControl's width. </summary>
        public float ControlWidth { get; set; } = 1000.0f;
        /// <summary> CanvasControl's height. </summary>
        public float ControlHeight { get; set; } = 1000.0f;
        /// <summary> CanvasControl's center. </summary>
        public Vector2 ControlCenter => new Vector2(this.ControlWidth / 2, this.ControlHeight / 2);

        /// <summary> <see cref="CanvasTransformer"/>'s translation. </summary>
        public Vector2 Position { get; set; } = new Vector2(0.0f, 0.0f);
        /// <summary> <see cref="CanvasTransformer"/>'s rotation. </summary>
        public float Radian { get; set; } = 0.0f;


        //@Construct
        /// <summary>
        /// Initialize a <see cref="CanvasTransformer"/>.
        /// </summary>
        public CanvasTransformer()
        {
            this.ReloadMatrix();
        }


        #region Size


        /// <summary> <see cref="CanvasTransformer.ControlWidth"/> and <see cref="CanvasTransformer.ControlHeight"/>'s setter. </summary>
        public Size Size
        {
            set
            {
                this.ControlWidth = (float)value.Width;
                this.ControlHeight = (float)value.Height;
            }
        }
        /// <summary> <see cref="CanvasTransformer.Width"/> and <see cref="CanvasTransformer.Height"/>'s setter. </summary>
        public BitmapSize BitmapSize
        {
            set
            {
                int width = (int)value.Width;
                int height = (int)value.Height;

                this.Width = width;
                this.Height = height;
            }
        }

        /// <summary>
        /// Get the scale.
        /// </summary>
        /// <param name="bitmapSize"> The bitmap size. </param>
        /// <returns> The produced vector. </returns>
        public Vector2 GetScale(BitmapSize bitmapSize) => new Vector2((float)bitmapSize.Width / (float)this.Width, (float)bitmapSize.Height / (float)this.Height);


        /// <summary> 
        /// Fit to the screen. 
        /// </summary>
        public void Fit()
        {
            float widthScale = this.ControlWidth / this.Width;
            float heightScale = this.ControlHeight / this.Height;

            this.Scale = System.Math.Min(widthScale, heightScale);

            this.Position = new Vector2(this.ControlWidth / 2.0f, this.ControlHeight / 2.0f);

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }

        /// <summary>
        /// Fit to the screen with scale.
        /// </summary>
        /// <param name="scale"> The scalar value. </param>
        public void Fit(float scale)
        {
            this.Scale = scale;

            this.Position = new Vector2(this.ControlWidth / 2.0f, this.ControlHeight / 2.0f);

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }


        #endregion


        /// <summary>
        /// Gets value by left, right, top, bottom.
        /// </summary>
        /// <param name="borderMode"> The border mode. </param>
        /// <returns> The produced value. </returns>
        public float GetBorderValue(BorderMode borderMode)
        {
            switch (borderMode)
            {
                case BorderMode.MinX: return 0;
                case BorderMode.CenterX: return this.Width / 2.0f;
                case BorderMode.MaxX: return this.Width;

                case BorderMode.MinY: return 0;
                case BorderMode.CenterY: return this.Height / 2.0f;
                case BorderMode.MaxY: return this.Height;
            }
            return 0;
        }

        /// <summary>
        /// Gets vector by left, right, top, bottom.
        /// </summary>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <returns> The produced vector. </returns>
        public Vector2 GetIndicatorVector(IndicatorMode indicatorMode)
        {
            switch (indicatorMode)
            {
                case IndicatorMode.LeftTop: return new Vector2(0, 0);
                case IndicatorMode.RightTop: return new Vector2(this.Width, 0);
                case IndicatorMode.RightBottom: return new Vector2(this.Width, this.Height);
                case IndicatorMode.LeftBottom: return new Vector2(0, this.Height);

                case IndicatorMode.Left: return new Vector2(0, this.Height / 2.0f);
                case IndicatorMode.Top: return new Vector2(this.Width / 2.0f, 0);
                case IndicatorMode.Right: return new Vector2(this.Width, this.Height / 2.0f);
                case IndicatorMode.Bottom: return new Vector2(this.Width / 2.0f, this.Height);

                case IndicatorMode.Center: return new Vector2(this.Width / 2.0f, this.Height / 2.0f);
            }
            return new Vector2(0, 0);
        }


        /// <summary>
        /// Returns a boolean indicating whether the given BitmapSize is equal to this CanvasTransformer instance.
        /// </summary>
        /// <param name="other"> The BitmapSize to compare this instance to. </param>
        /// <returns> Return **true** if the other BitmapSize is equal to this instance, otherwise **false**. </returns>
        public bool Equals(BitmapSize other)
        {
            return this.Width == (int)other.Width && this.Height == (int)other.Height;
        }

        /// <summary>
        /// Returns a value that indicates whether each pair of elements in two specified size is equal.
        /// </summary>
        /// <param name="left"> The first CanvasTransformer to compare. </param>
        /// <param name="right"> The second BitmapSize to compare. </param>
        /// <returns> Return **true** if left and right are equal, otherwise **false**. </returns>
        public static bool operator ==(CanvasTransformer left, BitmapSize right) => left.Equals(right);

        /// <summary>
        /// Returns a boolean indicating whether the two given size are not equal.
        /// </summary>
        /// <param name="left"> The first CanvasTransformer to compare. </param>
        /// <param name="right"> The second BitmapSize to compare. </param>
        /// <returns> Return **true** if the nodes are not equal; False if they are equal. </returns>
        public static bool operator !=(CanvasTransformer left, BitmapSize right) => !left.Equals(right);

    }
}