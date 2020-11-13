using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    public float speed;
    public MovementInput input;

    private Rigidbody2D rb;
    private Vector2 moveVector;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        this.moveVector = this.input.GetMovement();
    }

    void FixedUpdate() {
        this.rb.AddForce(this.moveVector * this.speed * 100 * Time.deltaTime);
    }
}