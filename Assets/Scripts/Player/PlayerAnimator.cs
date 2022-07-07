using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour {
    // Public members.
    public Animator[] childAnimators;

    // Private components.
    private Animator animator;
    private PlayerController controller;

    // Private state variables.
    private string currentAnimation = "Idle";

    void Start() {
        this.animator = this.GetComponent<Animator>();
        this.controller = this.GetComponent<PlayerController>();
    }

    void Update() {
        string animation = this.GetAnimation();

        if (animation != this.currentAnimation) {
            this.animator.CrossFade(animation, 0.0f, 0);

            // Play animation for all children as well.
            foreach (Animator childAnimator in this.childAnimators) {
                childAnimator.CrossFade(animation, 0.0f, 0);
            }
        }
    }

    /// Retrieves the animation to play according to the current state.
    private string GetAnimation() {
        string animation = currentAnimation;

        // if (Time.time < )
        switch (this.controller.state) {
            case PlayerController.State.Idle:
                // TODO.
                break;
            default:
                break;
        }

        return animation;
    }


    // Inspiration:
    // private void Update() {
    //     var state = GetState();

    //     if (state == _currentState) return;
    //     _anim.CrossFade(state, 0, 0);
    //     _currentState = state;
    // }

    // private int GetState() {
    //     if (Time.time < _lockedTill) return _currentState;

    //     // Priorities
    //     if (_attacking) return LockState(Attack, _attackAnimDuration);
    //     if (_crouching) return Crouch;
    //     if (_landed) return LockState(Land, _landAnimationDuration);
    //     if (_jumping) return Jump;

    //     if (_grounded) return _input.x == 0 ? Idle : Walk;
    //     return _speed.y > 0 ? Jump : Fall;

    //     int LockState(int s, float t) {
    //         _lockedTill = Time.time + t;
    //         return s;
    //     }
    // }
}
