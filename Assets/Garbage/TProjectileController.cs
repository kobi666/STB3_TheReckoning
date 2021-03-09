using UnityEngine;
using System;

public class TProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    

    

[SerializeField]
    public DamageRange _damageRange;
    public Damage_Type _damageType = new Damage_Type("normal");
    
    public UnitController target;
    
    public UnitController Target { get => target;
        set {
            target = value;
            StartCoroutine(Utils.MoveTowardsTargetWithEvent(gameObject, Target.gameObject, speed, ReachedTarget));
        }
    }

    private void Awake() {
        _reachedTarget += DestroySelf;
    }

    void DestroySelf() {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (Target != null) {
                if (other.gameObject.name == Target.gameObject.name) {
                HitTarget();
                Destroy(gameObject);
                }
        }
    }

    public void HitTarget() {
        Target.LifeManager.DamageToUnit(UnityEngine.Random.Range(_damageRange.min, _damageRange.max), _damageType);
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
