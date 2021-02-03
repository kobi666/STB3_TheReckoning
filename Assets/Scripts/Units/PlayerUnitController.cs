using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Animancer;

public abstract class PlayerUnitController : UnitController,ITypeTag,IActiveObject<PlayerUnitController>,IQueueable<PlayerUnitController>
{
    public void ApplyEffectOnTarget()
    {
        foreach (var ef in AttackEffects)
        {
            ef.Apply(Data.EffectableTarget,Data.EffectableTarget.transform.position);
        }
    }
    
    
    
    static string Tag = "Player_Unit";
    public string TypeTag {get => Tag;}
    private int unitBaseIndex = 0;
    public int UnitBaseIndex {
        get => unitBaseIndex;
        set {
            unitBaseIndex = value;
        }
    }
    public AnimationClip IdleAnimation;
    public AnimationClip WalkingAnimation;
    public AnimationClip AttackAnimation;
    public abstract bool CanEnterNewBattle();

    public event Action<PlayerUnitController> onAttack;
    public  void OnAttack() {
        onAttack?.Invoke(this);
    }
    public EnemyUnitController Target { get => Data.EnemyTarget ?? null;}
    
    // Start is called before the first frame update
    public bool isEnemyTargetSlotEmpty {
        get {
            if (Data.EnemyTarget == null) {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    private void PlayAttackAnimation(PlayerUnitController pc) {
        animationController.OnDirectBattleAttack();
    }

    

    public abstract IEnumerator OnEnterJoinBattle();
    public abstract IEnumerator OnExitJoinBattle();

    

    

    
    public abstract void OnTargetEnteredRange(EnemyUnitController ec);
        
    

    // void checkOrSetSingleTarget() {
    //     if (isEnemyTargetSlotEmpty) {
    //             Data.EnemyTarget = TargetBank.FindSingleTargetNearestToEndOfSpline();
    //     }
    // }

    void Test() {
        Debug.LogWarning("Target found, its name is : " + (Data.EnemyTarget?.name ?? "No Object"));
    }

    
    
    public abstract void LateStart();
    

    private void Start() {
        gameObject.tag = TypeTag;
        TargetBank.targetEnteredRange += OnTargetEnteredRange;
        States.JoinBattle.OnEnterState += OnEnterJoinBattle;
        States.JoinBattle.OnExitState += OnExitJoinBattle;
        DeathManager.Instance.onEnemyUnitDeath += Data.SetEnemyTargetToNull;
        States.Default.StateAnimation = IdleAnimation;
        States.PreBattle.StateAnimation = WalkingAnimation;
        onAttack += PlayAttackAnimation;
        LateStart();
    }


    public ActiveObjectPool<PlayerUnitController> ActivePool { get; set; }

    private PoolObjectQueue<PlayerUnitController> queuePool;
    public PoolObjectQueue<PlayerUnitController> QueuePool { get => queuePool; set => queuePool = value; }
    public List<Effect> GetEffectList()
    {
        throw new NotImplementedException();
    }

    public void UpdateEffect(Effect ef, List<Effect> appliedEffects)
    {
        throw new NotImplementedException();
    }
}
