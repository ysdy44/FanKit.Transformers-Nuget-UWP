// <copyright file="Complex32.cs" company="Math.NET">
// Math.NET Numerics, part of the Math.NET Project
// http://numerics.mathdotnet.com
// http://github.com/mathnet/mathnet-numerics
//
// Copyright (c) 2009-2010 Math.NET
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
using System.Globalization;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
#if !NETSTANDARD1_3
using System.Runtime;

#endif

namespace MathNet.Numerics
{
    /// <summary>
    ///     32-bit single precision complex numbers class.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         The class <c>Complex32</c> provides all elementary operations
    ///         on complex numbers. All the operators <c>+</c>, <c>-</c>,
    ///         <c>*</c>, <c>/</c>, <c>==</c>, <c>!=</c> are defined in the
    ///         canonical way. Additional complex trigonometric functions
    ///         are also provided. Note that the <c>Complex32</c> structures
    ///         has two special constant values <seecreComplex32.NaN" /> and
    ///         
    ///         
    ///         <seecreComplex32.PositiveInfinity" />.
    ///     
    ///     
    ///     </para>
    ///     <para>
    ///         <code>
    /// Complex32 x = new Complex32(1f,2f);
    /// Complex32 y = Complex32.FromPolarCoordinates(1f, Math.Pi);
    /// Complex32 z = (x + y) / (x - y);
    /// </code>
    ///     </para>
    ///     <para>
    ///         For mathematical details about complex numbers, please
    ///         have a look at the
    ///         <a href="http://en.wikipedia.org/wiki/Complex_number">
    ///             Wikipedia
    ///         </a>
    ///     </para>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    [DataContract(Namespace = "urn:MathNet/Numerics")]
    public struct Complex32 : IFormattable, IEquatable<Complex32>
    {
        /// <summary>
        ///     The real component of the complex number.
        /// </summary>
        [DataMember(Order = 1)] private readonly float _real;

        /// <summary>
        ///     The imaginary component of the complex number.
        /// </summary>
        [DataMember(Order = 2)] private readonly float _imag;

        /// <summary>
        ///     Initializes a new instance of the Complex32 structure with the given real
        ///     and imaginary parts.
        /// </summary>
        /// <param name="real">The value for the real component.</param>
        /// <param name="imaginary">The value for the imaginary component.</param>
        // [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public Complex32(float real, float imaginary)
        {
            _real = real;
            _imag = imaginary;
        }


        /// <summary>
        ///     Returns a new
        ///     <seecreT:MathNet.Numerics.Complex32" /> instance
        ///     with a real number equal to zero and an imaginary number equal to zero.
        /// 
        /// 
        /// </summary>
        public static readonly Complex32 Zero = new Complex32(0.0f, 0.0f);

        /// <summary>
        ///     Returns a new
        ///     <seecreT:MathNet.Numerics.Complex32" /> instance
        ///     with a real number equal to one and an imaginary number equal to zero.
        /// 
        /// 
        /// </summary>
        public static readonly Complex32 One = new Complex32(1.0f, 0.0f);

        /// <summary>
        ///     Returns a new
        ///     <seecreT:MathNet.Numerics.Complex32" /> instance
        ///     with a real number equal to zero and an imaginary number equal to one.
        /// 
        /// 
        /// </summary>
        public static readonly Complex32 ImaginaryOne = new Complex32(0, 1);

        /// <summary>
        ///     Returns a new
        ///     <seecreT:MathNet.Numerics.Complex32" /> instance
        ///     with real and imaginary numbers positive infinite.
        /// 
        /// 
        /// </summary>
        public static readonly Complex32 PositiveInfinity =
            new Complex32(float.PositiveInfinity, float.PositiveInfinity);

        /// <summary>
        ///     Returns a new
        ///     <seecreT:MathNet.Numerics.Complex32" /> instance
        ///     with real and imaginary numbers not a number.
        /// 
        /// 
        /// </summary>
        public static readonly Complex32 NaN = new Complex32(float.NaN, float.NaN);

        /// <summary>
        ///     Gets the real component of the complex number.
        /// </summary>
        /// <value>The real component of the complex number.</value>
        public float Real
        {
            get => _real;
        }

        /// <summary>
        ///     Gets the real imaginary component of the complex number.
        /// </summary>
        /// <value>The real imaginary component of the complex number.</value>
        public float Imaginary
        {
            get => _imag;
        }

        /// <summary>
        ///     Gets the magnitude (or absolute value) of a complex number.
        /// </summary>
        /// <remarks>Assuming that magnitude of (inf,a) and (a,inf) and (inf,inf) is inf and (NaN,a), (a,NaN) and (NaN,NaN) is NaN</remarks>
        /// <returns>The magnitude of the current instance.</returns>
        public float Magnitude
        {
            get
            {
                if (float.IsNaN(_real) || float.IsNaN(_imag))
                    return float.NaN;
                if (float.IsInfinity(_real) || float.IsInfinity(_imag))
                    return float.PositiveInfinity;
                var a = Math.Abs(_real);
                var b = Math.Abs(_imag);
                if (a > b)
                {
                    double tmp = b / a;
                    return a * (float) Math.Sqrt(1.0f + tmp * tmp);
                }

                if (a == 0.0f) // one can write a >= float.Epsilon here
                    return b;

                {
                    double tmp = a / b;
                    return b * (float) Math.Sqrt(1.0f + tmp * tmp);
                }
            }
        }

        /// <summary>
        ///     Gets the squared magnitude (or squared absolute value) of a complex number.
        /// </summary>
        /// <returns>The squared magnitude of the current instance.</returns>
        public float MagnitudeSquared => _real * _real + _imag * _imag;

        /// <summary>
        ///     Gets a value indicating whether the <c>Complex32</c> is zero.
        /// </summary>
        /// <returns><c>true</c> if this instance is zero; otherwise, <c>false</c>.</returns>
        public bool IsZero()
        {
            return _real == 0.0f && _imag == 0.0f;
        }

        /// <summary>
        ///     Gets a value indicating whether the <c>Complex32</c> is one.
        /// </summary>
        /// <returns><c>true</c> if this instance is one; otherwise, <c>false</c>.</returns>
        public bool IsOne()
        {
            return _real == 1.0f && _imag == 0.0f;
        }


        /// <summary>
        ///     Gets a value indicating whether the provided <c>Complex32</c>evaluates
        ///     to a value that is not a number.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is <seecreNaN" />; otherwise,
        ///     <c>false</c>.
        /// </returns>
        public bool IsNaN()
        {
            return float.IsNaN(_real) || float.IsNaN(_imag);
        }

        /// <summary>
        ///     Gets a value indicating whether the provided <c>Complex32</c> evaluates to an
        ///     infinite value.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is infinite; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        ///     True if it either evaluates to a complex infinity
        ///     or to a directed infinity.
        /// </remarks>
        public bool IsInfinity()
        {
            return float.IsInfinity(_real) || float.IsInfinity(_imag);
        }

        /// <summary>
        ///     Gets a value indicating whether the provided <c>Complex32</c> is real.
        /// </summary>
        /// <returns><c>true</c> if this instance is a real number; otherwise, <c>false</c>.</returns>
        public bool IsReal()
        {
            return _imag == 0.0f;
        }

        /// <summary>
        ///     Gets a value indicating whether the provided <c>Complex32</c> is real and not negative, that is &gt;= 0.
        /// </summary>
        /// <returns>
        ///     <c>true</c> if this instance is real nonnegative number; otherwise, <c>false</c>.
        /// </returns>
        public bool IsRealNonNegative()
        {
            return _imag == 0.0f && _real >= 0;
        }

        /// <summary>
        ///     Exponential of this <c>Complex32</c> (exp(x), E^x).
        /// </summary>
        /// <returns>
        ///     The exponential of this complex number.
        /// </returns>
        public Complex32 Exponential()
        {
            var exp = (float) Math.Exp(_real);
            if (IsReal()) return new Complex32(exp, 0.0f);

            return new Complex32(exp * (float) Math.Cos(_imag), exp * (float) Math.Sin(_imag));
        }


        /// <summary>
        ///     The Square (power 2) of this <c>Complex32</c>
        /// </summary>
        /// <returns>
        ///     The square of this complex number.
        /// </returns>
        public Complex32 Square()
        {
            if (IsReal()) return new Complex32(_real * _real, 0.0f);

            return new Complex32(_real * _real - _imag * _imag, 2 * _real * _imag);
        }

        /// <summary>
        ///     The Square Root (power 1/2) of this <c>Complex32</c>
        /// </summary>
        /// <returns>
        ///     The square root of this complex number.
        /// </returns>
        public Complex32 SquareRoot()
        {
            if (IsRealNonNegative()) return new Complex32((float) Math.Sqrt(_real), 0.0f);

            Complex32 result;

            var absReal = Math.Abs(Real);
            var absImag = Math.Abs(Imaginary);
            double w;
            if (absReal >= absImag)
            {
                var ratio = Imaginary / Real;
                w = Math.Sqrt(absReal) * Math.Sqrt(0.5 * (1.0f + Math.Sqrt(1.0f + ratio * ratio)));
            }
            else
            {
                var ratio = Real / Imaginary;
                w = Math.Sqrt(absImag) * Math.Sqrt(0.5 * (Math.Abs(ratio) + Math.Sqrt(1.0f + ratio * ratio)));
            }

            if (Real >= 0.0f)
                result = new Complex32((float) w, (float) (Imaginary / (2.0f * w)));
            else if (Imaginary >= 0.0f)
                result = new Complex32((float) (absImag / (2.0 * w)), (float) w);
            else
                result = new Complex32((float) (absImag / (2.0 * w)), (float) -w);

            return result;
        }


        /// <su
        /// <summary>
        ///     Equality test.
        /// </summary>
        /// <param name="complex1">One of complex numbers to compare.</param>
        /// <param name="complex2">The other complex numbers to compare.</param>
        /// <returns><c>true</c> if the real and imaginary components of the two complex numbers are equal; <c>false</c> otherwise.</returns>
        public static bool operator ==(Complex32 complex1, Complex32 complex2)
        {
            return complex1.Equals(complex2);
        }

        /// <summary>
        ///     Inequality test.
        /// </summary>
        /// <param name="complex1">One of complex numbers to compare.</param>
        /// <param name="complex2">The other complex numbers to compare.</param>
        /// <returns>
        ///     <c>true</c> if the real or imaginary components of the two complex numbers are not equal; <c>false</c>
        ///     otherwise.
        /// </returns>
        public static bool operator !=(Complex32 complex1, Complex32 complex2)
        {
            return !complex1.Equals(complex2);
        }

        /// <summary>
        ///     Unary addition.
        /// </summary>
        /// <param name="summand">The complex number to operate on.</param>
        /// <returns>Returns the same complex number.</returns>
        public static Complex32 operator +(Complex32 summand)
        {
            return summand;
        }

        /// <summary>
        ///     Unary minus.
        /// </summary>
        /// <param name="subtrahend">The complex number to operate on.</param>
        /// <returns>The negated value of the <paramref name="subtrahend" />.</returns>
        public static Complex32 operator -(Complex32 subtrahend)
        {
            return new Complex32(-subtrahend._real, -subtrahend._imag);
        }

        /// <summary>Addition operator. Adds two complex numbers together.</summary>
        /// <returns>The result of the addition.</returns>
        /// <param name="summand1">One of the complex numbers to add.</param>
        /// <param name="summand2">The other complex numbers to add.</param>
        public static Complex32 operator +(Complex32 summand1, Complex32 summand2)
        {
            return new Complex32(summand1._real + summand2._real, summand1._imag + summand2._imag);
        }

        /// <summary>Subtraction operator. Subtracts two complex numbers.</summary>
        /// <returns>The result of the subtraction.</returns>
        /// <param name="minuend">The complex number to subtract from.</param>
        /// <param name="subtrahend">The complex number to subtract.</param>
        public static Complex32 operator -(Complex32 minuend, Complex32 subtrahend)
        {
            return new Complex32(minuend._real - subtrahend._real, minuend._imag - subtrahend._imag);
        }

        /// <summary>Addition operator. Adds a complex number and float together.</summary>
        /// <returns>The result of the addition.</returns>
        /// <param name="summand1">The complex numbers to add.</param>
        /// <param name="summand2">The float value to add.</param>
        public static Complex32 operator +(Complex32 summand1, float summand2)
        {
            return new Complex32(summand1._real + summand2, summand1._imag);
        }

        /// <summary>Subtraction operator. Subtracts float value from a complex value.</summary>
        /// <returns>The result of the subtraction.</returns>
        /// <param name="minuend">The complex number to subtract from.</param>
        /// <param name="subtrahend">The float value to subtract.</param>
        public static Complex32 operator -(Complex32 minuend, float subtrahend)
        {
            return new Complex32(minuend._real - subtrahend, minuend._imag);
        }

        /// <summary>Addition operator. Adds a complex number and float together.</summary>
        /// <returns>The result of the addition.</returns>
        /// <param name="summand1">The float value to add.</param>
        /// <param name="summand2">The complex numbers to add.</param>
        public static Complex32 operator +(float summand1, Complex32 summand2)
        {
            return new Complex32(summand2._real + summand1, summand2._imag);
        }

        /// <summary>Subtraction operator. Subtracts complex value from a float value.</summary>
        /// <returns>The result of the subtraction.</returns>
        /// <param name="minuend">The float vale to subtract from.</param>
        /// <param name="subtrahend">The complex value to subtract.</param>
        public static Complex32 operator -(float minuend, Complex32 subtrahend)
        {
            return new Complex32(minuend - subtrahend._real, -subtrahend._imag);
        }

        /// <summary>Multiplication operator. Multiplies two complex numbers.</summary>
        /// <returns>The result of the multiplication.</returns>
        /// <param name="multiplicand">One of the complex numbers to multiply.</param>
        /// <param name="multiplier">The other complex number to multiply.</param>
        public static Complex32 operator *(Complex32 multiplicand, Complex32 multiplier)
        {
            return new Complex32(
                multiplicand._real * multiplier._real - multiplicand._imag * multiplier._imag,
                multiplicand._real * multiplier._imag + multiplicand._imag * multiplier._real);
        }

        /// <summary>Multiplication operator. Multiplies a complex number with a float value.</summary>
        /// <returns>The result of the multiplication.</returns>
        /// <param name="multiplicand">The float value to multiply.</param>
        /// <param name="multiplier">The complex number to multiply.</param>
        public static Complex32 operator *(float multiplicand, Complex32 multiplier)
        {
            return new Complex32(multiplier._real * multiplicand, multiplier._imag * multiplicand);
        }

        /// <summary>Multiplication operator. Multiplies a complex number with a float value.</summary>
        /// <returns>The result of the multiplication.</returns>
        /// <param name="multiplicand">The complex number to multiply.</param>
        /// <param name="multiplier">The float value to multiply.</param>
        public static Complex32 operator *(Complex32 multiplicand, float multiplier)
        {
            return new Complex32(multiplicand._real * multiplier, multiplicand._imag * multiplier);
        }

        /// <summary>Division operator. Divides a complex number by another.</summary>
        /// <remarks>Enhanced Smith's algorithm for dividing two complex numbers </remarks>
        /// <seecreInternalDiv( float, float, float, float, bool)" />
        /// 
        /// 
        /// <returns>The result of the division.</returns>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        public static Complex32 operator /(Complex32 dividend, Complex32 divisor)
        {
            if (dividend.IsZero() && divisor.IsZero()) return NaN;

            if (divisor.IsZero()) return PositiveInfinity;
            var a = dividend.Real;
            var b = dividend.Imaginary;
            var c = divisor.Real;
            var d = divisor.Imaginary;
            if (Math.Abs(d) <= Math.Abs(c))
                return InternalDiv(a, b, c, d, false);
            return InternalDiv(b, a, d, c, true);
        }

        /// <summary>
        ///     Helper method for dividing.
        /// </summary>
        /// <param name="a">Re first</param>
        /// <param name="b">Im first</param>
        /// <param name="c">Re second</param>
        /// <param name="d">Im second</param>
        /// <param name="swapped"></param>
        /// <returns></returns>
        private static Complex32 InternalDiv(float a, float b, float c, float d, bool swapped)
        {
            var r = d / c;
            var t = 1 / (c + d * r);
            float e, f;
            if (r != 0.0f) // one can use r >= float.Epsilon || r <= float.Epsilon instead
            {
                e = (a + b * r) * t;
                f = (b - a * r) * t;
            }
            else
            {
                e = (a + d * (b / c)) * t;
                f = (b - d * (a / c)) * t;
            }

            if (swapped)
                f = -f;
            return new Complex32(e, f);
        }

        /// <summary>Division operator. Divides a float value by a complex number.</summary>
        /// <remarks>Algorithm based on Smith's algorithm</remarks>
        /// <seecreInternalDiv( float, float, float, float, bool)" />
        /// 
        /// 
        /// <returns>The result of the division.</returns>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        public static Complex32 operator /(float dividend, Complex32 divisor)
        {
            if (dividend == 0.0f && divisor.IsZero()) return NaN;

            if (divisor.IsZero()) return PositiveInfinity;
            var c = divisor.Real;
            var d = divisor.Imaginary;
            if (Math.Abs(d) <= Math.Abs(c))
                return InternalDiv(dividend, 0, c, d, false);
            return InternalDiv(0, dividend, d, c, true);
        }

        /// <summary>Division operator. Divides a complex number by a float value.</summary>
        /// <returns>The result of the division.</returns>
        /// <param name="dividend">The dividend.</param>
        /// <param name="divisor">The divisor.</param>
        public static Complex32 operator /(Complex32 dividend, float divisor)
        {
            if (dividend.IsZero() && divisor == 0.0f) return NaN;

            if (divisor == 0.0f) return PositiveInfinity;

            return new Complex32(dividend._real / divisor, dividend._imag / divisor);
        }

        /// <summary>
        ///     Computes the conjugate of a complex number and returns the result.
        /// </summary>
        public Complex32 Conjugate()
        {
            return new Complex32(_real, -_imag);
        }


        #region IFormattable Members

        /// <summary>
        ///     Converts the value of the current complex number to its equivalent string representation in Cartesian form.
        /// </summary>
        /// <returns>The string representation of the current instance in Cartesian form.</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "({0}, {1})", _real, _imag);
        }


        /// <summary>
        ///     Converts the value of the current complex number to its equivalent string representation
        ///     in Cartesian form by using the specified format and culture-specific format information for its real and imaginary
        ///     parts.
        /// </summary>
        /// <returns>
        ///     The string representation of the current instance in Cartesian form, as specified by
        ///     <paramref name="format" /> and <paramref name="provider" />.
        /// </returns>
        /// <param name="format">A standard or custom numeric format string.</param>
        /// <param name="provider">An object that supplies culture-specific formatting information.</param>
        /// <exceptioncreT:System.FormatException">
        ///     
        /// 
        /// <paramref name="format" />
        /// is not a valid format string.
        /// </exception>
        public string ToString(string format, IFormatProvider provider)
        {
            return string.Format(provider, "({0}, {1})",
                _real.ToString(format, provider),
                _imag.ToString(format, provider));
        }

        #endregion

        #region IEquatable<Complex32> Members

        /// <summary>
        ///     Checks if two complex numbers are equal. Two complex numbers are equal if their
        ///     corresponding real and imaginary components are equal.
        /// </summary>
        /// <returns>
        ///     Returns <c>true</c> if the two objects are the same object, or if their corresponding
        ///     real and imaginary components are equal, <c>false</c> otherwise.
        /// </returns>
        /// <param name="other">
        ///     The complex number to compare to with.
        /// </param>
        public bool Equals(Complex32 other)
        {
            if (IsNaN() || other.IsNaN()) return false;

            if (IsInfinity() && other.IsInfinity()) return true;

            return _real.AlmostEqual(other._real) && _imag.AlmostEqual(other._imag);
        }

        /// <summary>
        ///     The hash code for the complex number.
        /// </summary>
        /// <returns>
        ///     The hash code of the complex number.
        /// </returns>
        /// <remarks>
        ///     The hash code is calculated as
        ///     System.Math.Exp(ComplexMath.Absolute(complexNumber)).
        /// </remarks>
        public override int GetHashCode()
        {
            var hash = 27;
            hash = 13 * hash + _real.GetHashCode();
            hash = 13 * hash + _imag.GetHashCode();
            return hash;
        }

        /// <summary>
        ///     Checks if two complex numbers are equal. Two complex numbers are equal if their
        ///     corresponding real and imaginary components are equal.
        /// </summary>
        /// <returns>
        ///     Returns <c>true</c> if the two objects are the same object, or if their corresponding
        ///     real and imaginary components are equal, <c>false</c> otherwise.
        /// </returns>
        /// <param name="obj">
        ///     The complex number to compare to with.
        /// </param>
        public override bool Equals(object obj)
        {
            return obj is Complex32 && Equals((Complex32) obj);
        }

        #endregion

        #region Parse Functions

/*
        /// <summary>
        ///     Creates a complex number based on a string. The string can be in the
        ///     following formats (without the quotes): 'n', 'ni', 'n +/- ni',
        ///     'ni +/- n', 'n,n', 'n,ni,' '(n,n)', or '(n,ni)', where n is a float.
        /// </summary>
        /// <returns>
        ///     A complex number containing the value specified by the given string.
        /// </returns>
        /// <param name="value">
        ///     the string to parse.
        /// </param>
        /// <param name="formatProvider">
        ///     An <seecreIFormatProvider" /> that supplies culture-specific
        ///     formatting information.
        /// 
        /// 
        /// </param>
        public static Complex32 Parse(string value, IFormatProvider formatProvider = null)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));

            value = value.Trim();
            if (value.Length == 0) throw new FormatException();

            // strip out parens
            if (value.StartsWith("(", StringComparison.Ordinal))
            {
                if (!value.EndsWith(")", StringComparison.Ordinal))
                {
                    //throw new FormatException();
                }

                value = value.Substring(1, value.Length - 2).Trim();
            }

            // keywords
            var numberFormatInfo = formatProvider.GetNumberFormatInfo();
            var textInfo = formatProvider.GetTextInfo();
            var keywords =
                new[]
                {
                    textInfo.ListSeparator, numberFormatInfo.NaNSymbol,
                    numberFormatInfo.NegativeInfinitySymbol, numberFormatInfo.PositiveInfinitySymbol,
                    "+", "-", "i", "j"
                };

            // lexing
            var tokens = new LinkedList<string>();
            GlobalizationHelper.Tokenize(tokens.AddFirst(value), keywords, 0);
            var token = tokens.First;

            // parse the left part
            bool isLeftPartImaginary;
            var leftPart = ParsePart(ref token, out isLeftPartImaginary, formatProvider);
            if (token == null) return isLeftPartImaginary ? new Complex32(0, leftPart) : new Complex32(leftPart, 0);

            // parse the right part
            if (token.Value == textInfo.ListSeparator)
            {
                // format: real,imag
                token = token.Next;

                if (isLeftPartImaginary)
                {
                    // left must not contain 'i', right doesn't matter.
                    //throw new FormatException();
                }

                bool isRightPartImaginary;
                var rightPart = ParsePart(ref token, out isRightPartImaginary, formatProvider);

                return new Complex32(leftPart, rightPart);
            }
            else
            {
                // format: real + imag
                bool isRightPartImaginary;
                var rightPart = ParsePart(ref token, out isRightPartImaginary, formatProvider);

                if (!(isLeftPartImaginary ^ isRightPartImaginary))
                {
                    // either left or right part must contain 'i', but not both.
                    //throw new FormatException();
                }

                return isLeftPartImaginary ? new Complex32(rightPart, leftPart) : new Complex32(leftPart, rightPart);
            }
        }
*/

        #endregion

        #region Conversion

        /// <summary>
        ///     Explicit conversion of a real decimal to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The decimal value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Complex32(decimal value)
        {
            return new Complex32((float) value, 0.0f);
        }

        /// <summary>
        ///     Explicit conversion of a <c>Complex</c> to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The decimal value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Complex32(Complex value)
        {
            return new Complex32((float) value.Real, (float) value.Imaginary);
        }

        /// <summary>
        ///     Implicit conversion of a real byte to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The byte value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(byte value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real short to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The short value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(short value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a signed byte to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The signed byte value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Complex32(sbyte value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a unsigned real short to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The unsigned short value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Complex32(ushort value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real int to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The int value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(int value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a BigInteger int to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The BigInteger value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(BigInteger value)
        {
            return new Complex32((long) value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real long to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The long value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(long value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real uint to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The uint value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Complex32(uint value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real ulong to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The ulong value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        [CLSCompliant(false)]
        public static implicit operator Complex32(ulong value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real float to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The float value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator Complex32(float value)
        {
            return new Complex32(value, 0.0f);
        }

        /// <summary>
        ///     Implicit conversion of a real double to a <c>Complex32</c>.
        /// </summary>
        /// <param name="value">The double value to convert.</param>
        /// <returns>The result of the conversion.</returns>
        public static explicit operator Complex32(double value)
        {
            return new Complex32((float) value, 0.0f);
        }

        #endregion

/*
        /// <summary>
        ///     Returns the additive inverse of a specified complex number.
        /// </summary>
        /// <returns>The result of the real and imaginary components of the value parameter multiplied by -1.</returns>
        /// <param name="value">A complex number.</param>
        [TargetedPatchingOptOut("Performance critical to inline this type of method across NGen image boundaries")]
        public static Complex32 Negate(Complex32 value)
        {
            return -value;
        }
*/

        /// <summary>
        ///     Adds two complex numbers and returns the result.
        /// </summary>
        /// <returns>The sum of <paramref name="left" /> and <paramref name="right" />.</returns>
        /// <param name="left">The first complex number to add.</param>
        /// <param name="right">The second complex number to add.</param>
        public static Complex32 Add(Complex32 left, Complex32 right)
        {
            return left + right;
        }

        /// <summary>
        ///     Subtracts one complex number from another and returns the result.
        /// </summary>
        /// <returns>The result of subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
        /// <param name="left">The value to subtract from (the minuend).</param>
        /// <param name="right">The value to subtract (the subtrahend).</param>
        public static Complex32 Subtract(Complex32 left, Complex32 right)
        {
            return left - right;
        }

        /// <summary>
        ///     Gets the absolute value (or magnitude) of a complex number.
        /// </summary>
        /// <returns>The absolute value of <paramref name="value" />.</returns>
        /// <param name="value">A complex number.</param>
        public static double Abs(Complex32 value)
        {
            return value.Magnitude;
        }

        /// <summary>
        ///     Returns e raised to the power specified by a complex number.
        /// </summary>
        /// <returns>The number e raised to the power <paramref name="value" />.</returns>
        /// <param name="value">A complex number that specifies a power.</param>
        public static Complex32 Exp(Complex32 value)
        {
            return value.Exponential();
        }
    }
}