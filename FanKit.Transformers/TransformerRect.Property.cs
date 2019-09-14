using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four range values (Left, Top, Right, Bottom). 
    /// </summary>
    public partial struct TransformerRect
    {

        /// <summary> Gets rectangle's left. </summary>
        public float Left { get; private set; }
        /// <summary> Gets rectangle's top. </summary>
        public float Top { get; private set; }
        /// <summary> Gets rectangle's right. </summary>
        public float Right { get; private set; }
        /// <summary> Gets rectangle's bottom. </summary>
        public float Bottom { get; private set; }

        /// <summary> Gets rectangle's width. </summary>
        public float Width { get; private set; }
        /// <summary> Gets rectangle's height. </summary>
        public float Height { get; private set; }
        /// <summary> Gets rectangle's center point. </summary>
        public Vector2 Center { get; private set; }
        /// <summary> Gets rectangle's center of X. </summary>
        public float CenterX { get; private set; }
        /// <summary> Gets rectangle's center of Y. </summary>
        public float CenterY { get; private set; }

        /// <summary> Gets rectangle's left-top point. </summary>
        public Vector2 LeftTop { get; private set; }
        /// <summary> Gets rectangle's right-top point. </summary>
        public Vector2 RightTop { get; private set; }
        /// <summary> Gets rectangle's right-bottom point. </summary>
        public Vector2 RightBottom { get; private set; }
        /// <summary> Gets rectangle's left-bottom point. </summary>
        public Vector2 LeftBottom { get; private set; }
        
    }
}