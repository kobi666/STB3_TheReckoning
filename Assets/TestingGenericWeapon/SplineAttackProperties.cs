using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.Threading.Tasks;

public abstract class SplineAttackProperties : AttackProperties
{
    public Effectable MainTarget;
    public Vector2 TargetPosition;
    [OdinSerialize] [GUIColor(1, 0.6f, 0.4f)] public List<SplineBehavior> Splines {get; set;} 
    public float AttackDuration = 2;
    private float AttackProgressCounter = 0f;

    public void InitializeAttackProperties(GenericWeaponController parentWeapon)
    {
        ParentWeapon = parentWeapon;
        InitlizeProperties();
    }
    public void InitlizeProperties()
    {
        foreach (var spline in Splines)
        {
            if (spline != null)
            {
                spline.Init();
            }
        }
    }
    
    [ShowInInspector]
    private bool AsyncAttackInProgress = false;

    public abstract void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition);

    public void StopAttack()
    {
        AsyncAttackInProgress = false;
    }
    public async void StartAsyncSplineAttack(Effectable targetEffectable, Vector2 targetPosition)
    {
        MainTarget = targetEffectable;
        TargetPosition = targetPosition;
        AttackProgressCounter = 0;
        if (AsyncAttackInProgress == false)
        {
            AsyncAttackInProgress = true;
            
            foreach (var spline in Splines)
            {
                spline.SplineMovement.initMovment(targetPosition);
                spline.OnAttackStart();
            }
            while (AttackProgressCounter < AttackDuration && AsyncAttackInProgress == true && MainTarget != null)
            {
                AttackProgressCounter += StaticObjects.Instance.DeltaGameTime;
                SplineAttackFunction(MainTarget, TargetPosition);
                await Task.Yield();
            }

            AsyncAttackInProgress = false;
            foreach (var sp in Splines)
            {
                sp.OnAttackEnd();
            }

            MainTarget = null;
        }
    }

}


public class OneSplineFromOriginToTarget : SplineAttackProperties
{
    public override void SplineAttackFunction(Effectable targetEffectable, Vector2 TargetPosition)
    {
        Splines[0].ConcurrentSplineBehavior(targetEffectable, TargetPosition);
    }
}

