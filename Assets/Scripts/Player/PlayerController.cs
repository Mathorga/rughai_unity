using UnityEngine;

public class PlayerController : MonoBehaviour {
    public enum State {
        Idle,
        Walk,
        Run,
        Fall,
        Attack0,
        Attack1
    }

    public Vector2Value startPosition;
    public PlayerStats stats;
    public float moveSpeed {
        get;
        private set;
    }
    public float faceDir {
        get;
        private set;
    }
    public Vector2 moveForce {
        get;
        private set;
    }
    public State state {
        get;
        private set;
    }

    private float walkThreshold = 0.1f;
    private float runThreshold  = 0.6f;
    private Rigidbody2D rb;
    private PlayerInput input;

    public void SetState(State state) {
        this.state = state;
    }

    void Awake() {
        this.transform.position = this.startPosition.value;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.input = this.GetComponent<PlayerInput>();
        this.state = State.Idle;
    }

    void FixedUpdate() {
        // Set state based on input.
        this.SetState();

        // Compute the amount of force to apply to apply to self.
        this.ComputeForce();

        // Actually apply the computed force if greater than 0.
        if (this.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.moveForce);
        }
    }

    private void SetState() {
        if (this.state != State.Attack0 &&
            this.state != State.Attack1 &&
            this.state != State.Fall) {
            if (this.input.attack) {
                this.state = State.Attack0;
            } else {
                if (this.input.moveLen <= this.walkThreshold) {
                    this.state = State.Idle;
                } else {
                    if (this.input.moveLen < this.runThreshold ||
                        this.input.walk) {
                        this.state = State.Walk;
                    } else {
                        this.state = State.Run;
                    }
                }
            }
        }
    }

    private void ComputeForce() {
        if (this.state == State.Attack0) {
            this.moveSpeed = this.stats.walkSpeed / 2;
        } else {
            if (this.state == State.Idle) {
                this.moveSpeed = 0f;
            } else {
                if (this.state == State.Walk) {
                    this.moveSpeed = this.stats.walkSpeed;
                } else {
                    this.moveSpeed = this.stats.speed;
                }
                this.faceDir = this.input.moveDir;
            }
        }

        this.moveForce = Utils.PolarToCartesian(this.faceDir, this.moveSpeed);
    }
}