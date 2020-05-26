using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a maximum boundary in a plane right-angle coordinate system,
    /// including the left right top bottom and center.
    /// </summary>
    public partial struct TransformerBorder : ITransformerLTRB
    {

        /// <summary> Gets border's left. </summary>
        public float Left { get; private set; }
        /// <summary> Gets border's top. </summary>
        public float Top { get; private set; }
        /// <summary> Gets border's right. </summary>
        public float Right { get; private set; }
        /// <summary> Gets border's bottom. </summary>
        public float Bottom { get; private set; }

        /// <summary> Gets border's center of X. </summary>
        public float CenterX { get; private set; }
        /// <summary> Gets border's center of Y. </summary>
        public float CenterY { get; private set; }
        /// <summary> Gets rectangle's center point. </summary>
        public Vector2 Center => new Vector2(this.CenterX, this.CenterY);

        /// <summary> Gets the center left vector. </summary>
        public Vector2 CenterLeft => new Vector2(this.Left, this.CenterY);
        /// <summary> Gets the center top vector. </summary>
        public Vector2 CenterTop => new Vector2(this.CenterX, this.Top);
        /// <summary> Gets the center right vector. </summary>
        public Vector2 CenterRight => new Vector2(this.Right, this.CenterY);
        /// <summary> Gets the center bottom vector. </summary>
        public Vector2 CenterBottom => new Vector2(this.CenterX, this.Bottom);

        /// <summary> Gets border's left-top point. </summary>
        public Vector2 LeftTop => new Vector2(this.Left, this.Top);
        /// <summary> Gets border's right-top point. </summary>
        public Vector2 RightTop => new Vector2(this.Right, this.Top);
        /// <summary> Gets border's right-bottom point. </summary>
        public Vector2 RightBottom => new Vector2(this.Right, this.Bottom);
        /// <summary> Gets border's left-bottom point. </summary>
        public Vector2 LeftBottom => new Vector2(this.Left, this.Bottom);
        
        /// <summary> Gets horizontal vector. </summary>
        public Vector2 Horizontal => this.CenterRight - this.CenterLeft;
        /// <summary> Gets vertical vector. </summary>
        public Vector2 Vertical => this.CenterBottom - this.CenterTop;


        /// <summary>
        /// Turn to transformer.
        /// </summary>
        public Transformer ToTransformer() => new Transformer(this.Left, this.Top, this.Right, this.Bottom);
        
    }
}