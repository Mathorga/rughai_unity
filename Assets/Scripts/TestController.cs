using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {
    private bool visible;
    
    void OnBecameVisible() {
        this.visible = true;
    }

    void OnBecameInvisible() {
        this.visible = false;
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (this.visible) {
            Debug.Log("Visible!");
        } else {
            Debug.Log("Invisible!");
        }
    }
}
