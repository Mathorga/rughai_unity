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
    private DepthController depthController;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.depthController = this.GetComponent<DepthController>();
    }

    public void SetFalling() {
        this.falling = true;
    }
}
