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
        public Control DestinationControl
        {
            get { return (Control)GetValue(DestinationControlProperty); }
            set { SetValue(DestinationControlProperty, value); }
        }
        /// <summary> Identifies the <see cref = "CanvasOperator.DestinationControl" /> dependency property. </summary>
        public static DependencyProperty DestinationControlProperty = DependencyProperty.Register(nameof(DestinationControl), typeof(Windows.UI.Xaml.Controls.Control), typeof(CanvasOperator), new PropertyMetadata(null, (sender, e) =>
        {
            CanvasOperator con = (CanvasOperator)sender;

            if (e.NewValue is Control value)
            {
                value.PointerEntered += con.Control_PointerEntered;
                value.PointerExited += con.Control_PointerExited;

                value.PointerPressed += con.Control_PointerPressed;
                value.PointerReleased += con.Control_PointerReleased;

                value.PointerMoved += con.Control_PointerMoved;
                value.PointerWheelChanged += con.Control_PointerWheelChanged;
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


        InputDevice _device = InputDevice.None;

        readonly HashSet<uint> _pointers = new HashSet<uint>();

        Vector2 _evenPointer; //it's ID%2==0
        Vector2 _oddPointer;//it's ID%2==1

        Vector2 _evenStartingPointer;
        Vector2 _oddStartingPointer;

        Vector2 _startingPointer;


        //Pointer Entered
        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
        }
        //Pointer Exited
        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            this.Control_PointerReleased(sender, e);
        }


        //Pointer Pressed
        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);

            if (CanvasOperator.PointerIsTouch(this.DestinationControl, e))
            {
                this._pointers.Add(e.Pointer.PointerId);
                if (e.Pointer.PointerId % 2 == 0) this._evenPointer = point;
                if (e.Pointer.PointerId % 2 == 1) this._oddPointer = point;

                if (this._pointers.Count > 1)
                {
                    if (this._device != InputDevice.Double)
                    {
                        this._evenStartingPointer = this._evenPointer;
                        this._oddStartingPointer = this._oddPointer;
                    }
                }
                else
                {
                    if (this._device != InputDevice.Single)
                        this._startingPointer = point;
                }
            }
            else
            {
                if (CanvasOperator.PointerIsRight(this.DestinationControl, e))
                {
                    if (this._device != InputDevice.Right)
                    {
                        this._device = InputDevice.Right;
                        this.Right_Start?.Invoke(point);//Delegate
                    }
                }
                if (CanvasOperator.PointerIsLeft(this.DestinationControl, e) || CanvasOperator.PointerIsPen(this.DestinationControl, e))
                {
                    if (this._device != InputDevice.Single)
                    {
                        this._device = InputDevice.Single;
                        this.Single_Start?.Invoke(point);//Delegate
                    }
                }
            }
        }
        //Pointer Released
        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);

            if (this._device == InputDevice.Right)
            {
                this._device = InputDevice.None;
                this.Right_Complete?.Invoke(point);//Delegate
            }
            else if (this._device == InputDevice.Double)
            {
                this._device = InputDevice.None;
                this.Double_Complete?.Invoke(point, (this._oddPointer - this._evenPointer).Length());//Delegate
            }
            else if (this._device == InputDevice.Single)
            {
                this._device = InputDevice.None;
                this.Single_Complete?.Invoke(point);//Delegate
            }

            this._pointers.Clear();
        }


        //Pointer Moved
        private void Control_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Vector2 point = CanvasOperator.PointerPosition(this.DestinationControl, e);

            if (CanvasOperator.PointerIsTouch(this.DestinationControl, e))
            {
                if (e.Pointer.PointerId % 2 == 0) this._evenPointer = point;
                if (e.Pointer.PointerId % 2 == 1) this._oddPointer = point;

                if (this._pointers.Count > 1)
                {
                    if (this._device != InputDevice.Double)
                    {
                        if (System.Math.Abs(this._evenStartingPointer.X - this._evenPointer.X) > 2 || System.Math.Abs(this._evenStartingPointer.Y - this._evenPointer.Y) > 2 || System.Math.Abs(this._oddStartingPointer.X - this._oddPointer.X) > 2 || System.Math.Abs(this._oddStartingPointer.Y - this._oddPointer.Y) > 2)
                        {
                            this._device = InputDevice.Double;
                            this.Double_Start?.Invoke((this._oddPointer + this._evenPointer) / 2, (this._oddPointer - this._evenPointer).Length());//Delegate
                        }
                    }
                    else if (this._device == InputDevice.Double) this.Double_Delta?.Invoke((this._oddPointer + this._evenPointer) / 2, (this._oddPointer - this._evenPointer).Length());//Delegate
                }
                else
                {
                    if (this._device != InputDevice.Single)
                    {
                        double length = (this._startingPointer - point).Length();

                        if (length > 2 && length < 12)
                        {
                            this._device = InputDevice.Single;
                            this.Single_Start?.Invoke(point);//Delegate
                        }
                    }
                    else if (this._device == InputDevice.Single) this.Single_Delta?.Invoke(point);//Delegate
                }
            }
            else
            {
                if (this._device == InputDevice.Right) this.Right_Delta?.Invoke(point);//Delegate

                if (this._device == InputDevice.Single) this.Single_Delta?.Invoke(point);//Delegate
            }
        }
        //Wheel Changed
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