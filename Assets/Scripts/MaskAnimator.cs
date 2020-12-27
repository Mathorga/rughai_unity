using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskAnimator : MonoBehaviour {
    private SpriteRenderer spriteRenderer;
    private SpriteMask spriteMask;

    void Start() {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.spriteMask = this.GetComponent<SpriteMask>();
    }

    void FixedUpdate() {
        this.spriteMask.sprite = this.spriteRenderer.sprite;
    }
}
