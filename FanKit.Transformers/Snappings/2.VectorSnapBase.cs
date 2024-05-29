using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI;

namespace FanKit.Transformers
{
    /// <summary>
    /// Snapping tool for <see cref="Vector2"/>.
    /// </summary>
    /// <typeparam name="D"> The destination type. </typeparam>
    public abstract class VectorSnapBase<D> : SnapBase<Vector2, D>
    {

        /// <summary>
        /// Snapping
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <returns> The product vector. </returns>
        public Vector2 Snap(Vector2 point)
        {
            this.SetSnap(point);
            Vector2 snapPoint = this.GetSnap(point);
            this.Source = snapPoint;
            return snapPoint;
        }


        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/> and <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="point"> The point. </param>
        public void SetSnap(Vector2 point)
        {
            if (this.Destinations == null) return;

            this.XSnap = 0;
            foreach (D destination in this.Destinations)
            {
                this.XDestination = destination;

                this.IsXSnap = this.SetXSnap(point.X, destination);
                if (this.IsXSnap) break;
            }


            this.YSnap = 0;
            foreach (D destination in this.Destinations)
            {
                this.YDestination = destination;

                this.IsYSnap = this.SetYSnap(point.Y, destination);
                if (this.IsYSnap) break;
            }
        }
        /// <summary>
        /// Gets <see cref="SnapBase{S, D}.XSnap"/> and <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <returns> The product vector. </returns>
        public Vector2 GetSnap(Vector2 point)
        {
            if (this.IsXSnap && this.IsYSnap)
                return new Vector2(this.XSnap, this.YSnap);

            if (this.IsXSnap)
                return new Vector2(this.XSnap, point.Y);

            if (this.IsYSnap)
                return new Vector2(point.X, this.YSnap);

            return point;
        }

        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="pointX"> The point X. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsXSnap"/></returns>
        protected abstract bool SetXSnap(float pointX, D destination);
        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.YSnap"/>.
        /// </summary>
        /// <param name="pointY"> The point Y. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsYSnap"/></returns>
        protected abstract bool SetYSnap(float pointY, D destination);


        /// <summary>
        /// Draw a ⊙.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
        public void DrawNode2(CanvasDrawingSession drawingSession, Matrix3x2 matrix)
        {
            if (base.IsXSnap && base.IsYSnap)
            {
                drawingSession.DrawNode2(Vector2.Transform(base.Source, matrix), Colors.Gold);
            }
            else if (base.IsXSnap)
            {
                drawingSession.DrawNode2(Vector2.Transform(base.Source, matrix), Colors.Red);
            }
            else if (base.IsYSnap)
            {
                drawingSession.DrawNode2(Vector2.Transform(base.Source, matrix), Colors.LimeGreen);
            }
        }

    }
}