using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// A interface provides vectors at LeftTop RightTop RightBottom LeftBottom. 
    /// </summary>
    public interface ITransformerLTRB
    {
        /// <summary> Gets the position of the top-left corner. </summary>
        Vector2 LeftTop { get; }
        /// <summary> Gets the position of the top-right corner. </summary>
        Vector2 RightTop { get; }
        /// <summary> Gets the position of the bottom-right corner. </summary>
        Vector2 RightBottom { get; }
        /// <summary> Gets the position of the bottom-left corner. </summary>
        Vector2 LeftBottom { get; }

        /// <summary>  Gets the X-axis value of the center. </summary>
        float CenterX { get; }
        /// <summary>  Gets the Y-axis position of the center. </summary>
        float CenterY { get; }
        /// <summary> Gets the position of the center. </summary>
        Vector2 Center { get; }

        /// <summary> Gets the position of the center of bottom-left and top-left corners. </summary>
        Vector2 CenterLeft { get; }
        /// <summary> Gets the position of the center of top-left and top-right corners. </summary>
        Vector2 CenterTop { get; }
        /// <summary> Gets the position of the center of top-right and bottom-right corners. </summary>
        Vector2 CenterRight { get; }
        /// <summary> Gets the position of the center of bottom-right and bottom-left corners. </summary>
        Vector2 CenterBottom { get; }

        /// <summary> Gets the horizontal vector. </summary>
        Vector2 Horizontal { get; }
        /// <summary> Gets the vertical vector. </summary>
        Vector2 Vertical { get; }
    }
}