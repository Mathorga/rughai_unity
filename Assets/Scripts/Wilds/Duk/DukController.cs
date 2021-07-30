using System.Collections;
using UnityEngine;

public class DukController : MonoBehaviour {
    public enum State {Idle, Chase};

    public Stats stats;
    public State state = State.Idle;
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
    private Pathfinder pathfinder;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.pathfinder = this.GetComponent<Pathfinder>();
        this.active = false;
        // this.state = State.Idle;
    }

    void FixedUpdate() {
        this.ComputeForce();
        if (this.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.moveForce);
        }
    }

    void ComputeForce() {
        switch (this.state) {
            case State.Idle:
                this.Idle();
                break;
            case State.Chase:
                this.Chase();
                break;
            default:
                break;
        }
    }

    void Idle() {
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

    void Chase() {
        if (this.pathfinder != null) {
            // Compute length and direction based on the next computed pathfinding step.
            Vector2 position = this.transform.position;

            float dir = Utils.AngleBetween(position, this.pathfinder.nextStep);

            this.moveSpeed = this.stats.walkSpeed;

            // Set move force based on computed move speed.
            this.moveForce = Utils.PolarToCartesian(dir, this.moveSpeed);
        }
    }
}
