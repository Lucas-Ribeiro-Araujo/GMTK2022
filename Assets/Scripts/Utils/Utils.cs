using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    /// <summary>
    /// <c>Remaps</c> a float from one range to another
    /// </summary>
    /// <param name="value">The orginal float</param>
    /// <param name="from1">Min value from the first range</param>
    /// <param name="to1">Max value from the first range</param>
    /// <param name="from2">Min value from the second range</param>
    /// <param name="to2">Max value from the second range</param>
    /// <returns></returns>
    public static float Remap(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public static bool RotationEquals(Quaternion r1, Quaternion r2, float threshold = 0.9999f)
    {
        float abs = Mathf.Abs(Quaternion.Dot(r1, r2));
        if (abs >= threshold)
            return true;
        return false;
    }

    public static bool DirectionEquals(Vector3 r1, Vector3 r2, float threshold = 0.9999f)
    {
        float abs = Mathf.Abs(Vector3.Dot(r1, r2));
        if (abs >= threshold)
            return true;
        return false;
    }

}
