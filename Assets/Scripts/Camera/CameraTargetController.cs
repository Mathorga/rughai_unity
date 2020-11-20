using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour {
    public float extension;
    public Transform pole;
    public CameraInput input;
    public Vector2 offset;

    void Start() {
        this.transform.position = this.pole.position;
    }

    void FixedUpdate() {
        this.transform.position = (Vector2) this.pole.transform.position + Utils.PolarToCartesian(this.input.moveDir, this.input.moveLen * this.extension) +  this.offset;
    }
}
