using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathfs
{
    public static Vector3 SmoothByHalfLife(Vector3 a, Vector3 b, float halfLife)
    {
        return b + (a - b) * Exp2(-Time.deltaTime / halfLife);
    }

    public static float SmoothByHalfLife(float a, float b, float halfLife)
    {
        return b + (a - b) * Exp2(-Time.deltaTime / halfLife);
    }

    public static Color SmoothByHalfLife(Color a, Color b, float halfLife)
    {
        return b + (a - b) * Exp2(-Time.deltaTime / halfLife);
    }

    public static float Exp2(float x)
    {
        return Mathf.Pow(2, x);
    }
}
