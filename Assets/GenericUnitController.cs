﻿using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


[Serializable]
public class GenericUnitController : MonoBehaviour,IQueueable<GenericUnitController>,IActiveObject<GenericUnitController>,IHasEffects,IHasRangeComponents,IHasStateMachine
{
    private void OnEnable()
    {
        GameObjectPool.Instance.OnUnitEnable(this);
    }

    private void OnDisable()
    {
        GameObjectPool.Instance.OnUnitDisable(name);
    }


    [FormerlySerializedAs("UnitStates")] [SerializeField][FoldoutGroup("states")]
    public List<UnitState> States =  new List<UnitState>();
    
    public bool WalksOnPath;
    public UnitLifeManager UnitLifeManager = new UnitLifeManager();
    public UnitData Data = new UnitData();
    [TagSelector]
    public string GroupTag;
    private BoxCollider2D selfCollider;



    private bool xDirection;
    private bool XDirection
    {
        get => xDirection;
        set
        {
            if (value != xDirection)
            {
                UnitBattleManager.FlipAttackArea(value);
                SpriteRenderer.flipX = value;
            }

            xDirection = value;
        }
    }
    public void FlipDirection(Vector2 targetPos)
    {
        if (transform.position.x > targetPos.x)
        {
            XDirection = true;
        }

        if (transform.position.x < targetPos.x)
        {
            XDirection = false;
        }
    }





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
        UpdateRange(Data.MetaData.DiscoveryRadius, GetTagDetectors());
        foreach (var s in States)
        {
            s.Init(this);
        }
        StateMachine.Init(this,States);
        UnitLifeManager.onUnitDeath += OnDeath;
    }

    public event Action onDeath;
    void OnDeath()
    {
        onDeath?.Invoke();
    }
    
    
    
    protected void Awake()
    {
        selfCollider = GetComponent<BoxCollider2D>();
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
        UnitMovementController.GenericUnitController = this;
        onDeath += delegate { StateMachine.SetState(UnitStates.Death.ToString()); };
    }

    protected void Start()
    {
        LifeBarmanager = GetComponentInChildren<LifeBarmanager>();
        RangeDetector = GetComponentInChildren<RangeDetector>();
        PathWalker = PathWalker ?? GetComponentInChildren<PathWalker>();
        if (Data.DynamicData.BasePosition == null)
        {
            Data.DynamicData.BasePosition = new Vector2();
        }
        Init();
    }

    public Type QueueableType { get; set; }
    public PoolObjectQueue<GenericUnitController> QueuePool { get; set; }
    public void OnEnqueue()
    {
       
    }

    public void OnDequeue()
    {
        gameObject.SetActive(false);
    }

    public ActiveObjectPool<GenericUnitController> ActivePool { get; set; }
    public List<Effect> GetEffectList()
    {
        return null;
    }

    public void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        
    }

    public void SetEffectList(List<Effect> effects)
    {
        Debug.LogWarning("Redundent");
    }

    public float rangeSize { get => Data.MetaData.DiscoveryRadius; set => Data.MetaData.DiscoveryRadius = value; }

    public List<TagDetector> GetTagDetectors()
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


    public void OnEnterStateDefaultBehavior()
    {
        EffectableUnit.IsTargetable();
    }
}
