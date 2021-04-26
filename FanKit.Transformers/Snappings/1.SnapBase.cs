using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System.Numerics;
using Windows.UI;

namespace FanKit.Transformers
{
    /// <summary>
    /// Base of snapping tool.
    /// </summary>
    /// <typeparam name="S"> The source type. </typeparam>
    /// <typeparam name="D"> The destination type. </typeparam>
    public abstract class SnapBase<S, D>
    {
        //NodeRadius
        /// <summary> Radius of node. Default 12. </summary>
        public float NodeRadius { get; set; } = FanKit.Math.NodeRadius;

        //Source Destination
        /// <summary> Gets or sets the source. </summary>
        public S Source { get; set; }
        /// <summary> Gets or sets the destinations. </summary>
        public IEnumerable<D> Destinations { get; set; }

        //X
        /// <summary> Whether to snap to the X-axis.  </summary>
        public bool IsXSnap;
        /// <summary> X-axis position. </summary>
        public float XSnap = 0;
        /// <summary> X-axis destination. </summary>
        protected D XDestination;
        //Y
        /// <summary> Whether to snap to the Y-axis.  </summary>
        public bool IsYSnap;
        /// <summary> Y-axis position. </summary>
        public float YSnap = 0;
        /// <summary> Y-axis destination. </summary>
        protected D YDestination;


        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
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
        /// <summary>
        /// Occurs when the canvas is drawn.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="matrix"> The matrix. </param>
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

        /// <summary> X-axis top. </summary>
        protected abstract float XTop();
        /// <summary> X-axis bottom. </summary>
        protected abstract float XBottom();

        /// <summary> Y-axis left. </summary>
        protected abstract float YLeft();
        /// <summary> Y-axis right. </summary>
        protected abstract float YRight();


        /// <summary>
        /// Default
        /// </summary>
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