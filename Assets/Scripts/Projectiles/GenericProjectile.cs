using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;

public class GenericProjectile : SerializedMonoBehaviour,IQueueable<GenericProjectile>,IActiveObject<GenericProjectile>
{
    public ProjectileEffect BaseProjectileEffect;
    public ProjectileMovementFunction MovementFunction;

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    
    private float MaxLifeTimeCounter = 0;
    public event Action onHitCounterZero;
    private RangeDetector rangeDetector = null;
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
        OnTargetPositionReachedEffect(null);
    }

    public event Action<Effectable> onTargetPositionReachedEffect;

    public void OnTargetPositionReachedEffect(Effectable ef)
    {
        onTargetPositionReachedEffect?.Invoke(ef);
    }


    public event Action onMaxLifeTimeReached;

    public void OnMaxLifeTimeReached()
    {
        onMaxLifeTimeReached.Invoke();
    }


    public void OnTriggerEnter2D(Collider2D other) {
        if (BaseProjectileEffect.TriggersOnCollision) {
            if (BaseProjectileEffect.TriggersOnSpecificTarget)
            {
                if (other.name == EffectableTarget.name)
                {
                    string n = EffectableTarget.name;
                    if (ActiveTargets?.ContainsKey(n) ?? false) {
                        if (ActiveTargets[n].IsTargetable())
                            OnTargetHit(ActiveTargets[n]);
                        BaseProjectileEffect.HitCounter -= 1;
                    }
                }
            }
            else { 
                if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(other.name) ?? false) {
                    if (ActiveTargets[other.name].IsTargetable())
                        OnTargetHit(ActiveTargets[other.name]);
                    BaseProjectileEffect.HitCounter -= 1;
                }
                
            }
        }
        if (BaseProjectileEffect.HitCounter == 0) {
            OnHitCounterZero();
        }
    }

    public event Action<Effectable> onTargetHit;

    public void OnTargetHit(Effectable ef)
    {
        onTargetHit?.Invoke(ef); 
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
    
    public event Action<Transform, Transform, Vector2, Vector2> onMovementEvent;
    
    public void OnMovementEvent(Transform projectileTransform, Transform targetTarnsform, Vector2 originPos,
        Vector2 targetPos)
    {
        onMovementEvent?.Invoke(projectileTransform,targetTarnsform,originPos,targetPos);
    }

    public event Action<Effectable> onHitSingleTarget;
    public void OnHitSingleTarget(Effectable ef) {
        onHitSingleTarget?.Invoke(ef);
    }


    void initAOEProperties()
    {
        if (BaseProjectileEffect.AOE)
        {
            if (BaseProjectileEffect.EffectRadius <= 0)
            {
                rangeDetector.SetRangeRadius(1);
            }
            else
            {
                rangeDetector.SetRangeRadius(BaseProjectileEffect.EffectRadius);
            } 
        }
    }

    private void Awake()
    {
        onProjectileInit += InitProjectileMovement;
        onProjectileInit += InitProjectileEffects;
        onProjectileInit += initAOEProperties;
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
                }
                else
                {
                    onHitSingleTarget += effect.Apply;
                }
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
            onMovementEvent = MovementFunction.MoveToTargetTransform;
        }
        else
        {
            onMovementEvent = MovementFunction.MoveToTargetPosition;
        }
    }
    
    

    public Action<Effectable> onHitMultipleTargets;

    public void OnHitMultipleTargets(Effectable effectable)
    {
        Effectable[] _targets = EffectableTargetBank.Targets.Values.ToArray();
        foreach (Effectable target in _targets)
        {
            onHitMultipleTargets?.Invoke(target);
        }
    }

    protected void Start()
    {
        //onTargetPositionReached += delegate { targetPositionSet = false; };
        onHitCounterZero += delegate {gameObject.SetActive(false);};
        SpriteRenderer = GetComponent<SpriteRenderer>() ?? null;
        if (OnHitAnimation != null) {
            onHitAnimationQueuePool = GameObjectPool.Instance.GetSingleAnimationObjectQueue(OnHitAnimation);
            onTargetHit += delegate(Effectable effectable) { PlayOnHitAnimation(effectable); };
        }
        gameObject.tag = TypeTag;
    }

    //public EffectableTargetBank TargetBank {get;set;}
    PoolObjectQueue<GenericProjectile> queuePool;
    public PoolObjectQueue<GenericProjectile> QueuePool {get => queuePool;set{queuePool = value;}}
    // Start is called before the first frame update
    public Effectable EffectableTarget { get => Data.EffectableTarget ; set { Data.EffectableTarget = value;}}

    public Vector2 TargetPosition { get => Data.TargetPosition; set {
        Data.TargetPosition = value;
        targetPositionSet = true;
        }}
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


    void OnEnable()
    {
        //MovementProgressCounter = 0f;
        RangeDetector.enabled = true;
        ActivePool?.AddObjectToActiveObjectPool(this);
        BaseProjectileEffect.HitCounter = 1;
    }

    protected void OnDisable()
    {
        RangeDetector.enabled = false;
        targetPositionSet = false;
        EffectableTarget = null;
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
        QueuePool?.ObjectQueue.Enqueue(this);
    }





}
