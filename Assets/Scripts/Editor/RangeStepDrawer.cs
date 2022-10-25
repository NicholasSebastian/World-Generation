using System;
using UnityEditor;
using UnityEngine;

// Sourced from VictorHHT at https://gist.github.com/VictorHHT.

[CustomPropertyDrawer(typeof(RangeStep))]
internal sealed class RangeStepDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var rangeAttribute = (RangeStep)base.attribute;

        if (!rangeAttribute.m_IsInt)
        {
            float rawFloat = EditorGUI.Slider(position, label, property.floatValue, rangeAttribute.m_Min, rangeAttribute.m_Max);
            property.floatValue = Step(rawFloat, rangeAttribute);
        }
        else
        {
            int rawInt = EditorGUI.IntSlider(position, label, property.intValue, (int)rangeAttribute.m_Min, (int)rangeAttribute.m_Max);
            property.intValue = Step(rawInt, rangeAttribute);
        }
            
    }

    private float Step(float rawValue, RangeStep range)
    {
        float f = rawValue;

        if (range.m_AllowNonStepReach)
        {
            // In order to ensure a reach, where the difference between rawValue and the max allowed value is less than step
            float topCap = (float)Math.Round(Mathf.Floor(range.m_Max / range.m_Step) * range.m_Step, range.m_Precision);
            float topRemaining = (float)Math.Round(range.m_Max - topCap, range.m_Precision);

            // If this is the special case near the top maximum
            if (topRemaining < range.m_Step && f > topCap + topRemaining / 2)
            {
                f = range.m_Max;
            }
            else
            {
                // Otherwise we do a regular snap
                f = (float)Math.Round(Snap(f, range.m_Step), range.m_Precision);
            }
        }
        else if(!range.m_AllowNonStepReach)
        {
            f = (float)Math.Round(Snap(f, range.m_Step), range.m_Precision);
            // Make sure the value doesn't exceed the maximum allowed range
            if (f > range.m_Max)
            {
                f -= range.m_Step;
                f = (float)Math.Round(f, range.m_Precision);
            }
        }

        return f;
    }

    private int Step(int rawValue, RangeStep range)
    {
        int f = rawValue;

        if (range.m_AllowNonStepReach)
        {
            // In order to ensure a reach, where the difference between rawValue and the max allowed value is less than step
            int topCap = (int)range.m_Max / (int)range.m_Step * (int)range.m_Step;
            int topRemaining = (int)range.m_Max - topCap;

            // If this is the special case near the top maximum
            if (topRemaining < range.m_Step && f > topCap)
            {
                f = (int)range.m_Max;
            }
            else
            {
                // Otherwise we do a regular snap
                f = (int)Snap(f, range.m_Step);
            }
        }
        else if (!range.m_AllowNonStepReach)
        {
            f = (int)Snap(f, range.m_Step);
            // Make sure the value doesn't exceed the maximum allowed range
            if (f > range.m_Max)
            {
                f -= (int)range.m_Step;
            }
        }

        return f;
    }

    /// <summary>
    /// Snap a value to a interval
    /// </summary>
    /// <param name="value"></param>
    /// <param name="snapInterval"></param>
    /// <returns></returns>
    private static float Snap(float value, float snapInterval)
    {
        return Mathf.Round(value / snapInterval) * snapInterval;
    }
}
