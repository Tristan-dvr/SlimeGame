using UnityEngine;

public static class Utils
{
    public static bool InRange(Vector3 center, Vector3 point, float radius)
    {
        var direction = point - center;
        return direction.sqrMagnitude < radius * radius;
    }
}
