using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides an interface for the graphics class in the transformation.
    /// </summary>
    public interface ICacheTransform
    {
        /// <summary>
        ///  Cache the class's transformer.
        ///  Ex: _oldTransformer = Transformer.
        /// </summary>
        void CacheTransform();

        /// <summary>
        ///  Transforms the class by the given matrix.
        ///  Ex: Transformer.Multiplies()
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        void TransformMultiplies(Matrix3x2 matrix);

        /// <summary>
        ///  Transforms the class by the given vector.
        ///  Ex: Transformer.Add()
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        void TransformAdd(Vector2 vector);
    }
}