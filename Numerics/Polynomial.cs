using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

#if !NETSTANDARD1_3

#endif

namespace MathNet.Numerics
{
    /// <summary>
    ///     A single-variable polynomial with real-valued coefficients and non-negative exponents.
    /// </summary>
    [DataContract(Namespace = "urn:MathNet/Numerics")]
    public class Polynomial : IFormattable, IEquatable<Polynomial>
    {
        /// <summary>
        ///     Only needed for the ToString method
        /// </summary>
        [DataMember(Order = 2)] public string VariableName = "x";

        /// <summary>
        ///     Create a zero-polynomial with a coefficient array of the given length.
        ///     An array of length N can support polynomials of a degree of at most N-1.
        /// </summary>
        /// <param name="n">Length of the coefficient array</param>
        public Polynomial(int n)
        {
            if (n < 0) throw new ArgumentOutOfRangeException(nameof(n), "n must be non-negative");

            Coefficients = new double[n];
        }

        /// <summary>
        ///     Create a zero-polynomial
        /// </summary>
        public Polynomial()
        {
#if NET40
            Coefficients = new double[0];
#else
            Coefficients = Array.Empty<double>();
#endif
        }

        /// <summary>
        ///     Create a constant polynomial.
        ///     Example: 3.0 -> "p : x -> 3.0"
        /// </summary>
        /// <param name="coefficient">The coefficient of the "x^0" monomial.</param>
        public Polynomial(double coefficient)
        {
            if (coefficient == 0.0)
            {
#if NET40
                Coefficients = new double[0];
#else
                Coefficients = Array.Empty<double>();
#endif
            }
            else
            {
                Coefficients = new[] {coefficient};
            }
        }

        /// <summary>
        ///     Create a polynomial with the provided coefficients (in ascending order, where the index matches the exponent).
        ///     Example: {5, 0, 2} -> "p : x -> 5 + 0 x^1 + 2 x^2".
        /// </summary>
        /// <param name="coefficients">Polynomial coefficients as array</param>
        public Polynomial(params double[] coefficients)
        {
            Coefficients = coefficients;
        }

        /// <summary>
        ///     Create a polynomial with the provided coefficients (in ascending order, where the index matches the exponent).
        ///     Example: {5, 0, 2} -> "p : x -> 5 + 0 x^1 + 2 x^2".
        /// </summary>
        /// <param name="coefficients">Polynomial coefficients as enumerable</param>
        public Polynomial(IEnumerable<double> coefficients) : this(coefficients.ToArray())
        {
        }

        /// <summary>
        ///     The coefficients of the polynomial in a
        /// </summary>
        [DataMember(Order = 1)]
        public double[] Coefficients { get; private set; }

        /// <summary>
        ///     Degree of the polynomial, i.e. the largest monomial exponent. For example, the degree of y=x^2+x^5 is 5, for y=3 it
        ///     is 0.
        ///     The null-polynomial returns degree -1 because the correct degree, negative infinity, cannot be represented by
        ///     integers.
        /// </summary>
        public int Degree => EvaluateDegree(Coefficients);

        public static Polynomial Zero => new Polynomial();


        private static int EvaluateDegree(double[] coefficients)
        {
            for (var i = coefficients.Length - 1; i >= 0; i--)
                if (coefficients[i] != 0.0)
                    return i;

            return -1;
        }

        #region Evaluation

        /// <summary>
        ///     Evaluate a polynomial at point x.
        ///     Coefficients are ordered ascending by power with power k at index k.
        ///     Example: coefficients [3,-1,2] represent y=2x^2-x+3.
        /// </summary>
        /// <param name="z">The location where to evaluate the polynomial at.</param>
        /// <param name="coefficients">The coefficients of the polynomial, coefficient for power k at index k.</param>
        public static double Evaluate(double z, params double[] coefficients)
        {
            var sum = coefficients[coefficients.Length - 1];
            for (var i = coefficients.Length - 2; i >= 0; --i)
            {
                sum *= z;
                sum += coefficients[i];
            }

            return sum;
        }

        #endregion

        #region Arithmetic Instance Methods (forwarders)

        #endregion

        #region Calculus

        #endregion

        #region Linear Algebra

        #endregion

        #region Arithmetic Operations

        /// <summary>
        ///     Addition of two Polynomials (point-wise).
        /// </summary>
        /// <param name="a">Left Polynomial</param>
        /// <param name="b">Right Polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial Add(Polynomial a, Polynomial b)
        {
            var ac = a.Coefficients;
            var bc = b.Coefficients;

            var degree = Math.Max(a.Degree, b.Degree);
            var result = new double[degree + 1];

            var commonLength = Math.Min(Math.Min(ac.Length, bc.Length), result.Length);
            for (var i = 0; i < commonLength; i++) result[i] = ac[i] + bc[i];

            var acLength = Math.Min(ac.Length, result.Length);
            for (var i = commonLength; i < acLength; i++)
                // no need to add since only one of both applies
                result[i] = ac[i];

            var bcLength = Math.Min(bc.Length, result.Length);
            for (var i = commonLength; i < bcLength; i++)
                // no need to add since only one of both applies
                result[i] = bc[i];

            return new Polynomial(result);
        }

        /// <summary>
        ///     Addition of a polynomial and a scalar.
        /// </summary>
        public static Polynomial Add(Polynomial a, double b)
        {
            var ac = a.Coefficients;

            var degree = Math.Max(a.Degree, 0);
            var result = new double[degree + 1];

            var commonLength = Math.Min(ac.Length, result.Length);
            for (var i = 0; i < commonLength; i++) result[i] = ac[i];

            result[0] += b;

            return new Polynomial(result);
        }

        /// <summary>
        ///     Subtraction of two Polynomials (point-wise).
        /// </summary>
        /// <param name="a">Left Polynomial</param>
        /// <param name="b">Right Polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial Subtract(Polynomial a, Polynomial b)
        {
            var ac = a.Coefficients;
            var bc = b.Coefficients;

            var degree = Math.Max(a.Degree, b.Degree);
            var result = new double[degree + 1];

            var commonLength = Math.Min(Math.Min(ac.Length, bc.Length), result.Length);
            for (var i = 0; i < commonLength; i++) result[i] = ac[i] - bc[i];

            var acLength = Math.Min(ac.Length, result.Length);
            for (var i = commonLength; i < acLength; i++)
                // no need to add since only one of both applies
                result[i] = ac[i];

            var bcLength = Math.Min(bc.Length, result.Length);
            for (var i = commonLength; i < bcLength; i++)
                // no need to add since only one of both applies
                result[i] = -bc[i];

            return new Polynomial(result);
        }

        /// <summary>
        ///     Addition of a scalar from a polynomial.
        /// </summary>
        public static Polynomial Subtract(Polynomial a, double b)
        {
            return Add(a, -b);
        }

        /// <summary>
        ///     Addition of a polynomial from a scalar.
        /// </summary>
        public static Polynomial Subtract(double b, Polynomial a)
        {
            var ac = a.Coefficients;

            var degree = Math.Max(a.Degree, 0);
            var result = new double[degree + 1];

            var commonLength = Math.Min(ac.Length, result.Length);
            for (var i = 0; i < commonLength; i++) result[i] = -ac[i];

            result[0] += b;

            return new Polynomial(result);
        }

        /// <summary>
        ///     Negation of a polynomial.
        /// </summary>
        public static Polynomial Negate(Polynomial a)
        {
            var ac = a.Coefficients;

            var degree = a.Degree;
            var result = new double[degree + 1];

            for (var i = 0; i < result.Length; i++) result[i] = -ac[i];

            return new Polynomial(result);
        }

        /// <summary>
        ///     Multiplies a polynomial by a polynomial (convolution)
        /// </summary>
        /// <param name="a">Left polynomial</param>
        /// <param name="b">Right polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial Multiply(Polynomial a, Polynomial b)
        {
            var ad = a.Degree;
            var bd = b.Degree;
            var ac = a.Coefficients;
            var bc = b.Coefficients;

            var degree = ad + bd;
            var result = new double[degree + 1];

            for (var i = 0; i <= ad; i++)
            for (var j = 0; j <= bd; j++)
                result[i + j] += ac[i] * bc[j];

            return new Polynomial(result);
        }

        /// <summary>
        ///     Scales a polynomial by a scalar
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <param name="k">Scalar value</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial Multiply(Polynomial a, double k)
        {
            var ac = a.Coefficients;

            var result = new double[a.Degree + 1];
            for (var i = 0; i < result.Length; i++) result[i] = ac[i] * k;

            return new Polynomial(result);
        }

        /// <summary>
        ///     Scales a polynomial by division by a scalar
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <param name="k">Scalar value</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial Divide(Polynomial a, double k)
        {
            var ac = a.Coefficients;

            var result = new double[a.Degree + 1];
            for (var i = 0; i < result.Length; i++) result[i] = ac[i] / k;

            return new Polynomial(result);
        }

        #endregion

        #region Arithmetic Pointwise Operations

        #endregion

        #region Arithmetic Operator Overloads (forwarders)

        /// <summary>
        ///     Addition of two Polynomials (piecewise)
        /// </summary>
        /// <param name="a">Left polynomial</param>
        /// <param name="b">Right polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator +(Polynomial a, Polynomial b)
        {
            return Add(a, b);
        }

        /// <summary>
        ///     adds a scalar to a polynomial.
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <param name="k">Scalar value</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator +(Polynomial a, double k)
        {
            return Add(a, k);
        }

        /// <summary>
        ///     adds a scalar to a polynomial.
        /// </summary>
        /// <param name="k">Scalar value</param>
        /// <param name="a">Polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator +(double k, Polynomial a)
        {
            return Add(a, k);
        }

        /// <summary>
        ///     Subtraction of two polynomial.
        /// </summary>
        /// <param name="a">Left polynomial</param>
        /// <param name="b">Right polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator -(Polynomial a, Polynomial b)
        {
            return Subtract(a, b);
        }

        /// <summary>
        ///     Subtracts a scalar from a polynomial.
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <param name="k">Scalar value</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator -(Polynomial a, double k)
        {
            return Subtract(a, k);
        }

        /// <summary>
        ///     Subtracts a polynomial from a scalar.
        /// </summary>
        /// <param name="k">Scalar value</param>
        /// <param name="a">Polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator -(double k, Polynomial a)
        {
            return Subtract(k, a);
        }

        /// <summary>
        ///     Negates a polynomial.
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator -(Polynomial a)
        {
            return Negate(a);
        }

        /// <summary>
        ///     Multiplies a polynomial by a polynomial (convolution).
        /// </summary>
        /// <param name="a">Left polynomial</param>
        /// <param name="b">Right polynomial</param>
        /// <returns>resulting Polynomial</returns>
        public static Polynomial operator *(Polynomial a, Polynomial b)
        {
            return Multiply(a, b);
        }

        /// <summary>
        ///     Multiplies a polynomial by a scalar.
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <param name="k">Scalar value</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator *(Polynomial a, double k)
        {
            return Multiply(a, k);
        }

        /// <summary>
        ///     Multiplies a polynomial by a scalar.
        /// </summary>
        /// <param name="k">Scalar value</param>
        /// <param name="a">Polynomial</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator *(double k, Polynomial a)
        {
            return Multiply(a, k);
        }

        /// <summary>
        ///     Divides a polynomial by scalar value.
        /// </summary>
        /// <param name="a">Polynomial</param>
        /// <param name="k">Scalar value</param>
        /// <returns>Resulting Polynomial</returns>
        public static Polynomial operator /(Polynomial a, double k)
        {
            return Divide(a, k);
        }

        #endregion

        #region ToString

        /// <summary>
        ///     Format the polynomial in ascending order, e.g. "4.3 + 2.0x^2 - x^3".
        /// </summary>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }


        /// <summary>
        ///     Format the polynomial in ascending order, e.g. "4.3 + 2.0x^2 - x^3".
        /// </summary>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (Degree < 0) return "0";

            var sb = new StringBuilder();
            var first = true;
            for (var i = 0; i < Coefficients.Length; i++)
            {
                var c = Coefficients[i];
                if (c == 0.0) continue;

                if (first)
                {
                    sb.Append(c.ToString(format, formatProvider));
                    if (i > 0) sb.Append(VariableName);

                    if (i > 1)
                    {
                        sb.Append("^");
                        sb.Append(i);
                    }

                    first = false;
                }
                else
                {
                    if (c < 0.0)
                    {
                        sb.Append(" - ");
                        sb.Append((-c).ToString(format, formatProvider));
                    }
                    else
                    {
                        sb.Append(" + ");
                        sb.Append(c.ToString(format, formatProvider));
                    }

                    if (i > 0) sb.Append(VariableName);

                    if (i > 1)
                    {
                        sb.Append("^");
                        sb.Append(i);
                    }
                }
            }

            return sb.ToString();
        }

        #endregion

        #region Equality

        public bool Equals(Polynomial other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            var n = Degree;
            if (n != other.Degree) return false;

            for (var i = 0; i <= n; i++)
                if (!Coefficients[i].Equals(other.Coefficients[i]))
                    return false;

            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(Polynomial)) return false;
            return Equals((Polynomial) obj);
        }

        public override int GetHashCode()
        {
            var hashNum = Math.Min(Degree + 1, 25);
            var hash = 17;
            unchecked
            {
                for (var i = 0; i < hashNum; i++) hash = hash * 31 + Coefficients[i].GetHashCode();
            }

            return hash;
        }

        #endregion

        #region Clone

        public Polynomial Clone()
        {
            var degree = EvaluateDegree(Coefficients);
            var coefficients = new double[degree + 1];
            Array.Copy(Coefficients, coefficients, coefficients.Length);
            return new Polynomial(coefficients);
        }
        

        #endregion
    }
}