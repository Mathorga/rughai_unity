using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInput : MonoBehaviour {
    public float threshold = 0.25f;
    public MainInput controls;

    void Awake() {
    }

    void Start() {}

    void Update() {}

    // Returns the normalized input data (with maximum magnitude of 1).
    public Vector2 GetMovement() {
        // Get raw input data (from controller or keyboard).
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        Vector2 input = new Vector2(hor, ver);

        float speed;

        // Magnitude is actually greater if both hor and ver are both maxed (1 or -1), but since magnitude check is only used
        // for animation control and speed (and not actual movement), it's not gonna be noticeable.
        if (input.magnitude < this.threshold) {
            hor = 0;
            ver = 0;
            speed = 0;
        } else {
            speed = 1;
        }

        // Compute the angle of the input vector.
        float dir = Vector2Converter.AngleBetween(Vector2.zero, input);

        // Return a normalized vector.
        return Vector2Converter.PolarToCartesian(dir, speed);
    }
}