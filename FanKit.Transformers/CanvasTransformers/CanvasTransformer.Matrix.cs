using System.Numerics;

namespace FanKit.Transformers
{
    public partial class CanvasTransformer
    {
        
        //Matrix
        Matrix3x2 _matrix;
        Matrix3x2 _canvasToVirtualMatrix;
        Matrix3x2 _virtualToControlMatrix;

        //InverseMatrix
        Matrix3x2 _inverseMatrix;
        Matrix3x2 _controlToVirtualInverseMatrix;
        Matrix3x2 _virtualToCanvasInverseMatrix;

        #region Matrix


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

    }
}
