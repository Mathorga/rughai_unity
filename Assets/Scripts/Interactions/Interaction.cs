using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
    public bool run {
        get;
        set;
    }

    void Start() {
        // Set trigger flag to false by default.
        // It has to be manually activated.
        this.run = false;
    }
}
