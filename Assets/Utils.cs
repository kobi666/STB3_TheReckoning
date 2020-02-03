using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Utils : MonoBehaviour 
{
    // Start is called before the first frame update
    public static GameObject FindObjectNearestToEndToEndOfSplineInGOLayer (GameObject GO)
    {
        GameObject TargetGO = null;
        LayerMask lm = LayerMask.NameToLayer(GO.GetComponent<SpriteRenderer>().sortingLayerName);
        float Proximity = 999.0f;
        Collider2D[] Collisions = new Collider2D[200];
        // Collider2D[] Collisions = Physics2D.OverlapCircleAll(GO.transform.position, GO.GetComponent<CircleCollider2D>().radius - 0.2f, 1 << lm);
        Physics2D.OverlapCircleNonAlloc(GO.transform.position, GO.GetComponent<CircleCollider2D>().radius - 0.2f, Collisions, 1 << lm);
        foreach (Collider2D collision in Collisions)
        {
            if (collision == null) {
                continue;
            }
            if (collision.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline < Proximity)
            {
                TargetGO = collision.gameObject;
                Proximity = collision.gameObject.GetComponent<BezierSolution.UnitWalker>().ProximityToEndOfSpline;
            }
        }
        return TargetGO;
    }

    public GameObject IdentifyCollidingUnitNearestToPathEndWithTag(Collider2D _col, string _tag) {
        
        if (_col.gameObject.tag == _tag) {
            return Utils.FindObjectNearestToEndToEndOfSplineInGOLayer(_col.gameObject);
        }
        else {
        return null;
        }  
    }

    public static Action DBG(string s) {
        Debug.Log(s);
        return null;
    }


}
