using System.Numerics;

namespace FanKit.Transformers
{
    public partial class CanvasTransformer
    {

        //Move
        Vector2 _moveStartPoint;
        Vector2 _moveStartPosition;

        //Pinch
        Vector2 _pinchStartCenter;
        float _pinchStartScale;
        float _pinchStartSpace;


        #region Move


        /// <summary>
        /// Cache data for a Move event.
        /// </summary>
        /// <param name="point"> The point. </param>
        public void CacheMove(Vector2 point)
        {
            this._moveStartPoint = point;
            this._moveStartPosition = this.Position;
        }

        /// <summary>
        /// Move position (CacheMove event occur first).
        /// </summary>
        /// <param name="point"> The point. </param>
        public void Move(Vector2 point)
        {
            this.Position = this._moveStartPosition - this._moveStartPoint + point;
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
            this._pinchStartCenter = (centerPoint - this.Position) / this.Scale + this.ControlCenter;

            this._pinchStartSpace = space;
            this._pinchStartScale = this.Scale;
        }

        /// <summary>
        /// Pinch the screen to change position and scale (CachePinch event occur first).
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="space"> The space between fingers. </param>
        public void Pinch(Vector2 centerPoint, float space)
        {
            this.Scale = this._pinchStartScale / this._pinchStartSpace * space;
            this.Position = centerPoint - (this._pinchStartCenter - this.ControlCenter) * this.Scale;
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
