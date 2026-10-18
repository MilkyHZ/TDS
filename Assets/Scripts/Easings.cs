using UnityEngine;

public static class Easing
{
    public static float EaseInElastic(float x)
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        float c4 = (2f * Mathf.PI) / 3f;

        return -(Mathf.Pow(2, 10 * x - 10) * Mathf.Sin((x * 10 - 10.75f) * c4));

    }

    public static float EaseOutElastic(float x)
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        float c4 = (2f * Mathf.PI) / 3f;
        
        return (Mathf.Pow(2, -10 * x + 10) * Mathf.Sin((x * 10 - 0.75f) * c4)) + 1;
    }

    public static float EaseInOutElastic(float x) 
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        float c5 = (2f * Mathf.PI) / 4.5f;

        if (x < 0.5)
            return -(Mathf.Pow(2, 20 * x - 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2;
        else
            return (Mathf.Pow(2, -20 * x + 10) * Mathf.Sin((20 * x - 11.125f) * c5)) / 2 + 1;
    }

    public static float EaseOutQuad(float x)
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        return 1f - (1f - x) * (1f - x);
    }

    public static float EaseInQuad(float x)
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        return Mathf.Pow(x, 2);
    }

    public static float EaseInCubic(float x)
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        return Mathf.Pow(x, 3);
    }

    public static float EaseOutCubic(float x)
    {
        if (x <= 0f) return 0f;
        if (x >= 1f) return 1f;

        return 1f - Mathf.Pow(1f - x, 3);
    }
}
