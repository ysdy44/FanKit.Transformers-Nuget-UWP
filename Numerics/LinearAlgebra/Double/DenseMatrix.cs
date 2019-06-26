using MathNet.Numerics.LinearAlgebra.Double.Factorization;
using MathNet.Numerics.LinearAlgebra.Factorization;
using MathNet.Numerics.LinearAlgebra.Storage;
using MathNet.Numerics.Providers.LinearAlgebra;
using MathNet.Numerics.Threading;
using System;
using System.Diagnostics;
using System.Linq;

namespace MathNet.Numerics.LinearAlgebra.Double
{
    /// <summary>
    ///     A Matrix class with dense storage. The underlying storage is a one dimensional array in column-major order (column
    ///     by column).
    /// </summary>
    [DebuggerDisplay("DenseMatrix {RowCount}x{ColumnCount}-Double")]
    public class DenseMatrix : Matrix
    {
        /// <summary>
        ///     Number of columns.
        /// </summary>
        /// <remarks>
        ///     Using this instead of the ColumnCount property to speed up calculating
        ///     a matrix index in the data array.
        /// </remarks>
        private readonly int _columnCount;

        /// <summary>
        ///     Number of rows.
        /// </summary>
        /// <remarks>
        ///     Using this instead of the RowCount property to speed up calculating
        ///     a matrix index in the data array.
        /// </remarks>
        private readonly int _rowCount;

        /// <summary>
        ///     Gets the matrix's data.
        /// </summary>
        /// <value>The matrix's data.</value>
        private readonly double[] _values;

        /// <summary>
        ///     Create a new dense matrix straight from an initialized matrix storage instance.
        ///     The storage is used directly without copying.
        ///     Intended for advanced scenarios where you're working directly with
        ///     storage for performance or interop reasons.
        /// </summary>
        public DenseMatrix(DenseColumnMajorMatrixStorage<double> storage)
            : base(storage)
        {
            _rowCount = storage.RowCount;
            _columnCount = storage.ColumnCount;
            _values = storage.Data;
        }

        /// <summary>
        ///     Create a new square dense matrix with the given number of rows and columns.
        ///     All cells of the matrix will be initialized to zero.
        ///     Zero-length matrices are not supported.
        /// </summary>
        /// <exceptioncreArgumentException">If the order is less than one.
        /// 
        /// </exception>
        public DenseMatrix(int order)
            : this(new DenseColumnMajorMatrixStorage<double>(order, order))
        {
        }

        /// <summary>
        ///     Create a new dense matrix with the given number of rows and columns.
        ///     All cells of the matrix will be initialized to zero.
        ///     Zero-length matrices are not supported.
        /// </summary>
        /// <exceptioncreArgumentException">If the row or column count is less than one.
        /// 
        /// </exception>
        public DenseMatrix(int rows, int columns)
            : this(new DenseColumnMajorMatrixStorage<double>(rows, columns))
        {
        }


        /// <summary>
        ///     Gets the matrix's data.
        /// </summary>
        /// <value>The matrix's data.</value>
        public double[] Values => _values;


        /// <summary>
        ///     Create a new dense matrix as a copy of the given two-dimensional array.
        ///     This new matrix will be independent from the provided array.
        ///     A new memory block will be allocated for storing the matrix.
        /// </summary>
        public static DenseMatrix OfArray(double[,] array)
        {
            return new DenseMatrix(DenseColumnMajorMatrixStorage<double>.OfArray(array));
        }


        /// <summary>
        ///     Create a new dense matrix and initialize each value to the same provided value.
        /// </summary>
        public static DenseMatrix Create(int rows, int columns, double value)
        {
            if (value == 0d) return new DenseMatrix(rows, columns);
            return new DenseMatrix(DenseColumnMajorMatrixStorage<double>.OfValue(rows, columns, value));
        }

        /// <summary>
        ///     Create a new dense matrix and initialize each value using the provided init function.
        /// </summary>
        public static DenseMatrix Create(int rows, int columns, Func<int, int, double> init)
        {
            return new DenseMatrix(DenseColumnMajorMatrixStorage<double>.OfInit(rows, columns, init));
        }


        /// <summary>
        ///     Negate each element of this matrix and place the results into the result matrix.
        /// </summary>
        /// <param name="result">The result of the negation.</param>
        protected override void DoNegate(Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult != null)
            {
                LinearAlgebraControl.Provider.ScaleArray(-1, _values, denseResult._values);
                return;
            }

            base.DoNegate(result);
        }

        /// <summary>
        ///     Add a scalar to each element of the matrix and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to add.</param>
        /// <param name="result">The matrix to store the result of the addition.</param>
        protected override void DoAdd(double scalar, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
            {
                base.DoAdd(scalar, result);
                return;
            }

            CommonParallel.For(0, _values.Length, 4096, (a, b) =>
            {
                var v = denseResult._values;
                for (var i = a; i < b; i++) v[i] = _values[i] + scalar;
            });
        }

        /// <summary>
        ///     Adds another matrix to this matrix.
        /// </summary>
        /// <param name="other">The matrix to add to this matrix.</param>
        /// <param name="result">The matrix to store the result of add</param>
        /// <exceptioncreArgumentNullException">If the other matrix is 
        /// 
        /// <see langword="null.
        /// 
        /// </exception>
        /// <exceptioncreArgumentOutOfRangeException">If the two matrices don't have the same dimensions.
        /// 
        /// </exception>
        protected override void DoAdd(Matrix<double> other, Matrix<double> result)
        {
            // dense + dense = dense
            var denseOther = other.Storage as DenseColumnMajorMatrixStorage<double>;
            var denseResult = result.Storage as DenseColumnMajorMatrixStorage<double>;
            if (denseOther != null && denseResult != null)
            {
                LinearAlgebraControl.Provider.AddArrays(_values, denseOther.Data, denseResult.Data);
                return;
            }

            // dense + diagonal = any
            var diagonalOther = other.Storage as DiagonalMatrixStorage<double>;
            if (diagonalOther != null)
            {
                Storage.CopyToUnchecked(result.Storage, ExistingData.Clear);
                var diagonal = diagonalOther.Data;
                for (var i = 0; i < diagonal.Length; i++) result.At(i, i, result.At(i, i) + diagonal[i]);
                return;
            }

            base.DoAdd(other, result);
        }

        /// <summary>
        ///     Subtracts a scalar from each element of the matrix and stores the result in the result vector.
        /// </summary>
        /// <param name="scalar">The scalar to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected override void DoSubtract(double scalar, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
            {
                base.DoSubtract(scalar, result);
                return;
            }

            CommonParallel.For(0, _values.Length, 4096, (a, b) =>
            {
                var v = denseResult._values;
                for (var i = a; i < b; i++) v[i] = _values[i] - scalar;
            });
        }

        /// <summary>
        ///     Subtracts another matrix from this matrix.
        /// </summary>
        /// <param name="other">The matrix to subtract.</param>
        /// <param name="result">The matrix to store the result of the subtraction.</param>
        protected override void DoSubtract(Matrix<double> other, Matrix<double> result)
        {
            // dense + dense = dense
            var denseOther = other.Storage as DenseColumnMajorMatrixStorage<double>;
            var denseResult = result.Storage as DenseColumnMajorMatrixStorage<double>;
            if (denseOther != null && denseResult != null)
            {
                LinearAlgebraControl.Provider.SubtractArrays(_values, denseOther.Data, denseResult.Data);
                return;
            }

            // dense + diagonal = matrix
            var diagonalOther = other.Storage as DiagonalMatrixStorage<double>;
            if (diagonalOther != null)
            {
                CopyTo(result);
                var diagonal = diagonalOther.Data;
                for (var i = 0; i < diagonal.Length; i++) result.At(i, i, result.At(i, i) - diagonal[i]);
                return;
            }

            base.DoSubtract(other, result);
        }

        /// <summary>
        ///     Multiplies each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="scalar">The scalar to multiply the matrix with.</param>
        /// <param name="result">The matrix to store the result of the multiplication.</param>
        protected override void DoMultiply(double scalar, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
                base.DoMultiply(scalar, result);
            else
                LinearAlgebraControl.Provider.ScaleArray(scalar, _values, denseResult._values);
        }

        /// <summary>
        ///     Multiplies this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Vector<double> rightSide, Vector<double> result)
        {
            var denseRight = rightSide as DenseVector;
            var denseResult = result as DenseVector;

            if (denseRight == null || denseResult == null)
                base.DoMultiply(rightSide, result);
            else
                LinearAlgebraControl.Provider.MatrixMultiply(
                    _values,
                    _rowCount,
                    _columnCount,
                    denseRight.Values,
                    denseRight.Count,
                    1,
                    denseResult.Values);
        }

        /// <summary>
        ///     Multiplies this matrix with another matrix and places the results into the result matrix.
        /// </summary>
        /// <param name="other">The matrix to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoMultiply(Matrix<double> other, Matrix<double> result)
        {
            var denseOther = other as DenseMatrix;
            var denseResult = result as DenseMatrix;
            if (denseOther != null && denseResult != null)
            {
                LinearAlgebraControl.Provider.MatrixMultiply(
                    _values,
                    _rowCount,
                    _columnCount,
                    denseOther._values,
                    denseOther._rowCount,
                    denseOther._columnCount,
                    denseResult._values);
                return;
            }

            var diagonalOther = other.Storage as DiagonalMatrixStorage<double>;
            if (diagonalOther != null)
            {
                var diagonal = diagonalOther.Data;
                var d = Math.Min(ColumnCount, other.ColumnCount);
                if (d < other.ColumnCount)
                    result.ClearSubMatrix(0, RowCount, ColumnCount, other.ColumnCount - ColumnCount);
                var index = 0;
                for (var j = 0; j < d; j++)
                    for (var i = 0; i < RowCount; i++)
                    {
                        result.At(i, j, _values[index] * diagonal[j]);
                        index++;
                    }

                return;
            }

            base.DoMultiply(other, result);
        }

        /// <summary>
        ///     Multiplies the transpose of this matrix with a vector and places the results into the result vector.
        /// </summary>
        /// <param name="rightSide">The vector to multiply with.</param>
        /// <param name="result">The result of the multiplication.</param>
        protected override void DoTransposeThisAndMultiply(Vector<double> rightSide, Vector<double> result)
        {
            var denseRight = rightSide as DenseVector;
            var denseResult = result as DenseVector;

            if (denseRight == null || denseResult == null)
                base.DoTransposeThisAndMultiply(rightSide, result);
            else
                LinearAlgebraControl.Provider.MatrixMultiplyWithUpdate(
                    Providers.LinearAlgebra.Transpose.Transpose,
                    Providers.LinearAlgebra.Transpose.DontTranspose,
                    1.0,
                    _values,
                    _rowCount,
                    _columnCount,
                    denseRight.Values,
                    denseRight.Count,
                    1,
                    0.0,
                    denseResult.Values);
        }

        /// <summary>
        ///     Divides each element of the matrix by a scalar and places results into the result matrix.
        /// </summary>
        /// <param name="divisor">The scalar to divide the matrix with.</param>
        /// <param name="result">The matrix to store the result of the division.</param>
        protected override void DoDivide(double divisor, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
                base.DoDivide(divisor, result);
            else
                LinearAlgebraControl.Provider.ScaleArray(1.0 / divisor, _values, denseResult._values);
        }


        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given divisor each element of the matrix.
        /// </summary>
        /// <param name="divisor">The scalar denominator to use.</param>
        /// <param name="result">Matrix to store the results in.</param>
        protected override void DoRemainder(double divisor, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
            {
                base.DoRemainder(divisor, result);
                return;
            }

            if (!ReferenceEquals(this, result)) CopyTo(result);

            CommonParallel.For(0, _values.Length, (a, b) =>
            {
                var v = denseResult._values;
                for (var i = a; i < b; i++) v[i] %= divisor;
            });
        }

        /// <summary>
        ///     Computes the remainder (% operator), where the result has the sign of the dividend,
        ///     for the given dividend for each element of the matrix.
        /// </summary>
        /// <param name="dividend">The scalar numerator to use.</param>
        /// <param name="result">A vector to store the results in.</param>
        protected override void DoRemainderByThis(double dividend, Matrix<double> result)
        {
            var denseResult = result as DenseMatrix;
            if (denseResult == null)
            {
                base.DoRemainderByThis(dividend, result);
                return;
            }

            CommonParallel.For(0, _values.Length, 4096, (a, b) =>
            {
                var v = denseResult._values;
                for (var i = a; i < b; i++) v[i] = dividend % _values[i];
            });
        }

        /// <summary>
        ///     Computes the trace of this matrix.
        /// </summary>
        /// <returns>The trace of this matrix</returns>
        /// <exceptioncreArgumentException">If the matrix is not square
        /// 
        /// </exception>
        public override double Trace()
        {
            if (_rowCount != _columnCount)
            {
                //throw new ArgumentException(Resources.ArgumentMatrixSquare);
            }

            var sum = 0.0;
            for (var i = 0; i < _rowCount; i++) sum += _values[i * _rowCount + i];

            return sum;
        }

        /// <summary>
        ///     Adds two matrices together and returns the results.
        /// </summary>
        /// <remarks>
        ///     This operator will allocate new memory for the result. It will
        ///     choose the representation of either <paramref name="leftSide or 
        ///     
        ///     <paramref name="rightSide depending on which
        /// is denser.
        /// 
        /// </remarks>
        /// <param name="leftSide">The left matrix to add.</param>
        /// <param name="rightSide">The right matrix to add.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide don't have the same dimensions.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator +(DenseMatrix leftSide, DenseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (leftSide._rowCount != rightSide._rowCount || leftSide._columnCount != rightSide._columnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(leftSide, rightSide);

            return (DenseMatrix)leftSide.Add(rightSide);
        }

        /// <summary>
        ///     Returns a <strong>Matrix</strong> containing the same values of <paramref name="rightSide.
        /// 
        /// 
        /// </summary>
        /// <param name="rightSide">The matrix to get the values from.</param>
        /// <returns>A matrix containing a the same values as <paramref name="rightSide.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator +(DenseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (DenseMatrix)rightSide.Clone();
        }

        /// <summary>
        ///     Subtracts two matrices together and returns the results.
        /// </summary>
        /// <remarks>
        ///     This operator will allocate new memory for the result. It will
        ///     choose the representation of either <paramref name="leftSide or 
        ///     
        ///     <paramref name="rightSide depending on which
        /// is denser.
        /// 
        /// </remarks>
        /// <param name="leftSide">The left matrix to subtract.</param>
        /// <param name="rightSide">The right matrix to subtract.</param>
        /// <returns>The result of the addition.</returns>
        /// <exceptioncreArgumentOutOfRangeException">If 
        /// 
        /// <paramref name="leftSide and 
        /// 
        /// <paramref name="rightSide don't have the same dimensions.
        /// 
        /// </exception>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator -(DenseMatrix leftSide, DenseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (leftSide._rowCount != rightSide._rowCount || leftSide._columnCount != rightSide._columnCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(leftSide, rightSide);

            return (DenseMatrix)leftSide.Subtract(rightSide);
        }

        /// <summary>
        ///     Negates each element of the matrix.
        /// </summary>
        /// <param name="rightSide">The matrix to negate.</param>
        /// <returns>A matrix containing the negated values.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator -(DenseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (DenseMatrix)rightSide.Negate();
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> by a constant and returns the result.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The constant to multiply the matrix by.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator *(DenseMatrix leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseMatrix)leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> by a constant and returns the result.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The constant to multiply the matrix by.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator *(double leftSide, DenseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (DenseMatrix)rightSide.Multiply(leftSide);
        }

        /// <summary>
        ///     Multiplies two matrices.
        /// </summary>
        /// <remarks>
        ///     This operator will allocate new memory for the result. It will
        ///     choose the representation of either <paramref name="leftSide or 
        ///     
        ///     <paramref name="rightSide depending on which
        /// is denser.
        /// 
        /// </remarks>
        /// <param name="leftSide">The left matrix to multiply.</param>
        /// <param name="rightSide">The right matrix to multiply.</param>
        /// <returns>The result of multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        /// <exceptioncreArgumentException">If the dimensions of 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide don't conform.
        /// 
        /// </exception>
        public static DenseMatrix operator *(DenseMatrix leftSide, DenseMatrix rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            if (leftSide._columnCount != rightSide._rowCount)
                throw DimensionsDontMatch<ArgumentOutOfRangeException>(leftSide, rightSide);

            return (DenseMatrix)leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> and a Vector.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The vector to multiply.</param>
        /// <returns>The result of multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator *(DenseMatrix leftSide, DenseVector rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseVector)leftSide.Multiply(rightSide);
        }

        /// <summary>
        ///     Multiplies a Vector and a <strong>Matrix</strong>.
        /// </summary>
        /// <param name="leftSide">The vector to multiply.</param>
        /// <param name="rightSide">The matrix to multiply.</param>
        /// <returns>The result of multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide or 
        /// 
        /// <paramref name="rightSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseVector operator *(DenseVector leftSide, DenseMatrix rightSide)
        {
            if (rightSide == null) throw new ArgumentNullException(nameof(rightSide));

            return (DenseVector)rightSide.LeftMultiply(leftSide);
        }

        /// <summary>
        ///     Multiplies a <strong>Matrix</strong> by a constant and returns the result.
        /// </summary>
        /// <param name="leftSide">The matrix to multiply.</param>
        /// <param name="rightSide">The constant to multiply the matrix by.</param>
        /// <returns>The result of the multiplication.</returns>
        /// <exceptioncreArgumentNullException">If 
        /// 
        /// <paramref name="leftSide is 
        /// 
        /// <see langword="null" />
        /// .
        /// </exception>
        public static DenseMatrix operator %(DenseMatrix leftSide, double rightSide)
        {
            if (leftSide == null) throw new ArgumentNullException(nameof(leftSide));

            return (DenseMatrix)leftSide.Remainder(rightSide);
        }

        /// <summary>
        ///     Evaluates whether this matrix is symmetric.
        /// </summary>
        public virtual bool IsSymmetric()
        {
            if (RowCount != ColumnCount) return false;

            for (var j = 0; j < ColumnCount; j++)
            {
                var index = j * RowCount;
                for (var i = j + 1; i < RowCount; i++)
                    if (_values[i * ColumnCount + j] != _values[index + i])
                        return false;
            }

            return true;
        }

        public override LU<double> LU()
        {
            return DenseLU.Create(this);
        }

        public override QR<double> QR(QRMethod method = QRMethod.Thin)
        {
            return DenseQR.Create(this, method);
        }

    }
}