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

    public bool ReducesLevelUnitsOnDeath = false;
    
    
    private bool firstRun = true;
    private void OnEnable()
    {
        if (!firstRun)
        {
            if (!StateMachine.CurrentState.StateIsRunning)
            {
                StateMachine.SetStateForce(StateMachine.InitialState);
                if (DeququeCounter >= 1)
                {
                    Debug.LogWarning("dequeue counter :"+ DeququeCounter + " , Cu");
                }
            }

            SpriteRenderer.color = BaseColor;
        }
        GameObjectPool.Instance.OnUnitEnable(this);
        UnitLifeManager.UnitDied = false;
    }

    private void AddMoney()
    {
        GameManager.Instance.UpdateMoney(MoneyOnDeath);
    }

    public int MoneyOnDeath = 0;
    private bool addMoneyOnDeath = true;
    
    public int DamageToBase = 1;






    private void OnDisable()
    {
        GameObjectPool.Instance.OnUnitDisable(MyGameObjectID);
        QueuePool.ObjectQueue.Enqueue(this);
        StateMachine.runLock = false;
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
    public void FlipDirection(Vector2? targetPos)
    {
        if (targetPos != null) {
            if (transform?.position.x > targetPos?.x)
            {
                XDirection = true;
            }

            if (transform?.position.x < targetPos?.x)
            {
                XDirection = false;
            }
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
        UnitMovementController.MovementSpeed = Data.MetaData.MovementSpeed;
        UnitMovementController.spline = PathWalker.spline;
        UnitMovementController.SplinePathController = PathWalker.SplinePathController;
        UnitLifeManager.InitialHP = Data.MetaData.HP;
        UnitLifeManager.Init();
        UpdateRange(Data.MetaData.DiscoveryRadius, GetTagDetectors());
        foreach (var s in States)
        {
            s.Init(this);
        }
        StateMachine.Init(this,States);
        StateMachine.onStateChange += CheckTargtableState;
        UnitLifeManager.onUnitDeath += OnDeath;
        if (!TargetTags.IsNullOrEmpty()) {
        UnitBattleManager.MeleeWeapon.TargetBank.DiscoverableTags = TargetTags.ToList();
        
        }
        
    }

    void CheckTargtableState()
    {
        var b = EffectableUnit?.IsTargetable();
    }

    public event Action onDeath;
    void OnDeath()
    {
        onDeath?.Invoke();
    }


    public DetectableCollider DetectableCollider;

    private Color BaseColor = new Color();
    protected void Awake()
    {
        BaseColor = SpriteRenderer.color;
        DetectableCollider = GetComponent<DetectableCollider>();
        selfCollider = GetComponent<BoxCollider2D>();
        StateMachine = GetComponent<UnitStateMachine>();
        //tag = GroupTag;
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
        onDeath += PlayDeathAnimation;
        onDeath += delegate { DetectableCollider.UnSubscribeFromGWCS(); };
        if (ReducesLevelUnitsOnDeath)
        {
            onDeath += delegate { GameManager.Instance.CurrentLevelManager.CurrentUnitsInLevel--; };
        }
        onDeath += delegate { DetectableCollider.DebugCollider = true; };
    }

    void PlayDeathAnimation()
    {
        AnimationController.PlayFiniteAnimation(UnitBattleManager.DeathAnimation);
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
        onDeath += AddMoney;
        firstRun = false;
    }

    public Type QueueableType { get; set; }

    private PoolObjectQueue<GenericUnitController> queuePool;
    public PoolObjectQueue<GenericUnitController> QueuePool { get => queuePool; set => queuePool = value; }
    public void OnEnqueue()
    {
        UnitBattleManager.TargetUnit = null;
    }

    public int DeququeCounter = 0;
    public void OnDequeue()
    {
        DeququeCounter++;
        UnitLifeManager.HP = Data.MetaData.HP;
        StateMachine.ChangeState(StateMachine.InitialState);
        StateMachine.ExecuteCurrentState();
    }

    public ActiveObjectPool<GenericUnitController> ActivePool { get; set; }
    public List<Effect> GetEffectList()
    {
        return null;
    }

    public void UpdateEffect(Effect ef)
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
