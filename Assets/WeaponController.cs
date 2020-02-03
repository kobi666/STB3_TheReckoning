using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponController : MonoBehaviour
{
    private event Action _enemyTargetIdentified;
    public void EnemyTargetIdentified() {
        if (_enemyTargetIdentified != null) {
            _enemyTargetIdentified();
        }
    }

    event Action _enemyLeftRange;

    public void EnemyLeftRange() {
        if (_enemyLeftRange != null) {
            _enemyLeftRange();
        }
    }

    

    private void Awake() {
        
    }

    // private event Action _checkForSurroundingUnits

    public float range;
    GameObject EnemyTarget;
    public CircleCollider2D rangeCollider;

    // Start is called before the first frame update
    void Start()
    {
        rangeCollider = gameObject.GetComponent<CircleCollider2D>();
        rangeCollider.radius = range;
    }

    


    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other) {
        Debug.Log(other.gameObject.name + "  On Stay");
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log(other.gameObject.name + " Entered");
    }



    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log(other.gameObject.name + " Exited");
    }
}
