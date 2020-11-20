using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DepthController : MonoBehaviour {
    void Update() {
        this.GetComponent<Renderer>().sortingOrder = (int) (this.transform.position.y * -22);
    }
}
