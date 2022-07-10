using System.Collections;
using UnityEngine;

public class DukController : MonoBehaviour, ILiving {
    public enum State {
        Idle,
        Chase,
        Dead
    };

    public State state {
        get;
        set;
    }
    public float moveSpeed {
        get;
        private set;
    }
    public Vector2 moveForce {
        get;
        private set;
    }

    public bool moving {
        get;
        set;
    }

    private Rigidbody2D rb;
    public bool active {
        get;
        set;
    }
    private ChaserController pathfinder;
    private CapsuleCollider2D capsCollider;
    private WildStats stats;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.pathfinder = this.GetComponent<ChaserController>();
        this.capsCollider = this.GetComponent<CapsuleCollider2D>();
        this.active = false;
        this.state = State.Idle;
        this.stats = this.GetComponent<WildStats>();
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

    void ILiving.Die() {
        // Stop any movement.
        this.rb.velocity = new Vector2(0.0f, 0.0f);

        // Disable collider.
        this.capsCollider.enabled = false;

        // Set state.
        this.state = State.Dead;
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
