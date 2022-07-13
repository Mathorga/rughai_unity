using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {
    public int immuneTime;

    private float currentHealth;
    private int timeSinceLastHit;

    private Rigidbody2D rb;
    private AudioSource audioSource;
    private WildStats stats;

    // Used to make the current object die.
    private ILiving controller;

    public bool hit {
        get;
        private set;
    }

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.audioSource = this.GetComponent<AudioSource>();
        this.stats = this.GetComponent<WildStats>();

        this.controller = this.GetComponent<ILiving>();
        this.hit = false;

        this.currentHealth = this.stats.health;
    }

    void Update() {
        if (this.currentHealth <= 0) {
            // Die.
            this.controller.Die();
            // Destroy(this.gameObject);
        }

        if (this.timeSinceLastHit <= this.immuneTime) {
            this.timeSinceLastHit += 1;
        } else {
            this.hit = false;
        }
    }

    public void takeDamage(Transform source, float damage) {
        if (!this.hit) {
            // Knockback.
            float dir = Utils.AngleBetween(source.position, this.transform.position);
            float len = 20.0f;
            Vector2 force = Utils.PolarToCartesian(dir, len);
            this.rb.AddForce(force, ForceMode2D.Impulse);

            // Actual health damage.
            this.currentHealth -= damage;

            this.timeSinceLastHit = 0;
            this.hit = true;
        }
    }
}
