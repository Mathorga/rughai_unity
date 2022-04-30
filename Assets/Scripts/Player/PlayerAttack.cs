using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private PlayerController controller;
    private AudioSource audioSource;

    public LayerMask hitLayer;
    public AudioClip swingSound;

    public float extent;

    // The y-axis offset applied to the hitbox.
    public int elevation;

    // The camera, used to add screen shake.
    public CameraShake cameraShake;

    public float shakeDuration;
    public float shakeMagnitude;
    public float shakeFade;

    private PlayerController.State currentState;

    void Start() {
        this.controller = this.GetComponent<PlayerController>();
        this.audioSource = this.GetComponent<AudioSource>();
        this.currentState = this.controller.state;
    }

    void FixedUpdate() {
        this.PlaySound();

        if ((this.controller.state == PlayerController.State.Atk0 ||
            this.controller.state == PlayerController.State.Atk1 ||
            this.controller.state == PlayerController.State.Atk2) &&
            this.controller.animationProgress > 0.4f &&
            this.controller.animationProgress < 0.6f) {
            this.Attack();
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
    void PlaySound() {
        if (this.controller.state == PlayerController.State.Atk0 ||
            this.controller.state == PlayerController.State.Atk1) {
            if (this.controller.state != this.currentState) {
                // Play attack sound.
                // this.audioSource.clip = this.swingSound;
                this.audioSource.Play();

                // Update current state.
                this.currentState = this.controller.state;
            }
        } else if (this.controller.state == PlayerController.State.Atk2) {
            if (this.controller.state != this.currentState) {
                // Play attack sound.
                // this.audioSource.clip = this.swingSound;
                this.audioSource.PlayDelayed(0.2f);

                // Update current state.
                this.currentState = this.controller.state;
            }
        } else {
            if (this.controller.state != this.currentState) {
                // Update current state.
                this.currentState = this.controller.state;
            }
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
        if (this.cameraShake != null) {
            this.cameraShake.Shake(this.shakeDuration * hits.Length, this.shakeMagnitude * hits.Length, this.shakeFade * hits.Length);
        }

        // Loop through hits.
        foreach (Collider2D hit in hits) {
            // TODO Damage.
            HitController hitController = hit.GetComponent<HitController>();
            if (hitController != null) {
                hitController.takeDamage(this.transform, 1);
            }
        }
    }
}
