using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour {
    private bool falling;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("FallTrigger")) {
            // Check if current position is inside the trigger.
            if (Utils.PointInsideCollider(other, this.transform.position)) {
                // Only set flag if not alredy set.
                if (!this.falling) {
                    this.rb.gravityScale = 2f;
                    this.spriteRenderer.sortingLayerName = "Fall";
                    this.falling = true;
                }
            }
        }
    }

    public bool isFalling() {
        return this.falling;
    }
}
