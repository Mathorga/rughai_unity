using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public float speed;
    public PlayerInput input;
    public Animator animator;

    private Rigidbody2D rb;
    private float faceX;
    private float faceY;

    // Actual moving speed.
    private float moveSpeed;
    private Vector2 moveForce;
    private float idleThreshold = 0.1f;
    private float walkThreshold = 0.9f;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        this.Animate();
        this.ComputeForce();
        this.rb.AddForce(this.moveForce);
    }

    void Animate() {
        float maxVelocity = this.speed / this.rb.drag;

        // Set facing for direction control.
        this.animator.SetFloat("FaceX", this.faceX);
        this.animator.SetFloat("FaceY", this.faceY);

        // Set animation state.
        if (this.rb.velocity.magnitude < idleThreshold * maxVelocity) {
            this.animator.Play("Stand");
        } else {
            // Set facing.
            this.faceX = this.rb.velocity.x;
            this.faceY = this.rb.velocity.y;

            if (this.rb.velocity.magnitude < walkThreshold * maxVelocity ||
                this.input.walk) {
                // Get current animation progress.
                float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float animationProgress = animationTime - Mathf.Floor(animationTime);
                this.animator.Play("Walk", 0, animationProgress);
            } else {
                // Get current animation progress.
                float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                float animationProgress = animationTime - Mathf.Floor(animationTime);
                this.animator.Play("Run", 0, animationProgress);
            }
        }
    }

    void ComputeForce() {
        if (this.input.moveLen <= idleThreshold) {
            this.moveSpeed = 0;
        } else {
            // Set facing.
            this.faceX = this.rb.velocity.x;
            this.faceY = this.rb.velocity.y;
            if (this.input.moveLen < walkThreshold ||
                this.input.walk) {
                this.moveSpeed = this.speed / 2;
            } else {
                this.moveSpeed = this.speed;
            }
        }

        this.moveForce = Vector2Converter.PolarToCartesian(this.input.moveDir, this.moveSpeed);
    }
}