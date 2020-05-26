using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;

namespace FanKit.Transformers
{

    public abstract class SnapBase<S, D>
    {
        //NodeRadius
        public float NodeRadius { get; set; } = FanKit.Math.NodeRadius;

        //Source Destination
        public S Source { get; set; }
        public IEnumerable<D> Destinations { get; set; }

        //X
        public bool IsXSnap;
        public float XSnap = 0;
        protected D XDestination;
        //Y
        public bool IsYSnap;
        public float YSnap = 0;
        protected D YDestination;


        public void Draw(CanvasDrawingSession drawingSession)
        {
            if (this.IsXSnap)
            {
                drawingSession.DrawLine
                (
                    point0: new Vector2(this.XSnap, this.XTop()),
                    point1: new Vector2(this.XSnap, this.XBottom()),
                    color: Colors.Red
                );
            }

            if (this.IsYSnap)
            {
                drawingSession.DrawLine
                (
                    point0: new Vector2(this.YLeft(), this.YSnap),
                    point1: new Vector2(this.YRight(), this.YSnap),
                    color: Colors.LimeGreen
                );
            }
        }
        public void Draw(CanvasDrawingSession drawingSession, Matrix3x2 matrix)
        {
            if (this.IsXSnap)
            {
                drawingSession.DrawLine
                (
                    point0: Vector2.Transform(new Vector2(this.XSnap, this.XTop()), matrix),
                    point1: Vector2.Transform(new Vector2(this.XSnap, this.XBottom()), matrix),
                    color: Colors.Red
                );
            }

            if (this.IsYSnap)
            {
                drawingSession.DrawLine
                (
                    point0: Vector2.Transform(new Vector2(this.YLeft(), this.YSnap), matrix),
                    point1: Vector2.Transform(new Vector2(this.YRight(), this.YSnap), matrix),
                    color: Colors.LimeGreen
                );
            }
        }

        protected abstract float XTop();
        protected abstract float XBottom();

        protected abstract float YLeft();
        protected abstract float YRight();


        public void Default()
        {
            this.NodeRadius = FanKit.Math.NodeRadius;

            //X
            this.IsXSnap = false;
            this.XSnap = 0;

            //Y
            this.IsYSnap = false;
            this.YSnap = 0;
        }
    }

}