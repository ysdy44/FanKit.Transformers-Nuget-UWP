using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Extensions of <see cref = "CanvasDrawingSession" />.
    /// </summary>
    public static partial class CanvasDrawingSessionExtensions
    {
        //Text
        /// <summary> The color of the text. Default (A255 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color TextColor = Windows.UI.Color.FromArgb(255, 127, 127, 127);
        /// <summary> The format of the text. </summary>
        public static readonly CanvasTextFormat TextFormat = new CanvasTextFormat()
        {
            FontSize = 14,
            HorizontalAlignment = CanvasHorizontalAlignment.Center,
            VerticalAlignment = CanvasVerticalAlignment.Center
        };



        //Axis
        /// <summary> The distance between two lines. Default 12.</summary>
        public const float AxisLine = 12;
        /// <summary> The distance between two thick lines. Default 20.</summary>
        public const float AxisThickLine = 20;
        /// <summary> The color of the axis. Default (A255 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color AxisColor = Windows.UI.Color.FromArgb(255, 127, 127, 127);
        /// <summary> The color of the axis line. Default (A127 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color AxisLineColor = Windows.UI.Color.FromArgb(127, 127, 127, 127);
        /// <summary> The color of the axis thick line. Default (A127 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color AxisThickLineColor = Windows.UI.Color.FromArgb(127, 127, 127, 127);

        private static void _drawAxis(CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float axisLine, float axisThickLine, Windows.UI.Color axisColor, Windows.UI.Color axisLineColor, Windows.UI.Color axisThickLineColor, Windows.UI.Color textColor, CanvasTextFormat textFormat)
        {
            //Canvas
            Vector2 position = canvasTransformer.Position;
            float scale = canvasTransformer.Scale;
            float controlWidth = canvasTransformer.ControlWidth;
            float controlHeight = canvasTransformer.ControlHeight;

            //Horizontal: Axis-X
            drawingSession.DrawLine(0, position.Y, controlWidth, position.Y, axisColor);
            //Vertical: Axis-Y
            drawingSession.DrawLine(position.X, 0, position.X, controlHeight, axisColor);

            //Space
            float space = 10 * scale;
            while (space < 10) space *= 5;
            while (space > 100) space /= 5;
            float space5 = space * 5;

            //Horizontal: Lines-X
            for (float X = position.X; X < controlWidth; X += space) drawingSession.DrawLine(X, position.Y - axisLine, X, position.Y, axisLineColor);//right
            for (float X = position.X; X > 0; X -= space) drawingSession.DrawLine(X, position.Y - axisLine, X, position.Y, axisLineColor);//left
            //Vertical: Lines-Y
            for (float Y = position.Y; Y < controlHeight; Y += space) drawingSession.DrawLine(position.X, Y, position.X + axisLine, Y, axisLineColor);//bottom
            for (float Y = position.Y; Y > 0; Y -= space) drawingSession.DrawLine(position.X, Y, position.X + axisLine, Y, axisLineColor);//top

            //Horizontal: ThickLine-X
            for (float X = position.X; X < controlWidth; X += space5) drawingSession.DrawLine(X, position.Y - axisThickLine, X, position.Y, axisThickLineColor);//right
            for (float X = position.X; X > 0; X -= space5) drawingSession.DrawLine(X, position.Y - axisThickLine, X, position.Y, axisThickLineColor);//left
            //Vertical: ThickLine-Y
            for (float Y = position.Y; Y < controlHeight; Y += space5) drawingSession.DrawLine(position.X, Y, position.X + axisThickLine, Y, axisThickLineColor);//bottom
            for (float Y = position.Y; Y > 0; Y -= space5) drawingSession.DrawLine(position.X, Y, position.X + axisThickLine, Y, axisThickLineColor);//top

            //Horizontal: Text-X
            for (float X = position.X; X < controlWidth; X += space5) drawingSession.DrawText(((int)(Math.Round((X - position.X) / scale))).ToString(), X, position.Y + axisLine, textColor, textFormat);
            for (float X = position.X; X > axisThickLine; X -= space5) drawingSession.DrawText(((int)(Math.Round((X - position.X) / scale))).ToString(), X, position.Y + axisLine, textColor, textFormat);
            //Vertical: Text-Y
            for (float Y = position.Y; Y < controlHeight; Y += space5) drawingSession.DrawText(((int)(Math.Round((Y - position.Y) / scale))).ToString(), position.X - axisLine, Y, textColor, textFormat);
            for (float Y = position.Y; Y > axisThickLine; Y -= space5) drawingSession.DrawText(((int)(Math.Round((Y - position.Y) / scale))).ToString(), position.X - axisLine, Y, textColor, textFormat);
        }


        /// <summary>
        /// Draw a Axis-XY.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        public static void DrawAxis(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer) => CanvasDrawingSessionExtensions._drawAxis(drawingSession, canvasTransformer, CanvasDrawingSessionExtensions.AxisLine, CanvasDrawingSessionExtensions.AxisThickLine, CanvasDrawingSessionExtensions.AxisColor, CanvasDrawingSessionExtensions.AxisLineColor, CanvasDrawingSessionExtensions.AxisThickLineColor, CanvasDrawingSessionExtensions.TextColor, CanvasDrawingSessionExtensions.TextFormat);

        /// <summary>
        /// Draw a Axis-XY.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="axisLine"> The distance between two lines. </param>
        /// <param name="axisThickLine"> The distance between two thick lines. </param>
        public static void DrawAxis(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float axisLine, float axisThickLine) => CanvasDrawingSessionExtensions._drawAxis(drawingSession, canvasTransformer, axisLine, axisThickLine, CanvasDrawingSessionExtensions.AxisColor, CanvasDrawingSessionExtensions.AxisLineColor, CanvasDrawingSessionExtensions.AxisThickLineColor, CanvasDrawingSessionExtensions.TextColor, CanvasDrawingSessionExtensions.TextFormat);

        /// <summary>
        /// Draw a Axis-XY.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="axisLine"> The distance between two lines. </param>
        /// <param name="axisThickLine"> The distance between two thick lines. </param>
        /// <param name="axisColor"> The color of the axis. </param>
        /// <param name="axisLineColor"> The color of the axis line. </param>
        /// <param name="axisThickLineColor"> The color of the axis thick line. </param>
        public static void DrawAxis(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float axisLine, float axisThickLine, Windows.UI.Color axisColor, Windows.UI.Color axisLineColor, Windows.UI.Color axisThickLineColor) => CanvasDrawingSessionExtensions._drawAxis(drawingSession, canvasTransformer, axisLine, axisThickLine, axisColor, axisLineColor, axisThickLineColor, CanvasDrawingSessionExtensions.TextColor, CanvasDrawingSessionExtensions.TextFormat);

        /// <summary>
        /// Draw a Axis-XY.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="axisLine"> The distance between two lines. </param>
        /// <param name="axisThickLine"> The distance between two thick lines. </param>
        /// <param name="axisColor"> The color of the axis. </param>
        /// <param name="axisLineColor"> The color of the axis line. </param>
        /// <param name="axisThickLineColor"> The color of the axis thick line. </param>
        /// <param name="textColor"> The color of the text. </param>
        /// <param name="textFormat"> The format of the text. </param>
        public static void DrawAxis(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float axisLine, float axisThickLine, Windows.UI.Color axisColor, Windows.UI.Color axisLineColor, Windows.UI.Color axisThickLineColor, Windows.UI.Color textColor, CanvasTextFormat textFormat) => CanvasDrawingSessionExtensions._drawAxis(drawingSession, canvasTransformer, axisLine, axisThickLine, axisColor, axisLineColor, axisThickLineColor, textColor, textFormat);




        //Ruler
        /// <summary> The width of the ruler. Default 20.</summary>
        public const float RulerWidth = 20;
        /// <summary> The distance between two lines. Default 8.</summary>
        public const float RulerLine = 8;
        /// <summary> The distance between two thick lines. Default 12.</summary>
        public const float RulerThickLine = 12;

        /// <summary> The color of the ruler background color. Default (A64 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color RulerBackgroundColor = Windows.UI.Color.FromArgb(64, 127, 127, 127);
        /// <summary> The color of the ruler. Default (A255 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color RulerColor = Windows.UI.Color.FromArgb(255, 127, 127, 127);
        /// <summary> The color of the ruler line. Default (A127 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color RulerLineColor = Windows.UI.Color.FromArgb(127, 127, 127, 127);
        /// <summary> The color of the ruler thick line. Default (A127 R127 G127 B127).</summary>
        public static readonly Windows.UI.Color RulerThickLineColor = Windows.UI.Color.FromArgb(127, 127, 127, 127);

        private static void _drawRuler(CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float rulerWidth, float rulerLine, float rulerThickLine, Windows.UI.Color rulerBackgroundColor, Windows.UI.Color rulerColor, Windows.UI.Color rulerLineColor, Windows.UI.Color rulerThickLineColor, Windows.UI.Color textColor, CanvasTextFormat textFormat)
        {
            //Canvas
            Vector2 position = canvasTransformer.Position;
            float scale = canvasTransformer.Scale;
            float controlWidth = canvasTransformer.ControlWidth;
            float controlHeight = canvasTransformer.ControlHeight;

            //Horizontal: Axis-X
            drawingSession.FillRectangle(0, 0, controlWidth, rulerWidth, rulerBackgroundColor);
            drawingSession.DrawLine(0, rulerWidth, controlWidth, rulerWidth, rulerColor);
            //Vertical: Axis-Y
            drawingSession.FillRectangle(0, 0, rulerWidth, controlHeight, rulerBackgroundColor);
            drawingSession.DrawLine(rulerWidth, 0, rulerWidth, controlHeight, rulerColor);

            //Space
            float space = 10 * scale;
            while (space < 10) space *= 5;
            while (space > 100) space /= 5;
            float spaceFive = space * 5;

            //End
            float lineEnd = rulerWidth - rulerLine;
            float thickLineEnd = rulerWidth - rulerThickLine;

            //Horizontal: Lines-X
            for (float X = position.X; X < controlWidth; X += space) drawingSession.DrawLine(X, lineEnd, X, rulerWidth, rulerLineColor);//right
            for (float X = position.X; X > rulerWidth; X -= space) drawingSession.DrawLine(X, lineEnd, X, rulerWidth, rulerLineColor);//left
            //Vertical: Lines-Y
            for (float Y = position.Y; Y < controlHeight; Y += space) drawingSession.DrawLine(lineEnd, Y, rulerWidth, Y, rulerLineColor);//bottom
            for (float Y = position.Y; Y > rulerWidth; Y -= space) drawingSession.DrawLine(lineEnd, Y, rulerWidth, Y, rulerLineColor);//top

            //Horizontal: ThickLine-X
            for (float X = position.X; X < controlWidth; X += spaceFive) drawingSession.DrawLine(X, thickLineEnd, X, rulerWidth, rulerThickLineColor);//right
            for (float X = position.X; X > rulerWidth; X -= spaceFive) drawingSession.DrawLine(X, thickLineEnd, X, rulerWidth, rulerThickLineColor);//left
            //Vertical: ThickLine-Y
            for (float Y = position.Y; Y < controlHeight; Y += spaceFive) drawingSession.DrawLine(thickLineEnd, Y, rulerWidth, Y, rulerThickLineColor);//bottom
            for (float Y = position.Y; Y > rulerWidth; Y -= spaceFive) drawingSession.DrawLine(thickLineEnd, Y, rulerWidth, Y, rulerThickLineColor);//top

            //Horizontal: Text-X
            for (float X = position.X; X < controlWidth; X += spaceFive) drawingSession.DrawText(((int)(Math.Round((X - position.X) / scale))).ToString(), X, lineEnd, textColor, textFormat);
            for (float X = position.X; X > rulerWidth; X -= spaceFive) drawingSession.DrawText(((int)(Math.Round((X - position.X) / scale))).ToString(), X, lineEnd, textColor, textFormat);
            //Vertical: Text-Y
            for (float Y = position.Y; Y < controlHeight; Y += spaceFive) drawingSession.DrawText(((int)(Math.Round((Y - position.Y) / scale))).ToString(), lineEnd, Y, textColor, textFormat);
            for (float Y = position.Y; Y > rulerWidth; Y -= spaceFive) drawingSession.DrawText(((int)(Math.Round((Y - position.Y) / scale))).ToString(), lineEnd, Y, textColor, textFormat);
        }

        /// <summary>
        /// Draw a Ruler.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        public static void DrawRuler(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer) => CanvasDrawingSessionExtensions._drawRuler(drawingSession, canvasTransformer, CanvasDrawingSessionExtensions.RulerWidth, CanvasDrawingSessionExtensions.RulerLine, CanvasDrawingSessionExtensions.RulerThickLine, CanvasDrawingSessionExtensions.RulerBackgroundColor, CanvasDrawingSessionExtensions.RulerColor, CanvasDrawingSessionExtensions.RulerLineColor, CanvasDrawingSessionExtensions.RulerThickLineColor, CanvasDrawingSessionExtensions.TextColor, CanvasDrawingSessionExtensions.TextFormat);

        /// <summary>
        /// Draw a Ruler.
        /// </summary>      
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="rulerWidth"> The width of the ruler. </param>
        /// <param name="rulerLine"> The distance between two lines. </param>
        /// <param name="rulerThickLine"> The distance between two thick lines. </param>
        public static void DrawRuler(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float rulerWidth, float rulerLine, float rulerThickLine) => CanvasDrawingSessionExtensions._drawRuler(drawingSession, canvasTransformer, rulerWidth, rulerLine, rulerThickLine, CanvasDrawingSessionExtensions.RulerBackgroundColor, CanvasDrawingSessionExtensions.RulerColor, CanvasDrawingSessionExtensions.RulerLineColor, CanvasDrawingSessionExtensions.RulerThickLineColor, CanvasDrawingSessionExtensions.TextColor, CanvasDrawingSessionExtensions.TextFormat);

        /// <summary>
        /// Draw a Ruler.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="rulerWidth"> The width of the ruler. </param>
        /// <param name="rulerLine"> The distance between two lines. </param>
        /// <param name="rulerThickLine"> The distance between two thick lines. </param>
        /// <param name="rulerBackgroundColor"> The color of the ruler background color. </param>
        /// <param name="rulerColor"> The color of the ruler. </param>
        /// <param name="rulerLineColor"> The color of the ruler line. </param>
        /// <param name="rulerThickLineColor"> The color of the ruler thick line. </param>
        public static void DrawRuler(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float rulerWidth, float rulerLine, float rulerThickLine, Windows.UI.Color rulerBackgroundColor, Windows.UI.Color rulerColor, Windows.UI.Color rulerLineColor, Windows.UI.Color rulerThickLineColor) => CanvasDrawingSessionExtensions._drawRuler(drawingSession, canvasTransformer, rulerWidth, rulerLine, rulerThickLine, rulerBackgroundColor, rulerColor, rulerLineColor, rulerThickLineColor, CanvasDrawingSessionExtensions.TextColor, CanvasDrawingSessionExtensions.TextFormat);

        /// <summary>
        /// Draw a Ruler.
        /// </summary>
        /// <param name="drawingSession"> The drawing-session. </param>
        /// <param name="canvasTransformer"> The canvas-transformer. </param>
        /// <param name="rulerWidth"> The width of the ruler. </param>
        /// <param name="rulerLine"> The distance between two lines. </param>
        /// <param name="rulerThickLine"> The distance between two thick lines. </param>
        /// <param name="rulerBackgroundColor"> The color of the ruler background color. </param>
        /// <param name="rulerColor"> The color of the ruler. </param>
        /// <param name="rulerLineColor"> The color of the ruler line. </param>
        /// <param name="rulerThickLineColor"> The color of the ruler thick line. </param>
        /// <param name="textColor"> The color of the text. </param>
        /// <param name="textFormat"> The format of the text. </param>
        public static void DrawRuler(this CanvasDrawingSession drawingSession, CanvasTransformer canvasTransformer, float rulerWidth, float rulerLine, float rulerThickLine, Windows.UI.Color rulerBackgroundColor, Windows.UI.Color rulerColor, Windows.UI.Color rulerLineColor, Windows.UI.Color rulerThickLineColor, Windows.UI.Color textColor, CanvasTextFormat textFormat) => CanvasDrawingSessionExtensions._drawRuler(drawingSession, canvasTransformer, rulerWidth, rulerLine, rulerThickLine, rulerBackgroundColor, rulerColor, rulerLineColor, rulerThickLineColor, textColor, textFormat);

    }
}
