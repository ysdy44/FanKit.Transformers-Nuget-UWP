// <copyright file="Vector.Arithmetic.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2016 Math.NET
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
// </copyright>

using System;

namespace MathNet.Numerics.LinearAlgebra
{
    public abstract partial class Vector<T>
    {
        /// <summary>
        ///     The zero value for type T.
        /// </summary>
        public static readonly T Zero = BuilderInstance<T>.Vector.Zero;

        /// <summary>
        ///     The value of 1.0 for type T.
        /// </summary>
        public static readonly T One = BuilderInstance<T>.Vector.One;

        /// <summary>
        ///     Negates vector and save result to <paramref name="result
        /// 
        /// 
        /// </summary>
        /// <param name="result">Target vector</param>
        protected abstract void DoNegate(Vector<T> result);


        /// <summary>
        ///     Adds a scalar to each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The vector to store the result of the addition.</param>
        protected abstract void DoAdd(T scalar, Vector<T> result);

        /// <summary>
        ///     Adds another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">The vector to add to this one.</param>
        /// <param name="result">The vector to store the result of the addition.</param>
        protected abstract void DoAdd(Vector<T> other, Vector<T> result);

        /// <summary>
        ///     Subtracts a scalar from each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        protected abstract void DoSubtract(T scalar, Vector<T> result);

        /// <summary>
        ///     Subtracts each element of the vector from a scalar and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract from.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        protected void DoSubtractFrom(T scalar, Vector<T> result)
        {
            DoNegate(result);
            result.DoAdd(scalar, result);
        }

        /// <summary>
        ///     Subtracts another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">The vector to subtract from this one.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        protected abstract void DoSubtract(Vector<T> other, Vector<T> result);

        /// <summary>
        ///     Multiplies a scalar to each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <param name="result">The vector to store the result of the multiplication.</param>
        protected abstract void DoMultiply(T scalar, Vector<T> result);

        /// <summary>
        ///     Computes the dot product between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The sum of a[i]*b[i] for all i.</returns>
        protected abstract T DoDotProduct(Vector<T> other);


        /// <summary>
        ///     Divides each element of the vector by a scalar and stores the result in the result vector.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">The vector to store the result of the division.</param>
        protected abstract void DoDivide(T divisor, Vector<T> result);

        /// <summary>
        ///     Divides a scalar by each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <param name="result">The vector to store the result of the division.</param>
        protected abstract void DoDivideByThis(T dividend, Vector<T> result);

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for each element of the vector for the given divisor.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected abstract void DoRemainder(T divisor, Vector<T> result);

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given dividend for each element of the vector.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected abstract void DoRemainderByThis(T dividend, Vector<T> result);

        /// <summary>
        ///     Pointwise divide this vector with another vector and stores the result into the result vector.
        /// </summary>
        /// <param name="divisor">The pointwise denominator vector to use.</param>
        /// <param name="result">The result of the division.</param>
        protected abstract void DoPointwiseDivide(Vector<T> divisor, Vector<T> result);

        /// <summary>
        ///     Pointwise remainder (% operator), where the result has the sign of the dividend,
        ///     of this vector with another vector and stores the result into the result vector.
        /// </summary>
        /// <param name="divisor">The pointwise denominator vector to use.</param>
        /// <param name="result">The result of the modulus.</param>
        protected abstract void DoPointwiseRemainder(Vector<T> divisor, Vector<T> result);

        /// <summary>
        ///     Pointwise applies the exponential function to each value and stores the result into the result vector.
        /// </summary>
        /// <param name="result">The vector to store the result.</param>
        protected abstract void DoPointwiseExp(Vector<T> result);

        protected abstract void DoPointwiseAbs(Vector<T> result);
        protected abstract void DoPointwiseAtan(Vector<T> result);
        protected abstract void DoPointwiseRound(Vector<T> result);
        protected abstract void DoPointwiseSin(Vector<T> result);
        protected abstract void DoPointwiseTan(Vector<T> result);

        /// <summary>
        ///     Adds a scalar to each element of the vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <returns>A copy of the vector with the scalar added.</returns>
        public Vector<T> Add(T scalar)
        {
            if (scalar.Equals(Zero)) return Clone();

            var result = Build.SameAs(this);
            DoAdd(scalar, result);
            return result;
        }

        /// <summary>
        ///     Adds a scalar to each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The vector to store the result of the addition.</param>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="result are not the same size.
        /// 
        /// </exception>
        public void Add(T scalar, Vector<T> result)
        {
            if (Count != result.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(result));
            }

            if (scalar.Equals(Zero))
            {
                CopyTo(result);
                return;
            }

            DoAdd(scalar, result);
        }

        /// <summary>
        ///     Adds another vector to this vector.
        /// </summary>
        /// <param name="other">The vector to add to this one.</param>
        /// <returns>A new vector containing the sum of both vectors.</returns>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="other are not the same size.
        /// 
        /// </exception>
        public Vector<T> Add(Vector<T> other)
        {
            if (Count != other.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(other));
            }

            var result = Build.SameAs(this, other);
            DoAdd(other, result);
            return result;
        }

        /// <summary>
        ///     Adds another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">The vector to add to this one.</param>
        /// <param name="result">The vector to store the result of the addition.</param>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="other are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="result are not the same size.
        /// 
        /// </exception>
        public void Add(Vector<T> other, Vector<T> result)
        {
            if (Count != result.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(result));
            }

            DoAdd(other, result);
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <returns>A new vector containing the subtraction of this vector and the scalar.</returns>
        public Vector<T> Subtract(T scalar)
        {
            if (scalar.Equals(Zero)) return Clone();

            var result = Build.SameAs(this);
            DoSubtract(scalar, result);
            return result;
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the vector and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="result are not the same size.
        /// 
        /// </exception>
        public void Subtract(T scalar, Vector<T> result)
        {
            if (Count != result.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(result));
            }

            if (scalar.Equals(Zero))
            {
                CopyTo(result);
                return;
            }

            DoSubtract(scalar, result);
        }

        /// <summary>
        ///     Subtracts each element of the vector from a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to subtract from.</param>
        /// <returns>A new vector containing the subtraction of the scalar and this vector.</returns>
        public Vector<T> SubtractFrom(T scalar)
        {
            var result = Build.SameAs(this);
            DoSubtractFrom(scalar, result);
            return result;
        }

        /// <summary>
        ///     Returns a negated vector.
        /// </summary>
        /// <returns>The negated vector.</returns>
        /// <remarks>Added as an alternative to the unary negation operator.</remarks>
        public Vector<T> Negate()
        {
            var retrunVector = Build.SameAs(this);
            DoNegate(retrunVector);
            return retrunVector;
        }
        

        /// <summary>
        ///     Subtracts another vector from this vector.
        /// </summary>
        /// <param name="other">The vector to subtract from this one.</param>
        /// <returns>A new vector containing the subtraction of the two vectors.</returns>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="other are not the same size.
        /// 
        /// </exception>
        public Vector<T> Subtract(Vector<T> other)
        {
            if (Count != other.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(other));
            }

            var result = Build.SameAs(this, other);
            DoSubtract(other, result);
            return result;
        }

        /// <summary>
        ///     Subtracts another vector to this vector and stores the result into the result vector.
        /// </summary>
        /// <param name="other">The vector to subtract from this one.</param>
        /// <param name="result">The vector to store the result of the subtraction.</param>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="other are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="result are not the same size.
        /// 
        /// </exception>
        public void Subtract(Vector<T> other, Vector<T> result)
        {
            if (Count != result.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(result));
            }

            DoSubtract(other, result);
        }


        /// <summary>
        ///     Multiplies a scalar to each element of the vector.
        /// </summary>
        /// <param name="scalar">The scalar to multiply.</param>
        /// <returns>A new vector that is the multiplication of the vector and the scalar.</returns>
        public Vector<T> Multiply(T scalar)
        {
            if (scalar.Equals(One)) return Clone();

            if (scalar.Equals(Zero)) return Build.SameAs(this);

            var result = Build.SameAs(this);
            DoMultiply(scalar, result);
            return result;
        }

        /// <summary>
        ///     Computes the dot product between this vector and another vector.
        /// </summary>
        /// <param name="other">The other vector.</param>
        /// <returns>The sum of a[i]*b[i] for all i.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="other is not of the same size.
        /// 
        /// </exception>
        /// <seealsocreConjugateDotProduct
        public T DotProduct(Vector<T> other)
        {
            if (Count != other.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(other));
            }

            return DoDotProduct(other);
        }


        /// <summary>
        ///     Divides each element of the vector by a scalar.
        /// </summary>
        /// <param name="scalar">The scalar to divide with.</param>
        /// <returns>A new vector that is the division of the vector and the scalar.</returns>
        public Vector<T> Divide(T scalar)
        {
            if (scalar.Equals(One)) return Clone();

            var result = Build.SameAs(this);
            DoDivide(scalar, result);
            return result;
        }

        /// <summary>
        ///     Divides a scalar by each element of the vector.
        /// </summary>
        /// <param name="scalar">The scalar to divide.</param>
        /// <returns>A new vector that is the division of the vector and the scalar.</returns>
        public Vector<T> DivideByThis(T scalar)
        {
            var result = Build.SameAs(this);
            DoDivideByThis(scalar, result);
            return result;
        }

        /// <summary>
        ///     Computes the remainder (vector % divisor), where the result has the sign of the dividend,
        ///     for each element of the vector for the given divisor.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <returns>A vector containing the result.</returns>
        public Vector<T> Remainder(T divisor)
        {
            var result = Build.SameAs(this);
            DoRemainder(divisor, result);
            return result;
        }

        /// <summary>
        ///     Computes the remainder (dividend % vector), where the result has the sign of the dividend,
        ///     for the given dividend for each element of the vector.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <returns>A vector containing the result.</returns>
        public Vector<T> RemainderByThis(T dividend)
        {
            var result = Build.SameAs(this);
            DoRemainderByThis(dividend, result);
            return result;
        }


        /// <summary>
        ///     Pointwise divide this vector with another vector.
        /// </summary>
        /// <param name="divisor">The pointwise denominator vector to use.</param>
        /// <returns>A new vector which is the pointwise division of the two vectors.</returns>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="divisor are not the same size.
        /// 
        /// </exception>
        public Vector<T> PointwiseDivide(Vector<T> divisor)
        {
            if (Count != divisor.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(divisor));
            }

            var result = Build.SameAs(this, divisor);
            DoPointwiseDivide(divisor, result);
            return result;
        }


        /// <summary>
        ///     Pointwise remainder (% operator), where the result has the sign of the dividend,
        ///     of this vector with another vector.
        /// </summary>
        /// <param name="divisor">The pointwise denominator vector to use.</param>
        /// <exceptioncreArgumentException">If this vector and 
        /// 
        /// <paramref name="divisor are not the same size.
        /// 
        /// </exception>
        public Vector<T> PointwiseRemainder(Vector<T> divisor)
        {
            if (Count != divisor.Count)
            {
                //throw new ArgumentException(Resources.ArgumentVectorsSameLength, nameof(divisor));
            }

            var result = Build.SameAs(this, divisor);
            DoPointwiseRemainder(divisor, result);
            return result;
        }

        /// <summary>
        ///     Helper function to apply a unary function to a vector. The function
        ///     f modifies the vector given to it in place.  Before its
        ///     called, a copy of the 'this' vector with the same dimension is
        ///     first created, then passed to f.  The copy is returned as the result
        /// </summary>
        /// <param name="f">Function which takes a vector, modifies it in place and returns void</param>
        /// <returns>New instance of vector which is the result</returns>
        protected Vector<T> PointwiseUnary(Action<Vector<T>> f)
        {
            var result = Build.SameAs(this);
            f(result);
            return result;
        }


        /// <summary>
        ///     Pointwise applies the abs function to each value
        /// </summary>
        public Vector<T> PointwiseAbs()
        {
            return PointwiseUnary(DoPointwiseAbs);
        }


        /// <summary>
        ///     Pointwise applies the atan function to each value
        /// </summary>
        public Vector<T> PointwiseAtan()
        {
            return PointwiseUnary(DoPointwiseAtan);
        }


        /// <summary>
        ///     Pointwise applies the round function to each value
        /// </summary>
        public Vector<T> PointwiseRound()
        {
            return PointwiseUnary(DoPointwiseRound);
        }


        /// <summary>
        ///     Pointwise applies the sin function to each value
        /// </summary>
        public Vector<T> PointwiseSin()
        {
            return PointwiseUnary(DoPointwiseSin);
        }


        /// <summary>
        ///     Pointwise applies the tan function to each value
        /// </summary>
        public Vector<T> PointwiseTan()
        {
            return PointwiseUnary(DoPointwiseTan);
        }


        /// <summary>
        ///     Calculates the L2 norm of the vector, also known as Euclidean norm.
        /// </summary>
        /// <returns>The square root of the sum of the squared values.</returns>
        public abstract double L2Norm();

        /// <summary>
        ///     Computes the p-Norm.
        /// </summary>
        /// <param name="p">The p value.</param>
        /// <returns>
        ///     <c>Scalar ret = (sum(abs(this[i])^p))^(1/p)</c>
        /// </returns>
        public abstract double Norm(double p);

        /// <summary>
        ///     Returns the value of maximum element.
        /// </summary>
        /// <returns>The value of maximum element.</returns>
        public T Maximum()
        {
            return At(MaximumIndex());
        }

        /// <summary>
        ///     Returns the index of the maximum element.
        /// </summary>
        /// <returns>The index of maximum element.</returns>
        public abstract int MaximumIndex();


        /// <summary>
        ///     Computes the sum of the vector's elements.
        /// </summary>
        /// <returns>The sum of the vector's elements.</returns>
        public abstract T Sum();
    }
}