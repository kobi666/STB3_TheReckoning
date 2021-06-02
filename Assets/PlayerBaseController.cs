using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerBaseController : MyGameObject
{
    
    
    [Required]
    public CollisionDetector CollisionDetector;

    public event Action<int> onEnemyEnter;
    public void OnEnemyEnter (int objectGID)
    {
        onEnemyEnter?.Invoke(objectGID);
    }

    protected void Awake()
    {
        CollisionDetector.onNewTargetEnter += OnEnemyEnter;
        onEnemyEnter += KillEnemyEntered;
    }

    void KillEnemyEntered(int EnemyCollisionID)
    {
        int GID = GameObjectPool.CollisionIDToGameObjectID[EnemyCollisionID].Item1;
        var enemy = GameObjectPool.Instance.ActiveUnits[GID];
        int DamageToBase = enemy.DamageToBase;
        GameManager.Instance.UpdateLife(-DamageToBase);
        enemy.EffectableUnit.ApplyDamage(99999);
    }
    
    
}
