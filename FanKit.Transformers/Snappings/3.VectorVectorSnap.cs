using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Snapping tool for <see cref="Vector2"/>.
    /// </summary>
    public class VectorVectorSnap : VectorSnapBase<Vector2>
    {

        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="pointX"> The point X. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsXSnap"/></returns>
        protected override bool SetXSnap(float pointX, Vector2 destination)
        {
            // X Snap
            float gap = pointX - destination.X;
            if (System.Math.Abs(gap) < base.NodeRadius)
            {
                base.XSnap = destination.X;
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
        protected override bool SetYSnap(float pointY, Vector2 destination)
        {
            // Y Snap
            float gap = pointY - destination.Y;
            if (System.Math.Abs(gap) < base.NodeRadius)
            {
                base.YSnap = destination.Y;
                return true;
            }

            return false;
        }


        /// <summary> X-axis top. </summary>
        protected override float XTop() => System.Math.Min(base.Source.Y, base.XDestination.Y);
        /// <summary> X-axis bottom. </summary>
        protected override float XBottom() => System.Math.Max(base.Source.Y, base.XDestination.Y);

        /// <summary> Y-axis left. </summary>
        protected override float YLeft() => System.Math.Min(base.Source.X, base.YDestination.X);
        /// <summary> Y-axis right. </summary>
        protected override float YRight() => System.Math.Max(base.Source.X, base.YDestination.X);
                     
    }
}