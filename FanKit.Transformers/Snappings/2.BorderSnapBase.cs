using System.Numerics;

namespace FanKit.Transformers
{
    public abstract class BorderSnapBase<D> : SnapBase<TransformerBorder, D>
    {
        //Source Destination
        public TransformerBorder StartingSource { get; set; }

        //X
        protected float XGap = 0;
        //Y
        protected float YGap = 0;


        public Vector2 Snap(Vector2 move)
        {
            this.SetSnapMove(move);

            Vector2 snapMove = this.GetSnapMove();
           return move - snapMove;
        }


        public void SetSnapMove(Vector2 canvasMove)
        {
            if (this.Destinations == null) return;


            this.XGap = 0;
            this.XSnap = 0;
            foreach (D destination in this.Destinations)
            {
                this.XDestination = destination;

                this.IsXSnap = this.SetXSnapMove(canvasMove.X, destination);
                if (this.IsXSnap) break;
            }


            this.YGap = 0;
            this.YSnap = 0;
            foreach (D destination in this.Destinations)
            {
                this.YDestination = destination;

                this.IsYSnap = this.SetYSnapMove(canvasMove.Y, destination);
                if (this.IsYSnap) break;
            }
        }
        public Vector2 GetSnapMove()
        {
            if (this.IsXSnap && this.IsYSnap)
                return new Vector2(this.XGap, this.YGap);

            if (this.IsXSnap)
                return new Vector2(this.XGap, 0);

            if (this.IsYSnap)
                return new Vector2(0, this.YGap);

            return Vector2.Zero;
        }


        protected abstract bool SetXSnapMove(float canvasMoveX, D destination);
        protected abstract bool SetYSnapMove(float canvasMoveY, D destination);

    }
}