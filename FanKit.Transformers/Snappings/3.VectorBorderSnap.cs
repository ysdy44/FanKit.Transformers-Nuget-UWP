namespace FanKit.Transformers
{

    public class VectorBorderSnap : VectorSnapBase<TransformerBorder>
    {

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


        protected override float XTop() => System.Math.Min(base.Source.Y, base.XDestination.Top);
        protected override float XBottom() => System.Math.Max(base.Source.Y, base.XDestination.Bottom);

        protected override float YLeft() => System.Math.Min(base.Source.X, base.YDestination.Left);
        protected override float YRight() => System.Math.Max(base.Source.X, base.YDestination.Right);
        
    }
}