using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {
    public Stats stats;
    private float currentHealth;
    public int immuneTime;

    private int timeSinceLastHit;

    private Rigidbody2D rb;
    public AudioSource hitAudio;

    // TODO Replace with generic controller.
    private DukController controller;

    public bool hit {
        get;
        private set;
    }

    void Start() {
        this.currentHealth = this.stats.health;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.hitAudio = this.GetComponent<AudioSource>();
        this.controller = this.GetComponent<DukController>();
        this.hit = false;
    }

    void FixedUpdate() {
        if (this.currentHealth <= 0) {
            // Die.
            this.controller.state = DukController.State.Dead;
            // Destroy(this.gameObject);
        }

        if (this.timeSinceLastHit <= this.immuneTime) {
            this.timeSinceLastHit += 1;
        } else {
            this.hit = false;
        }
    }

    public void takeDamage(Transform source, float damage) {
        if (this.timeSinceLastHit > this.immuneTime) {
            // Play hit sound.
            this.hitAudio.Play();

            // Knockback.
            float dir = Utils.AngleBetween(source.position, this.transform.position);
            float len = 10.0f;
            Vector2 force = Utils.PolarToCartesian(dir, len);
            this.rb.AddForce(force, ForceMode2D.Impulse);

            // Actual health damage.
            this.currentHealth -= damage;
            this.timeSinceLastHit = 0;
            this.hit = true;
        }
    }
}
