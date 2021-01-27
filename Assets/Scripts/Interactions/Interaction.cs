using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour {
    // Flag variable to run interaction routine.
    public bool run {
        get;
        set;
    }

    // Callback Actions.
    public Action startInteractionAction;

    void Start() {
        // Set trigger flag to false by default.
        // It has to be manually activated by interactor.
        this.run = false;
    }
}
