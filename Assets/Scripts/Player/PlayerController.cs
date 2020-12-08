using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Vector2Value startPosition;
    public PlayerStats stats;
    public float moveSpeed {
        get;
        private set;
    }
    public Vector2 moveForce {
        get;
        private set;
    }

    private float walkThreshold = 0.1f;
    private float runThreshold  = 0.6f;
    private Rigidbody2D rb;
    private PlayerInput input;

    void Awake() {
        this.transform.position = this.startPosition.value;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.input = this.GetComponent<PlayerInput>();
    }

    void FixedUpdate() {
        this.ComputeForce();
        if (this.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.moveForce);
        }
    }

    void ComputeForce() {
        if (this.input.moveLen <= this.walkThreshold) {
            this.moveSpeed = 0f;
        } else {
            if (this.input.moveLen < this.runThreshold ||
                this.input.walk) {
                this.moveSpeed = this.stats.walkSpeed;
            } else {
                this.moveSpeed = this.stats.speed;
            }
        }

        this.moveForce = Utils.PolarToCartesian(this.input.moveDir, this.moveSpeed);
    }
}