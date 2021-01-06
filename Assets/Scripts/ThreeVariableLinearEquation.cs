using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class repersenting an equation in the form of <see cref="XCoefficient"/> * x + <see cref="YCoefficient"/> * y + <see cref="ZCoefficient"/> * z = answer
/// </summary>
public class ThreeVariableLinearEquation
{
    public float XCoefficient;
    public float YCoefficient;
    public float ZCoefficient;
    public float Answer;

    public ThreeVariableLinearEquation(float a, float b, float c, float answer)
    {
        XCoefficient = a;
        YCoefficient = b;
        ZCoefficient = c;
        Answer = answer;
    }
}