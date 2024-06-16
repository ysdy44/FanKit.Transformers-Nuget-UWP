namespace FanKit.Transformers
{
    internal interface ITransformerWH
    {
        float Left { get; }
        float Right { get; }
        float Top { get; }
        float Bottom { get; }

        float Height { get; }
        float Width { get; }
    }
}