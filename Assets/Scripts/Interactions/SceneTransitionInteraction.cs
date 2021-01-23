using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionInteraction : Interaction {
    public string nextScene;
    public Animator sceneAnimator;
    public Vector2 nextPlayerPosition;
    public Vector2Value playerPosition;

    void FixedUpdate() {
        if (this.run) {
            // Interact.
            Debug.Log("Anselmoide Anemocomoro " + this.gameObject.ToString());

            //TODO Disable input.

            this.playerPosition.value = this.nextPlayerPosition;
            this.StartCoroutine(this.LoadScene());

            // Reset trigger after interaction is complete.
            this.run = false;
        }
    }

    IEnumerator LoadScene() {
        this.sceneAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(this.nextScene);
    }
}
