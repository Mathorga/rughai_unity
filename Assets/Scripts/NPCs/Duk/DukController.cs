using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DukController : MonoBehaviour {
    public Stats stats;
    public float moveSpeed {
        get;
        private set;
    }
    public Vector2 moveForce {
        get;
        private set;
    }

    private Rigidbody2D rb;
    private bool active;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.active = false;
    }

    void FixedUpdate() {
        this.ComputeForce();
        if (this.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.moveForce);
        }
    }

    void ComputeForce() {
        if (!this.active) {
            // Get random length and direction.
            float randomLen = Random.value;
            float randomDir = Random.Range(0f, 360f);

            if (randomLen < 0.9) {
                this.moveSpeed = 0f;
            } else {
                this.moveSpeed = this.stats.walkSpeed;
            }

            // Set move force based on computed move speed.
            this.moveForce = Utils.PolarToCartesian(randomDir, this.moveSpeed);

            // Wait.
            this.StartCoroutine(this.Activate());
        }
    }

    IEnumerator Activate() {
        this.active = true;
        yield return new WaitForSeconds(1f);
        this.active = false;
    }
}
