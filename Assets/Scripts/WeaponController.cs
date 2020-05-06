using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
[RequireComponent(typeof(CircleCollider2D))]
public class WeaponController : MonoBehaviour
{
    
    
    Collider2D[] Collisions;
    public event Action<GameObject> _OnTargetCheck;
    public void OnTargetCheck(GameObject self) {
        if (_OnTargetCheck != null) {
            _OnTargetCheck.Invoke(self);
        }
    }

    


    public UnitController[] GetCollidingUnitsOfType (GameObject SelfGO, string ObjectType)
    {
        
        LayerMask lm = LayerMask.NameToLayer(ObjectType);
        Collisions = Physics2D.OverlapCircleAll(SelfGO.transform.position, SelfGO.GetComponent<CircleCollider2D>().radius, 1 << lm);
        // Collider2D[] Collisions = Physics2D.OverlapCircleAll(GO.transform.position, GO.GetComponent<CircleCollider2D>().radius - 0.2f, 1 << lm);
        UnitController[] GOs = new UnitController[Collisions.Length];
        for (int i = 0 ; i <= Collisions.Length-1 ; i++) {
            if (Collisions[i] == null) {
                continue;
            }
            GOs[i] = Collisions[i].GetComponent<UnitController>();
        }
        return GOs;
    }

    public UnitController[] GetEnemiesInRange(GameObject _self) {
        UnitController[] Enemies = GetCollidingUnitsOfType(_self, "Enemy");
        return Enemies;
    }

    public UnitController FindEnemyNearestToEndOfPath(GameObject self) {
        UnitController target = null;
        float LowestProximity = 999.0f;
        foreach (UnitController Enemy in GetEnemiesInRange(self)) {
            if (Enemy.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline < LowestProximity) {
                target = Enemy;
                LowestProximity = Enemy.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline;
            }
            else {
                continue;
            }
        }
        return target;
    }

    


    



    

    

    

    

    // private event Action _checkForSurroundingUnits


    // Start is called before the first frame update
    

    


    // Update is called once per frame
    // private void OnTriggerStay2D(Collider2D other) {
    //     OnTargetCheck(rangeCollider, other);
    //     Debug.Log(other.gameObject.name + "  On Stay");
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        OnTargetCheck(this.gameObject);
        // Debug.Log(other.gameObject.name + " Entered");
    }



    private void OnTriggerExit2D(Collider2D other) {
        OnTargetCheck(this.gameObject);
        //Debug.Log(other.gameObject.name + " Exited");
    }
}
