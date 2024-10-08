﻿using System.Numerics;

namespace FanKit.Transformers
{
    partial struct Transformer
    {
        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct LinePD1C
        {
            public Vector2 Point;
            public Vector2 DiagonalPoint;

            public Vector2 CanvasPoint;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct LineP1C
        {
            public Vector2 Point;

            public Vector2 CanvasPoint;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct LineD1C
        {
            public Vector2 DiagonalPoint;

            public Vector2 CanvasPoint;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct CornerRL2
        {
            public Vector2 PointRight;
            public Vector2 PointLeft;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct CornerRLD3
        {
            public Vector2 DiagonalPoint;
            public Vector2 PointRight;
            public Vector2 PointLeft;
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static CornerRL2 ScaleCorner(Transformer startingTransformer, LineD1C line)
        {
            Vector2 horizontal = startingTransformer.Horizontal;
            Vector2 vertical = startingTransformer.Vertical;

            return new CornerRL2
            {
                PointRight = Math.IntersectionPoint(line.CanvasPoint, line.CanvasPoint - horizontal, line.DiagonalPoint + vertical, line.DiagonalPoint),
                PointLeft = Math.IntersectionPoint(line.CanvasPoint, line.CanvasPoint - vertical, line.DiagonalPoint + horizontal, line.DiagonalPoint),
            };
        }

        // Scale Corner
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static CornerRLD3 ScaleCornerCenter(Transformer startingTransformer, Vector2 canvasPoint)
        {
            Vector2 center = startingTransformer.Center * 2 - canvasPoint;
            Vector2 horizontal = startingTransformer.Horizontal;
            Vector2 vertical = startingTransformer.Vertical;

            return new CornerRLD3
            {
                DiagonalPoint = center,
                PointRight = Math.IntersectionPoint(canvasPoint, canvasPoint - horizontal, center + vertical, center),
                PointLeft = Math.IntersectionPoint(canvasPoint, canvasPoint - vertical, center + horizontal, center),
            };
        }

        #region ScaleCorner LeftTop

        /// <summary>
        /// Move the position of the top-left corner of the transformer.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="isConvexQuadrilateral"> Is the transformer is a convex quadrilateral? </param>
        /// <returns> The product point. </returns>
        public Transformer MoveLeftTop(Vector2 point, bool isConvexQuadrilateral) => new Transformer
        {
            LeftTop = isConvexQuadrilateral ?
            Math.MovePointOfConvexQuadrilateral(
                point: point,
                left: this.RightTop,
                diagonal: this.RightBottom,
                right: this.LeftBottom,
                margin: 8) : point,
            RightTop = this.RightTop,
            RightBottom = this.RightBottom,
            LeftBottom = this.LeftBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromLeftTop(Transformer startingTransformer, LineD1C line)
        {
            CornerRL2 corner = Transformer.ScaleCorner(startingTransformer, line);
            return new Transformer
            {
                LeftTop = line.CanvasPoint,
                RightTop = corner.PointRight,
                RightBottom = line.DiagonalPoint,
                LeftBottom = corner.PointLeft,
            };
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromLeftTop(Transformer startingTransformer, Vector2 canvasPoint)
        {
            CornerRLD3 corner = Transformer.ScaleCornerCenter(startingTransformer, canvasPoint);
            return new Transformer
            {
                LeftTop = canvasPoint,
                RightTop = corner.PointRight,
                RightBottom = corner.DiagonalPoint,
                LeftBottom = corner.PointLeft,
            };
        }

        #endregion

        #region ScaleCorner RightTop

        /// <summary>
        /// Move the position of the top-right corner of the transformer.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="isConvexQuadrilateral"> Is the transformer is a convex quadrilateral? </param>
        /// <returns> The product point. </returns>
        public Transformer MoveRightTop(Vector2 point, bool isConvexQuadrilateral) => new Transformer
        {
            LeftTop = this.LeftTop,
            RightTop = isConvexQuadrilateral ?
            Math.MovePointOfConvexQuadrilateral(
                point: point,
                right: this.LeftTop,
                diagonal: this.LeftBottom,
                left: this.RightBottom,
                margin: 8) : point,
            RightBottom = this.RightBottom,
            LeftBottom = this.LeftBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromRightTop(Transformer startingTransformer, LineD1C line)
        {
            CornerRL2 corner = Transformer.ScaleCorner(startingTransformer, line);
            return new Transformer
            {
                RightTop = line.CanvasPoint,
                LeftTop = corner.PointRight,
                LeftBottom = line.DiagonalPoint,
                RightBottom = corner.PointLeft,
            };
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromRightTop(Transformer startingTransformer, Vector2 canvasPoint)
        {
            CornerRLD3 corner = Transformer.ScaleCornerCenter(startingTransformer, canvasPoint);
            return new Transformer
            {
                RightTop = canvasPoint,
                LeftTop = corner.PointRight,
                LeftBottom = corner.DiagonalPoint,
                RightBottom = corner.PointLeft,
            };
        }

        #endregion

        #region ScaleCorner RightBottom

        /// <summary>
        /// Move the position of the bottom-right corner of the transformer.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="isConvexQuadrilateral"> Is the transformer is a convex quadrilateral? </param>
        /// <returns> The product point. </returns>
        public Transformer MoveRightBottom(Vector2 point, bool isConvexQuadrilateral) => new Transformer
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            RightBottom = isConvexQuadrilateral ?
            Math.MovePointOfConvexQuadrilateral(
                point: point,
                left: this.LeftBottom,
                diagonal: this.LeftTop,
                right: this.RightTop,
                margin: 8) : point,
            LeftBottom = this.LeftBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromRightBottom(Transformer startingTransformer, LineD1C line)
        {
            CornerRL2 corner = Transformer.ScaleCorner(startingTransformer, line);
            return new Transformer
            {
                RightBottom = line.CanvasPoint,
                LeftBottom = corner.PointRight,
                LeftTop = line.DiagonalPoint,
                RightTop = corner.PointLeft,
            };
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromRightBottom(Transformer startingTransformer, Vector2 canvasPoint)
        {
            CornerRLD3 corner = Transformer.ScaleCornerCenter(startingTransformer, canvasPoint);
            return new Transformer
            {
                RightBottom = canvasPoint,
                LeftBottom = corner.PointRight,
                LeftTop = corner.DiagonalPoint,
                RightTop = corner.PointLeft,
            };
        }

        #endregion

        #region ScaleCorner LeftBottom

        /// <summary>
        /// Move the position of the bottom-left corner of the transformer.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="isConvexQuadrilateral"> Is the transformer is a convex quadrilateral? </param>
        /// <returns> The product point. </returns>
        public Transformer MoveLeftBottom(Vector2 point, bool isConvexQuadrilateral) => new Transformer
        {
            LeftTop = this.LeftTop,
            RightTop = this.RightTop,
            RightBottom = this.RightBottom,
            LeftBottom = isConvexQuadrilateral ?
            Math.MovePointOfConvexQuadrilateral(
                point: point,
                right: this.RightBottom,
                diagonal: this.RightTop,
                left: this.LeftTop,
                margin: 8) : point,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromLeftBottom(Transformer startingTransformer, LineD1C line)
        {
            CornerRL2 corner = Transformer.ScaleCorner(startingTransformer, line);
            return new Transformer
            {
                LeftBottom = line.CanvasPoint,
                RightBottom = corner.PointRight,
                RightTop = line.DiagonalPoint,
                LeftTop = corner.PointLeft,
            };
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer FromLeftBottom(Transformer startingTransformer, Vector2 canvasPoint)
        {
            CornerRLD3 corner = Transformer.ScaleCornerCenter(startingTransformer, canvasPoint);
            return new Transformer
            {
                LeftBottom = canvasPoint,
                RightBottom = corner.PointRight,
                RightTop = corner.DiagonalPoint,
                LeftTop = corner.PointLeft,
            };
        }

        #endregion

    }
}