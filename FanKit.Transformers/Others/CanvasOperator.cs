using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Input;
using Windows.UI.Xaml.Input;

namespace FanKit.Transformers
{
    //@Delegate
    /// <summary>
    /// Method that represents the handling of the Single_Start, Single_Delta, Single_Complete event.
    /// </summary>
    /// <param name="point"> The position of the touch point. </param>
    /// <param name="properties"> The properties of the touch point. </param>
    public delegate void SingleHandler(Vector2 point, PointerPointProperties properties);
    /// <summary>
    /// Method that represents the handling of the Right_Start, Right_Delta, Right_Complete event.
    /// <param name="point"> The position of the mouse point. </param>
    /// </summary>>
    public delegate void RightHandler(Vector2 point);
    /// <summary>
    /// Method that represents the handling of the Double_Start, Double_Delta, Double_Complete event.
    /// </summary>
    /// <param name="center"> The center of the two finger. </param>
    /// <param name="space"> The space between two fingers. </param>
    public delegate void DoubleHandler(Vector2 center, float space);

    /// <summary> 
    /// Provides single-finger, double-finger, mobile events to pointer events for canvas controls.
    /// </summary>
    public class CanvasOperator : DependencyObject
    {

        #region DependencyProperty


        /// <summary>
        /// Gets the current input device type.
        /// </summary>
        public InputDevice Device { get; private set; }


        /// <summary>
        /// Gets or sets the touch mode.
        /// </summary>
        public TouchMode TouchMode { get; set; } = TouchMode.SingleFinger;


        /// <summary>
        /// <see cref = "CanvasOperator" />'s destination control.
        /// </summary>
        public UIElement DestinationControl
        {
            get => (UIElement)base.GetValue(DestinationControlProperty);
            set => base.SetValue(DestinationControlProperty, value);
        }
        /// <summary> Identifies the <see cref = "CanvasOperator.DestinationControl" /> dependency property. </summary>
        public static DependencyProperty DestinationControlProperty = DependencyProperty.Register(nameof(DestinationControl), typeof(UIElement), typeof(CanvasOperator), new PropertyMetadata(null, (sender, e) =>
        {
            CanvasOperator control = (CanvasOperator)sender;

            if (e.OldValue is UIElement oldValue)
            {
                oldValue.PointerPressed -= control.Control_PointerPressed;
                oldValue.PointerReleased -= control.Control_PointerReleased;
                oldValue.PointerCanceled -= control.Control_PointerReleased;
                oldValue.PointerMoved -= control.Control_PointerMoved;
                oldValue.PointerWheelChanged -= control.Control_PointerWheelChanged;
                oldValue.Holding -= control.Control_Holding;
            }

            if (e.NewValue is UIElement value)
            {
                value.PointerPressed += control.Control_PointerPressed;
                value.PointerReleased += control.Control_PointerReleased;
                value.PointerCanceled += control.Control_PointerReleased;
                value.PointerMoved += control.Control_PointerMoved;
                value.PointerWheelChanged += control.Control_PointerWheelChanged;
                value.Holding += control.Control_Holding;
            }
        }));


        #endregion


        #region Delegate


        //@Delegate
        /// <summary> Occurs when the one-finger | mouse-left-button | pen event starts. </summary>
        public event SingleHandler Single_Start = null;
        /// <summary> Occurs when one-finger | mouse-left-button | pen event change. </summary>
        public event SingleHandler Single_Delta = null;
        /// <summary> Occurs when the one-finger | mouse-left-button | pen event is complete. </summary>
        public event SingleHandler Single_Complete = null;


        /// <summary> Occurs when the mouse-right-button | one-finger-holding event starts. </summary>
        public event RightHandler Right_Start = null;
        /// <summary> Occurs when mouse-right-button | one-finger-holding event change. </summary>
        public event RightHandler Right_Delta = null;
        /// <summary> Occurs when the mouse-right-button | one-finger-holding event is complete. </summary>
        public event RightHandler Right_Complete = null;


        /// <summary> Occurs when the double-finger event starts. </summary>
        public event DoubleHandler Double_Start = null;
        /// <summary> Occurs when double-finger event change. </summary>
        public event DoubleHandler Double_Delta = null;
        /// <summary> Occurs when the double-finger event is complete. </summary>
        public event DoubleHandler Double_Complete = null;


        /// <summary>
        /// Occurs when the incremental value of the pointer wheel changes.
        /// </summary>
        public event DoubleHandler Wheel_Changed = null;


        #endregion


        #region Point


        uint PointerId;
        uint EvenPointerId;
        uint OddPointerId;

        Vector2 StartingEvenPoint;
        Vector2 EvenPoint;
        Vector2 OddPoint;


        // Pointer Pressed
        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            switch (e.Pointer.PointerDeviceType)
            {
                case PointerDeviceType.Touch:
                    if (this.EvenPointerId == default)
                    {
                        this.EvenPointerId = e.Pointer.PointerId;

                        PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                        this.StartingEvenPoint = this.EvenPoint = pointerPoint.Position.ToVector2();

                        this.Device = InputDevice.Indeterminacy;
                        return;
                    }
                    else if (this.OddPointerId == default && this.Device is InputDevice.Indeterminacy)
                    {
                        this.OddPointerId = e.Pointer.PointerId;

                        PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                        this.OddPoint = pointerPoint.Position.ToVector2();

                        this.Device = InputDevice.DoubleFinger;
                        this.DestinationControl.CapturePointer(e.Pointer);
                        this.Double_Start?.Invoke((this.OddPoint + this.EvenPoint) / 2, Vector2.Distance(this.OddPoint, this.EvenPoint)); // Delegate
                        return;
                    }
                    else
                    {
                        return;
                    }
                case PointerDeviceType.Pen:
                    if (this.PointerId == default)
                    {
                        this.PointerId = e.Pointer.PointerId;

                        PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                        Vector2 point = pointerPoint.Position.ToVector2();

                        if (pointerPoint.Properties.IsEraser)
                        {
                            this.Device = InputDevice.Eraser;
                            this.DestinationControl.CapturePointer(e.Pointer);
                            this.Single_Start?.Invoke(point, pointerPoint.Properties); // Delegate
                            return;
                        }
                        else
                        {
                            this.Device = InputDevice.Pen;
                            this.DestinationControl.CapturePointer(e.Pointer);
                            this.Single_Start?.Invoke(point, pointerPoint.Properties); // Delegate
                            return;
                        }
                    }
                    else
                    {
                        this.Device = InputDevice.None;
                        return;
                    }
                case PointerDeviceType.Mouse:
                    if (this.PointerId == default)
                    {
                        this.PointerId = e.Pointer.PointerId;

                        PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                        Vector2 point = pointerPoint.Position.ToVector2();

                        if (pointerPoint.Properties.IsRightButtonPressed)
                        {
                            this.Device = InputDevice.RightButton;
                            this.DestinationControl.CapturePointer(e.Pointer);
                            this.Right_Start?.Invoke(point); // Delegate
                            return;
                        }
                        else
                        {
                            this.Device = InputDevice.LeftButton;
                            this.DestinationControl.CapturePointer(e.Pointer);
                            this.Single_Start?.Invoke(point, pointerPoint.Properties); // Delegate
                            return;
                        }
                    }
                    else
                    {
                        this.Device = InputDevice.None;
                        return;
                    }
                default:
                    this.Device = InputDevice.None;
                    return;
            }
        }


        // Pointer Released
        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.PointerId = default;
            this.EvenPointerId = default;
            this.OddPointerId = default;

            PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
            Vector2 point = pointerPoint.Position.ToVector2();

            this.DestinationControl.ReleasePointerCaptures();
            switch (this.Device)
            {
                case InputDevice.Holding:
                    this.Right_Complete?.Invoke(point); // Delegate
                    break;
                case InputDevice.SingleFinger:
                    this.Single_Complete?.Invoke(point, pointerPoint.Properties); // Delegate
                    break;
                case InputDevice.DoubleFinger:
                    this.Double_Complete?.Invoke((this.OddPoint + this.EvenPoint) / 2, Vector2.Distance(this.OddPoint, this.EvenPoint)); // Delegate
                    break;
                case InputDevice.Pen:
                    this.Single_Complete?.Invoke(point, pointerPoint.Properties); // Delegate
                    break;
                case InputDevice.Eraser:
                    this.Single_Complete?.Invoke(point, pointerPoint.Properties); // Delegate
                    break;
                case InputDevice.LeftButton:
                    this.Single_Complete?.Invoke(point, pointerPoint.Properties); // Delegate
                    break;
                case InputDevice.RightButton:
                    this.Right_Complete?.Invoke(point); // Delegate
                    break;
                default:
                    break;
            }

            this.Device = default;
        }


        // Pointer Moved
        private void Control_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            switch (e.Pointer.PointerDeviceType)
            {
                case PointerDeviceType.Touch:
                    switch (this.Device)
                    {
                        case InputDevice.Indeterminacy:
                            {
                                PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                if (this.EvenPointerId == e.Pointer.PointerId)
                                {
                                    this.EvenPoint = pointerPoint.Position.ToVector2();

                                    if (Vector2.Distance(this.StartingEvenPoint, this.EvenPoint) > 12f)
                                    {
                                        switch (this.TouchMode)
                                        {
                                            case TouchMode.SingleFinger:
                                                this.Device = InputDevice.SingleFinger;
                                                this.DestinationControl.CapturePointer(e.Pointer);
                                                this.Single_Start?.Invoke(this.StartingEvenPoint, pointerPoint.Properties); // Delegate
                                                break;
                                            case TouchMode.RightButton:
                                                this.Device = InputDevice.RightButton;
                                                this.DestinationControl.CapturePointer(e.Pointer);
                                                this.Right_Start?.Invoke(this.StartingEvenPoint); // Delegate
                                                break;
                                            default:
                                                this.Device = InputDevice.None;
                                                break;
                                        }
                                    }
                                }
                                return;
                            }
                        case InputDevice.Holding:
                            {
                                PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                Vector2 point = pointerPoint.Position.ToVector2();

                                this.Right_Delta?.Invoke(point); // Delegate
                                return;
                            }
                        case InputDevice.SingleFinger:
                            {
                                PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);

                                if (this.EvenPointerId == e.Pointer.PointerId)
                                {
                                    this.EvenPoint = pointerPoint.Position.ToVector2();
                                }

                                this.Single_Delta?.Invoke(this.EvenPoint, pointerPoint.Properties); // Delegate
                                return;
                            }
                        case InputDevice.DoubleFinger:
                            {
                                PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);

                                if (this.EvenPointerId == e.Pointer.PointerId)
                                {
                                    this.EvenPoint = pointerPoint.Position.ToVector2();
                                }
                                if (this.OddPointerId == e.Pointer.PointerId)
                                {
                                    this.OddPoint = pointerPoint.Position.ToVector2();
                                }

                                this.Double_Delta?.Invoke((this.OddPoint + this.EvenPoint) / 2, Vector2.Distance(this.OddPoint, this.EvenPoint)); // Delegate
                                return;
                            }
                        case InputDevice.RightButton:
                            switch (this.TouchMode)
                            {
                                case TouchMode.RightButton:
                                    PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                    Vector2 point = pointerPoint.Position.ToVector2();

                                    this.Right_Delta?.Invoke(point); // Delegate
                                    break;
                                default:
                                    break;
                            }
                            return;
                        default:
                            return;
                    }
                case PointerDeviceType.Pen:
                    switch (this.Device)
                    {
                        case InputDevice.Pen:
                            if (this.PointerId != default)
                            {
                                if (this.PointerId == e.Pointer.PointerId)
                                {
                                    PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                    Vector2 point = pointerPoint.Position.ToVector2();

                                    this.Single_Delta?.Invoke(point, pointerPoint.Properties); // Delegate
                                }
                            }
                            return;
                        case InputDevice.Eraser:
                            if (this.PointerId != default)
                            {
                                if (this.PointerId == e.Pointer.PointerId)
                                {
                                    PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                    Vector2 point = pointerPoint.Position.ToVector2();

                                    this.Single_Delta?.Invoke(point, pointerPoint.Properties); // Delegate
                                }
                            }
                            return;
                        default:
                            return;
                    }
                case PointerDeviceType.Mouse:
                    switch (this.Device)
                    {
                        case InputDevice.LeftButton:
                            if (this.PointerId != default)
                            {
                                if (this.PointerId == e.Pointer.PointerId)
                                {
                                    PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                    Vector2 point = pointerPoint.Position.ToVector2();

                                    this.Single_Delta?.Invoke(point, pointerPoint.Properties); // Delegate
                                }
                            }
                            return;
                        case InputDevice.RightButton:
                            if (this.PointerId != default)
                            {
                                if (this.PointerId == e.Pointer.PointerId)
                                {
                                    PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);
                                    Vector2 point = pointerPoint.Position.ToVector2();

                                    this.Right_Delta?.Invoke(point); // Delegate
                                }
                            }
                            return;
                        default:
                            return;
                    }
                default:
                    return;
            }
        }


        // Wheel Changed
        private void Control_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            PointerPoint pointerPoint = e.GetCurrentPoint(this.DestinationControl);

            Vector2 point = pointerPoint.Position.ToVector2();
            float space = pointerPoint.Properties.MouseWheelDelta;

            this.Wheel_Changed?.Invoke(point, space);//Delegate
        }


        // Holding
        private void Control_Holding(object sender, HoldingRoutedEventArgs e)
        {
            if (this.Device is InputDevice.Indeterminacy)
            {
                this.Device = InputDevice.Holding;
                this.Right_Start?.Invoke(this.StartingEvenPoint); // Delegate
            }
        }


        #endregion

    }

    /// <summary>
    /// The touch mode of <see cref = "CanvasOperator" />.
    /// </summary>
    public enum TouchMode
    {
        /// <summary> Disabled. </summary>
        Disable,
        /// <summary> <see cref="InputDevice.SingleFinger"/>. </summary>
        SingleFinger,
        /// <summary> <see cref="InputDevice.RightButton"/>. </summary>
        RightButton,
    }

    /// <summary>
    /// The input device type of <see cref = "CanvasOperator" />.
    /// </summary>
    public enum InputDevice
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Indeterminacy. </summary>
        Indeterminacy,

        /// <summary>Holding. </summary>
        Holding,
        /// <summary> One Finger. </summary>
        SingleFinger,
        /// <summary> Two Fingers. </summary>
        DoubleFinger,

        /// <summary> Pen. </summary>
        Pen,
        /// <summary> Eraser. </summary>
        Eraser,

        /// <summary> Mouse-Left-Button. </summary>
        LeftButton,
        /// <summary> Mouse-Right-Button. </summary>
        RightButton,
    }
}