using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public float speed;
    public Transform target;

    private float zPosition = -10f;

    void Start() {
        this.transform.position = new Vector3(this.target.position.x, this.target.position.y, this.zPosition);
    }

    void FixedUpdate() {
        float dir = Utils.AngleBetween(this.transform.position, this.target.position);
        float len = Vector2.Distance(this.transform.position, this.target.position) * this.speed;

        Vector2 pos2D = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);
        this.transform.position = new Vector3(pos2D.x, pos2D.y, this.zPosition);
    }
}
