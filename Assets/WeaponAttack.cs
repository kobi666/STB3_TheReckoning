using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponAttack
{
    
    public abstract void Attack(Effectable singleTarget, Vector2 SingleTargetPosition);
    
    public abstract void InitlizeAttack();
}
