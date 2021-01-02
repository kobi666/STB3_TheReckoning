using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using Sirenix.Serialization;
using UnityEditor;

public class GenericProjectile : MonoBehaviour,IQueueable<GenericProjectile>,IActiveObject<GenericProjectile>
{
    [SerializeReference]
    public ProjectileEffect BaseProjectileEffect;

    [SerializeReference] public ProjectileMovementFunction MovementFunction;
    private ProjectileDynamicData DynamicData = new ProjectileDynamicData();
    [ShowInInspector] private Vector2 TargetPos
    {
        get => DynamicData.TargetPosition;
    }
    private int hitCounter;
    private bool projectileInitlized = false;
    private Collider2D selfCollider;
    
    
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

    private RangeDetector rangeDetector
    {
        get => EffectableTargetBank.Detector as RangeDetector;
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

    public RangeDetector RangeDetector
    {
        get
        {
            if (rangeDetector == null)
            {
                rangeDetector = GetComponentInChildren<RangeDetector>();
            }

            return rangeDetector;
        }
        set => rangeDetector = value;
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


    public void OnTriggerEnter2D(Collider2D other) {
        if (DiscoverableTagsList.Contains(other.tag))
        {
            if (BaseProjectileEffect.TriggersOnCollision)
            {
                if (BaseProjectileEffect.TriggersOnSpecificTarget)
                {
                    if (EffectableTarget != null) {
                        if (EffectableTarget?.name == other.name)
                        {
                            string n = EffectableTarget.name;
                            if (ActiveTargets?.ContainsKey(n) ?? false)
                            {
                                if (ActiveTargets[n].IsTargetable())
                                    OnTargetHit(ActiveTargets[n],ActiveTargets[n]?.transform.position ?? transform.position);
                                hitCounter -= 1;
                            }
                        }
                    }
                }
                else
                {
                    if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(other.name) ?? false)
                    {
                        if (!TargetExclusionList.Contains(other.name)) 
                        {
                            if (ActiveTargets[other.name].IsTargetable()) 
                            {
                                OnTargetHit(ActiveTargets[other.name],ActiveTargets[other.name]?.transform.position ?? transform.position );
                            }
                        }
                    }

                }
            }
        }
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

    public SpriteRenderer SpriteRenderer;
    public SingleAnimationObject OnHitAnimation;
    PoolObjectQueue<SingleAnimationObject> onHitAnimationQueuePool;

    public Dictionary<string,Effectable> ActiveTargets => GameObjectPool.Instance.ActiveEffectables.Pool;
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


    void initAOEProperties()
    {
        if (BaseProjectileEffect.AOE)
        {
            if (BaseProjectileEffect.EffectRadius <= 0)
            {
                rangeDetector.SetSize(1);
            }
            else
            {
                rangeDetector.SetSize(BaseProjectileEffect.EffectRadius);
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
        Effectable[] _targets = EffectableTargetBank.Targets.Values.ToArray();
        foreach (Effectable target in _targets)
        {
            if (target == null)
            {
                continue;
            }
            onHitMultipleTargets?.Invoke(target,target?.transform.position ?? transform.position);
        }
    }

    protected void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        //onTargetPositionReached += delegate { SpriteRenderer.enabled = false; Debug.LogWarning(Time.time); };
        onTargetHit += delegate(Effectable effectable, Vector2 targetPos) { SpriteRenderer.enabled = false; };
        selfCollider = GetComponent<Collider2D>();
        foreach (string item in DiscoverableTags)
        {
            if (!String.IsNullOrEmpty(item)) {
                DiscoverableTagsList.Add(item);
            }
        }
    }

    protected void Start()
    {
        //onTargetPositionReached += delegate { targetPositionSet = false; };
        onHitCounterZero += delegate {gameObject.SetActive(false);};
        
        if (OnHitAnimation != null) {
            onHitAnimationQueuePool = GameObjectPool.Instance.GetSingleAnimationObjectQueue(OnHitAnimation);
            onTargetHit += delegate(Effectable effectable,Vector2 targetPos) { PlayOnHitAnimation(effectable); };
        }
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

    

    public void PlayOnHitAnimation(Effectable ef) {
        SingleAnimationObject sao = onHitAnimationQueuePool.Get();
        sao.transform.position = transform.position;
        sao.gameObject.SetActive(true);
        sao.PlayOnceAndDisable();
    }

    public void PlayOnHitAnimationAtPosition(Effectable effectable, Vector2 TargetPosition) {
        SingleAnimationObject sao = onHitAnimationQueuePool.Get();
        sao.transform.position = TargetPosition;
        sao.gameObject.SetActive(true);
        sao.PlayOnceAndDisable();
    }

    public void InitProjectile()
    {
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
        RangeDetector.enabled = false;
        RangeDetector.enabled = true;
        selfCollider.enabled = false;
        selfCollider.enabled = true;
        ActivePool?.AddObjectToActiveObjectPool(this);
        
    }

    protected void OnDisable()
    {
        TargetExclusionList.Clear();
        SpriteRenderer.enabled = false;
        DynamicData.Clear();
        RangeDetector.enabled = false;
        MovementFunction.ExternalMovementLock = true;
        targetPositionSet = false;
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
        QueuePool?.ObjectQueue.Enqueue(this);
        hitCounter = BaseProjectileEffect.HitCounter;
    }





}
