using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {
    public Animator sceneAnimator;
    private BoxCollider2D boxCollider;

    void Start() {
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other) {
        // If other collider is a trigger it's not taken into account.
        if (!other.isTrigger) {
            // Retrieve other's fall controller.
            Transform otherTransform = other.gameObject.transform;
            FallController otherController = other.gameObject.GetComponent<FallController>();

            if (otherController != null) {
                // Check if other's position is inside collider.
                if (Utils.PointInsideCollider(this.boxCollider, (Vector2) otherTransform.position + other.offset)) {
                    // otherController.SetSortingLayer("GroundTiles");
                    otherController.SetFalling();
                    // if (other.GetType() == typeof(CapsuleCollider2D)) {
                    //     otherController.SetCapsuleHit();
                    // } else if (other.GetType() == typeof(CircleCollider2D)) {
                    //     otherController.SetCircleHit();
                    // }
                }
            }
        }
    }
}
