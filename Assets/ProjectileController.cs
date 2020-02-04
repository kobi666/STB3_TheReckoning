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
    public GameObject Target { 
        set {
            _target = value;
            StartCoroutine(Utils.ShootProjectileWithSingleTargetWithConfirmationEvents(gameObject, value, speed, MissedTarget, HitTarget));
        }
    }

    public void HitTarget() {
        Debug.Log("Target was hit");
    }

    public void MissedTarget() {
        Debug.Log("Target was missed");
    }

    
    Vector2 _targetPosition;
    public Vector2 TargetPosition {get => _targetPosition ;set {
        _targetPosition = value;
        //StartCoroutine(Utils.MoveToTargetWithEvent(gameObject, SelfPositon, _targetPosition, speed, ReachedTarget));
        StartCoroutine(Utils.ShootProjectileWithSingleTargetWithConfirmationEvents(gameObject, _target, speed, MissedTarget, HitTarget));
    }}

    void asdas () {
        Debug.Log("I did it");
    }

    Vector2 SelfPositon;
    event Action _reachedTarget;
    void ReachedTarget() {
        if (_reachedTarget != null) {
            _reachedTarget.Invoke();
        }
    }

    private void Awake() {
        SelfPositon = gameObject.transform.position;
    }

    private void Start() {
        
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
