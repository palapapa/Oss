using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matrix3x3
{
    public float A { get; set; }
    public float B { get; set; }
    public float C { get; set; }
    public float D { get; set; }
    public float E { get; set; }
    public float F { get; set; }
    public float G { get; set; }
    public float H { get; set; }
    public float I { get; set; }

    public Matrix3x3
    (
        float a,
        float b,
        float c,
        float d,
        float e,
        float f,
        float g,
        float h,
        float i
    )
    {
        A = a;
        B = b;
        C = c;
        D = d;
        E = e;
        F = f;
        G = g;
        H = h;
        I = i;
    }

    public float CalculateMatrix()
    {
        return A * E * I +
            B * F * G +
            C * D * H -
            C * E * G -
            B * D * I -
            A * F * H;
    }
}