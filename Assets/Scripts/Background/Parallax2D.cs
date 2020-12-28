using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax2D : MonoBehaviour {
    public Transform target;
    public float speed;

    void Start() {
        this.transform.position = this.target.position;
    }

    void FixedUpdate() {
        float dir = Utils.AngleBetween(this.transform.position, this.target.position);
        float len = Vector2.Distance(this.transform.position, this.target.position) * this.speed;
        this.transform.position = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);

        Transform anchor = this.target.GetComponent<CameraTargetController>().anchor;
        Vector2 offset = (Vector2) anchor.position - (Vector2) this.target.position;

        for (int i = 0; i < this.transform.childCount; i++) {
            Transform childTransform = this.transform.Find(i.ToString());
            // Vector2 targetPosition = new Vector2();
            // targetPosition.x = this.transform.position.x;
            // targetPosition.y = (this.transform.position.y) + (offset.y * ((this.transform.childCount - i) * 0.1f));
            // childTransform.position = targetPosition;

            // Vector2 childPosition = new Vector2();
            // childPosition.x = targetPosition.x;
            // childPosition.y = this.transform.position.y + targetPosition.y;



            dir = Utils.AngleBetween(Vector2.zero, offset);
            len = (Vector2.Distance(Vector2.zero, offset) + (offset.magnitude * (this.transform.childCount - i))) * this.speed * 0.5f;

            childTransform.position = (Vector2) this.transform.position + Utils.PolarToCartesian(dir, len);
        }
    }
}
