using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionAnimator : MonoBehaviour {
    public Stats stats;

    // Threshold from slow walk to walk animation.
    private float walkThreshold = 0.1f;

    private Rigidbody2D rb;
    private Animator animator;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
    }

    void FixedUpdate() {
        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.speed / this.rb.drag;

        if (this.rb.velocity.magnitude > this.walkThreshold * maxVelocity) {
            // Use velocity if > 0.
            this.animator.SetFloat("FaceX", this.rb.velocity.x);
            this.animator.SetFloat("FaceY", this.rb.velocity.y);
        }
    }
}
