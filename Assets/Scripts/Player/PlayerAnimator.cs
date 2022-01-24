﻿using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    public PlayerController controller;
    public Animator[] childAnimators;
    public Vector2Value startFacing;

    // Threshold from stand to slow walk animation.
    private float slowWalkThreshold = 0.01f;

    // Threshold from slow walk to walk animation.
    private float walkThreshold = 0.1f;

    // Threshold from walk to run animation.
    private float runThreshold = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;
    private FallController fallController;
    private PlayerStats stats;

    void Awake() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.fallController = this.GetComponent<FallController>();
        this.stats = this.GetComponent<PlayerStats>();
        this.animator.SetBool("Flip", this.startFacing.value.x < 0);
    }

    void FixedUpdate() {
        this.Animate();
    }
    
    void Animate() {
        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.data.speed / this.rb.drag;

        if (this.controller.state != PlayerController.State.Fall) {
            // Set facing for direction control.
            if (this.rb.velocity.magnitude > this.slowWalkThreshold * maxVelocity) {
                // Use velocity if > 0.
                Vector3 scale = this.transform.localScale;
                scale.x = this.rb.velocity.x < 0 ? -1 : 1;
                this.transform.localScale = scale;
            } else if (this.controller.moveSpeed > this.slowWalkThreshold) {
                // Use moveSpeed otherwise.
                Vector3 scale = this.transform.localScale;
                scale.x = this.controller.moveForce.x < 0 ? -1 : 1;
                this.transform.localScale = scale;
            }
        }

        // Get current animation progress.
        float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float animationProgress = animationTime - Mathf.Floor(animationTime);

        if (this.controller.state == PlayerController.State.Attack0) {
            this.animator.speed = 1f;

            foreach (Animator childAnimator in this.childAnimators) {
                childAnimator.speed = 1f;
            }

            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Attack0")) {
                this.animator.Play("Attack0");

                foreach (Animator childAnimator in this.childAnimators) {
                    childAnimator.Play("Attack0");
                }
            }

            // Reset state after animation ends.
            if (animationTime >= 1f) {
                this.controller.SetState(PlayerController.State.Idle);
            }
        } else {
            if (this.fallController.isFalling) {
                // Set slower speed for stand animation.
                this.animator.speed = 0.5f;

                foreach (Animator childAnimator in this.childAnimators) {
                    childAnimator.speed = 0.5f;
                }
                this.animator.Play("Fall");

                foreach (Animator childAnimator in this.childAnimators) {
                    childAnimator.Play("Fall");
                }

                // Reset state after animation ends.
                if (animationTime >= 1f) {
                    this.controller.SetState(PlayerController.State.Idle);
                }
            } else {
                // Set animation state.
                if (this.rb.velocity.magnitude < this.slowWalkThreshold * maxVelocity &&
                    this.controller.moveSpeed < this.stats.data.walkSpeed) {
                    // Set slower speed for stand animation.
                    this.animator.speed = 0.5f;

                    foreach (Animator childAnimator in this.childAnimators) {
                        childAnimator.speed = 0.5f;
                    }

                    // Current velocity is below slow walk, so stand.
                    // Current move speed is also checked in order to be able to walk against walls.
                    if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")) {
                        this.animator.Play("Idle");

                        foreach (Animator childAnimator in this.childAnimators) {
                            childAnimator.Play("Idle");
                        }
                    }
                } else {
                    if (this.rb.velocity.magnitude < this.walkThreshold * maxVelocity) {
                        // In order to achieve slow walk, the standard walk animation is played at slower speed.
                        this.animator.speed = 0.5f;

                        foreach (Animator childAnimator in this.childAnimators) {
                            childAnimator.speed = 0.5f;
                        }

                        // Current velocity is above slow walk and below walk, so slow walk.
                        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                            this.animator.Play("Walk", 0, animationProgress);

                            foreach (Animator childAnimator in this.childAnimators) {
                                childAnimator.Play("Walk", 0, animationProgress);
                            }
                        }
                    } else {
                        // Set standard speed for standard walk and run animations.
                        this.animator.speed = 1f;

                        foreach (Animator childAnimator in this.childAnimators) {
                            childAnimator.speed = 1f;
                        }

                        if (this.rb.velocity.magnitude < this.runThreshold * maxVelocity) {
                            // Current velocity is above walk and below run (or walk is triggered), so walk.
                            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                                this.animator.Play("Walk", 0, animationProgress);

                                foreach (Animator childAnimator in this.childAnimators) {
                                    childAnimator.Play("Walk", 0, animationProgress);
                                }
                            }
                        } else {
                            // Current velocity is above run, so run.
                            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
                                this.animator.Play("Run", 0, animationProgress);

                                foreach (Animator childAnimator in this.childAnimators) {
                                    childAnimator.Play("Run", 0, animationProgress);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
