using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ArcingProjectile : MonoBehaviour
{
     


    public float MaximumArcValue = 5;
    public float speed = 5;
    public bool init = false;
    public bool init2 = false;

    

    public float MovementDuration = 1.5f;





public IEnumerator MoveInArc3(Vector2 targetPos) {
    //point[1] = point[0] +(point[2] -point[0])/2 +Vector3.up *5.0f;
    Vector2 initPos = transform.position;
    Vector2 middlePos = initPos + (targetPos - initPos) / 2 + Vector2.down * MaximumArcValue;
    float count = 0;
    while (count < 1.0f) {
        count += Time.deltaTime * speed;

        Vector2 m1 = Vector2.Lerp( initPos, middlePos, count );
        Vector3 m2 = Vector2.Lerp( middlePos, targetPos, count );
        transform.position = Vector2.Lerp(m1, m2, count);
        yield return new WaitForFixedUpdate();
    }
    yield break;
}

    

    public GameObject pfp;
    void Start()
    {
        //StartCoroutine( MoveInArcAndInvokeEvent(pfp.transform.position));
        StartCoroutine(MoveInArc3(pfp.transform.position));
    }

    
    void Update()
    {
        
    }
    
}
