using System;
using UnityEngine;

// Sourced from VictorHHT at https://gist.github.com/VictorHHT.

// Use Example 1: [RangeStep(0f, 10f, 0.25f)]
// Use Example 2: [RangeStep(100, 1000, 25)]

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public sealed class RangeStep : PropertyAttribute
{
    public readonly float m_Min = 0f;
    public readonly float m_Max = 100f;
    public readonly float m_Step = 1;
    public readonly int m_Precision;
    // Whether a increase that is not the step is allowed (Occurs when we are reaching the end)
    public readonly bool m_AllowNonStepReach = true;
    public readonly bool m_IsInt = false;


    /// <summary>
    /// Allow you to increase a float value in step, make sure the type of the variable matches the the parameters
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="step"></param>
    /// <param name="allowNonStepReach">Whether a increase that is not the step is allowed (Occurs when we are reaching the end)</param>
    public RangeStep(float min, float max, float step = 1f, bool allowNonStepReach = true)
    {
        m_Min = min;
        m_Max = max;
        m_Step = step;
        m_Precision = Precision(m_Step);
        m_AllowNonStepReach = allowNonStepReach;
        m_IsInt = false;
    }

    /// <summary>
    /// Allow you to increase a int value in step, make sure the type of the variable matches the the parameters
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="step"></param>
    /// <param name="allowNonStepReach"></param>
    public RangeStep(int min, int max, int step = 1, bool allowNonStepReach = true)
    {
        m_Min = min;
        m_Max = max;
        m_Step = step;
        m_AllowNonStepReach = allowNonStepReach;
        m_IsInt = true;
    }

    private static int Precision(float f)
    {
        if ((int)f % 10 == 0)
        {
            return 0;
        }

        int precision = 0;

        while (f % 10f != 0)
        {
            f *= 10f;
            precision++;
        }

        // Since float only got precision up to 7
        return Math.Min(precision, 7);
    }
}
