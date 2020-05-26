using Microsoft.Graphics.Canvas;
using System.Numerics;
using Windows.UI;

namespace FanKit.Transformers
{

    public abstract class VectorSnapBase<D> : SnapBase<Vector2, D>
    {

        public Vector2 Snap(Vector2 point)
        {
            this.SetSnap(point);
            Vector2 snapPoint = this.GetSnap(point);
            this.Source = snapPoint;
            return snapPoint;
        }


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


        protected abstract bool SetXSnap(float pointX, D destination);
        protected abstract bool SetYSnap(float pointY, D destination);


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