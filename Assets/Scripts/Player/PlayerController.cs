using UnityEngine;

public class PlayerController : MonoBehaviour {
    public Vector2Value startPosition;
    public PlayerStats stats;

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
        if (this.stats.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.stats.moveForce);
        }
    }

    void ComputeForce() {
        if (this.input.moveLen <= this.walkThreshold) {
            this.stats.moveSpeed = 0f;
        } else {
            if (this.input.moveLen < this.runThreshold ||
                this.input.walk) {
                this.stats.moveSpeed = this.stats.walkSpeed;
            } else {
                this.stats.moveSpeed = this.stats.speed;
            }
        }

        this.stats.moveForce = Utils.PolarToCartesian(this.input.moveDir, this.stats.moveSpeed);
    }
}