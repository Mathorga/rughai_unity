using UnityEngine;

public static class Utils {
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

    public static bool PointInsideCollider(Collider2D collider, Vector2 point) {
        Vector2 closest = collider.ClosestPoint(point);

        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }
}