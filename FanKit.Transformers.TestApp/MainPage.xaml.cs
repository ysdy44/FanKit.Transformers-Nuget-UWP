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
    public class Layer0
    {
        public TransformerBorder Source;
        public TransformerBorder Destination;
        public TransformerBorder StartingDestination;

        public Matrix3x2 GetMatrix() => TransformerBorder.FindHomography(this.Source, this.Destination);

        public void CacheTransform() => this.StartingDestination = this.Destination;
        public void TransformAdd(Vector2 vector) => this.Destination = this.StartingDestination + vector;
    }

    public class Layer1 : ICacheTransform
    {
        public TransformerBorder Source;
        public Transformer Destination;
        public Transformer StartingDestination;

        public Matrix3x2 GetMatrix() => Transformer.FindHomography(this.Source, this.Destination);

        public void CacheTransform() => this.StartingDestination = this.Destination;
        public void TransformMultiplies(Matrix3x2 matrix) => this.Destination = this.StartingDestination * matrix;
        public void TransformAdd(Vector2 vector) => this.Destination = this.StartingDestination + vector;
    }

    public class Layer2
    {
        public TransformerBorder Source;
        public Transformer Destination;

        public Matrix4x4 GetMatrix3D() => Transformer.FindHomography3D(this.Source, this.Destination);
    }

    public sealed partial class MainPage : Page
    {
        int SelectedIndex => this.ComboBox.SelectedIndex;

        private TransformerMode TransformerMode;
        private CanvasBitmap Image;

        readonly Layer0 Layer0 = new Layer0();
        readonly Layer1 Layer1 = new Layer1();
        readonly Layer2 Layer2 = new Layer2();

        Vector2 StartingPoint;

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsLoaded)
            {
                this.RatioButton.IsEnabled = this.SelectedIndex != 2;
                this.CenterButton.IsEnabled = this.SelectedIndex != 2;
                this.ConvexQuadrilateralButton.IsEnabled = this.SelectedIndex == 2;

                this.M12.Opacity = ((this.SelectedIndex > 0) ? 1.0 : 0.5);
                this.M21.Opacity = ((this.SelectedIndex > 0) ? 1.0 : 0.5);
                this.M23.Opacity = ((this.SelectedIndex > 1) ? 1.0 : 0.5);
                this.M13.Opacity = ((this.SelectedIndex > 1) ? 1.0 : 0.5);
            }

            if (this.CanvasControl.ReadyToDraw)
            {
                this.CanvasControl.Invalidate();
            }
        }

        private void CanvasControl_CreateResources(CanvasControl sender, CanvasCreateResourcesEventArgs args)
        {
            args.TrackAsyncAction(this.CreateResourcesAsync(sender).AsAsyncAction());
        }
        private async Task CreateResourcesAsync(CanvasControl sender)
        {
            this.Image = await CanvasBitmap.LoadAsync(sender, "Icon/Avatar.jpg");
            float width = this.Image.SizeInPixels.Width;
            float height = this.Image.SizeInPixels.Height;

            float centerX = (float)base.ActualWidth / 2;
            float centerY = (float)base.ActualHeight / 2;
            float scale = System.Math.Min(centerX / width, centerY / height);

            float bitmapWidthOver2 = scale * width / 2;
            float bitmapHeightOver2 = scale * height / 2;

            TransformerBorder border = new TransformerBorder(width, height);
            TransformerBorder rect = new TransformerBorder(width, height, Vector2.Zero);
            TransformerBorder transformerBorder = new TransformerBorder
            {
                Left = centerX - bitmapWidthOver2,
                Top = centerY - bitmapHeightOver2,
                Right = centerX + bitmapWidthOver2,
                Bottom = centerY + bitmapHeightOver2
            };
            Transformer transformer = new Transformer
            {
                LeftTop = new Vector2(centerX - bitmapWidthOver2, centerY - bitmapHeightOver2),
                RightTop = new Vector2(centerX + bitmapWidthOver2, centerY - bitmapHeightOver2),
                RightBottom = new Vector2(centerX + bitmapWidthOver2, centerY + bitmapHeightOver2),
                LeftBottom = new Vector2(centerX - bitmapWidthOver2, centerY + bitmapHeightOver2),
            };

            this.Layer0.Source = border;
            this.Layer1.Source = rect;
            this.Layer2.Source = rect;

            this.Layer0.StartingDestination = transformerBorder;
            this.Layer1.StartingDestination = transformer;

            this.Layer0.Destination = transformerBorder;
            this.Layer1.Destination = transformer;
            this.Layer2.Destination = transformer;
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
            switch (this.SelectedIndex)
            {
                case 0:
                    {
                        Matrix3x2 matrix = this.Layer0.GetMatrix();

                        this.M11.Text = $"{matrix.M11}"; this.M12.Text = $"{matrix.M12}"; this.M13.Text = "0";
                        this.M21.Text = $"{matrix.M21}"; this.M22.Text = $"{matrix.M22}"; this.M23.Text = "0";
                        this.M31.Text = $"{matrix.M31}"; this.M32.Text = $"{matrix.M32}";

                        args.DrawingSession.DrawImage(new Transform2DEffect
                        {
                            Source = this.Image,
                            TransformMatrix = matrix
                            //TransformMatrix = matrix * this.Matrix
                        });
                        args.DrawingSession.DrawBoundNodes(this.Layer0.Destination);
                    }
                    break;
                case 1:
                    {
                        Matrix3x2 matrix = this.Layer1.GetMatrix();
                        this.M11.Text = $"{matrix.M11}"; this.M12.Text = $"{matrix.M12}"; this.M13.Text = "0";
                        this.M21.Text = $"{matrix.M21}"; this.M22.Text = $"{matrix.M22}"; this.M23.Text = "0";
                        this.M31.Text = $"{matrix.M31}"; this.M32.Text = $"{matrix.M32}";

                        args.DrawingSession.DrawImage(new Transform2DEffect
                        {
                            Source = this.Image,
                            TransformMatrix = matrix,
                        });

                        Vector2 center = Vector2.Transform(this.Layer1.Source.Center, matrix);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer1.Destination.LeftTop, center);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer1.Destination.RightTop, center);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer1.Destination.RightBottom, center);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer1.Destination.LeftBottom, center);
                        args.DrawingSession.DrawBoundNodes(this.Layer1.Destination);
                        args.DrawingSession.DrawNode4(center);
                    }
                    break;
                case 2:
                    {
                        Matrix4x4 matrix = this.Layer2.GetMatrix3D();
                        this.M11.Text = $"{matrix.M11}"; this.M12.Text = $"{matrix.M12}"; this.M13.Text = $"{matrix.M14}";
                        this.M21.Text = $"{matrix.M21}"; this.M22.Text = $"{matrix.M22}"; this.M23.Text = $"{matrix.M24}";
                        this.M31.Text = $"{matrix.M41}"; this.M32.Text = $"{matrix.M42}";

                        args.DrawingSession.DrawImage(new Transform3DEffect
                        {
                            Source = this.Image,
                            TransformMatrix = matrix
                        });

                        Vector2 center = FanKit.Math.Transform3D(this.Layer2.Source.Center, matrix);
                        args.DrawingSession.DrawBound(this.Layer2.Destination);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer2.Destination.LeftTop, center);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer2.Destination.RightTop, center);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer2.Destination.RightBottom, center);
                        args.DrawingSession.DrawLineDodgerBlue(this.Layer2.Destination.LeftBottom, center);
                        args.DrawingSession.DrawNode2(this.Layer2.Destination.LeftTop);
                        args.DrawingSession.DrawNode2(this.Layer2.Destination.RightTop);
                        args.DrawingSession.DrawNode2(this.Layer2.Destination.RightBottom);
                        args.DrawingSession.DrawNode2(this.Layer2.Destination.LeftBottom);
                        args.DrawingSession.DrawNode4(center);
                    }
                    break;
                default:
                    break;
            }
        }



        private void CanvasOperator_Single_Start(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            switch (this.SelectedIndex)
            {
                case 0:
                    {
                        TransformerBorder transformer = this.Layer0.Destination;

                        this.StartingPoint = point;

                        this.TransformerMode = TransformerBorder.ContainsNodeMode(point, transformer);
                        this.Layer0.CacheTransform();

                        this.CanvasControl.Invalidate();
                    }
                    break;
                case 1:
                    {
                        Transformer transformer = this.Layer1.Destination;

                        this.StartingPoint = point;

                        this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, disabledRadian: false);
                        this.Layer1.CacheTransform();

                        this.CanvasControl.Invalidate();
                    }
                    break;
                case 2:
                    {
                        Transformer transformer = this.Layer2.Destination;

                        this.TransformerMode = Transformer.ContainsNodeMode(point, transformer, disabledRadian: true);

                        this.CanvasControl.Invalidate();
                    }
                    break;
            }
        }

        private void CanvasOperator_Single_Delta(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            switch (this.SelectedIndex)
            {
                case 0:
                    {
                        bool isRatio = this.RatioButton.IsOn;
                        bool isCenter = this.CenterButton.IsOn;

                        TransformerBorder transformer = TransformerBorder.Controller(this.TransformerMode, this.StartingPoint, point, this.Layer0.StartingDestination, isRatio, isCenter);
                        this.Layer0.Destination = transformer;
                    }
                    break;
                case 1:
                    //Single layer.
                    if (true)
                    {
                        bool isRatio = this.RatioButton.IsOn;
                        bool isCenter = this.CenterButton.IsOn;

                        Transformer transformer = Transformer.Controller(this.TransformerMode, this.StartingPoint, point, this.Layer1.StartingDestination, isRatio, isCenter);
                        this.Layer1.Destination = transformer;
                    }
                    //Multiple layer.
                    else
                    {
                        bool isRatio = this.RatioButton.IsOn;
                        bool isCenter = this.CenterButton.IsOn;

                        Transformer transformer = Transformer.Controller(this.TransformerMode, this.StartingPoint, point, this.Layer1.StartingDestination, isRatio, isCenter);
                        Matrix3x2 matrix = Transformer.FindHomography(this.Layer1.StartingDestination, transformer);

                        this.Layer1.TransformMultiplies(matrix);
                        //this.Layer2...
                        //this.Layer3...
                    }
                    break;
                case 2:
                    switch (this.TransformerMode)
                    {
                        case TransformerMode.ScaleLeftTop: this.Layer2.Destination = this.Layer2.Destination.MoveLeftTop(point, this.ConvexQuadrilateralButton.IsOn); break;
                        case TransformerMode.ScaleRightTop: this.Layer2.Destination = this.Layer2.Destination.MoveRightTop(point, this.ConvexQuadrilateralButton.IsOn); break;
                        case TransformerMode.ScaleRightBottom: this.Layer2.Destination = this.Layer2.Destination.MoveRightBottom(point, this.ConvexQuadrilateralButton.IsOn); break;
                        case TransformerMode.ScaleLeftBottom: this.Layer2.Destination = this.Layer2.Destination.MoveLeftBottom(point, this.ConvexQuadrilateralButton.IsOn); break;
                        default: break;
                    }
                    break;
            }
            this.CanvasControl.Invalidate();
        }

        private void CanvasOperator_Single_Complete(Vector2 point, InputDevice device, PointerPointProperties properties)
        {
            this.CanvasControl.Invalidate();
        }
    }
}