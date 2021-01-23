using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionInteraction : Interaction {
    void FixedUpdate() {
        if (this.run) {
            // Interact.
            Debug.Log("Anselmoide Anemocomoro " + this.gameObject.ToString());

            // Reset trigger after interaction is complete.
            this.run = false;
        }
    }
}
