using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimationController : MonoBehaviour
{
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    bool walkingTrigger = false;
    public bool WalkingTrigger {
        get {
            if (walkingTrigger == false) {
                walkingTrigger = true;
            }
            else {
                walkingTrigger = false;
            }
            return walkingTrigger;
        }
    }

    public void TriggerWalking() {
        animator.SetBool("Walking", WalkingTrigger);
    }

    public void TriggerAttack() {
        animator.SetTrigger("Attack");
    }

    public void TriggerDeath() {
        animator.SetTrigger("Death");
    }

   

    

}
