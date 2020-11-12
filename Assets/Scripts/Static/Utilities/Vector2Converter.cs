using UnityEngine;

public static class Vector2Converter {
    public static float AngleBetween(Vector2 v1, Vector2 v2) {
     return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * (180 / Mathf.PI);
    }

    // Angle is in degrees.
    public static Vector2 PolarToCartesian(float angle, float magnitude) {
        // Convert angle to radians.
        float radAngle = angle * (Mathf.PI / 180);

        // Compute.
        return new Vector2(Mathf.Cos(radAngle), Mathf.Sin(radAngle)) * magnitude;
    }
}