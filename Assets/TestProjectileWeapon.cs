using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[RequireComponent(typeof(CircleCollider2D))]
public class TestProjectileWeapon : WeaponController
{
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
        GameObject _projectile = GameObject.Instantiate(Projectile, this.gameObject.transform.position, Quaternion.identity);
        _projectile.GetComponent<ProjectileController>().Target = EnemyTarget;
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
    void Update()
    {
        
    }
}
