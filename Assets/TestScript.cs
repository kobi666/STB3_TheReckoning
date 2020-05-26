using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    public GameObject orbital;

    [SerializeField]
    public float angle;
    public float Angle {
        get => angle;
        set {
            angle = value;
            if (angle > 360) {
                angle = 0;
            }
            if (angle < 0) {
                angle = 360;
            }
        }
    }

    public static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion angle) {
        return angle * ( point - pivot) + pivot;
    }

    private void Update() {
        Angle += Time.deltaTime;
//        orbital.transform.position = (Vector2)RotatePointAroundPivot(orbital.transform.position, transform.position, Quaternion.Euler(360 * Time.deltaTime,360 * Time.deltaTime , 360 * Time.deltaTime ));
              
    }
}
