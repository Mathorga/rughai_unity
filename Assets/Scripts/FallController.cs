using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallController : MonoBehaviour {
    public bool falling {
        get;
        set;
    }

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    public void SetGravity(bool on) {
        this.rb.gravityScale = 2f;
    }

    public void SetSortingLayer(string layerName) {
        this.spriteRenderer.sortingLayerName = layerName;
    }
}
