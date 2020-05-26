 using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

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

        /// <summary> <see cref = "CanvasTransformer" />'s width. </summary>
        public int Width = 1000;
        /// <summary> <see cref = "CanvasTransformer" />'s height. </summary>
        public int Height = 1000;

        /// <summary> <see cref = "CanvasTransformer" />'s scale. </summary>
        public float Scale = 1.0f;

        /// <summary> CanvasControl's width. </summary>
        public float ControlWidth { get; set; } = 1000.0f;
        /// <summary> CanvasControl's height. </summary>
        public float ControlHeight { get; set; } = 1000.0f;
        /// <summary> CanvasControl's center. </summary>
        public Vector2 ControlCenter => new Vector2(this.ControlWidth / 2, this.ControlHeight / 2);

        /// <summary> <see cref = "CanvasTransformer" />'s translation. </summary>
        public Vector2 Position = new Vector2(0.0f, 0.0f);
        /// <summary> <see cref = "CanvasTransformer" />'s rotation. </summary>
        public float Radian = 0.0f;


        //@Construct
        /// <summary>
        /// Initialize a <see cref = "CanvasTransformer" />.
        /// </summary>
        public CanvasTransformer()
        {
            this.ReloadMatrix();
        }


        #region Size
        

        /// <summary> <see cref = "CanvasTransformer.ControlWidth" /> and <see cref = "CanvasTransformer.ControlHeight" />'s setter. </summary>
        public Size Size
        {
            get => this.size;
            set
            {
                this.ControlWidth = (float)size.Width;
                this.ControlHeight =  (float)size.Height;
                this.size = value;
            }
        }
        private Size size;

        /// <summary> 
        /// Fit to the screen. 
        /// </summary>
        public void Fit()
        {
            float widthScale = this.ControlWidth / this.Width;
            float heightScale = this.ControlHeight / this.Height;

            this.Scale = System.Math.Min(widthScale, heightScale);

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

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

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }


        #endregion

    }
}