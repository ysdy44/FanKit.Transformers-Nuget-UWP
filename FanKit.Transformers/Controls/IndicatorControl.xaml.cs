using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace FanKit.Transformers
{
    /// <summary>
    /// Represents a control that indicates the center point of the transformer.
    /// </summary>
    public sealed partial class IndicatorControl : UserControl
    {
        //@Delegate
        /// <summary> Click on different small squares to trigger **ModeChanged** event. </summary>
        public event IndicatorModeHandler ModeChanged;


        //@VisualState
        IndicatorMode vsMode;
        private VisualState VisualState
        {
            get
            {
                if (this.vsMode == IndicatorMode.None)
                    return this.Normal;
                else
                    return this.Enable;
            }
            set => VisualStateManager.GoToState(this, value.Name, false);
        }

        #region DependencyProperty


        /// <summary> Gets or sets <see cref="IndicatorControl"/>'s mode. </summary>
        public IndicatorMode Mode
        {
            get => this.vsMode;
            set
            {
                if (this.vsMode == value) return;

                this.IsHitTestVisible = (value != IndicatorMode.None);

                this.SetMode(value, this.vsMode);
                this.vsMode = value;
                this.VisualState = this.VisualState; // VisualState
            }
        }


        private IndicatorMode ModeCore
        {
            set
            {
                if (this.vsMode == value) return;

                this.SetMode(value, this.vsMode);
                this.vsMode = value;

                this.ModeChanged?.Invoke(this, value); // Delegate
            }
        }


        private void SetMode(IndicatorMode value, IndicatorMode startingValue)
        {
            switch (startingValue)
            {
                case IndicatorMode.LeftTop: this.Fade(this.LeftTopRectangle); break;
                case IndicatorMode.RightTop: this.Fade(this.RightTopRectangle); break;
                case IndicatorMode.RightBottom: this.Fade(this.RightBottomRectangle); break;
                case IndicatorMode.LeftBottom: this.Fade(this.LeftBottomRectangle); break;

                case IndicatorMode.Left: this.Fade(this.LeftRectangle); break;
                case IndicatorMode.Top: this.Fade(this.TopRectangle); break;
                case IndicatorMode.Right: this.Fade(this.RightRectangle); break;
                case IndicatorMode.Bottom: this.Fade(this.BottomRectangle); break;

                case IndicatorMode.Center: this.Fade(this.CenterRectangle); break;

                default: break;
            }


            switch (value)
            {
                case IndicatorMode.LeftTop: this.Show(this.LeftTopRectangle); break;
                case IndicatorMode.RightTop: this.Show(this.RightTopRectangle); break;
                case IndicatorMode.RightBottom: this.Show(this.RightBottomRectangle); break;
                case IndicatorMode.LeftBottom: this.Show(this.LeftBottomRectangle); break;

                case IndicatorMode.Left: this.Show(this.LeftRectangle); break;
                case IndicatorMode.Top: this.Show(this.TopRectangle); break;
                case IndicatorMode.Right: this.Show(this.RightRectangle); break;
                case IndicatorMode.Bottom: this.Show(this.BottomRectangle); break;

                case IndicatorMode.Center: this.Show(this.CenterRectangle); break;

                default: break;
            }
        }


        #endregion

        /// <summary> Rotating radians. </summary>
        public double Radians { get => this.RotateTransform.Angle; set => this.RotateTransform.Angle = value; }

        //@Constructs
        /// <summary>
        /// Initialize a <see cref="IndicatorControl"/>.
        /// </summary>
        public IndicatorControl()
        {
            this.InitializeComponent();

            // Button
            this.LeftTopButton.Click += (s, e) => this.ModeCore = IndicatorMode.LeftTop;
            this.RightTopButton.Click += (s, e) => this.ModeCore = IndicatorMode.RightTop;
            this.RightBottomButton.Click += (s, e) => this.ModeCore = IndicatorMode.RightBottom;
            this.LeftBottomButton.Click += (s, e) => this.ModeCore = IndicatorMode.LeftBottom;

            this.LeftButton.Click += (s, e) => this.ModeCore = IndicatorMode.Left;
            this.TopButton.Click += (s, e) => this.ModeCore = IndicatorMode.Top;
            this.RightButton.Click += (s, e) => this.ModeCore = IndicatorMode.Right;
            this.BottomButton.Click += (s, e) => this.ModeCore = IndicatorMode.Bottom;

            this.CenterButton.Click += (s, e) => this.ModeCore = IndicatorMode.Center;


            this.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;

                // Size
                float size = (float)System.Math.Min(e.NewSize.Width, e.NewSize.Height);
                this.Size(size);
            };
        }
    }

    public sealed partial class IndicatorControl : UserControl
    {

        private void Show(Rectangle rectangle)
        {
            this.ShowStoryboard.Stop();

            Storyboard.SetTarget(this.ShowDoubleAnimationX, rectangle);
            Storyboard.SetTarget(this.ShowDoubleAnimationY, rectangle);

            this.ShowStoryboard.Begin();
        }
        private void Fade(Rectangle rectangle)
        {
            this.FadeStoryboard.Stop();

            Storyboard.SetTarget(this.FadeDoubleAnimationX, rectangle);
            Storyboard.SetTarget(this.FadeDoubleAnimationY, rectangle);

            this.FadeStoryboard.Begin();
        }

        private void Size(float size)
        {
            float square = size / 3 / 1.4142135623730950488016887242097f; // Root Number 2
            float squareHalf = square / 2;


            // Control
            this.RootGrid.Width = this.RootGrid.Height = size;


            // Rectangle
            this.LeftTopButton.Width = this.LeftTopButton.Height =
            this.RightTopButton.Width = this.RightTopButton.Height =
            this.RightBottomButton.Width = this.RightBottomButton.Height =
            this.LeftBottomButton.Width = this.LeftBottomButton.Height =

            this.LeftButton.Width = this.LeftButton.Height =
            this.TopButton.Width = this.TopButton.Height =
            this.RightButton.Width = this.RightButton.Height =
            this.BottomButton.Width = this.BottomButton.Height =

            this.CenterButton.Width = this.CenterButton.Height = square;


            // Vector
            Vector2 center = new Vector2(size / 2);

            Vector2 leftTop = new Vector2(-square, -square) + center;
            Vector2 rightTop = new Vector2(square, -square) + center;
            Vector2 rightBottom = new Vector2(square, square) + center;
            Vector2 leftBottom = new Vector2(-square, square) + center;

            Vector2 left = new Vector2(-square, 0) + center;
            Vector2 top = new Vector2(0, -square) + center;
            Vector2 right = new Vector2(square, 0) + center;
            Vector2 bottom = new Vector2(0, square) + center;


            // Rectangle
            Canvas.SetLeft(this.CenterButton, center.X - squareHalf); Canvas.SetTop(this.CenterButton, center.Y - squareHalf);

            Canvas.SetLeft(this.LeftTopButton, leftTop.X - squareHalf); Canvas.SetTop(this.LeftTopButton, leftTop.Y - squareHalf);
            Canvas.SetLeft(this.RightTopButton, rightTop.X - squareHalf); Canvas.SetTop(this.RightTopButton, rightTop.Y - squareHalf);
            Canvas.SetLeft(this.RightBottomButton, rightBottom.X - squareHalf); Canvas.SetTop(this.RightBottomButton, rightBottom.Y - squareHalf);
            Canvas.SetLeft(this.LeftBottomButton, leftBottom.X - squareHalf); Canvas.SetTop(this.LeftBottomButton, leftBottom.Y - squareHalf);

            Canvas.SetLeft(this.LeftButton, left.X - squareHalf); Canvas.SetTop(this.LeftButton, left.Y - squareHalf);
            Canvas.SetLeft(this.TopButton, top.X - squareHalf); Canvas.SetTop(this.TopButton, top.Y - squareHalf);
            Canvas.SetLeft(this.RightButton, right.X - squareHalf); Canvas.SetTop(this.RightButton, right.Y - squareHalf);
            Canvas.SetLeft(this.BottomButton, bottom.X - squareHalf); Canvas.SetTop(this.BottomButton, bottom.Y - squareHalf);


            // Line
            this.ForeTopLine.X1 = this.ForeLeftLine.X2 = this.BackTopLine.X1 = this.BackLeftLine.X2 = leftTop.X;
            this.ForeTopLine.Y1 = this.ForeLeftLine.Y2 = this.BackTopLine.Y1 = this.BackLeftLine.Y2 = leftTop.Y;

            this.ForeRightLine.X1 = this.ForeTopLine.X2 = this.BackRightLine.X1 = this.BackTopLine.X2 = rightTop.X;
            this.ForeRightLine.Y1 = this.ForeTopLine.Y2 = this.BackRightLine.Y1 = this.BackTopLine.Y2 = rightTop.Y;

            this.ForeBottomLine.X1 = this.ForeRightLine.X2 = this.BackBottomLine.X1 = this.BackRightLine.X2 = rightBottom.X;
            this.ForeBottomLine.Y1 = this.ForeRightLine.Y2 = this.BackBottomLine.Y1 = this.BackRightLine.Y2 = rightBottom.Y;

            this.ForeLeftLine.X1 = this.ForeBottomLine.X2 = this.BackLeftLine.X1 = this.BackBottomLine.X2 = leftBottom.X;
            this.ForeLeftLine.Y1 = this.ForeBottomLine.Y2 = this.BackLeftLine.Y1 = this.BackBottomLine.Y2 = leftBottom.Y;
        }

    }
}