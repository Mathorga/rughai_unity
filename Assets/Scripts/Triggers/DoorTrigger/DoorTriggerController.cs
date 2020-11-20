using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorTriggerController : MonoBehaviour {
    public string nextScene;

    void OnTriggerEnter2D(Collider2D other) {
        // other.transform.position = this.transform.position;
        if (other.CompareTag("Player")) {
            //TODO Scene transition.
            this.StartCoroutine(this.LoadScene());
            // SceneManager.LoadScene(this.nextScene);
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        // Debug.Log("Stay " + other);
    }

    void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Exit " + other.gameObject.name);
    }
    
    IEnumerator LoadScene() {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(this.nextScene);
    }
}
