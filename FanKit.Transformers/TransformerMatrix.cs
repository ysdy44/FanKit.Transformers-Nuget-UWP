using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// A structure encapsulating two transformer values  (Source and Destination). 
    /// </summary>
    public struct TransformerMatrix: ICacheTransform
    {
        /// <summary> The source Transformer. </summary>
        public Transformer Source;

        /// <summary> The destination Transformer. </summary>
        public Transformer Destination;

        /// <summary> <see cref = "TransformerMatrix.Destination" />'s old cache. </summary>
        public Transformer OldDestination;

        /// <summary> Is disable rotate radian? Defult **false**. </summary>
        public bool DisabledRadian;


        //@Constructs
        /// <summary> 
        /// Constructs a <see cref = "TransformerMatrix" />. 
        /// </summary>
        public TransformerMatrix(Transformer transformer)
        {
            //Source
            this.Source = transformer;

            //Destination
            this.Destination = transformer;

            this.OldDestination = transformer;

            this.DisabledRadian = false;
        }
        /// <summary>
        /// Constructs a <see cref = "TransformerMatrix" />.
        /// </summary>
        /// <param name="pointA"> The frist point of transformer matrix. </param>
        /// <param name="pointB"> The second point of transformer matrix. </param>
        public TransformerMatrix(Vector2 pointA, Vector2 pointB)
        {
            Transformer transformer = new Transformer(pointA, pointB);

            //Source
            this.Source = transformer;

            //Destination
            this.Destination = transformer;

            this.OldDestination = transformer;

            this.DisabledRadian = false;
        }
        /// <summary>
        /// Constructs a <see cref = "TransformerMatrix" />. 
        /// </summary> 
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        /// <param name="postion"> The postion. </param>
        public TransformerMatrix(float width, float height, Vector2 postion)
        {
            Transformer transformer = new Transformer(width, height,postion);

            //Source
            this.Source = transformer;

            //Destination
            this.Destination = transformer;

            this.OldDestination = transformer;

            this.DisabledRadian = false;
        }


        /// <summary>
        /// Gets transformer-matrix>'s resulting matrix.
        /// </summary>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);


        //@Override
        /// <summary>
        ///  Cache the transformer-matrix's transformer.
        /// </summary>
        public void CacheTransform() => this.OldDestination = this.Destination;
        /// <summary>
        ///  Transforms the TransformerMatrix by the given matrix.
        /// </summary>
        /// <param name="matrix"> The resulting matrix. </param>
        public void TransformMultiplies(Matrix3x2 matrix) => this.Destination = this.OldDestination * matrix;

        /// <summary>
        ///  Transforms the TransformerMatrix by the given vector.
        /// </summary>
        /// <param name="vector"> The add value use to summed. </param>
        public void TransformAdd(Vector2 vector) => this.Destination = this.OldDestination + vector;
    }
}