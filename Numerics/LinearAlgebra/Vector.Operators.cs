// <copyright file="Vector.Operators.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2013 Math.NET
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

namespace MathNet.Numerics.LinearAlgebra
{
    public abstract partial class Vector<T>
    {
        /// <summary>
        ///     Returns a <strong>Vector</strong> containing the same values of <paramref name="rightSide.
        /// 
        /// 
        /// </summary>
        /// <remarks>This method is included for completeness.</remarks>
        /// <param name="rightSide">The vector to get the values from.</param>
        /// <returns>A vector containing the same values as <paramref name="rightSide.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator +(Vector<T> rightSide)
        {
            return rightSide.Clone();
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
        public static Vector<T> operator -(Vector<T> rightSide)
        {
            return rightSide.Negate();
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
        public static Vector<T> operator +(Vector<T> leftSide, Vector<T> rightSide)
        {
            return leftSide.Add(rightSide);
        }

        /// <summary>
        ///     Adds a scalar to each element of a vector.
        /// </summary>
        /// <param name="leftSide">The vector to add to.</param>
        /// <param name="rightSide">The scalar value to add.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator +(Vector<T> leftSide, T rightSide)
        {
            return leftSide.Add(rightSide);
        }

        /// <summary>
        ///     Adds a scalar to each element of a vector.
        /// </summary>
        /// <param name="leftSide">The scalar value to add.</param>
        /// <param name="rightSide">The vector to add to.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator +(T leftSide, Vector<T> rightSide)
        {
            return rightSide.Add(leftSide);
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
        public static Vector<T> operator -(Vector<T> leftSide, Vector<T> rightSide)
        {
            return leftSide.Subtract(rightSide);
        }

        /// <summary>
        ///     Subtracts a scalar from each element of a vector.
        /// </summary>
        /// <param name="leftSide">The vector to subtract from.</param>
        /// <param name="rightSide">The scalar value to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator -(Vector<T> leftSide, T rightSide)
        {
            return leftSide.Subtract(rightSide);
        }

        /// <summary>
        ///     Subtracts each element of a vector from a scalar.
        /// </summary>
        /// <param name="leftSide">The scalar value to subtract from.</param>
        /// <param name="rightSide">The vector to subtract.</param>
        /// <returns>The result of the subtraction.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator -(T leftSide, Vector<T> rightSide)
        {
            return rightSide.SubtractFrom(leftSide);
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
        public static Vector<T> operator *(Vector<T> leftSide, T rightSide)
        {
            return leftSide.Multiply(rightSide);
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
        public static Vector<T> operator *(T leftSide, Vector<T> rightSide)
        {
            return rightSide.Multiply(leftSide);
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
        public static T operator *(Vector<T> leftSide, Vector<T> rightSide)
        {
            return leftSide.DotProduct(rightSide);
        }

        /// <summary>
        ///     Divides a scalar with a vector.
        /// </summary>
        /// <param name="dividend">The scalar to divide.</param>
        /// <param name="divisor">The vector.</param>
        /// <returns>The result of the division.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="divisor is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator /(T dividend, Vector<T> divisor)
        {
            return divisor.DivideByThis(dividend);
        }

        /// <summary>
        ///     Divides a vector with a scalar.
        /// </summary>
        /// <param name="dividend">The vector to divide.</param>
        /// <param name="divisor">The scalar value.</param>
        /// <returns>The result of the division.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="dividend is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator /(Vector<T> dividend, T divisor)
        {
            return dividend.Divide(divisor);
        }

        /// <summary>
        ///     Pointwise divides two <strong>Vectors</strong>.
        /// </summary>
        /// <param name="dividend">The vector to divide.</param>
        /// <param name="divisor">The other vector.</param>
        /// <returns>The result of the division.</returns>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="dividend and 
        /// 
        /// <paramref name="divisor are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="dividend is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator /(Vector<T> dividend, Vector<T> divisor)
        {
            return dividend.PointwiseDivide(divisor);
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     of each element of the vector of the given divisor.
        /// </summary>
        /// <param name="dividend">The vector whose elements we want to compute the remainder of.</param>
        /// <param name="divisor">The divisor to use.</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="dividend is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator %(Vector<T> dividend, T divisor)
        {
            return dividend.Remainder(divisor);
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     of the given dividend of each element of the vector.
        /// </summary>
        /// <param name="dividend">The dividend we want to compute the remainder of.</param>
        /// <param name="divisor">The vector whose elements we want to use as divisor.</param>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="dividend is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator %(T dividend, Vector<T> divisor)
        {
            return divisor.RemainderByThis(dividend);
        }

        /// <summary>
        ///     Computes the pointwise remainder (% operator), where the result has the sign of the dividend,
        ///     of each element of two vectors.
        /// </summary>
        /// <param name="dividend">The vector whose elements we want to compute the remainder of.</param>
        /// <param name="divisor">The divisor to use.</param>
        /// <exceptioncreArgumentException">If 
        /// 
        /// <paramref name="dividend and 
        /// 
        /// <paramref name="divisor are not the same size.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="dividend is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static Vector<T> operator %(Vector<T> dividend, Vector<T> divisor)
        {
            return dividend.PointwiseRemainder(divisor);
        }

        /// <summary>
        ///     Computes the exponential of a vector pointwise
        /// </summary>
        /// <param name="x">The input vector</param>
        /// <returns></returns>
        public static Vector<T> Exp(Vector<T> x)
        {
            return x.PointwiseUnary(x.DoPointwiseExp);
        }


        /// <summary>
        ///     Computes the sin of a vector pointwise
        /// </summary>
        /// <param name="x">The input vector</param>
        /// <returns></returns>
        public static Vector<T> Sin(Vector<T> x)
        {
            return x.PointwiseSin();
        }


        /// <summary>
        ///     Computes the tan of a vector pointwise
        /// </summary>
        /// <param name="x">The input vector</param>
        /// <returns></returns>
        public static Vector<T> Tan(Vector<T> x)
        {
            return x.PointwiseTan();
        }


        /// <summary>
        ///     Computes the atan of a vector pointwise
        /// </summary>
        /// <param name="x">The input vector</param>
        /// <returns></returns>
        public static Vector<T> Atan(Vector<T> x)
        {
            return x.PointwiseAtan();
        }


        /// <summary>
        ///     Computes the absolute value of a vector pointwise
        /// </summary>
        /// <param name="x">The input vector</param>
        /// <returns></returns>
        public static Vector<T> Abs(Vector<T> x)
        {
            return x.PointwiseAbs();
        }


        /// <summary>
        ///     Computes the rounded value of a vector pointwise
        /// </summary>
        /// <param name="x">The input vector</param>
        /// <returns></returns>
        public static Vector<T> Round(Vector<T> x)
        {
            return x.PointwiseRound();
        }
    }
}