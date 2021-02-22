using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BansheeGz.BGSpline.Components;
using Sirenix.OdinInspector;
using UnityEngine;

namespace UnitBehaviors
{
    public class WalkAlongSpline : UnitBehavior
    {
        public float Distance = 0;
        public float MovementSpeed = 1;
        [Required]
        private PathWalker PathWalker;
        
        public override void Behavior()
        {
            PathWalker.OnPathMovement(StaticObjects.DeltaGameTime * MovementSpeed);
        }

        public override void InitBehavior()
        {
            PathWalker = UnitObject.PathWalker;
        }

        public override bool ExecCondition()
        {
            return (PathWalker?.EndOfPathReached ?? false) ;
        }
    }

    public class WaitForEnemiesToEnterRange : UnitBehavior
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

    public class ChangeState : UnitBehavior
    {
        public UnitStates State;
        public override void Behavior()
        {
            UnitObject.StateMachine.SetState(State.ToString());
            Debug.LogWarning("Set state to " + State + " on Unit : " + UnitObject.name);
        }

        public override void InitBehavior()
        {
            
        }

        public override bool ExecCondition()
        {
            return false;
        }
    }

    public class DoNothing : UnitBehavior
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

    public class MoveTowardsTarget : UnitBehavior
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

    public class GetTarget : UnitBehavior
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

    public class Fight : UnitBehavior
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

    public class StartFight : UnitBehavior
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

    public class EndFight : UnitBehavior
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


