using UnityEngine;

public class Parallax2D : MonoBehaviour {
    public Transform target;
    public float speed;

    void FixedUpdate() {
        this.Move();
    }

    void Move() {
        for (int i = 0; i < this.transform.childCount; i++) {
            // Get child by its name.
            Transform childTransform = this.transform.Find(i.ToString());

            childTransform.position = this.transform.position + (this.target.position * (((-i) * this.speed) + 1));
        }
    }
}
