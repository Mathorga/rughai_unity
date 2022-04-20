using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
    private PlayerController controller;
    public float extent;

    // The y-axis offset applied to the hitbox.
    public int elevation;

    void Start() {
        this.controller = this.GetComponent<PlayerController>();
    }

    void FixedUpdate() {
        if ((this.controller.state == PlayerController.State.Atk0 ||
            this.controller.state == PlayerController.State.Atk1 ||
            this.controller.state == PlayerController.State.Atk2) &&
            this.controller.animationProgress > 0.4f &&
            this.controller.animationProgress < 0.6f) {

            // Calculate the application point of the attack.
            float dir = this.controller.faceDir;
            float len = this.extent;

            Vector2 pos2D = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);
            pos2D[1] += this.elevation;

            // Check what was hit by the attack.
            Collider2D[] hits = Physics2D.OverlapCircleAll(pos2D, this.extent);

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

}
