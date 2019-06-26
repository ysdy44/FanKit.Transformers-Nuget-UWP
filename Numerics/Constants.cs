// <copyright file="Constants.cs" company="Math.NET">
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

namespace MathNet.Numerics
{
    /// <summary>
    ///     A collection of frequently used mathematical constants.
    /// </summary>
    public static class Constants
    {
        #region Mathematical Constants

        /// <summary>The number e</summary>
        public const double E = 2.7182818284590452353602874713526624977572470937000d;
        
        /// <summary>The number sqrt(2)</summary>
        public const double Sqrt2 = 1.4142135623730950488016887242096980785696718753769d;

        /// <summary>The number pi</summary>
        public const double Pi = 3.1415926535897932384626433832795028841971693993751d;

        /// <summary>The number pi*3/2</summary>
        public const double Pi3Over2 = 4.71238898038468985769396507491925432629575409906266d;

        /// <summary>The number log(sqrt(2*pi))</summary>
        public const double LogSqrt2Pi = 0.91893853320467274178032973640561763986139747363778;

        /// <summary>The number 2/pi</summary>
        public const double TwoInvPi = 0.63661977236758134307553505349005744813783858296182d;

        /// <summary>
        ///     The size of a double in bytes.
        /// </summary>
        public const int SizeOfDouble = sizeof(double);

        /// <summary>
        ///     The size of an int in bytes.
        /// </summary>
        public const int SizeOfInt = sizeof(int);

        /// <summary>
        ///     The size of a float in bytes.
        /// </summary>
        public const int SizeOfFloat = sizeof(float);

        #endregion

        #region UNIVERSAL CONSTANTS

        #endregion

        #region ELECTROMAGNETIC CONSTANTS

        #endregion

        #region ATOMIC AND NUCLEAR CONSTANTS

        #endregion

        #region Scientific Prefixes

        /// <summary>The SI prefix factor corresponding to 1 000 000 000 000</summary>
        public const double Tera = 1e12;


        /// <summary>The SI prefix factor corresponding to 100</summary>
        public const double Hecto = 1e2;

        /// <summary>The SI prefix factor corresponding to 0.01</summary>
        public const double Centi = 1e-2;

        #endregion
    }
}