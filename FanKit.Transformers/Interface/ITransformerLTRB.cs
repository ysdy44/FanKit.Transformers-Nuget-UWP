using System.Numerics;

namespace FanKit.Transformers
{
    public interface ITransformerLTRB
    {
        float CenterX { get; }
        float CenterY { get; }
        Vector2 Center { get; }

        Vector2 CenterLeft { get; }
        Vector2 CenterTop { get; }
        Vector2 CenterRight { get; }
        Vector2 CenterBottom { get; }

        Vector2 Horizontal { get; }
        Vector2 Vertical { get; }
    }
}