using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Brushes;
using System;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Provides brushe for drawing dotted-line.
    /// </summary>
    public class DottedLineBrush : IDisposable
    {

        CanvasGradientStop[] Stops = new CanvasGradientStop[2]
        {
            new CanvasGradientStop
            {
                Color = Windows.UI.Colors.White,
                Position = 0
            },
            new CanvasGradientStop
            {
                Color = Windows.UI.Colors.Black, Position = 1
            }
        };

        /// <summary> Gets the linear-gradient-brush. </summary>
        public CanvasLinearGradientBrush Brush { get; private set; }

        //@Constructs
        /// <summary>
        /// Initialize a brush for drawing dotted-line.
        /// </summary>
        /// <param name="resourceCreator"> The resource-creator. </param>
        /// <param name="distance"> The sistance between two gradient points. </param>
        public DottedLineBrush(ICanvasResourceCreator resourceCreator, float distance = 6)
        {
            this.Brush = new CanvasLinearGradientBrush(resourceCreator, Stops, CanvasEdgeBehavior.Mirror, CanvasAlphaMode.Premultiplied)
            {
                StartPoint = new Vector2(0, 0),
                EndPoint = new Vector2(distance, distance)
            };
        }

        /// <summary>
        /// Update the displacement of the brush
        /// </summary>
        /// <param name="space"> The change of displacement. </param>
        public void Update(float space = 1)
        {
            Vector2 vector = new Vector2(space, space);
            this.Brush.StartPoint -= vector;
            this.Brush.EndPoint -= vector;
        }

        /// <summary>
        /// Execute and release or reset unmanaged resources
        /// </summary>
        public void Dispose()
        {
            this.Brush.Dispose();
            this.Brush = null;
            this.Stops = null;
        }
    }
}