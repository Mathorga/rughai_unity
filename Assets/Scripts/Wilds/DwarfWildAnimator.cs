using UnityEngine;

public class DwarfWildAnimator : MonoBehaviour {
    // Threshold from stand to walk animation.
    private float walkThreshold = 0.05f;

    private Rigidbody2D rb;
    private Animator animator;

    private HitController hitController;
    private IWild controller;
    private WildStats stats;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.hitController = this.GetComponent<HitController>();
        this.controller = this.GetComponent<IWild>();
        this.stats = this.GetComponent<WildStats>();
    }

    void Update() {
        // Get current animation progress.
        float animationTime = this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float animationProgress = animationTime - Mathf.Floor(animationTime);

        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.speed / this.rb.drag;

        switch(this.controller.mode) {
            case IWild.Mode.Dead:
                this.animator.Play("Dead", 0, animationProgress);
                break;
            case IWild.Mode.Idle:
                if (this.hitController.hit) {
                    if (animationTime >= 1 && !Utils.InAnimation(this.animator, "Hit")){
                        this.animator.Play("Hit");
                    }
                } else if (this.rb.velocity.magnitude < this.walkThreshold * maxVelocity) {
                    if (animationTime >= 1 && !Utils.InAnimation(this.animator, "Idle0")){
                        this.animator.Play("Idle0");
                    }
                } else {
                    if (!Utils.InAnimation(this.animator, "Walk")) {
                        this.animator.Play("Walk", 0, animationProgress);
                    }
                }
                break;
            default:
                break;
        }
    }
}
