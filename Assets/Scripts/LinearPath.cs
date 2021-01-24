using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearPath : SliderPath
{
    public LinearPath(System.Numerics.Vector2 start, System.Numerics.Vector2 end, int accuracy, int segments)
    {
        Points = CalculatePoints(start, end, accuracy);
        EvenlySpacedPoints = CalculatePoints(start, end, segments);
    }

    private List<System.Numerics.Vector2> CalculatePoints(System.Numerics.Vector2 start, System.Numerics.Vector2 end, int accuracy)
    {
        List<System.Numerics.Vector2> result = new List<System.Numerics.Vector2>();
        for (int i = 0; i < accuracy; i++)
        {
            result.Add(System.Numerics.Vector2.Lerp(start, end, 1.0f / accuracy * i));
        }
        return result;
    }
}
