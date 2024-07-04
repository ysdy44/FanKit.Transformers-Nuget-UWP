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

        /// <summary> Gets or sets the width of canvas. </summary>
        public int Width { get; set; } = 1000;
        /// <summary> Gets or sets the height of canvas. </summary>
        public int Height { get; set; } = 1000;

        /// <summary> Gets or sets the scalar value of canvas. </summary>
        public float Scale { get; set; } = 1.0f;

        /// <summary> Gets or sets the width of control. </summary>
        public float ControlWidth { get; set; } = 1000.0f;
        /// <summary> Gets or sets the height of control. </summary>
        public float ControlHeight { get; set; } = 1000.0f;
        /// <summary> Gets the position of center of control. </summary>
        public Vector2 ControlCenter => new Vector2(this.ControlWidth / 2, this.ControlHeight / 2);

        /// <summary> Gets or sets the translation component of this canvas. </summary>
        public Vector2 Position { get; set; } = new Vector2(0.0f, 0.0f);
        /// <summary> Gets or sets the rotation value of canvas. </summary>
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


        /// <summary> Gets the size of control. </summary>
        public Size Size
        {
            set
            {
                this.ControlWidth = (float)value.Width;
                this.ControlHeight = (float)value.Height;
            }
        }
        /// <summary> Gets the size of canvas. </summary>
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
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
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

    }
}