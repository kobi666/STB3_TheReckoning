using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Sirenix.OdinInspector;

public class ProjectileFinalPoint : SerializedMonoBehaviour
{
    WeaponController parentTowerComponent;
    private GenericWeaponController ParentWeaponController;
    public float rotationSpeed = 1;
    public Transform PositionTransform;

    public Vector2 Position
    {
        get => PositionTransform.position;
        set => PositionTransform.position = value;
    }
    public TargetUnit Target
    {
        get => ParentWeaponController?.Target;
    }
    
    public RangeDetector RangeDetector;
    public EffectableTargetBank EffectableTargetBank;
    protected void Awake()
    {
        EffectableTargetBank = GetComponent<EffectableTargetBank>() ?? null;
    }
    
    protected void Start() {
        parentTowerComponent = transform.parent?.GetComponent<WeaponController>() ?? null;
        RangeDetector = RangeDetector ?? GetComponentInChildren<RangeDetector>() ?? null;
        if (parentTowerComponent != null) {
            transform.position = new Vector2(transform.position.x + parentTowerComponent.Data.componentRadius, transform.position.y);
        }
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
        transform.rotation = Quaternion.Slerp(transform.rotation, q, StaticObjects.Instance.DeltaGameTime * rotationSpeed);
    }

}
