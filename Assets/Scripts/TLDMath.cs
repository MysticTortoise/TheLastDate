
using UnityEngine;

public class TLDMath
{
    public static float CloseToZero(float a, float b)
    {
        if (Mathf.Abs(a) < Mathf.Abs(b))
            return a;
        else
            return b;
    }
}
