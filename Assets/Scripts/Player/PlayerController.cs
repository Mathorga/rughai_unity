﻿using UnityEngine;
using System;
using System.Collections;

public class PlayerController : MonoBehaviour {
    public enum State {
        Idle,
        Walk,
        Run,
        Fall,
        Atk0,
        Atk1,
        Atk2,
        AtkIdle
    }
    public Animator[] childAnimators;
    public Vector2Value startFacing;
    public Vector2Value startPosition;

    public float moveSpeed {
        get;
        private set;
    }
    public float faceDir {
        get;
        private set;
    }
    public Vector2 moveForce {
        get;
        private set;
    }
    public State state {
        get;
        private set;
    }

    // Used to trigger attack idle state.
    public bool justAttacked {
        get;
        private set;
    }

    // Threshold from stand to slow walk animation.
    private float slowWalkThreshold = 0.01f;

    // Threshold from slow walk to walk animation.
    private float walkThreshold = 0.1f;

    // Threshold from walk to run animation.
    private float runThreshold = 0.8f;

    private bool atkCombo = false;

    private float animationTime = 0.0f;
    private float animationProgress = 0.0f;

    private Rigidbody2D rb;
    private PlayerInput input;
    private Animator animator;
    private PlayerStats stats;
    private FallController fallController;

    void Awake() {
        this.transform.position = this.startPosition.value;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.input = this.GetComponent<PlayerInput>();
        this.animator = this.GetComponent<Animator>();
        this.stats = this.GetComponent<PlayerStats>();
        this.fallController = this.GetComponent<FallController>();
        this.state = State.Idle;
        this.justAttacked = false;
        this.animator.SetBool("Flip", this.startFacing.value.x < 0);
    }

    void FixedUpdate() {
        // Set state based on input.
        this.FindState();

        // Act according to state.
        this.Behave();
    }
    
    private void Behave() {
        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.data.speed / this.rb.drag;

        // Get current animation progress.
        // float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        // this.animationProgress = animationTime - Mathf.Floor(animationTime);

        if (this.state != State.Fall) {
            // Set facing for direction control.
            if (this.rb.velocity.magnitude > this.slowWalkThreshold * maxVelocity) {
                // Use velocity if > 0.
                Vector3 scale = this.transform.localScale;
                scale.x = this.rb.velocity.x < 0 ? -1 : 1;
                this.transform.localScale = scale;
            } else if (this.moveSpeed > this.slowWalkThreshold) {
                // Use moveSpeed otherwise.
                Vector3 scale = this.transform.localScale;
                scale.x = this.moveForce.x < 0 ? -1 : 1;
                this.transform.localScale = scale;
            }
        }

        switch (this.state) {
            case State.Idle:
                if (this.rb.velocity.magnitude < this.slowWalkThreshold * maxVelocity) {
                    this.PlayAnimation("Idle", 1.0f);
                } else if (this.rb.velocity.magnitude < this.runThreshold * maxVelocity) {
                    this.PlayAnimation("Walk", this.rb.velocity.magnitude < this.walkThreshold * maxVelocity ? 0.5f : 1.0f);
                } else {
                    this.PlayAnimation("Run", 1.0f);
                }
                break;
            case State.Walk:
                // Compute the amount of force to apply to apply to self.
                this.ComputeForce();

                // Actually apply the computed force if greater than 0.
                if (this.moveForce.magnitude > 0f) {
                    this.rb.AddForce(this.moveForce);
                }

                this.PlayAnimation("Walk", this.rb.velocity.magnitude < this.walkThreshold * maxVelocity ? 0.5f : 1.0f);
                break;
            case State.Run:
                // Compute the amount of force to apply to apply to self.
                this.ComputeForce();

                // Actually apply the computed force if greater than 0.
                if (this.moveForce.magnitude > 0f) {
                    this.rb.AddForce(this.moveForce);
                }

                this.PlayAnimation("Run", 1.0f);
                break;
            case State.Fall:
                this.PlayAnimation("Fall", 1.0f);

                // Reset state after animation ends.
                if (this.AnimationDone("Fall")) {
                    this.SetState(State.Idle);
                }
                break;
            case State.Atk0:
                this.rb.velocity = Vector2.zero;

                this.PlayAnimation("Atk0", 1.0f);

                if (this.animationProgress > 0.8f && this.animationProgress < 0.9f && this.input.attack) {
                    // Combo.
                    this.atkCombo = true;
                }

                if (this.AnimationDone("Atk0")) {
                    // Reset state after animation ends.
                    if (this.atkCombo) {
                        this.atkCombo = false;
                        this.SetState(State.Atk1);
                    } else {
                        this.SetState(State.AtkIdle);
                    }
                }
                break;
            case State.Atk1:
                Debug.Log("Bef COMBO! " + this.animator.GetCurrentAnimatorStateInfo(0).IsName("Atk1") + animationProgress.ToString());

                this.PlayAnimation("Atk1", 1.0f);

                Debug.Log("Aft COMBO! " + this.animator.GetCurrentAnimatorStateInfo(0).IsName("Atk1") + animationProgress.ToString());

                if (this.AnimationDone("Atk1")) {
                    // Reset state after animation ends.
                    this.SetState(State.AtkIdle);
                }
                break;
            case State.Atk2:
                break;
            case State.AtkIdle:
                this.PlayAnimation("AtkIdle", 1.0f);

                if (this.AnimationDone("AtkIdle") || this.rb.velocity.magnitude > 0.0f) {
                    this.SetState(State.Idle);
                }
                break;
            default:
                break;
        }
    }

    private void SetState(State state) {
        this.state = state;
    }

    private bool AnimationDone(string animationName) {
        return this.animationTime >= 1.0f && this.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName);
    }

    private void FindState() {
        if (this.fallController.isFalling) {
            this.state = State.Fall;
        } else {
            if (this.state != State.Atk0 && this.state != State.Atk1) {
                if (this.input.attack) {
                    this.justAttacked = true;
                    this.state = State.Atk0;
                } else {
                    if (this.input.moveLen <= this.walkThreshold) {
                        // Idle.
                        this.state = this.justAttacked ? State.AtkIdle : State.Idle;
                    } else if (this.input.moveLen < this.runThreshold || this.input.walk) {
                        // Walk.
                        this.justAttacked = false;
                        this.state = State.Walk;
                    } else {
                        // Run.
                        this.justAttacked = false;
                        this.state = State.Run;
                    }
                }
            }
        }
    }

    private void ComputeForce() {
        if (this.state == State.Walk) {
            this.moveSpeed = this.stats.data.walkSpeed;
        } else if (this.state == State.Run) {
            this.moveSpeed = this.stats.data.speed;
        } else {
            this.moveSpeed = 0f;
        }
        this.faceDir = this.input.moveDir;

        this.moveForce = Utils.PolarToCartesian(this.faceDir, this.moveSpeed);
    }

    private void PlayAnimation(string animationName, float animationSpeed) {
        // Set animation speed.
        this.animator.speed = animationSpeed;

        foreach (Animator childAnimator in this.childAnimators) {
            childAnimator.speed = animationSpeed;
        }

        // Play animation if not already playing.
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            this.animator.Play(animationName, 0, 0.0f);
            foreach (Animator childAnimator in this.childAnimators) {
                childAnimator.Play(animationName, 0, 0.0f);
            }
        }

        // Save animation progress.
        this.animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        this.animationProgress = animationTime - Mathf.Floor(animationTime);
    }
}