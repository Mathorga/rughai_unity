using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private PlayerController controller;
    private AudioSource audioSource;

    public LayerMask hitLayer;
    public AudioClip swingSound;
    public AudioClip hitSound;

    public float extent;

    // The y-axis offset applied to the hitbox.
    public int elevation;

    // The camera, used to add screen shake.
    public CameraShake cameraShake;

    public float shakeDuration;
    public float shakeMagnitude;
    public float shakeFade;

    private PlayerController.State currentState;

    private bool hitPlayed;

    void Start() {
        this.controller = this.GetComponent<PlayerController>();
        this.audioSource = this.GetComponent<AudioSource>();
        this.currentState = this.controller.state;
        this.hitPlayed = false;
    }

    void FixedUpdate() {
        this.PlaySwing();

        if ((this.controller.InAnimation("Atk0") ||
            this.controller.InAnimation("Atk1") ||
            this.controller.InAnimation("Atk2")) &&
            this.controller.GetAnimationProgress() > 0.4f &&
            this.controller.GetAnimationProgress() < 0.6f) {
            this.Attack();
        } else {
            this.hitPlayed = false;
        }
    }

    void OnDrawGizmos() {
        if (Application.isPlaying) {
            // Calculate the application point of the attack.
            float dir = this.controller.faceDir;
            float len = this.extent;

            Vector2 pos2D = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);
            pos2D[1] += this.elevation;

            Gizmos.color = new Color(1.0f, 1.0f, 0.0f, 0.5f);
            Gizmos.DrawWireSphere(pos2D, len);
        }
    }

    // Plays the attack sound according to state.
    void PlaySwing() {
        if (this.controller.state != this.currentState) {
            if (this.controller.state == PlayerController.State.Atk0 ||
                this.controller.state == PlayerController.State.Atk1) {
                // Play attack sound.
                // this.audioSource.clip = this.swingSound;
                this.audioSource.Play();
            } else if (this.controller.state == PlayerController.State.Atk2) {
                // Play attack sound.
                // this.audioSource.clip = this.swingSound;
                this.audioSource.PlayDelayed(0.2f);
            }
            // Update current state.
            this.currentState = this.controller.state;
        }
    }

    // Performs the attack.
    void Attack() {
        // Calculate the application point of the attack.
        float dir = this.controller.faceDir;
        float len = this.extent;

        // Compute attack position with elevation.
        Vector2 pos2D = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);
        pos2D[1] += this.elevation;

        // Check what was hit by the attack.
        Collider2D[] hits = Physics2D.OverlapCircleAll(pos2D, this.extent, this.hitLayer);

        // Only perform screen shake if a camera shake behavior is provided.
        if (this.cameraShake != null && hits.Length > 0) {
            this.cameraShake.Shake(this.shakeDuration, this.shakeMagnitude, this.shakeFade * hits.Length);
        }

        // Loop through hits.
        foreach (Collider2D hit in hits) {
            // TODO Damage.
            HitController hitController = hit.GetComponent<HitController>();
            if (hitController != null) {
                // Only play hit sound once.
                // This must go before the hit controller takes damage, otherwise controller.hit will always be true.
                if (!hitController.hit && !this.hitPlayed) {
                    this.audioSource.PlayOneShot(this.hitSound, 1.0f);
                    this.hitPlayed = true;
                }

                // Actually take damage.
                hitController.takeDamage(this.transform, 1);
            }
        }
    }
}
