using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthController : MonoBehaviour {
    void FixedUpdate() {
        this.GetComponent<Renderer>().sortingOrder = (int) (this.transform.position.y * -22);
    }
}
