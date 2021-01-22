using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PlayerInteractor : MonoBehaviour {
    public float interactionOffset;
    public GameObject interactionSignType;

    private CircleCollider2D interactionCollider;
    private PlayerInput input;
    private Transform otherTransform;
    private GameObject interactionSign;

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
                // Instantiate(this.interactionSignType, this.transform.position, Quaternion.identity);
                // Debug.Log("Instantiated AF " + this.interactionSignType.ToString());
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        //TODO Check if other is an interactable.
        if (other.isTrigger) {
            if (this.interactionSign == null) {
                this.otherTransform = other.transform;
                Vector3 offset = new Vector3(other.offset.x, other.offset.y, 0.0f);
                this.interactionSign = Instantiate(this.interactionSignType, other.transform.position + offset, Quaternion.identity);
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
