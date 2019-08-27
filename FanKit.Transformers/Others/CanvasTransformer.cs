 using System;
using System.Numerics;
using Windows.Foundation;

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
        public Vector2 ControlCenter { get; private set; } = new Vector2(500.0f, 500.0f);

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


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        /// <summary> <see cref = "CanvasTransformer.ControlWidth" /> and <see cref = "CanvasTransformer.ControlHeight" />'s setter. </summary>
        public Size Size
        {
            get => this.size;
            set
            {
                this.ControlWidth = size.Width < 100 ? 100.0f : (float)size.Width;
                this.ControlHeight = size.Height < 100 ? 100.0f : (float)size.Height;
                this.ControlCenter = new Vector2(this.ControlWidth / 2, this.ControlHeight / 2);
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


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


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
        /// <param name="mode"> The matrix mode. </param>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix(MatrixTransformerMode mode = MatrixTransformerMode.CanvasToVirtualToControl)
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
        /// <param name="mode"> The inverse matrix mode. </param>
        /// <returns> The product inverse matrix. </returns>
        public Matrix3x2 GetInverseMatrix(InverseMatrixTransformerMode mode = InverseMatrixTransformerMode.ControlToVirtualToCanvas)
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

    }
}