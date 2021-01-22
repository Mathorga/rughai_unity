using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthController : MonoBehaviour {
    public bool rooted = false;
    public int offset = 0;

    private Renderer drawer;
    private SpriteMask mask;

    public void SetRooted(bool rooted) {
        this.rooted = rooted;
    }

    void Start() {
        this.drawer = this.GetComponent<Renderer>();
        this.mask = this.GetComponent<SpriteMask>();
        this.SetDepth();
    }

    void FixedUpdate() {
        if (!this.rooted) {
            this.SetDepth();
        }
    }

    private void SetDepth() {
        int depth = (int) (this.transform.position.y * -22);

        if (this.drawer != null) {
            this.drawer.sortingOrder = depth + this.offset;
        }
        if (this.mask != null) {
            // mask.sortingOrder = (int) (this.transform.position.y * -22);
            this.mask.isCustomRangeActive = true;
            this.mask.backSortingOrder = (depth + this.offset) - 1000;
            this.mask.frontSortingOrder = depth + this.offset;
        }
    }
}
