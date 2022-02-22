using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour {
    public int extension;
    public Transform anchor;
    public CameraInput input;
    public Vector2 offset;
    public Vector2Value startPosition;

    void Awake() {
        // It's important to place to a given position instead of to anchor in order not to mess with event order.
        this.transform.position = this.startPosition.value + this.offset;
    }

    void FixedUpdate() {
        this.transform.position = (Vector2) this.anchor.transform.position + Utils.PolarToCartesian(this.input.moveDir, this.input.moveLen * this.extension) + this.offset;
    }
}
