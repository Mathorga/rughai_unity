using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour {
    private MainControls controls;
    public float moveDir {
        get;
        private set;
    }
    public float moveLen {
        get;
        private set;
    }
    public bool walk {
        get;
        private set;
    }

    private void Awake() {
        this.controls = new MainControls();

        // Check for movement change.
        this.controls.Player.Move.performed += (context) => this.SetMoveDirection(context.ReadValue<Vector2>());
        this.controls.Player.Move.performed += (context) => this.SetMoveLength(context.ReadValue<Vector2>());

        // Check for movement end.
        this.controls.Player.Move.canceled += (context) => this.SetMoveDirection(context.ReadValue<Vector2>());
        this.controls.Player.Move.canceled += (context) => this.SetMoveLength(context.ReadValue<Vector2>());

        // Check for walk trigger start.
        this.controls.Player.WalkTrigger.started += (context) => this.SetWalkTrigger(true);

        // Check for walk trigger end.
        this.controls.Player.WalkTrigger.canceled += (context) => this.SetWalkTrigger(false);
    }

    private void OnEnable() {
        this.controls.Enable();
    }

    private void OnDisable() {
        this.controls.Disable();
    }

    private void SetMoveDirection(Vector2 moveVector) {
        this.moveDir = Vector2Converter.AngleBetween(Vector2.zero, moveVector);
    }

    public void SetMoveLength(Vector2 moveVector) {
        this.moveLen = Mathf.Clamp(moveVector.magnitude, 0, 1);
    }

    public void SetWalkTrigger(bool value) {
        this.walk = value;
    }
}