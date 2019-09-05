using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Effects;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides output image for drawing dotted-line.
    /// </summary>
    public class DottedLineImage : IDisposable
    {
        CanvasRenderTarget _input;

        /// <summary> The input images baked. </summary>
        public ICanvasImage Output { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DottedLineImage class.
        /// </summary>
        /// <param name="input"> The input image. </param>
        public DottedLineImage(CanvasRenderTarget input) => this._input = input;

        /// <summary>
        /// Returns a new drawing session. The drawing session draws onto the CanvasRenderTarget.
        /// </summary>
        /// <returns> The drawing-session. </returns>
        public CanvasDrawingSession CreateDrawingSession() => this._input.CreateDrawingSession();

        /// <summary>
        /// Turn the input image into an image edge line.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="isCrop"> Whether to crop the edge. </param>
        public void Baking(ICanvasResourceCreator resourceCreator, bool isCrop = true)
        {
            IGraphicsEffectSource crop = (isCrop == false) ? this._input : this._createCrop(resourceCreator, this._input);

            this.Output = this._createLuminance(resourceCreator, crop);
        }

        /// <summary>
        /// Turn the input image into an image edge line.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="matrix"> The matrix. </param>
        /// <param name="isCrop"> Whether to crop the edge. </param>
        public void Baking(ICanvasResourceCreator creator, Matrix3x2 matrix, bool isCrop = true)
        {
            IGraphicsEffectSource crop = (isCrop == false) ? this._input : this._createCrop(creator, this._input);

            Transform2DEffect transform = new Transform2DEffect//Transform
            {
                TransformMatrix = matrix,
                Source = crop
            };

              this.Output = this._createLuminance(creator, transform);
        }

        private ICanvasImage _createLuminance(ICanvasResourceCreator resourceCreator, IGraphicsEffectSource image)
        {
            return new LuminanceToAlphaEffect//Alpha
            {
                Source = new EdgeDetectionEffect//Edge
                {
                    Amount = 1,
                    Source = image,
                }
            };
        }
        private IGraphicsEffectSource _createCrop(ICanvasResourceCreator resourceCreator, CanvasRenderTarget canvasRenderTarget)
        {
            Rect cropRectangle = new Rect(2, 2, canvasRenderTarget.SizeInPixels.Width - 4, canvasRenderTarget.SizeInPixels.Height - 4);

            CanvasCommandList canvasCommandList = new CanvasCommandList(resourceCreator);
            using (CanvasDrawingSession drawingSession = canvasCommandList.CreateDrawingSession())
            {
                drawingSession.Clear(Windows.UI.Colors.Transparent);
                drawingSession.DrawImage(new CropEffect
                {
                    Source = canvasRenderTarget,
                    SourceRectangle = cropRectangle,
                });
            };
            return canvasCommandList;
        }


        /// <summary>
        /// Execute and release or reset unmanaged resources
        /// </summary>
        public void Dispose()
        {
            if (this.Output != null)
            {
                this.Output.Dispose();
                this.Output = null;
            }

            if (this._input != null)
            {
                this._input.Dispose();
            }
        }
    }
}