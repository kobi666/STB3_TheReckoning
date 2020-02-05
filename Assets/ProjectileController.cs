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
    
    public GameObject _target;
    public GameObject Target { get => _target;
        set {
            _target = value;
            Debug.Log(gameObject.name);
            StartCoroutine(Utils.MoveToTargetWithEvent(gameObject, Target, speed, ReachedTarget));
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (Target != null || other != null) {
            if (other.gameObject.name == Target.gameObject.name) {
            HitTarget();
            }
        }
    }

    public void HitTarget() {
        //Debug.Log("Target was hit");
    }

    public void MissedTarget() {
        Debug.Log("Target was missed");
    }

    
    
    

    void asdas () {
        Debug.Log("I did it");
    }

    
    event Action _reachedTarget;
    void ReachedTarget() {
        if (_reachedTarget != null) {
            _reachedTarget.Invoke();
        }
    }

    private void Awake() {
        
    }

    private void Start() {
        _reachedTarget += asdas;
    }

    // IEnumerator MoveToTargetWithSpeed(GameObject Self, Vector2 OriginPosition, Vector2 TargetPosition, float _speed) {
    //     float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
    //     float t = 0;
    //     while (t <= 1.0f) {
    //         t += step;
    //         Self.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
    //         yield return new WaitForFixedUpdate();
    //     }
    //     Self.transform.position = TargetPosition;
    // }

    

    
}
