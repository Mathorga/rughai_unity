using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionInteraction : Interaction {
    public string nextScene;
    public Animator sceneAnimator;
    public Vector2 nextPlayerPosition;
    public Vector2Value playerPosition;
    public Vector2 nextPlayerFacing;
    public Vector2Value playerFacing;

    void FixedUpdate() {
        if (this.run) {
            // Interact.
            this.startInteractionAction();

            this.playerPosition.value = this.nextPlayerPosition;
            this.playerFacing.value = this.nextPlayerFacing;
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
