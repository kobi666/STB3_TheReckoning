using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;
using MyBox;

public abstract class TowerComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public bool debug;
    [ConditionalField("debug")]
    public SpriteRenderer SR;
    [ConditionalField("debug")]
    public AnimancerComponent Animancer;
    
    public TowerComponentData Data;
    [ConditionalField("debug")]
    public Tower ParentTower;
    
    
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

    
    
}
