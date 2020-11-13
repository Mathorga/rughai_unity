using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementInput : MonoBehaviour {
    public float threshold = 0.25f;
    public MainInput controls;

    public float moveDir {
        get;
        private set;
    }
    public float moveLen {
        get;
        private set;
    }

    private Vector2 GetInput() {
        // Get raw input data (from controller or keyboard).
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");
        return new Vector2(hor, ver);
    }

    // Returns the normalized input data (with maximum magnitude of 1).
    public Vector2 GetMovement() {
        Vector2 input = this.GetInput();

        // float speed;

        // Magnitude is actually greater if both hor and ver are both maxed (1 or -1), but since magnitude check is only used
        // for animation control and speed (and not actual movement), it's not gonna be noticeable.
        // if (input.magnitude < this.threshold) {
        //     hor = 0;
        //     ver = 0;
        //     speed = 0;
        // } else {
        //     speed = 1;
        // }

        // Compute the angle of the input vector.
        float dir = Vector2Converter.AngleBetween(Vector2.zero, input);
        float len = input.magnitude < this.threshold ? 0 : input.magnitude;

        return Vector2Converter.PolarToCartesian(dir, len);
    }

    public void ReadInput() {
        this.ReadMoveDirection();
        this.ReadMoveLength();
    }

    public void ReadMoveDirection() {
        Vector2 input = this.GetInput();
        this.moveDir = Vector2Converter.AngleBetween(Vector2.zero, input);
    }

    public void ReadMoveLength() {
        Vector2 input = this.GetInput();
        this.moveLen = Mathf.Clamp(input.magnitude, 0, 1);
    }
}