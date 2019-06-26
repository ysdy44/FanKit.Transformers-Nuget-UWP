using System;
using System.Diagnostics;
using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.Providers.LinearAlgebra;
using MathNet.Numerics.Threading;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.Providers.LinearAlgebra;
using MathNet.Numerics.Threading;




namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    ///     A vector using dense storage.
    /// </summary>
    [DebuggerDisplay("DenseVector {" + nameof(Count) + "}-Double")]
    public class DenseVector : Vector
    {
        /// <summary>
        ///     Number of elements
        /// </summary>
        private readonly int _length;

        /// <summary>
        ///     Gets the vector's data.
        /// </summary>
        private readonly double[] _values;

        /// <summary>
        ///     Create a new dense vector straight from an initialized vector storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public DenseVector(DenseVectorStorage<double> storage)
            : base(storage)
        {
            _length = storage.Length;
            _values = storage.Data;
        }

        /// <summary>
        ///     Create a new dense vector with the given length.
        ///     All cells of the vector will be initialized to zero.
        ///     Zero-length vectors are not supported.
        /// </summary>
        /// <exceptioncreArgumentException">If length is less than one.
        /// 
        /// </exception>
        public DenseVector(int length)
            : this(new DenseVectorStorage<double>(length))
        {
        }

        /// <summary>
        ///     Create a new dense vector directly binding to a raw array.
        ///     The array is used directly without copying.
        ///     Very efficient, but changes to the array and the vector will affect each other.
        /// </summary>
        public DenseVector(double[] storage)
            : this(new DenseVectorStorage<double>(storage.Length, storage))
        {
        }

        /// <summary>
        ///     Gets the vector's data.
        /// </summary>
        /// <value>The vector's data.</value>
        public double[] Values => _values;

        /// <summary>
        ///     Create a new dense vector and initialize each value using the provided value.
        /// </summary>
        public static DenseVector Create(int length, double value)
        {
            if (value == 0d) return new DenseVector(length);
            return new DenseVector(DenseVectorStorage<double>.OfValue(length, value));
        }

        /// <summary>
        ///     Create a new dense vector and initialize each value using the provided init function.
        /// </summary>
        public static DenseVector Create(int length, Func<int, double> init)
        {
            return new DenseVector(DenseVectorStorage<double>.OfInit(length, init));
        }

        /// <summary>
        ///     Returns a reference to the internal data structure.
        /// </summary>
        /// <param name="vector">
        ///     The <c>DenseVector</c> whose internal data we are
        ///     returning.
        /// </param>
        /// <returns>
        ///     A reference to the internal date of the given vector.
        /// </returns>
        public static explicit operator double[] (DenseVector vector)
        {
            if (vector == null) throw new ArgumentNullException(nameof(vector));

            return vector.Values;
        }

        /// <summary>
        ///     Returns a vector bound directly to a reference of the provided array.
        /// </summary>
        /// <param name="array">The array to bind to the <c>DenseVector</c> object.</param>
        /// <returns>
        ///     A <c>DenseVector</c> whose values are bound to the given array.
        /// </returns>
        public static implicit operator DenseVector(double[] array)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));

            return new DenseVector(array);
        }

        /// <summary>
        ///     Adds a scalar to each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The vector to store the result of the addition.</param>
        protected override void DoAdd(double scalar, Vector<double> result)
        {
            var dense = result as DenseVector;
            if (dense == null)
                base.DoAdd(scalar, result);
            else
                CommonParallel.For(0, _values.Length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) dense._values[i] = _values[i] + scalar;
                });
        }

        /// <summary>
        ///     Adds another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">The vector to add to this one.</param>
        /// <param name="result">The vector to store the result of the addition.</param>
        protected override void DoAdd(Vector<double> other, Vector<double> result)
        {
            var otherDense = other as DenseVector;
            var resultDense = result as DenseVector;

            if (otherDense == null || resultDense == null)
                base.DoAdd(other, result);
            else
                LinearAlgebraControl.Provider.AddArrays(_values, otherDense._values, resultDense._values);
        }

        /// <summary>
        ///     Adds two <strong>Vectors</strong> together and returns the results.
        /// </summary>
        /// <param name="leftSide">One of the vectors to add.</param>
        /// <param name="rightSide">The other vector to add.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator +(DenseVector leftSide, DenseVector rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (leftSide.Count != rightSide.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(rightSide));
            }

            return (DenseVector)leftSide.Add(rightSide);
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        protected override void DoSubtract(double scalar, Vector<double> result)
        {
            var dense = result as DenseVector;
            if (dense == null)
                base.DoSubtract(scalar, result);
            else
                CommonParallel.For(0, _values.Length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) dense._values[i] = _values[i] - scalar;
                });
        }

        /// <summary>
        ///     Subtracts another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">The vector to subtract from this one.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        protected override void DoSubtract(Vector<double> other, Vector<double> result)
        {
            var otherDense = other as DenseVector;
            var resultDense = result as DenseVector;

            if (otherDense == null || resultDense == null)
                base.DoSubtract(other, result);
            else
                LinearAlgebraControl.Provider.SubtractArrays(_values, otherDense._values, resultDense._values);
        }

        /// <summary>
        ///     Returns a <strong>Vector</strong> containing the negated values of <paramref name="rightSide.
        /// 
        /// 
        /// </summary>
        /// <param name="rightSide">The vector to get the values from.</param>
        /// <returns>A vector containing the negated values as <paramref name="rightSide.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator -(DenseVector rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (DenseVector)rightSide.Negate();
        }

        /// <summary>
        ///     Subtracts two <strong>Vectors</strong> and returns the results.
        /// </summary>
        /// <param name="leftSide">The vector to subtract from.</param>
        /// <param name="rightSide">The vector to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator -(DenseVector leftSide, DenseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseVector)leftSide.Subtract(rightSide);
        }

        /// <summary>
        ///     Negates vector and saves result to <paramref name="result
        /// 
        /// 
        /// </summary>
        /// <param name="result">Target vector</param>
        protected override void DoNegate(Vector<double> result)
        {
            var denseResult = result as DenseVector;
            if (denseResult == null)
            {
                base.DoNegate(result);
                return;
            }

            LinearAlgebraControl.Provider.ScaleArray(-1.0d, _values, denseResult.Values);
        }

        /// <summary>
        ///     Multiplies a scalar to each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="result">The vector to store the result of the multiplication.</param>
        /// <remarks></remarks>
        protected override void DoMultiply(double scalar, Vector<double> result)
        {
            var denseResult = result as DenseVector;
            if (denseResult == null)
            {
                base.DoMultiply(scalar, result);
                return;
            }

            LinearAlgebraControl.Provider.ScaleArray(scalar, _values, denseResult.Values);
        }

        /// <summary>
        ///     Computes the dot product between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The sum of a[i]*b[i] for all i.</returns>
        protected override double DoDotProduct(Vector<double> other)
        {
            var denseVector = other as DenseVector;
            return denseVector == null
                ? base.DoDotProduct(other)
                : LinearAlgebraControl.Provider.DotProduct(_values, denseVector.Values);
        }

        /// <summary>
        ///     Multiplies a vector with a scalar.
        /// </summary>
        /// <param name="leftSide">The vector to scale.</param>
        /// <param name="rightSide">The scalar value.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator *(DenseVector leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseVector)leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a vector with a scalar.
        /// </summary>
        /// <param name="leftSide">The scalar value.</param>
        /// <param name="rightSide">The vector to scale.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator *(double leftSide, DenseVector rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (DenseVector)rightSide.Multiply(leftSide);
        }

        /// <summary>
        ///     Computes the dot product between two <strong>Vectors</strong>.
        /// </summary>
        /// <param name="leftSide">The left row vector.</param>
        /// <param name="rightSide">The right column vector.</param>
        /// <returns>The dot product between the two vectors.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static double operator *(DenseVector leftSide, DenseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return leftSide.DotProduct(rightSide);
        }

        /// <summary>
        ///     Divides a vector with a scalar.
        /// </summary>
        /// <param name="leftSide">The vector to divide.</param>
        /// <param name="rightSide">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator /(DenseVector leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseVector)leftSide.Divide(rightSide);
        }

        /// <summary>
        ///     Computes the canonical modulus, where the result has the sign of the divisor,
        ///     for each element of the vector for the given divisor.
        /// </summary>
        /// <param name="divisor">The divisor to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoModulus(double divisor, Vector<double> result)
        {
            var dense = result as DenseVector;
            if (dense == null)
                base.DoModulus(divisor, result);
            else
                CommonParallel.For(0, _length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) dense._values[i] = Euclid.Modulus(_values[i], divisor);
                });
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for each element of the vector for the given divisor.
        /// </summary>
        /// <param name="divisor">The divisor to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoRemainder(double divisor, Vector<double> result)
        {
            var dense = result as DenseVector;
            if (dense == null)
                base.DoRemainder(divisor, result);
            else
                CommonParallel.For(0, _length, 4096, (a, b) =>
                {
                    for (var i = a; i < b; i++) dense._values[i] = _values[i] % divisor;
                });
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     of each element of the vector of the given divisor.
        /// </summary>
        /// <param name="leftSide">The vector whose elements we want to compute the remainder of.</param>
        /// <param name="rightSide">The divisor to use,</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator %(DenseVector leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseVector)leftSide.Remainder(rightSide);
        }

        /// <summary>
        ///     Returns the index of the maximum element.
        /// </summary>
        /// <returns>The index of maximum element.</returns>
        public override int MaximumIndex()
        {
            var index = 0;
            var max = _values[0];
            for (var i = 1; i < _length; i++)
                if (max < _values[i])
                {
                    index = i;
                    max = _values[i];
                }

            return index;
        }

        /// <summary>
        ///     Computes the sum of the vector's elements.
        /// </summary>
        /// <returns>The sum of the vector's elements.</returns>
        public override double Sum()
        {
            var sum = 0.0;
            for (var index = 0; index < _length; index++) sum += _values[index];
            return sum;
        }

        /// <summary>
        ///     Calculates the L1 norm of the vector, also known as Manhattan norm.
        /// </summary>
        /// <returns>The sum of the absolute values.</returns>
        public double L1Norm()
        {
            var sum = 0d;
            for (var index = 0; index < _length; index++) sum += Math.Abs(_values[index]);
            return sum;
        }

        /// <summary>
        ///     Calculates the L2 norm of the vector, also known as Euclidean norm.
        /// </summary>
        /// <returns>The square root of the sum of the squared values.</returns>
        public override double L2Norm()
        {
            // TODO: native provider
            return _values.Aggregate(0d, SpecialFunctions.Hypotenuse);
        }

        /// <summary>
        ///     Calculates the infinity norm of the vector.
        /// </summary>
        /// <returns>The maximum absolute value.</returns>
        public double InfinityNorm()
        {
            return CommonParallel.Aggregate(_values, (i, v) => Math.Abs(v), Math.Max, 0d);
        }

        /// <summary>
        ///     Computes the p-Norm.
        /// </summary>
        /// <param name="p">The p value.</param>
        /// <returns>Scalar <c>ret = ( ∑|this[i]|^p )^(1/p)</c></returns>
        public override double Norm(double p)
        {
            if (p < 0d)
            {
            } //throw new ArgumentOutOfRangeException(nameof(p));

            if (p == 1d) return L1Norm();
            if (p == 2d) return L2Norm();
            if (double.IsPositiveInfinity(p)) return InfinityNorm();

            var sum = 0d;
            for (var index = 0; index < _length; index++) sum += Math.Pow(Math.Abs(_values[index]), p);
            return Math.Pow(sum, 1.0 / p);
        }

        /// <summary>
        ///     Pointwise divide this vector with another vector and stores the result into the result vector.
        /// </summary>
        /// <param name="divisor">The vector to pointwise divide this one by.</param>
        /// <param name="result">The vector to store the result of the pointwise division.</param>
        /// <remarks></remarks>
        protected override void DoPointwiseDivide(Vector<double> divisor, Vector<double> result)
        {
            var denseOther = divisor as DenseVector;
            var denseResult = result as DenseVector;

            if (denseOther == null || denseResult == null)
                base.DoPointwiseDivide(divisor, result);
            else
                LinearAlgebraControl.Provider.PointWiseDivideArrays(_values, denseOther._values, denseResult._values);
        }


        #region Parse Functions

        #endregion
    }
}


