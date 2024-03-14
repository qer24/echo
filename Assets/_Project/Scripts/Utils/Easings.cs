using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Easings
{
    public enum EaseType{
        Linear, EaseOutQuad, EaseInQuad, EaseInOutQuad, EaseInCubic, EaseOutCubic, EaseInOutCubic, EaseInQuart, EaseOutQuart, EaseInOutQuart,
        EaseInQuint, EaseOutQuint, EaseInOutQuint, EaseInSine, EaseOutSine, EaseInOutSine, EaseInExpo, EaseOutExpo, EaseInOutExpo, EaseInCirc, EaseOutCirc, EaseInOutCirc,
        EaseInBounce, EaseOutBounce, EaseInOutBounce, EaseInBack, EaseOutBack, EaseInOutBack, EaseInElastic, EaseOutElastic, EaseInOutElastic, EaseSpring, EaseShake, Punch, Once, Clamp, PingPong
    }

    public static float EaseLinear(float val)
    {
        return val;
    }

    public static float EaseOutQuad(float val)
    {
        return -1 * val * (val - 2);
    }

    public static float EaseInQuad(float val)
    {
        return val * val;
    }

    public static float EaseInOutQuad(float val)
    {
        val /= 0.5f;
        if (val < 1) return 0.5f * val * val;
        val--;
        return -0.5f * (val * (val - 2) - 1);
    }

    public static float EaseInCubic(float val)
    {
        return val * val * val;
    }

    public static float EaseOutCubic(float val)
    {
        val--;
        return (val * val * val + 1);
    }

    public static float EaseInOutCubic(float val)
    {
        val /= 0.5f;
        if (val < 1) return 0.5f * val * val * val;
        val -= 2;
        return 0.5f * (val * val * val + 2);
    }

    public static float EaseInQuart(float val)
    {
        return val * val * val * val;
    }

    public static float EaseOutQuart(float val)
    {
        val--;
        return -(val * val * val * val - 1);
    }

    public static float EaseInOutQuart(float val)
    {
        val /= 0.5f;
        if (val < 1) return 0.5f * val * val * val * val;
        val -= 2;
        return -0.5f * (val * val * val * val - 2);
    }

    public static float EaseInQuint(float val)
    {
        return val * val * val * val * val;
    }

    public static float EaseOutQuint(float val)
    {
        val--;
        return (val * val * val * val * val + 1);
    }

    public static float EaseInOutQuint(float val)
    {
        val /= 0.5f;
        if (val < 1) return 0.5f * val * val * val * val * val;
        val -= 2;
        return 0.5f * (val * val * val * val * val + 2);
    }

    public static float EaseInSine(float val)
    {
        return -1 * Mathf.Cos(val * (Mathf.PI / 2)) + 1;
    }

    public static float EaseOutSine(float val)
    {
        return Mathf.Sin(val * (Mathf.PI / 2));
    }

    public static float EaseInOutSine(float val)
    {
        return -0.5f * (Mathf.Cos(Mathf.PI * val) - 1);
    }

    public static float EaseInExpo(float val)
    {
        return (val == 0) ? 0 : Mathf.Pow(2, 10 * (val - 1));
    }

    public static float EaseOutExpo(float val)
    {
        return (val == 1) ? 1 : -Mathf.Pow(2, -10 * val) + 1;
    }

    public static float EaseInOutExpo(float val)
    {
        val /= 0.5f;
        if (val < 1) return 0.5f * Mathf.Pow(2, 10 * (val - 1));
        val--;
        return 0.5f * (-Mathf.Pow(2, -10 * val) + 2);
    }

    public static float EaseInCirc(float val)
    {
        return -1 * (Mathf.Sqrt(1 - val * val) - 1);
    }

    public static float EaseOutCirc(float val)
    {
        val--;
        return Mathf.Sqrt(1 - val * val);
    }

    public static float EaseInOutCirc(float val)
    {
        val /= 0.5f;
        if (val < 1) return -0.5f * (Mathf.Sqrt(1 - val * val) - 1);
        val -= 2;
        return 0.5f * (Mathf.Sqrt(1 - val * val) + 1);
    }

    public static float EaseInBounce(float val)
    {
        return 1 - EaseOutBounce(1 - val);
    }

    public static float EaseOutBounce(float val)
    {
        if (val < (1 / 2.75f))
        {
            return (7.5625f * val * val);
        }
        else if (val < (2 / 2.75f))
        {
            val -= (1.5f / 2.75f);
            return (7.5625f * (val) * val + .75f);
        }
        else if (val < (2.5 / 2.75))
        {
            val -= (2.25f / 2.75f);
            return (7.5625f * (val) * val + .9375f);
        }
        else
        {
            val -= (2.625f / 2.75f);
            return (7.5625f * (val) * val + .984375f);
        }
    }

    public static float EaseInOutBounce(float val)
    {
        if (val < 0.5f) return EaseInBounce(val * 2) * 0.5f;
        return EaseOutBounce(val * 2 - 1) * 0.5f + 0.5f;
    }

    public static float EaseInBack(float val)
    {
        float s = 1.70158f;
        return val * val * ((s + 1) * val - s);
    }

    public static float EaseOutBack(float val)
    {
        float s = 1.70158f;
        val--;
        return (val) * val * ((s + 1) * val + s) + 1;
    }

    public static float EaseInOutBack(float val)
    {
        float s = 1.70158f;
        val /= 0.5f;
        if ((val) < 1)
        {
            s *= (1.525f);
            return 0.5f * (val * val * (((s) + 1) * val - s));
        }
        val -= 2;
        s *= (1.525f);
        return 0.5f * ((val) * val * (((s) + 1) * val + s) + 2);
    }

    public static float EaseInElastic(float val)
    {
        float p = 0.3f;
        float s = p / 4;
        return -(Mathf.Pow(2, 10 * (val -= 1)) * Mathf.Sin((val - s) * (2 * Mathf.PI) / p));
    }

    public static float EaseOutElastic(float val)
    {
        float p = 0.3f;
        float s = p / 4;
        return (Mathf.Pow(2, -10 * val) * Mathf.Sin((val - s) * (2 * Mathf.PI) / p) + 1);
    }

    public static float EaseInOutElastic(float val)
    {
        float p = 0.45f;
        float s = p / 4;
        val /= 0.5f;
        if (val < 1) return -0.5f * (Mathf.Pow(2, 10 * (val -= 1)) * Mathf.Sin((val - s) * (2 * Mathf.PI) / p));
        return Mathf.Pow(2, -10 * (val -= 1)) * Mathf.Sin((val - s) * (2 * Mathf.PI) / p) * 0.5f + 1;
    }

    public static float EaseSpring(float val)
    {
        val = Mathf.Clamp01(val);
        val = (Mathf.Sin(val * Mathf.PI * (0.2f + 2.5f * val * val * val)) * Mathf.Pow(1f - val, 2.2f) + val) * (1f + (1.2f * (1f - val)));
        return val;
    }

    public static float EaseShake(float val)
    {
        val = Mathf.Clamp01(val);
        val *= Mathf.Sin(val * Mathf.PI * 10);
        return val;
    }

    public static float Punch(float val)
    {
        val = Mathf.Clamp01(val);
        val = Mathf.Sin(val * Mathf.PI * 9);
        return val;
    }

    public static float Once(float val)
    {
        val = Mathf.Clamp01(val);
        if (val > 0.999f) val = 1;
        return val;
    }

    public static float Clamp(float val)
    {
        val = Mathf.Clamp01(val);
        return val;
    }

    public static float PingPong(float val)
    {
        val = Mathf.Clamp01(val);
        val = Mathf.PingPong(val, 1);
        return val;
    }

    public static float Ease(EaseType easeType, float val) => easeType switch
    {
        EaseType.Linear           => EaseLinear(val),
        EaseType.EaseOutQuad      => EaseOutQuad(val),
        EaseType.EaseInQuad       => EaseInQuad(val),
        EaseType.EaseInOutQuad    => EaseInOutQuad(val),
        EaseType.EaseInCubic      => EaseInCubic(val),
        EaseType.EaseOutCubic     => EaseOutCubic(val),
        EaseType.EaseInOutCubic   => EaseInOutCubic(val),
        EaseType.EaseInQuart      => EaseInQuart(val),
        EaseType.EaseOutQuart     => EaseOutQuart(val),
        EaseType.EaseInOutQuart   => EaseInOutQuart(val),
        EaseType.EaseInQuint      => EaseInQuint(val),
        EaseType.EaseOutQuint     => EaseOutQuint(val),
        EaseType.EaseInOutQuint   => EaseInOutQuint(val),
        EaseType.EaseInSine       => EaseInSine(val),
        EaseType.EaseOutSine      => EaseOutSine(val),
        EaseType.EaseInOutSine    => EaseInOutSine(val),
        EaseType.EaseInExpo       => EaseInExpo(val),
        EaseType.EaseOutExpo      => EaseOutExpo(val),
        EaseType.EaseInOutExpo    => EaseInOutExpo(val),
        EaseType.EaseInCirc       => EaseInCirc(val),
        EaseType.EaseOutCirc      => EaseOutCirc(val),
        EaseType.EaseInOutCirc    => EaseInOutCirc(val),
        EaseType.EaseInBounce     => EaseInBounce(val),
        EaseType.EaseOutBounce    => EaseOutBounce(val),
        EaseType.EaseInOutBounce  => EaseInOutBounce(val),
        EaseType.EaseInBack       => EaseInBack(val),
        EaseType.EaseOutBack      => EaseOutBack(val),
        EaseType.EaseInOutBack    => EaseInOutBack(val),
        EaseType.EaseInElastic    => EaseInElastic(val),
        EaseType.EaseOutElastic   => EaseOutElastic(val),
        EaseType.EaseInOutElastic => EaseInOutElastic(val),
        EaseType.EaseSpring       => EaseSpring(val),
        EaseType.EaseShake        => EaseShake(val),
        EaseType.Punch            => Punch(val),
        EaseType.Once             => Once(val),
        EaseType.Clamp            => Clamp(val),
        EaseType.PingPong         => PingPong(val),
        _                         => EaseLinear(val)
    };
}

[Serializable]
public struct Ease
{
    public Easings.EaseType EaseType;
    public float Time;

    public float Evaluate(float t) => Easings.Ease(EaseType, t);
}

public static class EasingsExtensions
{
    public static float Ease(this Easings.EaseType easeType, float val) => Easings.Ease(easeType, val);
}
