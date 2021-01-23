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
    private bool interacted = false;

    void Start() {
        this.interactionCollider = this.GetComponent<CircleCollider2D>();
        this.input = this.GetComponent<PlayerInput>();
    }

    void FixedUpdate() {
        if (this.input.moveLen > 0.0f) {
            this.interactionCollider.offset = Utils.PolarToCartesian(this.input.moveDir, this.interactionOffset);
        }

        if (this.otherTransform != null) {
            if (this.input.interact) {
                this.otherTransform.GetComponent<Interaction>().run = true;
                this.input.interact = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.isTrigger) {
            // Check if other has an interaction.
            if (other.GetComponent<Interaction>() != null) {
                if (this.interactionSign == null) {
                    Vector3 offset = other.offset + this.signPositionOffset;
                    this.otherTransform = other.transform;
                    this.interactionSign = Instantiate(this.interactionSignType, other.transform.position + offset, Quaternion.identity);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        //TODO Check if other is an interactable.
        if (other.isTrigger) {
            this.otherTransform = null;
            Destroy(this.interactionSign);
        }
    }
}
