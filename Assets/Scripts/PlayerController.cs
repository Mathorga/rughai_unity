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
    private float walkSpeed;
    private Vector2 moveForce;
    private float slowWalkThreshold = 0.01f;
    private float walkThreshold = 0.1f;
    private float runThreshold = 0.9f;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.walkSpeed = this.speed / 2.5f;
    }

    void FixedUpdate() {
        this.ComputeForce();
        this.Animate();
        this.rb.AddForce(this.moveForce);
    }

    void Animate() {
        float maxVelocity = this.speed / this.rb.drag;

        // Set facing for direction control.
        this.animator.SetFloat("FaceX", this.faceX);
        this.animator.SetFloat("FaceY", this.faceY);

        // Set animation state.
        if (this.rb.velocity.magnitude < this.slowWalkThreshold * maxVelocity) {
            // Current velocity is below slow walk, so stand.
            this.animator.Play("Stand");
        } else {
            // Get current animation progress.
            float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float animationProgress = animationTime - Mathf.Floor(animationTime);

            if (this.rb.velocity.magnitude < this.walkThreshold * maxVelocity) {
                // In order to achieve slow walk, the standard walk animation is played at slower speed.
                this.animator.speed = 0.5f;

                // Current velocity is above slow walk and below walk, so slow walk.
                this.animator.Play("Walk", 0, animationProgress);
            } else {
                // Set standard speed for standard walk and run animations.
                this.animator.speed = 1f;
                if (this.rb.velocity.magnitude < this.runThreshold * maxVelocity ||
                    this.input.walk) {
                    // Current velocity is above walk and below run (or walk is triggered), so walk.
                    this.animator.Play("Walk", 0, animationProgress);
                } else {
                    // Current velocity is above run, so run.
                    this.animator.Play("Run", 0, animationProgress);
                }
            }
        }
    }

    void ComputeForce() {
        if (this.input.moveLen <= walkThreshold) {
            this.moveSpeed = 0;
        } else {
            // Set facing.
            this.faceX = this.moveForce.x;
            this.faceY = this.moveForce.y;

            if (this.input.moveLen < runThreshold ||
                this.input.walk) {
                this.moveSpeed = this.walkSpeed;
            } else {
                this.moveSpeed = this.speed;
            }
        }

        this.moveForce = Utils.PolarToCartesian(this.input.moveDir, this.moveSpeed);
    }
}