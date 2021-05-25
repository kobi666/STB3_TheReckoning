using System;
using System.Collections.Generic;
using System.Linq;
using MyBox;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnitBehaviors
{
    public class AnnounceDeath : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            DeathManager.Instance.OnUnitDeath(UnitObject.MyGameObjectID);
            UnitObject.DetectableCollider.UnSubscribeFromGWCS();
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }

    public class MoveToBeginingOfSpline : UnitConcurrentBehavior
    {
        private PathWalker PathWalker;
        private Vector2? BeginingOfSpline = null;
        public override void Behavior()
        {
            UnitObject.UnitMovementController.MoveTowardsTargetAsync(BeginingOfSpline);
        }

        void RecheckPosition()
        {
            BeginingOfSpline = PathWalker.spline.Curve.Points[0].PositionWorld;
        }

        public override void InitBehavior()
        {
            PathWalker = UnitObject.PathWalker;
            BeginingOfSpline = PathWalker.spline.Curve.Points[0].PositionWorld;
            //PathWalker.SplinePathController.parentPath.onPathUpdate += RecheckPosition;
        }

        public override bool ExecCondition()
        {
            return PathWalker.SplineAttached && BeginingOfSpline != null && UnitObject.transform.position != BeginingOfSpline;
        }
    }
    
    
    public class WalkAlongSpline : UnitConcurrentBehavior
    {
        public float Distance = 0;
        public float MovementSpeed;
        [Required]
        private PathWalker PathWalker;
        
        public override void Behavior()
        {
            PathWalker.OnPathMovement(StaticObjects.DeltaGameTime * MovementSpeed);
        }

        public override void InitBehavior()
        {
            PathWalker = UnitObject.PathWalker;
            MovementSpeed = UnitObject.UnitMovementController.MovementSpeed;
        }

        public override bool ExecCondition()
        {
            bool b = PathWalker.EndOfPathReached == false;
            return b;
        }
    }

    

    public class WaitForEnemiesToEnterRange : UnitConcurrentBehavior
    {
        public CollisionDetector Detector;
        
        private bool TargetableInRange;
        public override void Behavior()
        {
            
        }

        public override void InitBehavior()
        {
            Detector = Detector ?? UnitObject.CollisionDetector;
        }

        public override bool ExecCondition()
        {
            return !TargetBank.HasTargetableTargets;
        }
    }
    
    [Serializable][GUIColor(1,1,1)]
    public class BehaviorToState
    {
        [TypeFilter("GetBehaviors")][SerializeReference]
        public List<UnitConcurrentBehavior> Behaviors = new List<UnitConcurrentBehavior>();
        public UnitStates AutomaticState;

        private IEnumerable<Type> GetBehaviors()
        {
            return UnitConcurrentBehavior.GetConcurrentBehaviors();
        }
    }

    public class DivergingBehaviors : UnitConcurrentBehavior
    {
        [SerializeField]
        public List<BehaviorToState> BehaviorsToState = new List<BehaviorToState>();
        private event Func<bool> conditions;
        private UnitStates NextState = UnitStates.None;
        public override void Behavior()
        {
            foreach (var bs in BehaviorsToState)
            {
                foreach (var b in bs.Behaviors)
                {
                    b.InvokeBehavior();
                }
            }
        }

        public bool EvaluateConditions()
        {
            bool eval = true;
            foreach (var behavior in BehaviorsToState)
            {
                foreach (var bs in BehaviorsToState)
                {
                    foreach (var b in bs.Behaviors)
                    {
                        if (b.ExecCondition())
                        {
                            continue;
                        }
                        eval = false;
                        break;
                    }
                    if (eval == false)
                    {
                        if (bs.AutomaticState != UnitStates.None)
                        {
                            parentState.AutomaticNextState = bs.AutomaticState.ToString();
                        }
                        
                        break;
                    }
                }
            }
            return eval;
        }

        public override void InitBehavior()
        {
            foreach (var bs in BehaviorsToState)
            {
                foreach (var b in bs.Behaviors)
                {
                    b.Init(UnitObject, parentState);
                }
            }
        }

        public override bool ExecCondition()
        {
            return EvaluateConditions();
        }
    }

    

    

    public class MoveToBasePosition : UnitConcurrentBehavior
    {
        UnitMovementController UnitMovementController;
        private Transform unitTransform;
        public override void Behavior()
        {
            UnitMovementController.MoveTowardsTargetAsync(UnitData.DynamicData.BasePosition);
        }

        public override void InitBehavior()
        {
            unitTransform = UnitObject.transform;
            UnitMovementController = UnitObject.UnitMovementController;
        }

        public override bool ExecCondition()
        {
            bool b = ((Vector2) unitTransform.position != UnitData.DynamicData.BasePosition);
            return b;
        }
    }

    public class StopUnitFreeMovement : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            UnitObject.UnitMovementController.FreeMovementInprogress = false;
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }
    
    public class StopUnitPathMovement : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            UnitObject.UnitMovementController.PathMovementInProgress = false;
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }

    public class ChangeState : UnitSingleBehvaior
    {
        public UnitStates State;
        public override void Behavior()
        {
            UnitStates nextState = State;
            UnitObject.StateMachine.SetState(nextState.ToString());
            Debug.LogWarning("Set state to " + nextState + " on Unit : " + UnitObject.name );
            UnitObject.EffectableUnit.IsTargetable();
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return false;
        }
    }

    public class DoNothing : UnitConcurrentBehavior
    {
        public override void Behavior()
        {
            
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }

    public class MoveTowardsTarget : UnitConcurrentBehavior
    {
        public float MovementSpeed = 1;
        [Required]
        
        private UnitMovementController UnitMovementController;

        private int targetGameObjectID
        {
            get => UnitBattleManager.TargetUnit?.MyGameObjectID ?? 0;
        }
        public override void Behavior()
        {
            if (UnitBattleManager.targetExists) {
                if (UnitBattleManager.TargetUnit.EffectableUnit) {
            UnitMovementController.MoveTowardsTargetAsync(UnitBattleManager.TargetUnit.transform.position);
                }
                else
                {
                    foreach (var target in TargetBank.Targets)
                    {
                        if (target.Value.Item1.IsTargetable())
                        {
                            EffectableUnit efu = target.Value.Item1 as EffectableUnit;
                            UnitBattleManager.TargetUnit = efu.GenericUnitController;
                        }
                    }
                }
            }
        }

        public override void InitBehavior()
        {
            UnitMovementController = UnitObject.UnitMovementController;
        }

        public override bool ExecCondition()
        {
            if (TargetBank.HasTargetableTargets)
            {
                if (UnitBattleManager.TargetUnit == null)
                {
                    UnitBattleManager.TargetUnit = GameObjectPool.Instance.ActiveUnits[TargetBank.Targets.First().Key];
                }
                if (AttackAreaTargetBank.HasTargetableTargets)
                {
                    return false;
                }
                return true;
            }

            return false;
        }
    }
    public enum TargetPriority
    {
        ClosestToPathEnd,
        HighestHP,
        LowestHP,
        HighestDamage,
        LowestDamage,
        Random
    }

    public class GetTarget : UnitSingleBehvaior
    {
        public TargetPriority TargetPriority;

        void GetTargetAccordingToPriority()
        {
            if (TargetPriority == TargetPriority.ClosestToPathEnd)
            {
                float proximity = 999f;
                GenericUnitController t_target = null;
                foreach (var ef in TargetBank.Targets)
                {
                    if (GameObjectPool.Instance.Targetables.Contains(ef.Key))
                    {
                        if (GameObjectPool.Instance.ActiveUnits[ef.Key].PathWalker.SplineAttached) {
                            float p = GameObjectPool.Instance.ActiveUnits[ef.Key].PathWalker.ProximityToPathEnd;
                            if (p < proximity)
                            {
                                t_target = GameObjectPool.Instance.ActiveUnits[ef.Key];
                                proximity = p;
                            }
                        }
                    }
                }
                if (!t_target)
                {
                    if (!TargetBank.Targets.IsNullOrEmpty()) {
                        if (GameObjectPool.Instance.ActiveUnits.ContainsKey(TargetBank.Targets.Keys.First()))
                        t_target = GameObjectPool.Instance.ActiveUnits[TargetBank.Targets.Keys.First()];
                    }
                }

                UnitBattleManager.TargetUnit = t_target;
            }
        }
        
        public override void Behavior()
        {
            GetTargetAccordingToPriority();
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return UnitObject.EffectableTargetBank.HasTargetableTargets;
        }
    }

    public class Fight : UnitConcurrentBehavior
    {
        public override void Behavior()
        {
            
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            
            if (TargetBank.HasTargetableTargets)
            {
                if (AttackAreaTargetBank.HasTargetableTargets)
                    /*if (AttackAreaTargetBank.Targets.ContainsKey(UnitObject.UnitBattleManager
                        .TargetUnit.MyGameObjectID))*/
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class StartFight : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            UnitBattleManager.OnFightStart();
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }

    public class EndFight : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            UnitBattleManager.OnFightEnd();
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }
    
    
    
    

    public class FadeOut : UnitConcurrentBehavior
    {
        private SpriteRenderer UnitSpriteRenderer;
        public float FadeOutSpeed = 0.1f;
        public override void Behavior()
        {
            Color currentColor = UnitSpriteRenderer.color;
            UnitSpriteRenderer.color = new Color(currentColor.r,currentColor.g,currentColor.b, currentColor.a - (FadeOutSpeed * StaticObjects.DeltaGameTime));
        }

        public override void InitBehavior()
        {
            UnitSpriteRenderer = UnitObject.SpriteRenderer;
        }

        public override bool ExecCondition()
        {
            return (UnitSpriteRenderer.color.a > 0f);
        }
    }

    public class DisableObject : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            UnitObject.gameObject.SetActive(false);
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }

    public class FindClosestPointOnPath : UnitSingleBehvaior
    {
        public override void Behavior()
        {
            
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return true;
        }
    }


}


