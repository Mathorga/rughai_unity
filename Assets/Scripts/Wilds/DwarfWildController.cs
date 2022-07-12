using System.Collections;
using UnityEngine;

public class DwarfWildController : MonoBehaviour, IWild {
    public IWild.State state {
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

    private HitController hitController;
    private ChaserController pathfinder;
    private CapsuleCollider2D capsCollider;
    private WildStats stats;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
        this.hitController = this.GetComponent<HitController>();
        this.pathfinder = this.GetComponent<ChaserController>();
        this.capsCollider = this.GetComponent<CapsuleCollider2D>();
        this.active = true;
        this.state = IWild.State.Idle;
        this.stats = this.GetComponent<WildStats>();
    }

    void Update() {
        // Retrieve max velocity based on current speed and linear drag.
        float maxVelocity = this.stats.speed / this.rb.drag;

        // Fetch state.
        if (this.state != IWild.State.Dead) {
            this.state = IWild.State.Idle;
        }
    }

    void FixedUpdate() {
        this.ComputeForce();

        if (this.moveForce.magnitude > 0f) {
            this.rb.AddForce(this.moveForce);
        }
    }

    void ComputeForce() {
        switch (this.state) {
            case IWild.State.Idle:
                if (this.active) {
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
                break;
            case IWild.State.Chase:
                if (this.pathfinder != null) {
                    // Compute length and direction based on the next computed pathfinding step.
                    Vector2 position = this.transform.position;

                    float dir = Utils.AngleBetween(position, this.pathfinder.nextStep);

                    this.moveSpeed = this.stats.walkSpeed;

                    // Set move force based on computed move speed.
                    this.moveForce = Utils.PolarToCartesian(dir, this.moveSpeed);
                }
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
        this.state = IWild.State.Dead;
    }

    IEnumerator Activate() {
        this.active = false;
        yield return new WaitForSeconds(1f);
        this.active = true;
    }
}
