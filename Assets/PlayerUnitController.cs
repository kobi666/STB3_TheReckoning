using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerUnitController : MonoBehaviour
{

    public GameObject EnemyTarget;
    public Vector2 SetPosition;
    Collider2D [] collisions;

    public void SetEnemyTarget() {
        EnemyTarget = Utils.FindEnemyNearestToEndOfPath(gameObject, collisions);
        Debug.Log("Player Unit Found Enemy " + EnemyTarget.name);
    }
    // Start is called before the first frame update
    public event Action _onTargetCheck;
    public void OnTargetCheck() {
        if (_onTargetCheck != null) {
            _onTargetCheck.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        OnTargetCheck();
    }

    private void Awake() {
        _onTargetCheck += SetEnemyTarget;
    }


}
