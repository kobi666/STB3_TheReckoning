using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[RequireComponent(typeof(CircleCollider2D))]
public class WeaponController : MonoBehaviour
{
    public event Action<CircleCollider2D, Collider2D> _OnTargetCheck;
    public void OnTargetCheck(CircleCollider2D col, Collider2D othercol) {
        if (_OnTargetCheck != null) {
            _OnTargetCheck.Invoke(col, othercol);
        }
    }

    



    

    

    

    private void Awake() {
        rangeCollider = gameObject.GetComponent<CircleCollider2D>();
        rangeCollider.radius = range;
    }

    // private event Action _checkForSurroundingUnits

    public float range;
    GameObject EnemyTarget;
    private CircleCollider2D rangeCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    


    // Update is called once per frame
    // private void OnTriggerStay2D(Collider2D other) {
    //     OnTargetCheck(rangeCollider, other);
    //     Debug.Log(other.gameObject.name + "  On Stay");
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        OnTargetCheck(rangeCollider, other);
        Debug.Log(other.gameObject.name + " Entered");
    }



    private void OnTriggerExit2D(Collider2D other) {
        OnTargetCheck(rangeCollider, other);
        Debug.Log(other.gameObject.name + " Exited");
    }
}
