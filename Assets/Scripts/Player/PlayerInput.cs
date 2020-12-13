using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool interact {
        get;
        private set;
    }
    public bool attack {
        get;
        private set;
    }

    public void Disable() {
        float len = this.moveLen;
        float dir = this.moveDir;
        this.controls.Disable();
        this.moveLen = len;
        this.moveDir = dir;
    }

    public void SetMoveDir(float dir) {
        this.moveDir = dir;
    }

    public void SetMoveLen(float len) {
        this.moveLen = len;
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

        // Check for interact start.
        this.controls.Player.Interact.started += (context) => this.SetInteract(true);

        // Check for interact end.
        this.controls.Player.Interact.canceled += (context) => this.SetInteract(false);

        // Check for attack start.
        this.controls.Player.Attack.started += (context) => this.SetAttack(true);

        // Check for attack end.
        this.controls.Player.Attack.canceled += (context) => this.SetAttack(false);
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

    public void SetWalkTrigger(bool value) {
        this.walk = value;
    }

    public void SetInteract(bool value) {
        this.interact = value;
    }

    public void SetAttack(bool value) {
        this.attack = value;
    }
}