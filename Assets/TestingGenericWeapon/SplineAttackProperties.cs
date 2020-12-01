using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.Threading.Tasks;

public abstract class SplineAttackProperties
{
    public List<SplineBehavior> Splines {get; set;}
    public float AttackDuration { get; set; } = 2;
    private float AttackProgressCounter = 0f;
    
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

    public async void StartAsyncSplineAttack(Effectable targetEffectable, Vector2 TargetPosition)
    {
        if (AsyncAttackInProgress == false)
        {
            AsyncAttackInProgress = true;
            while (AttackProgressCounter < AttackDuration && AsyncAttackInProgress == true)
            {
                AttackProgressCounter += StaticObjects.Instance.DeltaGameTime;
                SplineAttackFunction(targetEffectable, TargetPosition);
                await Task.Yield();
            }

            AsyncAttackInProgress = false;
        }
    }

}
