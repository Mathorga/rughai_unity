using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeController : MonoBehaviour {
    private List<GameObject> children;

    void Start() {
        // Retrieve all children objects.
        foreach (Transform child in transform) {
            this.children.Add(child.gameObject);
        }
    }
}
