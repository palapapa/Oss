using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SliderPath
{
    /// <summary>
    /// Points on the path.
    /// </summary>
    public List<System.Numerics.Vector2> Points { get; protected set; } = new List<System.Numerics.Vector2>();
    /// <summary>
    /// Evenly spaced points on the path.
    /// <br/>
    /// This property has no elements until <see cref="CalculateEvenlySpacedPoints(int)"/> is called.
    /// </summary>
    public List<System.Numerics.Vector2> EvenlySpacedPoints { get; protected set; } = new List<System.Numerics.Vector2>();
    public abstract void CalculateEvenlySpacedPoints(int segments);
}
