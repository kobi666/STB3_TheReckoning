using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTest : MonoBehaviour
{
    PlayerInput PI;
    Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
        PI = new PlayerInput();
    }

    private void OnEnable() {
        PI.TestButtons.Enable();
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

   

    
    // Start is called before the first frame update
    void Start()
    {
        PI.TestButtons.TestWalking.performed += ctx => TriggerWalking();
        PI.TestButtons.TestAttack.performed += ctx => TriggerAttack();
        PI.TestButtons.TestDeath.performed += ctx => TriggerDeath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
