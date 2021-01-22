using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public class BezierPath : SliderPath
{
    public List<System.Numerics.Vector2> ControlPoints { get; private set; } = new List<System.Numerics.Vector2>();
    public float Length { get; private set; }
    private int segments;
    /// <summary>
    /// Correspond each interpolation value to a ratio(0 ~ 1) of the total length of the curve.
    /// </summary>
    private Dictionary<float, float> interpolationToSegment = new Dictionary<float, float>();
    /// <summary>
    /// Generate a bezier curve.
    /// </summary>
    /// <param name="controlPoints">The control points used to generate the curve.</param>
    /// <param name="accuracy">The number of points to calculate.</param>
    /// <param name="segments">The number of evenly spaced points to calculate.</param>
    public BezierPath(List<System.Numerics.Vector2> controlPoints, int accuracy, int segments)
    {
        ControlPoints = controlPoints;
        CalculateCurve(controlPoints, accuracy);
        CalculateEvenlySpacedPoints(segments);
    }
    
    private System.Numerics.Vector2 GetPointByInterpolationIndependent(List<System.Numerics.Vector2> controlPoints, float interpolation)
    {
        if (interpolation < 0 || interpolation > 1)
        {
            throw new ArgumentException("Value can only range from 0 to 1", nameof(interpolation));
        }
        if (controlPoints.Count == 0)
        {
            throw new ArgumentException("Doesn't contain any points", nameof(controlPoints));
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
            throw new ArgumentException("Must be larger than 0", nameof(accuracy));
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
            throw new ArgumentException("Must be larger than 0", nameof(accuracy));
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
            interpolationToSegment.Add(1 / (float)accuracy * (i + 1), result);
        }
        Length = result;
        interpolationToSegment.Remove(interpolationToSegment.Keys.ElementAt(interpolationToSegment.Count - 1)); // remove last pair
        interpolationToSegment.Add(1, result);
        return;
    }

    /// <summary>
    /// Get a point on the curve using interpolation.
    /// </summary>
    /// <param name="interpolation">A value from 0 to 1.</param>
    /// <returns></returns>
    public System.Numerics.Vector2 GetPointByInterpolation(float interpolation)
    {
        if (interpolation < 0 || interpolation > 1)
        {
            throw new ArgumentException("Value can only range from 0 to 1", nameof(interpolation));
        }
        if (ControlPoints.Count == 0)
        {
            throw new ArgumentException("Doesn't contain any points", nameof(ControlPoints));
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
    /// <param name="segments">The number of evenly spaced points(including start and end) to be calculated.</param>
    /// <param name="pointIndex">The index of the calculated point to return.</param>
    public System.Numerics.Vector2 GetPointBySegment(int segments, int pointIndex)
    {
        if (segments < 2)
        {
            throw new ArgumentException("Cannot be less than 2", nameof(segments));
        }
        if (pointIndex > segments - 1 || pointIndex < 0)
        {
            throw new ArgumentOutOfRangeException("Out of range", nameof(pointIndex));
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
    /// Calculate <paramref name="segments"/> number of evenly spaced points on the curve and updates <see cref="EvenlySpacedPoints"/>.
    /// <br/>
    /// <paramref name="segments"/> will be cached and will be used to recalculate <see cref="EvenlySpacedPoints"/> when <see cref="UpdateAccuracy(int)"/> or <see cref="CalculateCurve(List{System.Numerics.Vector2}, int)"/> is called.
    /// </summary>
    /// <param name="segments">The number of evenly spaced points(including start and end) to be calculated.</param>
    protected override void CalculateEvenlySpacedPoints(int segments)
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
    /// Recalculate the curve using the new resolution and update <see cref="ControlPoints"/>, <see cref="Points"/>, and <see cref="EvenlySpacedPoints"/>.
    /// </summary>
    /// <param name="accuracy">The number of points on the curve to be calculated including start and end.</param>
    private void UpdateAccuracy(int accuracy)
    {
        if (accuracy <= 0)
        {
            throw new ArgumentException("Must be larger than 0", nameof(accuracy));
        }
        Points.Clear();
        EvenlySpacedPoints.Clear();
        CalculatePoints(accuracy);
        CalculateEvenlySpacedPoints(segments);
        CalculateLength(accuracy);
    }

    /// <summary>
    /// Recalculate the curve and update <see cref="ControlPoints"/>, <see cref="Points"/>, and <see cref="EvenlySpacedPoints"/>.
    /// </summary>
    /// <param name="controlPoints">The control points defining the curve.</param>
    /// <param name="accuracy">The number of points on the curve to be calculated including start and end.</param>
    private void CalculateCurve(List<System.Numerics.Vector2> controlPoints, int accuracy)
    {
        if (accuracy <= 0)
        {
            throw new ArgumentException("Must be larger than 0", nameof(accuracy));
        }
        ControlPoints = controlPoints;
        Points.Clear();
        EvenlySpacedPoints.Clear();
        CalculatePoints(accuracy);
        CalculateEvenlySpacedPoints(segments);
        CalculateLength(accuracy);
    }
}
