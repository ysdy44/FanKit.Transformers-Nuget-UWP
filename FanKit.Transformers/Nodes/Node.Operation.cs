using System.Numerics;

namespace FanKit.Transformers
{
    public partial class Node : ICacheTransform
    {

        /// <summary>
        /// Sharpen.
        /// </summary>
        public void Sharp()
        {
            switch (this.Type)
            {
                case NodeType.BeginFigure:
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


        /// <summary>
        /// Smoothly.
        /// </summary>
        public void Smooth(Vector2 space)
        {
            switch (this.Type)
            {
                case NodeType.BeginFigure:
                case NodeType.Node:
                    {
                        if (this.IsSmooth == false)
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