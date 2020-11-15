using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInput : MonoBehaviour {
    private MainControls controls;
    public float moveDir {
        get;
        private set;
    }
    public float moveLen {
        get;
        private set;
    }

    private void Awake() {
        this.controls = new MainControls();

        // Check for movement change.
        this.controls.Camera.Move.performed += (context) => this.SetMoveDirection(context.ReadValue<Vector2>());
        this.controls.Camera.Move.performed += (context) => this.SetMoveLength(context.ReadValue<Vector2>());

        // Check for movement end.
        this.controls.Camera.Move.canceled += (context) => this.SetMoveDirection(context.ReadValue<Vector2>());
        this.controls.Camera.Move.canceled += (context) => this.SetMoveLength(context.ReadValue<Vector2>());
    }

    private void OnEnable() {
        this.controls.Enable();
    }

    private void OnDisable() {
        this.controls.Disable();
    }

    private void SetMoveDirection(Vector2 moveVector) {
        this.moveDir = Utils.AngleBetween(Vector2.zero, moveVector);
    }

    public void SetMoveLength(Vector2 moveVector) {
        this.moveLen = Mathf.Clamp(moveVector.magnitude, 0, 1);
    }
}
