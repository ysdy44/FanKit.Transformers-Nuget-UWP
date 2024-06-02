using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Snapping tool for <see cref="Vector2"/> and <see cref="TransformerBorder"/>.
    /// </summary>
    public class VectorBorderSnap : VectorSnapBase<TransformerBorder>
    {

        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="pointX"> The point X. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsXSnap"/></returns>
        protected override bool SetXSnap(float pointX, TransformerBorder destination)
        {
            // X Snap
            float gapLeft = pointX - destination.Left;
            if (System.Math.Abs(gapLeft) < base.NodeRadius)
            {
                base.XSnap = destination.Left;
                return true;
            }


            // X Snap: Center
            float gapCenter = pointX - destination.CenterX;
            if (System.Math.Abs(gapCenter) < base.NodeRadius)
            {
                base.XSnap = destination.CenterX;
                return true;
            }


            // X Snap: Right
            float gapRight = pointX - destination.Right;
            if (System.Math.Abs(gapRight) < base.NodeRadius)
            {
                base.XSnap = destination.Right;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.YSnap"/>.
        /// </summary>
        /// <param name="pointY"> The point Y. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsYSnap"/></returns>
        protected override bool SetYSnap(float pointY, TransformerBorder destination)
        {
            // Y Snap: Top
            float gapTop = pointY - destination.Top;
            if (System.Math.Abs(gapTop) < base.NodeRadius)
            {
                base.YSnap = destination.Top;
                return true;
            }


            // Y Snap: Center
            float gapCenter = pointY - destination.CenterY;
            if (System.Math.Abs(gapCenter) < base.NodeRadius)
            {
                base.YSnap = destination.CenterY;
                return true;
            }


            // Y Snap: Bottom
            float gapBottom = pointY - destination.Bottom;
            if (System.Math.Abs(gapBottom) < base.NodeRadius)
            {
                base.YSnap = destination.Bottom;
                return true;
            }

            return false;
        }


        /// <summary> X-axis top. </summary>
        protected override float XTop() => System.Math.Min(base.Source.Y, base.XDestination.Top);
        /// <summary> X-axis bottom. </summary>
        protected override float XBottom() => System.Math.Max(base.Source.Y, base.XDestination.Bottom);

        /// <summary> Y-axis left. </summary>
        protected override float YLeft() => System.Math.Min(base.Source.X, base.YDestination.Left);
        /// <summary> Y-axis right. </summary>
        protected override float YRight() => System.Math.Max(base.Source.X, base.YDestination.Right);

    }
}