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
                Vector2 fallingPosition = (Vector2) otherTransform.position + other.offset;
                // Check if other's position is inside collider.
                if (!otherController.falling &&
                    Utils.PointInsideCollider(this.boxCollider, fallingPosition)) {
                    otherController.SetFalling();
                }

                if (otherController.falling &&
                    Utils.PointInsideCollider(this.boxCollider, new Vector2(fallingPosition.x, fallingPosition.y + otherController.height))) {
                    otherController.SetSortingLayer("Fall");
                }
            }
        }
    }
}
