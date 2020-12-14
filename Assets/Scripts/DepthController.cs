using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthController : MonoBehaviour {
    void Update() {
        Renderer renderer = this.GetComponent<Renderer>();
        SpriteMask mask = this.GetComponent<SpriteMask>();

        int depth = (int) (this.transform.position.y * -22);

        if (renderer != null) {
            renderer.sortingOrder = depth;
        }
        if (mask != null) {
            // mask.sortingOrder = (int) (this.transform.position.y * -22);
            mask.isCustomRangeActive = true;
            mask.backSortingOrder = depth - 1000;
            mask.frontSortingOrder = depth - 1;
        }
    }
}
