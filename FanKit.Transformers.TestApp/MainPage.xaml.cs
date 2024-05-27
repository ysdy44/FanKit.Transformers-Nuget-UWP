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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CanvasControl.ReadyToDraw)
            {
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

                Vector2 center = Transform3D(this.Layer.Source.Center, matrix);
                args.DrawingSession.DrawBound(this.Layer.Destination);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.LeftTop, center);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.RightTop, center);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.RightBottom, center);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.LeftBottom, center);
                args.DrawingSession.DrawNode2(this.Layer.Destination.LeftTop);
                args.DrawingSession.DrawNode2(this.Layer.Destination.RightTop);
                args.DrawingSession.DrawNode2(this.Layer.Destination.RightBottom);
                args.DrawingSession.DrawNode2(this.Layer.Destination.LeftBottom);
                args.DrawingSession.DrawNode4(center);
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

                Vector2 center = Vector2.Transform(this.Layer.Source.Center, matrix);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.LeftTop, center);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.RightTop, center);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.RightBottom, center);
                args.DrawingSession.DrawLineDodgerBlue(this.Layer.Destination.LeftBottom, center);
                args.DrawingSession.DrawBoundNodes(this.Layer.Destination);
                args.DrawingSession.DrawNode4(center);
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
                bool isConvexQuadrilateral = this.ConvexQuadrilateralButton.IsOn;
                if (isConvexQuadrilateral)
                {
                    Transformer t = this.Layer.StartingDestination;
                    switch (this.TransformerMode)
                    {
                        case TransformerMode.ScaleLeftTop: this.Layer.Destination.LeftTop = MovePointOnConvexQuadrilateral(point, t.LeftTop, t.RightTop, t.RightBottom, t.LeftBottom, 8); break;
                        case TransformerMode.ScaleRightTop: this.Layer.Destination.RightTop = MovePointOnConvexQuadrilateral(point, t.RightTop, t.RightBottom, t.LeftBottom, t.LeftTop, 8); break;
                        case TransformerMode.ScaleRightBottom: this.Layer.Destination.RightBottom = MovePointOnConvexQuadrilateral(point, t.RightBottom, t.LeftBottom, t.LeftTop, t.RightTop, 8); break;
                        case TransformerMode.ScaleLeftBottom: this.Layer.Destination.LeftBottom = MovePointOnConvexQuadrilateral(point, t.LeftBottom, t.LeftTop, t.RightTop, t.RightBottom, 8); break;
                        default: break;
                    }
                }
                else
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

        private static Vector3 Transform3D(Vector3 p, Matrix4x4 m)
        {
            //return Vector3.Transform(p, m);
            float x = m.M11 * p.X + m.M21 * p.Y + m.M41 * p.Z;
            float y = m.M12 * p.X + m.M22 * p.Y + m.M42 * p.Z;
            float z = m.M14 * p.X + m.M24 * p.Y + m.M44 * p.Z;
            return new Vector3(x, y, z);
        }

        private static Vector2 Transform3D(Vector2 p, Matrix4x4 m)
        {
            //Vector3 v = Vector3.Transform(new Vector3(p, 1), m);
            //return new Vector2(v.X / v.Z, v.Y / v.Z);
            float x = m.M11 * p.X + m.M21 * p.Y + m.M41;
            float y = m.M12 * p.X + m.M22 * p.Y + m.M42;
            float z = m.M14 * p.X + m.M24 * p.Y + m.M44;
            return new Vector2(x / z, y / z);
        }

        private static Vector2 MovePointOnConvexQuadrilateral(Vector2 point, Vector2 leftTop, Vector2 rightTop, Vector2 rightBottom, Vector2 leftBottom, int limitFactor = 8)
        {
            Vector2 limit1 = 2 * limitFactor * Vector2.Normalize(leftTop - rightBottom);
            Vector2 limit2 = limitFactor * Vector2.Normalize(leftBottom - rightTop);
            Vector2 left = limit1 + limit2 + rightTop;
            Vector2 right = limit1 - limit2 + leftBottom;
            Vector2 tag = rightBottom;

            Vector2 foot = point;
            int i = 0;

            do
            {
                Vector2 lineA;
                Vector2 lineB;
                switch (i)
                {
                    case 1:
                        lineA = tag;
                        lineB = right;
                        break;
                    case 2:
                        lineA = left;
                        lineB = tag;
                        break;
                    default:
                        lineA = left;
                        lineB = right;
                        break;
                }

                float bx = lineA.X - lineB.X;
                float by = lineA.Y - lineB.Y;
                float px = lineA.X - foot.X;
                float py = lineA.Y - foot.Y;

                if (bx * py - by * px < 0f)
                {
                    float s = bx * bx + by * by;
                    float p = py * by + px * bx;
                    foot = new Vector2
                    {
                        X = lineA.X - bx * p / s,
                        Y = lineA.Y - by * p / s
                    };
                }
                i++;
            } while (i < 4);

            return foot;
        }

    }
}