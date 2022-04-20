using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitController : MonoBehaviour {
    public Stats stats;
    private float currentHealth;
    public int immuneTime;

    private int timeSinceLastHit;

    void Start() {
        this.currentHealth = this.stats.health;
    }

    void FixedUpdate() {
        if (this.currentHealth <= 0) {
            // Die.
            Destroy(this.gameObject);
        }

        this.timeSinceLastHit += 1;
    }

    public void takeDamage(float damage) {
        if (this.timeSinceLastHit > this.immuneTime) {
            Debug.Log("Accidenti, m'hanno danneggiato");
            this.currentHealth -= damage;
            this.timeSinceLastHit = 0;
        }
    }
}
