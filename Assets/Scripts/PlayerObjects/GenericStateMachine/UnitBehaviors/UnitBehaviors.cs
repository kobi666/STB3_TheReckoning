using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BansheeGz.BGSpline.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnitBehaviors
{
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
            return (PathWalker?.EndOfPathReached ?? false) ;
        }
    }

    

    public class WaitForEnemiesToEnterRange : UnitConcurrentBehavior
    {
        public TagDetector Detector;
        
        private bool TargetableInRange;
        public override void Behavior()
        {
            
        }

        public override void InitBehavior()
        {
            Detector = Detector ?? UnitObject.RangeDetector;
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
                            parentState.AutomaticNextState = bs.AutomaticState;
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
            UnitMovementController.MoveTowardsTarget(UnitData.DynamicData.BasePosition);
        }

        public override void InitBehavior()
        {
            unitTransform = UnitObject.transform;
            UnitMovementController = UnitObject.UnitMovementController;
        }

        public override bool ExecCondition()
        {
            return ((Vector2)unitTransform.position != UnitData.DynamicData.BasePosition);
        }
    }

    public class ChangeState : UnitSingleBehvaior
    {
        public UnitStates State;
        public override void Behavior()
        {
            UnitStates nextState = State;

            bool Automatic()
            {
                return (parentState.AutomaticNextState != UnitStates.None);
            }
            if (Automatic())
            {
                State = parentState.AutomaticNextState;
            }
            UnitObject.StateMachine.SetState(nextState.ToString());
            Debug.LogWarning("Set state to " + nextState + " on Unit : " + UnitObject.name + " automatic : " + Automatic());
            UnitObject.EffectableUnit.IsTargetable();
            if (Automatic())
            {
                parentState.AutomaticNextState = UnitStates.None;
            }
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
        public EffectableTargetBank AttackAreaTargetbank;
        private UnitMovementController UnitMovementController;

        private string targetname
        {
            get => UnitBattleManager.TargetUnit.name;
        }
        public override void Behavior()
        {
            UnitMovementController.MoveTowardsTarget(UnitBattleManager.TargetUnit.transform.position);
        }

        public override void InitBehavior()
        {
            UnitMovementController = UnitObject.UnitMovementController;
        }

        public override bool ExecCondition()
        {
            if (UnitBattleManager.targetExists)
            {
                if (AttackAreaTargetbank.Targets.ContainsKey(targetname))
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
                    t_target = GameObjectPool.Instance.ActiveUnits[TargetBank.Targets.Keys.First()];
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
            if (UnitBattleManager.targetExists)
            {
                if (AttackAreaTargetBank.Targets.ContainsKey(UnitObject.UnitBattleManager
                    .TargetUnit.name))
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
    
    
}


