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
        /// Set the rect of the source.
        /// </summary>
        /// <param name="postion"> The postion of source. </param>
        /// <param name="width"> The width of source. </param>
        /// <param name="height"> The height of source. </param>
        public void TransitionSource(Vector2 postion, float width, float height)
        {
            this._sourcePosition.X = postion.X + width / 2.0f;
            this._sourcePosition.Y = postion.Y + height / 2.0f;
            this._sourceScale = this._getTransitionScale(width, height);
        }
        /// <summary>
        /// Set the rect of the source.
        /// </summary>
        public void TransitionSource(Rect rect) => this.TransitionSource(new Vector2((float)rect.X, (float)rect.Y), (float)rect.Width, (float)rect.Height);


        /// <summary>
        /// Set the rect of the destination.
        /// </summary>
        /// <param name="postion"> The postion of destination. </param>
        /// <param name="width"> The width of destination. </param>
        /// <param name="height"> The height of destination. </param>
        public void TransitionDestination(Vector2 postion, float width, float height)
        {
            this._destinationPosition.X = postion.X + width / 2.0f;
            this._destinationPosition.Y = postion.Y + height / 2.0f;
            this._destinationScale = this._getTransitionScale(width, height);
        }
        /// <summary>
        /// Set the rect of the destination.
        /// </summary>
        public void TransitionDestination(Rect rect) => this.TransitionSource(new Vector2((float)rect.X, (float)rect.Y), (float)rect.Width, (float)rect.Height);



        private float _getTransitionScale(float width, float height)
        {
            float widthScale = width / this.Width;
            float heightScale = height / this.Height;

            float scale = System.Math.Min(widthScale, heightScale);
            return scale;
        }
        
        /// <summary>
        /// Make the matrix transition between source and destination.
        /// </summary>
        /// <param name="value"> That transition value (0.0f is source, 1.0f is destination). </param>
        public void Transition(float value)
        {
            if (value == 0.0f)
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