using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Devices.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace FanKit.Transformers
{
    /// <summary> 
    /// Provides single-finger, double-finger, mobile events to pointer events for canvas controls.
    /// </summary>
    public class CanvasOperator : DependencyObject
    {

        #region DependencyProperty


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
                oldValue.PointerEntered -= control.Control_PointerEntered;
                oldValue.PointerExited -= control.Control_PointerExited;

                oldValue.PointerPressed -= control.Control_PointerPressed;
                oldValue.PointerReleased -= control.Control_PointerReleased;

                oldValue.PointerMoved -= control.Control_PointerMoved;
                oldValue.PointerWheelChanged -= control.Control_PointerWheelChanged;
            }

            if (e.NewValue is UIElement value)
            {
                value.PointerEntered += control.Control_PointerEntered;
                value.PointerExited += control.Control_PointerExited;

                value.PointerPressed += control.Control_PointerPressed;
                value.PointerReleased += control.Control_PointerReleased;

                value.PointerMoved += control.Control_PointerMoved;
                value.PointerWheelChanged += control.Control_PointerWheelChanged;
            }
        }));


        #endregion


        #region Delegate


        //@Delegate
        /// <summary>
        /// Method that represents the handling of the Single_Start, Single_Delta, Single_Complete event.
        /// </summary>
        /// <param name="point"> The position of the touch point. </param>
        public delegate void SingleHandler(Vector2 point);
        /// <summary> Occurs when the one-finger | mouse-left-button | pen event starts. </summary>
        public event SingleHandler Single_Start = null;
        /// <summary> Occurs when one-finger | mouse-left-button | pen event change. </summary>
        public event SingleHandler Single_Delta = null;
        /// <summary> Occurs when the one-finger | mouse-left-button | pen event is complete. </summary>
        public event SingleHandler Single_Complete = null;


        /// <summary>
        /// Method that represents the handling of the Right_Start, Right_Delta, Right_Complete event.
        /// <param name="point"> The position of the mouse point. </param>
        /// </summary>>
        public delegate void RightHandler(Vector2 point);
        /// <summary> Occurs when the mouse-right-button event starts. </summary>
        public event RightHandler Right_Start = null;
        /// <summary> Occurs when mouse-right-button event change. </summary>
        public event RightHandler Right_Delta = null;
        /// <summary> Occurs when the mouse-right-button event is complete. </summary>
        public event RightHandler Right_Complete = null;


        /// <summary>
        /// Method that represents the handling of the Double_Start, Double_Delta, Double_Complete event.
        /// </summary>
        /// <param name="center"> The center of the two finger. </param>
        /// <param name="space"> The space between two fingers. </param>
        public delegate void DoubleHandler(Vector2 center, float space);
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


        InputDevice Device = InputDevice.None;

        readonly HashSet<uint> Pointers = new HashSet<uint>();

        Vector2 EvenPointer; // it's ID%2==0
        Vector2 OddPointer; // it's ID%2==1

        Vector2 EvenStartingPointer;
        Vector2 OddStartingPointer;

        Vector2 StartingPointer;


        // Pointer Entered
        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }
        // Pointer Exited
        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
        }


        // Pointer Pressed
        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);
            this.DestinationControl?.CapturePointer(e.Pointer);

            if (CanvasOperator.PointerIsTouch(this.DestinationControl, e))
            {
                this.Pointers.Add(e.Pointer.PointerId);
                if (e.Pointer.PointerId % 2 == 0) this.EvenPointer = point;
                if (e.Pointer.PointerId % 2 == 1) this.OddPointer = point;

                if (this.Pointers.Count > 1)
                {
                    if (this.Device != InputDevice.Double)
                    {
                        this.EvenStartingPointer = this.EvenPointer;
                        this.OddStartingPointer = this.OddPointer;
                    }
                }
                else
                {
                    if (this.Device != InputDevice.Single)
                        this.StartingPointer = point;
                }
            }
            else
            {
                if (CanvasOperator.PointerIsRight(this.DestinationControl, e))
                {
                    if (this.Device != InputDevice.Right)
                    {
                        this.Device = InputDevice.Right;
                        this.Right_Start?.Invoke(point);//Delegate
                    }
                }
                if (CanvasOperator.PointerIsLeft(this.DestinationControl, e) || CanvasOperator.PointerIsPen(this.DestinationControl, e))
                {
                    if (this.Device != InputDevice.Single)
                    {
                        this.Device = InputDevice.Single;
                        this.Single_Start?.Invoke(point);//Delegate
                    }
                }
            }
        }
        // Pointer Released
        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);
            this.DestinationControl?.ReleasePointerCapture(e.Pointer);

            if (this.Device == InputDevice.Right)
            {
                this.Device = InputDevice.None;
                this.Right_Complete?.Invoke(point);//Delegate
            }
            else if (this.Device == InputDevice.Double)
            {
                this.Device = InputDevice.None;
                this.Double_Complete?.Invoke(point, (this.OddPointer - this.EvenPointer).Length());//Delegate
            }
            else if (this.Device == InputDevice.Single)
            {
                this.Device = InputDevice.None;
                this.Single_Complete?.Invoke(point);//Delegate
            }

            this.Pointers.Clear();
        }


        // Pointer Moved
        private void Control_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);

            if (CanvasOperator.PointerIsTouch(this.DestinationControl, e))
            {
                if (e.Pointer.PointerId % 2 == 0) this.EvenPointer = point;
                if (e.Pointer.PointerId % 2 == 1) this.OddPointer = point;

                if (this.Pointers.Count > 1)
                {
                    if (this.Device != InputDevice.Double)
                    {
                        if (System.Math.Abs(this.EvenStartingPointer.X - this.EvenPointer.X) > 2 || System.Math.Abs(this.EvenStartingPointer.Y - this.EvenPointer.Y) > 2 || System.Math.Abs(this.OddStartingPointer.X - this.OddPointer.X) > 2 || System.Math.Abs(this.OddStartingPointer.Y - this.OddPointer.Y) > 2)
                        {
                            this.Device = InputDevice.Double;
                            this.Double_Start?.Invoke((this.OddPointer + this.EvenPointer) / 2, (this.OddPointer - this.EvenPointer).Length());//Delegate
                        }
                    }
                    else if (this.Device == InputDevice.Double) this.Double_Delta?.Invoke((this.OddPointer + this.EvenPointer) / 2, (this.OddPointer - this.EvenPointer).Length());//Delegate
                }
                else
                {
                    if (this.Device != InputDevice.Single)
                    {
                        double length = (this.StartingPointer - point).Length();

                        if (length > 2 && length < 12)
                        {
                            this.Device = InputDevice.Single;
                            this.Single_Start?.Invoke(point);//Delegate
                        }
                    }
                    else if (this.Device == InputDevice.Single) this.Single_Delta?.Invoke(point);//Delegate
                }
            }
            else
            {
                if (this.Device == InputDevice.Right) this.Right_Delta?.Invoke(point);//Delegate

                if (this.Device == InputDevice.Single) this.Single_Delta?.Invoke(point);//Delegate
            }
        }
        // Wheel Changed
        private void Control_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);
            float space = CanvasOperator.PointerWheelDelta(this.DestinationControl, e);

            this.Wheel_Changed?.Invoke(point, space);//Delegate
        }


        #endregion


        #region PointerRouted

        //@Static
        /// <summary>
        /// Gets the position of touch point
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> The position of touch point.</returns>
        public static Vector2 PointerPosition(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).Position.ToVector2();
        /// <summary>
        /// Gets the pressure of touch point
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> The pressure of touch point.</returns>
        public static float PointerPressure(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).Properties.Pressure;
        /// <summary>
        /// Gets the value entered by the wheel button.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> The value entered by the wheel button. </returns>
        public static float PointerWheelDelta(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).Properties.MouseWheelDelta;

        /// <summary>
        /// Judging the action.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> If the action is triggered by a touch, return **true**. </returns>
        public static bool PointerIsTouch(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).PointerDevice.PointerDeviceType == PointerDeviceType.Touch;
        /// <summary>
        /// Judging the action.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> If the action is triggered by a pen, return **true**. </returns>
        public static bool PointerIsPen(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).PointerDevice.PointerDeviceType == PointerDeviceType.Pen && e.GetCurrentPoint(relativeTo).Properties.IsBarrelButtonPressed == false && e.GetCurrentPoint(relativeTo).IsInContact;
        /// <summary>
        /// Judging the action.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> If the action is triggered by a barrel, return **true**. </returns>
        public static bool PointerIsBarrel(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).PointerDevice.PointerDeviceType == PointerDeviceType.Pen && e.GetCurrentPoint(relativeTo).Properties.IsBarrelButtonPressed == true && e.GetCurrentPoint(relativeTo).IsInContact;

        /// <summary>
        /// Judging the action.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> If the action is triggered by a mouse, return **true**. </returns>
        public static bool PointerIsMouse(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).PointerDevice.PointerDeviceType == PointerDeviceType.Mouse;
        /// <summary>
        /// Judging the action.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> If the action is triggered by a mouse-left-button, return **true**. </returns>
        public static bool PointerIsLeft(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).PointerDevice.PointerDeviceType == PointerDeviceType.Mouse && e.GetCurrentPoint(relativeTo).Properties.IsLeftButtonPressed;
        /// <summary>
        /// Judging the action.
        /// </summary>
        /// <param name="relativeTo"> Any UIElement derived objects connected to the same object tree. </param>
        /// <param name="e"> Event data for events. </param>
        /// <returns> If the action is triggered by a mouse-right-button, return **true**. </returns>
        public static bool PointerIsRight(UIElement relativeTo, PointerRoutedEventArgs e) => e.GetCurrentPoint(relativeTo).Properties.IsRightButtonPressed || e.GetCurrentPoint(relativeTo).Properties.IsMiddleButtonPressed;


        #endregion

    }

    /// <summary>
    /// The input device type of <see cref = "CanvasOperator" />.
    /// </summary>
    public enum InputDevice
    {
        /// <summary> Normal. </summary>
        None,

        /// <summary> One-finger | Mouse-Left-Button | Pen. </summary>
        Single,

        /// <summary> Two Fingers. </summary>
        Double,

        /// <summary> Mouse-Right-Button. </summary>
        Right,
    }
}