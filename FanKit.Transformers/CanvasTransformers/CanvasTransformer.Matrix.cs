using System.Numerics;

namespace FanKit.Transformers
{
    public partial class CanvasTransformer
    {
        
        // Matrix
        Matrix3x2 Matrix;
        Matrix3x2 CanvasToVirtualMatrix;
        Matrix3x2 VirtualToControlMatrix;

        // InverseMatrix
        Matrix3x2 InverseMatrix;
        Matrix3x2 ControlToVirtualInverseMatrix;
        Matrix3x2 VirtualToCanvasInverseMatrix;

        #region Matrix


        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s matrix.
        /// Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <returns> The product matrix. </returns>
        public Matrix3x2 GetMatrix() => this.Matrix;
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
                    return this.Matrix;
                case MatrixTransformerMode.CanvasToVirtual:
                    return this.CanvasToVirtualMatrix;
                case MatrixTransformerMode.VirtualToControl:
                    return this.VirtualToControlMatrix;
            }
            return this.Matrix;
        }

        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s inverse matrix.
        /// Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <returns> The product inverse matrix. </returns>
        public Matrix3x2 GetInverseMatrix() => this.InverseMatrix;
        /// <summary>
        /// Gets <see cref = "CanvasTransformer" />'s inverse matrix.
        /// Call the <see cref = "CanvasTransformer.ReloadMatrix" /> method before using.
        /// </summary>
        /// <param name="mode"> The inverse matrix mode. </param>
        /// <returns> The product inverse matrix. </returns>
        public Matrix3x2 GetInverseMatrix(InverseMatrixTransformerMode mode)
        {
            switch (mode)
            {
                case InverseMatrixTransformerMode.ControlToVirtualToCanvas:
                    return this.InverseMatrix;
                case InverseMatrixTransformerMode.ControlToVirtual:
                    return this.ControlToVirtualInverseMatrix;
                case InverseMatrixTransformerMode.VirtualToCanvas:
                    return this.VirtualToCanvasInverseMatrix;
            }
            return this.InverseMatrix;
        }


        /// <summary>
        /// Reload <see cref = "CanvasTransformer" />'s all matrix.
        ///   If the width, height, scale, position or radian change, call this method to update the matrix
        /// </summary>
        public void ReloadMatrix()
        {
            // Matrix
            this.VirtualToControlMatrix = Matrix3x2.CreateRotation(this.Radian) * Matrix3x2.CreateTranslation(this.Position);
            this.CanvasToVirtualMatrix = Matrix3x2.CreateTranslation(-this.Width / 2, -this.Height / 2) * Matrix3x2.CreateScale(this.Scale);
            this.Matrix = this.CanvasToVirtualMatrix * this.VirtualToControlMatrix;

            // InverseMatrix
            this.VirtualToCanvasInverseMatrix = Matrix3x2.CreateScale(1 / this.Scale) * Matrix3x2.CreateTranslation(this.Width / 2, this.Height / 2);
            this.ControlToVirtualInverseMatrix = Matrix3x2.CreateTranslation(-this.Position) * Matrix3x2.CreateRotation(-this.Radian);
            this.InverseMatrix = this.ControlToVirtualInverseMatrix * this.VirtualToCanvasInverseMatrix;
        }


        #endregion

    }
}
