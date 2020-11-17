using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerController : MonoBehaviour {
    void OnTriggerEnter2D(Collider2D other) {
        // other.transform.position = this.transform.position;
        if (other.tag == "CameraTarget") {
            other.GetComponent<CameraTargetController>().ToggleBlocked();
            other.transform.position = this.transform.position;
        }
        Debug.Log("Enter " + other.tag);
    }

    void OnTriggerStay2D(Collider2D other) {
        // Debug.Log("Stay " + other);
    }

    void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Exit " + other.gameObject.name);
    }
    
}
