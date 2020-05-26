using System.Collections.Generic;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides a static method for converting to geometry.
    /// </summary>
    public static partial class TransformerGeometry
    {
               
        #region Pentagon


        /// <summary>
        /// Convert to curves from pentagon.
        /// </summary>
        /// <param name="points"> The points count. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromPentagon(ITransformerLTRB transformer, int points)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromPentagon(points, oneMatrix)
            };
        }

        /// <summary>
        /// Convert to curves from pentagon.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="points"> The points count. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromPentagon(ITransformerLTRB transformer, Matrix3x2 matrix, int points)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromPentagon(points, oneMatrix2)
            };
        }

        private static IEnumerable<Node> _convertToCurveFromPentagon(int points, Matrix3x2 oneMatrix)
        {
            float rotation = TransformerGeometry.StartingRotation;
            float angle = FanKit.Math.Pi * 2.0f / points;

            List<Node> nodes = new List<Node>();
            for (int i = 0; i < points; i++)
            {
                int index = i;

                //Outer
                Vector2 outer = TransformerGeometry.GetRotationVector(rotation);
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                nodes.Add(new Node { Point = outerTransform });
                rotation += angle;
            }
            {
                Vector2 outer = TransformerGeometry.GetRotationVector(TransformerGeometry.StartingRotation);
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                nodes.Add(new Node { Point = outerTransform });
            }
            return nodes;
        }


        #endregion


        #region Star


        /// <summary>
        /// Convert to curves from star.
        /// </summary>
        /// <param name="points"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromStar(ITransformerLTRB transformer, int points, float innerRadius)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            
            return new List<IEnumerable<Node>>
            {
                 TransformerGeometry._convertToCurveFromStar(points, innerRadius, oneMatrix)
            };
        }

        /// <summary>
        /// Convert to curves from star.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="points"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromStar(ITransformerLTRB transformer, Matrix3x2 matrix, int points, float innerRadius)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromStar(points, innerRadius, oneMatrix2)
            };
        }

        private static IEnumerable<Node> _convertToCurveFromStar(int points, float innerRadius, Matrix3x2 oneMatrix)
        {
            float rotation = TransformerGeometry.StartingRotation;
            float angle = FanKit.Math.Pi / points;

            List<Node> nodes = new List<Node>();
            for (int i = 0; i < points; i++)
            {
                int index = i * 2;

                //Outer
                Vector2 outer = TransformerGeometry.GetRotationVector(rotation);
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                nodes.Add(new Node { Point = outerTransform });
                rotation += angle;

                //Inner
                Vector2 inner = TransformerGeometry.GetRotationVector(rotation);
                Vector2 inner2 = inner * innerRadius;
                Vector2 inner2Transform = Vector2.Transform(inner2, oneMatrix);
                nodes.Add(new Node { Point = inner2Transform });
                rotation += angle;
            }
            {
                Vector2 outer = TransformerGeometry.GetRotationVector(TransformerGeometry.StartingRotation);
                Vector2 outerTransform = Vector2.Transform(outer, oneMatrix);
                nodes.Add(new Node { Point = outerTransform });
            }
            return nodes;
        }


        #endregion


        #region Cog


        /// <summary>
        /// Convert to curves from cog.
        /// </summary>
        /// <param name="count"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="tooth"> The tooth. </param>
        /// <param name="notch"> The notch. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromCog(ITransformerLTRB transformer, int count, float innerRadius, float tooth, float notch)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromCog(count, innerRadius, tooth, notch, oneMatrix)
            };
        }

        /// <summary>
        /// Convert to curves from cog.
        /// </summary>
        /// <param name="transformer"> The source transformer. </param>
        /// <param name="count"> The point count. </param>
        /// <param name="innerRadius"> The inner-radius. </param>
        /// <param name="tooth"> The tooth. </param>
        /// <param name="notch"> The notch. </param>
        /// <returns> The product curves. </returns>
        public static IEnumerable<IEnumerable<Node>> ConvertToCurvesFromCog(ITransformerLTRB transformer, Matrix3x2 matrix, int count, float innerRadius, float tooth, float notch)
        {
            Matrix3x2 oneMatrix = Transformer.FindHomography(Transformer.One, transformer);
            Matrix3x2 oneMatrix2 = oneMatrix * matrix;

            return new List<IEnumerable<Node>>
            {
                TransformerGeometry._convertToCurveFromCog(count, innerRadius, tooth, notch, oneMatrix2)
            };
        }

        private static IEnumerable<Node> _convertToCurveFromCog(int count, float innerRadius, float tooth, float notch, Matrix3x2 oneMatrix)
        {
            float angle = FanKit.Math.Pi * 2f / count;//angle
            float angleTooth = angle * tooth;//angle tooth
            float angleNotch = angle * notch;//angle notch
            float angleDiffHalf = (angleNotch - angleTooth) / 2;//Half the angle difference between the tooth and the notch

            float rotation = 0;//Start angle is zero
            int countQuadra = count * 4;
            List<Node> nodes = new List<Node>();

            for (int i = 0; i < countQuadra; i++)
            {
                Vector2 vector = new Vector2((float)System.Math.Cos(rotation), (float)System.Math.Sin(rotation));
                int remainder = i % 4;//remainder

                if (remainder == 0)//凸 left-bottom point
                {
                    //Inner
                    Vector2 inner = vector * innerRadius;
                    Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                    nodes.Add(new Node { Point = innerTransform });
                    rotation += angleDiffHalf;
                }
                else if (remainder == 1)//凸 left-top point
                {
                    //Outer
                    Vector2 outer = vector;
                    Vector2 outerTransform = Vector2.Transform(vector, oneMatrix);
                    nodes.Add(new Node { Point = outerTransform });
                    rotation += angleTooth;
                }
                else if (remainder == 2)//凸 right-top point
                {
                    //Outer
                    Vector2 outer = vector;
                    Vector2 outerTransform = Vector2.Transform(vector, oneMatrix);
                    nodes.Add(new Node { Point = outerTransform });
                    rotation += angleDiffHalf;
                }
                else if (remainder == 3)//凸 right-bottom point
                {
                    //Inner
                    Vector2 inner = vector * innerRadius;
                    Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                    nodes.Add(new Node { Point = innerTransform });
                    rotation += angle - angleNotch;
                }
            }
            {
                Vector2 inner = new Vector2(1, 0) * innerRadius;
                Vector2 innerTransform = Vector2.Transform(inner, oneMatrix);
                nodes.Add(new Node { Point = innerTransform });
            }
            return nodes;
        }


        #endregion
        
    }
}