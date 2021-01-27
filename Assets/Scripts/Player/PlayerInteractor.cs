using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerInteractor : MonoBehaviour {
    public float interactionOffset;
    public GameObject interactionSignType;
    public Vector2 signPositionOffset;

    private CircleCollider2D interactionCollider;
    private PlayerInput input;
    private Transform otherTransform;
    private GameObject interactionSign;

    void Awake() {
        this.interactionCollider = this.GetComponent<CircleCollider2D>();
        this.input = this.GetComponent<PlayerInput>();
    }

    void FixedUpdate() {
        if (this.input.moveLen > 0.0f) {
            this.interactionCollider.offset = Utils.PolarToCartesian(this.input.moveDir, this.interactionOffset);
        }

        if (this.otherTransform != null) {
            if (this.input.interact) {
                // Retrieve interaction component from other.
                Interaction otherInteraction = this.otherTransform.GetComponent<Interaction>();

                // Enable interaction start.
                otherInteraction.run = true;

                // Register callback method for interaction start.
                otherInteraction.startInteractionAction += this.DisableInput;

                // Turn off interaction input, so that it's not triggered indefinitely.
                this.input.interact = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger) {
            // Check if other has an interaction.
            Interaction otherInteraction = other.GetComponent<Interaction>();
            if (otherInteraction != null) {
                if (this.interactionSign == null) {
                    Vector3 offset = other.offset + this.signPositionOffset;
                    this.otherTransform = other.transform;
                    this.interactionSign = Instantiate(this.interactionSignType, other.transform.position + offset, Quaternion.identity);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.isTrigger) {
            //TODO Check if other is an interactable.
            this.otherTransform = null;
            Destroy(this.interactionSign);
        }
    }

    void DisableInput() {
        this.input.SetMoveLen(0.0f);
        this.input.Disable();
    }
}
