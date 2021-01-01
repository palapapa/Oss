﻿using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class Bezier
{
    public List<System.Numerics.Vector2> ControlPoints { get; private set; } = new List<System.Numerics.Vector2>();
    public List<System.Numerics.Vector2> Points { get; private set; } = new List<System.Numerics.Vector2>();
    /// <summary>
    /// This property has no elements until <see cref="CalculateEvenlySpacedPoints(int)"/> is called
    /// </summary>
    public List<System.Numerics.Vector2> EvenlySpacedPoints { get; private set; } = new List<System.Numerics.Vector2>();
    public float Length { get; private set; }
    private int segments;
    /// <summary>
    /// Correspond each interpolation value to a ratio(0 ~ 1) of the total length of the curve
    /// </summary>
    private Dictionary<float, float> interpolationToSegment = new Dictionary<float, float>();
    public Bezier(List<System.Numerics.Vector2> controlPoints, int accuracy)
    {
        ControlPoints = controlPoints;
        RecalculateCurve(controlPoints, accuracy);
    }
    
    private System.Numerics.Vector2 GetPointByInterpolationIndependent(List<System.Numerics.Vector2> controlPoints, float interpolation)
    {
        if (interpolation < 0 || interpolation > 1)
        {
            throw new ArgumentException("\'interpolation\' value can only range from 0 to 1");
        }
        if (controlPoints.Count == 0)
        {
            throw new ArgumentException("\'controlPoints\' doesn't contain any points");
        }
        if (controlPoints.Count == 1)
        {
            return controlPoints[0];
        }
        else
        {
            System.Numerics.Vector2 p1 = GetPointByInterpolationIndependent(controlPoints.GetRange(0, controlPoints.Count - 1), interpolation);
            System.Numerics.Vector2 p2 = GetPointByInterpolationIndependent(controlPoints.GetRange(1, controlPoints.Count - 1), interpolation);
            return System.Numerics.Vector2.Lerp(p1, p2, interpolation);
        }
    }

    private void CalculatePoints(int accuracy)
    {
        if (accuracy <= 0)
        {
            throw new ArgumentException("\'accuracy\' must be larger than 0");
        }
        for (int i = 0; i < accuracy; i++)
        {
            Points.Add(GetPointByInterpolation(1 / (float)accuracy * i));
        }
    }

    private void CalculateLength(int accuracy)
    {
        if (accuracy <= 0)
        {
            throw new ArgumentException("\'accuracy\' must be larger than 0");
        }
        interpolationToSegment.Clear();
        interpolationToSegment.Add(0, 0);
        System.Numerics.Vector2 currentPoint, nextPoint;
        float result = 0;
        for (int i = 0; i < accuracy; i++)
        {
            currentPoint = GetPointByInterpolation(1 / (float)accuracy * i);
            nextPoint = GetPointByInterpolation(1 / (float)accuracy * (i + 1));
            result += System.Numerics.Vector2.Distance(currentPoint, nextPoint);
            try
            {
                interpolationToSegment.Add(1 / (float)accuracy * (i + 1), result);
            }
            catch (ArgumentException)
            {

            }
        }
        Length = result;
        interpolationToSegment.Remove(interpolationToSegment.Keys.ElementAt(interpolationToSegment.Count - 1)); // remove last pair
        interpolationToSegment.Add(1, result);
        return;
    }

    /// <summary>
    /// Get a point on the curve using interpolation
    /// </summary>
    /// <param name="interpolation">A value from 0 to 1</param>
    /// <returns></returns>
    public System.Numerics.Vector2 GetPointByInterpolation(float interpolation)
    {
        if (interpolation < 0 || interpolation > 1)
        {
            throw new ArgumentException("\'interpolation\' value can only range from 0 to 1");
        }
        if (ControlPoints.Count == 0)
        {
            throw new ArgumentException("\'controlPoints\' doesn't contain any points");
        }
        if (ControlPoints.Count == 1)
        {
            return ControlPoints[0];
        }
        else
        {
            System.Numerics.Vector2 p1 = GetPointByInterpolationIndependent(ControlPoints.GetRange(0, ControlPoints.Count - 1), interpolation);
            System.Numerics.Vector2 p2 = GetPointByInterpolationIndependent(ControlPoints.GetRange(1, ControlPoints.Count - 1), interpolation);
            return System.Numerics.Vector2.Lerp(p1, p2, interpolation);
        }
    }

    /// <summary>
    /// Calculate <paramref name="segments"/> number of evenly spaced points on the curve, then get the <paramref name="pointIndex"/>th point. Does not update propreties.
    /// </summary>
    /// <param name="segments">The number of evenly spaced points(including start and end) to be calculated</param>
    /// <param name="pointIndex">The index of the calculated point to return</param>
    public System.Numerics.Vector2 GetPointBySegment(int segments, int pointIndex)
    {
        if (segments < 2)
        {
            throw new ArgumentException("\'segments\' cannot be less than 2");
        }
        if (pointIndex > segments - 1 || pointIndex < 0)
        {
            throw new ArgumentOutOfRangeException("\'pointIndex\' out of range");
        }
        float unitLength = Length / segments;
        for (int i = 0; i < segments; i++)
        {
            float targetInterpolation = 0;
            for (int j = 0; j < interpolationToSegment.Count; j++)
            {
                if (interpolationToSegment.ElementAt(j).Value == unitLength * i)
                {
                    targetInterpolation = interpolationToSegment.ElementAt(j).Key;
                    break;
                }
                else if (interpolationToSegment.ElementAt(j).Value > unitLength * i)
                {
                    targetInterpolation = Mathf.Lerp(interpolationToSegment.ElementAt(j - 1).Key,
                        interpolationToSegment.ElementAt(j).Key,
                        (unitLength * i - interpolationToSegment.ElementAt(j - 1).Value) / (interpolationToSegment.ElementAt(j).Value - interpolationToSegment.ElementAt(j - 1).Value));
                    break;
                }
            }
            if (i == pointIndex)
            {
                return GetPointByInterpolation(targetInterpolation);
            }
        }
        throw new NotImplementedException("This should not happen! If this exception is thrown then there is a bug in this method");
    }

    /// <summary>
    /// Calculate <paramref name="segments"/> number of evenly spaced points on the curve and updates <see cref="EvenlySpacedPoints"/>
    /// <br/>
    /// <paramref name="segments"/> will be cached and will be used to recalculate <see cref="EvenlySpacedPoints"/> when <see cref="UpdateAccuracy(int)"/> or <see cref="RecalculateCurve(List{System.Numerics.Vector2}, int)"/> is called
    /// </summary>
    /// <param name="segments">The number of evenly spaced points(including start and end) to be calculated</param>
    public void CalculateEvenlySpacedPoints(int segments)
    {
        if (segments < 2)
        {
            EvenlySpacedPoints.Clear();
            return;
        }
        EvenlySpacedPoints.Clear();
        this.segments = segments;
        float unitLength = Length / segments;
        for (int i = 0; i < segments; i++)
        {
            float targetInterpolation = 0;
            for (int j = 0; j < interpolationToSegment.Count; j++)
            {
                if (interpolationToSegment.ElementAt(j).Value == unitLength * i)
                {
                    targetInterpolation = interpolationToSegment.ElementAt(j).Key;
                    break;
                }
                else if (interpolationToSegment.ElementAt(j).Value > unitLength * i)
                {
                    targetInterpolation = Mathf.Lerp(interpolationToSegment.ElementAt(j - 1).Key,
                        interpolationToSegment.ElementAt(j).Key,
                        (unitLength * i - interpolationToSegment.ElementAt(j - 1).Value) / (interpolationToSegment.ElementAt(j).Value - interpolationToSegment.ElementAt(j - 1).Value));
                    break;
                }
            }
            EvenlySpacedPoints.Add(GetPointByInterpolation(targetInterpolation));
        }
    }

    /// <summary>
    /// Recalculate the curve using the new resolution and update <see cref="ControlPoints"/>, <see cref="Points"/>, and <see cref="EvenlySpacedPoints"/>
    /// </summary>
    /// <param name="accuracy">The number of points on the curve to be calculated including start and end</param>
    public void UpdateAccuracy(int accuracy)
    {
        if (accuracy <= 0)
        {
            throw new ArgumentException("\'accuracy\' must be larger than 0");
        }
        Points.Clear();
        EvenlySpacedPoints.Clear();
        CalculatePoints(accuracy);
        CalculateEvenlySpacedPoints(segments);
        CalculateLength(accuracy);
    }

    /// <summary>
    /// Recalculate the curve and update <see cref="ControlPoints"/>, <see cref="Points"/>, and <see cref="EvenlySpacedPoints"/>
    /// </summary>
    /// <param name="controlPoints">The control points defining the curve</param>
    /// <param name="accuracy">The number of points on the curve to be calculated including start and end</param>
    public void RecalculateCurve(List<System.Numerics.Vector2> controlPoints, int accuracy)
    {
        if (accuracy <= 0)
        {
            throw new ArgumentException("\'accuracy\' must be larger than 0");
        }
        ControlPoints = controlPoints;
        Points.Clear();
        EvenlySpacedPoints.Clear();
        CalculatePoints(accuracy);
        CalculateEvenlySpacedPoints(segments);
        CalculateLength(accuracy);
    }
}
