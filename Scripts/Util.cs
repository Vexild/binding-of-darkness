using System;
using Godot;

public class Util
{
    public int ToHalfUnits(float value)
    {
        if (value < 0)
        {
            value = 0;
        }
        return Mathf.RoundToInt(value * 2f);
    }

    public float FromHalfUnits(int value)
    {
        return value / 2f;
    }
    public float NormalizeHalfStep(float value)
    {
        if (value < 0)
        {
            value = 0;
        }

        float normalized = (float)Math.Round(value * 2, MidpointRounding.AwayFromZero) / 2;
        if (!Mathf.IsEqualApprox(value, normalized))
        {
            GD.PushWarning("Value should be in 0.5 increments. Normalized to nearest 0.5.");
        }
        return normalized;
    }
}