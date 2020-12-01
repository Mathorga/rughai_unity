using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    public PlayerStats stats;

    // Threshold from stand to slow walk animation.
    private float slowWalkThreshold = 0.01f;

    // Threshold from slow walk to walk animation.
    private float walkThreshold = 0.1f;

    // Threshold from walk to run animation.
    private float runThreshold = 0.5f;

    private Rigidbody2D rb;
    private Animator animator;

    void Awake() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
    }

    void FixedUpdate() {
        this.Animate();
    }
    
    void Animate() {
        float maxVelocity = this.stats.speed / this.rb.drag;

        // Set facing for direction control.
        if (this.rb.velocity.magnitude > 0) {
            // Use velocity if > 0.
            this.animator.SetFloat("FaceX", this.rb.velocity.x);
            this.animator.SetFloat("FaceY", this.rb.velocity.y);
        } else if (this.stats.moveSpeed > 0) {
            // Use moveSpeed otherwise.
            this.animator.SetFloat("FaceX", this.stats.moveForce.x);
            this.animator.SetFloat("FaceY", this.stats.moveForce.y);
        }

        // Set animation state.
        if (this.rb.velocity.magnitude < this.slowWalkThreshold * maxVelocity &&
            this.stats.moveSpeed < this.stats.walkSpeed) {
            // Set slower speed for stand animation.
            this.animator.speed = 0.5f;

            // Current velocity is below slow walk, so stand.
            // Current move speed is also checked in order to be able to walk against walls.
            if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Stand")) {
                this.animator.Play("Stand");
            }
        } else {
            // Get current animation progress.
            float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
            float animationProgress = animationTime - Mathf.Floor(animationTime);

            if (this.rb.velocity.magnitude < this.walkThreshold * maxVelocity) {
                // In order to achieve slow walk, the standard walk animation is played at slower speed.
                this.animator.speed = 0.5f;

                // Current velocity is above slow walk and below walk, so slow walk.
                if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                    this.animator.Play("Walk", 0, animationProgress);
                }
            } else {
                // Set standard speed for standard walk and run animations.
                this.animator.speed = 1f;

                if (this.rb.velocity.magnitude < this.runThreshold * maxVelocity) {
                    // Current velocity is above walk and below run (or walk is triggered), so walk.
                    if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) {
                        this.animator.Play("Walk", 0, animationProgress);
                    }
                } else {
                    // Current velocity is above run, so run.
                    if (!this.animator.GetCurrentAnimatorStateInfo(0).IsName("Run")) {
                        this.animator.Play("Run", 0, animationProgress);
                    }
                }
            }
        }
    }
}
