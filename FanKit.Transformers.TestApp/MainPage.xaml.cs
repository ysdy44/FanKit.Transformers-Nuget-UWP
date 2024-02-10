using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Input;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformers.TestApp
{
    public class Layer : ICacheTransform
    {
        public CanvasBitmap Image;
        public TransformerRect Source;
        public Transformer Destination;
        public Transformer StartingDestination;

        public void Resize(float windowWidth, float windowHeight)
        {
            CanvasBitmap bitmap = this.Image;
            float width = bitmap.SizeInPixels.Width;
            float height = bitmap.SizeInPixels.Height;

            Vector2 center = new Vector2(windowWidth, windowHeight) / 2;
            float widthScale = center.X / width;
            float heightScale = center.Y / height;
            float scale = System.Math.Min(widthScale, heightScale);

            float bitmapWidth = scale * width;
            float bitmapHeight = scale * height;

            float bitmapWidthOver2 = 1.0f / 2.0f * bitmapWidth;
            float bitmapHeightOver2 = 1.0f / 2.0f * bitmapHeight;

            this.Source = new TransformerRect(width, height, Vector2.Zero);
            this.StartingDestination = this.Destination = new Transformer
            {
                LeftTop = center + new Vector2(-bitmapWidthOver2, -bitmapHeightOver2),
                RightTop = center + new Vector2(+bitmapWidthOver2, -bitmapHeightOver2),
                RightBottom = center + new Vector2(+bitmapWidthOver2, +bitmapHeightOver2),
                LeftBottom = center + new Vector2(-bitmapWidthOver2, +bitmapHeightOver2),
            };
        }

        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);

        public void CacheTransform() => this.StartingDestination = this.Destination;
        public void TransformMultiplies(Matrix3x2 matrix) => this.Destination = this.StartingDestination * matrix;
        public void TransformAdd(Vector2 vector) => this.Destination = this.StartingDestination + vector;
    }

    public sealed partial class MainPage : Page
    {
        public TransformerMode TransformerMode { get; private set; }
        public Layer Layer { get; private set; }

        Vector2 _startingPoint;
        Transformer _oldTransformer;

        public MainPage()
        {
            this.InitializeComponent();
        }



        private void CanvasControl_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(this.CreateResourcesAsync(sender).AsAsyncAction());
        }
        private async Task CreateResourcesAsync(CanvasControl sender)
        {
            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(sender, "Icon/Avatar.jpg");
            this.Layer = new Layer
            {
                Image = bitmap
            };
            this.Layer.Resize((float)base.ActualWidth, (float)base.ActualHeight);
        }

        /*
        private Transformer AlignCenter(CanvasBitmap bitmap)
        {
            //Transformer
            Vector2 center = new Vector2((float)this.ActualWidth, (float)this.ActualHeight) / 2;
            float width = center.X;
            float height = center.Y;

            float widthScale = width / bitmap.SizeInPixels.Width;
            float heightScale = height / bitmap.SizeInPixels.Width;
            float scale = System.Math.Min(widthScale, heightScale);

            float bitmapWidth = scale * bitmap.SizeInPixels.Width;
            float bitmapHeight = scale * bitmap.SizeInPixels.Height;

            float bitmapWidthOver2 = 1.0f / 2.0f * bitmapWidth;
            float bitmapHeightOver2 = 1.0f / 2.0f * bitmapHeight;

            return new Transformer
            {
                LeftTop = center + new Vector2(-bitmapWidthOver2, -bitmapHeightOver2),
                RightTop = center + new Vector2(+bitmapWidthOver2, -bitmapHeightOver2),
                RightBottom = center + new Vector2(+bitmapWidthOver2, +bitmapHeightOver2),
                LeftBottom = center + new Vector2(-bitmapWidthOver2, +bitmapHeightOver2),
            };
        }
         */

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            {
                Matrix3x2 matrix = this.Layer.GetMatrix();

                args.DrawingSession.DrawImage(new Transform2DEffect
                {
                    Source = this.Layer.Image,
                    TransformMatrix = matrix,
                });
                args.DrawingSession.DrawBoundNodes(this.Layer.Destination);
            }
        }



        private void CanvasOperator_Single_Start(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            Transformer transformer = this.Layer.Destination;

            this._startingPoint = point;
            this._oldTransformer = transformer;

            this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, false);
            this.Layer.CacheTransform();

            this.CanvasControl.Invalidate();
        }
        private void CanvasOperator_Single_Delta(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            this._startingPoint = point;

            //Single layer.
            if (true)
            {
                bool isRatio = this.RatioButton.IsOn;
                bool isCenter = this.CenterButton.IsOn;

                Transformer transformer = Transformer.Controller(this.TransformerMode, this._startingPoint, point, this._oldTransformer, isRatio, isCenter);
                this.Layer.Destination = transformer;
            }
            //Multiple layer.
            else
            {
                bool isRatio = this.RatioButton.IsOn;
                bool isCenter = this.CenterButton.IsOn;

                Transformer transformer = Transformer.Controller(this.TransformerMode, this._startingPoint, point, this._oldTransformer, isRatio, isCenter);
                Matrix3x2 matrix = Transformer.FindHomography(this._oldTransformer, transformer);

                this.Layer.TransformMultiplies(matrix);
                //this.Layer2...
                //this.Layer3...
            }

            this.CanvasControl.Invalidate();
        }
        private void CanvasOperator_Single_Complete(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            this.CanvasControl.Invalidate();
        }

    }
}