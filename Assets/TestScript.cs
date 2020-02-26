using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{

    public GameObject Target;
    public GameObject Target1;
    public GameObject Target2;

     
    public int speed;
    bool MovementInProgress;
    public Vector2 TargetPosition {
        get => Target.transform.position;
    }

    public void Test1() {
        Debug.Log("Test 11111");
    }

    public void Test2() {
        Debug.Log("Test 22222");
    }


    event Action test;
    event Action test1;
    event Action test2;

    IEnumerator MovementCourtinePlaceholder;

    public void MoveToTargetWithConditionAndEvent(Vector2 targetPosition, bool condition, Action action, float speed ) {
        // if (MovementInProgress == true) {
        //     StopCoroutine(MovementCourtinePlaceholder);
        // }
        MovementCourtinePlaceholder = MovementCourutine(targetPosition, condition, action, speed);
        StartCoroutine(MovementCourtinePlaceholder);
        //StartCoroutine(Utils.MoveToTarget(gameObject, transform.position, TargetPosition, speed));
    }

    public IEnumerator MovementCourutine(Vector2 targetPosition, bool condition, Action action, float speed) {
        MovementInProgress = true;
        Vector2 originPosition = transform.position;
            while (condition == true) {
                transform.position = Vector2.MoveTowards(originPosition, targetPosition, speed * Time.fixedDeltaTime);
                yield return new WaitForFixedUpdate();
            }
        action.Invoke();
        MovementInProgress = false;
    }

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


    // Start is called before the first frame update
    void Start()
    {
        test1 += Test1;
        test2 += Test2;
        MoveToTargetWithConditionAndEvent(TargetPosition, true, test, speed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.S)) {
            Debug.Log("S pressed");
            if (Target.name == Target1.name) {
                Target = Target2;
                test = test2;
            }
            else {
                Target = Target1;
                test = test1;
            }
            MoveToTargetWithConditionAndEvent(TargetPosition, true, test, speed);
        }
    }
}
