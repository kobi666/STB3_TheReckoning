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
    public ProjectileBehaviorData ProjectileBehaviorData = new ProjectileBehaviorData();

    public void Activate()
    {
        gameObject.SetActive(true);
    }
    
    public bool DirectHitProjectile = false;
    public bool AOEProjectile = false;
    public bool SingleTargetProjectile;
    public bool HomingProjectile;
    public float AOERadius = 1;
    
    public int HitCounter = 1;
    private float MaxLifeTime = 3;
    private float MaxLifeTimeCounter = 0;
    public event Action onHitCounterZero;
    private RangeDetector rangeDetector = null;
    public float assistingFloat1;
    public float assistingFloat2;
    private EffectableTargetBank effectableTargetBank;
    private float MovementProgressCounter;
    
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
    }


    public event Action onMaxLifeTimeReached;

    public void OnMaxLifeTimeReached()
    {
        onMaxLifeTimeReached.Invoke();
    }


    public async void StartAsyncMovement()
    {
        
        MovementFunction();
            if ((Vector2)transform.position == TargetPosition)
            {
                OnTargetPositionReached();
            }
            if (HomingProjectile)
        {
            if (transform.position == EffectableTarget.transform.position)
            {
                OnTargetPositionReached();
            }
        }
    }
    
    
    public void OnTriggerEnter2D(Collider2D other) {
        if (DirectHitProjectile) {
            if (GameObjectPool.Instance.ActiveEffectables?.Pool.ContainsKey(other.name) ?? false) {
                if (ActiveTargets[other.name].IsTargetable())
                    OnHit(ActiveTargets[other.name]);
                HitCounter -= 1;
                if (HitCounter == 0) {
                    OnHitCounterZero();
                }
            }
        }
    }

    public event Action<Effectable> onHit;

    public void OnHit(Effectable ef)
    {
        onHit?.Invoke(ef); 
    }

    void onHitSingle(Effectable ef)
    {
        OnHitSingleTarget(ef);
    }

    void onHitMulti(Effectable ef)
    {
        OnHitMultipleTargets(ef);
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

    public event ProjectileMovementDelegate onMainMovementAction;

    public void OnMainMovementAction(Transform selfTransform, Vector2 originPos, Vector2 targetPos, float speed, float assistingfloat1,
        ref float movementProgressCounter)
    {
        // ////////// //////// ///////  onMainMovementAction?.Invoke(selfTransform, originPos, targetPos,speed,assistingfloat1,ref movementProgressCounter);
    }



    public event Action movementEvent;
    
    public void MovementEvent()
    {
        movementEvent?.Invoke();
    }
    
    public void MovementFunction()
    {
        MovementEvent();
    }

    void HomingMovement()
    {
        OnMainMovementAction(transform,Data.OriginPosition,EffectableTarget.transform.position,Speed,assistingFloat1, ref MovementProgressCounter);
    }

    void MoveToTargetPosition()
    {
        OnMainMovementAction(transform,Data.OriginPosition,TargetPosition,Speed,assistingFloat1,ref MovementProgressCounter);
    }
    
    public event Action<Effectable> onHitSingleTarget;
    public void OnHitSingleTarget(Effectable ef) {
        if (HomingProjectile) {
            if (ef.name != EffectableTarget.name) {
        onHitSingleTarget?.Invoke(ef);
            }
        }
        else
        {
            onHitSingleTarget?.Invoke(ef);
        }
    }

    public void OnHitSpecificTarget(Effectable ef)
    {
        if (ef.name == EffectableTarget.name)
        {
            onHitSingleTarget?.Invoke(ef);
        } 
    }


    void SetProjectileActions()
    {
        if (HomingProjectile)
        {
            movementEvent += HomingMovement;
        }
        else
        {
            movementEvent += MoveToTargetPosition;
        }
        
        
        
        if (SingleTargetProjectile)
        {
            onHit += OnHitSingleTarget;
        }

        if (AOEProjectile)
        {
            onHit += OnHitMultipleTargets;
        }

        if (HomingProjectile)
        {
            onHit += OnHitSpecificTarget;
        }
        
    }
    
    

    public Action<Effectable[]> onHitMultipleTargets;

    public void OnHitMultipleTargets(Effectable effectable)
    {
        Effectable[] _targets = EffectableTargetBank.Targets.Values.ToArray();
        onHitMultipleTargets?.Invoke(_targets);
    }

    protected void Start()
    {
        onTargetPositionReached += delegate { targetPositionSet = false; };
        onHitCounterZero += delegate {gameObject.SetActive(false);};
        SpriteRenderer = GetComponent<SpriteRenderer>() ?? null;
        if (OnHitAnimation != null) {
            onHitAnimationQueuePool = GameObjectPool.Instance.GetSingleAnimationObjectQueue(OnHitAnimation);
            onHit += delegate(Effectable effectable) { PlayOnHitAnimation(effectable); };
        }
        gameObject.tag = TypeTag; 
        SetProjectileActions();
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
        MovementProgressCounter = 0f;
        RangeDetector.enabled = true;
        ActivePool?.AddObjectToActiveObjectPool(this);
        HitCounter = 1;
    }

    protected void OnDisable()
    {
        
        RangeDetector.enabled = false;
        targetPositionSet = false;
        EffectableTarget = null;
        GameObjectPool.Instance.RemoveObjectFromAllPools(name,name);
        QueuePool?.ObjectQueue.Enqueue(this);
    }

    private void Update() {
        if (targetPositionSet) {
        MovementFunction();
        if ((Vector2)transform.position == TargetPosition)
            {
                OnTargetPositionReached();
            }
        }

        if (HomingProjectile)
        {
            if (transform.position == EffectableTarget.transform.position)
            {
                OnTargetPositionReached();
            }
        }
    }

    

    

}
