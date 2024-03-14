using System;
using Unity.Mathematics;
using UnityEngine;

public class Squirrel3
{
    private const uint Noise1 = 0xb5297a4d;
    private const uint Noise2 = 0x68e31da4;
    private const uint Noise3 = 0x1b56c4e9;
    private const uint Cap = uint.MaxValue;
    private readonly int _seed;

    private int _n;

    public Squirrel3(int seed = 0)
    {
        if (seed == 0) seed = Guid.NewGuid().GetHashCode();
        _seed = seed;
    }

    public float Value()
    {
        ++_n;

        return Rnd(_n, _seed) / (float)Cap;
    }

    public double Next()
    {
        return Lerp(0, double.MaxValue, Value());
    }

    public Vector3 Vector3()
    {
        return new Vector3(Value(), Value(), Value());
    }

    public double Range(double min, double max)
    {
        return Lerp(min, max, Value());
    }

    public float Range(float min, float max)
    {
        return (float)Lerp(min, max, Value());
    }

    public int Range(int min, int max)
    {
        return (int)Lerp(min, max, Value());
    }

    public bool Bool()
    {
        return Value() > 0.5f;
    }

    public Quaternion Angle(Vector3 axis)
    {
        return Quaternion.AngleAxis(Range(0f, 360f), axis);
    }

    private static double Lerp(double a, double b, double t)
    {
        return a + (b - a) * math.clamp(t, 0d, 1d);
    }

    private static long Rnd(long n, int seed = 0)
    {
        n *= Noise1;
        n += seed;
        n ^= n >> 8;
        n += Noise2;
        n ^= n << 8;
        n *= Noise3;
        n ^= n >> 8;

        return n % Cap;
    }
}

[Serializable]
public struct MinMaxFloat
{
    public Vector2 MinMax;
    public float x => MinMax.x;
    public float y => MinMax.y;
    public float RandomValue => new Squirrel3().Range(MinMax.x, MinMax.y);
}
