using System.Numerics;

namespace FanKit.Transformers
{
    partial struct TransformerBorder
    {

        /// <summary>
        /// It controls the transformation of border.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        /// <param name="startingBorder"> The starting border. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <returns> The controlled border. </returns>
        public static TransformerBorder Controller(TransformerMode mode, Vector2 startingPoint, Vector2 point, TransformerBorder startingBorder, bool isRatio = false, bool isCenter = false)
        {
            switch (mode)
            {
                case TransformerMode.None:
                case TransformerMode.Rotation:
                case TransformerMode.SkewLeft:
                case TransformerMode.SkewTop:
                case TransformerMode.SkewRight:
                case TransformerMode.SkewBottom:
                    return startingBorder;
                case TransformerMode.ScaleLeft:
                    {
                        float right = isCenter ? (startingBorder.Left + startingBorder.Right - point.X) : startingBorder.Right;
                        if (isRatio)
                        {
                            float scale = isCenter ? ((startingBorder.CenterX - point.X) * startingBorder.Height / startingBorder.Width) : ((startingBorder.Right - point.X) * startingBorder.Height / startingBorder.Width / 2f);
                            return new TransformerBorder(point.X, startingBorder.CenterY - scale, right, startingBorder.CenterY + scale);
                        }
                        else
                            return new TransformerBorder(point.X, startingBorder.Top, right, startingBorder.Bottom);
                    }
                case TransformerMode.ScaleTop:
                    {
                        float bottom = isCenter ? (startingBorder.Top + startingBorder.Bottom - point.Y) : startingBorder.Bottom;
                        if (isRatio)
                        {
                            float scale = isCenter ? ((startingBorder.CenterY - point.Y) * startingBorder.Width / startingBorder.Height) : ((startingBorder.Bottom - point.Y) * startingBorder.Width / startingBorder.Height / 2f);
                            return new TransformerBorder(startingBorder.CenterX - scale, point.Y, startingBorder.CenterX + scale, bottom);
                        }
                        else
                            return new TransformerBorder(startingBorder.Left, point.Y, startingBorder.Right, bottom);
                    }
                case TransformerMode.ScaleRight:
                    {
                        float left = isCenter ? (startingBorder.Left + startingBorder.Right - point.X) : startingBorder.Left;
                        if (isRatio)
                        {
                            float scale = isCenter ? ((startingBorder.CenterX - point.X) * startingBorder.Height / startingBorder.Width) : ((startingBorder.Left - point.X) * startingBorder.Height / startingBorder.Width / 2f);
                            return new TransformerBorder(left, startingBorder.CenterY + scale, point.X, startingBorder.CenterY - scale);
                        }
                        else
                            return new TransformerBorder(left, startingBorder.Top, point.X, startingBorder.Bottom);
                    }
                case TransformerMode.ScaleBottom:
                    {
                        float top = isCenter ? (startingBorder.Top + startingBorder.Bottom - point.Y) : startingBorder.Top;
                        if (isRatio)
                        {
                            float scale = isCenter ? ((startingBorder.CenterY - point.Y) * startingBorder.Width / startingBorder.Height) : ((startingBorder.Top - point.Y) * startingBorder.Width / startingBorder.Height / 2f);
                            return new TransformerBorder(startingBorder.CenterX + scale, top, startingBorder.CenterX - scale, point.Y);
                        }
                        else
                            return new TransformerBorder(startingBorder.Left, top, startingBorder.Right, point.Y);
                    }
                case TransformerMode.ScaleLeftTop:
                    if (isRatio)
                    {
                        if (isCenter)
                            return TransformerBorder.ScaleAroundCenter(point, startingBorder, startingBorder.Left, startingBorder.Top);
                        else
                            return TransformerBorder.ScaleAround(point, startingBorder, startingBorder.Left, startingBorder.Top, startingBorder.Right, startingBorder.Bottom);
                    }
                    else if (isCenter)
                        return new TransformerBorder(point.X, point.Y, startingBorder.Left + startingBorder.Right - point.X, startingBorder.Top + startingBorder.Bottom - point.Y);
                    else
                        return new TransformerBorder(point.X, point.Y, startingBorder.Right, startingBorder.Bottom);
                case TransformerMode.ScaleRightTop:
                    if (isRatio)
                    {
                        if (isCenter)
                            return TransformerBorder.ScaleAroundCenter(point, startingBorder, startingBorder.Right, startingBorder.Top);
                        else
                            return TransformerBorder.ScaleAround(point, startingBorder, startingBorder.Right, startingBorder.Top, startingBorder.Left, startingBorder.Bottom);
                    }
                    else if (isCenter)
                        return new TransformerBorder(startingBorder.Left + startingBorder.Right - point.X, point.Y, point.X, startingBorder.Top + startingBorder.Bottom - point.Y);
                    else
                        return new TransformerBorder(startingBorder.Left, point.Y, point.X, startingBorder.Bottom);
                case TransformerMode.ScaleRightBottom:
                    if (isRatio)
                    {
                        if (isCenter)
                            return TransformerBorder.ScaleAroundCenter(point, startingBorder, startingBorder.Right, startingBorder.Bottom);
                        else
                            return TransformerBorder.ScaleAround(point, startingBorder, startingBorder.Right, startingBorder.Bottom, startingBorder.Left, startingBorder.Top);
                    }
                    else if (isCenter)
                        return new TransformerBorder(startingBorder.Left + startingBorder.Right - point.X, startingBorder.Top + startingBorder.Bottom - point.Y, point.X, point.Y);
                    else
                        return new TransformerBorder(startingBorder.Left, startingBorder.Top, point.X, point.Y);
                case TransformerMode.ScaleLeftBottom:
                    if (isRatio)
                    {
                        if (isCenter)
                            return TransformerBorder.ScaleAroundCenter(point, startingBorder, startingBorder.Left, startingBorder.Bottom);
                        else
                            return TransformerBorder.ScaleAround(point, startingBorder, startingBorder.Left, startingBorder.Bottom, startingBorder.Right, startingBorder.Top);
                    }
                    else if (isCenter)
                        return new TransformerBorder(point.X, startingBorder.Top + startingBorder.Bottom - point.Y, startingBorder.Left + startingBorder.Right - point.X, point.Y);
                    else
                        return new TransformerBorder(point.X, startingBorder.Top, startingBorder.Right, point.Y);
                default:
                    return startingBorder;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static TransformerBorder ScaleAroundCenter(Vector2 point, TransformerBorder startingBorder, float linePointX, float linePointY)
        {
            float centerX = startingBorder.CenterX;
            float centerY = startingBorder.CenterY;
            Vector2 vector = Math.FootPoint(point, linePointX, linePointY, centerX, centerY);
            float num = new LineDistance(vector.X, vector.Y, linePointX, linePointY, centerX, centerY);
            return startingBorder.Scale(num, num, centerX, centerY);
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static TransformerBorder ScaleAround(Vector2 point, TransformerBorder startingBorder, float linePointX, float linePointY, float centerX, float centerY)
        {
            Vector2 vector = Math.FootPoint(point, linePointX, linePointY, centerX, centerY);
            float num = new LineDistance(vector.X, vector.Y, linePointX, linePointY, centerX, centerY);
            return startingBorder.Scale(num, num, centerX, centerY);
        }

    }
}