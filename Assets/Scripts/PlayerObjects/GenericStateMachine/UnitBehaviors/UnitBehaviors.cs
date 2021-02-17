using System.Collections;
using System.Collections.Generic;
using BansheeGz.BGSpline.Components;
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
}


