using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    public PlayerController controller;
    public Animator[] childAnimators;
    public Vector2Value startFacing;

    // Threshold from stand to slow walk animation.
    private float slowWalkThreshold = 0.01f;

    // Threshold from slow walk to walk animation.
    private float walkThreshold = 0.1f;

    // Threshold from walk to run animation.
    private float runThreshold = 0.8f;

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

    void PlayAnimation(string animationName, float animationSpeed) {
        // Set animation speed.
        this.animator.speed = animationSpeed;

        foreach (Animator childAnimator in this.childAnimators) {
            childAnimator.speed = animationSpeed;
        }

        // Play animation if not already playing.
        if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName(animationName)) {
            this.animator.Play(animationName);

            foreach (Animator childAnimator in this.childAnimators) {
                childAnimator.Play(animationName);
            }
        }
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

        switch (this.controller.state) {
            case PlayerController.State.Attack0:
                this.PlayAnimation("Atk0", 1.0f);

                // Reset state after animation ends.
                if (animationTime >= 1f) {
                    this.controller.SetState(PlayerController.State.Idle);
                }
                break;
            case PlayerController.State.Fall:
                this.PlayAnimation("Fall", 1.0f);

                // Reset state after animation ends.
                if (animationTime >= 1f) {
                    this.controller.SetState(PlayerController.State.Idle);
                }
                break;

            case PlayerController.State.Run:
                this.PlayAnimation("Run", 1.0f);
                break;
            
            case PlayerController.State.Walk:
                this.PlayAnimation("Walk", this.rb.velocity.magnitude < this.walkThreshold * maxVelocity ? 0.5f : 1.0f);
                break;

            case PlayerController.State.Idle:
                if (this.rb.velocity.magnitude < this.slowWalkThreshold * maxVelocity) {
                    this.PlayAnimation("Idle", 1.0f);
                } else if (this.rb.velocity.magnitude < this.runThreshold * maxVelocity) {
                    this.PlayAnimation("Walk", this.rb.velocity.magnitude < this.walkThreshold * maxVelocity ? 0.5f : 1.0f);
                } else {
                    this.PlayAnimation("Run", 1.0f);
                }
                break;

            case PlayerController.State.AttackIdle:
                this.PlayAnimation("AtkIdle", 1.0f);
                break;

            default:
                break;
        }
    }
}
