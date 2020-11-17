using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTargetController : MonoBehaviour {
    public float extension;
    public Transform pole;
    public CameraInput input;

    private bool blocked;

    void Start() {
        this.transform.position = this.pole.position;
        this.blocked = false;
    }

    void FixedUpdate() {
        if (!this.blocked) {
            this.transform.position = (Vector2) this.pole.transform.position + Utils.PolarToCartesian(this.input.moveDir, this.input.moveLen * this.extension);
        }
    }

    void OnDrawGizmos() {
        Gizmos.DrawIcon(this.transform.position, "CameraTarget.png");
    }

    public void ToggleBlocked() {
        this.blocked = true;
    }
}
