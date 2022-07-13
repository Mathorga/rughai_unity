using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {
    private float duration;
    private float magnitude;
    private float fadeSpeed;

    void Update() {
        if (this.duration > 0.0f) {
            this.transform.position += Random.insideUnitSphere * this.magnitude;
            
            this.duration -= this.fadeSpeed;
        } else {
            this.duration = 0.0f;
        }
    }

    public void Shake(float duration, float magnitude, float fadeSpeed) {
        this.duration = duration;
        this.magnitude = magnitude;
        this.fadeSpeed = fadeSpeed;
    }
}
