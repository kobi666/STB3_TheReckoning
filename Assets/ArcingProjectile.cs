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

    public void MoveInArc(Vector2 targetPos) {
        if (init == false) {
            MaximumArcValue = Vector2.Distance(transform.position, targetPos);
            init = true;
        }
        bool reachedHighestPoint = false;
        float targetY = (targetPos.y + MaximumArcValue) / 2;
        transform.position = new Vector2(Mathf.SmoothStep(transform.position.x, targetPos.x, speed * Time.deltaTime), Mathf.SmoothStep(transform.position.y, targetY,(speed * Time.deltaTime) * 2));
        if (transform.position.y >= targetY && init2 == false) {
            reachedHighestPoint = true;
            targetY = targetPos.y;
            init2 = true;
        }
    }

    public float MovementDuration = 1.5f;

    public IEnumerator MoveInArcAndInvokeEvent(Vector2 TargetPosition) {
        float startTime = Time.time;
        float t;
        Action act = delegate { Debug.LogWarning("I reached");};
        float distanceToTargetPos = Vector2.Distance(transform.position,TargetPosition);
        float targetY = transform.position.y + (distanceToTargetPos / 2);
        while ((Vector2)transform.position != TargetPosition) {
            t = (Time.time - startTime) / MovementDuration;
            if (transform.position.y >= targetY - 0.10f) {
                targetY = TargetPosition.y;
            }
            // transform.position = new Vector2(Mathf.SmoothStep(transform.position.x, TargetPosition.x, t), 
            // Mathf.SmoothStep(transform.position.y, targetY, t * 2));
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(TargetPosition.x, transform.position.y), speed * Time.deltaTime);
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, targetY), (speed * Time.deltaTime));
            yield return new WaitForFixedUpdate();
        }
        act.Invoke();
        yield break;
    }

    

    public GameObject pfp;
    void Start()
    {
        StartCoroutine( MoveInArcAndInvokeEvent(pfp.transform.position));
    }

    
    void Update()
    {
        
    }
    
}
