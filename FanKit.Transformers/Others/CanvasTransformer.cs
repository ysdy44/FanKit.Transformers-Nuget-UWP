 using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Xaml.Controls;

namespace FanKit.Transformers
{
    /// <summary>
    /// Transformer: 
    /// Provide matrix by Position、Scale、Radians.
    /// 
    /// 
    /// TODO:
    /// Canvas Layer:
    ///    The original size of the layer.
    /// Virtual Layer:
    ///    Render all layers together and make their center points coincide with the origin (0,0) and then zoom;
    /// Control Layer:
    ///    Rotate around the origin first, then shift. (The canvas has a rotation angle)
    ///    
    /// </summary>
    public class CanvasTransformer
    {

        /// <summary> <see cref = "CanvasTransformer" />'s width. </summary>
        public int Width = 1000;
        /// <summary> <see cref = "CanvasTransformer" />'s height. </summary>
        public int Height = 1000;

        /// <summary> <see cref = "CanvasTransformer" />'s scale. </summary>
        public float Scale = 1.0f;

        /// <summary> CanvasControl's width. </summary>
        public float ControlWidth { get; private set; } = 1000.0f;
        /// <summary> CanvasControl's height. </summary>
        public float ControlHeight { get; private set; } = 1000.0f;
        /// <summary> CanvasControl's center. </summary>
        public Vector2 ControlCenter => new Vector2(this.ControlWidth / 2, this.ControlHeight / 2);

        /// <summary> <see cref = "CanvasTransformer" />'s translation. </summary>
        public Vector2 Position = new Vector2(0.0f, 0.0f);
        /// <summary> <see cref = "CanvasTransformer" />'s rotation. </summary>
        public float Radian = 0.0f;


        //@Construct
        /// <summary>
        /// Constructs a <see cref = "CanvasTransformer" />.
        /// </summary>
        public CanvasTransformer()
        {
            this.ReloadMatrix();
        }


        #region Size
        

        /// <summary> <see cref = "CanvasTransformer.ControlWidth" /> and <see cref = "CanvasTransformer.ControlHeight" />'s setter. </summary>
        public Size Size
        {
            get => this.size;
            set
            {
                this.ControlWidth = size.Width < 100 ? 100.0f : (float)size.Width;
                this.ControlHeight = size.Height < 100 ? 100.0f : (float)size.Height;
                this.size = value;
            }
        }
        private Size size;

        /// <summary> 
        /// Fit to the screen. 
        /// </summary>
        public void Fit()
        {
            float widthScale = this.ControlWidth / this.Width / 8.0f * 7.0f;
            float heightScale = this.ControlHeight / this.Height / 8.0f * 7.0f;

            this.Scale = System.Math.Min(widthScale, heightScale);

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }

        /// <summary>
        /// Fit to the screen with scale.
        /// </summary>
        /// <param name="scale"> The scalar value. </param>
        public void Fit(float scale)
        {
            this.Scale = scale;

            this.Position.X = this.ControlWidth / 2.0f;
            this.Position.Y = this.ControlHeight / 2.0f;

            this.Radian = 0.0f;

            this.ReloadMatrix();
        }


        #endregion

        #region Matrix


        //Matrix
        Matrix3x2 _matrix;
        Matrix3x2 _canvasToVirtualMatrix;
        Matrix3x2 _virtualToControlMatrix;

        //InverseMatrix
        Matrix3x2 _inverseMatrix;
        Matrix3x2 _controlToVirtualInverseMatrix;
        Matrix3x2 _virtualToCanvasInverseMatrix;


        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s matrix.
        /// Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix() => this._matrix;
        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s matrix.
        /// Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <param name="mode"> The matrix mode. </param>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix(MatrixTransformerMode mode)
        {
            switch (mode)
            {
                case MatrixTransformerMode.CanvasToVirtualToControl:
                    return this._matrix;
                case MatrixTransformerMode.CanvasToVirtual:
                    return this._canvasToVirtualMatrix;
                case MatrixTransformerMode.VirtualToControl:
                    return this._virtualToControlMatrix;
            }
            return this._matrix;
        }

        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s inverse matrix.
        ///   Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <returns> The product inverse matrix. </returns>
        public Matrix3x2 GetInverseMatrix() => this._inverseMatrix;
        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s inverse matrix.
        ///   Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <param name="mode"> The inverse matrix mode. </param>
        /// <returns> The product inverse matrix. </returns>
        public Matrix3x2 GetInverseMatrix(InverseMatrixTransformerMode mode)
        {
            switch (mode)
            {
                case InverseMatrixTransformerMode.ControlToVirtualToCanvas:
                    return this._inverseMatrix;
                case InverseMatrixTransformerMode.ControlToVirtual:
                    return this._controlToVirtualInverseMatrix;
                case InverseMatrixTransformerMode.VirtualToCanvas:
                    return this._virtualToCanvasInverseMatrix;
            }
            return this._inverseMatrix;
        }


        /// <summary>
        /// Reload <see cref = "CanvasTransformer" />'s all matrix.
        ///   If the width, height, scale, position or radian change, call this method to update the matrix
        /// </summary>
        public void ReloadMatrix()
        {
            //Matrix
            this._virtualToControlMatrix = Matrix3x2.CreateRotation(this.Radian) * Matrix3x2.CreateTranslation(this.Position);
            this._canvasToVirtualMatrix = Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateScale(this.Scale);
            this._matrix = this._canvasToVirtualMatrix * this._virtualToControlMatrix;

            //InverseMatrix
            this._virtualToCanvasInverseMatrix = Matrix3x2.CreateScale(1 / this.Scale) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);
            this._controlToVirtualInverseMatrix = Matrix3x2.CreateTranslation(-this.Position) * Matrix3x2.CreateRotation(-this.Radian);
            this._inverseMatrix = this._controlToVirtualInverseMatrix * this._virtualToCanvasInverseMatrix;
        }


        #endregion


        #region Move


        Vector2 _moveStartPoint;
        Vector2 _moveStartPosition;

        /// <summary>
        /// Cache data for a Move event.
        /// </summary>
        /// <param name="point"> The point. </param>
        public void CacheMove(Vector2 point)
        {
            this._moveStartPoint = point;
            this._moveStartPosition = this.Position;
        }

        /// <summary>
        /// Move position (CacheMove event occur first).
        /// </summary>
        /// <param name="point"> The point. </param>
        public void Move(Vector2 point)
        {
            this.Position = this._moveStartPosition - this._moveStartPoint + point;
            this.ReloadMatrix();
        }


        #endregion


        #region Pinch


        Vector2 _pinchStartCenter;
        float _pinchStartScale;
        float _pinchStartSpace;

        /// <summary>
        /// Cache data for a Pinch event.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="space"> The space between fingers. </param>
        public void CachePinch(Vector2 centerPoint, float space)
        {
            this._pinchStartCenter = (centerPoint - this.Position) / this.Scale + this.ControlCenter;

            this._pinchStartSpace = space;
            this._pinchStartScale = this.Scale;
        }

        /// <summary>
        /// Pinch the screen to change position and scale (CachePinch event occur first).
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="space"> The space between fingers. </param>
        public void Pinch(Vector2 centerPoint, float space)
        {
            this.Scale = this._pinchStartScale / this._pinchStartSpace * space;
            this.Position = centerPoint - (this._pinchStartCenter - this.ControlCenter) * this.Scale;
            this.ReloadMatrix();
        }


        #endregion


        #region ZoomIn ZoomOut


        /// <summary>
        /// To manipulate a display so as to make the image smaller.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="zoomInScale"> The scale. </param>
        /// <param name="maximum">The maximum scale. </param>
        public void ZoomIn(Vector2 centerPoint, float zoomInScale = 1.1f, float maximum = 10f)
        {
            if (this.Scale < maximum)
            {
                this.Scale *= zoomInScale;
                this.Position = centerPoint + (this.Position - centerPoint) * zoomInScale;
            }
            this.ReloadMatrix();
        }

        /// <summary>
        ///  To manipulate a display so as to make the image larger.
        /// </summary>
        /// <param name="centerPoint"> The center point. </param>
        /// <param name="zoomOutScale"> The scale. </param>
        /// <param name="minimum">The minimum scale. </param>
        public void ZoomOut(Vector2 centerPoint, float zoomOutScale = 1.1f, float minimum = 0.1f)
        {
            if (this.Scale > minimum)
            {
                this.Scale /= zoomOutScale;
                this.Position = centerPoint + (this.Position - centerPoint) / zoomOutScale;
            }
            this.ReloadMatrix();
        }


        #endregion

    }
}