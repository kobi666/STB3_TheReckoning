using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
using Sirenix.Serialization;

public class GenericProjectile : SerializedMonoBehaviour,IQueueable<GenericProjectile>,IActiveObject<GenericProjectile>
{
    [OdinSerialize]
    public ProjectileEffect BaseProjectileEffect;
    [OdinSerialize]
    public ProjectileMovementFunction MovementFunction;
    private ProjectileDynamicData DynamicData = new ProjectileDynamicData();
    private int hitCounter;
    private bool projectileInitlized = false;

    public void Activate()
    {
        gameObject.SetActive(true);
        OnMovementEvent(transform,EffectableTarget.transform,transform.position,TargetPosition);
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
                        hitCounter -= 1;
                    }
                }
            }
            else { 
                if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(other.name) ?? false) {
                    if (ActiveTargets[other.name].IsTargetable())
                        OnTargetHit(ActiveTargets[other.name]);
                    hitCounter -= 1;
                }
                
            }
        }
        if (hitCounter == 0) {
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
    
    [ShowInInspector]
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
            onMovementEvent += MovementFunction.MoveToTargetTransform;
        }
        else
        {
            onMovementEvent += MovementFunction.MoveToTargetPosition;
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
        //onHitCounterZero += delegate {gameObject.SetActive(false);};
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
    }

    /*public void initProjectileArgs(ProjectileEffect pe, ProjectileMovementFunction pm)
    {
        BaseProjectileEffect = pe;
        MovementFunction = pm;
        Debug.LogWarning("pm null:" +  (pm == null));
        Debug.LogWarning("projectile movement null" + (MovementFunction == null));
        if (BaseProjectileEffect!= null) {
        InitProjectileEffects();
        initAOEProperties();
        if (MovementFunction != null)
        {
            InitProjectileMovement();
        }
        }
        else
        {
            Debug.LogWarning("NULLLZZZ");
        }
            
    }*/


    void OnEnable()
    {
        if (!projectileInitlized)
        {
            InitProjectile();
        }
        RangeDetector.enabled = true;
        ActivePool?.AddObjectToActiveObjectPool(this);
        hitCounter = BaseProjectileEffect.HitCounter;
    }

    protected void OnDisable()
    {
        DynamicData.Clear();
        RangeDetector.enabled = false;
        targetPositionSet = false;
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
        QueuePool?.ObjectQueue.Enqueue(this);
    }





}
