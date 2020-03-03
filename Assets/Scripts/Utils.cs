using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils
{
    

    public static IEnumerator MoveToTarget(GameObject Self, Vector2 OriginPosition, Vector2 TargetPosition, float _speed) {
        float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            Self.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
            yield return new WaitForFixedUpdate();
        }
        Self.transform.position = TargetPosition;
    }

    public static IEnumerator MoveToTargetWithCondition(GameObject Self, Vector2 OriginPosition, Vector2 TargetPosition, float _speed, bool conditon) {
        float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f && conditon == true) {
            t += step;
            Self.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
            yield return new WaitForFixedUpdate();
        }
        
    }


    public static IEnumerator MoveToTargetWithEvent(GameObject Self, Vector2 OriginPosition, Vector2 TargetPosition, float _speed, Action _actionEvent) {
        float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            Self.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
            yield return new WaitForFixedUpdate();
        }
        Self.transform.position = TargetPosition;
        _actionEvent.Invoke();
    }

    public static IEnumerator MoveToTargetWithEventAndCondition(GameObject self, Vector2 originPosition, Vector2 targetPosition, float speed, Action actionEvent, bool condition) {
        float step = (speed / (originPosition - targetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f && condition) {
            t += step;
            self.transform.position = Vector2.Lerp(originPosition, targetPosition, t);
            yield return new WaitForFixedUpdate();
        }
        actionEvent.Invoke();
    }

    public static IEnumerator MoveTowardsTargetWithEvent(GameObject _selfGO, GameObject _targetGO, float _speed, Action _actionEvent) {
        Vector2 OriginPosition = _selfGO.transform.position;
        Vector2 TargetPosition =  _targetGO.transform.position;
        float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            if (_targetGO != null) {
            _selfGO.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
            }
            else {
                _selfGO.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
            }
            yield return new WaitForFixedUpdate();
        }
        //_selfGO.transform.position = TargetPosition;
        _actionEvent.Invoke();
    }

    public static IEnumerator ShootProjectileWithSingleTargetWithConfirmationEvents(GameObject Self, GameObject TargetGO, float _speed, Action _missedTarget, Action _HitTarget) {
        Vector2 TargetPosition = TargetGO.transform.position;
        Vector2 OriginPosition = Self.transform.position;
        float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            Self.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
            yield return new WaitForFixedUpdate();
        }
        Self.transform.position = TargetPosition;
        Collider2D[] collisions = Physics2D.OverlapCircleAll(OriginPosition, Self.GetComponent<CircleCollider2D>().radius, 1 << TargetGO.layer);
        bool TargetWasHit = false;
        foreach (Collider2D col in collisions) 
        {
            
            if (col == null) {
                Debug.Log("null colider");
                continue;
                
            }

            Debug.Log("Collider Name: " + col.gameObject.name);
            Debug.Log("TargetGO name: " + TargetGO.name);

            if (col.gameObject.name == TargetGO.name) {
                TargetWasHit = true;
                break;
            }
        }
        if (TargetWasHit) {
            Debug.Log("Hit");
            _HitTarget.Invoke();
            yield break;
        }
        else {
            Debug.Log("miss");
            _missedTarget.Invoke();
            yield break;
        }
            
             
    }

    public static GameObject SetSingleEnemyTarget(GameObject _self, Collider2D[] _collisions) {
        return FindEnemyNearestToEndOfPath(_self, _collisions);
    }

    public static GameObject[] GetCollidingObjectsOfType (GameObject _SelfGO, string _objectType, Collider2D[] _collisions)
    {
        
        LayerMask lm = LayerMask.NameToLayer(_objectType);
        _collisions = Physics2D.OverlapCircleAll(_SelfGO.transform.position, _SelfGO.GetComponent<CircleCollider2D>().radius, 1 << lm);
        // Collider2D[] Collisions = Physics2D.OverlapCircleAll(GO.transform.position, GO.GetComponent<CircleCollider2D>().radius - 0.2f, 1 << lm);
        GameObject[] GOs = new GameObject[_collisions.Length];
        for (int i = 0 ; i <= _collisions.Length-1 ; i++) {
            if (_collisions[i] == null) {
                continue;
            }
            GOs[i] = _collisions[i].gameObject;
        }
        return GOs;
    }


    public static GameObject[] GetEnemiesInRange(GameObject _self, Collider2D[] _collisions) {
        GameObject[] Enemies = GetCollidingObjectsOfType(_self, "Enemy",_collisions);
        return Enemies;
    }

    public static GameObject[] GetObjectsOfTypeInRange(GameObject _self, string _type, Collider2D[] _collisions) {
        GameObject[] Enemies = GetCollidingObjectsOfType(_self, _type,_collisions);
        return Enemies;
    }

    public static GameObject FindEnemyNearestToEndOfPath(GameObject self, Collider2D[] _collisions) {
        GameObject target = null;
        float LowestProximity = 999.0f;
        foreach (GameObject Enemy in GetEnemiesInRange(self, _collisions)) {
            if (Enemy.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline < LowestProximity && Enemy.GetComponent<EnemyUnitController2>().IsTargetable == true) {
                target = Enemy;
                LowestProximity = Enemy.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline;
            }
            else {
                continue;
            }
        }
        return target;
    }

    public static IEnumerator IncrementCounterOverTimeAndInvokeAction(float counter, float counterMax, float IncrementMultiplier, bool condition, Action action) {
        while (condition) {
            counter += Time.fixedDeltaTime * IncrementMultiplier;
            if (counter >= counterMax) {
                action.Invoke();
                counter = 0.0f;
            }
            yield return new WaitForFixedUpdate();
        }
        counter = counterMax;
        //Debug.Log("Finished Incrementing");
        yield break;
    }




    

    // public static GameObject IdentifyCollidingUnitNearestToPathEndWithTag(Collider2D _col, string _tag, Collider2D _othercol) {
        
    //     if (_othercol.gameObject.tag == _tag) {
    //         return Utils.FindObjectNearestToEndToEndOfSplineInGOLayer(_col.gameObject, _othercol);
    //     }
    //     else {
    //     return null;
    //     }  
    // }

    


}


