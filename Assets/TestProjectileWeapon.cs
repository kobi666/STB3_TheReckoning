using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[RequireComponent(typeof(CircleCollider2D))]
public class TestProjectileWeapon : WeaponController
{




    [SerializeField]
    public float _fireRate;
    public float FireRate {get => _fireRate ; set {
        _fireRate = value;
    }}

    float _lastShot = 0.0f;

    int t;
    int t2;

    
    
    // Start is called before the first frame update
    // Do not Declare things in Awake, it overrides functions in parent class AwakeFunction!!
    // Individual changes must be declared at Start() !
    public event Action _enemyTargetIdentified;
    public void EnemyTargetIdentified() {
        if (_enemyTargetIdentified != null) {
            _enemyTargetIdentified.Invoke();
        }
    }

    public GameObject Projectile;
    public void CreateProjectile() {
        if (1 > 0) {
        Debug.Log("I fired a projectile");
        GameObject _projectile = GameObject.Instantiate(Projectile, this.gameObject.transform.position, Quaternion.identity);
        _projectile.name = (_projectile.name+UnityEngine.Random.Range(10000, 99999));
        _projectile.GetComponent<ProjectileController>().Target = EnemyTarget;
        _lastShot = Time.time;
        }
        else {
            Debug.Log("Something");
        }
    }

    public event Action _enemyTargetRelease;
    public void EnemyTargetRelease() {
        if (_enemyTargetRelease != null) {
            _enemyTargetRelease.Invoke();
        }
    }

    [SerializeField]
    private GameObject _EnemyTarget;
    public GameObject EnemyTarget { get => _EnemyTarget ;  set {
        if (value != null) {
            _EnemyTarget = value;
            EnemyTargetIdentified();
        }
        if (value == null) {
            _EnemyTarget = value;
            EnemyTargetRelease();
        }
    }}

    


    
    



    public void SetSingleEnemyTarget(GameObject __self) {
        EnemyTarget = FindEnemyNearestToEndOfPath(__self);
    }

    void Start()
    {
        _OnTargetCheck += SetSingleEnemyTarget;
        _enemyTargetIdentified += CreateProjectile;
    }

    // Update is called once per frame
    private void FixedUpdate() {
       // Debug.Log("Time : " + Time.time +  " Last shot: " + _lastShot);
    if (Time.time > FireRate + _lastShot )    {
        t = 1;
        _lastShot = Time.time;
    }
    else {
        t = 2;
    }
        
    }
    

   
        
    
}
