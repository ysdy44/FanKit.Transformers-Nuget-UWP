﻿using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {
        /// <summary>
        /// Gets the radians of rotation.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <returns> The product radians. </returns>
        public static float GetRadians(Vector2 centerPoint)
        {
            float radians = FanKit.Math.VectorToRadians(centerPoint);
            if (float.IsNaN(radians)) return 0.0f;

            float value = radians * 180.0f / FanKit.Math.Pi;
            return value % 180.0f;
        }

        /// <summary>
        /// Gets the radians of skew.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="radians"> The radians of rotation. </param>
        /// <returns> The product radians. </returns>
        public static float GetSkew(Vector2 centerPoint, float radians)
        {
            float skew = FanKit.Math.VectorToRadians(centerPoint);
            if (float.IsNaN(skew)) return 0;

            skew = skew * 180.0f / FanKit.Math.Pi;
            skew = skew - radians - 90.0f;

            return skew % 180.0f;
        }


        /// <summary>
        /// X
        /// </summary>
        /// <param name="value"> The destination value. </param>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <returns> The produced vector. </returns>
        public Vector2 TransformX(float value, IndicatorMode indicatorMode = IndicatorMode.LeftTop)
        {
            Vector2 indicatorVector = this.GetIndicatorVector(indicatorMode);

            float x = value - indicatorVector.X;

            return new Vector2(x, 0);
        }

        /// <summary>
        /// Y
        /// </summary>
        /// <param name="value"> The destination value. </param>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <returns> The produced vector. </returns>
        public Vector2 TransformY(float value, IndicatorMode indicatorMode = IndicatorMode.LeftTop)
        {
            Vector2 indicatorVector = this.GetIndicatorVector(indicatorMode);

            float y = value - indicatorVector.Y;

            return new Vector2(0, y);
        }


        /// <summary>
        /// Width
        /// </summary>
        /// <param name="value"> The destination value. </param>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        /// <returns> The produced matrix. </returns>
        public Matrix3x2 TransformWidth(float value, IndicatorMode indicatorMode = IndicatorMode.LeftTop, bool isRatio = false)
        {
            Vector2 horizontal = this.Horizontal;
            Vector2 indicatorVector = this.GetIndicatorVector(indicatorMode);

            float width = horizontal.Length();
            float radian = FanKit.Math.VectorToRadians(this.CenterTop - this.Center);
            float scale = value / width;

            return
                Matrix3x2.CreateRotation(-radian, indicatorVector) *
                Matrix3x2.CreateScale(isRatio ? scale : 1, scale, indicatorVector) *
                Matrix3x2.CreateRotation(radian, indicatorVector);
        }

        /// <summary>
        /// Height
        /// </summary>
        /// <param name="value"> The destination value. </param>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <param name="isRatio"> Maintain a ratio when scaling. </param>
        /// <returns> The produced matrix. </returns>
        public Matrix3x2 TransformHeight(float value, IndicatorMode indicatorMode = IndicatorMode.LeftTop, bool isRatio = false)
        {
            Vector2 vertical = this.Vertical;
            Vector2 indicatorVector = this.GetIndicatorVector(indicatorMode);

            float height = vertical.Length();
            float radian = FanKit.Math.VectorToRadians(this.CenterTop - this.Center);
            float scale = value / height;

            return
            Matrix3x2.CreateRotation(-radian, indicatorVector) *
            Matrix3x2.CreateScale(scale, isRatio ? scale : 1, indicatorVector) *
            Matrix3x2.CreateRotation(radian, indicatorVector);
        }


        /// <summary>
        /// Rotate
        /// </summary>
        /// <param name="value"> The destination value. </param>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <returns> The produced matrix. </returns>
        public Matrix3x2 TransformRotate(float value, IndicatorMode indicatorMode = IndicatorMode.LeftTop)
        {
            Vector2 indicatorVector = this.GetIndicatorVector(indicatorMode);

            float value2 = value / 180.0f * FanKit.Math.Pi;
            float radian = FanKit.Math.VectorToRadians(this.CenterTop - this.Center);
            float radians = value2 - radian - FanKit.Math.PiOver2;

            return Matrix3x2.CreateRotation(radians, indicatorVector);
        }

        /// <summary>
        /// Skew
        /// </summary>
        /// <param name="value"> The destination value. </param>
        /// <param name="indicatorMode"> The indicator mode. </param>
        /// <returns> The produced matrix. </returns>
        public Matrix3x2 TransformSkew(float value, IndicatorMode indicatorMode = IndicatorMode.LeftTop)
        {
            Vector2 horizontal = this.Horizontal;
            float horizontalHalf = horizontal.Length() / 2;

            Vector2 footPoint = FanKit.Math.FootPoint(this.Center, this.LeftBottom, this.RightBottom);
            float verticalHalf = Vector2.Distance(this.Center, footPoint);

            float radians = Transformer.GetRadians(horizontal) / 180.0f * FanKit.Math.Pi;
            float skew = -value / 180.0f * FanKit.Math.Pi;


            //Vector2
            Vector2 postion;
            Vector2 center;
            switch (indicatorMode)
            {
                case IndicatorMode.LeftTop:
                case IndicatorMode.Top:
                case IndicatorMode.RightTop:
                    postion = new Vector2(-horizontalHalf, 0);
                    center = this.CenterTop;
                    break;
                case IndicatorMode.LeftBottom:
                case IndicatorMode.Bottom:
                case IndicatorMode.RightBottom:
                    postion = new Vector2(-horizontalHalf, -verticalHalf * 2);
                    center = this.CenterBottom;
                    break;
                default:
                    postion = new Vector2(-horizontalHalf, -verticalHalf);
                    center = this.Center;
                    break;
            }


            //Matrix
            Matrix3x2 skewMatrix =
            Matrix3x2.CreateSkew(skew, 0) *
            Matrix3x2.CreateRotation(radians) *
            Matrix3x2.CreateTranslation(center);

            Transformer zeroTransformer = new Transformer(horizontalHalf * 2, verticalHalf * 2, postion);

            Transformer transformer = Transformer.Multiplies(zeroTransformer, skewMatrix);


            return Transformer.FindHomography(transformer, this);
        }


    }
}