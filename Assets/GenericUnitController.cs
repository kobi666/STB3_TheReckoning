using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


[Searchable][Serializable]
public class GenericUnitController : MyGameObject,IQueueable<GenericUnitController>,IActiveObject<GenericUnitController>,IHasEffects,IHasRangeComponents,IHasStateMachine
{
    private bool firstRun = true;
    private void OnEnable()
    {
        if (!firstRun) {
            Init();
        }
    }

    private void OnDisable()
    {
        GameObjectPool.Instance.OnUnitDisable(MyGameObjectID);
    }


    [FormerlySerializedAs("UnitStates")] [SerializeField][FoldoutGroup("states")]
    public List<UnitState> States =  new List<UnitState>();
    
    public bool WalksOnPath;
    public UnitLifeManager UnitLifeManager = new UnitLifeManager();
    public UnitData Data = new UnitData();
#if UNITY_EDITOR
    [TagSelector]
# endif
    [SerializeField,ValidateInput("gtNotEmpty")]
    public string GroupTag;
#if UNITY_EDITOR
    [TagSelector]
# endif
    [SerializeField,ValidateInput("ttNotEmpty")]
    public string[] TargetTags;
    private BoxCollider2D selfCollider;


    bool ttNotEmpty()
    {
        return !TargetTags.IsNullOrEmpty();
    }

    bool gtNotEmpty()
    {
        return !GroupTag.IsNullOrEmpty();
    }


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
    public CollisionDetector CollisionDetector;
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

    public float Proximity => PathWalker.ProximityToPathEnd;

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
        if (!TargetTags.IsNullOrEmpty()) {
        UnitBattleManager.MeleeWeapon.TargetBank.DiscoverableTags = TargetTags.ToList();
        GameObjectPool.Instance.OnUnitEnable(this);
        }
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
        onDeath += SetStateToDeath;
    }

    private void SetStateToDeath()
    {
        //Debug.LogError("!!! " + gameObject.name);
        StateMachine.SetState(UnitStates.Death.ToString());
    }

    protected void Start()
    {
        LifeBarmanager = GetComponentInChildren<LifeBarmanager>();
        PathWalker = PathWalker ?? GetComponentInChildren<PathWalker>();
        if (Data.DynamicData.BasePosition == null)
        {
            Data.DynamicData.BasePosition = new Vector2();
        }
        Init();
        firstRun = false;
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

    public List<CollisionDetector> GetTagDetectors()
    {
        List<CollisionDetector> rds = new List<CollisionDetector>();
        rds.Add(CollisionDetector);
        return rds;
    }

    public void UpdateRange(float RangeSizeDelta, List<CollisionDetector> detectorsToApplyChangeOn)
    {
        foreach (var d in detectorsToApplyChangeOn)
        {
            d?.UpdateSize(RangeSizeDelta);
        }
    }


    public void OnEnterStateDefaultBehavior()
    {
        EffectableUnit.IsTargetable();
    }
}
