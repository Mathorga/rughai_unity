using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour {
    public bool isFalling {
        get;
        set;
    }
    public Vector2 safePosition {
        get;
        set;
    }

    public float height;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private DepthController depthController;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.depthController = this.GetComponent<DepthController>();
    }

    public void SetFalling(bool falling) {
        if (falling) {
            // Set falling trigger.
            this.isFalling = true;

            // Cut current velocity.
            this.rb.velocity /= 2.0f;

            // Start gravity.
            this.rb.gravityScale = 1;

            // Cut drag.
            this.rb.drag /= 4;

            // Disable collider.
            this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;

            // Restore fall position.
            this.StartCoroutine(this.RestoreSafePosition());
        } else {
            // Set falling trigger.
            this.isFalling = false;

            // Cut current velocity.
            this.rb.velocity = Vector2.zero;

            // Start gravity.
            this.rb.gravityScale = 0;

            // Cut drag.
            this.rb.drag *= 4;

            // Disable collider.
            this.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
        }
    }

    public void SetSortingLayer(string sortingLayerName) {
        this.spriteRenderer.sortingLayerName = sortingLayerName;
    }

    IEnumerator RestoreSafePosition() {
        yield return new WaitForSeconds(1f);

        // Restore safe position.
        this.transform.position = this.safePosition;

        // Restore sorting layer.
        this.SetSortingLayer("Default");

        // Turn falling off.
        this.SetFalling(false);
    }
}
