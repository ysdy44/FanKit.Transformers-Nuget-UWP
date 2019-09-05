using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Linq;
using System.Numerics;
using Windows.Foundation;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {

        /// <summary>
        /// Fill a marquee mask with a marquee-tool.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="toolType"> The marquee-tool type. </param>
        /// <param name="marqueeTool"> The marquee-tool. </param>
        /// <param name="sourceRectangle"> The source rectangle. </param>
        /// <param name="compositeMode"> The composite mode. </param>
        public static void FillMarqueeMaskl(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, MarqueeToolType toolType, MarqueeTool marqueeTool, Rect sourceRectangle, MarqueeCompositeMode compositeMode)
        {
            switch (toolType)
            {
                case MarqueeToolType.Rectangular:
                    drawingSession.DrawMarqueeToolRectangular(resourceCreator, marqueeTool.TransformerRect, sourceRectangle, compositeMode);
                    break;
                case MarqueeToolType.Elliptical:
                    drawingSession.DrawMarqueeToolEllipse(resourceCreator, marqueeTool.TransformerRect, sourceRectangle, compositeMode);
                    break;
                case MarqueeToolType.Polygonal:
                case MarqueeToolType.FreeHand:
                    Vector2[] points = marqueeTool.Points.ToArray();
                    CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(resourceCreator, marqueeTool.Points.ToArray());
                    drawingSession.DrawMarqueeToolGeometry(resourceCreator, canvasGeometry, sourceRectangle, compositeMode);
                    break;
            }
        }


        #region Draw

        /// <summary>
        /// Draw a marquee-tool.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="toolType"> The marquee-tool type. </param>
        /// <param name="marqueeTool"> The marquee-tool. </param>
        public static void DrawMarqueeTool(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, MarqueeToolType toolType, MarqueeTool marqueeTool)
        {
            if (marqueeTool.IsStarted)
            {
                switch (toolType)
                {
                    case MarqueeToolType.Rectangular:
                        {
                            drawingSession.DrawThickRectangle(marqueeTool.TransformerRect);
                        }
                        break;
                    case MarqueeToolType.Elliptical:
                        {
                            drawingSession.DrawThickRectangle(marqueeTool.TransformerRect);

                        }
                        break;
                    case MarqueeToolType.Polygonal:
                        {
                            Vector2[] points = marqueeTool.Points.ToArray();
                            CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(resourceCreator, points);
                            drawingSession.DrawThickGeometry(canvasGeometry);

                            Vector2 firstPoint = marqueeTool.Points.First();
                            Vector2 lastPoint = marqueeTool.Points.Last();
                            drawingSession.DrawNode5(firstPoint);
                            drawingSession.DrawNode5(lastPoint);
                        }
                        break;
                    case MarqueeToolType.FreeHand:
                        {
                            Vector2[] points = marqueeTool.Points.ToArray();
                            CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(resourceCreator, points);
                            drawingSession.DrawThickGeometry(canvasGeometry);
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// Draw a marquee-tool.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="toolType"> The marquee-tool type. </param>
        /// <param name="marqueeTool"> The marquee-tool. </param>
        /// <param name="matrix"> The matrix. </param>
        public static void DrawMarqueeTool(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, MarqueeToolType toolType, MarqueeTool marqueeTool, Matrix3x2 matrix)
        {
            if (marqueeTool.IsStarted)
            {
                switch (toolType)
                {
                    case MarqueeToolType.Rectangular:
                        {
                            CanvasGeometry canvasGeometry = TransformerRect.CreateRectangle(resourceCreator, marqueeTool.TransformerRect, matrix);
                            drawingSession.DrawThickGeometry(canvasGeometry);
                        }
                        break;
                    case MarqueeToolType.Elliptical:
                        {
                            CanvasGeometry canvasGeometry = TransformerRect.CreateEllipse(resourceCreator, marqueeTool.TransformerRect, matrix);
                            drawingSession.DrawThickGeometry(canvasGeometry);
                        }
                        break;
                    case MarqueeToolType.Polygonal:
                        {
                            Vector2[] points = marqueeTool.Points.ToArray();
                            CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(resourceCreator, points);
                            CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                            drawingSession.DrawThickGeometry(canvasGeometryTransform);

                            Vector2 firstPoint = Vector2.Transform(marqueeTool.Points.First(), matrix);
                            Vector2 lastPoint = Vector2.Transform(marqueeTool.Points.Last(), matrix);
                            drawingSession.DrawNode5(firstPoint);
                            drawingSession.DrawNode5(lastPoint);
                        }
                        break;
                    case MarqueeToolType.FreeHand:
                        {
                            Vector2[] points = marqueeTool.Points.ToArray();
                            CanvasGeometry canvasGeometry = CanvasGeometry.CreatePolygon(resourceCreator, points);
                            CanvasGeometry canvasGeometryTransform = canvasGeometry.Transform(matrix);
                            drawingSession.DrawThickGeometry(canvasGeometryTransform);
                        }
                        break;
                }
            }
        }

        #endregion


        #region Geometry

        private static CanvasCommandList _getMarqueeToolGeometry(CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, CanvasGeometry canvasGeometry)
        {
            CanvasCommandList canvasCommandList = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = canvasCommandList.CreateDrawingSession())
            {
                ds.FillGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
            }
            return canvasCommandList;
        }

        /// <summary>
        /// Draw a Geometry for marquee-tool (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="canvasGeometry"> The canvas-geometry. </param>
        /// <param name="sourceRectangle"> The source rectangle. </param>
        /// <param name="compositeMode"> The composite mode. </param>
        public static void DrawMarqueeToolGeometry(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, CanvasGeometry canvasGeometry, Rect sourceRectangle, MarqueeCompositeMode compositeMode = MarqueeCompositeMode.New)
        {
            switch (compositeMode)
            {
                case MarqueeCompositeMode.New:
                    {
                        drawingSession.Clear(Windows.UI.Colors.Transparent);
                        drawingSession.FillGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
                    }
                    break;
                case MarqueeCompositeMode.Add:
                    {
                        drawingSession.FillGeometry(canvasGeometry, Windows.UI.Colors.DodgerBlue);
                    }
                    break;
                case MarqueeCompositeMode.Subtract:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolGeometry(drawingSession,resourceCreator, canvasGeometry);
                        CanvasComposite canvasComposite = CanvasComposite.DestinationOut;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
                case MarqueeCompositeMode.Intersect:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolGeometry(drawingSession, resourceCreator, canvasGeometry);
                        CanvasComposite canvasComposite = CanvasComposite.DestinationIn;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
                case MarqueeCompositeMode.Xor:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolGeometry(drawingSession, resourceCreator, canvasGeometry);
                        CanvasComposite canvasComposite = CanvasComposite.Xor;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
            }
        }
        
        #endregion


        #region Rectangular

        private static CanvasCommandList _getMarqueeToolRectangular(CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect)
        {
            CanvasCommandList canvasCommandList = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = canvasCommandList.CreateDrawingSession())
            {
                ds.FillRectangleDodgerBlue(transformerRect);
            }
            return canvasCommandList;
        }

        /// <summary>
        /// Draw a Rectangular for marquee-tool (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <param name="sourceRectangle"> The source rectangle. </param>
        /// <param name="compositeMode"> The composite mode. </param>
        public static void DrawMarqueeToolRectangular(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect, Rect sourceRectangle,MarqueeCompositeMode compositeMode = MarqueeCompositeMode.New)
        {
            switch (compositeMode)
            {
                case MarqueeCompositeMode.New:
                    {
                        drawingSession.Clear(Windows.UI.Colors.Transparent);
                        Rect rect = transformerRect.ToRect();
                        drawingSession.FillRectangle(rect, Windows.UI.Colors.DodgerBlue);
                    }
                    break;
                case MarqueeCompositeMode.Add:
                    {
                        Rect rect = transformerRect.ToRect();
                        drawingSession.FillRectangle(rect, Windows.UI.Colors.DodgerBlue);
                    }
                    break;
                case MarqueeCompositeMode.Subtract:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolRectangular(drawingSession, resourceCreator, transformerRect);
                        CanvasComposite canvasComposite = CanvasComposite.DestinationOut;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
                case MarqueeCompositeMode.Intersect:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolRectangular(drawingSession, resourceCreator, transformerRect);
                        CanvasComposite canvasComposite = CanvasComposite.DestinationIn;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
                case MarqueeCompositeMode.Xor:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolRectangular(drawingSession, resourceCreator, transformerRect);
                        CanvasComposite canvasComposite = CanvasComposite.Xor;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
            }
        }

        #endregion

                 
        #region Ellipse
        
        private static CanvasCommandList _getMarqueeToolEllipse(CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect)
        {
            CanvasCommandList canvasCommandList = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = canvasCommandList.CreateDrawingSession())
            {
                ds.FillEllipseDodgerBlue(transformerRect);
            }
            return canvasCommandList;
        }

        /// <summary>
        /// Draw a Ellipse for marquee-tool (color is <see cref="Windows.UI.Colors.DodgerBlue"/>).
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="transformerRect"> The transformer-rect. </param>
        /// <param name="sourceRectangle"> The source rectangle. </param>
        /// <param name="compositeMode"> The composite mode. </param>
        public static void DrawMarqueeToolEllipse(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, TransformerRect transformerRect,Rect sourceRectangle, MarqueeCompositeMode compositeMode = MarqueeCompositeMode.New)
        {
            switch (compositeMode)
            {
                case MarqueeCompositeMode.New:
                    {
                        drawingSession.Clear(Windows.UI.Colors.Transparent);
                        drawingSession.FillEllipseDodgerBlue(transformerRect);
                    }
                    break;
                case MarqueeCompositeMode.Add:
                    {
                        drawingSession.FillEllipseDodgerBlue(transformerRect);
                    }
                    break;
                case MarqueeCompositeMode.Subtract:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolEllipse(drawingSession, resourceCreator, transformerRect);
                        CanvasComposite canvasComposite = CanvasComposite.DestinationOut;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
                case MarqueeCompositeMode.Intersect:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolEllipse(drawingSession, resourceCreator, transformerRect);
                        CanvasComposite canvasComposite = CanvasComposite.DestinationIn;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
                case MarqueeCompositeMode.Xor:
                    {
                        CanvasCommandList canvasCommandList = CanvasDrawingSessionExtensions._getMarqueeToolEllipse(drawingSession, resourceCreator, transformerRect);
                        CanvasComposite canvasComposite = CanvasComposite.Xor;
                        drawingSession.DrawImage(canvasCommandList, 0, 0, sourceRectangle, 1, CanvasImageInterpolation.Linear, canvasComposite);
                    }
                    break;
            }
        }

        #endregion

    }
}