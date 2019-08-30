﻿using System.Numerics;

namespace FanKit.Transformers
{
    /// <summary>
    /// Mode of restriction by self control point.
    /// </summary>
    public enum SelfControlPointMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> No change the angle. </summary>
        Angle,
        /// <summary> No change the length. </summary>
        Length,
        /// <summary> Disable control point. </summary>
        Disable,
    }

    /// <summary>
    /// Mode of length by each control point.
    /// </summary>
    public enum EachControlPointLengthMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Equal length. </summary>
        Equal,
        /// <summary> Ratio length. </summary>
        Ratio,
    }
    /// <summary>
    /// Mode of angle by each control point.
    /// </summary>
    public enum EachControlPointAngleMode
    {
        /// <summary> Normal. </summary>
        None,
        /// <summary> Origin symmetry. </summary>
        Asymmetric,
        /// <summary> Fixe angle. </summary>
        Fixed,
    }
    
    /// <summary>  
    /// Nodes of the Bezier Curve.
    /// </summary>
    public partial struct Node : ICacheTransform
    {
        //@Static
        /// <summary>
        /// It controls the transformation of node contol point.
        /// </summary>
        /// <param name="mode"> The mode. </param>
        /// <param name="lengthMode"> The length mode. </param>
        /// <param name="angleMode"> The angle mode. </param>
        /// <param name="point"> The point. </param>
        /// <param name="startingNode"> The starting node. </param>
        /// <param name="isLeftControlPoint"> <see cref="Node.LeftControlPoint"/> or <see cref="Node.RightControlPoint"/>. </param>
        /// <returns> The controlled node. </returns>
        public static Node Controller(SelfControlPointMode mode, EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 point, Node startingNode, bool isLeftControlPoint = true)
        {
            Vector2 startingPoint = startingNode.Point;

            Vector2 startingSelfControlPoint = isLeftControlPoint ? startingNode.LeftControlPoint : startingNode.RightControlPoint;
            Vector2 startingEachControlPoint = isLeftControlPoint ? startingNode.RightControlPoint : startingNode.LeftControlPoint;

            Vector2 selfControlPoint = Node._getSelfControlPoint(mode, point, startingPoint, startingSelfControlPoint);
            Vector2 eachControlPoint = Node._getEachControlPoint(lengthMode, angleMode, startingPoint, selfControlPoint - startingPoint, startingSelfControlPoint - startingPoint, startingEachControlPoint - startingPoint);

            return new Node
            {
                Point = startingNode.Point,
                LeftControlPoint = isLeftControlPoint ? selfControlPoint : eachControlPoint,
                RightControlPoint = isLeftControlPoint ? eachControlPoint : selfControlPoint,
                IsChecked = startingNode.IsChecked,
                IsSmooth = startingNode.IsSmooth
            };
        }
        
        //Self
        private static Vector2 _getSelfControlPoint(SelfControlPointMode mode, Vector2 point, Vector2 startingPoint, Vector2 startingSelfControlPoint)
        {
            switch (mode)
            {
                case SelfControlPointMode.None: return point;
                case SelfControlPointMode.Angle:
                    {
                        Vector2 selfControlPoint = Math.FootPoint(point, startingPoint, startingSelfControlPoint);
                        return selfControlPoint;
                    }
                case SelfControlPointMode.Length:
                    {
                        Vector2 startingSelfVector  = startingSelfControlPoint - startingPoint;
                        Vector2 selfVector = point - startingPoint;

                        float startingSelfLength = startingSelfVector .Length();
                        float selfLength = selfVector.Length();

                        //Vector2 startingSelfUnit = startingSelfVector  / startingSelfLength;
                        Vector2 selfUnit = selfVector / selfLength;

                        Vector2 selfControlPoint = startingPoint + startingSelfLength * selfUnit;
                        return selfControlPoint;
                    }
            }
            return point;
        }
        
        //Each
        private static Vector2 _getEachControlPoint(EachControlPointLengthMode lengthMode, EachControlPointAngleMode angleMode, Vector2 startingPoint, Vector2 selfVector, Vector2 startingSelfVector, Vector2 startingEachVector)
        {
            float selfLength = selfVector.Length();
            float startingSelfLength = startingSelfVector.Length();
            float startingEachLength = startingEachVector.Length();

            float eachLength = Node._getEachLength(lengthMode, selfLength, startingSelfLength, startingEachLength);
            Vector2 eachUnit = Node._getEachUnit(angleMode, selfVector / selfLength, startingSelfVector / selfLength, startingEachVector / startingEachLength);

            Vector2 eachControlPoint = startingPoint + eachLength * eachUnit;
            return eachControlPoint;
        }
        private static float _getEachLength(EachControlPointLengthMode lengthMode, float selfLength, float startingSelfLength, float startingEachLength)
        {
            switch (lengthMode)
            {
                case EachControlPointLengthMode.None: return startingEachLength;
                case EachControlPointLengthMode.Equal: return selfLength;
                case EachControlPointLengthMode.Ratio: return selfLength / startingSelfLength * startingEachLength;
            }
            return 0;
        }
        private static Vector2 _getEachUnit(EachControlPointAngleMode angleMode, Vector2 selfUnit, Vector2 startingSelfUnit, Vector2 startingEachUnit)
        {
            switch (angleMode)
            {
                case EachControlPointAngleMode.None: return startingEachUnit;
                case EachControlPointAngleMode.Asymmetric: return -selfUnit;
                case EachControlPointAngleMode.Fixed:
                    {
                        float selfRadians = Math.VectorToRadians(selfUnit);
                        float startingSelfRadians = Math.VectorToRadians(startingSelfUnit);
                        float startingEachRadians = Math.VectorToRadians(startingEachUnit);

                        float radians = selfRadians - startingSelfRadians + startingEachRadians;
                        return new Vector2((float)System.Math.Cos(radians), (float)System.Math.Sin(radians));
                    }
            }
            return Vector2.Zero;
        }
        
    }
}
