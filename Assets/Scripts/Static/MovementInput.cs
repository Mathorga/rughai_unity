using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MovementInput {
    public static float threshold = 0.25f;

    // Returns the normalized input data (with maximum magnitude of 1).
    public static Vector2 GetMovement() {
        // Get raw input data (from controller or keyboard).
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(hor, ver);

        float speed;

        // Magnitude is actually greater if both hor and ver are both maxed (1 or -1), but since magnitude check is only used
        // for animation control and speed (and not actual movement), it's not gonna be noticeable.
        if (input.magnitude < MovementInput.threshold) {
            hor = 0;
            ver = 0;
            speed = 0;
        } else {
            speed = 1;
        }

        // Compute the angle of the input vector.
        float dir = AngleBetween(Vector2.zero, input);

        // Return a normalized vector.
        return PolarToCartesian(dir * (Mathf.PI / 180), speed);
    }

    static float AngleBetween(Vector2 v1, Vector2 v2) {
     return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * (180 / Mathf.PI);
    }

    private static Vector2 PolarToCartesian(float angle, float magnitude) {
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
    }
}