using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2D : MonoBehaviour {
    public Transform target;
    public Vector2 offset;

    void Start() {
        this.transform.position = this.target.position;
    }

    void FixedUpdate() {
        this.transform.position = (Vector2) this.target.position + this.offset;

        Transform anchor = this.target.GetComponent<CameraTargetController>().anchor;
        Vector2 offset = (Vector2) anchor.position - (Vector2) this.target.position;

        for (int i = 0; i < this.transform.childCount; i++) {
            Transform childTransform = this.transform.Find(i.ToString());
            childTransform.position = ((Vector2) this.transform.position) + (offset * (i * 0.1f + 1));
        }
    }
}
