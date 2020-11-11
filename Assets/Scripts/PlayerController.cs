using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;
    public float threshold;

    private Rigidbody2D rb;
    private Vector2 moveVector;
    private float magnitude;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        // float hor = Input.GetAxisRaw("Horizontal") * Time.deltaTime;
        // float ver = Input.GetAxisRaw("Vertical") * Time.deltaTime;
        float hor = Input.GetAxisRaw("Horizontal");
        float ver = Input.GetAxisRaw("Vertical");

        float moveSpeed = this.speed;

        Vector2 moveInput = new Vector2(hor, ver);

        // Magnitude is actually greater if both hor and ver are 1 or -1.
        this.magnitude = moveInput.magnitude;

        if (this.magnitude < this.threshold) {
            hor = 0;
            ver = 0;
            moveSpeed = 0;
        }

        float dir = this.AngleBetween(Vector2.zero, moveInput);
        // Debug.Log(dir);

        this.moveVector = this.PolarToCartesian(dir * (Mathf.PI / 180), moveSpeed);
        // Debug.Log(this.moveVector.x);
        // Debug.Log(this.moveVector.y);
    }

    void FixedUpdate() {
        if (this.magnitude > this.threshold) {
            this.rb.AddForce(this.moveVector * 100 * Time.deltaTime);
        }
    }

    Vector2 PolarToCartesian(float angle, float magnitude) {
        Debug.Log(angle);
        Debug.Log(Mathf.Cos(angle));
        Debug.Log(Mathf.Sin(angle));
        return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * magnitude;
    }
    // Vector2 CartesianToPolar(Vector2 cartesian) {
    //     float angle = Mathf.Atan(cartesian.y / cartesian.x);
    //     float magnitude = Mathf.Sqrt(Mathf.Pow(cartesian.x, 2) + Mathf.Pow(cartesian.y, 2));
    //     return new Vector2(angle, magnitude);
    // }
    float AngleBetween(Vector2 v1, Vector2 v2) {
     return Mathf.Atan2(v2.y - v1.y, v2.x - v1.x) * (180 / Mathf.PI);
    }
}
