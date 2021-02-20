﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public abstract class TowerComponent : MonoBehaviour, IHasEffects,IHasRangeComponent
{
    public abstract void InitComponent();
    public event Action onInitComponent;

    public void OnInitComponent()
    {
        InitComponent();
        onInitComponent?.Invoke();
    }
    
    public bool RotatingComponent;
    [ShowIf("RotatingComponent")]
    public float RotationSpeed = 5;

    // Start is called before the first frame update
    public bool debug;
    [ConditionalField("debug")]
    public SpriteRenderer SR;
    [ConditionalField("debug")]
    public AnimancerComponent Animancer;

    public bool legacyTower = true;
    
    [ShowIf("legacyTower")]
    public TowerComponentData Data;
    
    [FormerlySerializedAs("ParentTower")] public TowerControllerLegacy ParentTowerLegacy;
    public TowerController ParentTowerConroller;
   
    public TowerSlotController parentTowerSlot;
    public TowerComponent parentTowerComponent;
    public TowerSlotController ParentTowerSlot
    {
        get
        {
            if (parentTowerSlot != null)
            {
                return parentTowerSlot;
            }
            else
            {
                parentTowerSlot = ParentTowerLegacy.ParentSlotController ?? ParentTowerLegacy.GetComponentInParent<TowerSlotController>() ?? null;
                return parentTowerSlot;
            }
        }
        set
        {
            parentTowerSlot = value ?? null;
        }
    }
    
    
    public EnemyTargetBank EnemyTargetBank {get ; private set;}
    
    public TagDetector RangeDetector;
    
    [ConditionalField("debug")]
    public CircleCollider2D RangeCollider;    
    public abstract void PostAwake();
    protected void Awake() {
        SR = GetComponent<SpriteRenderer>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
        PostAwake();
    }
    
    

    protected void Start()
    {
        //legacy
        parentTowerComponent = parentTowerComponent ?? GetComponentInParent<TowerComponent>() ?? null;
        ParentTowerLegacy =  ParentTowerLegacy ?? GetComponentInParent<TowerControllerLegacy>() ?? null;
        ParentTowerSlot = parentTowerSlot ?? GetComponentInParent<TowerSlotController>() ?? null;
    }

    public abstract List<Effect> GetEffectList();

    public abstract void UpdateEffect(Effect ef, List<Effect> appliedEffects);

    public abstract List<TagDetector> GetRangeDetectors();

    public virtual void UpdateRange(float RangeSizeDelta, List<TagDetector> detectors)
    {
        Data.componentRadius = Data.componentRadius += RangeSizeDelta;
    }

}
