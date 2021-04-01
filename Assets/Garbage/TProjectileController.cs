using UnityEngine;
using System;

public class TProjectileController : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    

    

[SerializeField]
    public DamageRange _damageRange;
    public Damage_Type _damageType = new Damage_Type("normal");
    
    

    private void Awake() {
        _reachedTarget += DestroySelf;
    }

    void DestroySelf() {
        Destroy(gameObject);
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
