using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Graphics.Canvas.UI;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformers.TestApp
{
    public class Layer
    {
        public CanvasBitmap Image;
        public TransformerMatrix TransformerMatrix;
    }

    public sealed partial class MainPage : Page
    {

        TransformerMode TransformerMode;
        Vector2 startingPoint;
        Layer Layer;


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
            //Bitmap
            CanvasBitmap bitmap = await CanvasBitmap.LoadAsync(sender, "Icon/Avatar.jpg");
            Transformer transformer = new Transformer(new TransformerRect(bitmap.SizeInPixels.Width, bitmap.SizeInPixels.Height, new Vector2()));
            TransformerMatrix transformerMatrix = new TransformerMatrix(transformer);


            //Transformer
            Vector2 center = new Vector2((float)this.ActualWidth, (float)this.ActualHeight) / 2;
            float width = center.X;
            float height = center.Y;

            float widthScale = width / bitmap.SizeInPixels.Width;
            float heightScale = height / bitmap.SizeInPixels.Width;
            float scale = Math.Min(widthScale, heightScale);

            float bitmapWidth = scale * bitmap.SizeInPixels.Width;
            float bitmapHeight = scale * bitmap.SizeInPixels.Height;

            float bitmapWidthOver2 = 1.0f / 2.0f * bitmapWidth;
            float bitmapHeightOver2 = 1.0f / 2.0f * bitmapHeight;

            Transformer destination = new Transformer
            {
                LeftTop = center + new Vector2(-bitmapWidthOver2, -bitmapHeightOver2),
                RightTop = center + new Vector2(+bitmapWidthOver2, -bitmapHeightOver2),
                RightBottom = center + new Vector2(+bitmapWidthOver2, +bitmapHeightOver2),
                LeftBottom = center + new Vector2(-bitmapWidthOver2, +bitmapHeightOver2),
            };
            transformerMatrix.Destination = destination;


            //Layer
            Layer layer = new Layer
            {
                TransformerMatrix = transformerMatrix,
                Image = bitmap,
            };
            this.Layer = layer;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            Transformer transformer = this.Layer.TransformerMatrix.Destination;

            args.DrawingSession.DrawImage(new Transform2DEffect
            {
                Source = this.Layer.Image,
                TransformMatrix = this.Layer.TransformerMatrix.GetMatrix(),
            });
            args.DrawingSession.DrawBoundNodes(transformer);
        }



        private void CanvasOperator_Single_Start(Vector2 point)
        {
            this.startingPoint = point;

            //this.Layer.TransformerMatrix._oldDestination = this.Layer.TransformerMatrix.Destination;
            this.Layer.TransformerMatrix.CacheTransform();

            this.TransformerMode = Transformer.ContainsNodeMode(point, this.Layer.TransformerMatrix.Destination);

            this.CanvasControl.Invalidate();
        }
        private void CanvasOperator_Single_Delta(Vector2 point)
        {
            bool isRatio = this.RatioButton.IsOn;
            bool isCenter = this.CenterButton.IsOn;

            //Controller
            Transformer transformer = Transformer.Controller(this.TransformerMode, startingPoint, point, this.Layer.TransformerMatrix._oldDestination, isRatio, isCenter);

            this.Layer.TransformerMatrix.Destination = transformer;

            this.CanvasControl.Invalidate();
        }
        private void CanvasOperator_Single_Complete(Vector2 point)
        {
            this.CanvasControl.Invalidate();
        }

    }
}