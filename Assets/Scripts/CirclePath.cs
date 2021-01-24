using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class CirclePath : SliderPath
{
    private System.Numerics.Vector2 start;
    private System.Numerics.Vector2 middle;
    private System.Numerics.Vector2 end;

    public CirclePath(System.Numerics.Vector2 start, System.Numerics.Vector2 middle, System.Numerics.Vector2 end, int accuracy, int segments)
    {
        this.start = start;
        this.middle = middle;
        this.end = end;
        Points = CalculatePoints(start, middle, end, accuracy);
        CalculateEvenlySpacedPoints(segments);
    }

    protected void CalculateEvenlySpacedPoints(int segments)
    {
        EvenlySpacedPoints.Clear();
        EvenlySpacedPoints = CalculatePoints(start, middle, end, segments);
    }

    private List<System.Numerics.Vector2> CalculatePoints(System.Numerics.Vector2 start, System.Numerics.Vector2 middle, System.Numerics.Vector2 end, int accuracy)
    {
        List<System.Numerics.Vector2> result = new List<System.Numerics.Vector2>();
        if (Utilities.IsCollinear(start, middle, end))
        {
            result.Add(start);
            result.Add(middle);
        }
        else
        {
            System.Numerics.Vector2 center = Utilities.CalculateCircleCenter(start, middle, end);
            // Debug.Log($"{center:F6}, r={radius:F6}, A={Utilities.OsuPixelToScreenPoint(slider.SliderPoints[0]):F6}, B={Utilities.OsuPixelToScreenPoint(slider.SliderPoints[1]):F6}, C={Utilities.OsuPixelToScreenPoint(slider.SliderPoints[2]):F6}");
            System.Numerics.Vector2 OA = new System.Numerics.Vector2(start.X - center.X, start.Y - center.Y);
            System.Numerics.Vector2 OB = new System.Numerics.Vector2(middle.X - center.X, middle.Y - center.Y);
            System.Numerics.Vector2 OC = new System.Numerics.Vector2(end.X - center.X, end.Y - center.Y);
            float radius = OA.Length();
            float angleCcwOaOb = Utilities.CalculateOrientedAngle(OA, OB);
            if (angleCcwOaOb < 0)
            {
                angleCcwOaOb += 2 * (float)Math.PI;
            }
            float angleCcwOaOc = Utilities.CalculateOrientedAngle(OA, OC);
            if (angleCcwOaOc < 0)
            {
                angleCcwOaOc += 2 * (float)Math.PI;
            }
            if (angleCcwOaOb > angleCcwOaOc)
            {
                angleCcwOaOc -= 2 * (float)Math.PI;
            }
            float segmentAngle = angleCcwOaOc / accuracy;
            float currentAngle = Mathf.Atan2(OA.Y, OA.X);
            for (int i = 0; i < accuracy; i++)
            {
                result.Add(new System.Numerics.Vector2(center.X + radius * Mathf.Cos(currentAngle), center.Y + radius * Mathf.Sin(currentAngle)));
                currentAngle += segmentAngle;
            }
        }
        return result;
    }
}
