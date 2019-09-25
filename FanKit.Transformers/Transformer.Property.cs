﻿using System;
using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary> 
    /// A structure encapsulating four vector values (LeftTop, RightTop, RightBottom, LeftBottom). 
    /// </summary>
    public partial struct Transformer : ITransformerLTRB, ITransformerGeometry
    {
        /// <summary> Vector in LeftTop. </summary>
        public Vector2 LeftTop { get; set; }
        /// <summary> Vector in RightTop. </summary>
        public Vector2 RightTop { get; set; }
        /// <summary> Vector in RightBottom. </summary>
        public Vector2 RightBottom { get; set; }
        /// <summary> Vector in LeftBottom. </summary>
        public Vector2 LeftBottom { get; set; }


        /// <summary> Gets the center vector. </summary>
        public Vector2 Center => (this.LeftTop + this.RightTop + this.RightBottom + this.LeftBottom) / 4;

        /// <summary> Gets the center left vector. </summary>
        public Vector2 CenterLeft => (this.LeftTop + this.LeftBottom) / 2;
        /// <summary> Gets the center top vector. </summary>
        public Vector2 CenterTop => (this.LeftTop + this.RightTop) / 2;
        /// <summary> Gets the center right vector. </summary>
        public Vector2 CenterRight => (this.RightTop + this.RightBottom) / 2;
        /// <summary> Gets the center bottom vector. </summary>
        public Vector2 CenterBottom => (this.RightBottom + this.LeftBottom) / 2;

        /// <summary> Gets the minimum value on the X-Axis. </summary>
        public float MinX => System.Math.Min(System.Math.Min(this.LeftTop.X, this.RightTop.X), System.Math.Min(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the maximum  value on the X-Axis. </summary>
        public float MaxX => System.Math.Max(System.Math.Max(this.LeftTop.X, this.RightTop.X), System.Math.Max(this.RightBottom.X, this.LeftBottom.X));
        /// <summary> Gets the minimum value on the Y-Axis. </summary>
        public float MinY => System.Math.Min(System.Math.Min(this.LeftTop.Y, this.RightTop.Y), System.Math.Min(this.RightBottom.Y, this.LeftBottom.Y));
        /// <summary> Gets the maximum  value on the Y-Axis. </summary>
        public float MaxY => System.Math.Max(System.Math.Max(this.LeftTop.Y, this.RightTop.Y), System.Math.Max(this.RightBottom.Y, this.LeftBottom.Y));

        /// <summary> Gets horizontal vector. </summary>
        public Vector2 Horizontal => (this.RightTop + this.RightBottom - this.LeftTop - this.LeftBottom) / 2;
        /// <summary> Gets vertical vector. </summary>
        public Vector2 Vertical => (this.RightBottom + this.LeftBottom - this.LeftTop - this.RightTop) / 2;
        
    }
}