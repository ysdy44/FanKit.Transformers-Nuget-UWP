using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// A interface provides vectors at LeftTop RightTop RightBottom LeftBottom. 
    /// </summary>
    public interface ITransformerLTRB
    {
        /// <summary> Vector in LeftTop. </summary>
         Vector2 LeftTop { get; }
        /// <summary> Vector in RightTop. </summary>
        Vector2 RightTop { get; }
        /// <summary> Vector in RightBottom. </summary>
        Vector2 RightBottom { get; }
        /// <summary> Vector in LeftBottom. </summary>
        Vector2 LeftBottom { get; }

        /// <summary> Gets the center vector. </summary>
        Vector2 Center { get; }

        /// <summary> Gets the center left vector. </summary>
        Vector2 CenterLeft { get; }
        /// <summary> Gets the center top vector. </summary>
        Vector2 CenterTop { get; }
        /// <summary> Gets the center right vector. </summary>
        Vector2 CenterRight { get; }
        /// <summary> Gets the center bottom vector. </summary>
        Vector2 CenterBottom { get; }

        /// <summary> Gets horizontal vector. </summary>
        Vector2 Horizontal { get; }
        /// <summary> Gets vertical vector. </summary>
        Vector2 Vertical { get; }
    }
}