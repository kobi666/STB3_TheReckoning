using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ProjectileFinalPoint : MonoBehaviour
{
    WeaponController parentTowerComponent;
    private GenericWeaponController ParentWeaponController;

    public TargetUnit Target
    {
        get => parentTowerComponent.Target;
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

    

    
    public async void rotateTowerdsTarget(Transform target)
    {
        
    }
    
}
