using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial class Node : ICacheTransform
    {

        /// <summary>
        /// Sharpen.
        /// </summary>
        public void Sharp()
        {
            if (this.IsChecked)
            {
                switch (this.Type)
                {
                    case NodeType.Node:
                        {
                            if (this.IsSmooth)
                            {
                                this.LeftControlPoint = this.Point;
                                this.RightControlPoint = this.Point;
                                this.IsSmooth = false;
                            }
                        }
                        break;
                }
            }
        }


        /// <summary>
        /// Smoothly.
        /// </summary>
        public void Smooth(Vector2 space)
        {
            if (this.IsChecked)
            {
                switch (this.Type)
                {
                    case NodeType.Node:
                        {
                            if (this.IsSmooth==false)
                            {
                                this.LeftControlPoint = this.Point - space;
                                this.RightControlPoint = this.Point + space;
                                this.IsSmooth = true;
                            }
                        }
                        break;
                }
            }
        }

    }
}