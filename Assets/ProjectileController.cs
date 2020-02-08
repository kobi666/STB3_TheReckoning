using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    

    

[SerializeField]
    public DamageRange _damageRange;
    public Damage_Type _damageType = new Damage_Type("normal");
    
    public GameObject _target;
    public EnemyUnitController _EnemyUnitController {get => Target.GetComponent<EnemyUnitController>();}
    public GameObject Target { get => _target;
        set {
            _target = value;
            StartCoroutine(Utils.MoveToTargetWithEvent(gameObject, Target, speed, ReachedTarget));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (Target != null && other != null) {
            if (other.gameObject.name == Target.gameObject.name) {
            HitTarget();
            }
        }
    }

    public void HitTarget() {
        _EnemyUnitController._UnitStats.DamageToUnit(UnityEngine.Random.Range(_damageRange.min, _damageRange.max), _damageType);
    }

    public void MissedTarget() {
        Debug.Log("Target was missed");
    }

    
    event Action _reachedTarget;
    void ReachedTarget() {
        if (_reachedTarget != null) {
            _reachedTarget.Invoke();
        }
    }

    

    

    

    
}
