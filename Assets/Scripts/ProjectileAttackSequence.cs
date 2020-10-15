using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class ProjectileAttackSequence
{
    private SerializedMonoBehaviour exec = null;
    public IEnumerator attackSequence(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
        Vector2 originPosition,
        Vector2 targetPosition, Quaternion direction, float assistingfloat1, float assistingFloat2,
        int numOfProjectiles)
            {
                yield break;
            }

    public int BaseNumOfProjectiles;

    public virtual int NumOfProjectiles()
    {
        return BaseNumOfProjectiles;
    }

    public float assistingFloat1;
    public float assistingFloat2;

    public virtual float AssistingFloat1
    {
        get => assistingFloat1;
        set => assistingFloat1 = value;
    }

    public virtual float AssistingFloat2
    {
        get => assistingFloat1;
        set => assistingFloat1 = value;
    }

    public void StartAttackSequence(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
        Vector2 originPosition,
        Vector2 targetPosition, Quaternion direction, float assistingfloat1, float assistingFloat2,
        int numOfProjectiles)
    {
        exec.StartCoroutine(attackSequence(pool, targetEffectable, originPosition, targetPosition, direction,
            assistingfloat1, assistingFloat2, NumOfProjectiles()));
    }
}
