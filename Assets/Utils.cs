using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils : MonoBehaviour 
{
    
    // Start is called before the first frame update
    // public static GameObject FindObjectNearestToEndToEndOfSplineInGOLayer (GameObject SelfGO, Collider2D otherGO)
    // {
    //     GameObject TargetGO = new GameObject();
    //     LayerMask lm = LayerMask.NameToLayer(otherGO.gameObject.GetComponent<SpriteRenderer>().sortingLayerName);
    //     float Proximity = 999.0f;
    //     Collider2D[] Collisions = new Collider2D[200];
    //     // Collider2D[] Collisions = Physics2D.OverlapCircleAll(GO.transform.position, GO.GetComponent<CircleCollider2D>().radius - 0.2f, 1 << lm);
    //     Physics2D.OverlapCircleNonAlloc(SelfGO.transform.position, SelfGO.GetComponent<CircleCollider2D>().radius, Collisions, 1 << lm);
    //     foreach (Collider2D collision in Collisions)
    //     {
    //         if (collision == null) {
    //             continue;
    //         }
    //         if (collision.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline < Proximity)
    //         {
    //             TargetGO = collision.gameObject;
    //             Proximity = collision.gameObject.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline;
    //         }
    //     }
    //     return TargetGO;
    // }

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

    public static IEnumerator MoveToTargetWithEvent(GameObject _selfGO, GameObject _targetGO, float _speed, Action _actionEvent) {
        Vector2 OriginPosition = _selfGO.transform.position;
        Vector2 TargetPosition =  _targetGO.transform.position;
        float step = (_speed / (OriginPosition - TargetPosition).magnitude * Time.fixedDeltaTime );
        float t = 0;
        while (t <= 1.0f) {
            t += step;
            _selfGO.transform.position = Vector2.Lerp(OriginPosition, TargetPosition, t);
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

    public static ContactFilter2D FilterByLayerName(string _layerName) {
        ContactFilter2D EF = new ContactFilter2D();
        EF.SetLayerMask(LayerMask.NameToLayer("Enemy"));
        return EF;
    }

    public static ContactFilter2D FilterByLayerObject(GameObject _go) {
        ContactFilter2D EF = new ContactFilter2D();
        LayerMask lm = _go.layer;
        EF.SetLayerMask(_go.layer);
        return EF;
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


