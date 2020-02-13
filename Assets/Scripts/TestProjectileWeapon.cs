using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[RequireComponent(typeof(CircleCollider2D))]
public class TestProjectileWeapon : WeaponController
{

    public DamageRange _damageRange;

    public event Action _onShootprojectile;
    public void OnShootProjectile() {
        if (_onShootprojectile != null) {
            _onShootprojectile.Invoke();
        }
    }

    public void Shoot() {
        CreateProjectile();
        fireCounter = 0.0f;
    }

    bool ShootCondition() {
        if (EnemyTarget != null && fireCounter >= 1.0f) {
            return true;
        }
        else {
            return false ;
        }
    }

    [SerializeField]
    public float _fireRate;
    public float FireRate {get => _fireRate ; set {
        _fireRate = value;
    }}

    public float fireCounter;

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
    public virtual void CreateProjectile() {
        //Debug.Log("I fired a projectile");
        GameObject _projectile = GameObject.Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
        _projectile.name = (_projectile.name+UnityEngine.Random.Range(10000, 99999));
        _projectile.transform.parent = StaticObjects.PPH;
        _projectile.GetComponent<ProjectileController>()._damageRange = _damageRange;
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

    private void Awake() {
        _OnTargetCheck += SetSingleEnemyTarget;
        _onShootprojectile += Shoot;
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if (ShootCondition()) {
            OnShootProjectile();
        }
        if (fireCounter < 1.0f) {
            fireCounter += (Time.fixedDeltaTime * FireRate) / 10;
        }
    
        
    }
}
    

   
        
    

