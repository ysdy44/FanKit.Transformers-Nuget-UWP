using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// The marquee-tool that can make selections that are  rectangular and elliptical.
    /// </summary>
    public class MarqueeTool
    {

        /// <summary> Gets or sets whether the tool in use. </summary>
        public bool IsStarted { get; set; }
        /// <summary> Gets transformer-rect for rectangular or elliptical tool. </summary>
        public TransformerRect TransformerRect { get; private set; }
        /// <summary> Gets points for polygonal or free hand tool. </summary>
        public List<Vector2> Points { get; private set; } = new List<Vector2>();


        /// <summary>
        /// Started using marquee-tool.
        /// </summary>
        /// <param name="point"> The point. </param>
        /// <param name="toolType"> The tool type.</param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isSquare"> Equal in width and height. </param>
        public void Start(Vector2 point, MarqueeToolType toolType, bool isCenter, bool isSquare)
        {
            switch (toolType)
            {
                case MarqueeToolType.Rectangular:
                    this.IsStarted = true;
                    this.TransformerRect = new TransformerRect(point, point, isCenter, isSquare);
                    break;
                case MarqueeToolType.Elliptical:
                    this.IsStarted = true;
                    this.TransformerRect = new TransformerRect(point, point, isCenter, isSquare);
                    break;
                case MarqueeToolType.Polygonal:
                    if (this.IsStarted == false)
                    {
                        this.IsStarted = true;
                        this.Points.Clear();
                    }
                    this.Points.Add(point);
                    break;
                case MarqueeToolType.FreeHand:
                    this.IsStarted = true;
                    this.Points.Clear();
                    this.Points.Add(point);
                    break;
            }
        }


        /// <summary>
        /// In use marquee-tool.
        /// </summary>
        /// <param name="startingPoint"> The starting point. </param>
        /// <param name="point"> The point. </param>
        /// <param name="toolType"> The tool type.</param>
        /// <param name="isCenter"> Scaling around the center. </param>
        /// <param name="isSquare"> Equal in width and height. </param>
        public void Delta(Vector2 startingPoint, Vector2 point, MarqueeToolType toolType, bool isCenter, bool isSquare)
        {
            switch (toolType)
            {
                case MarqueeToolType.Rectangular:
                    this.TransformerRect = new TransformerRect(startingPoint, point, isCenter, isSquare);
                    break;
                case MarqueeToolType.Elliptical:
                    this.TransformerRect = new TransformerRect(startingPoint, point, isCenter, isSquare);
                    break;
                case MarqueeToolType.Polygonal:
                    if (this.Points.Count == 1)
                        this.Points.Add(point);
                    else
                        this.Points[this.Points.Count - 1] = point;
                    break;
                case MarqueeToolType.FreeHand:
                    this.Points.Add(point);
                    break;
            }
        }


        /// <summary>
        /// End up using the marquee-tool.
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="point"></param>
        /// <param name="toolType"></param>
        /// <param name="isCenter"></param>
        /// <param name="isSquare"></param>
        /// <returns> Return **true** if the marquee mask need to redraw, otherwise **false**. </returns>
        public bool Complete(Vector2 startingPoint, Vector2 point, MarqueeToolType toolType, bool isCenter = false, bool isSquare = false)
        {
            switch (toolType)
            {
                case MarqueeToolType.Rectangular:
                    this.IsStarted = false;
                    this.TransformerRect = new TransformerRect(startingPoint, point, isCenter, isSquare);
                    return true;
                case MarqueeToolType.Elliptical:
                    this.IsStarted = false;
                    this.TransformerRect = new TransformerRect(startingPoint, point, isCenter, isSquare);
                    return true;
                case MarqueeToolType.Polygonal:
                    Vector2 firstPoint = this.Points.First();
                    if (this.Points.Count > 2 && Math.InNodeRadius(firstPoint, point))
                    {
                        this.IsStarted = false;
                        this.Points.Add(point);
                        return true;
                    }
                    else
                    {
                        this.Points[this.Points.Count - 1] = point;
                        return false;
                    }
                case MarqueeToolType.FreeHand:
                    this.IsStarted = false;
                    this.Points.Add(point);
                    return true;
            }
            return false;
        }
        /// <summary>
        /// End up using the marquee-tool.
        /// </summary>
        /// <param name="startingPoint"></param>
        /// <param name="point"></param>
        /// <param name="toolType"></param>
        /// <param name="matrix"></param>
        /// <param name="isCenter"></param>
        /// <param name="isSquare"></param>
        /// <returns> Return **true** if the marquee mask need to redraw, otherwise **false**. </returns>
        public bool Complete(Vector2 startingPoint, Vector2 point, MarqueeToolType toolType, Matrix3x2 matrix, bool isCenter = false, bool isSquare = false)
        {
            switch (toolType)
            {
                case MarqueeToolType.Rectangular:
                    this.IsStarted = false;
                    this.TransformerRect = new TransformerRect(startingPoint, point, isCenter, isSquare);
                    return true;
                case MarqueeToolType.Elliptical:
                    this.IsStarted = false;
                    this.TransformerRect = new TransformerRect(startingPoint, point, isCenter, isSquare);
                    return true;
                case MarqueeToolType.Polygonal:
                    Vector2 firstPoint = Vector2.Transform(this.Points.First(), matrix);
                    if (this.Points.Count > 2 && Math.InNodeRadius(firstPoint, point))
                    {
                        this.IsStarted = false;
                        this.Points.Add(point);
                        return true;
                    }
                    else
                    {
                        this.Points[this.Points.Count - 1] = point;
                        return false;
                    }
                case MarqueeToolType.FreeHand:
                    this.IsStarted = false;
                    this.Points.Add(point);
                    return true;
            }
            return false;
        }


    }
}