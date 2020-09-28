﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;
using MyBox;
using Sirenix.OdinInspector;

public abstract class TowerComponent : SerializedMonoBehaviour
{

    public bool IsMainTowerComponent = true;
    // Start is called before the first frame update
    public bool debug;
    [ConditionalField("debug")]
    public SpriteRenderer SR;
    [ConditionalField("debug")]
    public AnimancerComponent Animancer;
    
    public TowerComponentData Data;
    
    public TowerController ParentTower;

   
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
                parentTowerSlot = ParentTower.ParentSlotController ?? ParentTower.GetComponentInParent<TowerSlotController>() ?? null;
                return parentTowerSlot;
            }
        }
        set
        {
            parentTowerSlot = value ?? null;
        }
    }
    
    
    public EnemyTargetBank EnemyTargetBank {get ; private set;}

    [ConditionalField("debug")]
    public RangeDetector RangeDetector;
    
    [ConditionalField("debug")]
    public CircleCollider2D RangeCollider;    
    public abstract void PostAwake();
    protected void Awake() {
        //EnemyTargetBank = GetComponentInChildren<EnemyTargetBank>() ?? ParentTower?.TargetBank ?? null;
        SR = GetComponent<SpriteRenderer>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
        
        PostAwake();
    }

    protected void Start()
    {
        parentTowerComponent = parentTowerComponent ?? GetComponentInParent<TowerComponent>() ?? null;
        ParentTower =  ParentTower ?? GetComponentInParent<TowerController>() ?? null;
        ParentTowerSlot = ParentTower.ParentSlotController ?? parentTowerComponent.ParentTower.ParentSlotController ?? null;
    }
}
