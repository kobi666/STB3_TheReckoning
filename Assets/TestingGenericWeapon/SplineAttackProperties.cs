using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

public abstract class SplineAttackProperties
{
    public abstract List<SplineBehavior> Splines {get; set;}


    public abstract void InitlizeAttack();

}
