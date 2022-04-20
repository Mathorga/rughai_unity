using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingAnimator : MonoBehaviour {
    public Stats stats;
    public float threshold = 0.05f;

    private Rigidbody2D rb;
    private Animator animator;

    private HitController hitController;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.hitController = this.GetComponent<HitController>();
    }
    void FixedUpdate() {
        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.speed / this.rb.drag;

        // Set animator facing if current velocity exceeds the threshold.
        if ((this.hitController == null || !this.hitController.hit) && this.rb.velocity.magnitude > this.threshold * maxVelocity) {
            Vector3 scale = this.transform.localScale;
            scale.x = this.rb.velocity.x < 0 ? -1 : 1;
            this.transform.localScale = scale;
        }
    }
}
