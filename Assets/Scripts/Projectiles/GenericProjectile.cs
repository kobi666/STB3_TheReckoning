using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(DetectableCollider))]
public class GenericProjectile : MyGameObject,IQueueable<GenericProjectile>,IActiveObject<GenericProjectile>,IHasEffectAnimation
{
    public EffectAnimationController EffectAnimationController;
    public AnimationClip OnHitAnimation;
    [SerializeReference]
    public ProjectileEffect BaseProjectileEffect;

    public ProjectileFamily ProjectileFamily;
    
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
    
    public List<int> TargetExclusionList = new List<int>();
    

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

    public CollisionDetector AOERangeDetector
    {
        get => EffectableTargetBank.Detector;
    }

    [Required] public EffectableTargetBank EffectableTargetBank;

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
    
    
    public void OnTargetEnter(int targetCollisionID)
    {
        int GID = GameObjectPool.CollisionIDToGameObjectID[targetCollisionID].Item1;
        if (BaseProjectileEffect.TriggersOnCollision)
            {
                if (BaseProjectileEffect.TriggersOnSpecificTarget)
                {
                    if (EffectableTarget != null) {
                        if (EffectableTarget?.MyGameObjectID == GID)
                        {
                            if (ActiveTargets?.ContainsKey(GID) ?? false)
                            {
                                if (ActiveTargets[GID].IsTargetable())
                                    OnTargetHit(ActiveTargets[GID],ActiveTargets[GID]?.transform.position ?? transform.position);
                                hitCounter -= 1;
                            }
                        }
                    }
                }
                else
                {
                    if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(GID) ?? false)
                    {
                        if (!TargetExclusionList.Contains(GID)) 
                        {
                            if (ActiveTargets[GID].IsTargetable()) 
                            {
                                OnTargetHit(ActiveTargets[GID],ActiveTargets[GID]?.transform.position ?? transform.position );
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
                AOERangeDetector.UpdateSize(1);
            }
            else
            {
                AOERangeDetector.UpdateSize(BaseProjectileEffect.EffectRadius);
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
                if (effect.IsAOE)
                {
                    onHitMultipleTargets += effect.Apply;
                    onTargetPositionReachedEffect += OnHitMultipleTargets;
                }
                else
                {
                    onTargetPositionReachedEffect += effect.Apply;    
                }
                
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
        CollisionDetector.onTargetEnter += OnTargetEnter;
        //CollisionDetector = GetComponent<CollisionDetector>();
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
        firstRun = false;
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
        MovementFunction.onPositionReached += OnTargetPositionReached;
        onTargetPositionReached += DisableAfterPeriod;
        
        
        projectileInitlized = true;
        
        
    }

    async void DisableAfterPeriod()
    {
        for (int i = 0; i < 2; i++)
        {
            await Task.Yield();
        }
        gameObject.SetActive(false);
    }

    


    void OnEnable()
    {
        ActivePool?.AddObjectToActiveObjectPool(this);
        if (!firstRun) {
        MovementFunction.posreachedLock = false;
        }
    }

    private bool firstRun = true;

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


public enum ProjectileFamily
{
    Reguler,
    Energy,
    Ballistic,
    someotherBS
}