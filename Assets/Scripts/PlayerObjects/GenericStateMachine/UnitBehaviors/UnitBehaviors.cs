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
        public BGCcMath Spline;
        public float Distance = 0;
        public float MovementSpeed = 1;
        public override void Behavior()
        {
            Distance += StaticObjects.Instance.DeltaGameTime * MovementSpeed;
            UnitObject.transform.position = Spline.CalcPositionByDistance(Distance);
        }

        public override void InitBehavior()
        {
            Spline = UnitData.DynamicData.Spline;
        }

        public override bool ExecCondition()
        {
            return (Spline && Distance < 1) ;
        }
    }

    public class WaitForEnemiesToEnterRange : UnitBehavior
    {
        public TagDetector Detector;
        private EffectableTargetBank TargetBank;
        private bool TargetableInRange;
        public override void Behavior()
        {
            
        }

        public override void InitBehavior()
        {
            TargetBank = UnitObject.EffectableTargetBank;
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
        public UnitBattleManager UnitBattleManager;
        [Required]
        public GenericWeaponController GenericWeaponController;
        public float MovementSpeed = 1;
        private EffectableTargetBank AttackAreaTargetbank;

        private string targetname
        {
            get => UnitBattleManager.TargetUnit.name;
        }
        public override void Behavior()
        {
            UnitObject.transform.position = Vector2.MoveTowards(UnitObject.transform.position,
                UnitBattleManager.TargetUnit.transform.position, MovementSpeed);
        }

        public override void InitBehavior()
        {
            UnitBattleManager = UnitObject.UnitBattleManager;
            AttackAreaTargetbank = GenericWeaponController.TargetBank;
        }

        public override bool ExecCondition()
        {
            if (UnitBattleManager.targetExists) {
                if (GameObjectPool.Instance.Targetables.Contains(targetname))
                {
                    if (!AttackAreaTargetbank.Targets.ContainsKey(targetname))
                    {
                        return true;
                    }
                }
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
        private UnitBattleManager UnitBattleManager;

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
            
        }

        public override void InitBehavior()
        {
            UnitBattleManager = UnitObject.UnitBattleManager;
        }

        public override bool ExecCondition()
        {
            return UnitObject.EffectableTargetBank.HasTargetableTargets;
        }
    }
}


