using System.Numerics;

namespace FanKit.Transformers
{

    public class VectorVectorSnap : VectorSnapBase<Vector2>
    {

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


        protected override float XTop() => System.Math.Min(base.Source.Y, base.XDestination.Y);
        protected override float XBottom() => System.Math.Max(base.Source.Y, base.XDestination.Y);

        protected override float YLeft() => System.Math.Min(base.Source.X, base.YDestination.X);
        protected override float YRight() => System.Math.Max(base.Source.X, base.YDestination.X);

    }
}