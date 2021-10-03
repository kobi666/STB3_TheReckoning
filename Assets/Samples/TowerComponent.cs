using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine.Serialization;

public abstract class TowerComponent : MyGameObject, IHasEffects,IHasRangeComponents
{
    public bool Autonomous = true;
    public abstract void InitComponent();
    public event Action onInitComponent;

    [FormerlySerializedAs("TowerComponentFamily")] public ComponentFamily componentFamily;


    public abstract void OnAwake();
    public abstract void OnStart();

    public event Action onComponentInitlized;

    public void OnComponentInitlized()
    {
        onComponentInitlized?.Invoke();
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
    
    [FormerlySerializedAs("ParentTower")][FoldoutGroup("parentComponents")] public TowerControllerLegacy ParentTowerLegacy;
    [FoldoutGroup("parentComponents")]
    public TowerController ParentTowerConroller;
    [FoldoutGroup("parentComponents")]
    public TowerSlotController parentTowerSlot;
    [FoldoutGroup("parentComponents")]
    public TowerComponent parentTowerComponent;
    [FoldoutGroup("parentComponents")]
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
    
    [Required]
    public CollisionDetector RangeDetector;
    
    [ConditionalField("debug")]
    public CircleCollider2D RangeCollider;    
    public abstract void PostAwake();
    protected void Awake() {
        SR = GetComponent<SpriteRenderer>() ?? null;
        Animancer = GetComponent<AnimancerComponent>() ?? null;
        PostAwake();

        if (componentFamily == ComponentFamily.None)
        {
            Debug.LogError("Tower Component Family is NONE : " + name);
        }
        OnAwake();
    }
    
    

    protected void Start()
    {
        parentTowerComponent = parentTowerComponent ?? GetComponentInParent<TowerComponent>() ?? null;
        ParentTowerLegacy =  ParentTowerLegacy ?? GetComponentInParent<TowerControllerLegacy>() ?? null;
        ParentTowerSlot = parentTowerSlot ?? GetComponentInParent<TowerSlotController>() ?? null;
        OnStart();
        
    }

    public abstract List<Effect> GetEffectList();

    public abstract void UpdateEffect(Effect ef);
    public void SetEffectList(List<Effect> effects)
    {
        Debug.LogWarning("Redundent");
    }

    public float rangeSize { get => Data.componentRadius; set => Data.componentRadius = value; }
    public abstract List<CollisionDetector> GetTagDetectors();

    public virtual void UpdateRange(float RangeSizeDelta, List<CollisionDetector> detectors)
    {
        Data.componentRadius = Data.componentRadius += RangeSizeDelta;
    }

}


public enum ComponentFamily
{
    None,
    AllShooter,
    FastShooter,
    SlowShooter,
    AOE,
    Spawner,
    Beam,
    Cursor,
    Melee,
    All
}
