using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerInteractor : MonoBehaviour {
    public float interactionOffset;
    
    private CircleCollider2D interactionCollider;
    private PlayerInput input;

    void Start() {
        this.interactionCollider = this.GetComponent<CircleCollider2D>();
        this.input = this.GetComponent<PlayerInput>();
    }

    void Update() {
        if (this.input.moveLen > 0.0f) {
            this.interactionCollider.offset = Utils.PolarToCartesian(this.input.moveDir, this.interactionOffset);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        // Check if other is an interactable.
        if (other.isTrigger) {
            // Debug.Log("ENTERED!!!!!");
            Debug.Log(other.ToString());
        }
    }
}
