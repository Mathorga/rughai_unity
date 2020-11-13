using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public float speed;
    public MovementInput input;
    public Animator animator;

    private Rigidbody2D rb;
    private Vector2 moveVector;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        this.moveVector = this.input.GetMovement();
        this.Animate();
    }

    void FixedUpdate() {
        this.rb.AddForce(this.moveVector * this.speed * 100 * Time.deltaTime);
    }

    void Animate() {
        // Set facing for direction control.
        this.animator.SetFloat("FaceX", this.moveVector.x);
        this.animator.SetFloat("FaceY", this.moveVector.y);

        // Set animation state.
        if (this.moveVector.magnitude == 0) {
            this.animator.Play("Stand");
        } else {
            this.animator.Play("Walk");
        }
    }
}