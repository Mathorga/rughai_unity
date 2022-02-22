using Unity.Mathematics;
using UnityEngine;

public static class Utils {
    public static float TILE_RATIO = 1.0f;
    private const int STRAIGHT_COST = 10;
    private const int DIAGONAL_COST = 14;

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

    public static int ComputeIndex(int x, int y, int width) {
        return x + y * width;
    }

    // Returns the given position offset by width. Useful for parallax scrolling.
    public static Vector3 offsetPosition(Vector3 target, float offset) {
        return target * offset;
    }
    

    public static int ComputeHCost(int2 start, int2 end) {
        int xDistance = math.abs(start.x - end.x);
        int yDistance = math.abs(start.y - end.y);
        int remaining = math.abs(xDistance - yDistance);

        return DIAGONAL_COST * math.min(xDistance, yDistance) + STRAIGHT_COST * remaining;
    }
}