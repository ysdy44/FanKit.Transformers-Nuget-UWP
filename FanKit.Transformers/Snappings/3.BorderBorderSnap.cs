namespace FanKit.Transformers
{
    /// <summary>
    /// Snapping tool for <see cref="TransformerBorder"/>.
    /// </summary>
    public class BorderBorderSnap : BorderSnapBase<TransformerBorder>
    {

        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="moveX"> The move X. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsXSnap"/></returns>
        protected override bool SetXSnapMove(float moveX, TransformerBorder destination)
        {
            // X Snap: Left
            float gapLeftLeft = (base.StartingSource.Left + moveX) - destination.Left;
            if (System.Math.Abs(gapLeftLeft) < base.NodeRadius)
            {
                base.XSnap = destination.Left;
                base.XGap = gapLeftLeft;
                return true;
            }
            float gapCenterLeft = (base.StartingSource.CenterX + moveX) - destination.Left;
            if (System.Math.Abs(gapCenterLeft) < base.NodeRadius)
            {
                base.XSnap = destination.Left;
                base.XGap = gapCenterLeft;
                return true;
            }
            float gapRightLeft = (base.StartingSource.Right + moveX) - destination.Left;
            if (System.Math.Abs(gapRightLeft) < base.NodeRadius)
            {
                base.XSnap = destination.Left;
                base.XGap = gapRightLeft;
                return true;
            }


            // X Snap: Center
            float gapLeftCenter = (base.StartingSource.Left + moveX) - destination.CenterX;
            if (System.Math.Abs(gapLeftCenter) < base.NodeRadius)
            {
                base.XSnap = destination.CenterX;
                base.XGap = gapLeftCenter;
                return true;
            }
            float gapCenterCenter = (base.StartingSource.CenterX + moveX) - destination.CenterX;
            if (System.Math.Abs(gapCenterCenter) < base.NodeRadius)
            {
                base.XSnap = destination.CenterX;
                base.XGap = gapCenterCenter;
                return true;
            }
            float gapRightCenter = (base.StartingSource.Right + moveX) - destination.CenterX;
            if (System.Math.Abs(gapRightCenter) < base.NodeRadius)
            {
                base.XSnap = destination.CenterX;
                base.XGap = gapRightCenter;
                return true;
            }


            // X Snap: Right
            float gapLeftRight = (base.StartingSource.Left + moveX) - destination.Right;
            if (System.Math.Abs(gapLeftRight) < base.NodeRadius)
            {
                base.XSnap = destination.Right;
                base.XGap = gapLeftRight;
                return true;
            }
            float gapCenterRight = (base.StartingSource.CenterX + moveX) - destination.Right;
            if (System.Math.Abs(gapCenterRight) < base.NodeRadius)
            {
                base.XSnap = destination.Right;
                base.XGap = gapCenterRight;
                return true;
            }
            float gapRightRight = (base.StartingSource.Right + moveX) - destination.Right;
            if (System.Math.Abs(gapRightRight) < base.NodeRadius)
            {
                base.XSnap = destination.Right;
                base.XGap = gapRightRight;
                return true;
            }

            return false;
        }
        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.YSnap"/>.
        /// </summary>
        /// <param name="moveY"> The move Y. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsYSnap"/></returns>
        protected override bool SetYSnapMove(float moveY, TransformerBorder destination)
        {
            // Y Snap: Top
            float gapTopTop = (base.StartingSource.Top + moveY) - destination.Top;
            if (System.Math.Abs(gapTopTop) < base.NodeRadius)
            {
                base.YSnap = destination.Top;
                base.YGap = gapTopTop;
                return true;
            }
            float gapCenterTop = (base.StartingSource.CenterY + moveY) - destination.Top;
            if (System.Math.Abs(gapCenterTop) < base.NodeRadius)
            {
                base.YSnap = destination.Top;
                base.YGap = gapCenterTop;
                return true;
            }
            float gapBottomTop = (base.StartingSource.Bottom + moveY) - destination.Top;
            if (System.Math.Abs(gapBottomTop) < base.NodeRadius)
            {
                base.YSnap = destination.Top;
                base.YGap = gapBottomTop;
                return true;
            }


            // Y Snap: Center
            float gapTopCenter = (base.StartingSource.Top + moveY) - destination.CenterY;
            if (System.Math.Abs(gapTopCenter) < base.NodeRadius)
            {
                base.YSnap = destination.CenterY;
                base.YGap = gapTopCenter;
                return true;
            }
            float gapCenterCenter = (base.StartingSource.CenterY + moveY) - destination.CenterY;
            if (System.Math.Abs(gapCenterCenter) < base.NodeRadius)
            {
                base.YSnap = destination.CenterY;
                base.YGap = gapCenterCenter;
                return true;
            }
            float gapBottomCenter = (base.StartingSource.Bottom + moveY) - destination.CenterY;
            if (System.Math.Abs(gapBottomCenter) < base.NodeRadius)
            {
                base.YSnap = destination.CenterY;
                base.YGap = gapBottomCenter;
                return true;
            }


            // Y Snap: Bottom
            float gapTopBottom = (base.StartingSource.Top + moveY) - destination.Bottom;
            if (System.Math.Abs(gapTopBottom) < base.NodeRadius)
            {
                base.YSnap = destination.Bottom;
                base.YGap = gapTopBottom;
                return true;
            }
            float gapCenterBottom = (base.StartingSource.CenterY + moveY) - destination.Bottom;
            if (System.Math.Abs(gapCenterBottom) < base.NodeRadius)
            {
                base.YSnap = destination.Bottom;
                base.YGap = gapCenterBottom;
                return true;
            }
            float gapBottomBottom = (base.StartingSource.Bottom + moveY) - destination.Bottom;
            if (System.Math.Abs(gapBottomBottom) < base.NodeRadius)
            {
                base.YSnap = destination.Bottom;
                base.YGap = gapBottomBottom;
                return true;
            }

            return false;
        }


        /// <summary> X-axis top. </summary>
        protected override float XTop() => System.Math.Min(base.Source.Top, base.XDestination.Top);
        /// <summary> X-axis bottom. </summary>
        protected override float XBottom() => System.Math.Max(base.Source.Bottom, base.XDestination.Bottom);

        /// <summary> Y-axis left. </summary>
        protected override float YLeft() => System.Math.Min(base.Source.Left, base.YDestination.Left);
        /// <summary> Y-axis right. </summary>
        protected override float YRight() => System.Math.Max(base.Source.Right, base.YDestination.Right);

    }
}