using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVector;

    void Start() {
        this.rb = this.GetComponent<Rigidbody2D>();
    }

    void Update() {
        this.moveVector = MovementInput.GetMovement();
    }

    void FixedUpdate() {
        this.rb.AddForce(this.moveVector * this.speed * 100 * Time.deltaTime);
    }
}