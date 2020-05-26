using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FanKit.Transformers
{
    /// <summary>
    /// Transformed remote control.
    /// </summary>
    public sealed partial class RemoteControl : UserControl
    {
        //@Delegate
        /// <summary> Occurs when the value change starts. </summary>
        public event RemoteVectorHandler ValueChangeStarted;
        /// <summary> Occurs when value change. </summary>
        public event RemoteVectorHandler ValueChangeDelta;
        /// <summary> Occurs when the value change is complete. </summary>
        public event RemoteVectorHandler ValueChangeCompleted;
        

        /// <summary> Click the button around to trigger **Moved** event. </summary>
        public event RemoteVectorHandler Moved;


        //Size
         Size _rootGrigSize;
         readonly Size _canvasSize = new Size(246, 246);
         readonly Size _manipulationSize = new Size(140, 140);
         readonly Size _remoteSize = new Size(40, 40);

        /// <summary>
        /// Remote's Center.
        /// </summary>
        public Vector2 RemoteCenter
        {
           get => this.remoteCenter;
            set
            {
                Vector2 vector = value;

                float length = value.Length() * 2;
                float manipulationEdge = (float)System.Math.Sqrt((this._manipulationSize.Width - this.RemoteEllipse.Width) * (this._manipulationSize.Height - this.RemoteEllipse.Height));

                // Exceeding the edge?
                if (length > manipulationEdge)
                {
                    // Fixed at the edge
                    vector = value * manipulationEdge / length;
                }

                //Canvas
                Canvas.SetLeft(this.RemoteEllipse, vector.X + (this._canvasSize.Width - this._remoteSize.Width) / 2);
                Canvas.SetTop(this.RemoteEllipse, vector.Y + (this._canvasSize.Height - this._remoteSize.Height) / 2);

                this.remoteCenter = value;
            }
        }
        private Vector2 remoteCenter;

        private Vector2 _manipulationCenter
        {
            set
            {
                Canvas.SetLeft(this.ManipulationEllipse, value.X + (this._canvasSize.Width - this._manipulationSize.Width) / 2);
                Canvas.SetTop(this.ManipulationEllipse, value.Y + (this._canvasSize.Height - this._manipulationSize.Height) / 2);
            }
        }

        private bool _isManipulation
        {
            set
            {
                if (value)
                {
                    this.PressedManipulationStoryboard.Begin();

                    this.LeftPath.IsHitTestVisible = false;
                    this.TopPath.IsHitTestVisible = false;
                    this.RightPath.IsHitTestVisible = false;
                    this.BottomPath.IsHitTestVisible = false;
                }
                else
                {
                    this.NormalManipulationStoryboard.Begin();

                    this.LeftPath.IsHitTestVisible = true;
                    this.TopPath.IsHitTestVisible = true;
                    this.RightPath.IsHitTestVisible = true;
                    this.BottomPath.IsHitTestVisible = true;
                }
            }
        }

        Vector2 _vector;


        //@Constructs
        /// <summary>
        /// Initialize a <see cref="RemoteControl"/>.
        /// </summary>
        public RemoteControl()
        {
            this.InitializeComponent();
            this.RootGrid.SizeChanged += (s, e) =>
            {
                if (e.NewSize == e.PreviousSize) return;
                this._rootGrigSize = e.NewSize;
            };


            //Canvas
            this.Canvas.Width = this._canvasSize.Width;
            this.Canvas.Height = this._canvasSize.Height;
            //Manipulation
            this.ManipulationEllipse.Width = this._manipulationSize.Width;
            this.ManipulationEllipse.Height = this._manipulationSize.Height;
            this._manipulationCenter = Vector2.Zero;
            //Remote
            this.RemoteEllipse.Width = this._remoteSize.Width;
            this.RemoteEllipse.Height = this._remoteSize.Height;
            this.RemoteCenter = Vector2.Zero;


            //Left
            this.LeftPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(-1, 0)); //Delegate
            this.LeftPath.PointerEntered += (s, e) => this.PointerOverLeftStoryboard.Begin();
            this.LeftPath.PointerExited += (s, e) => this.NormalLeftStoryboard.Begin();
            this.LeftPath.PointerPressed += (s, e) => this.PressedLeftStoryboard.Begin();
            this.LeftPath.PointerReleased += (s, e) => this.NormalLeftStoryboard.Begin();


            //Top
            this.TopPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(0, - 1)); //Delegate
            this.TopPath.PointerEntered += (s, e) => this.PointerOverTopStoryboard.Begin();
            this.TopPath.PointerExited += (s, e) => this.NormalTopStoryboard.Begin();
            this.TopPath.PointerPressed += (s, e) => this.PressedTopStoryboard.Begin();
            this.TopPath.PointerReleased += (s, e) => this.NormalTopStoryboard.Begin();


            //Right
            this.RightPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(1, 0)); //Delegate
            this.RightPath.PointerEntered += (s, e) => this.PointerOverRightStoryboard.Begin();
            this.RightPath.PointerExited += (s, e) => this.NormalRightStoryboard.Begin();
            this.RightPath.PointerPressed += (s, e) => this.PressedRightStoryboard.Begin();
            this.RightPath.PointerReleased += (s, e) => this.NormalRightStoryboard.Begin();


            //Bottom
            this.BottomPath.Tapped += (s, e) => this.Moved?.Invoke(this, new Vector2(0, 1)); //Delegate
            this.BottomPath.PointerEntered += (s, e) => this.PointerOverBottomStoryboard.Begin();
            this.BottomPath.PointerExited += (s, e) => this.NormalBottomStoryboard.Begin();
            this.BottomPath.PointerPressed += (s, e) => this.PressedBottomStoryboard.Begin();
            this.BottomPath.PointerReleased += (s, e) => this.NormalBottomStoryboard.Begin();


            //Manipulation
            this.ManipulationEllipse.ManipulationMode = ManipulationModes.All;
            this.ManipulationEllipse.ManipulationStarted += (s, e) =>
            {
                //Manipulation
                this._isManipulation = true;

                this._vector = Vector2.Zero;
                this.ValueChangeStarted?.Invoke(this, Vector2.Zero); //Delegate
            };
            this.ManipulationEllipse.ManipulationDelta += (s, e) =>
            {
                //Manipulation
                Point point = e.Delta.Translation;
                double x = point.X * this._canvasSize.Width / this._rootGrigSize.Width;
                double y = point.Y * this._canvasSize.Height / this._rootGrigSize.Height;
                this.RemoteCenter += new Vector2((float)x, (float)y);
                
                this._vector += point.ToVector2();
                this.ValueChangeDelta?.Invoke(this, this._vector); //Delegate
            };
            this.ManipulationEllipse.ManipulationCompleted += (s, e) =>
            {
                //Manipulation
                this._isManipulation = false;
                this.RemoteCenter = Vector2.Zero;

                this.ValueChangeCompleted?.Invoke(this, this._vector); //Delegate
            };
        }
    }
}