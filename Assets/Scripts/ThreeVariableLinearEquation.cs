using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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