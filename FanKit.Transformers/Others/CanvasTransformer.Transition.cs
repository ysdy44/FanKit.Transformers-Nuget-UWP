using System.Numerics;
using Windows.Foundation;

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


        Vector2 _sourcePosition = new Vector2(100, 100);
        float _sourceScale = 0.2f;

        Vector2 _destinationPosition;
        float _destinationScale;

        /// <summary>
        /// Set the size of the source thumbnail.
        /// </summary>
        /// <param name="postion"> The postion of source. </param>
        /// <param name="actualWidth"> The width of source. </param>
        /// <param name="actualHeight"> The height of source. </param>
        public void TransitionSource(Point postion, double actualWidth, double actualHeight)
        {
            float left = (float)postion.X;
            float top = (float)postion.Y;
            float width = (float)actualWidth;
            float height = (float)actualHeight;

            this._sourcePosition = new Vector2(left + width / 2.0f, top + height / 2.0f);
            this._sourceScale = this._getTransitionScale(width, height);
        }
        private float _getTransitionScale(float width, float height)
        {
            float widthScale = width / this.Width;
            float heightScale = height / this.Height;

            float scale = (widthScale + heightScale) / 2;
            return scale;
        }

        /// <summary>
        /// Set the size of the destination canvas.
        /// </summary>
        public void TransitionDestination()
        {
            this._destinationPosition.X = this.ControlWidth / 2.0f;
            this._destinationPosition.Y = this.ControlHeight / 2.0f;
            this._destinationScale = this._getTransitionFitScale();
        }
        private float _getTransitionFitScale()
        {
            float widthScale = this.ControlWidth / this.Width;
            float heightScale = this.ControlHeight / this.Height;

            float scale = System.Math.Min(widthScale, heightScale);
            return scale;
        }

        /// <summary>
        /// Make the matrix transition between source and destination.
        /// </summary>
        /// <param name="value"> That transition value (0.0f is source, 1.0f is destination). </param>
        public void Transition(float value)
        {
            if (value==0.0f)
            {
                this.Position = this._sourcePosition;
                this.Scale = this._sourceScale;
            }
            else if (value == 1.0f)
            {
                this.Position = this._destinationPosition;
                this.Scale = this._destinationScale;
            }
            else
            {
                float minusValue = 1.0f - value;

                Vector2 position = this._sourcePosition * minusValue + this._destinationPosition * value;
                this.Position = position;

                float scale = this._sourceScale * minusValue + this._destinationScale * value;
                this.Scale = scale;
            }

            this.ReloadMatrix();
        }

    }
}