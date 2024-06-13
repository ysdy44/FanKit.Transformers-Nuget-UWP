using System.Numerics;

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

        #region ScaleCorner RightBottom

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

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleLeft(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            RightTop = startingTransformer.RightTop,
            RightBottom = startingTransformer.RightBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleLeftCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            RightTop = startingTransformer.RightTop - vector,
            RightBottom = startingTransformer.RightBottom - vector,
        };

        #endregion

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleTop(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            RightTop = startingTransformer.RightTop + vector,

            RightBottom = startingTransformer.RightBottom,
            LeftBottom = startingTransformer.LeftBottom,
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleTopCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            LeftTop = startingTransformer.LeftTop + vector,
            RightTop = startingTransformer.RightTop + vector,

            RightBottom = startingTransformer.RightBottom - vector,
            LeftBottom = startingTransformer.LeftBottom - vector,
        };

        #endregion

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleRight(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightTop = startingTransformer.RightTop + vector,
            RightBottom = startingTransformer.RightBottom + vector,

            LeftTop = startingTransformer.LeftTop,
            LeftBottom = startingTransformer.LeftBottom
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleRightCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightTop = startingTransformer.RightTop + vector,
            RightBottom = startingTransformer.RightBottom + vector,

            LeftTop = startingTransformer.LeftTop - vector,
            LeftBottom = startingTransformer.LeftBottom - vector,
        };

        #endregion

        #region ScaleAround Left

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleBottom(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightBottom = startingTransformer.RightBottom + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            LeftTop = startingTransformer.LeftTop,
            RightTop = startingTransformer.RightTop
        };

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static Transformer ScaleBottomCenter(Transformer startingTransformer, Vector2 vector) => new Transformer
        {
            RightBottom = startingTransformer.RightBottom + vector,
            LeftBottom = startingTransformer.LeftBottom + vector,

            LeftTop = startingTransformer.LeftTop - vector,
            RightTop = startingTransformer.RightTop - vector
        };

        #endregion

    }
}