using UnityEngine;

public static class Utils
{
    public static Vector2 QuadraticLinearBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2)
    {
        var u = 1 - t;
        var p = u * u * p0;
        p += 2 * u * t * p1;
        p += t * t * p2;
        return p;
    }
}