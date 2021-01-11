using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2D : MonoBehaviour {
    public Transform target;
    public Vector2 offset;
    public float speed;
    public bool absolute;

    void Start() {
        this.transform.position = (Vector2) this.target.position;
    }

    void FixedUpdate() {
        if (this.absolute) {
            this.MoveAbsolute();
        } else {
            this.MoveRelative();
        }
    }

    void MoveRelative() {
        Transform anchor = this.target.GetComponent<CameraTargetController>().anchor;

        float dir = Utils.AngleBetween(this.transform.position, anchor.position);
        float len = Vector2.Distance(this.transform.position, anchor.position) * this.speed;
        this.transform.position = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len) + this.offset;

        Vector2 anchorOffset = (Vector2) anchor.position - (Vector2) this.target.position;

        for (int i = 0; i < this.transform.childCount; i++) {
            // Get child by its name.
            Transform childTransform = this.transform.Find(i.ToString());

            dir = Utils.AngleBetween(Vector2.zero, anchorOffset);
            len = (Vector2.Distance(Vector2.zero, anchorOffset) + (anchorOffset.magnitude * (this.transform.childCount - i * 2f))) * this.speed * 0.5f;

            childTransform.position = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);
        }
    }

    void MoveAbsolute() {
        for (int i = 0; i < this.transform.childCount; i++) {
            // Get child by its name.
            Transform childTransform = this.transform.Find(i.ToString());

            childTransform.position = this.target.position * ((i * this.speed) + 1);
        }
    }
}
