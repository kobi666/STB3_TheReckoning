using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using System.Threading.Tasks;

public class ProjectileExitPoint : SerializedMonoBehaviour
{
    public GenericWeaponController ParentWeaponController;
    public float rotationSpeed = 1;
    public TargetUnit Target
    {
        get => ParentWeaponController?.Target;
    }


    private void Start()
    {
        ParentWeaponController = GetComponentInParent<GenericWeaponController>();
    }

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
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.instance.DeltaGameTime * rotationSpeed);
        
    }
    
}