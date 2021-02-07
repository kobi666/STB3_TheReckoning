using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenericUnitController : MonoBehaviour,IQueueable<GenericUnitController>,IActiveObject<GenericUnitController>,IHasEffects,IHasRangeComponent
{
    public bool WalksOnPath;
    private UnitLifeManager UnitLifeManager = new UnitLifeManager();
    public UnitData Data = new UnitData();
    [TagSelector]
    public string GroupTag;

    public float UnitDiscoveryRadius = 1;
    //public GenericStateMachine StateMachine;
    public AnimationController AnimationController;
    public SpriteRenderer SpriteRenderer;
    public UnitBattleManager UnitBattleManager;
    public LifeBarmanager LifeBarmanager;
    public RangeDetector RangeDetector;
    public EffectableUnit EffectableUnit;
    public EffectableTargetBank EffectableTargetBank;
    

    void Init()
    {
        UnitLifeManager.InitialHP = Data.HP;
        UnitLifeManager.Init();
        UpdateRange(Data.DiscoveryRadius,GetRangeDetectors());
    }
    
    protected void Awake()
    {
        tag = GroupTag;
        //StateMachine = StateMachine ?? GetComponent<GenericStateMachine>();
        AnimationController = AnimationController ?? GetComponent<AnimationController>();
        SpriteRenderer = SpriteRenderer ?? GetComponent<SpriteRenderer>();
        UnitBattleManager = UnitBattleManager ?? GetComponent<UnitBattleManager>();
        EffectableUnit = EffectableUnit ?? GetComponent<EffectableUnit>();
        EffectableTargetBank = EffectableTargetBank ?? GetComponent<EffectableTargetBank>();
        
    }

    protected void Start()
    {
        LifeBarmanager = GetComponentInChildren<LifeBarmanager>();
        RangeDetector = GetComponentInChildren<RangeDetector>();
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
