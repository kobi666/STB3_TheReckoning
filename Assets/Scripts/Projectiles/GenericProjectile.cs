using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(CollisionDetector)),RequireComponent(typeof(DetectableCollider))]
public class GenericProjectile : MyGameObject,IQueueable<GenericProjectile>,IActiveObject<GenericProjectile>,IHasEffectAnimation
{
    public EffectAnimationController EffectAnimationController;
    public AnimationClip OnHitAnimation;
    [SerializeReference]
    public ProjectileEffect BaseProjectileEffect;

    public CollisionDetector CollisionDetector;
    public DetectableCollider DetectableCollider;

    [SerializeReference] public ProjectileMovementFunction MovementFunction;
    private ProjectileDynamicData DynamicData = new ProjectileDynamicData();
    [ShowInInspector] private Vector2 TargetPos
    {
        get => DynamicData.TargetPosition;
    }
    private int hitCounter;
    private bool projectileInitlized = false;
    

#if UNITY_EDITOR
    [TagSelectorAttribute]
# endif
    public string[] DiscoverableTags = new string[] { };
    public void AddTagToDiscoverableTags(string _tag) {
        
        for (int i = 0; i < DiscoverableTags.Length ; i++)
        {
            
            if (String.IsNullOrEmpty(DiscoverableTags[i])) {
                DiscoverableTags[i] = _tag;
                break;
            }
            if (i >= DiscoverableTags.Length -1) {
                Debug.LogWarning("No Empty Tag slots!");
            }
        }
    }
    List<string> DiscoverableTagsList = new List<string>();
    
    public List<string> TargetExclusionList = new List<string>();
    

    public void Activate()
    {
        if (SpriteRenderer != null)
        {
            SpriteRenderer.enabled = true;
        }
        if (!projectileInitlized)
        {
            InitProjectile();
        }
        MovementFunction.ExternalMovementLock = false;
        gameObject.SetActive(true);
        OnMovementEvent(transform,EffectableTarget?.transform ?? null,transform.position,TargetPosition);
    }
    
    private float MaxLifeTimeCounter = 0;
    public event Action onHitCounterZero;

    private CollisionDetector rangeDetector
    {
        get => EffectableTargetBank.Detector;
        set => effectableTargetBank.Detector = value;
    }
    private EffectableTargetBank effectableTargetBank;

    private EffectableTargetBank EffectableTargetBank
    {
        get
        {
            if (effectableTargetBank == null)
            {
                effectableTargetBank = GetComponent<EffectableTargetBank>();
            }
            return effectableTargetBank;
        }
        set => effectableTargetBank = value;
    }
    
    public void OnHitCounterZero() {
        onHitCounterZero?.Invoke();
    }
    
    
    public event Action onTargetPositionReached;
    public void OnTargetPositionReached() {
        onTargetPositionReached?.Invoke();
        OnTargetPositionReachedEffect(null,transform.position);
    }

    public event Action<Effectable,Vector2> onTargetPositionReachedEffect;

    public void OnTargetPositionReachedEffect(Effectable ef,Vector2 targetPos)
    {
        onTargetPositionReachedEffect?.Invoke(ef,ef?.transform.position ?? transform.position);
    }


    public event Action onMaxLifeTimeReached;

    public void OnMaxLifeTimeReached()
    {
        onMaxLifeTimeReached.Invoke();
    }

    void ReduceHitCounterAndInvokeOnHitCounterZero(Effectable ef,Vector2 targetPos)
    {
        hitCounter -= 1;
        if (hitCounter <= 0)
        {
            OnHitCounterZero();
        }
    }

    public event Action<Effectable,Vector2> onTargetHit;

    public void OnTargetHit(Effectable ef,Vector2 targetPos)
    {
        onTargetHit?.Invoke(ef, ef?.transform.position ?? transform.position); 
    }




    public Type QueueableType {get;set;}
    public ProjectileData Data = new ProjectileData();
    public void OnEnqueue() {
        
    }

    public void OnDequeue()
    {
        EffectableTarget = null;
    }

    public SpriteRenderer SpriteRenderer;

    public Dictionary<int,Effectable> ActiveTargets => GameObjectPool.Instance.ActiveEffectables.Pool;
    ActiveObjectPool<GenericProjectile> activePool;
    public ActiveObjectPool<GenericProjectile> ActivePool { get => activePool; set { activePool = value;}}
    string TypeTag = "Projectile";
    
    [ShowInInspector]
    public event Action<Transform, Transform, Vector2, Vector2> onMovementEvent;
    
    public void OnMovementEvent(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 targetPos)
    {
        onMovementEvent?.Invoke(projectileTransform,targetTarnsform,originPos,targetPos);
    }

    public event Action<Effectable,Vector2> onHitSingleTarget;
    public void OnHitSingleTarget(Effectable ef,Vector2 targetPos) {
        onHitSingleTarget?.Invoke(ef, ef?.transform.position ?? transform.position);
    }

    public List<EffectAnimationController> EffectAnimationControllers { get; set; } = new List<EffectAnimationController>();

    /*public void InitEffectAnimation()
    {
        if (OnHitAnimation != null)
        {
            EffectAnimationController = GameObjectPool.Instance.GetEffectAnimationQueue().Get();
            EffectAnimationController.AnimationClip = OnHitAnimation;
            if (BaseProjectileEffect.TriggersOnCollision)
            {
                onTargetHit += delegate(Effectable effectable, Vector2 vector2) { PlayAnimationOnce(effectable.transform.position); };
            }

            if (BaseProjectileEffect.OnPositionReachedEffect)
                
            {
                onTargetPositionReachedEffect += delegate(Effectable effectable, Vector2 targetPos) {  PlayAnimationOnce(targetPos);};
            }
        }
    }*/

    void PlayAnimationOnHit(Effectable ef, Vector2 targetPos)
    {
        foreach (var eac in EffectAnimationControllers)
        {
            eac.PlayOnceAtPosition(ef.transform.position);
        }
    }

    void PlayAnimationOnPosReached(Effectable ef, Vector2 pos)
    {
        foreach (var eac in EffectAnimationControllers)
        {
            eac.PlayOnceAtPosition(pos);
        }
    }
    
    
    public void OnTargetEnter(MyGameObject other) {
        if (DiscoverableTagsList.Contains(other.tag))
        {
            if (BaseProjectileEffect.TriggersOnCollision)
            {
                if (BaseProjectileEffect.TriggersOnSpecificTarget)
                {
                    if (EffectableTarget != null) {
                        if (EffectableTarget?.name == other.name)
                        {
                            int targetID = EffectableTarget.MyGameObjectID;
                            if (ActiveTargets?.ContainsKey(targetID) ?? false)
                            {
                                if (ActiveTargets[targetID].IsTargetable())
                                    OnTargetHit(ActiveTargets[targetID],ActiveTargets[targetID]?.transform.position ?? transform.position);
                                hitCounter -= 1;
                            }
                        }
                    }
                }
                else
                {
                    if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(other.MyGameObjectID) ?? false)
                    {
                        if (!TargetExclusionList.Contains(other.name)) 
                        {
                            if (ActiveTargets[other.MyGameObjectID].IsTargetable()) 
                            {
                                OnTargetHit(ActiveTargets[other.MyGameObjectID],ActiveTargets[other.MyGameObjectID]?.transform.position ?? transform.position );
                            }
                        }
                    }

                }
            }
        }
    }
    
    public void InitEffectAnimation()
    {
        if (OnHitAnimation != null)
        {
            EffectAnimationController eac = GameObjectPool.Instance.GetEffectAnimationQueue().Get();
            EffectAnimationControllers.Add(eac);
            eac.transform.position = transform.position;
            eac.AnimationClip = OnHitAnimation;
            //EffectAnimationController = eac;

            if (BaseProjectileEffect.TriggersOnCollision)
            {
                onTargetHit += PlayAnimationOnHit;
            }

            if (!BaseProjectileEffect.OnPositionReachedEffect)
            {
                onTargetPositionReachedEffect += PlayAnimationOnPosReached;
            }
        }
    }


    void PlayAnimationOnce(Vector2 targetPos)
    {
        EffectAnimationController.PlayOnceAtPosition(targetPos);
    }


    void initAOEProperties()
    {
        if (BaseProjectileEffect.AOE)
        {
            if (BaseProjectileEffect.EffectRadius <= 0)
            {
                rangeDetector.UpdateSize(1);
            }
            else
            {
                rangeDetector.UpdateSize(BaseProjectileEffect.EffectRadius);
            } 
        }
    }

    private event Action onProjectileInit;

    public void OnProjectileInit()
    {
        onProjectileInit?.Invoke();
    }

    void InitProjectileEffects()
    {
        if (BaseProjectileEffect.OnHitEffect)
        {
            foreach (var effect in BaseProjectileEffect.onHitEffects)
            {
                if (effect.IsAOE)
                {
                    onHitMultipleTargets += effect.Apply;
                    if (BaseProjectileEffect.TriggersOnCollision)
                    {
                        onTargetHit += OnHitMultipleTargets;
                    }
                }
                else
                {
                    onHitSingleTarget += effect.Apply;
                    if (BaseProjectileEffect.TriggersOnCollision)
                    {
                        onTargetHit += OnHitSingleTarget;
                    }
                }
            }

            if (BaseProjectileEffect.TriggersOnCollision)
            {
                onTargetHit += ReduceHitCounterAndInvokeOnHitCounterZero;
            }
        }

        if (BaseProjectileEffect.OnPositionReachedEffect)
        {
            foreach (var effect in BaseProjectileEffect.onPositionReachedEffects)
            {
                onTargetPositionReachedEffect += effect.Apply;
            }
        }

        
    }

    void InitProjectileMovement()
    {
        if (BaseProjectileEffect.Homing)
        {
            onMovementEvent += MovementFunction.MoveToTargetTransform;
        }
        else
        {
            onMovementEvent += MovementFunction.MoveToTargetPosition;
        }
    }
    
    

    public Action<Effectable,Vector2> onHitMultipleTargets;

    public void OnHitMultipleTargets(Effectable effectable,Vector2 targetPos)
    {
        (Effectable,bool)[] _targets = EffectableTargetBank.Targets.Values.ToArray();
        foreach (var target in _targets)
        {
            if (target.Item1 == null)
            {
                continue;
            }
            onHitMultipleTargets?.Invoke(target.Item1,target.Item1?.transform.position ?? transform.position);
        }
    }

    protected void Awake()
    {
        CollisionDetector = GetComponent<CollisionDetector>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        //onTargetPositionReached += delegate { SpriteRenderer.enabled = false; Debug.LogWarning(Time.time); };
        onTargetHit += delegate(Effectable effectable, Vector2 targetPos) { SpriteRenderer.enabled = false; };
        foreach (string item in DiscoverableTags)
        {
            if (!String.IsNullOrEmpty(item)) {
                DiscoverableTagsList.Add(item);
            }
        }
    }

    protected void Start()
    {
        EffectAnimationController = GameObjectPool.Instance.GetEffectAnimationQueue().Get();
        onHitCounterZero += delegate {gameObject.SetActive(false);};
        gameObject.tag = TypeTag;
        
    }

    //public EffectableTargetBank TargetBank {get;set;}
    PoolObjectQueue<GenericProjectile> queuePool;
    public PoolObjectQueue<GenericProjectile> QueuePool {get => queuePool;set{queuePool = value;}}
    // Start is called before the first frame update


    public Effectable EffectableTarget
    {
        get => DynamicData.EffectableTarget;
        set => DynamicData.EffectableTarget = value;
    }

    public Vector2 TargetPosition
    {
        get => DynamicData.TargetPosition;
        set => DynamicData.TargetPosition = value;

    }
    public bool targetPositionSet = false;
    
    
    public float Speed {get => Data.Speed ; set {Data.Speed = value;}}
    public int Damage { get => Data.Damage ; set {Data.Damage = value;}}
    
    

    public void InitProjectile()
    {
        InitEffectAnimation();
        if (BaseProjectileEffect != null)
        {
            InitProjectileEffects();
            initAOEProperties();
        }
        if (BaseProjectileEffect == null)
        {

            Debug.LogWarning("Init projectile called with effect null");
        }
        if (MovementFunction != null)
        {
            InitProjectileMovement();
            if (MovementFunction == null)
            {
                Debug.LogWarning("Init projectile called with movement function null" + " " + name);
            }
        
            projectileInitlized = true;
        }
        onTargetPositionReached += Disable;
        MovementFunction.onPositionReached += OnTargetPositionReached;
        
        projectileInitlized = true;
        
        
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    


    void OnEnable()
    {
        ActivePool?.AddObjectToActiveObjectPool(this);
    }

    protected void OnDisable()
    {
        TargetExclusionList.Clear();
        SpriteRenderer.enabled = false;
        DynamicData.Clear();
        MovementFunction.ExternalMovementLock = true;
        targetPositionSet = false;
        GameObjectPool.Instance.RemoveObjectFromAllPools(MyGameObjectID,name);
        QueuePool?.ObjectQueue.Enqueue(this);
        hitCounter = BaseProjectileEffect.HitCounter;
    }


    
}
