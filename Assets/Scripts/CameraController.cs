using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;
    public float speed;

    void Start() {
        this.transform.position = this.target.position;
    }

    void FixedUpdate() {
        float dir = Vector2Converter.AngleBetween(this.transform.position, this.target.position);
        float len = Vector2.Distance(this.transform.position, this.target.position) * this.speed;

        this.transform.position = (Vector2) this.transform.position + Vector2Converter.PolarToCartesian(dir, len);
    }
}
