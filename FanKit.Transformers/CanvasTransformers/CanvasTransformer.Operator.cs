using System.Numerics;

namespace FanKit.Transformers
{
    public partial class CanvasTransformer
    {

        // Move
        Vector2 MoveStartPoint;
        Vector2 MoveStartPosition;

        // Pinch
        Vector2 PinchStartCenter;
        float PinchStartScale;
        float PinchStartSpace;


        #region Move


        /// <summary>
        /// Cache data for a Move event.
        /// </summary>
        /// <param name="point"> The point. </param>
        public void CacheMove(Vector2 point)
        {
            this.MoveStartPoint = point;
            this.MoveStartPosition = this.Position;
        }

        /// <summary>
        /// Move position (CacheMove event occur first).
        /// </summary>
        /// <param name="point"> The point. </param>
        public void Move(Vector2 point)
        {
            this.Position = this.MoveStartPosition - this.MoveStartPoint + point;
            this.ReloadMatrix();
        }


        #endregion


        #region Pinch


        /// <summary>
        /// Cache data for a Pinch event.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="space"> The space between fingers. </param>
        public void CachePinch(Vector2 centerPoint, float space)
        {
            this.PinchStartCenter = (centerPoint - this.Position) / this.Scale + this.ControlCenter;

            this.PinchStartSpace = space;
            this.PinchStartScale = this.Scale;
        }

        /// <summary>
        /// Pinch the screen to change position and scale (CachePinch event occur first).
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="space"> The space between fingers. </param>
        public void Pinch(Vector2 centerPoint, float space)
        {
            this.Scale = this.PinchStartScale / this.PinchStartSpace * space;
            this.Position = centerPoint - (this.PinchStartCenter - this.ControlCenter) * this.Scale;
            this.ReloadMatrix();
        }


        #endregion


        #region ZoomIn ZoomOut


        /// <summary>
        /// To manipulate a display so as to make the image smaller.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="zoomInScale"> The scale. </param>
        /// <param name="maximum">The maximum scale. </param>
        public void ZoomIn(Vector2 centerPoint, float zoomInScale = 1.1f, float maximum = 10f)
        {
            if (this.Scale < maximum)
            {
                this.Scale *= zoomInScale;
                this.Position = centerPoint + (this.Position - centerPoint) * zoomInScale;
            }
            this.ReloadMatrix();
        }

        /// <summary>
        ///  To manipulate a display so as to make the image larger.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="zoomOutScale"> The scale. </param>
        /// <param name="minimum">The minimum scale. </param>
        public void ZoomOut(Vector2 centerPoint, float zoomOutScale = 1.1f, float minimum = 0.1f)
        {
            if (this.Scale > minimum)
            {
                this.Scale /= zoomOutScale;
                this.Position = centerPoint + (this.Position - centerPoint) / zoomOutScale;
            }
            this.ReloadMatrix();
        }


        #endregion

    }
}
