﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;


[Serializable]
public class GenericUnitController : MonoBehaviour,IQueueable<GenericUnitController>,IActiveObject<GenericUnitController>,IHasEffects,IHasRangeComponent,IHasStateMachine
{
    private void OnEnable()
    {
        try
        {
            GameObjectPool.Instance.ActiveUnits.Add(name,this);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
        
    }

    private void OnDisable()
    {
        try
        {
            GameObjectPool.Instance.ActiveUnits.Remove(name);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e);
        }
    }


    [SerializeField]
    public List<UnitState> UnitStates =  new List<UnitState>();
    
    public bool WalksOnPath;
    private UnitLifeManager UnitLifeManager = new UnitLifeManager();
    public UnitData Data = new UnitData();
    [TagSelector]
    public string GroupTag;

    public float UnitDiscoveryRadius;
    //public GenericStateMachine StateMachine;
    
    [FoldoutGroup("Components")]
    public AnimationController AnimationController;
    [FoldoutGroup("Components")]
    public SpriteRenderer SpriteRenderer;
    [FoldoutGroup("Components")]
    public UnitBattleManager UnitBattleManager;
    [FoldoutGroup("Components")]
    public LifeBarmanager LifeBarmanager;
    [FoldoutGroup("Components")]
    public TagDetector RangeDetector;
    [FoldoutGroup("Components")]
    public EffectableUnit EffectableUnit;
    [FoldoutGroup("Components")]
    public EffectableTargetBank EffectableTargetBank;
    [FoldoutGroup("Components")]
    public UnitStateMachine StateMachine;
    [FoldoutGroup("Components")]
    public PathWalker PathWalker;
    [FoldoutGroup("Components")] 
    public UnitMovementController UnitMovementController;
    

    void Init()
    {
        UnitLifeManager.InitialHP = Data.MetaData.HP;
        UnitLifeManager.Init();
        UpdateRange(Data.MetaData.DiscoveryRadius, GetRangeDetectors());
        foreach (var s in UnitStates)
        {
            s.Init(this);
        }
        
        StateMachine.Init(this,UnitStates);
    }
    
    protected void Awake()
    {
        StateMachine = GetComponent<UnitStateMachine>();
        tag = GroupTag;
        //StateMachine = StateMachine ?? GetComponent<GenericStateMachine>();
        AnimationController = AnimationController ?? GetComponent<AnimationController>();
        SpriteRenderer = SpriteRenderer ?? GetComponent<SpriteRenderer>();
        UnitBattleManager = UnitBattleManager ?? GetComponent<UnitBattleManager>();
        EffectableUnit = EffectableUnit ?? GetComponent<EffectableUnit>();
        EffectableTargetBank = EffectableTargetBank ?? GetComponent<EffectableTargetBank>();
        UnitMovementController = UnitMovementController ?? GetComponent<UnitMovementController>();
        UnitMovementController.UnitTransform = transform;
    }

    protected void Start()
    {
        LifeBarmanager = GetComponentInChildren<LifeBarmanager>();
        RangeDetector = GetComponentInChildren<RangeDetector>();
        PathWalker = PathWalker ?? GetComponentInChildren<PathWalker>();
        Init();
    }

    public Type QueueableType { get; set; }
    public PoolObjectQueue<GenericUnitController> QueuePool { get; set; }
    public void OnEnqueue()
    {
       
    }

    public void OnDequeue()
    {
        
    }

    public ActiveObjectPool<GenericUnitController> ActivePool { get; set; }
    public List<Effect> GetEffectList()
    {
        return null;
    }

    public void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        
    }

    public List<TagDetector> GetRangeDetectors()
    {
        List<TagDetector> rds = new List<TagDetector>();
        rds.Add(RangeDetector);
        return rds;
    }

    public void UpdateRange(float RangeSizeDelta, List<TagDetector> detectorsToApplyChangeOn)
    {
        foreach (var d in detectorsToApplyChangeOn)
        {
            d.UpdateSize(RangeSizeDelta);
        }
    }

    
}
