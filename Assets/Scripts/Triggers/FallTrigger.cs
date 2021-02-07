using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {
    public Animator sceneAnimator;
    private BoxCollider2D boxCollider;

    void Start() {
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        // If other is not a capsule collider it's not taken into account.
        if (other is CapsuleCollider2D) {
            // Retrieve other's fall controller.
            Transform otherTransform = other.gameObject.transform;
            FallController otherController = other.gameObject.GetComponent<FallController>();

            if (otherController != null) {
                if (!otherController.isFalling) {
                    otherController.safePosition = otherTransform.position;
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        // If other is not a capsule collider it's not taken into account.
        if (other is CapsuleCollider2D) {
            // Retrieve other's fall controller.
            Transform otherTransform = other.gameObject.transform;
            FallController otherController = other.gameObject.GetComponent<FallController>();

            if (otherController != null) {
                Vector2 fallingPosition = (Vector2) otherTransform.position + other.offset;
                // Check if other's position is inside collider.
                if (!otherController.isFalling &&
                    Utils.PointInsideCollider(this.boxCollider, fallingPosition)) {
                    otherController.SetFalling(true);
                }

                if (otherController.isFalling) {
                    // Check if the whole body is in a fall collider.
                    // Retrieve layermask to filter raycast.
                    LayerMask mask = LayerMask.GetMask("FallTrigger");

                    // First check for collision at height.
                    RaycastHit2D topHit = Physics2D.Raycast(new Vector2(otherTransform.position.x, otherTransform.position.y + otherController.height),
                                                            Vector2.up,
                                                            0.0f,
                                                            mask);

                    // Then check for collision at half height.
                    RaycastHit2D midHit = Physics2D.Raycast(new Vector2(otherTransform.position.x, otherTransform.position.y + (otherController.height / 2)),
                                                            Vector2.up,
                                                            0.0f,
                                                            mask);

                    if (topHit.collider != null && midHit.collider != null) {
                        if (topHit.collider.gameObject.tag == "FallTrigger" && midHit.collider.gameObject.tag == "FallTrigger") {
                            otherController.SetSortingLayer("Fall");
                        }
                    }
                }
            }
        }
    }
}
