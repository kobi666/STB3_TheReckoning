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
        Collider2D[] collisions = new Collider2D[20];
        Physics2D.OverlapCollider(Self.GetComponent<CircleCollider2D>() ,FilterByLayerObject(TargetGO), collisions);
        bool TargetWasHit = false;
        foreach (Collider2D col in collisions) 
        {
            if (col.gameObject.name == TargetGO.name) {
                _HitTarget.Invoke();
                break;
            }
        }
        if (TargetWasHit) {
            _HitTarget.Invoke();
        }
        else {
            _missedTarget.Invoke();
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


