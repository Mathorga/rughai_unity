using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour {
    public string nextScene;
    public Animator sceneAnimator;
    public Vector2 nextPlayerPosition;
    public Vector2Value playerPosition;

    // Movement direction after entering.
    public float dir;

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerStats playerStats = other.gameObject.GetComponent<PlayerStats>();
            PlayerInput playerInput = other.gameObject.GetComponent<PlayerInput>();
            playerInput.SetMoveDir(this.dir);
            playerInput.SetMoveLen(0.2f);
            playerInput.Disable();
            this.playerPosition.value = this.nextPlayerPosition;
            this.StartCoroutine(this.LoadScene());
        }
    }

    IEnumerator LoadScene() {
        this.sceneAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(this.nextScene);
    }
}
