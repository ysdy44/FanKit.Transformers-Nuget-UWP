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
        public Matrix4x4 GetMatrix3D() => Transformer.FindHomography3D(this.Source, this.Destination);

        public void CacheTransform() => this.StartingDestination = this.Destination;
        public void TransformMultiplies(Matrix3x2 matrix) => this.Destination = this.StartingDestination * matrix;
        public void TransformAdd(Vector2 vector) => this.Destination = this.StartingDestination + vector;
    }

    public sealed partial class MainPage : Page
    {
        bool Is3D => this.ComboBox.SelectedIndex != 0;

        public TransformerMode TransformerMode { get; private set; }
        public Layer Layer { get; private set; }

        Vector2 _startingPoint;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CanvasControl.ReadyToDraw)
            {
                this.RatioButton.IsEnabled = this.CenterButton.IsEnabled = this.ComboBox.SelectedIndex == 0;
                this.Layer.Resize((float)base.ActualWidth, (float)base.ActualHeight);
                this.CanvasControl.Invalidate();
            }
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
            if (Is3D)
            {
                Matrix4x4 matrix = this.Layer.GetMatrix3D();
                this.M11.Text = $"{matrix.M11}"; this.M12.Text = $"{matrix.M12}"; this.M13.Text = $"{matrix.M14}";
                this.M21.Text = $"{matrix.M21}"; this.M22.Text = $"{matrix.M22}"; this.M23.Text = $"{matrix.M24}";
                this.M31.Text = $"{matrix.M41}"; this.M32.Text = $"{matrix.M42}";

                args.DrawingSession.DrawImage(new Transform3DEffect
                {
                    Source = this.Layer.Image,
                    TransformMatrix = matrix
                });

                args.DrawingSession.DrawBound(this.Layer.Destination);
                args.DrawingSession.DrawNode2(this.Layer.Destination.LeftTop);
                args.DrawingSession.DrawNode2(this.Layer.Destination.RightTop);
                args.DrawingSession.DrawNode2(this.Layer.Destination.RightBottom);
                args.DrawingSession.DrawNode2(this.Layer.Destination.LeftBottom);
            }
            else
            {
                Matrix3x2 matrix = this.Layer.GetMatrix();
                this.M11.Text = $"{matrix.M11}"; this.M12.Text = $"{matrix.M12}"; this.M13.Text = "0";
                this.M21.Text = $"{matrix.M21}"; this.M22.Text = $"{matrix.M22}"; this.M23.Text = "0";
                this.M31.Text = $"{matrix.M31}"; this.M32.Text = $"{matrix.M32}";

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

            this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, Is3D);
            this.Layer.CacheTransform();

            this.CanvasControl.Invalidate();
        }
        private void CanvasOperator_Single_Delta(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            if (Is3D)
            {
                switch (this.TransformerMode)
                {
                    case TransformerMode.ScaleLeftTop: this.Layer.Destination.LeftTop = point; break;
                    case TransformerMode.ScaleRightTop: this.Layer.Destination.RightTop = point; break;
                    case TransformerMode.ScaleRightBottom: this.Layer.Destination.RightBottom = point; break;
                    case TransformerMode.ScaleLeftBottom: this.Layer.Destination.LeftBottom = point; break;
                    default: break;
                }
            }
            //Single layer.
            else if (true)
            {
                bool isRatio = this.RatioButton.IsOn;
                bool isCenter = this.CenterButton.IsOn;

                Transformer transformer = Transformer.Controller(this.TransformerMode, this._startingPoint, point, this.Layer.StartingDestination, isRatio, isCenter);
                this.Layer.Destination = transformer;
            }
            //Multiple layer.
            else
            {
                bool isRatio = this.RatioButton.IsOn;
                bool isCenter = this.CenterButton.IsOn;

                Transformer transformer = Transformer.Controller(this.TransformerMode, this._startingPoint, point, this.Layer.StartingDestination, isRatio, isCenter);
                Matrix3x2 matrix = Transformer.FindHomography(this.Layer.StartingDestination, transformer);

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