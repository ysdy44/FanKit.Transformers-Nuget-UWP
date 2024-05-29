using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Snapping tool for <see cref="TransformerBorder"/>.
    /// </summary>
    /// <typeparam name="D"> The destination type. </typeparam>
    public abstract class BorderSnapBase<D> : SnapBase<TransformerBorder, D>
    {
        //Source Destination
        /// <summary> The cache of <see cref="SnapBase{S, D}.Source"/>. </summary>
        public TransformerBorder StartingSource { get; set; }

        //X
        /// <summary> X-axis gap. </summary>
        protected float XGap = 0;
        //Y
        /// <summary> Y-axis gap. </summary>
        protected float YGap = 0;

        /// <summary>
        /// Snapping
        /// </summary>
        /// <param name="move"> The move vector. </param>
        /// <returns> The product vector. </returns>
        public Vector2 Snap(Vector2 move)
        {
            this.SetSnapMove(move);

            Vector2 snapMove = this.GetSnapMove();
            return move - snapMove;
        }

        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/> and <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="move"> The move vector. </param>
        public void SetSnapMove(Vector2 move)
        {
            if (this.Destinations == null) return;


            this.XGap = 0;
            this.XSnap = 0;
            foreach (D destination in this.Destinations)
            {
                this.XDestination = destination;

                this.IsXSnap = this.SetXSnapMove(move.X, destination);
                if (this.IsXSnap) break;
            }


            this.YGap = 0;
            this.YSnap = 0;
            foreach (D destination in this.Destinations)
            {
                this.YDestination = destination;

                this.IsYSnap = this.SetYSnapMove(move.Y, destination);
                if (this.IsYSnap) break;
            }
        }
        /// <summary>
        /// Gets <see cref="SnapBase{S, D}.XSnap"/> and <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <returns> The product vector. </returns>
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

        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.XSnap"/>.
        /// </summary>
        /// <param name="moveX"> The move X. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsXSnap"/></returns>
        protected abstract bool SetXSnapMove(float moveX, D destination);
        /// <summary>
        /// Sets <see cref="SnapBase{S, D}.YSnap"/>.
        /// </summary>
        /// <param name="moveY"> The move Y. </param>
        /// <param name="destination"> The destination. </param>
        /// <returns> The <see cref="SnapBase{S, D}.IsYSnap"/></returns>
        protected abstract bool SetYSnapMove(float moveY, D destination);

    }
}