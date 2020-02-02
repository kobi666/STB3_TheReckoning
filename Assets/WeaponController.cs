﻿using System.Collections;
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

    public void stam() {
        Debug.Log("Meshy is pretty");
    }

    public void stam2 () {
        Debug.Log("stam");
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

    public GameObject IdentifyUnitNearestToPathEnd(Collider2D _col, string _tag) {
        
        if (_col.gameObject.tag == _tag) {
            return Utils.FindObjectNearestToEndToEndOfSplineInGOLayer(_col.gameObject);
        }
        else {
        return null;
        }  
    }


    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            EnemyTargetIdentified();
        }
    }



    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            EnemyLeftRange();
            Debug.Log(other.gameObject.name);
        }
    }
}
