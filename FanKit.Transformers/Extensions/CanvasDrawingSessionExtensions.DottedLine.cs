using Microsoft.Graphics.Canvas;
using Windows.Foundation;

namespace FanKit.Transformers
{
    public static partial class CanvasDrawingSessionExtensions
    {

        /// <summary>
        /// Draw dotted-line.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="dottedLineBrush"> The brush. </param>
        /// <param name="dottedLineImage"> The image. </param>
        /// <param name="width"> The width. </param>
        /// <param name="height"> The height. </param>
        /// <param name="x"> The X-axis distance. </param>
        /// <param name="y"> The Y-axis distance. </param>
        public static void DrawDottedLine(this CanvasDrawingSession drawingSession, ICanvasResourceCreator resourceCreator, DottedLineBrush dottedLineBrush, DottedLineImage dottedLineImage, float width, float height, float x = 0, float y = 0)
        {
            ICanvasImage image = dottedLineImage.Output;
            Rect canvasBounds = new Rect(x, y, width, height);

            CanvasCommandList commandList = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession ds = commandList.CreateDrawingSession())
            {
                ds.FillRectangle(canvasBounds, dottedLineBrush.Brush);
                ds.DrawImage(image, x, y, canvasBounds, 1, CanvasImageInterpolation.NearestNeighbor, CanvasComposite.DestinationIn);
            }
            drawingSession.DrawImage(commandList);
        }

    }
}