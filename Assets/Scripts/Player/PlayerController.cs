using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public Vector2Value startPosition;
    public Vector2Value facing;
    public PlayerStats stats;

    private Rigidbody2D rb;
    private PlayerInput input;

    // Actual moving speed.
    private Vector2 moveForce;

    void Awake() {
        this.transform.position = this.startPosition.value;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.input = this.GetComponent<PlayerInput>();
    }

    void FixedUpdate() {
        this.ComputeForce();
        if (this.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.moveForce);
        }
    }

    void ComputeForce() {
        if (this.input.moveLen <= this.input.walkThreshold) {
            this.stats.moveSpeed = 0f;
        } else {
            if (this.input.moveLen < this.input.runThreshold ||
                this.input.walk) {
                this.stats.moveSpeed = this.stats.walkSpeed;
            } else {
                this.stats.moveSpeed = this.stats.speed;
            }
        }

        this.moveForce = Utils.PolarToCartesian(this.input.moveDir, this.stats.moveSpeed);

        if (this.stats.moveSpeed > 0f) {
            // Set facing.
            this.facing.value.x = this.moveForce.x;
            this.facing.value.y = this.moveForce.y;
        }
    }
}