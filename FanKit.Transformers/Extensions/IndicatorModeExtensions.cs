using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "IndicatorMode" />.
    /// </summary>
    public static partial class IndicatorModeExtensions
    {

        /// <summary>
        /// Convert to <see cref="HorizontalAlignment"/>.
        /// </summary>
        /// <param name="indicatorMode"> The indicato rmode. </param>
        /// <returns> The produced alignment. </returns>
        public static HorizontalAlignment ToHorizontalAlignment(this IndicatorMode  indicatorMode)
        {
            switch (indicatorMode)
            {
                case IndicatorMode.LeftTop:
                case IndicatorMode.Left: 
                case IndicatorMode.LeftBottom:
                    return HorizontalAlignment.Left;

                case IndicatorMode.Top:
                case IndicatorMode.Center: 
                case IndicatorMode.Bottom:
                    return HorizontalAlignment.Center;

                case IndicatorMode.RightTop: 
                case IndicatorMode.Right: 
                case IndicatorMode.RightBottom:
                    return HorizontalAlignment.Right;

                default:
                    return HorizontalAlignment.Center;
            }
        }

        /// <summary>
        /// Convert to <see cref="VerticalAlignment"/>.
        /// </summary>
        /// <param name="indicatorMode"> The indicato rmode. </param>
        /// <returns> The produced alignment. </returns>
        public static VerticalAlignment ToVerticalAlignment(this IndicatorMode indicatorMode)
        {
            switch (indicatorMode)
            {
                case IndicatorMode.LeftTop: 
                case IndicatorMode.RightTop: 
                case IndicatorMode.Top:
                    return VerticalAlignment.Top;

                case IndicatorMode.Center: 
                case IndicatorMode.Left: 
                case IndicatorMode.Right:
                    return VerticalAlignment.Center;

                case IndicatorMode.RightBottom:
                case IndicatorMode.LeftBottom: 
                case IndicatorMode.Bottom:
                    return VerticalAlignment.Bottom;

                default:
                    return VerticalAlignment.Center;
            }
        }

    }
}