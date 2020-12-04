using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour {
    public bool falling {
        get;
        set;
    }

    private bool circleHit;
    private bool capsuleHit;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void SetGravity() {
        this.rb.gravityScale = 2f;
    }

    public void SetCircleHit() {
        this.circleHit = true;
        
        if (this.capsuleHit) {
            this.SetSortingLayer("Fall");
        }
    }

    public void SetCapsuleHit() {
        this.capsuleHit = true;
        
        if (!this.falling) {
            this.SetGravity();
            this.falling = true;
        }

        if (this.circleHit) {
            this.SetSortingLayer("Fall");
        }
    }

    // public void SetFalling() {
    //     // Only set flag if not alredy set.
    //     if (!this.falling) {
    //         this.SetGravity();
    //         this.falling = true;
    //     }
    //     if (!this.falling) {
    //         this.falling = true;
    //         this.SetGravity(true);
    //         if (this.capsuleHit && this.circleHit) {
    //             this.SetSortingLayer("Fall");
    //         }
    //     }
    // }

    private void SetSortingLayer(string layerName) {
        this.spriteRenderer.sortingLayerName = layerName;
    }
}
