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
    private PlayerStats stats;
    private FallController fallController;

    public void SetState(State state) {
        this.state = state;
    }

    void Awake() {
        this.transform.position = this.startPosition.value;
        this.rb = this.GetComponent<Rigidbody2D>();
        this.input = this.GetComponent<PlayerInput>();
        this.stats = this.GetComponent<PlayerStats>();
        this.fallController = this.GetComponent<FallController>();
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

        // Cut velocity if in a static state.
        if (this.state == State.Attack0 ||
            this.state == State.Fall) {
            this.rb.velocity = Vector2.zero;
        }
    }

    private void SetState() {
        if (this.fallController.falling) {
            this.input.Disable();
            this.state = State.Fall;
        } else {
            this.input.Enable();
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
    }

    private void ComputeForce() {
        if (this.state == State.Walk) {
            this.moveSpeed = this.stats.data.walkSpeed;
        } else if (this.state == State.Run) {
            this.moveSpeed = this.stats.data.speed;
        } else {
            this.moveSpeed = 0f;
        }
        this.faceDir = this.input.moveDir;

        this.moveForce = Utils.PolarToCartesian(this.faceDir, this.moveSpeed);
    }
}