using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimator : MonoBehaviour {
    public Stats stats;

    // Threshold from slow walk to walk animation.
    private float walkThreshold = 0.05f;

    private Rigidbody2D rb;
    private Animator animator;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
    }

    void FixedUpdate() {
        // Get current animation progress.
        float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float animationProgress = animationTime - Mathf.Floor(animationTime);

        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.speed / this.rb.drag;

        if (this.rb.velocity.magnitude > this.walkThreshold * maxVelocity) {
            // Use velocity if > 0.
            this.animator.SetFloat("FaceX", this.rb.velocity.x);
            this.animator.SetFloat("FaceY", this.rb.velocity.y);
        }

        if (this.rb.velocity.magnitude < this.walkThreshold * maxVelocity) {
            if (animationTime >= 1){
                if (Random.Range(0.0f, 1.0f) > 0.01) {
                    if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Stand0")) {
                        this.animator.Play("Stand0");
                    }
                } else {
                    if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Stand1")) {
                        this.animator.Play("Stand1");
                    }
                }
            }
        } else {
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                this.animator.Play("Walk", 0, animationProgress);
            }
        }
    }
}
