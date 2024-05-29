using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    partial class CanvasTransformer
    {

        Vector2 SourcePosition = new Vector2(100, 100);
        float SourceScale = 0.2f;

        Vector2 DestinationPosition;
        float DestinationScale;


        /// <summary>
        /// Set the rect of the source.
        /// </summary>
        /// <param name="postion"> The postion of source. </param>
        /// <param name="width"> The width of source. </param>
        /// <param name="height"> The height of source. </param>
        public void TransitionSource(Vector2 postion, float width, float height)
        {
            this.SourcePosition.X = postion.X + width / 2.0f;
            this.SourcePosition.Y = postion.Y + height / 2.0f;
            this.SourceScale = this.GetTransitionScale(width, height);
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
            this.DestinationPosition.X = postion.X + width / 2.0f;
            this.DestinationPosition.Y = postion.Y + height / 2.0f;
            this.DestinationScale = this.GetTransitionScale(width, height);
        }
        /// <summary>
        /// Set the rect of the destination.
        /// </summary>
        public void TransitionDestination(Rect rect) => this.TransitionSource(new Vector2((float)rect.X, (float)rect.Y), (float)rect.Width, (float)rect.Height);



        private float GetTransitionScale(float width, float height)
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
                this.Position = this.SourcePosition;
                this.Scale = this.SourceScale;
            }
            else if (value == 1.0f)
            {
                this.Position = this.DestinationPosition;
                this.Scale = this.DestinationScale;
            }
            else
            {
                float minusValue = 1.0f - value;

                Vector2 position = this.SourcePosition * minusValue + this.DestinationPosition * value;
                this.Position = position;

                float scale = this.SourceScale * minusValue + this.DestinationScale * value;
                this.Scale = scale;
            }

            this.ReloadMatrix();
        }


    }
}