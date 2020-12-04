using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {
    private BoxCollider2D boxCollider;

    void Start() {
        this.boxCollider = this.GetComponent<BoxCollider2D>();
    }

    void OnTriggerStay2D(Collider2D other) {
        // Retrieve other's fall controller.
        Transform otherTransform = other.gameObject.transform;
        FallController otherController = other.gameObject.GetComponent<FallController>();

        // Check if other's position is inside collider.
        if (Utils.PointInsideCollider(this.boxCollider, (Vector2) otherTransform.position + other.offset)) {
            if (other.GetType() == typeof(CapsuleCollider2D)) {
                otherController.SetCapsuleHit();
                // // Only set flag if not alredy set.
                // if (!otherController.falling) {
                //     otherController.SetGravity();
                //     otherController.falling = true;
                // }
            } else if (other.GetType() == typeof(CircleCollider2D)) {
                otherController.SetCircleHit();
            }
        }
    }

    // void OnTriggerExit2D(Collider2D other) {
    //     // Retrieve other's fall controller.
    //     Transform otherTransform = other.gameObject.transform;
    //     FallController otherController = other.gameObject.GetComponent<FallController>();

    //     if (other.GetType() == typeof(CapsuleCollider2D)) {
    //         otherController.SetCapsuleHit(false);
    //     } else if (other.GetType() == typeof(CircleCollider2D)) {
    //         otherController.SetCircleHit(false);
    //     }
    // }
}
