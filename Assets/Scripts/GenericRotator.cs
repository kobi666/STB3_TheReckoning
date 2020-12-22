using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class GenericRotator : MonoBehaviour
{
    public Transform Target;
    public float rotationSpeed = 1;
    bool AsyncRotationInProgress = false;

    public void StopAsyncRotation() {
        AsyncRotationInProgress = false;
    }
    public async void StartAsyncRotation() {
        if (AsyncRotationInProgress == true) {
            AsyncRotationInProgress = false;
            await Task.Yield();
        }
        AsyncRotationInProgress = true;
        while (AsyncRotationInProgress == true) {
            DefaultRotationFunction();
            await Task.Yield();
        }
        AsyncRotationInProgress = false;
    }
    
    public void DefaultRotationFunction() {
        Vector2 vecToTarget = Target.transform.position - transform.position;
        float angleToTarget = Mathf.Atan2(vecToTarget.y, vecToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angleToTarget, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.Instance.DeltaGameTime * rotationSpeed);
    }
}
