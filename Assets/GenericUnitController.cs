using System;
using System.Collections;
using System.Collections.Generic;
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
    public AnimationController AnimationController;
    public SpriteRenderer SpriteRenderer;
    public UnitBattleManager UnitBattleManager;
    public LifeBarmanager LifeBarmanager;
    public TagDetector RangeDetector;
    public EffectableUnit EffectableUnit;
    public EffectableTargetBank EffectableTargetBank;
    public UnitStateMachine StateMachine;
    public PathWalker PathWalker;
    

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
