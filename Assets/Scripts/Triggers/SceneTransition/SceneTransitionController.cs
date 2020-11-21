using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour {
    public string nextScene;
    public Animator sceneAnimator;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            this.StartCoroutine(this.LoadScene());
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        // Debug.Log("Stay " + other);
    }

    void OnTriggerExit2D(Collider2D other) {
        // Debug.Log("Exit " + other.gameObject.name);
    }
    
    IEnumerator LoadScene() {
        this.sceneAnimator.SetTrigger("FadeOut");
        //this.sceneAnimator.Play("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(this.nextScene);
    }
}
